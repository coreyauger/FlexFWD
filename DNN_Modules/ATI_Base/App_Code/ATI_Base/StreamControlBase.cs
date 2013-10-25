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

namespace Affine.Dnn.Modules.ATI_Base
{
    public class StreamControlBase : DotNetNuke.Framework.UserControlBase
    {                
        public DataList atiDataListStream;

        public bool ShowCommentAdd
        {
            get
            {
                if (ViewState["ShowCommentAdd"] != null)
                {
                    return (bool)ViewState["ShowCommentAdd"];
                }
                return true;
            }
            set
            {
                ViewState["ShowCommentAdd"] = value;
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

        public IList<UserStream> DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    return null;
                }
                return (IList<UserStream>)ViewState["DataSource"];
            }
            set
            {
                ViewState["DataSource"] = value;
            }
        }

        public Affine.Utils.UnitsUtil.MeasureUnit UserDistanceUnits
        {
            get
            {
                if (ViewState["UserDistanceUnits"] != null)
                {
                    return (Affine.Utils.UnitsUtil.MeasureUnit)ViewState["UserDistanceUnits"];
                }
                return Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;   // default distance units if not passed in
            }

            set
            {
                ViewState["UserDistanceUnits"] = value;
            }
        }

        private bool UseThirdPartyId
        {
            get
            {
                if (ViewState["UseThirdPartyId"] != null)
                {
                    return (bool)ViewState["UseThirdPartyId"];
                }
                return false;
            }

            set
            {
                ViewState["UseThirdPartyId"] = value;
            }
        }

        public override void DataBind()
        {
            base.DataBind();
            this.EnsureChildControls();
            if (this.DataSource != null)
            {
                string baseUrl = ResolveUrl("~/");
                IList<Affine.Data.json.StreamData> streamData = new List<Affine.Data.json.StreamData>();

                // TODO: tmp using this users image for all profile shots
                aqufitEntities entities = new aqufitEntities();
                foreach (UserStream s in this.DataSource)
                {
                    DateTime date = Convert.ToDateTime(s.Date).ToLocalTime();
                    DateTime postDate = s.TimeStamp.ToLocalTime();
                    TimeSpan dayAgo = DateTime.Now.ToUniversalTime().Subtract(s.TimeStamp);
                    if (s is Workout)
                    {
                        Workout w = (Workout)s;
                        long id = 0;
                        if (w.Id > 0)
                        {
                            id = w.Id;
                        }
                        else
                        {
                            id = Convert.ToInt64(w.ThirdPartyId);
                            this.UseThirdPartyId = true;
                        }
                        string title = w.Title;
                        //if (string.IsNullOrEmpty(title))
                        //{
                            // TODO: need to replace "Ran"
                        //    title = unames.FirstName + " Ran " + String.Format("{0:0.0}", distance) + " " + Affine.Utils.UnitsUtil.unitToStringName(this.UserDistanceUnits) + " in " + String.Format("{0:0}", duration.TotalMinutes) + " minutes.";
                        //}

                        // TODO: don't make a ton of "copys" of the profile image for each stream data.
                        streamData.Add(new Affine.Data.json.StreamData()
                        {
                            Id = id,
                            UserKey = w.UserSetting.UserKey,
                            UserName = w.UserSetting.UserName,
                            //UserShortName = unames.CombinedName,
                            PortalKey = w.PortalKey,
                            DateTicks = w.Date.ToShortDateString() + " " + w.Date.ToLongTimeString(),
                            Title = "<a href=\"" + baseUrl + w.UserSetting.UserName + "/workout/" + id + "\">" + title + "</a>",     // TODO: we will need a proper view button
                            Description = w.Description,
                            Distance = Convert.ToDouble(w.Distance),
                            Duration = Convert.ToInt64(w.Duration),
                            Calories = Convert.ToDouble(w.Calories),
                            Emotion = Convert.ToInt16(w.Emotion),
                            Weather = Convert.ToInt16(w.Weather),
                            Terrain = Convert.ToInt16(w.Terrain),
                            WorkoutType = Convert.ToInt64(w.WorkoutTypeKey),
                        });
                    }
                    else if (s is Shout)
                    {
                        Shout w = (Shout)s;
                        // Convert into friendly stream formats
                        // TODO: this should be UserSettings and from the global cache
                        Affine.Data.UserNames unames = Affine.Utils.CacheUtil.GetUserNamesData(this.PortalSettings.PortalId, w.UserSetting.UserKey);
                        string title = w.Title;
                        if (string.IsNullOrEmpty(title))
                        {
                            // TODO: need to replace "Ran"
                            title = unames.FirstName + " Shout ";
                        }
                        streamData.Add(new Affine.Data.json.StreamData()
                        {
                            Id = w.Id,
                            UserKey = w.UserSetting.UserKey,
                            UserName = unames.UserName,
                            UserShortName = unames.CombinedName,
                            PortalKey = w.PortalKey,
                            DateTicks = w.Date.ToShortDateString() + " " + w.Date.ToLongTimeString(),
                            Title = title,
                            Description = w.Description,
                            Date = date.ToShortDateString() + " T " + date.ToLongTimeString(),
                        });
                    }
                    else if (s is Notification)
                    {
                        Notification n = (Notification)s;
                        // Convert into friendly stream formats
                        // TODO: this should be UserSettings and from the global cache
                        Affine.Data.UserNames unames = Affine.Utils.CacheUtil.GetUserNamesData(this.PortalSettings.PortalId, n.UserSetting.UserKey);
                        string title = n.Title;
                        if (string.IsNullOrEmpty(title))
                        {
                            // TODO: we should be setting this when 
                            title = "Notification";
                        }
                        streamData.Add(new Affine.Data.json.StreamData()
                        {
                            Id = n.Id,
                            UserKey = n.UserSetting.UserKey,
                            UserName = unames.UserName,
                            UserShortName = unames.CombinedName,
                            PortalKey = n.PortalKey,
                            DateTicks = n.Date.ToShortDateString() + " " + n.Date.ToLongTimeString(),
                            Title = title,
                            Description = n.Description,
                            Date = date.ToShortDateString() + " T " + date.ToLongTimeString(),
                        });
                    }
                }
                atiDataListStream.DataSource = streamData;
                atiDataListStream.DataBind();
            }
        }

