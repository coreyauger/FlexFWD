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
using Affine.Utils.Linq;

namespace Affine.Dnn.Modules.ATI_FitnessStats
{
    // Temp permission class
    enum AqufitPermission { OWNER, FRIEND, PUBLIC };

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewATI_Builder class displays the content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    partial class ViewATI_FitnessStats : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members    
        
        private const int DEFAULT_TAKE = 10;
        private const int TAKE_INC = 10;
        #endregion       

        #region Public Methods
      
        #endregion


        

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string uname = this.ProfileSettings != null ? this.ProfileSettings.UserName : this.UserSettings.UserName;
            Page.Title = "FlexFWD Workout: " + uname;
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
                    imgLoading.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/proLoading.gif");
                    if (Settings["Configure"] != null && Convert.ToString( Settings["Configure"] ).Equals("ConfigureLists") )
                    {
                        imgAd.Src = ResolveUrl("~/images/iphoneAd.png");
                        aqufitEntities entities = new aqufitEntities();
                        if (HttpContext.Current.Items["w"] != null || Request["w"] != null)   // view a specific workouts history
                        {
                            atiWorkoutList.Visible = true;
                            atiStatsPanel.Visible = false;
                            atiWorkoutViewer.Visible = true;
                            atiWorkoutListPanel.Visible = false;                            
                            long wid = Request["w"] != null ? Convert.ToInt64(Request["w"]) : Convert.ToInt64(HttpContext.Current.Items["w"]);
                            txtRouteLink.Text = "http://flexfwd.com/"+ ProfileSettings.UserName + "/workout-history/" + wid;
                            Workout workout = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wid);
                            // get all the workouts that have the same title
                            Workout[] workouts = entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == ProfileSettings.Id && w.WorkoutTypeKey == workout.WorkoutTypeKey && w.Title == workout.Title).ToArray();
                            workoutTabTitle.Text = "&nbsp;" + workout.Title;

