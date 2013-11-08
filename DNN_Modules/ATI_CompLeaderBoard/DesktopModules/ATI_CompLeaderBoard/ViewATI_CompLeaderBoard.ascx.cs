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

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;

using Affine.Data;

using Telerik.Web.UI;


namespace Affine.Dnn.Modules.ATI_CompLeaderBoard
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
    partial class ViewATI_CompLeaderBoard : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
    {

        #region Private Members              
               
        private string baseUrl = string.Empty;
        #endregion       

        #region Public Methods    
        

        public bool IsMyWorkouts
        {
            get
            {
                if (ViewState["IsMyWorkouts"] == null)
                {
                    return false;
                }
                return Convert.ToBoolean(ViewState["IsMyWorkouts"]);
            }
            set
            {
                ViewState["IsMyWorkouts"] = value;
            }

        }

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
                    aqufitEntities entities = new aqufitEntities();
                    if (Request["a"] != null)
                    {
                        long aId = Convert.ToInt64(Request["a"]);
                        CompetitionAffiliate affiliate = entities.CompetitionAffiliates.FirstOrDefault(a => a.Id == aId);
                        atiRadComboSearchAffiliates.Items.Add(new RadComboBoxItem() { Selected = true, Text = affiliate.Name, Value = "" + affiliate.Id });
                        // set the sex to all
                        rblSex.Items.FindByValue("A").Selected = true;
                    }
                    
                    baseUrl = ResolveUrl("~/");                   
                    
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
                    workoutTabTitle.Text = "Competition: "+comp.Name;
                    atiWorkoutPanel.Visible = true;
                        
                    var regionList = entities.CompetitionRegions.Select(r => new {Text = r.Name, Id = r.Id }).ToList();
                    ddlRegion.DataTextField = "Text";
                    ddlRegion.DataValueField = "Id";                                         
                   // ddlRegion.DataSource = regionList;                        
                 //   ddlRegion.DataBind();
                    ddlRegion.Items.Add(new ListItem() { Text = "All Regions", Value = "0", Selected = true });
                    foreach (var item in regionList)
                    {
                        ddlRegion.Items.Add(new ListItem() { Text = item.Text, Value = "" + item.Id });
                    }
                    //RadGrid1.MasterTable

                    //var regionList = entities.Com.Select(r => new {Text = r.Name, Id = r.Id }).ToList();
                    //ddlAffiliate
                    if (RadGrid1.MasterTableView.SortExpressions.Count == 0)
                    {
                        GridSortExpression expression = new GridSortExpression();
                        expression.FieldName = "OverallRank";
                        expression.SortOrder = GridSortOrder.Ascending;
                        RadGrid1.MasterTableView.SortExpressions.AddSortExpression(expression);
                    }
                    User angie = entities.UserSettings.OfType<User>().Include("CompetitionAthletes").FirstOrDefault(u => u.Id == 515);
                    atiFeaturedProfile.Settings = angie;
                    litRank.Text = "<span class=\"grad-FFF-EEE\" style=\"float: right; display: inline-block; border: 1px solid #ccc; text-align: center; padding: 7px;\">World Rank<br /><em style=\"font-size: 18px;\">" + angie.CompetitionAthletes.First().OverallRank + "</em></span>";

                    //grad-FFF-EEE
                }                           
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
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
            IQueryable<CompetitionAthlete> athleteQuery = entities.CompetitionAthletes.Include("CompetitionWODs").Where(a => a.Competition.Id == comp.Id);
            if (rblSex.SelectedValue != "A" )
            {
                athleteQuery = athleteQuery.Where(a => a.Sex == rblSex.SelectedValue);
            }
            long rid = Convert.ToInt64( ddlRegion.SelectedValue );
            if (rid > 0)
            {
                athleteQuery = athleteQuery.Where(a => a.CompetitionRegion.Id == rid);
            }

            try
            {
                long aId = Convert.ToInt64(atiRadComboSearchAffiliates.SelectedValue);
                if (aId > 0)
                {
                    athleteQuery = athleteQuery.Where(a => a.CompetitionAffiliate.Id == aId);
                }                
            }
            catch (Exception) { }
            RadGrid1.DataSource = athleteQuery.Select(a =>
                    new {
                        FlexId =  (a.UserSetting != null ? a.UserSetting.UserName : ""),
                        AffiliateName = a.AffiliateName,
                        AthleteName = a.AthleteName,
                        Country = a.Country,
                        Height = a.Height,
                        Age = (a.BirthYear.HasValue ? (DateTime.Today.Year - a.BirthYear.Value) : 0),
                        Hometown = a.Hometown,
                        Id = a.Id,
                        ImgUrl = a.ImgUrl,
                        OverallRank = a.OverallRank,
                        OverallScore = a.OverallScore,
                        RegionKey = (a.CompetitionRegion != null ? a.CompetitionRegion.Id : 0),
                        Sex = a.Sex,
                        RegionName = a.RegionName,
                        Weight = a.Weight,
                        UId = a.UId,
                        W1Score = a.CompetitionWODs.Count > 0 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 0).Score : 0,
                        W1Rank = a.CompetitionWODs.Count > 0 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 0).Rank  : 0,
                        W2Score = a.CompetitionWODs.Count > 1 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 1).Score : 0,
                        W2Rank = a.CompetitionWODs.Count > 1 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 1).Rank : 0,
                        W3Score = a.CompetitionWODs.Count > 2 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 2).Score : 0,
                        W3Rank = a.CompetitionWODs.Count > 2 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 2).Rank : 0,
                        W4Score = a.CompetitionWODs.Count > 3 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 3).Score : 0,
                        W4Rank = a.CompetitionWODs.Count > 3 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 3).Rank : 0,
                        W5Score = a.CompetitionWODs.Count > 4 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 4).Score : 0,
                        W5Rank = a.CompetitionWODs.Count > 4 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 4).Rank : 0,
                        W6Score = a.CompetitionWODs.Count > 5 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 5).Score : 0,
                        W6Rank = a.CompetitionWODs.Count > 5 ? a.CompetitionWODs.FirstOrDefault(w => w.Order == 5).Rank : 0
                    }
                ).ToArray();
        }

        protected string DisplayFlexIcon(object c)
        {
            string flexId = DataBinder.Eval(c, "DataItem.FlexId").ToString();
            string html = string.Empty;
            if (!string.IsNullOrWhiteSpace( flexId) )
            {
                html = "<a href=\"/" + flexId + "\"><img src=\"/DesktopModules/ATI_Base/resources/images/iFlex16.png\" /></a>";
            }
            return html;

        }  



        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
        {
            this.UpdateToolTip(args.Value, args.UpdatePanel);
        }
        private void UpdateToolTip(string elementID, UpdatePanel panel)
        {
            

            Control ctrl = Page.LoadControl("~/DesktopModules/ATI_Base/controls/ATI_CompetitionAthlete.ascx");
            panel.ContentTemplateContainer.Controls.Add(ctrl);
            
       //     ASP.desktopmodules_ati_base_controls_ati_c_ascx
            ASP.desktopmodules_ati_base_controls_ati_competitionathlete_ascx details = (ASP.desktopmodules_ati_base_controls_ati_competitionathlete_ascx)ctrl;
           // CompetitionAthleteC
            //ASP.CompetitionAthlet details = (ProductDetailsCS)ctrl;
        //    details.ProductID = elementID;
            long aid= Convert.ToInt64(elementID);
            details.CompetitionAthleteId = aid;
            panel.ContentTemplateContainer.Controls.Add(details);
        }
        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {                
                Control target = e.Item.FindControl("targetControl");
                if (!Object.Equals(target, null))
                {                    
                    if (!Object.Equals(this.RadToolTipManager1, null))
                    {
                        //Add the button (target) id to the tooltip manager
                        this.RadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("Id").ToString(), true);
                    }
                }
                Label lbl = e.Item.FindControl("numberLabel") as Label;
                if (!Object.Equals(lbl, null))
                {
                    lbl.Text = Convert.ToString( (RadGrid1.MasterTableView.CurrentPageIndex * RadGrid1.MasterTableView.PageSize) + (e.Item.ItemIndex + 1) );
                }
            }
        }
        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
            {
                RadToolTipManager1.TargetControls.Clear();
            }
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
     

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                aqufitEntities entities = new aqufitEntities();
                switch (hiddenAjaxAction.Value)
                {                   
                    case "delComment":
                        long cid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteComment(UserSettings, cid);
                        break;
                }
            }
            catch (Exception ex)
            {
                // TODO: better error handling
                RadAjaxManager1.ResponseScripts.Add( " alert('" + ex.Message + "');");
            }
        }


        protected void atiRadComboBoxSearchAffiliate_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboSearchWorkouts = (RadComboBox)sender;
            atiRadComboSearchAffiliates.Items.Clear();
            const int TAKE = 10;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<CompetitionAffiliate> affiliates = null;
            affiliates = entities.CompetitionAffiliates.OrderBy( a => a.Name );
            int length = affiliates.Count();
            affiliates = string.IsNullOrEmpty(e.Text) ? affiliates.Skip(itemOffset).Take(TAKE) : affiliates.Where(w => w.Name.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            CompetitionAffiliate[] afArray = affiliates.ToArray();

            foreach (CompetitionAffiliate a in afArray)
            {
                RadComboBoxItem item = new RadComboBoxItem(a.Name);
                item.Value = "" + a.Id;
                atiRadComboSearchWorkouts.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
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

