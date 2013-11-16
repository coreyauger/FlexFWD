using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Linq;
using System.Xml.Linq;
using System.Net;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

using Affine.Data;

using Telerik.Web.UI;

namespace Affine.Dnn.Modules.ATI_Friends
{   
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewATI_Builder class displays the content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    partial class ViewATI_Friends : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
    {

        #region Private Members              
             
       
        #endregion       

        #region Public Methods    
        

        #endregion

        #region Event Handlers

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            base.Page_Load(sender, e);            
            try
            {
                ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                service.InlineScript = true;
                ScriptManager.GetCurrent(Page).Services.Add(service);

                if (ProfileSettings is Group)
                {
                    litFriendTerm.Text = "Members";
                    litGroupName.Text = "<a class=\"username\" href=\"/groups/" + ProfileSettings.UserName + "\">" + ProfileSettings.UserFirstName + "</a>";
                    atiProfileImage.IsOwner = false;
                    atiWebLinksList.ProfileSettings = ProfileSettings;
                    panelGroupInfo.Visible = true;
                }
                atiProfileImage.Settings = base.ProfileSettings != null ? base.ProfileSettings : base.UserSettings;
                if (Settings["ProfileMode"] != null && Convert.ToString(Settings["ProfileMode"]).Equals("None"))
                {
                    divLeftNav.Visible = false;
                    divAd.Visible = false;
                    divCenterWrapper.Style["margin-right"] = "0px";
                    divControl.Style["float"] = "right";
                }               
                if (Permissions == AqufitPermission.PUBLIC && Settings["ModuleMode"] == null )
                {
                    Response.Write("<h1>TODO: error page => You can not view this persons friends.. you are not friends with them</h1>");
                }
                else
                {
                    if (Permissions == AqufitPermission.PUBLIC && Settings["ModuleMode"] == "Friend")
                    {
                        Response.Write("<h1>TODO: error page => You can not view this persons friends.. you are not friends with them</h1>");
                    }
                    if (!Page.IsPostBack && !Page.IsCallback)
                    {                                                
                        if (Settings["ModuleMode"] != null && Convert.ToString(Settings["ModuleMode"]).Equals("Follow"))
                        {
                            tabFriends.Visible = pageViewFriends.Visible = false;
                            SetupPageFollowMode();
                        }
                        else if (Settings["ModuleMode"] != null && Convert.ToString(Settings["ModuleMode"]).Equals("FriendFollow"))
                        {
                            SetupPageFollowMode();
                            SetupPageFriendMode(); 
                        }
                        else
                        {
                            tabFollowing.Visible = pageViewFollowing.Visible = false;
                            tabFollowers.Visible = pageViewFollowers.Visible = false;
                            SetupPageFriendMode();       
                        }       
                    }
                }
                imgAdRight.Src = ResolveUrl("/portals/0/images/adTastyPaleo.jpg");
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }


        private void SetupPageFollowMode()
        {
            WebService.StreamService streamService = new WebService.StreamService();
            if (Request["c"] != null)
            {
                streamService.FollowUser(this.UserSettings.Id, Convert.ToInt64(Request["c"]));
            }
            UserSettings settings = ProfileSettings != null ? ProfileSettings : UserSettings;
            if (!(settings is Group))
            {
                string json = streamService.getFriendListData(settings.UserKey, this.PortalId, Affine.Utils.ConstsUtil.FriendListModes.FOLLOWING, 0, 25);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "FollowingList", "$(function(){ Aqufit.Page.atiFollowingList.generateStreamDom('" + json + "'); });", true);

                json = streamService.getFriendListData(settings.UserKey, this.PortalId, Affine.Utils.ConstsUtil.FriendListModes.FOLLOWERS, 0, 25);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "FollowerList", "$(function(){ Aqufit.Page.atiFollowerList.generateStreamDom('" + json + "'); });", true);
            }
            
        }

        private void SetupPageFriendMode()
        {
            atiFriendListScript.ControlMode = DesktopModules_ATI_Base_controls_ATI_FriendListScript.Mode.FRIEND_LIST;
            WebService.StreamService streamService = new WebService.StreamService();
            UserSettings settings = ProfileSettings != null ? ProfileSettings : UserSettings;
            if (settings is Group)
            {
                if (this.UserSettings.Id != settings.Id)
                {
                    atiFriendListScript.ControlMode = DesktopModules_ATI_Base_controls_ATI_FriendListScript.Mode.FRIEND_REQUEST;
                }
                tabFollowing.Visible = pageViewFollowing.Visible = false;
                tabFollowers.Visible = pageViewFollowers.Visible = false;
                string json = streamService.getMemberListData(settings.Id, 0, 25, this.UserSettings.Id);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "FriendList", "$(function(){ Aqufit.Page.atiFriendListScript.generateStreamDom('" + json + "'); });", true);
            }
            else
            {
                if (this.UserSettings.Id != settings.Id)
                {
                    atiFriendListScript.ControlMode = DesktopModules_ATI_Base_controls_ATI_FriendListScript.Mode.FRIEND_REQUEST;
                }
                string json = streamService.getFriendListData(settings.UserKey, this.PortalId, Affine.Utils.ConstsUtil.FriendListModes.FRIEND, 0, 25, this.UserSettings.Id);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "FriendList", "$(function(){ Aqufit.Page.atiFriendListScript.generateStreamDom('" + json + "'); });", true);                
            }
        }   

        protected void atiRadComboBoxSearchFriends_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboSearchFriends = (RadComboBox)sender;
            atiRadComboSearchFriends.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            long[] friendIds = null;
            if (ProfileSettings is Group)
            {
                friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id || f.DestUserSettingKey == this.ProfileSettings.Id) ).Select(f => f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            }
            else
            {
                friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id || f.DestUserSettingKey == this.ProfileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND).Select(f => f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            }
            int itemOffset = e.NumberOfItems;
            IQueryable<User> friends = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<User, long>(w => w.Id, friendIds)).OrderBy( w => w.UserName );
            int length = friends.Count();
            friends = string.IsNullOrEmpty(e.Text) ? friends.Skip(itemOffset).Take(TAKE) : friends.Where(w => w.UserName.ToLower().StartsWith(e.Text) || w.UserFirstName.ToLower().StartsWith(e.Text) || w.UserLastName.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            User[] users = friends.ToArray();

            foreach (User u in users)
            {
                RadComboBoxItem item = new RadComboBoxItem(u.UserName + " (" + u.UserFirstName + " " + u.UserLastName + ")");
                item.Value = "" + u.UserName;
                item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + u.UserKey + "&p=" + u.PortalKey;
                atiRadComboSearchFriends.Items.Add(item);
            }            
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }


        protected void atiRadComboBoxSearchFollowing_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboSearchFriends = (RadComboBox)sender;
            atiRadComboSearchFriends.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            int itemOffset = e.NumberOfItems;
            IQueryable<User> friends = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<User, long>(w => w.Id, friendIds)).OrderBy(w => w.UserName);
            int length = friends.Count();
            friends = string.IsNullOrEmpty(e.Text) ? friends.Skip(itemOffset).Take(TAKE) : friends.Where(w => w.UserName.ToLower().StartsWith(e.Text) || w.UserFirstName.ToLower().StartsWith(e.Text) || w.UserLastName.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            User[] users = friends.ToArray();

            foreach (User u in users)
            {
                RadComboBoxItem item = new RadComboBoxItem(u.UserFirstName + " (" + u.UserFirstName + " " + u.UserLastName + ")");
                item.Value = "" + u.UserName;
                item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + u.UserKey + "&p=" + u.PortalKey;
                atiRadComboSearchFriends.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }


        protected void atiRadComboBoxSearchFollower_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            /*
            RadComboBox atiRadComboSearchFriends = (RadComboBox)sender;
            atiRadComboSearchFriends.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            int itemOffset = e.NumberOfItems;
            IQueryable<User> friends = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<User, long>(w => w.Id, friendIds)).OrderBy(w => w.UserName);
            int length = friends.Count();
            friends = string.IsNullOrEmpty(e.Text) ? friends.Skip(itemOffset).Take(TAKE) : friends.Where(w => w.UserName.ToLower().StartsWith(e.Text) || w.UserFirstName.ToLower().StartsWith(e.Text) || w.UserLastName.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            User[] users = friends.ToArray();

            foreach (User u in users)
            {
                RadComboBoxItem item = new RadComboBoxItem(u.UserFirstName + " (" + u.UserFirstName + " " + u.UserLastName + ")");
                item.Value = "" + u.UserName;
                item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + u.UserKey + "&p=" + u.PortalKey;
                atiRadComboSearchFriends.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
             */
        }



        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;
                Affine.WebService.StreamService streamService = new WebService.StreamService();
                int skip = 0;
                int take = 0;
                string json = string.Empty;
                switch (hiddenAjaxAction.Value)
                {
                    case "friendRequest":
                        long fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataManager.sendFriendRequest(UserSettings.Id, fid);
                        status = "Friend request has been sent.";
                        break;
                    case "friendListDataPage":
                        skip = Convert.ToInt32(hiddenAjaxValue.Value);
                        take = Convert.ToInt32(hiddenAjaxValue2.Value);
                        if (ProfileSettings is Group)
                        {
                            json = streamService.getMemberListData(ProfileSettings.Id, skip, take, this.UserSettings.Id);
                        }
                        else
                        {
                            json = streamService.getFriendListData(ProfileSettings.UserKey, ProfileSettings.PortalKey, Affine.Utils.ConstsUtil.FriendListModes.FRIEND, skip, take, this.UserSettings.Id);
                        }
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiFriendListScript.generateStreamDom('" + json + "'); ");
                        break;
                    case "followingListDataPage":
                        skip = Convert.ToInt32(hiddenAjaxValue.Value);
                        take = Convert.ToInt32(hiddenAjaxValue2.Value);
                        json = streamService.getFriendListData(ProfileSettings.UserKey, ProfileSettings.PortalKey, Affine.Utils.ConstsUtil.FriendListModes.FOLLOWING, skip, take);
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiFollowingList.generateStreamDom('" + json + "'); ");
                        break;
                    case "followerListDataPage":
                        skip = Convert.ToInt32(hiddenAjaxValue.Value);
                        take = Convert.ToInt32(hiddenAjaxValue2.Value);
                        json = streamService.getFriendListData(ProfileSettings.UserKey, ProfileSettings.PortalKey, Affine.Utils.ConstsUtil.FriendListModes.FOLLOWERS, skip, take);
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiFollowerList.generateStreamDom('" + json + "'); ");
                        break;

                }
            }
            catch (Exception ex)
            {
                status = "ERROR: There was a problem with the action (" + ex.Message + ")";
            }
            RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
        }        

        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Registers the module actions required for interfacing with the portal framework
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), ModuleActionType.AddContent, "", "", this.EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }
}