        // Custom Event Deligates
        public delegate void DeleteEvent(object sender, StreamDataEventArgs e);
        public event DeleteEvent Stream_DeleteEvent;
        public delegate void UpdateEvent(object sender, StreamDataEventArgs e);
        public event UpdateEvent Stream_UpdateEvent;
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

        protected void atiDataListStream_EditCommand(object source, DataListCommandEventArgs e)
        {
            atiDataListStream.EditItemIndex = e.Item.ItemIndex;
            this.DataBind();
        }

        protected void atiDataListStream_CancelCommand(object source, DataListCommandEventArgs e)
        {
            atiDataListStream.EditItemIndex = -1;
            this.DataBind();

        }

        protected void atiDataListStream_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            long id = (long)atiDataListStream.DataKeys[e.Item.ItemIndex];
            if (id > 0 && this.DataSource != null)
            {
                UserStream ustream = this.UseThirdPartyId ? this.DataSource.Cast<Workout>().First(us => us.ThirdPartyId == id) : this.DataSource.First(us => us.Id == id);
                if (this.Stream_DeleteEvent != null)
                {
                    this.Stream_DeleteEvent(this, new StreamDataEventArgs() { UserStream = ustream });
                }
                this.DataSource.Remove(ustream);
            }
            // TODO: fire item deleted event.
            atiDataListStream.EditItemIndex = -1;
            this.DataBind();
        }

        protected void atiDataListStream_ItemCreated(object sender, DataListItemEventArgs e)
        {

        }

        protected void atiDataListStream_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            long id = (long)atiDataListStream.DataKeys[e.Item.ItemIndex];
            if (id > 0 && this.DataSource != null)
            {
                string categoryName = ((TextBox)e.Item.FindControl("textCategoryName")).Text;
                string description = ((TextBox)e.Item.FindControl("textDescription")).Text;
                UserStream ustream = this.DataSource.First(us => us.Id == id);
                ustream.Title = categoryName;
                // TODO: update
                if (this.Stream_UpdateEvent != null)
                {
                    this.Stream_UpdateEvent(this, new StreamDataEventArgs() { UserStream = ustream });
                }
            }
            atiDataListStream.EditItemIndex = -1;
            this.DataBind();
        }


    }
}