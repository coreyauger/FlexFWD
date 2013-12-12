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
using System.Web.Script.Serialization;

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

namespace Affine.Dnn.Modules.ATI_Streamer
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
    partial class ViewATI_Streamer : ATI_PermissionPageBase, IActionable
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
            try
            {
                base.Page_Load(sender, e);
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    
                    int numEntries = 3;
                    if (Settings["NumEntries"] != null)
                    {
                        numEntries = Convert.ToInt32(Settings["NumEntries"]);
                    }
                    Affine.WebService.StreamService streamService = new WebService.StreamService();
                    string json = streamService.getRecentWorkouts(this.PortalId, 0, numEntries);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "WorkoutList", "$(function(){ Aqufit.Page.atiStreamScript.generateStreamDom('" + json + "'); });", true);
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
                Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
                Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;

                switch (hiddenAjaxAction.Value)
                {
                     case "delStream":
                        long sid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteStream(UserSettings, sid);
                        status = "Your stream item has been deleted.";
                        RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
                        break;
                    case "delComment":
                        long cid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteComment(UserSettings, cid);
                        status = "Your commnet has been deleted.";
                        RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
                        break;
                    case "AddComment":
                        try
                        {
                            JavaScriptSerializer jserializer = new JavaScriptSerializer();
                            Shout shoutRet = dataManager.SaveShout(-1, this.UserId, -1, this.PortalId, hiddenAjaxValue.Value);
                            // get the json serializable version of the object.
                            Affine.Data.json.StreamData sd = Affine.Data.Managers.LINQ.StreamManager.Instance.UserStreamEntityToStreamData(shoutRet, null);
                            // RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.Controls.atiStreamPanel.prependJson(" + _jserializer.Serialize(sd) + "); })();"); 
                            string json = jserializer.Serialize(sd);
                            RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.atiStreamScript.prependJson('" + json + "'); Aqufit.Page.atiComment.clear(); })();");
                            //Affine.WebService.StreamService service = new WebService.StreamService();
                            //RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.atiStreamScript.prependJson('" + service.SaveStreamShout(-1, this.UserId, this.PortalId, hiddenAjaxValue.Value) + "'); Aqufit.Page.atiComment.clear(); })();");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;                   
                }
            }
            catch (Exception ex)
            {
                //status = "ERROR: There was a problem with the action (" + ex.Message + ")";
                RadAjaxManager1.ResponseScripts.Add(" alert('" + ex.Message + "'); ");
            }
        }
     

        /*
        private string GetFlexEmbed()
        {
            string ret = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" id=\"ATI_Streamer\" width=\"100%\" height=\"100%\" codebase=\"http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab\">";
            ret += "<param name=\"movie\" value=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/ATI_Aqufit.swf") + "\" />";
            ret += "<param name=\"quality\" value=\"high\" /><param name=\"bgcolor\" value=\"#FFFFFF\" />";
            ret += "<param name=\"flashvars\" value=\"u=" + this.UserId + "&m=design&url=http://flexfwd.com/\" />";
            ret += "<param name=\"allowScriptAccess\" value=\"sameDomain\" />";
            ret += "<param name=\"wmode\" VALUE=\"transparent\">";
            ret += "<embed src=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/ATI_Aqufit.swf") + "\" wmode=\"transparent\" quality=\"high\" bgcolor=\"#FFFFFF\" width=\"100%\" height=\"100%\" name=\"ATI_Streamer\" align=\"middle\" play=\"true\" loop=\"false\" quality=\"high\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\"";
            ret += " flashvars=\"u=" + this.UserId + "&m=design&url=http://flexfwd.com/\"";
            ret += " pluginspage=\"http://www.adobe.com/go/getflashplayer\"></embed></object>";
            return ret;
        }
            */          

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

