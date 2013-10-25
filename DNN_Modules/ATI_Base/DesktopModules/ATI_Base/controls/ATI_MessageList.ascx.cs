using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Telerik.Web.UI;

using Affine.Data;
using Affine.Data.EventArgs;


public partial class DesktopModules_ATI_Base_controls_ATI_MessageList : DotNetNuke.Framework.UserControlBase
{   
    public class MessageListData
    {
        public long Id { get; set; }
        public long UserKey { get; set; }
        public string UserName { get; set; }
        public long PortalKey { get; set; }
        public DateTime Date { get; set; }
        public string DaysAgo { get; set; }
        public int Status { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }       
    }    

    public bool AllowPaging
    {
        get
        {
            if (ViewState["AllowPaging"] != null)
            {
                return (bool)ViewState["AllowPaging"];
            }
            return true;
        }
        set
        {
            ViewState["AllowPaging"] = value;
        }
    }

    public int PageSize
    {
        get
        {
            if (ViewState["PageSize"] != null)
            {
                return (int)ViewState["PageSize"];
            }
            return 15;  // defualt 15
        }
        set
        {
            ViewState["PageSize"] = value;
        }
    }

    public IList<Message> DataSource
    {
        get
        {
            if (ViewState["DataSource"] == null)
            {
                return null;
            }
            return (IList<Message>)ViewState["DataSource"];
        }
        set
        {
            ViewState["DataSource"] = value;
        }
    }

    public bool IsMessageHistory
    {
        get
        {
            if (ViewState["IsMessageHistory"] == null)
            {
                return false;
            }
            return (bool)ViewState["IsMessageHistory"];
        }
        set
        {
            ViewState["IsMessageHistory"] = value;
        }
    }

    public override void  DataBind()
    {
 	    base.DataBind();
        this.EnsureChildControls();
        if (this.DataSource != null)
        {
            IList<MessageListData> source = new List<MessageListData>();            
            foreach (Message m in this.DataSource)
            {
                
                TimeSpan dayAgo = DateTime.Now.ToUniversalTime().Subtract(m.DateTime);
                MessageListData mListData = new MessageListData()
                {
                    Id = m.Id,
                    PortalKey = m.PortalKey,
                    UserName = m.UserSetting.UserName,                       
                    Date = m.DateTime,
                    UserKey = m.UserSetting.UserKey,
                    Subject = m.Subject,
                    Text = this.IsMessageHistory ? m.Text : (m.LastText.Length < 128 ? m.LastText : m.LastText.Substring(0, 128) + "..."),
                    Status = m.Status,
                    DaysAgo = dayAgo.Days > 0 ? dayAgo.Days + " days ago" : (dayAgo.Minutes > 0) ? dayAgo.Minutes + " minutes ago" : dayAgo.Seconds + " seconds ago",
                };
                source.Add(mListData);                
            }
            atiDataListStream.DataSource = source;
            atiDataListStream.DataBind();
        }
    }
   
    // Custom Event Deligates
    public delegate void DeleteEvent(object sender, StreamDataEventArgs e);
    public event DeleteEvent Stream_DeleteEvent;   
    // END - Custom Event Deligates

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {
            
            atiDataListStream.ShowFooter = this.AllowPaging;
                        
            // TODO: make a view button (me & friends) (just me) that sets the stream view

            // TODO: make the "take" configurable
            
        }       
    }      
    

    protected void atiDataListStream_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        long id = (long)atiDataListStream.DataKeys[e.Item.ItemIndex];
        if (id > 0 && this.DataSource != null)
        {
            Message message = this.DataSource.First(m => m.Id == id);
            if (this.Stream_DeleteEvent != null)
            {
                // TODO: send a delete event
              //  this.Stream_DeleteEvent(this, new StreamDataEventArgs() { UserStream = ustream });
            }
            this.DataSource.Remove(message);          
        }
        // TODO: log item deleted event.
        atiDataListStream.EditItemIndex = -1;
        this.DataBind();       
    }

    protected void atiDataListStream_ItemCreated(object sender, DataListItemEventArgs e)
    {
       
    }           
}
