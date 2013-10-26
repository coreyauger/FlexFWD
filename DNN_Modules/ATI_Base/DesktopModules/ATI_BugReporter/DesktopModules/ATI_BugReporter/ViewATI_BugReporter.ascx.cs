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

namespace Affine.Dnn.Modules.ATI_BugReporter
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
    partial class ViewATI_BugReporter : ATI_PageBase, IActionable
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
                bool IsContactMode = false;
                if( (Settings["ConfigAsContact"] != null && Convert.ToBoolean(Settings["ConfigAsContact"])) ){
                    IsContactMode = true;
                    adminTitle.InnerHtml = "Open Contact Requests";
                }
                if (this.UserInfo.IsSuperUser)
                {
                    aqufitEntities entities = new aqufitEntities();
                    atiAdminPanel.Visible = true;
                    atiAjaxPanel.Visible = false;
                    EntityDataSource eds = new EntityDataSource();
                    eds.ID = "aqufitEntities";
                    eds.ContextTypeName = "Affine.Data.aqufitEntities";
                   // eds.ConnectionString = "name=aqufitEntities";
                    eds.EntitySetName = "BugReports";
                   // eds.DefaultContainerName = "aqufitEntities";
                    eds.OrderBy = "it.[DateTime] DESC";
                    eds.EntityTypeFilter = "BugReport";
                    eds.EnableDelete = true;
                    eds.EnableInsert = true;
                    eds.EnableUpdate = true;
                    if (Request["b"] != null)
                    {
                        eds.Where = "it.Id = " + Request["b"] + " && it.Status != 'Closed' && it.PortalId = " + this.PortalId + " && it.IsContactRequest = " + IsContactMode;
                    }
                    else
                    {
                        eds.Where = "it.Status != 'Closed' && it.PortalId = " + this.PortalId + " && it.IsContactRequest = " + IsContactMode;
                    }                           
                    RadGrid1.DataSource = eds;                   
                    RadGrid1.MasterTableView.DataSource = eds;
                }

                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    if (Settings["ConfigAsContact"] != null && Convert.ToBoolean( Settings["ConfigAsContact"] ))
                    {
                        panelBugHead.Visible = false;
                        panelContactHead.Visible = true;
                        imgContact.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iContact.png");
                        if (this.UserId < 0)
                        {
                            divEmail.Visible = true;
                        }                        
                    }
                    else
                    {
                        panelBugHead.Visible = true;
                        panelContactHead.Visible = false;
                        imgBug.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iBug.png");
                        if (this.UserId < 0)
                        {
                            divEmail.Visible = true;
                        }
                    }
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void RadGrid1_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                if (!e.Item.OwnerTableView.IsItemInserted)
                {
                    GridEditableItem item = e.Item as GridEditableItem;

                    GridEditManager manager = item.EditManager;

                    GridTextBoxColumnEditor editor = manager.GetColumnEditor("Id") as GridTextBoxColumnEditor;

                    editor.TextBoxControl.Enabled = false;
                }

            }
        }

        protected void bSend_Click(object sender, EventArgs e)
        {            
            aqufitEntities entities = new aqufitEntities();
            UserSettings settings = null;
            if (this.UserId > 0)
            {
                settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
            }
            string un = settings != null ? settings.UserName : "";
            string url = Request["u"] != null ? Request["u"] : Request.Url.AbsoluteUri;
            long tab = Request["t"] != null ? Convert.ToInt64(Request["t"]) : this.TabId;
            string email = string.Empty;
            if (this.UserId > 0)
            {
                email = entities.UserSettings.FirstOrDefault(u => u.Id == this.UserId && u.PortalKey == this.PortalId).UserEmail;
            }
            else
            {
                email = txtEmail.Text;
            }
            BugReport bug = new BugReport()
            {
                Description = txtDescription.Text,
                UserAgent = Request.UserAgent,
                UserId = this.UserId,
                UserName = un,
                PortalId = this.PortalId,
                PortalName = this.PortalAlias.HTTPAlias,
                AbsoluteUrl = Request.Url.AbsoluteUri,
                AbsoluteUrlReferrer = url,
                DateTime = DateTime.Now,
                Ip = Request.ServerVariables["REMOTE_ADDR"],
                RawUrl = Request.RawUrl,
                ScreenResolution = hiddenScreenRes.Value,
                Status = "Open",
                Email = email,
                ActiveTabId = this.TabId,
                IsContactRequest = panelContactHead.Visible
            };
            entities.AddToBugReports(bug);
            entities.SaveChanges();
            bSend.Visible = false;
            txtDescription.Visible = false;
            if (panelBugHead.Visible)   // This moduel is configured to report bugs
            {
                sendBugNotificationEmailAsync(bug);
                litStatus.Text = "<em>Thank You</em>.  Bear with us as we work out some of the kinks in our system.";
            }
            else
            {
                sendContactNotificationEmailAsync(bug);
                litStatus.Text = "<em>Thank You</em>.  Your request has been sent.  We will get back to you as soon as we possibly can.";
            }                                         
        }

        private void sendBugNotificationEmailAsync(BugReport bug)
        {
            string emailBody = "Bug Report\n\n";
            emailBody += "Email: " + bug.Email + "\n";
            emailBody += "PortalName: " + bug.PortalName + "\n";
            emailBody += "PortalId: " + bug.PortalId + "\n";
            emailBody += "AbsoluteUrl: " + bug.AbsoluteUrl + "\n";
            emailBody += "AbsoluteUrlReferrer: " + bug.AbsoluteUrlReferrer + "\n";
            emailBody += "DateTime: " + bug.DateTime.ToShortDateString() + "\n";
            emailBody += "\nDescription: \n" + bug.Description + "\n\n";
            emailBody += "To see the message thread, follow the link below:\n";
            emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/Admin/BugReports.aspx") + "?b="+bug.Id;

            string subject = "BUG: A bug has been reported for '" + bug.PortalName + "'";

            Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
            
            aqufitEntities entities = new aqufitEntities();
            gmail.Send("coreyauger@gmail.com", subject, emailBody);            
        }

        private void sendContactNotificationEmailAsync(BugReport bug)
        {
            string emailBody = "Contact Request\n\n";
            emailBody += "Email: " + bug.Email + "\n";
            emailBody += "PortalName: " + bug.PortalName + "\n";
            emailBody += "PortalId: " + bug.PortalId + "\n";
            emailBody += "AbsoluteUrl: " + bug.AbsoluteUrl + "\n";
            emailBody += "AbsoluteUrlReferrer: " + bug.AbsoluteUrlReferrer + "\n";
            emailBody += "DateTime: " + bug.DateTime.ToShortDateString() + "\n";
            emailBody += "\nDescription: \n" + bug.Description + "\n\n";
            emailBody += "To see the message thread, follow the link below:\n";
            emailBody += "http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/Admin/ContactRequests.aspx") + "?b=" + bug.Id;

            string subject = "CONTACT: A contact request from '" + bug.PortalName + "'";

            Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();

            aqufitEntities entities = new aqufitEntities();
            gmail.Send("coreyauger@gmail.com", subject, emailBody);
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

