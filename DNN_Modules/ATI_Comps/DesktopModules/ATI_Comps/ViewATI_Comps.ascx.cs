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

using Telerik.Web.UI;

using Affine.Data;
using Affine.Data.EventArgs;
using Affine.Utils;
using Affine.Utils.Linq;

namespace Affine.Dnn.Modules.ATI_Comps
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
    partial class ViewATI_Comps : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members
        private string baseUrl = "";
        #endregion       

        #region Public Methods
        public string BackgroundImageUrl { get; set; }
        public string ProfileCSS { get; set; }
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
                this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx")+"?u=-1&p=0&bg=1";
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    baseUrl = ResolveUrl("~/");
                    aqufitEntities entities = new aqufitEntities();
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);

                    if (GroupSettings != null || (UserSettings != null && UserSettings.MainGroupKey.HasValue))
                    {
                        tabMyGroupComps.Visible = true;
                        pageMyGroupComps.Visible = true;
                      //  if (GroupSettings != null)
                     //   {
                   //         RadAjaxManager1.ResponseScripts.Add("Aqufit.addLoadEvent(function(){ Aqufit.Page.Tabs.SwitchTab(1); });");
                 //       }
                    }

                    Group taranis = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == 511); ;
                    atiFGProfileImg.Settings = taranis;
                    hrefGroupLink2.HRef = baseUrl + "group/" + taranis.UserName;
                    imgSearch.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iSearch.png");
                    if (GroupSettings == null)
                    {       // Let people search for a group in this case..
                        atiGroupListPanel.Visible = true;                        
                        
                        if (this.UserSettings != null && (this.UserSettings.LatHome != null && this.UserSettings.LatHome.Value > 0.0))
                        {
                            atiGMap.Lat = this.UserSettings.LatHome.Value;
                            atiGMap.Lng = this.UserSettings.LngHome.Value;
                            atiGMap.Zoom = 13;
                        }
                        imgAd.Src = ResolveUrl("~/portals/0/images/adTastyPaleo.jpg");
                        WebService.StreamService streamService = new WebService.StreamService();                       
                       // string search = streamService.searchGroupListData(PortalId, null, 0, 15);
                        
                        // we need to setup for a location based group search                    
                       // ScriptManager.RegisterStartupScript(this, Page.GetType(), "GroupSearch", "$(function(){ Aqufit.Page.atiGroupSearch.generateStreamDom('" + search + "'); });", true);
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
            try
            {
                /*
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {
                    case "delStream":
                        long sid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.deleteStream(UserSettings, sid);
                        break;                                      
                }
                 */
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('ERROR: There was a problem with the action (" + ex.Message.Replace("'","").Replace("\r","").Replace("\n","") + ")');");
            }
        }


        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            Competition comp = null;
            long compKey = 0;
            if (Request["c"] != null)
            {
                compKey = Convert.ToInt64(Request["c"]);
                comp = entities.Competitions.FirstOrDefault(c => c.Id == compKey);
            }
            if (compKey <= 0)
            {
                comp = entities.Competitions.FirstOrDefault();
            }
            IQueryable<Competition> comps = entities.Competitions.Include("Groups").Where(c => c.CompetitionStartDate.CompareTo(DateTime.Now) < 0).OrderByDescending( c => c.CompetitionStartDate );
            RadGrid1.DataSource = comps.Select(c =>
                    new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Group = (c.Group != null ? c.Group.UserFirstName : ""),
                        StartDate = c.CompetitionStartDate,
                        EndDate = c.CompetitionEndDate                       
                    }
                ).ToArray();
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                Control target = e.Item.FindControl("targetControl");
                if (!Object.Equals(target, null))
                {
                    HyperLink a = (HyperLink)target;
                    a.NavigateUrl = ResolveUrl("~/Community/CompetitionScoring?c=" + (e.Item as GridDataItem).GetDataKeyValue("Id").ToString());
                   // if (!Object.Equals(this.RadToolTipManager1, null))
                   // {
                        //Add the button (target) id to the tooltip manager
                   //     this.RadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("Id").ToString(), true);
                   // }
                }
                Label lbl = e.Item.FindControl("numberLabel") as Label;
                if (!Object.Equals(lbl, null))
                {
                    lbl.Text = Convert.ToString((RadGrid1.MasterTableView.CurrentPageIndex * RadGrid1.MasterTableView.PageSize) + (e.Item.ItemIndex + 1));
                }
            }
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {            
            ConfigureExport();
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName)
            {
                RadGrid1.MasterTableView.ExportToExcel();
            }
            else if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
            {
                RadGrid1.MasterTableView.ExportToWord();
            }
            else if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                RadGrid1.MasterTableView.ExportToCSV();
            }
        }

        public void ConfigureExport()
        {
            RadGrid1.ExportSettings.ExportOnlyData = false;
            RadGrid1.ExportSettings.IgnorePaging = false;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
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
            //    Actions.Add(this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), ModuleActionType.AddContent, "", "", this.EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }
}

