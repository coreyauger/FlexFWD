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
using Telerik.Web.UI.Upload;


using Affine.Data;

namespace Affine.Dnn.Modules.ATI_MessagesSend
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
    partial class ViewATI_MessagesSend : ATI_PermissionPageBase, IActionable
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
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);

                    aqufitEntities entities = new aqufitEntities();                                        
                    // First thing we want to do here is to varify that the people are really friends
                    if (ProfileSettings != null)    // if profilesettins is null .. then this is the user opening the send interface from their "Inbox"
                    {   
                        UserFriends friend = entities.UserFriends.FirstOrDefault(f => (f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == ProfileSettings.Id) || (f.SrcUserSettingKey == ProfileSettings.Id && f.DestUserSettingKey == UserSettings.Id));
                        if (friend != null)
                        {
                            Affine.Data.User friendProfile = entities.UserSettings.OfType<User>().FirstOrDefault(f => f.UserKey == ProfileSettings.UserKey && f.PortalKey == this.PortalId);
                            atiSendMessage.UserSettings = new UserSettings[] { friendProfile };
                        }
                    }
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                Affine.WebService.StreamService service = new WebService.StreamService();
                service.SendMessage(UserSettings.Id, atiSendMessage.To.ToArray(), atiSendMessage.Subject, atiSendMessage.Message);
                litStatus.Text = "Your message has been sent.";
                atiSendMessage.Visible = false;
                bSend.Visible = false;
                bClose.Visible = true;
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiSendMessage.refresh();");
                litStatus.Text = "ERROR: There was a problem sending your message. " + ex.Message;
            }
        }

        private void sendEmailAsync(Message message)
        {
            string emailBody = string.Empty;
            emailBody += "Subject: \"" + message.Subject + "\"\n\n";
            emailBody += message.Text + "\n\n";
            emailBody += "To see reply to the message, follow the link below:\n";
            emailBody += "http://" + Request.Url.Host +  System.Web.VirtualPathUtility.ToAbsolute("~/Inbox.aspx") + "?m=" + message.Id;          
            
            aqufitEntities entities = new aqufitEntities();
            UserSettings sender = entities.UserSettings.FirstOrDefault(us => us.UserKey == message.UserSetting.UserKey && us.PortalKey == message.UserSetting.PortalKey );
            Affine.Utils.GmailUtil email = new Utils.GmailUtil();
            foreach (MessageRecipiant r in message.MessageRecipiants)
            {
                if (message.UserSetting.UserKey != r.UserSettingsKey)
                {
                    // TODO: cache
                    UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == r.UserSettingsKey && us.PortalKey == message.UserSetting.PortalKey);
                    if (settings != null)
                    {
                        email.Send(settings.UserEmail, "New message from " + sender.UserName,"New message from " + sender.UserName + "\n\n" + emailBody);                        
                    }
                }
            }           
        }
      
      
     
        protected void bPostBack_Click(object sender, EventArgs e)
        {

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

