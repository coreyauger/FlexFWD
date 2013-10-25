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
using Affine.Data.EventArgs;
using Affine.Utils.Linq;


namespace Affine.Dnn.Modules
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
    public class ATI_PermissionPageBase : ATI_PageBase
    {

        #region Private Members

        // simple permissions class
        public enum AqufitPermission { OWNER=0, FRIEND, USER, PUBLIC };

        #endregion

        #region Public Methods

        public Affine.Data.UserSettings ProfileSettings { get; protected set; }
        public Affine.Data.User UserSettings { get; protected set; }
        public Affine.Utils.ConstsUtil.Relationships GroupPermissions { get; protected set; }
        public AqufitPermission Permissions { get; private set; }
        public Affine.Data.Group GroupSettings { get; protected set; }
        public Affine.Utils.UnitsUtil.MeasureUnit DistanceUnits { get; protected set; }
        public Affine.Utils.UnitsUtil.MeasureUnit WeightUnits { get; protected set; }
        public bool Following { get; set; }

        #endregion
       
        protected string GetProfileJs()
        {
            string ret = string.Empty;            
            ret += "Aqufit.Page.Permission = " + Convert.ToInt32( this.Permissions ) + "; ";
            // Distance            
            ret += "Aqufit.Page.DistanceUnits = " + (int)this.DistanceUnits + "; ";
            // Weight            
            ret += "Aqufit.Page.WeightUnits = " + (int)this.WeightUnits + "; ";            
            if (ProfileSettings != null)
            {
                ret += "Aqufit.Page.ProfileId = " + ProfileSettings.UserKey + "; ";
                ret += "Aqufit.Page.UserName = '" + ProfileSettings.UserName + "'; ";
                ret += "Aqufit.Page.UserEmail = '" + ProfileSettings.UserEmail + "'; ";
                ret += "Aqufit.Page.UserFirstName = '" + ProfileSettings.UserFirstName + "'; ";
                ret += "Aqufit.Page.UserLastName = '" + ProfileSettings.UserLastName + "'; ";
                ret += "Aqufit.Page.ProfileUserSettingsId = " + ProfileSettings.Id + "; ";
                ret += "Aqufit.Page.ProfileUserName = '" + ProfileSettings.UserName + "'; ";
                ret += "Aqufit.Page.UserSettingsId = " + (UserSettings != null ? "" + UserSettings.Id : "-1") + "; ";
                ret += "Aqufit.Page.GroupSettingsId = " + (GroupSettings != null ? "" + GroupSettings.Id : "-1") + "; ";
                ret += "Aqufit.Page.GroupUserName = '" + (GroupSettings != null ? "" + GroupSettings.UserName : "") + "'; ";
                ret += "Aqufit.Page.GroupUserKey = " + (GroupSettings != null ? "" + GroupSettings.UserKey : "-1") + "; ";
                ret += "Aqufit.Page.MainGroupKey = " + (ProfileSettings.MainGroupKey.HasValue ? ProfileSettings.MainGroupKey.Value : -1) + "; ";
            }
            else
            {
                ret += "Aqufit.Page.ProfileId = -1; ";                
                ret += "Aqufit.Page.UserEmail = ''; ";
                ret += "Aqufit.Page.UserFirstName = ''; ";
                ret += "Aqufit.Page.UserLastName = ''; ";
                ret += "Aqufit.Page.ProfileUserSettingsId = -1; ";
                ret += "Aqufit.Page.ProfileUserName = ''; ";
                ret += "Aqufit.Page.UserName = ''; ";
                ret += "Aqufit.Page.UserSettingsId = -1; ";
                ret += "Aqufit.Page.GroupSettingsId = -1; ";
                ret += "Aqufit.Page.GroupUserName = ''; ";
                ret += "Aqufit.Page.GroupUserKey = -1; ";
                ret += "Aqufit.Page.MainGroupKey = -1; ";
                if (UserSettings != null)
                {
                    ret += "Aqufit.Page.UserName = '" + UserSettings.UserName + "'; ";
                    ret += "Aqufit.Page.UserSettingsId = " +  UserSettings.Id  + "; ";                    
                    ret += "Aqufit.Page.MainGroupKey = " + (UserSettings.MainGroupKey.HasValue ? UserSettings.MainGroupKey.Value : -1) + "; ";
                }               
                if (GroupSettings != null)
                {
                    ret += "Aqufit.Page.GroupSettingsId = " + GroupSettings.Id  + "; ";
                    ret += "Aqufit.Page.GroupUserName = '" + GroupSettings.UserName + "'; ";
                    ret += "Aqufit.Page.GroupUserKey = " + GroupSettings.UserKey + "; ";
                }
            }
            return ret;
        }



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
            try
            {
                base.Page_Load(sender, e);
                this.Permissions = AqufitPermission.PUBLIC;
                this.GroupPermissions = Utils.ConstsUtil.Relationships.NONE;
                this.Following = false;
                aqufitEntities entities = new aqufitEntities();
                if (this.UserId > 0)
                {             
                    this.UserSettings = entities.UserSettings.OfType<User>().Include("UserSettingsExtendeds").Include("SiteSettings").FirstOrDefault(s => s.UserKey == this.UserId && s.PortalKey == this.PortalId);
                }
                this.GroupSettings = null;
                if (HttpContext.Current.Items["g"] != null || Request["g"] != null)
                {
                    string groupname = HttpContext.Current.Items["g"] != null ? Convert.ToString(HttpContext.Current.Items["g"]) : Convert.ToString(Request["g"]);
                    this.GroupSettings = entities.UserSettings.OfType<Group>().Include("GroupType").Include("UserSettingsExtendeds").FirstOrDefault(g => g.PortalKey == this.PortalId && g.UserName == groupname);
                }
          //      throw new Exception("UserName: " + this.UserId);

                // We have to figure out the profile that is being viewed and setup the proper security settings for the person viewing the page.
                // - First off we need to figure out the profile page that is being requested.
                if (HttpContext.Current.Items["u"] != null || Request["u"] != null)
                {   // there was a user passed in the url.
                    string uname = HttpContext.Current.Items["u"] != null ? Convert.ToString(HttpContext.Current.Items["u"]) : Request["u"];
                   
                    // TODO: cache the user settings
                    this.ProfileSettings = entities.UserSettings.Include("Metrics").Include("UserSettingsExtendeds").Include("SiteSettings").FirstOrDefault(us => us.UserName == uname && us.PortalKey == this.PortalId);                    
                    //     this.ProfileSettings.Guid = Guid.NewGuid();
               //     entities.SaveChanges();
                    if (this.ProfileSettings == null)
                    {   // TODO: error handeling
                        throw new Exception("No User Profile found for: " + uname);
                        // DNN 5.0.3 
                        //return;
                    }
                    if (this.ProfileSettings is Group)
                    {
                        this.GroupSettings = this.ProfileSettings as Group;
                    }
                }
                else
                {   // there was no user name on the url.  So if there is a user logged in.  Lets defualt the page to them
                    if (this.UserId > 0)
                    {   
 
                        /* CA - this is old.. i am thinking that we just want to set UserSettings...
                        this.ProfileSettings = entities.UserSettings.OfType<User>().Include("Metrics").Include("UserSettingsExtendeds").FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
                        if (this.ProfileSettings == null)
                        {   // TODO: error handeling
                            throw new Exception("No User Profile found for uid: " + this.UserId);
                        }
                         */
                        this.UserSettings = entities.UserSettings.OfType<User>().Include("Metrics").Include("UserSettingsExtendeds").Include("SiteSettings").FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
                        if (this.UserSettings == null)
                        {   // TODO: error handeling
                            throw new Exception("No User Profile found for uid: " + this.UserId);
                        } 
                    }
                    else if (this.GroupSettings != null)
                    {
                        this.ProfileSettings = ((Affine.Data.UserSettings)this.GroupSettings);
                    }
                    else
                    {   // TODO: This person is not logged in.. send them a 404
                        //throw new Exception("404 Error needed here");
                        //Response.Redirect(ResolveUrl("~/"), true);

                        // This is a "public" connection to the page. (that might not have a URL routemapping)
                        return;
                    }
                }
                if (this.ProfileSettings != null && this.ProfileSettings is Group)
                {
                    this.GroupSettings = (Group)this.ProfileSettings;
                }
                // PERMISSIONS: So now we have a profileSettings object.  It is time to figure out what kind of permissions we have for the page
                // Permissions work with the following structure:
                //  1) You are the owner (full permissions)
                //  2) You are a friend (friend permissions)
                //  3) Public (public permissions)
                if (this.ProfileSettings != null && this.ProfileSettings.UserKey == this.UserId && this.ProfileSettings.PortalKey == this.PortalId)
                {   // PERMISSIONS: Set owner permissions
                    this.Permissions = AqufitPermission.OWNER;
                }
                else
                {
                    // TODO: this is expensive to query on EVERY postback
                    UserFriends areFriends = null;
                    UserFriends areFollowing = null;
                    if (this.UserSettings != null && this.ProfileSettings != null)
                    {
                        areFriends = entities.UserFriends.FirstOrDefault(f => ((f.SrcUserSettingKey == this.UserSettings.Id && f.DestUserSettingKey == this.ProfileSettings.Id) || (f.SrcUserSettingKey == this.ProfileSettings.Id && f.DestUserSettingKey == this.UserSettings.Id)) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND );
                        areFollowing = entities.UserFriends.FirstOrDefault(f => ((f.SrcUserSettingKey == this.UserSettings.Id && f.DestUserSettingKey == this.ProfileSettings.Id) ) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW);
                    }
                    if (areFriends != null )
                    {   // PERMISSIONS: set friend permissions
                        this.Permissions = AqufitPermission.FRIEND;                        
                    }
                    else if (areFollowing != null)
                    {
                        this.Following = true;
                    }
                    else
                    {
                        if (this.UserId > 0)
                        {
                            this.Permissions = AqufitPermission.USER;
                        }
                        else
                        {
                            // PERMISSIONS: Set public permissions
                            this.Permissions = AqufitPermission.PUBLIC;
                        }
                        // PERMISSIONS: We can check if there is a pending friend request for this person
                        // TODO: check pending requests.
                    }
                }
                if (this.GroupSettings != null)
                {
                    // First we need to find out the relationship that the user has with the group
                    if (UserSettings != null)
                    {
                        UserFriends relationship = entities.UserFriends.FirstOrDefault(f => (f.SrcUserSettingKey == this.GroupSettings.Id && f.DestUserSettingKey == UserSettings.Id || f.DestUserSettingKey == this.GroupSettings.Id && f.SrcUserSettingKey == UserSettings.Id));
                        if (relationship == null)
                        {   // This means that the user is NOT a member of the group TODO: check permissions that group is "Open"
                            //base.GroupSettings.GroupType.TypeName == "

                        }
                        else
                        {
                            GroupPermissions = Utils.ConstsUtil.IntToRelationship(relationship.Relationship);
                        }
                    }
                    else
                    {   // you are not logged in .. so just show basic details
                        GroupPermissions = Utils.ConstsUtil.Relationships.NONE;
                    }
                }                

                this.DistanceUnits = UserSettings != null && UserSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit(UserSettings.DistanceUnits.Value) : Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
                this.WeightUnits = UserSettings != null && UserSettings.WeightUnits != null ? Affine.Utils.UnitsUtil.ToUnit(UserSettings.WeightUnits.Value) : Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;   
                // END - setup for permissions
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "ProfileVars", GetProfileJs(), true);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        #endregion        
    }
}
