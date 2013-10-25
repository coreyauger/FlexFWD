using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;
using Affine.Data.EventArgs;

public partial class DesktopModules_ATI_Base_controls_ATI_FriendList : DotNetNuke.Framework.UserControlBase
{
    public class Friend
    {
        public long Id { get; set; }
        public long UserKey { get; set; }
        public byte[] Image{ get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }     // stores if the user has a pending friend request 
        // TODO: the score and the awards for that user.
    }


    public long[] PendingRequestStatusList
    {
        private get{
            if (ViewState["PendingRequestStatusList"] != null)
            {
                return (long[])ViewState["PendingRequestStatusList"];
            }
            return null;
        }

        set
        {
            ViewState["PendingRequestStatusList"] = value;
        }
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

    public IList<UserSettings> DataSource
    {
        get
        {
            if (ViewState["DataSource"] == null)
            {
                return null;
            }
            return (IList<UserSettings>)ViewState["DataSource"];
        }
        set
        {
            ViewState["DataSource"] = value;
        }
    }


      

    public override void DataBind()
    {
        base.DataBind();
        this.EnsureChildControls();
        if (this.DataSource != null)
        {
            // TODO: Need the default image for people that dont have it set. (from data cache)
                
            // Need to divide the fiend list into pending requests and non pending
            IList<Friend> friendDataSource = null;
            if (PendingRequestStatusList != null)
            {
                friendDataSource = this.DataSource.Where(f => PendingRequestStatusList.Contains(f.UserKey) ).Select(f => new Friend() { Id = f.Id, UserKey = f.UserKey, Email = this.ShowEmail ? f.UserEmail : "", FirstName = f.UserFirstName, LastName = f.UserLastName, UserName = f.UserName, Image = f.Image != null ? f.Image.Bytes : new byte[0], Status = 1 }).ToList<Friend>();
                friendDataSource = friendDataSource.Concat(this.DataSource.Where(f => !PendingRequestStatusList.Contains(f.UserKey)).Select(f => new Friend() { Id = f.Id, UserKey = f.UserKey, Email = this.ShowEmail ? f.UserEmail : "", FirstName = f.UserFirstName, LastName = f.UserLastName, UserName = f.UserName, Image = f.Image != null ? f.Image.Bytes : new byte[0], Status = 0 })).ToList();
            }
            else
            {
                friendDataSource = this.DataSource.Select(f => new Friend() { Id = f.Id, UserKey = f.UserKey, Email = this.ShowEmail ? f.UserEmail : "", FirstName = f.UserFirstName, LastName = f.UserLastName, UserName = f.UserName, Image = f.Image != null ? f.Image.Bytes : new byte[0], Status = 0 }).ToList<Friend>();
            }
            atiFriendList.DataSource = friendDataSource;
            atiFriendList.DataBind();
        }
    }

    // Custom Event Deligates
    public delegate void DeleteEvent(object sender, FriendDataEventArgs e);
    public event DeleteEvent Friend_DeleteEvent;
    // END - Custom Event Deligates

    public string ClientOnFreindRequest
    {
        get;
        set;
    }

    public string Text
    {
        get;
        set;
    }

    public bool ShowEmail
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {

            atiFriendList.ShowFooter = this.AllowPaging;

            if (!string.IsNullOrEmpty(ClientOnFreindRequest))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "friendRequestScript", "function sendFriendRequest(id) { " + ClientOnFreindRequest + "(id); }", true);
            }

            // get a list of all the users stram entries

            // TODO: make a view button (me & friends) (just me) that sets the stream view

            // TODO: make the "take" configurable

        }
    }

   

    protected void atiFriendList_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        long id = (long)atiFriendList.DataKeys[e.Item.ItemIndex];
        if (id > 0 && this.DataSource != null)
        {
            /*
            UserStream ustream = this.UseThirdPartyId ? this.DataSource.First(us => us.ThirdPartyId == id) : this.DataSource.First(us => us.Id == id);
            if (this.Friend_DeleteEvent != null)
            {
                this.Friend_DeleteEvent(this, new FriendDataEventArgs() { UserStream = ustream });
            }
            this.DataSource.Remove(ustream);
             */
        }
        // TODO: fire item deleted event.
        atiFriendList.EditItemIndex = -1;
        this.DataBind();
    }

    protected void atiFriendList_ItemCreated(object sender, DataListItemEventArgs e)
    {

    }

}
