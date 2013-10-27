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
using System.IO;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

using Telerik.Web.UI;
using Affine.Data;

namespace Affine.Dnn.Modules.ATI_AdminTools
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
    partial class ViewATI_AdminTools : ATI_PageBase, IActionable
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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private UserSettings populateUserSettings(UserSettings us, UserInfo ui)
        {
            aqufitEntities entities = new aqufitEntities();

            long usId = us.Id;
            us.UserKey = ui.UserID;
            us.PortalKey = ui.PortalID;
            us.UserEmail = ui.Email;
            us.UserName = ui.Username;
            us.UserFirstName = ui.FirstName;
            us.UserLastName = ui.LastName;
            //us.TimeZone = ui.Profile.TimeZone;          
            return us;
        }

        /*
        private long RegisterGroup(string name, string email, string address, string city, string url, string phone, double lat, double lng)
        {
            //3-933 Ellery Street, Victoria, BC V9A 4R9, Canada
            //1840 Gadsden Hwy # 112, Birmingham, AL            
            string postal = string.Empty; 
            string street = string.Empty; 
            string region = string.Empty;
            string country = string.Empty;
            if (address != null)
            {
                string[] split = address.Split(',');
                if (split.Length > 0)
                {
                    street = split[0];
                }
                if (split.Length > 1)
                {
                    // city we have
                }
                if (split.Length > 2)
                {
                    region = split[2];
                }
            }
            // check required fields               
            string uName = name.Replace(" ", "_").Replace("'", "");
            aqufitEntities entities = new aqufitEntities();
            Group test = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.UserName == uName);
                // if a user is found with that username, error. this prevents you from adding a username with the same name as a superuser.
           if (test != null)
           {
               throw new Exception("We already have an entry for the Group Name.");

           }            
            Affine.WebService.RegisterService regService = new WebService.RegisterService();                     
            Group us = new Data.Group();
            us.UserKey = 0;
            us.PortalKey = PortalId;
            us.UserEmail = email;
            us.UserName = uName;
            us.UserFirstName = name;
            us.UserLastName = "";
            us.GroupType = entities.GroupTypes.FirstOrDefault(g => g.Id == 1);
            us.DefaultMapLat = lat;
            us.DefaultMapLng = lng;
            us.LngHome = lat;
            us.LngHome = lng;
            //us.Status = atTxtGroupDescription.Text;
            // TODO: don't make the Place required for groups..
            
            us.Places.Add(new Place()
            {
                City = city,
                Country = country,
                Email = email,
                Lat = lat,
                Lng = lng,
                Name = name,
                Postal = postal,
                Region = region,
                Street = street,
                Website = url,
                Phone = phone
            });
            entities.AddToUserSettings(us);
            entities.SaveChanges();
            // Now assign the creator of the group as an admin
            Group group = entities.UserSettings.OfType<Group>().First(g => g.UserName == us.UserName);
            return group.Id;
            
        }
        */

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                switch (hiddenAjaxAction.Value)
                {
                    case "su":
                        long uid = Convert.ToInt64(hiddenAjaxValue.Value);
                        aqufitEntities entities = new aqufitEntities();
                        User fbUser = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid);
                        UserController uc = new UserController();
                        UserInfo ui = uc.GetUser((int)fbUser.PortalKey, (int)fbUser.UserKey);
                        UserController.UserLogin(PortalId,ui,PortalSettings.PortalName, Request.UserHostAddress, true);
                        Response.Redirect(ResolveUrl("~"), true); 
                        break;                    
                }
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog('" + ex.Message.Replace("'", "") + "'); ");
            }
        }

        protected void RadGridUsers_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            RadGridUsers.DataSource = entities.UserSettings.OfType<User>().Where( u => u.PortalKey == PortalId).OrderBy(u => u.UserName).Select(u => new { UserName = u.UserName, Id = u.Id, UserFirstName = u.UserFirstName, UserLastName = u.UserLastName, FBKey = u.FBUid, UserEmail = u.UserEmail }).ToArray();
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

