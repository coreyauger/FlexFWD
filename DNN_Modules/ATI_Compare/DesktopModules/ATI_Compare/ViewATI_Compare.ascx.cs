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



using Affine.Data;
using Affine.Data.EventArgs;
using Affine.Utils;
using Affine.Utils.Linq;

namespace Affine.Dnn.Modules.ATI_Compare
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
    partial class ViewATI_Compare : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members    
        
        private const int DEFAULT_TAKE = 15;
        private const int TAKE_INC = 10;
        private Affine.Data.Managers.IStreamManager _IStreamManager = Affine.Data.Managers.LINQ.StreamManager.Instance;
        private JavaScriptSerializer _jserializer = new JavaScriptSerializer();

        #endregion       

        #region Public Methods
        
        #endregion        

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
           // Page.Title = "FlexFWD: " + (this.ProfileSettings != null ? this.ProfileSettings.UserName : "" );
        }               

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

                    lUserName.Text = "<h2>" + ProfileSettings.UserName + "</h2>";
                    lUserNameYou.Text = "<h2 style=\"float: right;\">" + UserSettings.UserName + "</h2>";
                    atiProfile.ProfileSettings = base.ProfileSettings;
                    atiProfile.IsFriend = base.Permissions == AqufitPermission.FRIEND;
                    atiProfile.IsFollowing = base.Following;
                    atiProfileYou.ProfileSettings = base.UserSettings;
                    atiProfileYou.IsOwner = true;

                    imgAddToFriends.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iAddToFriends.png");
                    imgAddToFollow.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iAddToFollow.png");
                }
                atiProfile.IsFollowing = base.Following;                
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }

        protected void bExportToCsv_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = ProfileSettings.UserName + "_vs_" + UserSettings.UserName;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.MasterTableView.ExportToCSV();
        }

        protected void bExportToExcel_Click(object sender, EventArgs e)
        {
            RadGrid1.ExportSettings.FileName = ProfileSettings.UserName + "_vs_" + UserSettings.UserName;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.MasterTableView.ExportToExcel();
        }

        protected void RadGrid1_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable< Workout > workouts = entities.UserStreamSet.OfType<Workout>().Where(w => (w.UserSetting.Id == UserSettings.Id || w.UserSetting.Id == ProfileSettings.Id ) && w.IsBest == true).GroupBy( w => w.WOD.Id ).Where( g => g.Count() > 1 ).SelectMany( g => g.Select( w => w ) );
            var data = workouts.Select( w => new{ Id = w.Id, Title = w.Title, WODKey = w.WOD.Id,
                                            WODType = w.WOD.WODType.Id,
                                            UserName = w.UserSetting.UserName,
                                            Date = w.Date,
                                            Score = (w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED ?                                            
                                            w.Duration.Value 
                                            :
                                             ( w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT ?
                                             w.Max.Value
                                             :
                                            w.Score.Value ))} ).ToArray();
            var dataMessage = data.OrderBy( d => d.Score ).Select(d => new
            {
                Id = d.Id,
                Title = d.Title,
                WODKey = d.WODKey,
                UserName = d.UserName,
                WODType = d.WODType,   
                Date = d.Date,
                Score = (d.WODType == (int)Affine.Utils.WorkoutUtil.WodType.TIMED ?
                    Affine.Utils.UnitsUtil.durationToTimeString( (long)d.Score )
                :
                 (d.WODType == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT ?
                    Affine.Utils.UnitsUtil.systemDefaultToUnits(d.Score, base.WeightUnits) + " " + Affine.Utils.UnitsUtil.unitToStringName(base.WeightUnits)
                 :
                "" + d.Score))
            }).ToArray();
            RadGrid1.DataSource = dataMessage;
        }
        

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            
            string status = string.Empty;
            try
            {

                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {                    
                    case "AddFriend":
                        try
                        {
                            status = dataMan.sendFriendRequest(UserSettings.Id, this.ProfileSettings.Id);
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Action.ShowOk('A Friend request has been sent to " + ProfileSettings.UserName + "');");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Action.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;
                    case "AddFollow":
                        try
                        {
                            Affine.WebService.StreamService ss = new Affine.WebService.StreamService();   
                            status = ss.FollowUser(UserSettings.Id, ProfileSettings.Id);
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Action.ShowOk('You are now following " + ProfileSettings.UserName + "');");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Action.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;                    
                }
            }
            catch (Exception ex)
            {
                //status = "ERROR: There was a problem with the action (" + ex.Message + ")";
                RadAjaxManager1.ResponseScripts.Add(" alert('"+ex.Message+"'); ");
            }
        }


        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fitnesss the module actions required for interfacing with the portal framework
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