                            WebService.StreamService streamService = new WebService.StreamService();
                            string json = streamService.getWokoutStreamData(base.ProfileSettings.Id, workout);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "WorkoutList", "$(function(){ Aqufit.Page.atiStreamScript.generateStreamDom('" + json + "'); });", true);

                            //atiStreamScript
                        }
                        else
                        {
                            atiProfileImg2.Settings = base.ProfileSettings;
                            if (ProfileSettings != null && ProfileSettings is Group)
                            {
                                RadGrid1.Visible = false;
                                RadGrid2.Visible = true;
                                atiWorkoutSelector.Visible = false;
                            }
                            atiWorkoutList.Visible = true;
                            atiStatsPanel.Visible = false;
                            workoutTabTitle.Text = "Workout History";
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
                            
                        }
                    }
                    else
                    {   
                        // for now if the user is not logged in they can not view stats...
                        if (UserSettings == null)
                        {
                            Response.Redirect(ResolveUrl("~/Login") + "?returnurl=" + Request.Url.ToString(), true);
                            return;
                        }

                        atiWorkoutList.Visible = false;
                        atiStatsPanel.Visible = true;

                        string requestsUrl = Convert.ToString(Settings["RequestsPage"]);
                        // cache workout types if not already done.
                        // TODO: store this in the application startup

                        // Get all the stream data for the user.
                        // TODO: cache the stream list.       
                        long wId = 0;
                        if (HttpContext.Current.Items["w"] != null)
                        {
                            wId = Convert.ToInt64(HttpContext.Current.Items["w"]);
                        }
                        else if (Request["w"] != null)
                        {
                            wId = Convert.ToInt64(Request["w"]);
                        }
                        if (wId == 0)
                        {
                            throw new Exception("No Workout specified");
                        }
                        aqufitEntities entities = new aqufitEntities();
                        Workout workout = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("WOD.WODType").FirstOrDefault(w => w.Id == wId);
                        atiWorkoutVisualizer.Text = GetFlexEmbed(workout);

                        // Get the 'LONG' list of past workouts (only one time)
                        const int workoutTake = 50;
                        IQueryable<IGrouping<long, Workout>> workoutHistory = entities.UserStreamSet.OfType<Workout>().Include("WOD").OrderByDescending(w => w.Id).Where(w => w.UserSetting.Id == UserSettings.Id && w.WorkoutTypeKey.HasValue).Take(workoutTake).GroupBy(w => w.WorkoutTypeKey.Value);
                        IEnumerable<Workout> workoutHistoryList = new List<Workout>();
                        foreach (IGrouping<long, Workout> wh in workoutHistory)
                        {
                            workoutHistoryList = workoutHistoryList.Concat(wh.AsEnumerable());
                        }                        
                        hiddenWorkoutHistory.Value = workoutHistoryList.Select(w => new { 
                                                    Title = w.Title, 
                                                    Id = w.Id, 
                                                    Type = w.WorkoutTypeKey, 
                                                    Wod = (w.WOD != null ? w.WOD.Id : -1), 
                                                    Distance = w.Distance, 
                                                    Score = w.Score, 
                                                    Max = w.Max, 
                                                    Duration = w.Duration }).OrderBy( w => w.Title).ToArray().ToJson();
                        
                        LoadFlexDataForWorkout(workout, Utils.ConstsUtil.GraphContext.DEFAULT);
                    }                    
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
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

        protected void RadGrid2_ItemCommand(object source, GridCommandEventArgs e)
        {
            ConfigureExport();
            if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName)
            {
                RadGrid2.MasterTableView.ExportToExcel();
            }
            else if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
            {
                RadGrid2.MasterTableView.ExportToWord();
            }
            else if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
            {
                RadGrid2.MasterTableView.ExportToCSV();
            }
        }

        public void ConfigureExport()
        {
            RadGrid grid = RadGrid1.Visible ? RadGrid1 : RadGrid2;
            grid.ExportSettings.FileName = ProfileSettings.UserName + "_workout-history";
            grid.ExportSettings.ExportOnlyData = false;
            grid.ExportSettings.IgnorePaging = false;
            grid.ExportSettings.OpenInNewWindow = true;
        }

        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            RadGrid1.DataSource = entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == ProfileSettings.Id ).
                                    Select(w => new { 
                                        Id = w.Id, 
                                        WorkoutTypeKey = w.WorkoutTypeKey, 
                                        Date = w.Date, 
                                        WODkey =  (w.WOD != null ? w.WOD.Id : -1),
                                        Title = w.Title, 
                                        Distance = w.Distance,
                                        Duration = w.Duration,
                                        Score = w.Score,
                                        Max = w.Max,
                                        DataSrc = w.DataSrc,
                                        Notes = w.Description
                                    }).OrderByDescending( w => w.Id ).ToArray();
        }


        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            aqufitEntities entities = new aqufitEntities();
            RadGrid2.DataSource = entities.WODSchedules.Where( w => w.UserSetting.Id == ProfileSettings.Id ).Select( w => new{ Id = w.WOD.Id, Name = w.WOD.Name, Date = w.Date, WODType = w.WOD.WODType.Name } ).OrderByDescending( w => w.Date ).ToArray();
        }       

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = e.Item as GridDataItem;
                    item["WorkoutTypeKey"].Text = Affine.Utils.WorkoutUtil.WorkoutTypeKeyToString(Convert.ToInt32(item["WorkoutTypeKey"].Text));
                    try
                    {
                        item["Duration"].Text = Affine.Utils.UnitsUtil.durationToTimeString(Convert.ToInt64(item["Duration"].Text));
                    }
                    catch (Exception) { }
                    item["Title"].Text = "<a href=\"" + ResolveUrl("~/") + ProfileSettings.UserName + "/Workout/" + item["Id"].Text + "\">" + item["Title"].Text + "</a>";
                    try
                    {
                        double dist = Convert.ToDouble(item["Distance"].Text);
                        item["Distance"].Text = Math.Round(Affine.Utils.UnitsUtil.systemDefaultToUnits(dist, base.DistanceUnits), 2) + " " + Affine.Utils.UnitsUtil.unitToStringName(base.DistanceUnits);
                    }
                    catch (Exception) { item["Distance"].Text = "&nbsp;"; }
                    if ( !string.IsNullOrEmpty( item["Score"].Text ) &&  item["Score"].Text != "0" && item["Score"].Text != "&nbsp;")
                    {
                        item["Duration"].Text = item["Score"].Text;
                    }
                    else if (!string.IsNullOrEmpty(item["Max"].Text) && item["Max"].Text != "0" && item["Max"].Text != "&nbsp;")
                    {
                        double max = Convert.ToDouble( item["Max"].Text );
                        item["Duration"].Text = "" + Math.Round( Affine.Utils.UnitsUtil.systemDefaultToUnits(max, WeightUnits), 2) + " " + Affine.Utils.UnitsUtil.unitToStringName(WeightUnits);
                    }
                  
                }
            }
            catch (System.FormatException) { }
        }


        protected void atiWorkoutSelector_WorkoutItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboBoxWorkouts = (RadComboBox)sender;
            atiRadComboBoxWorkouts.Items.Clear();
            const int TAKE = 9;
            aqufitEntities entities = new aqufitEntities();
            IQueryable<Workout> workoutQuery = string.IsNullOrEmpty(e.Text) ?
                            entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == ProfileSettings.Id).OrderBy(w => w.Title) :
                            entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == ProfileSettings.Id).Where(r => r.Title.ToLower().StartsWith(e.Text)).OrderBy(r => r.Title);
            int itemOffset = e.NumberOfItems > 0 ? e.NumberOfItems - 1 : 0;
            List<Workout> workouts = workoutQuery.Skip(itemOffset).Take(TAKE).ToList();
            foreach (Workout w in workouts)
            {
                if (atiRadComboBoxWorkouts.Items.FirstOrDefault(i => i.Text == w.Title) == null)
                {
                    RadComboBoxItem item = new RadComboBoxItem(w.Title);
                    item.Value = "" + w.Id;
                    atiRadComboBoxWorkouts.Items.Add(item);
                }
            }
            int length = workoutQuery.Count();
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //
        // CA - this will need to be cleaned up a lot.. but here is the iteration one version
        // ** People can view workouts in 4 modes (everyone, watchlist, friends, just them)
        //     -In the case of running, cycling, swimming, and STANDARD crossfit Wods... we are going to 
        //      be storing peoples best times for (distance rnages) and standard wods... so in these cases
        //      we can shorted the query time when we do an "everyone" query
        //     -Watch list and friends we do the work of getting every time that person did the workout.. not just
        //      there best times.
        //     -Same for the (you only) view
        //        

        private void LoadFlexDataForWorkout(Workout workout, Affine.Utils.ConstsUtil.GraphContext context)
        {
            Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            // onload we send friend, watch, and group member data that is needed to the app
            aqufitEntities entities = new aqufitEntities();
            IList<long> friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == ProfileSettings.Id || f.DestUserSettingKey == ProfileSettings.Id)).Select(f => (f.SrcUserSettingKey == ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList();
            friendIds.Add(ProfileSettings.Id);

            // send the persons profile through
            var profile = new { UserName = ProfileSettings.UserName, Id = ProfileSettings.Id, FirstName = ProfileSettings.UserFirstName, LastName= ProfileSettings.UserLastName };
            hiddenProfileJson.Value = serializer.Serialize(profile);

            var friends = entities.UserSettings.OfType<User>().Where(LinqUtils.BuildContainsExpression<User, long>(s => s.Id, friendIds)).Select(u => new { Id = u.Id, Un = u.UserName, Fn = u.UserFirstName, Ln = u.UserLastName }).ToArray();
            hiddenFriendJson.Value = serializer.Serialize(friends);
            // get groups
            long[] groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == ProfileSettings.Id || f.DestUserSettingKey == ProfileSettings.Id) && f.Relationship > 0).Select(f => f.SrcUserSettingKey == ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            UserSettings[] groupList = entities.UserSettings.OfType<Group>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, groupIds)).OrderBy(f => f.UserName).ToArray();
            var groups = entities.UserSettings.OfType<Group>().Where(LinqUtils.BuildContainsExpression<Group, long>(s => s.Id, groupIds)).Select(u => new { Id = u.Id, Un = u.UserName }).ToArray();
            hiddenGroupJson.Value = serializer.Serialize(groups);
            // next we load the workout compare to all (friends)
            
            Affine.WebService.StreamService ss = new WebService.StreamService();
            
            //hiddenWorkout.Value = ss.getWorkout(workout.Id);
            Affine.Data.json.Workout jsonWorkout = dataManager.workoutToJsonWorkout(workout);
            Affine.Data.json.WOD wod = null;
            Affine.Data.json.WorkoutData[] jsonWorkoutData = null;
            //var workoutJson = new { Id = workout.Id, WorkoutTypeKey = workout.WorkoutTypeKey, Title = workout.Title, Date = workout.Date.ToLocalTime().ToShortDateString(), DataSrc = workout.DataSrc };

            Affine.Utils.UnitsUtil.MeasureUnit weight = base.WeightUnits;         
            if (workout.WorkoutTypeKey == (int)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT)
            {
                if (context == Utils.ConstsUtil.GraphContext.DEFAULT)
                {   // default context for crossfit is the watchlist
                    context = Utils.ConstsUtil.GraphContext.WATCHLIST;
                }
                // if this is a crossfit workout
                // we want to get ALL the same WoDs for you and your friends
                IQueryable<Workout> workoutDataQuery = context == Utils.ConstsUtil.GraphContext.EVERYONE ?
                                    entities.UserStreamSet.Include("UserSettings").OfType<Workout>().Where(w => w.WOD.Id == workout.WOD.Id).AsQueryable()
                                    : 
                                    context == Utils.ConstsUtil.GraphContext.WATCHLIST ?                                    
                                    entities.UserStreamSet.Include("UserSettings").OfType<Workout>().Where(w => w.WOD.Id == workout.WOD.Id).Where(LinqUtils.BuildContainsExpression<Workout, long>(w => w.UserSetting.Id, friendIds)).AsQueryable()
                                    : 
                                    context == Utils.ConstsUtil.GraphContext.FRIENDS ?
                                    entities.UserStreamSet.Include("UserSettings").OfType<Workout>().Where(w => w.WOD.Id == workout.WOD.Id).Where(LinqUtils.BuildContainsExpression<Workout, long>(w => w.UserSetting.Id, friendIds)).AsQueryable()
                                    :
                                    entities.UserStreamSet.Include("UserSettings").OfType<Workout>().Where(w => w.WOD.Id == workout.WOD.Id && w.UserSetting.Id == ProfileSettings.Id ).AsQueryable();
                var workoutData = workoutDataQuery.Select(w => new { Id = w.Id, UsKey = w.UserSetting.Id, Un = w.UserSetting.UserName, T = w.Duration, S = w.Score, M = w.Max, Rx = w.RxD, D = w.Date }).ToArray();
                wod = new Affine.Data.json.WOD (){ Id = workout.WOD.Id, Standard = workout.WOD.Standard, Type = workout.WOD.WODType.Id, Name = workout.WOD.Name, Description = workout.WOD.Description };
                if (wod.Type == (int)Affine.Utils.WorkoutUtil.WodType.TIMED)
                {   // time in sec
                    jsonWorkoutData = workoutData.OrderByDescending(w => w.T).Select(w => new Affine.Data.json.WorkoutData() { UId = w.Un + "_" + w.Id, Id = w.Id, S = (double)w.T, Un = w.Un, D = w.D.ToLocalTime().ToShortDateString(), Rx = (w.Rx.HasValue ? w.Rx.Value : false) }).ToArray();
                }
                else if (wod.Type == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT)
                {
                    jsonWorkoutData = workoutData.OrderByDescending(w => w.M).Select(w => new Affine.Data.json.WorkoutData() { UId = w.Un + "_" + w.Id, Id = w.Id, S = Math.Round(Affine.Utils.UnitsUtil.systemDefaultToUnits(Convert.ToDouble(w.M), weight), 2), Un = w.Un, D = w.D.ToLocalTime().ToShortDateString(), Rx = (w.Rx.HasValue ? w.Rx.Value : false) }).ToArray();
                }
                else
                {
                    jsonWorkoutData = workoutData.OrderByDescending(w => w.S).Select(w => new Affine.Data.json.WorkoutData() { UId = w.Un + "_" + w.Id , Id = w.Id, S = Math.Round(Convert.ToDouble(w.S), 2), Un = w.Un, D = w.D.ToLocalTime().ToShortDateString(), Rx = (w.Rx.HasValue ? w.Rx.Value : false) }).ToArray();
                }
            }
            else
            {   // for now this will handle "running, swimming, walking, cycling"
                if (context == Utils.ConstsUtil.GraphContext.DEFAULT)
                {
                    if (workout.DataSrc == (short)Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP || workout.DataSrc == (short)Utils.WorkoutUtil.DataSrc.MANUAL_WITH_MAP)
                    {
                        context = Utils.ConstsUtil.GraphContext.EVERYONE;
                    }
                    else if (workout.DataSrc == (short)Utils.WorkoutUtil.DataSrc.NIKE_NO_MAP)
                    {
                        context = Utils.ConstsUtil.GraphContext.WATCHLIST;
                    }
                    // TODO: not sure about this one....
                    /*
                else if (workout.DataSrc == (short)Utils.WorkoutUtil.DataSrc.MANUAL_WITH_MAP)
                {   // TODO: ?? this is a special case that we want to compare all the times of people that have logged a run for the route.
                    long mapRouteKey = entities.UserStreamSet.OfType<Workout>().Where(w => w.Id == workout.Id).Select(w => w.WorkoutExtendeds.FirstOrDefault().MapRoute.Id).FirstOrDefault();
                    IQueryable<Workout> workoutQuery = entities.WorkoutExtendeds.Include("UserStream").Where(we => we.MapRoute != null && we.MapRoute.Id == mapRouteKey).Select(we => we.UserStream).OfType<Workout>().OrderBy(w => w.Duration).Take(5000);                        
                    workoutQuery.Select(w => w.UserSetting).ToArray(); // hydrate.. not working above so do it here.
                    Workout[] data = workoutQuery.ToArray();
                    var wd = data.OrderByDescending(w => w.Duration).Select(w => new
                    {
                        UId = w.UserSetting.UserName + "_" + w.Id,
                        Id = w.Id,
                        S = w.Duration,
                        Un = w.UserSetting.UserName,
                        D = w.Date.ToLocalTime().ToShortDateString()
                    }).ToArray();
                    var ret = new { WOD = new { }, WorkoutData = wd, Workout = workoutJson, Context = (int)context };
                    hiddenWorkoutData.Value = serializer.Serialize(ret);
                }*/
                    else
                    {
                        context = Utils.ConstsUtil.GraphContext.ME;
                    }
                }
                             
                if (context == Utils.ConstsUtil.GraphContext.EVERYONE)
                {   // we want to compare achievments...
                    IQueryable<Workout> workoutQuery = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Where(a =>
                                                                                                    a.AchievementType.WorkoutType.Id == workout.WorkoutTypeKey &&
                                                                                                    workout.Distance >= a.AchievementType.DistanceRangeA &&
                                                                                                    workout.Distance < a.AchievementType.DistanceRangeB).Take(5000).Select(a => a.UserStream).OfType<Workout>().AsQueryable();
                    workoutQuery = workoutQuery.Union(entities.UserStreamSet.OfType<Workout>().Where(w => w.Id == workout.Id));
                    workoutQuery.Select(w => w.UserSetting).ToArray(); // hydrate.. not working above so do it here.
                    Workout[] data = workoutQuery.ToArray();
                    jsonWorkoutData = data.OrderByDescending(w => w.Duration).Select(w => new Affine.Data.json.WorkoutData()
                    {
                        UId = w.UserSetting.UserName + "_" + w.Id,
                        Id = w.Id,
                        S = w.Duration.Value,
                        Un = w.UserSetting.UserName,
                        D = w.Date.ToLocalTime().ToShortDateString()
                    }).ToArray();                    
                }
                else if (context == Utils.ConstsUtil.GraphContext.WATCHLIST || context == Utils.ConstsUtil.GraphContext.FRIENDS)
                {
                    IQueryable<Workout> workoutQuery = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Where(a =>
                                                                                                    a.AchievementType.WorkoutType.Id == workout.WorkoutTypeKey &&
                                                                                                    workout.Distance >= a.AchievementType.DistanceRangeA &&
                                                                                                    workout.Distance < a.AchievementType.DistanceRangeB).
                                                                                                    Where(LinqUtils.BuildContainsExpression<Achievement, long>(a => a.UserSetting.Id, friendIds)).Take(5000).Select(a => a.UserStream).OfType<Workout>().AsQueryable();
                    workoutQuery = workoutQuery.Union(entities.UserStreamSet.OfType<Workout>().Where(w => w.Id == workout.Id));
                    workoutQuery.Select(w => w.UserSetting).ToArray(); // hydrate.. not working above so do it here.
                    Workout[] data = workoutQuery.ToArray();
                    jsonWorkoutData = data.OrderByDescending(w => w.Duration).Select(w => new Affine.Data.json.WorkoutData()
                    {
                        UId = w.UserSetting.UserName + "_" + w.Id,
                        Id = w.Id,
                        S = w.Duration.Value,
                        Un = w.UserSetting.UserName,
                        D = w.Date.ToLocalTime().ToShortDateString()
                    }).ToArray();                    
                }
                else if (context == Utils.ConstsUtil.GraphContext.ME)   // this is just you for the ranges.. since no map data
                {
                    IQueryable<Workout> workoutQuery = entities.Achievements.Include("UserStream").Include("UserStream.UserSetting").Where(a => a.UserSetting.Id == ProfileSettings.Id &&
                                                                                                    a.AchievementType.WorkoutType.Id == workout.WorkoutTypeKey &&
                                                                                                    workout.Distance >= a.AchievementType.DistanceRangeA &&
                                                                                                    workout.Distance < a.AchievementType.DistanceRangeB).
                                                                                                    Take(5000).Select(a => a.UserStream).OfType<Workout>().AsQueryable();
                   workoutQuery = workoutQuery.Union(entities.UserStreamSet.OfType<Workout>().Where(w => w.Id == workout.Id));
                   workoutQuery.Select(w => w.UserSetting).ToArray(); // hydrate.. not working above so do it here.                 
                   Workout[] data = workoutQuery.ToArray();
                   jsonWorkoutData = data.OrderByDescending(w => w.Duration).Select(w => new Affine.Data.json.WorkoutData()
                   {
                       UId = w.UserSetting.UserName + "_" + w.Id,
                       Id = w.Id,
                       S = w.Duration.Value,
                       Un = w.UserSetting.UserName,
                       D = w.Date.ToLocalTime().ToShortDateString()
                   }).ToArray();                                         
                }                
            }
            var retWorkout = new { WOD = wod, WorkoutData = jsonWorkoutData, Workout = jsonWorkout, Context = (int)context };
            hiddenWorkoutData.Value = serializer.Serialize(retWorkout);
        }


        private string GetFlexEmbed(Workout workout)
        {            
            string ret = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" id=\"ATI_Aqufit\" width=\"100%\" height=\"100%\" codebase=\"http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab\">";
            ret += "<param name=\"movie\" value=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/Scratch.swf") + "\" />";
            ret += "<param name=\"quality\" value=\"high\" /><param name=\"bgcolor\" value=\"#FFFFFF\" />";
            ret += "<param name=\"flashvars\" value=\"u=" + ProfileSettings.UserKey + "&w="+workout.Id + "&url=http://flexfwd.com/\" />";
            ret += "<param name=\"allowScriptAccess\" value=\"sameDomain\" />";
            ret += "<param name=\"wmode\" VALUE=\"transparent\">";
            ret += "<embed src=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/Scratch.swf") + "\" wmode=\"transparent\" quality=\"high\" bgcolor=\"#FFFFFF\" width=\"100%\" height=\"100%\" name=\"ATI_Aqufit\" align=\"middle\" play=\"true\" loop=\"false\" quality=\"high\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\"";
            ret += " flashvars=\"u=" + ProfileSettings.UserKey + "&w=" + workout.Id + "&url=http://flexfwd.com/\"";
            ret += " pluginspage=\"http://www.adobe.com/go/getflashplayer\"></embed></object>";
            return ret;
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                //Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
                aqufitEntities entities = new aqufitEntities();
                switch (hiddenAjaxAction.Value)
                {
                    case "getProfile":
                        // TODO: verify that this person can view stats on the person?
                        UserSettings profile = entities.UserSettings.Include("Metrics").FirstOrDefault(u => u.UserName == hiddenAjaxValue.Value && u.PortalKey == this.PortalId);
                        DotNetNuke.Entities.Users.UserController ucontroller = new UserController();
                        UserInfo ui = ucontroller.GetUser((int)profile.PortalKey, (int)profile.UserKey);
                        Affine.Data.json.UserSetting us = new Affine.Data.json.UserSetting()
                        {
                            Id = profile.Id,
                            PortalKey = profile.PortalKey,
                            FirstName = profile.UserFirstName,
                            LastName = profile.UserLastName,
                            UserKey = profile.UserKey,
                            UserName = profile.UserName,                            
                            LatHome = Convert.ToDouble(profile.LatHome),
                            LngHome = Convert.ToDouble(profile.LngHome)
                        };
                        us.Metrics = profile.Metrics.Select(m => new Affine.Data.json.Metric() { Id = m.Id, MetricType = m.MetricType, MetricValue = m.MetricValue }).ToArray();
                        WorkoutTotal totals = entities.WorkoutTotals.FirstOrDefault(wt => wt.UserKey == profile.UserKey && wt.PortalKey == profile.PortalKey && wt.WorkoutTypeKey == 0);   // When workout type key == 0 This is the "Entire" totals for all workout types
                        us.Totals = new Data.json.Totals() { Calories = totals.Calories, Count = totals.Count, Distance = totals.Distance, Id = totals.Id };
                        RadAjaxManager1.ResponseScripts.Add("pushProfileToFlex('" + us.ToJson() + "'); ");                  
                        break;
                    case "getWorkout":
                        long wid = Convert.ToInt64(hiddenAjaxValue.Value);
                        int context = Convert.ToInt32(hiddenAjaxContext.Value);
                        Workout wo = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("WOD.WODType").Include("WorkoutExtendeds").FirstOrDefault(w => w.Id == wid);
                        LoadFlexDataForWorkout(wo, Utils.ConstsUtil.toGraphContext(context));
                        RadAjaxManager1.ResponseScripts.Add("pushWorkoutToFlex('" + hiddenWorkoutData.Value + "'); ");
                        break;
                }
            }
            catch (Exception ex)
            {
                status = "ERROR: There was a problem with the action (" + ex.Message + ")" + ex.StackTrace;
                RadAjaxManager1.ResponseScripts.Add("alert('" + status.Replace("\r\n","") + "'); ");
            }
            //RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
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

