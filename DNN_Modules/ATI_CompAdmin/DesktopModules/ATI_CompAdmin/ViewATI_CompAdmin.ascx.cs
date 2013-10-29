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

namespace Affine.Dnn.Modules.ATI_CompAdmin
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
    partial class ViewATI_CompAdmin : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members
        private string baseUrl = "";
        private long CompetitionKey = 0;
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
                aqufitEntities entities = new aqufitEntities();
                bool isAdmin = false;
                if (Request["c"] != null)
                {
                    // TODO: make sure they are a group admin to the comp..
                    CompetitionKey = Convert.ToInt64(Request["c"]);
                    Competition comp = entities.Competitions.Include("Group").FirstOrDefault(c => c.Id == CompetitionKey);
                    if (comp != null)
                    {
                        UserFriends admin = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == comp.Group.Id && f.Relationship <= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN);
                        if (admin != null)
                        {
                            isAdmin = true;
                        }
                        litCompOwner.Text = comp.Group.UserFirstName;
                        txtCompName.Text = comp.Name;
                        RadCompStartDate.SelectedDate = comp.CompetitionStartDate;
                        RadCompEndDate.SelectedDate = comp.CompetitionEndDate;
                    }
                }
                
                if( !isAdmin )
                {
                    Response.Redirect(ResolveUrl("~/") + UserSettings.UserName);
                }
                RadListBoxTeamSource.DataSource = entities.CompetitionRegistrations.Where(r => r.Competition.Id == CompetitionKey && r.CompetitionCategory.Id == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM).Select(r => new { Name = r.TeamName, Value = r.Id }).ToArray();
                RadListBoxTeamSource.DataTextField = "Name";
                RadListBoxTeamSource.DataValueField = "Value";
                RadListBoxTeamSource.DataBind();

                

                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    ddlTeamPools.DataSource = entities.CompetitionTeamPools.Where(tp => tp.Competition.Id == CompetitionKey).Select(tp => new { Name = tp.Name, Value = tp.Id }).ToArray();
                    ddlTeamPools.DataTextField = "Name";
                    ddlTeamPools.DataValueField = "Value";
                    ddlTeamPools.DataBind();

                    if (ddlTeamPools.Items.Count == 0)
                    {
                        buttonAddPoolWorkout.Visible = false;
                        RadGrid2.Visible = false;
                    }
                                      
                    baseUrl = ResolveUrl("~/");                    
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);

                    
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
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                aqufitEntities entities = new aqufitEntities();
                switch (hiddenAjaxAction.Value)
                {
                    case "AddWOD":
                        long wodKey = Convert.ToInt64(hiddenAjaxValue.Value);
                        int compType = Convert.ToInt32(hiddenAjaxValue2.Value);
                        long poolKey = 0;
                        if (compType == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
                        {
                            poolKey = Convert.ToInt32(hiddenAjaxValue3.Value);
                        }
                        WOD wod = entities.WODs.Include("WODType").FirstOrDefault(w => w.Id == wodKey);
                        Competition comp = entities.Competitions.Include("CompetitionWODs").FirstOrDefault(c => c.Id == CompetitionKey);
                        if (wod != null && comp != null)
                        {
                            CompetitionWOD compWod = new CompetitionWOD()
                            {
                                WOD = wod,
                                Order = comp.CompetitionWODs.Count,
                                Competition = comp,
                                CompetitionCategory= entities.CompetitionCategories.FirstOrDefault( c => c.Id == compType ),
                                CompetitionTeamPool = entities.CompetitionTeamPools.FirstOrDefault( tp => tp.Id == poolKey )
                            };                            
                            entities.AddToCompetitionWODs(compWod);
                            entities.SaveChanges();
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.WorkoutFinder.close(); Aqufit.Page.Actions.refreshGrid("+compType+"); ");
                        }
                        break;    
                    case "DeleteWOD":
                        long compWodKey = Convert.ToInt64(hiddenAjaxValue.Value);
                        CompetitionWOD compW = entities.CompetitionWODs.Include("CompetitionCategory").FirstOrDefault(c => c.Id == compWodKey && c.Competition.Id == CompetitionKey);
                        if (compW != null)
                        {
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.Actions.refreshGrid(" + compW.CompetitionCategory.Id + "); ");
                            entities.DeleteObject(compW);
                            entities.SaveChanges();
                            // rebuild the order of the workouts now..
                            CompetitionWOD[] orderList = entities.CompetitionWODs.Where(c => c.Competition.Id == CompetitionKey).OrderBy( c => c.Order ).ToArray();
                            int order = 0;
                            foreach (CompetitionWOD cw in orderList)
                            {
                                cw.Order = (order++);
                            }
                            entities.SaveChanges();
                        }                        
                        break;
                    case "AddTeamPool":
                        string name = Convert.ToString(hiddenAjaxValue.Value);
                        CompetitionTeamPool pool = new CompetitionTeamPool()
                        {
                            Name = name,
                            Competition = entities.Competitions.FirstOrDefault( c => c.Id == CompetitionKey)
                        };
                        entities.AddToCompetitionTeamPools(pool);
                        entities.SaveChanges();
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.TeamPoolWin.close(); Aqufit.Page.Actions.refreshTeamPoolGrid(); ");
                        break;
                    case "LoadTeamPool":
                        long tpKey = Convert.ToInt64(hiddenAjaxValue.Value);
                        string[] teamKeyArray = entities.CompetitionRegistrations.Where(r => r.CompetitionRegistration2TeamPool
                                                                                 .FirstOrDefault(c => c.CompetitionTeamPool.Id == tpKey) != null)
                                                                                 .Select( r => r.Id ).Cast<string>().ToArray();
                        string jsTeamArray = "[]";
                        if (teamKeyArray.Length > 0)
                        {
                            jsTeamArray = "[" + teamKeyArray.Aggregate((i, j) => "" + i + "," + j) + "]";
                        }
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.TeamPoolManagerWin.loadData(" + jsTeamArray + "); ");
                        break;
                    case "SaveTeamPool":
                        long tPoolKey = Convert.ToInt64(hiddenAjaxValue.Value);
                        CompetitionRegistration2TeamPool[] clearArray = entities.CompetitionRegistration2TeamPool.Where(c => c.CompetitionTeamPool.Id == tPoolKey).ToArray();
                        foreach (CompetitionRegistration2TeamPool clear in clearArray)
                        {
                            entities.DeleteObject(clear);
                        }
                        entities.SaveChanges(); 
                        CompetitionTeamPool tpool = entities.CompetitionTeamPools.FirstOrDefault(tp => tp.Id == tPoolKey);
                        foreach (RadListBoxItem item in RadListBoxPoolDest.Items)
                        {
                            long rId = Convert.ToInt64(item.Value);
                            CompetitionRegistration2TeamPool c2t = new CompetitionRegistration2TeamPool()
                            {
                                CompetitionTeamPool = tpool,
                                CompetitionRegistration = entities.CompetitionRegistrations.FirstOrDefault( r => r.Id == rId )
                            };
                            entities.AddToCompetitionRegistration2TeamPool(c2t);
                        }
                        entities.SaveChanges();    
                        RadListBoxTeamSource.DataSource = entities.CompetitionRegistrations.Where(r => r.Competition.Id == CompetitionKey && r.CompetitionCategory.Id == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM).Select(r => new { Name = r.TeamName, Value = r.Id }).ToArray();
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.TeamPoolManagerWin.close(); Aqufit.Page.Actions.refreshTeamPoolGrid(); ");
                        break;
                }                 
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('ERROR: There was a problem with the action (" + ex.StackTrace.Replace("'","").Replace("\r","").Replace("\n","") + ")');");
            }
        }

        protected class CompPoolData
        {
            public long Id { get; set; }
            public string Pool { get; set; }
            public string Teams { get; set; }
        }

        protected void RadGridTeamPool_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
          
             aqufitEntities entities = new aqufitEntities();
             IQueryable<CompetitionTeamPool> compPoolQuery = entities.CompetitionTeamPools.Include("CompetitionRegistration2TeamPool").Where(c => c.Competition.Id == CompetitionKey);
             CompPoolData[] ctArray = compPoolQuery.Select(c =>
                     new CompPoolData()
                     {
                         Id = c.Id,
                         Pool = c.Name,
                         Teams = ""
                     }
                 ).ToArray();
             
            CompetitionTeamPool[] compoolArray = entities.CompetitionTeamPools.Include("CompetitionRegistration2TeamPool.CompetitionRegistration").Where(c => c.Competition.Id == CompetitionKey).ToArray();
            for( int i=0; i<ctArray.Length; i++){
                foreach( CompetitionRegistration2TeamPool reg2team in compoolArray[i].CompetitionRegistration2TeamPool)
                {
                    ctArray[i].Teams += reg2team.CompetitionRegistration.TeamName + ",";
                }
            }
         
            RadGridTeamPool.DataSource = ctArray;
            
        }

        

        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable< CompetitionWOD > compWodQuery = entities.CompetitionWODs.Include("WOD").Include("WOD.WODType").Where( c => c.Competition.Id == CompetitionKey && c.CompetitionCategory.Id == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.IND_MALE ).OrderBy( c => c.Order );
            RadGrid1.DataSource = compWodQuery.Select(c =>
                    new
                    {
                        Id = c.Id,
                        WorkoutName = c.WOD.Name,
                        WorkoutType = c.WOD.WODType.Name,
                        WorkoutOrder = (c.Order+1)
                    }
                ).ToArray();
        }

        protected void RadGrid3_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable<CompetitionWOD> compWodQuery = entities.CompetitionWODs.Include("WOD").Include("WOD.WODType").Where(c => c.Competition.Id == CompetitionKey && c.CompetitionCategory.Id == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.IND_FEMALE).OrderBy(c => c.Order);
            RadGrid3.DataSource = compWodQuery.Select(c =>
                    new
                    {
                        Id = c.Id,
                        WorkoutName = c.WOD.Name,
                        WorkoutType = c.WOD.WODType.Name,
                        WorkoutOrder = (c.Order + 1)
                    }
                ).ToArray();
        }

        protected void RadGrid2_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                long tpKey = Convert.ToInt64(ddlTeamPools.SelectedValue);

                aqufitEntities entities = new aqufitEntities();
                IQueryable<CompetitionWOD> compWodQuery = entities.CompetitionWODs.Include("WOD").Include("WOD.WODType").Where(c => c.Competition.Id == CompetitionKey && c.CompetitionCategory.Id == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM && c.CompetitionTeamPool.Id == tpKey).OrderBy(c => c.Order);
                RadGrid2.DataSource = compWodQuery.Select(c =>
                        new
                        {
                            Id = c.Id,
                            WorkoutName = c.WOD.Name,
                            WorkoutType = c.WOD.WODType.Name,
                            WorkoutOrder = (c.Order + 1)
                        }
                    ).ToArray();
            }
            catch { }
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

