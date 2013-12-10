/*
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
 */

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

namespace Affine.Dnn.Modules.ATI_Requests
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
    partial class ViewATI_Requests : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
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
                    if (base.Permissions == AqufitPermission.PUBLIC)
                    {   // You need to be logged in to see this page.. redirect to login
                        Response.Redirect(ResolveUrl("~/Login.aspx"), true);
                        return;
                    }
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    atiProfile.ProfileSettings = UserSettings;
                    atiProfile.IsOwner = true;
                    imgIphoneAd.ImageUrl = ResolveUrl("~/Portals/0/images/adTastyPaleo.jpg");
                    // CA - you should only ever see this page as an owner..
                    atiFriendRequestScript.IsOwner = true;  

                    if (Permissions == AqufitPermission.PUBLIC)
                    {
                        Response.Write("TODO: error page => You can not view this persons friends.. you are not friends with them");
                    }
                    else
                    {
                        SetupStreams();
                        
                    }
                }                
               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        private void SetupStreams()
        {
            // We need to get all the different types of requests that belong to the user.
            // - Once we have all the request.  Divide them up into categories by object type.
            // - Display each type using the Request List control.
            WebService.StreamService streamService = new WebService.StreamService();
            Affine.Data.Managers.IStreamManager streamMan = Affine.Data.Managers.LINQ.StreamManager.Instance;
            Affine.Data.json.UserSetting[] freindRequest = streamMan.GetFriendRequests(this.UserSettings.Id);
            Affine.Data.json.UserSetting[] groupInvite = streamMan.GetGroupInviteRequests(this.UserSettings.Id);
            string json = streamMan.ToJsonWithPager(freindRequest);
            string json2 = streamService.getNotifications(UserSettings.Id, 0, 50);
            string js = string.Empty;
            if (Request["n"] != null)
            {
                aqufitEntities entities = new aqufitEntities();
                long nid = Convert.ToInt64(Request["n"]);
                Notification notification = entities.UserStreamSet.OfType<Notification>().FirstOrDefault(n => n.Id == nid);
                if (notification != null)
                {
                    notification.PublishSettings = (int)Affine.Utils.ConstsUtil.PublishSettings.NOTIFICATION_READ;  // this will take it out of the top menu since the user clicked to deal with it..
                    entities.SaveChanges();
                    js += "var $sel = $('#atiStreamItem" + Request["n"] + "');";
                    js += "var targetOffset = $sel.offset().top; ";
                    js += "$sel.css('background-color','#ffcc99').css('border', '2px solid #e47526'); ";
                    js += "$('html,body').animate({scrollTop: targetOffset}, 500);";
                }
            }
            if (groupInvite.Length > 0)
            {
                atiGroupJoinRequest.Visible = true;
                string json3 = streamMan.ToJsonWithPager(groupInvite);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "GroupInviteList", "$(function(){ Aqufit.Page.atiGroupJoinRequest.generateStreamDom('" + json3 + "'); });", true);
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "NotificationList", "$(function(){ Aqufit.Page.atiFriendRequestScript.generateStreamDom('" + json + "'); Aqufit.Page.atiStreamScript.generateStreamDom('" + json2 + "'); " + js + " });", true);
        }


        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                aqufitEntities entities = new aqufitEntities();
                long fid = 0;
                switch (hiddenAjaxAction.Value)
                {
                    case "friendAccept":
                        fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.AcceptFriendRequests(UserSettings.Id, fid);
                        status = "Friend has been added.";
                        break;
                    case "friendReject":
                        fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.RejectFriendRequests(UserSettings.Id, fid);
                        status = "Friend request rejected.";
                        break;
                    case "deleteNotification":
                        long nid = Convert.ToInt64(hiddenAjaxValue.Value);                        
                        Notification notification = entities.UserStreamSet.OfType<Notification>().FirstOrDefault(n => n.Id == nid && n.UserSetting.Id == UserSettings.Id );
                        if (notification != null)
                        {
                            entities.DeleteObject(notification);
                            entities.SaveChanges();
                        }
                        break;
                    case "joinGroup":
                        fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        GroupInviteRequest request = entities.UserRequestSet.Include("UserSetting").OfType<GroupInviteRequest>().FirstOrDefault(r => r.FriendRequestSettingsId == UserSettings.Id && r.UserSetting.Id == fid);
                        dataMan.JoinGroup(UserSettings.Id, request.UserSetting.Id, Utils.ConstsUtil.Relationships.GROUP_MEMBER);
                        request.Status = 0;
                        entities.SaveChanges();
                        break;
                    case "deleteRequest":
                        fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        GroupInviteRequest delRequest = entities.UserRequestSet.Include("UserSetting").OfType<GroupInviteRequest>().FirstOrDefault(r => r.FriendRequestSettingsId == UserSettings.Id && r.UserSetting.Id == fid);
                        entities.DeleteObject(delRequest);
                        break;
                    case "AddSuggestFriend":
                        try
                        {
                            long usid = Convert.ToInt64(hiddenAjaxValue.Value);
                            status = dataMan.sendFriendRequest(UserSettings.Id, usid);
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiProfile.ShowOk('A Friend request has been sent');");
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
                status = "ERROR: There was a problem with the action (" + ex.Message + ")";
            }
            RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
        }

        protected void bRemoveAll_Click(object sender, EventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            Notification[] notifications = entities.UserStreamSet.OfType<Notification>().Where(n => n.UserSetting.Id == UserSettings.Id ).ToArray();
            foreach (Notification notification in notifications)
            {
                entities.DeleteObject(notification);
            }
            entities.SaveChanges();
            Response.Redirect("~/fitnessprofile/requests", true);
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

