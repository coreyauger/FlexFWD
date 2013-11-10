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

namespace Affine.Dnn.Modules.ATI_CompLiveScore
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
    partial class ViewATI_CompLiveScore : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members
        private string baseUrl = "";
        private long CompetitionKey = 0;
        private CompetitionWOD _compWod = null;
        private long _compType = 1;
        private char _compSex = 'M';
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

                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    baseUrl = ResolveUrl("~/");
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    if (Request["a"] != null)
                    {
                        ddlCompSelector.Visible = false;
                        scrollPanel.Attributes["class"] = "scrollPanel";
                    }
                }

                if (Settings["ModuleMode"] != null && string.Compare(Convert.ToString( Settings["ModuleMode"]), "admin", true) == 0)
                {
                    
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
                        }
                    }
                    if (!isAdmin)
                    {
                        Response.Redirect(ResolveUrl("~/") + UserSettings.UserName);
                    }
                    if (Request["t"] != null)
                    {
                        _compType = Convert.ToInt64(Request["t"]);
                    }
                    if (Request["s"] != null)
                    {
                        _compSex = Convert.ToChar(Request["s"]);
                    }                    

                    if (!Page.IsPostBack && !Page.IsCallback)
                    {
                        ddlCompetitionType.DataSource = entities.CompetitionCategories.Select(c => new { Name = c.Category, Value = c.Id }).ToArray();
                        ddlCompetitionType.DataValueField = "Value";
                        ddlCompetitionType.DataTextField = "Name";
                        ddlCompetitionType.DataBind();
                        ddlCompetitionType.Items.FindByValue("" + _compType).Selected = true;

                        if (_compType == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
                        {
                            ddlTeamPool.Visible = true;
                            ddlTeamPool.DataSource = entities.CompetitionTeamPools.Where(tp => tp.Competition.Id == CompetitionKey).Select(tp => new { Name = tp.Name, Value = tp.Id }).ToArray();
                            ddlTeamPool.DataValueField = "Value";
                            ddlTeamPool.DataTextField = "Name";
                            ddlTeamPool.DataBind();
                            if (Request["tp"] != null)
                            {
                                ddlTeamPool.Items.FindByValue(Request["tp"]).Selected = true;
                            }
                        }
                        else
                        {
                            ddlTeamPool.Visible = false;
                        }

                        panelAdmin.Visible = true;
                        panelLiveScore.Visible = false;
                        // ok so at this point we know that they are an admin for the comp... 
                        // now lets find what workout they are scoring
                        IQueryable<CompetitionWOD> compWodQuery = entities.CompetitionWODs.OrderBy(w => w.Order).Where(w => w.Competition.Id == CompetitionKey && w.CompetitionCategory.Id == _compType);
                        if (_compType == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
                        {
                            long teamPoolKey = Convert.ToInt64( ddlTeamPool.SelectedValue );
                            compWodQuery = compWodQuery.Where(w => w.CompetitionTeamPool.Id == teamPoolKey);
                        }
                        CompetitionWOD compWod = compWodQuery.Where(c => c.CompetitionWODResults.Count() > 0).Select( w => w).ToArray().Reverse().FirstOrDefault();
                        if (compWod == null)
                        {
                            compWod = compWodQuery.FirstOrDefault();
                        }
                        if (Request["w"] != null)
                        {
                            long cwId = Convert.ToInt64(Request["w"]);
                            if (cwId == -1)
                            {   // this is now a final scoring..
                                bAdvance.Visible = false;
                                litWodName.Text = "Final Score<span style=\"padding-right: 150px;\">&nbsp;</span>";
                                hiddenCompWodKey.Value = "-1";
                                return;
                            }
                            else if (cwId == -2)
                            {   // allow this to fall through and setup first wod..
                                bBack.Visible = false;
                                cwId = compWod.Id;
                            }
                            compWod = entities.CompetitionWODs.FirstOrDefault(w => w.Id == cwId);
                        }

                        if (compWod == null)
                        {   // TODO: send a message that they need to setup some workouts..
                            
                        }
                        hiddenCompWodKey.Value = "" + compWod.Id;
                    }
                    long comWodKey = Convert.ToInt64(hiddenCompWodKey.Value);
                    _compWod = entities.CompetitionWODs.Include("WOD").Include("WOD.WODType").FirstOrDefault(w => w.Competition.Id == CompetitionKey && w.Id == comWodKey );
                    if (_compWod != null)
                    {
                        litWodName.Text = "" + _compWod.WOD.Name;
                    }
                }
                else
                {   // Live Score board...
                    if (!Page.IsPostBack && !Page.IsCallback)
                    {
                        if (Request["c"] != null)
                        {
                            CompetitionKey = Convert.ToInt64(Request["c"]);
                        }
                        hiddenAjaxCompCategoryKey.Value = "" + (int)Affine.Utils.WorkoutUtil.CompetitionCategory.IND_MALE;
                        hiddenTeamPoolKey.Value = "0";

                        CompetitionTeamPool[] teamPools = entities.CompetitionTeamPools.Where(p => p.Competition.Id == CompetitionKey).ToArray();
                        ddlCompSelector.Items.Add(new ListItem()
                        {
                            Text = "Individual - Mens",
                            Value = "{cat:1, tp:0}"
                        });
                        ddlCompSelector.Items.Add(new ListItem()
                        {
                            Text = "Individual - Womens",
                            Value = "{cat:3, tp:0}"
                        });
                        int ind = 0;
                        foreach (CompetitionTeamPool team in teamPools)
                        {
                            ddlCompSelector.Items.Add(new ListItem()
                            {
                                Text = "Team - " + team.Name,
                                Value = "{cat:2, tp:" + (ind++) + "}"
                            });
                        }
                    }

                    
                }
                
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }         
        }

       

        protected void ddlCompetitionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/Community/CompetitionScoringAdmin") + "?c=" + CompetitionKey + "&t=" + ddlCompetitionType.SelectedValue, true);
        }

        protected void ddlTeamPool_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/Community/CompetitionScoringAdmin") + "?c=" + CompetitionKey + "&t=" + ddlCompetitionType.SelectedValue + "&tp="+ddlTeamPool.SelectedValue, true);
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                aqufitEntities entities = new aqufitEntities();
                switch (hiddenAjaxAction.Value)
                {
                    case "updateAthleteScore":
                        CompetitionWODResult res = null;
                        try
                        {
                            long compAthlete = Convert.ToInt64(hiddenAjaxValue.Value);
                            
                            long compWodKey = Convert.ToInt64(hiddenCompWodKey.Value);
                            CompetitionAthlete athlete = entities.CompetitionAthletes.FirstOrDefault(a => a.Id == compAthlete && a.Competition.Id == CompetitionKey );
                            CompetitionWOD compWod = entities.CompetitionWODs.Include("WOD").Include("WOD.WODType").FirstOrDefault(w => w.Id == compWodKey && w.Competition.Id == CompetitionKey && w.CompetitionCategory.Id == _compType);
                            // check if we have a score yet..
                            if (athlete != null && compWod != null)
                            {
                                res = entities.CompetitionWODResults.FirstOrDefault(r => r.CompetitionAthlete.Id == compAthlete && r.CompetitionWOD.Id == compWodKey);
                                double score = 0;
                                
                                if (compWod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED)
                                {
                                    string[] ts = hiddenAjaxValue2.Value.Split(':');
                                    double seconds = 0.0;
                                    if (ts.Length == 3)
                                    {
                                        seconds = Convert.ToDouble(ts[0]) * 60 * 60 + Convert.ToDouble(ts[1]) * 60 + Convert.ToDouble(ts[2]);
                                    }
                                    else if (ts.Length == 2)
                                    {
                                        seconds = Convert.ToDouble(ts[0]) * 60 + Convert.ToDouble(ts[1]);
                                    }
                                    else if (ts.Length == 1)
                                    {
                                        seconds = Convert.ToDouble(ts[0]);
                                    }
                                    else
                                    {
                                        seconds = Convert.ToDouble(hiddenAjaxValue2.Value);
                                    }
                                    score = Affine.Utils.UnitsUtil.unitsToSystemDefualt(seconds, UnitsUtil.MeasureUnit.UNIT_SEC); // TODO: switch between units..
                                }
                                else if (compWod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT)
                                {
                                    score = Convert.ToDouble(hiddenAjaxValue2.Value);
                                    score = Affine.Utils.UnitsUtil.unitsToSystemDefualt(score, UnitsUtil.MeasureUnit.UNIT_LBS); // TODO: switch between units..
                                }
                                else
                                {
                                    score = Convert.ToDouble(hiddenAjaxValue2.Value);
                                }
                                if (res == null)
                                {
                                    res = new CompetitionWODResult()
                                    {
                                        CompetitionWOD = compWod,
                                        CompetitionAthlete = athlete,
                                        WOD = compWod.WOD
                                    };
                                }                                
                                res.Score = score;
                                entities.SaveChanges();
                            }
                        }
                        catch (Exception) {
                            if (res != null)
                            {
                                entities.DeleteObject(res);
                                entities.SaveChanges();
                            }
                        }
                        break;                        
                }                 
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('ERROR: There was a problem with the action (" + ex.StackTrace.Replace("'","").Replace("\r","").Replace("\n","") + ")');");
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                /*
                Control target = e.Item.FindControl("targetControl");
                if (!Object.Equals(target, null))
                {
                    if (!Object.Equals(this.RadToolTipManager1, null))
                    {
                        //Add the button (target) id to the tooltip manager
                        this.RadToolTipManager1.TargetControls.Add(target.ClientID, (e.Item as GridDataItem).GetDataKeyValue("Id").ToString(), true);
                    }
                }
                 */
                if (_compWod != null && _compWod.Id > 0)
                {   
                    GridDataItem item = (GridDataItem)e.Item;
                    double score = Convert.ToDouble(item["WodScore"].Text);
                    DesktopModules_ATI_Base_controls_ATI_UnitControl atiMaxWeightUnits = e.Item.FindControl("atiMaxWeightUnits") as DesktopModules_ATI_Base_controls_ATI_UnitControl;
                    RegularExpressionValidator revReal = e.Item.FindControl("revReal") as RegularExpressionValidator;
                    RegularExpressionValidator revTime = e.Item.FindControl("revTime") as RegularExpressionValidator;
                    TextBox atiTxtScore = e.Item.FindControl("atiTxtScore") as TextBox;
                    Literal litDebug = e.Item.FindControl("litDebug") as Literal;
                    if ( _compWod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED)
                    {
                        atiTxtScore.Visible = true;                                               
                        atiMaxWeightUnits.Visible = false;
                        if (!Object.Equals(revReal, null))
                        {
                            revReal.Enabled = false;
                            revReal.Visible = false;
                        }
                        if (!Object.Equals(revTime, null))
                        {
                            revTime.Enabled = true;
                            revTime.Visible = true;
                        }
                        if (score >= 0.0)
                        {
                            atiTxtScore.Text = Affine.Utils.UnitsUtil.durationToTimeString((long)score);
                        }
                        else
                        {
                            atiTxtScore.CssClass = atiTxtScore.CssClass + " dull";
                            atiTxtScore.Text = "hh:mm:ss";
                        }
                        
                    }
                    else if (!Object.Equals(atiMaxWeightUnits, null) && _compWod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT)
                    {
                        atiTxtScore.Visible = true;
                        atiMaxWeightUnits.Visible = true;
                        atiMaxWeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
              //          atiMaxWeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                        atiMaxWeightUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                        if (!Object.Equals(revReal, null))
                        {
                            revReal.Enabled = true;
                            revReal.Visible = true;
                        }
                        if (!Object.Equals(revTime, null))
                        {
                            revTime.Enabled = false;
                            revTime.Visible = false;
                        }
                        if (score >= 0)
                        {
                            atiTxtScore.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits( score, UnitsUtil.MeasureUnit.UNIT_LBS);
                        }
                    }
                    else if (!Object.Equals(atiTxtScore, null) && (_compWod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.AMRAP || _compWod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.SCORE) )
                    {
                        atiTxtScore.Visible = true;
                        atiMaxWeightUnits.Visible = false;
                        if (!Object.Equals(revReal, null))
                        {
                            revReal.Enabled = true;
                            revReal.Visible = true;
                        }
                        if (!Object.Equals(revTime, null))
                        {
                            revTime.Enabled = false;
                            revTime.Visible = false;
                        }
                        if (score >= 0)
                        {
                            atiTxtScore.Text = "" + score;
                        }

                    }
                }
            }
        }
    
        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            if (_compWod == null)
            {
                _compWod = new CompetitionWOD()
                {
                    Id = -1
                };
            }
            if (_compWod != null)
            {
                if (_compType == 2  && ddlTeamPool.Visible)
                {   // team event..
                    long teamPoolKey = Convert.ToInt64(ddlTeamPool.SelectedValue);
                    IQueryable<CompetitionRegistration2TeamPool> compPoolQuery = entities.CompetitionRegistrations.Where(r => r.CompetitionRegistration2TeamPool.FirstOrDefault(tp => tp.CompetitionTeamPool.Id == teamPoolKey) != null).Select(r => r.CompetitionRegistration2TeamPool.FirstOrDefault(tp => tp.CompetitionTeamPool.Id == teamPoolKey));
                   // compAthleteQuery = compAthleteQuery.Where(a => a.UserSetting != null);  // select captins to eliminate team duplicates..
                    compPoolQuery = compPoolQuery.OrderBy(a => a.OverallRank);
                    RadGrid1.DataSource = compPoolQuery.Select(a =>
                            new
                            {
                                Id = a.CompetitionRegistration.CompetitionAthletes.FirstOrDefault().Id,
                                AthleteName = a.CompetitionRegistration.TeamName,
                                Sex = "",
                                Rank = a.OverallRank,
                                Score = a.OverallScore,
                                WodScore = (a.CompetitionRegistration.CompetitionAthletes.FirstOrDefault().CompetitionWODResults.FirstOrDefault(w => w.CompetitionWOD.Id == _compWod.Id) != null ? a.CompetitionRegistration.CompetitionAthletes.FirstOrDefault().CompetitionWODResults.FirstOrDefault(w => w.CompetitionWOD.Id == _compWod.Id).Score : -1)
                            }
                        ).ToArray();
                }
                else
                {
                    IQueryable<CompetitionAthlete> compAthleteQuery = entities.CompetitionAthletes.Where(a => a.CompetitionRegistration.CompetitionCategory.Id == _compType && a.Competition.Id == CompetitionKey ).OrderBy(a => a.OverallRank);
                    compAthleteQuery = compAthleteQuery.OrderBy(a => a.OverallRank);
                    RadGrid1.DataSource = compAthleteQuery.Select(a =>
                            new
                            {
                                Id = a.Id,
                                AthleteName = a.AthleteName,
                                Sex = a.Sex,
                                Rank = a.OverallRank,
                                Score = a.OverallScore,
                                WodScore = (a.CompetitionWODResults.FirstOrDefault(w => w.CompetitionWOD.Id == _compWod.Id) != null ? a.CompetitionWODResults.FirstOrDefault(w => w.CompetitionWOD.Id == _compWod.Id).Score : -1)
                            }
                        ).ToArray();
                }
            }
        }

        protected void bBack_Click(object sender, EventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable<CompetitionWOD> compWodsQuery = entities.CompetitionWODs.Include("WOD.WODType").OrderBy(w => w.Order).Where(w => w.Competition.Id == CompetitionKey && w.CompetitionCategory.Id == _compType);
            CompetitionWOD back = null;
            long teamPoolKey = 0;
            if (Request["tp"] != null)
            {
                teamPoolKey = Convert.ToInt64(Request["tp"]);
                compWodsQuery = compWodsQuery.Where( w => w.CompetitionTeamPool.Id == teamPoolKey);
            }
            CompetitionWOD[] compWods = compWodsQuery.ToArray();

            if (_compWod == null)
            {
                back = compWods[compWods.Count() - 1];
            }
            else
            {
                foreach (CompetitionWOD cw in compWods)
                {
                    if (cw.Id == _compWod.Id)
                    {
                        break;
                    }
                    back = cw;
                }
            }
            if (back != null)
            {
                Response.Redirect(ResolveUrl("~/Community/CompetitionScoringAdmin") + "?c=" + CompetitionKey + "&w=" + back.Id + "&t=" + _compType + "&tp=" + teamPoolKey, true);
            }
            else
            {
                Response.Redirect(ResolveUrl("~/Community/CompetitionScoringAdmin") + "?c=" + CompetitionKey + "&w=-2" + "&t=" + _compType + "&tp=" + teamPoolKey, true);
            }
            

        }

        protected void bAdvance_Click(object sender, EventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            IQueryable< CompetitionWOD > compWodQuery = entities.CompetitionWODs.Include("WOD.WODType").OrderBy(w => w.Order).Where(w => w.Competition.Id == CompetitionKey && w.CompetitionCategory.Id == _compType);
            IQueryable<CompetitionAthlete> athleteQuery = entities.CompetitionAthletes.Where(a => a.Competition.Id == CompetitionKey && a.CompetitionRegistration.CompetitionCategory.Id == _compType);
            long teamPool = 0;
            if (_compType == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
            {
                
                teamPool = Convert.ToInt64(ddlTeamPool.SelectedValue);
                // TODO: here is this super lame "captin" check again with a.UserSetting != null
                athleteQuery = athleteQuery.Where(a => a.UserSetting != null && a.CompetitionRegistration.CompetitionRegistration2TeamPool.FirstOrDefault(tp => tp.CompetitionTeamPool.Id == teamPool) != null );
                compWodQuery = compWodQuery.Where(w => w.CompetitionTeamPool.Id == teamPool);
            }
            CompetitionWOD[] compWods = compWodQuery.ToArray();
            CompetitionAthlete[] athletes = athleteQuery.ToArray();
            foreach (CompetitionAthlete athlete in athletes)
            {
                athlete.OverallScore = 0;
                athlete.OverallRank = 0; 
            }
            entities.SaveChanges();
            CompetitionWOD next = null;
            double lastScore = 0;
            for (int i = 0; i < compWods.Length; i++ )
            {
                CompetitionWOD cwod = compWods[i];
                IQueryable<CompetitionWODResult> resultQuery = entities.CompetitionWODResults.Include("CompetitionAthlete").Where(w => w.CompetitionWOD.Id == cwod.Id);
                if (cwod.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED)
                {
                    resultQuery = resultQuery.OrderBy(w => w.Score);
                }
                else
                {
                    resultQuery = resultQuery.OrderByDescending(w => w.Score);
                }
                int score = 1;
                int inc = 1;
                foreach (CompetitionWODResult res in resultQuery)
                {
                    if (res.Score != lastScore)
                    {
                        score = inc;
                        res.Rank = score;
                        res.CompetitionAthlete.OverallScore += score;                        
                    }
                    else
                    {
                        res.Rank = score;
                        res.CompetitionAthlete.OverallScore += score;
                    }                                   
                    lastScore = res.Score;
                    inc++;
                }

                
                // TODO: will need to add the POINTS to this as well
                CompetitionAthlete[] dnf = athletes.Except(resultQuery.Select(w => w.CompetitionAthlete).ToArray()).ToArray();
                foreach (CompetitionAthlete athlete in dnf)
                {
                    athlete.OverallScore += score;
                }
                if (cwod.Id == _compWod.Id)
                {
                    if ((i + 1) < compWods.Length)
                    {
                        next = compWods[i + 1];
                    }
                    break;
                }
                
            }
            entities.SaveChanges();
            IQueryable<CompetitionAthlete> athleteRankerQuery = entities.CompetitionAthletes.Where(a => a.Competition.Id == CompetitionKey && a.CompetitionRegistration.CompetitionCategory.Id == _compType).OrderBy(a => a.OverallScore);
            double lastRank = 0;
            int rank = 1;
            int count = 1;
            IDictionary<long, CompetitionRegistration2TeamPool> hash = new Dictionary<long, CompetitionRegistration2TeamPool>();
            if (_compType == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
            {
                // TODO: this a.UserSetting != null (sux) .. it is the way of determining the captin on the team.. and thus a single team element.
                athleteRankerQuery = athleteRankerQuery.Where(a => a.CompetitionRegistration.CompetitionRegistration2TeamPool.FirstOrDefault(tp => tp.CompetitionTeamPool.Id == teamPool) != null && a.UserSetting != null );
                CompetitionRegistration2TeamPool[] teamArray = entities.CompetitionRegistration2TeamPool.Include("CompetitionRegistration.CompetitionAthletes").Where(r2t => r2t.CompetitionTeamPool.Id == teamPool).ToArray();                
                foreach (CompetitionRegistration2TeamPool team in teamArray)
                {
                    hash.Add(new KeyValuePair<long, CompetitionRegistration2TeamPool>(team.CompetitionRegistration.CompetitionAthletes.First().Id, team));
                }
            }
            CompetitionAthlete[] athleteRanker = athleteRankerQuery.ToArray();
            foreach (CompetitionAthlete athlete in athleteRanker)
            {
                if (lastRank != athlete.OverallScore)
                {
                    rank = count;
                    athlete.OverallRank = rank++;                    
                }
                else
                {
                    athlete.OverallRank = rank - 1;
                }
                lastRank = athlete.OverallScore;
                count++;
                if (hash.ContainsKey(athlete.Id))
                {
                    hash[athlete.Id].OverallRank = athlete.OverallRank;
                    hash[athlete.Id].OverallScore = athlete.OverallScore;
                }
                
            }
            entities.SaveChanges();                               
            if (next != null)
            {
                Response.Redirect(ResolveUrl("~/Community/CompetitionScoringAdmin") + "?c=" + CompetitionKey + "&w=" + next.Id + "&t=" + _compType + "&tp=" + teamPool, true);
            }
            else
            {
                Response.Redirect(ResolveUrl("~/Community/CompetitionScoringAdmin") + "?c=" + CompetitionKey + "&w=-1" + "&t=" + _compType + "&tp="+teamPool, true);
            }
        }


        protected class AthleteScore
        {
            public string FlexId { get; set; }
            public string AffiliateName{get; set;}
            public string AthleteName{ get; set; }
            public long UserSettingKey{ get; set; }
            public double? Height { get;set;}
            public float Age{ get; set ;}
            public long Id { get; set ;}
            public string ImgUrl{get;set;}
            public int OverallRank { get; set;}
            public double OverallScore{ get; set;}
            public string Sex{ get; set; }
            public double? Weight { get; set; }
            public string WodRankScore{ get; set; }            
        }

        protected void RadGridLiveScore_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
            long compCategoryKey = Convert.ToInt64(hiddenAjaxCompCategoryKey.Value);
            CompetitionTeamPool[] poolArray = entities.CompetitionTeamPools.Where(t => t.Competition.Id == compKey).OrderBy(t => t.Id).ToArray();
            if (poolArray.Length <= 0 && compCategoryKey == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
            {
                compCategoryKey = (int)Affine.Utils.WorkoutUtil.CompetitionCategory.IND_MALE;
            }
            if (compCategoryKey == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.TEAM)
            {          
                long poolInd = Convert.ToInt64(hiddenTeamPoolKey.Value);               
                CompetitionTeamPool pool = poolArray[poolInd];
                IQueryable<CompetitionRegistration2TeamPool> teamArray = entities.CompetitionRegistration2TeamPool.Include("CompetitionRegistration.CompetitionAthletes.CompetitionWODResults").Include("CompetitionRegistration.CompetitionAthletes").Where(r2t => r2t.CompetitionTeamPool.Id == pool.Id && r2t.CompetitionRegistration != null).OrderBy( r2t => r2t.OverallRank );
           
                AthleteScore[] aScoreArray = teamArray.Select(a =>
                        new AthleteScore()
                        {
                            FlexId = (a.CompetitionRegistration.UserSetting != null ? a.CompetitionRegistration.UserSetting.UserName : ""),
                            AffiliateName = a.CompetitionRegistration.CompetitionAthletes.FirstOrDefault().AffiliateName,
                            AthleteName = a.CompetitionRegistration.TeamName,
                            UserSettingKey = a.CompetitionRegistration.GroupAffiliate.Id,
                            Height = 0,
                            Age = 0,
                            Id = a.Id,
                            ImgUrl = " ",
                            OverallRank = a.OverallRank,
                            OverallScore = a.OverallScore,
                            Sex = " ",
                            Weight = 0
                        }).ToArray();
                int i = 0;
                string aa = "";
                foreach (CompetitionRegistration2TeamPool ct in teamArray)
                {
                    string scoringStr = string.Empty;
                    // Note: At this point this is a fucking mess...
                    long aid = ct.CompetitionRegistration.CompetitionAthletes.FirstOrDefault().Id;
                    aa += " a:"+aid + ", ";
                    CompetitionWODResult[] resArray = entities.CompetitionWODResults.Where(r => r.CompetitionWOD.CompetitionTeamPool.Id == pool.Id && r.CompetitionAthlete.Id == aid).OrderBy( w => w.CompetitionWOD.Order).ToArray();
                    aa += " s:" + resArray.Length + ", ";
                    aa += " p:" + pool.Id + ", ";
                    //foreach (CompetitionWODResult wr in ct.CompetitionRegistration.CompetitionAthletes.FirstOrDefault().CompetitionWODResults.Where( c => c.CompetitionWOD != null && c.CompetitionWOD.CompetitionTeamPool.Id == poolKey ).OrderBy(w => w.Order))
                    foreach( CompetitionWODResult wr in resArray )
                    {
                        scoringStr += "<span>(" + wr.Rank + ")</span>";
                    }
                    aScoreArray[i++].WodRankScore = scoringStr;
                }
               // RadAjaxManager1.ResponseScripts.Add("alert('aa: "+aa+"');");
                RadGridLiveScore.DataSource = aScoreArray;
                
                poolInd++;
                if (poolInd >= poolArray.Length)
                {   // this will go back to disply the men...
                    RadAjaxManager1.ResponseScripts.Add(" $('#compCategoryName').html('-- Team Pool: " + pool.Name + " --'); $('#" + hiddenAjaxCompCategoryKey.ClientID + "').val(1); $('#" + hiddenTeamPoolKey.ClientID + "').val(0);");
                }
                else
                {   // next team pool...'
                    RadAjaxManager1.ResponseScripts.Add(" $('#compCategoryName').html('-- Team Pool: "+pool.Name+" --'); $('#" + hiddenAjaxCompCategoryKey.ClientID + "').val(2); $('#" + hiddenTeamPoolKey.ClientID + "').val(" + poolInd + ");");
                }
            //    RadAjaxManager1.ResponseScripts.Add("alert('poolkey2: '+$('#" + hiddenAjaxTeamPoolKey.ClientID + "').val());");
            }
            else
            {
                IQueryable<CompetitionAthlete> athleteQuery = entities.CompetitionAthletes.Include("CompetitionWODResults").Where(a => a.Competition.Id == comp.Id);
                athleteQuery = athleteQuery.Where(a => a.CompetitionRegistration.CompetitionCategory.Id == compCategoryKey).OrderBy(a => a.OverallRank);
                IQueryable<CompetitionWOD> compWods = entities.CompetitionWODs.Where(w => w.Competition.Id == compKey && w.CompetitionCategory.Id == compCategoryKey).OrderBy(w => w.Order);

                AthleteScore[] aScoreArray = athleteQuery.Select(a =>
                        new AthleteScore()
                        {
                            FlexId = (a.UserSetting != null ? a.UserSetting.UserName : ""),
                            AffiliateName = a.AffiliateName,
                            AthleteName = a.AthleteName,
                            UserSettingKey = a.UserSetting.Id,
                            Height = a.Height,
                            Age = (a.BirthYear.HasValue ? (DateTime.Today.Year - a.BirthYear.Value) : 0),
                            Id = a.Id,
                            ImgUrl = a.ImgUrl,
                            OverallRank = a.OverallRank,
                            OverallScore = a.OverallScore,
                            Sex = a.Sex,
                            Weight = a.Weight
                        }).ToArray();
                int i = 0;
                foreach (AthleteScore ca in aScoreArray)
                {
                    string scoringStr = string.Empty;
                    CompetitionWODResult[] wodResultArray = entities.CompetitionWODResults.Where(r => r.CompetitionAthlete.Id == ca.Id).OrderBy(r => r.CompetitionWOD.Order).ToArray();
                   // foreach (CompetitionWODResult wr in ca.CompetitionWODResults.OrderBy(w => w.Order))
                    foreach (CompetitionWODResult wr in wodResultArray)
                    {
                        scoringStr += "<span>(" + wr.Rank + ")</span>";
                    }
                    ca.WodRankScore = scoringStr;
                }
                RadGridLiveScore.DataSource = aScoreArray;
                if (compCategoryKey == (int)Affine.Utils.WorkoutUtil.CompetitionCategory.IND_MALE)
                {   // display the females
                    RadAjaxManager1.ResponseScripts.Add(" $('#compCategoryName').html('-- Mens Individuals --'); $('#" + hiddenAjaxCompCategoryKey.ClientID + "').val(3); $('#" + hiddenAjaxTeamPoolKey.ClientID + "').val(0);");
                }
                else
                {   // display the teams..
                    RadAjaxManager1.ResponseScripts.Add(" $('#compCategoryName').html('-- Womens Individuals --'); $('#" + hiddenAjaxCompCategoryKey.ClientID + "').val(2); $('#" + hiddenAjaxTeamPoolKey.ClientID + "').val(0);");
                }
            }
        }

        protected string DisplayFlexIcon(object c)
        {
            string flexId = DataBinder.Eval(c, "DataItem.FlexId").ToString();
            string html = string.Empty;
            if (!string.IsNullOrWhiteSpace(flexId))
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
            long aid = Convert.ToInt64(elementID);
            details.CompetitionAthleteId = aid;
            panel.ContentTemplateContainer.Controls.Add(details);
        }
        protected void RadGridLiveScore_ItemDataBound(object sender, GridItemEventArgs e)
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
                    lbl.Text = Convert.ToString((RadGrid1.MasterTableView.CurrentPageIndex * RadGrid1.MasterTableView.PageSize) + (e.Item.ItemIndex + 1));
                }
            }
        }
        protected void RadGridLiveScore_ItemCommand(object source, GridCommandEventArgs e)
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
            RadGrid1.ExportSettings.IgnorePaging = true;
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

