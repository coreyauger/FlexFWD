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

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace Affine.Dnn.Modules.ATI_People
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
    partial class ViewATI_People : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
    {

        #region Private Members              
               
        private static readonly int PEOPLE_TAKE = 5;
        private IQueryable<UserSettings> userQuery = null;
        private IQueryable<UserSettings> userQueryRecentActive = null;
        private IQueryable<UserSettings> userQueryLiveNear = null;
        private IQueryable<UserSettings> userQueryMostWorkouts = null;
        private IQueryable<UserSettings> userQueryMostWatched = null;
        private IQueryable<Workout> workoutQueryFastest1 = null;
        private IQueryable<Workout> workoutQueryFastest2 = null;   
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
                imgAd.Src = ResolveUrl("~/images/iphoneAd.png");

                Affine.Utils.WorkoutUtil.WorkoutType workoutType = Affine.Utils.WorkoutUtil.WorkoutType.RUNNING;
                if (Request["wt"] != null)
                {
                    workoutType = Affine.Utils.WorkoutUtil.IntToWorkoutType(Convert.ToInt32(Request["wt"]));
                }
                atiWorkoutTypes.SelectedType = workoutType;
                aqufitEntities entities = new aqufitEntities();
                WorkoutType selectedType = entities.WorkoutType.First(w => w.Id == (int)workoutType);
                userQuery = entities.UserSettings.OfType<User>().Where(u => u.PortalKey == this.PortalId && !u.User2WorkoutType.Select( wt => wt.WorkoutType.Id ).Contains(selectedType.Id)).OrderBy(u => u.Id).AsQueryable();
                userQueryRecentActive = userQuery.OrderByDescending(w => w.LastLoginDate);
                if (this.UserSettings != null)
                {
                    userQueryLiveNear = userQuery.Where( u => u.LngHome.HasValue && u.LngHome.HasValue ).OrderBy(u => Math.Abs(u.LatHome.Value - UserSettings.LatHome.Value) + Math.Abs(u.LngHome.Value - UserSettings.LngHome.Value));
                }
                userQueryMostWatched = userQuery.Where(u => u.Metrics.FirstOrDefault(m => m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_FOLLOWERS) != null ).OrderByDescending(u => u.Metrics.FirstOrDefault(m => m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_FOLLOWERS).MetricValue);
                IQueryable<Workout> fastWorkoutQuery = entities.UserStreamSet.Include("UserSettings").OfType<Workout>().Where(w => w.WorkoutTypeKey.HasValue && w.WorkoutTypeKey.Value == (int)workoutType && w.Duration.HasValue && w.Duration.Value > 0 );
                if (workoutType == Utils.WorkoutUtil.WorkoutType.RUNNING)
                {
                    // metrics will be 1 mile, 5km and 10km
                    hFastTime1.InnerText = "Fastest 1 mile";
                    double mile = Affine.Utils.UnitsUtil.distanceMetricToDistance(Utils.UnitsUtil.DistanceMetric.MILE_1);
                    workoutQueryFastest1 = fastWorkoutQuery.Where(w => w.Distance >= mile).OrderBy(w => w.Duration);

                    hFastTime2.InnerText = "Fastest 5 km";
                    double km5 = Affine.Utils.UnitsUtil.distanceMetricToDistance(Utils.UnitsUtil.DistanceMetric.KM_5);
                    workoutQueryFastest2 = fastWorkoutQuery.Where(w => w.Distance >= km5).OrderBy(w => w.Duration);
                }
                else if (workoutType == Utils.WorkoutUtil.WorkoutType.ROW)
                {
                    // row metrics will be fastest 500 M and 1 Km
                    hFastTime1.InnerText = "Fastest 500 M";
                    double m500 = Affine.Utils.UnitsUtil.distanceMetricToDistance(Utils.UnitsUtil.DistanceMetric.M_500);
                    workoutQueryFastest1 = fastWorkoutQuery.Where(w => w.Distance >= m500).OrderBy(w => w.Duration);

                    hFastTime2.InnerText = "Fastest 1 km";
                    double km1 = Affine.Utils.UnitsUtil.distanceMetricToDistance(Utils.UnitsUtil.DistanceMetric.KM_1);
                    workoutQueryFastest2 = fastWorkoutQuery.Where(w => w.Distance >= km1).OrderBy(w => w.Duration);
                }
                else if (workoutType == Utils.WorkoutUtil.WorkoutType.CROSSFIT)
                {
                    hFastTime1.InnerText = "Fran";
                    workoutQueryFastest1 = fastWorkoutQuery.Where(w => w.WOD.Standard > 0 && w.WOD.Name == "Fran" && w.RxD.HasValue && w.RxD.Value).OrderBy(w => w.Duration);

                    hFastTime2.InnerText = "Helen";
                    workoutQueryFastest2 = fastWorkoutQuery.Where(w => w.WOD.Standard > 0 && w.WOD.Name == "Helen" && w.RxD.HasValue && w.RxD.Value).OrderBy(w => w.Duration);
                }
                
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    if (ProfileSettings != null)
                    {
                        liFindFriends.Visible = false;
                        pageViewFindFriends.Visible = false;
                        atiProfileImg.Settings = ProfileSettings;
                        atiProfileImg.IsOwner = base.Permissions == AqufitPermission.OWNER;
                        atiPeoplePanel.Visible = false;
                        atiPeopleViewer.Visible = true;
                        peopleTabTitle.Text = ProfileSettings.UserName + " Achievements";
                        litUserName.Text = "<a class=\"midBlue\" href=\"" + ResolveUrl("~") + ProfileSettings.UserName+ "\">" + ProfileSettings.UserName + "</a> <span> (" + ProfileSettings.UserFirstName + " " + ProfileSettings.UserLastName + ")</span>";
                        atiShareLink.ShareLink = Request.Url.AbsoluteUri;
                        atiShareLink.ShareTitle = ProfileSettings.UserName + " Achievements";
                       
                        if (ProfileSettings is Group)
                        {
                            Affine.Utils.ConstsUtil.Relationships memberType = Utils.ConstsUtil.Relationships.NONE;
                            if (UserSettings != null)
                            {
                                UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == ProfileSettings.Id);
                                if (uf != null)
                                {
                                    memberType = Utils.ConstsUtil.IntToRelationship(uf.Relationship);
                                }
                            }
                            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                            atiWorkoutTotals.Visible = false;                                                                                                                                           
                            nvgCrossfit.Visible = false;
                            liConfig.Visible = (memberType == Utils.ConstsUtil.Relationships.GROUP_ADMIN || memberType == Utils.ConstsUtil.Relationships.GROUP_OWNER);
                            if ((memberType == Utils.ConstsUtil.Relationships.GROUP_ADMIN || memberType == Utils.ConstsUtil.Relationships.GROUP_OWNER) && Request["c"] != null)
                            {
                                atiPeopleViewer.Visible = false;
                                atiLeaderBoardConfig.Visible = true;
                                bool hasCustom = entities.LeaderBoard2WOD.FirstOrDefault(lb => lb.UserSetting.Id == GroupSettings.Id) != null;
                                if (hasCustom)
                                {
                                    Affine.Data.json.LeaderBoardWOD[] all = dataMan.CalculatCrossFitLeaderBoard(base.GroupSettings.Id);
                                    System.Web.Script.Serialization.JavaScriptSerializer jsSerial = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    RadAjaxManager1.ResponseScripts.Add("$(function(){ Aqufit.Page." + atiLeaderBoard2Config.ID + ".loadLeaderBoardFromJson('" + jsSerial.Serialize(all) + "'); });");
                                }
                            }
                            else
                            {
                                Affine.Data.json.LeaderBoardWOD[] all = dataMan.CalculatCrossFitLeaderBoard(base.GroupSettings.Id);
                                System.Web.Script.Serialization.JavaScriptSerializer jsSerial = new System.Web.Script.Serialization.JavaScriptSerializer();
                                RadAjaxManager1.ResponseScripts.Add("$(function(){ Aqufit.Page." + atiLeaderBoard2.ID + ".loadLeaderBoardFromJson('" + jsSerial.Serialize(all) + "'); });");
                            }
                        }
                        else
                        {
                            atiWorkoutTotals.Cols = 3;
                            // get number of workouts for each workout type                            
                            WorkoutType[] wtypes = entities.WorkoutType.ToArray();
                            IQueryable<Workout> workoutQuery = entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == ProfileSettings.Id);
                            foreach (WorkoutType wt in wtypes)
                            {
                                atiWorkoutTotals.Totals.Add(new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem()
                                {
                                    Name = wt.Name,
                                    Icon = wt.Icon,
                                    Total = "" + workoutQuery.Where(w => w.WorkoutTypeKey == wt.Id).Count()
                                });
                            }
                            IQueryable<Workout> crossfitWorkouts = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == ProfileSettings.Id && w.IsBest == true);
                            int numDistinct = crossfitWorkouts.Select(w => w.WOD).Distinct().Count();
                            IQueryable<Workout> wodsToDisplay = null;
                            wodsToDisplay = crossfitWorkouts.OrderByDescending(w => w.Id);
                            string baseUrl = ResolveUrl("~") + "workout/";
                            // We need to split up into WOD types now...
                            Workout[] timedWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED).OrderBy(w => w.Duration).ToArray();
                            IList<DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem> cfTotals = timedWods.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.Title, Total = Affine.Utils.UnitsUtil.durationToTimeString(Convert.ToInt64(w.Duration)), Link = baseUrl + w.WOD.Id }).ToList();
                            // Now all the scored ones...
                            Workout[] scoredWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.SCORE || w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.AMRAP ).ToArray();
                            cfTotals = cfTotals.Concat(scoredWods.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.Title, Total = Convert.ToString(w.Score), Link = baseUrl + w.WOD.Id }).ToList()).ToList();
                            Workout[] maxWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT).ToArray();
                            cfTotals = cfTotals.Concat(maxWods.Select(w => new DesktopModules_ATI_Base_controls_ATI_NameValueGrid.TotalItem() { Name = w.Title, Total = Affine.Utils.UnitsUtil.systemDefaultToUnits(w.Max.Value, WeightUnits) + " " + Affine.Utils.UnitsUtil.unitToStringName(WeightUnits), Link = baseUrl + w.WOD.Id }).ToList()).ToList();
                            // TODO: we should have workout names link to that workout (a you vs. them kinda deal)
                            nvgCrossfit.Totals = cfTotals.OrderBy(t => t.Name).ToArray();

                            #region Achievments

                            Affine.WebService.StreamService ss = new WebService.StreamService();
                            string js = string.Empty;

                            Affine.Utils.WorkoutUtil.WorkoutType running = Utils.WorkoutUtil.WorkoutType.RUNNING;
                            Achievement[] runningAchievements = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Include("AchievementType").Where(a => a.AchievementType.WorkoutType.Id == (int)running && a.UserSetting.Id == ProfileSettings.Id).OrderBy(a => a.AchievementType.DistanceRangeA).ToArray();
                            foreach (Achievement a in runningAchievements)
                            {
                                string json = ss.getStreamData((Workout)a.UserStream);
                                js += "Aqufit.Page.atiFeaturedRunning.addItem('" + json + "','" + a.AchievementType.Name + "'); ";
                            }
                            Affine.Utils.WorkoutUtil.WorkoutType rowing = Utils.WorkoutUtil.WorkoutType.ROW;
                            Achievement[] rowingAchievements = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Include("AchievementType").Where(a => a.AchievementType.WorkoutType.Id == (int)rowing && a.UserSetting.Id == ProfileSettings.Id).OrderBy(a => a.AchievementType.DistanceRangeA).ToArray();
                            foreach (Achievement a in rowingAchievements)
                            {
                                string json = ss.getStreamData((Workout)a.UserStream);
                                js += "Aqufit.Page.atiFeaturedRowing.addItem('" + json + "','" + a.AchievementType.Name + "'); ";

                            }
                            Affine.Utils.WorkoutUtil.WorkoutType cycling = Utils.WorkoutUtil.WorkoutType.CYCLING;
                            Achievement[] cyclingAchievements = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Include("AchievementType").Where(a => a.AchievementType.WorkoutType.Id == (int)cycling && a.UserSetting.Id == ProfileSettings.Id).OrderBy(a => a.AchievementType.DistanceRangeA).ToArray();
                            foreach (Achievement a in cyclingAchievements)
                            {
                                string json = ss.getStreamData((Workout)a.UserStream);
                                js += "Aqufit.Page.atiFeaturedCycling.addItem('" + json + "','" + a.AchievementType.Name + "'); ";
                            }
                            Affine.Utils.WorkoutUtil.WorkoutType swimming = Utils.WorkoutUtil.WorkoutType.SWIMMING;
                            Achievement[] swimmingAchievements = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Include("AchievementType").Where(a => a.AchievementType.WorkoutType.Id == (int)swimming && a.UserSetting.Id == ProfileSettings.Id).OrderBy(a => a.AchievementType.DistanceRangeA).ToArray();
                            foreach (Achievement a in swimmingAchievements)
                            {
                                string json = ss.getStreamData((Workout)a.UserStream);
                                js += "Aqufit.Page.atiFeaturedSwimming.addItem('" + json + "','" + a.AchievementType.Name + "'); ";

                            }

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "Achievements", "$(function(){" + js + " });", true);
                            #endregion
                        }
                    }
                    else
                    {                       
                        peopleTabTitle.Text = "Athletes";
                        atiPeoplePanel.Visible = true;
                        atiPeopleViewer.Visible = false;
                        SetupPage();        // do normal page setup                          
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
                aqufitEntities entities = new aqufitEntities();
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {
                    case "page1":
                        int skip1 = Convert.ToInt32(hiddenAjaxValue.Value);
                        var users1 = userQueryLiveNear.Skip(skip1).Take(PEOPLE_TAKE).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey, Duration = 0 }).ToArray();
                        string json1 = users1.ToJson();
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiPeopleList1.generateStreamDom('" + json1 + "');");
                        break;
                    case "page2":
                        int skip2 = Convert.ToInt32(hiddenAjaxValue.Value);
                        var users2 = userQueryRecentActive.Skip(skip2).Take(PEOPLE_TAKE).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey, Duration = 0 }).ToArray();
                        string json2 = users2.ToJson();
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiPeopleList2.generateStreamDom('" + json2 + "');");
                        break;
                    case "page3":
                        int skip3 = Convert.ToInt32(hiddenAjaxValue.Value);
                        var users3 = userQueryMostWatched.Skip(skip3).Take(PEOPLE_TAKE).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey, Duration = 0 }).ToArray();
                        string json3 = users3.ToJson();
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiPeopleList3.generateStreamDom('" + json3 + "');");
                        break;   
                    case "pageFast1":
                        int skip4 = Convert.ToInt32(hiddenAjaxValue.Value);
                        var users4 = workoutQueryFastest1.Skip(skip4).Select(w => new { Id = w.UserSetting.Id, UserName = w.UserSetting.UserName, FName = w.UserSetting.UserFirstName, LName = w.UserSetting.UserLastName, UserKey = w.UserSetting.UserKey, Duration = w.Duration }).Take(PEOPLE_TAKE).ToArray();
                        string json4 = users4.ToJson();
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiPeopleListFast1.generateStreamDom('" + json4 + "');");
                        break;
                    case "configAddWorkout":
                        long workoutKey = Convert.ToInt64(hiddenWodKey.Value);
                        int numshow = Convert.ToInt32( hiddenAjaxValue.Value );
                        hiddenWodKey.Value = ""; // clear the workout key now
                        Affine.Data.json.LeaderBoardWOD[] data = dataMan.SaveCustomLeaderBoardWorkout(ProfileSettings.Id, UserSettings.Id, workoutKey, numshow, 0);
                        System.Web.Script.Serialization.JavaScriptSerializer serial = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string json = serial.Serialize(data);
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page." + atiLeaderBoard2Config.ID + ".appendLeaderBoardJson('" + json + "');  Aqufit.Windows.WorkoutSelectDialog.close();");
                        break;
                    case "RestoreDefault":
                        dataMan.DeleteCustomLeaderBoard(ProfileSettings.Id, UserSettings.Id);
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page." + atiLeaderBoard2Config.ID + ".clear(); ");
                        break;
                    case "configDelWorkout":
                        long wodKey = Convert.ToInt32( hiddenAjaxValue.Value );
                        dataMan.DeleteCustomLeaderBoardWorkout(ProfileSettings.Id, UserSettings.Id, wodKey);
                        break;
                }
            }
            catch (Exception ex)
            {
                status = "ERROR: There was a problem with the action (" + ex.Message + ")";
            }            
        }

        

        private void SetupPage()
        {
            string js = string.Empty;
            if (this.UserSettings == null)
            {   // user is not logged in.. so we can not sure users "near" 
                panelLiveNearMe.Visible = false;
            }
            else
            {
                var usersNear = userQueryLiveNear.Take(PEOPLE_TAKE * 2).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey, Duration = 0 }).ToArray();
                string jsonNear = usersNear.ToJson();
                js += "Aqufit.Page.atiPeopleList1.generateStreamDom('" + jsonNear + "'); ";
            }

           // var users = userQuery.Take(PEOPLE_TAKE * 2).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey }).ToArray();
           // string json = users.ToJson();

            var usersRecentActive = userQueryRecentActive.Take(PEOPLE_TAKE * 2).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey, Duration = 0 }).ToArray();
            string json2 = usersRecentActive.ToJson();
            js += " Aqufit.Page.atiPeopleList2.generateStreamDom('" + json2 + "'); ";

            var usersMostWatched = userQueryMostWatched.Take(PEOPLE_TAKE * 2).Select(u => new { Id = u.Id, UserName = u.UserName, FName = u.UserFirstName, LName = u.UserLastName, UserKey = u.UserKey, Duration = 0 }).ToArray();
            string json3 = usersMostWatched.ToJson();
            js += " Aqufit.Page.atiPeopleList3.generateStreamDom('" + json3 + "'); ";

            // now we grab people with fast times for some of the distance metrics..
            //var usersFastest1 = workoutQueryFastest1.Select(w => new { Id = w.UserSetting.Id, UserName = w.UserSetting.UserName, FName = w.UserSetting.UserFirstName, LName = w.UserSetting.UserLastName, UserKey = w.UserSetting.UserKey, Duration = w.Duration }).OrderBy( w => w.Duration ).GroupBy(w => w.Id).Select(w => w.FirstOrDefault()).Take(PEOPLE_TAKE * 2).ToArray();
            var usersFastest1 = workoutQueryFastest1.Select(w => new { Id = w.UserSetting.Id, UserName = w.UserSetting.UserName, FName = w.UserSetting.UserFirstName, LName = w.UserSetting.UserLastName, UserKey = w.UserSetting.UserKey, Duration = w.Duration }).Take(PEOPLE_TAKE * 2).ToArray();
            string json4 = usersFastest1.ToJson();
            js += " Aqufit.Page.atiPeopleListFast1.generateStreamDom('" + json4 + "'); ";

            var usersFastest2 = workoutQueryFastest2.Select(w => new { Id = w.UserSetting.Id, UserName = w.UserSetting.UserName, FName = w.UserSetting.UserFirstName, LName = w.UserSetting.UserLastName, UserKey = w.UserSetting.UserKey, Duration = w.Duration }).Take(PEOPLE_TAKE * 2).ToArray();
            string json5 = usersFastest2.ToJson();
            js += " Aqufit.Page.atiPeopleListFast2.generateStreamDom('" + json5 + "'); ";            

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiPeopleList1", "$(function(){  "+js+" });", true);
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

