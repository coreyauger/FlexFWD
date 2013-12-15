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



namespace Affine.Dnn.Modules.ATI_Viral
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
    partial class ViewATI_Viral : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
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
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    atiFindInvite.UserSettings = UserSettings;
                    atiFindInvite.TabId = TabId;
                    atiProfileImage.Settings = UserSettings;
                    aqufitEntities entities = new aqufitEntities();
                    int rand = new Random(DateTime.Now.Millisecond).Next(25);
                    UserFriends uf = entities.UserFriends.OrderByDescending(f => f.Id).Skip(rand).FirstOrDefault(f => f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER);
                    Group group = entities.UserSettings.OfType<Group>().FirstOrDefault( g => g.Id == uf.DestUserSettingKey );                    
                    int numMembers = entities.UserFriends.Where(f => f.DestUserSettingKey == group.Id && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Count();
                    atiFeaturedGroup.Group = group;
                    atiFeaturedGroup.NumMembers = numMembers;
                    long[] memIds = entities.UserFriends.Where(f => f.DestUserSettingKey == group.Id && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).OrderBy(f => f.Id).Take(7).Select(f => f.SrcUserSettingKey).ToArray();
                    string[] memberUserNames = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, memIds)).Select(us => us.UserName).ToArray();
                    atiFeaturedGroup.MemberNames = memberUserNames;
                    imgAd.ImageUrl = ResolveUrl("/portals/0/images/adTastyPaleo.jpg");
                    if (Settings["ModuleMode"] != null && string.Compare( (string)Settings["ModuleMode"], "Modal") == 0 )
                    {
                        divRightAdUnit.Visible = false;
                        divLeftNav.Visible = false;
                        divCenterWrapper.Attributes["class"] = "";
                        divCenterWrapper.Attributes["style"] = "width: 729px;";
                    }

                                         
                }
                           
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }


        

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {
                    case "friendRequest":
                        long fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.sendFriendRequest(UserSettings.Id, fid);
                        status = "Friend request has been sent.";
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

