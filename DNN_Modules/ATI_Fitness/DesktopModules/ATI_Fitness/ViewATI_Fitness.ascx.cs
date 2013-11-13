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

namespace Affine.Dnn.Modules.ATI_Fitness
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
    partial class ViewATI_Fitness : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members    
        
        private const int DEFAULT_TAKE = 15;
        private const int TAKE_INC = 10;
        private Affine.Data.Managers.IStreamManager _IStreamManager = Affine.Data.Managers.LINQ.StreamManager.Instance;
        private JavaScriptSerializer _jserializer = new JavaScriptSerializer();
        protected string PhotoUrl { get; set; }
        private string baseUrl;
        #endregion       

        #region Public Methods
        public string BackgroundImageUrl { get; set; }
        public string ProfileCSS { get; set; }
        #endregion        

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
            Page.Title = "FlexFWD: " + (this.ProfileSettings != null ? this.ProfileSettings.UserName : "" );
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
            this.EnsureChildControls();
            try
            {
                /////////////////////////////
                // TESTING
           //     Workout ww = entities.UserStreamSet.Include("UserSetting").OfType<Workout>().Where(w => w.UserSetting.Id == UserSettings.Id && w.WorkoutTypeKey == (long)WorkoutUtil.WorkoutType.CROSSFIT).OrderByDescending(w => w.Id).FirstOrDefault();
           //     DisplayWorkoutStatistics(ww);
                ///////////////////////////
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(panelAjax, panelAjax, RadAjaxLoadingPanel2);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadListViewTags, RadListViewTags, RadAjaxLoadingPanel2);
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(RadGrid2, RadGrid2, RadAjaxLoadingPanel2);                

                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    this.baseUrl = ResolveUrl("~/");
                    RadAjaxManager1.ResponseScripts.Add("$(function(){ $('#tabs').tabs(); });");
                    if (base.UserSettings != null && base.ProfileSettings == null && (!Page.IsPostBack && !Page.IsCallback))
                    {   // This should fix the case of landing on "FitnessProfile.aspx" and we want them at "/username"
                        Response.Redirect(baseUrl + base.UserSettings.UserName, true);
                    }
                    else if (base.UserSettings == null && base.ProfileSettings == null)
                    {   // This only happens when someone "logs out" on their profile page and is redirected back there.
                        Response.Redirect(baseUrl, true);
                    }
                    if (base.ProfileSettings is Group)
                    {
                        Response.Redirect(baseUrl + "group/" + base.ProfileSettings.UserName, true);
                    }                    

                    PhotoUrl = this.baseUrl + ProfileSettings.UserName + "/photos";
                    aStats.HRef = baseUrl + ProfileSettings.UserName + "/achievements";
                    aHistory.HRef = baseUrl + ProfileSettings.UserName + "/workout-history";
                    aGroups.HRef = baseUrl + "Groups";
                    aPhotos.HRef = baseUrl + ProfileSettings.UserName + "/photos";

                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);

                    aqufitEntities entities = new aqufitEntities();
                    // get the groups that the person is in...                    
                    lUserName.Text = "<h2>" + ProfileSettings.UserName + "</h2>";
                    LoadChartData();
                    atiProfile.ProfileSettings = base.ProfileSettings;
                    if (base.ProfileSettings.MainGroupKey.HasValue)
                    {
                        atiProfile.MainGroup = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == ProfileSettings.MainGroupKey.Value);
                    }
                    this.ProfileCSS = base.ProfileSettings.CssStyle;
                    imgAdRight.Src = ResolveUrl("/portals/0/images/adTastyPaleo.jpg");
                    imgAddToFriends.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iAddToFriends.png");
                    imgAddToFollow.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iAddToFollow.png");
                                      
                    // PERMISSIONS: The action panel is only visible to OWNERS
                    if (Permissions == AqufitPermission.OWNER)
                    {
                        panelStreamPost.Visible = false;
                        long[] gIgs = entities.UserFriends.Where(f => f.SrcUserSettingKey == UserSettings.Id).Select(f => f.DestUserSettingKey).ToArray();
                        var groupList = entities.UserSettings.OfType<Group>().Where(Utils.Linq.LinqUtils.BuildContainsExpression<Group, long>(g => g.Id, gIgs)).Select(g => new { Id = g.Id, Name = g.UserFirstName });
                        ddlGroupSchedule.DataTextField = "Name";
                        ddlGroupSchedule.DataValueField = "Id";
                        ddlGroupSchedule.DataSource = groupList;
                        ddlGroupSchedule.DataBind();
                        if (UserSettings.MainGroupKey.HasValue)
                        {
                            // This looks like shit but for some reason other methods of setting the value did not work ???
                            ddlGroupSchedule.SelectedIndex = ddlGroupSchedule.Items.IndexOf(ddlGroupSchedule.Items.FindByValue("" + UserSettings.MainGroupKey.Value));
                        }
                        else
                        {
                            panelNoGroups.Visible = true;
                        }
                                                                    
                        atiProfile.IsOwner = true;
                        atiStreamScript.ShowStreamSelect = true;
                        SiteSetting introSettings = ProfileSettings.SiteSettings.FirstOrDefault(ss => ss.Name == "SiteIntro" );
                        SiteSetting StreamTutorial = ProfileSettings.SiteSettings.FirstOrDefault(ss => ss.Name == "StreamTutorial");
                        // if the user has NO WORKOUTS posted... then we show the tutorial
                        long aWorkoutId = entities.UserStreamSet.OfType<Workout>().Where(s => s.UserSetting.Id == UserSettings.Id).Select(s => s.Id).FirstOrDefault();
                        
                        if( introSettings != null ){
                            RadListViewTags.Visible = false;
                            atiStepByStep.Visible = true;
                            atiStepByStep.Step1Link = ResolveUrl("~/Community/HowTo.aspx") + "?h=2";
                            atiStepByStep.Step2Link = ResolveUrl("~/Community/HowTo.aspx") + "?h=3";
                            atiStepByStep.Step3Link = ResolveUrl("~/Community/HowTo.aspx") + "?h=4";
                            atiStepByStep.Step4Link = ResolveUrl("~/Community/HowTo.aspx") + "?h=5";
                            long aFriend =  entities.UserFriends.Where( f => (f.DestUserSettingKey == UserSettings.Id || f.SrcUserSettingKey == UserSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND ).Select( f => f.Id ).FirstOrDefault();
                            atiStepByStep.Step1Check = aFriend > 0;
                            atiStepByStep.Step2Check = UserSettings.MainGroupKey.HasValue;
                            atiStepByStep.Step3Check = aWorkoutId > 0;
                        }
                        if (StreamTutorial != null)
                        {
                            atiStreamTutorial.Visible = true;
                           // if (aWorkoutId > 0)
                           // {   
                           // }
                        }
                        // always default to workout tab for your Own profile..
                        RadAjaxManager1.ResponseScripts.Add("$(function(){ Aqufit.Page.Tabs.SwitchTab(1); });");  
                        if (Request["w"] != null )
                        {                                                      
                            long wId = Convert.ToInt64(Request["w"]);
                            WOD wod = entities.WODs.Include("WODType").FirstOrDefault(w => w.Id == wId);
                            atiWorkout.LastWorkoutType = Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT;
                            if (wod != null)
                            {
                                atiWorkout.SetControlToWOD = wod;
                            }
                            else
                            {
                                scehduledWorkoutSetup();
                            }
                        }else if(Request["r"] != null ){
                            long rId = Convert.ToInt64(Request["r"]);
                            MapRoute route = entities.MapRoutes.FirstOrDefault(w => w.Id == rId);
                            if (route != null)
                            {   
                                atiWorkout.SetControlToMapRoute = route;
                                atiWorkout.Distance = UnitsUtil.systemDefaultToUnits( route.RouteDistance, base.DistanceUnits);
                            }
                        }                
                        else
                        {   // default the wod to one that is scheduled for the day if that exists ...
                            scehduledWorkoutSetup();                           
                        }

                        
                    }
                    else
                    {                        
                        tabComment.Visible = pageViewComment.Visible = false;
                        tabWorkout.Visible = pageViewWorkout.Visible = false;                                                
                    }                    
                    if (Permissions == AqufitPermission.FRIEND) // You are viewing a friends profile... so should the "Send Message Button";
                    {
                        panelStreamPost.Visible = true;
                        litStreamPostTitle.Text = "<h3>Post a message into " + ProfileSettings.UserName + "'s stream</h3>";                        
                        atiStreamScript.ShowStreamSelect = true;
                        atiProfile.IsFriend = true;
                    }
                    atiProfile.IsFollowing = base.Following;                                                            
                    this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u="+base.ProfileSettings.UserKey+"&p="+base.ProfileSettings.PortalKey+"&bg=1";

                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        protected void RadListViewTags_NeedDataSource(object sender, RadListViewNeedDataSourceEventArgs e)
        {          
            aqufitEntities entities = new aqufitEntities();           
            if (ProfileSettings == null)
            {
                ProfileSettings = UserSettings;                               
            }
            if (ProfileSettings != null)
            {
                Photo[] photos = entities.User2Photo.Where(p => p.UserSettingsKey == ProfileSettings.Id).OrderByDescending(p => p.Photo.Id).Select(p => p.Photo).OfType<Photo>().ToArray();
                RadListViewTags.DataSource = photos;
            }
        }

        private void scehduledWorkoutSetup()
        {
            aqufitEntities entities = new aqufitEntities();
            //long[] groupIds = null;
            //groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == UserSettings.Id || f.DestUserSettingKey == UserSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.SrcUserSettingKey == UserSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            WODSchedule schedule = UserSettings.MainGroupKey.HasValue ? entities.WODSchedules.Include("WOD").Include("WOD.WODType").Where(s => s.UserSetting.Id == UserSettings.MainGroupKey.Value).FirstOrDefault(s => s.Date.CompareTo(DateTime.Today) == 0) : null;
            if (schedule != null)
            {

                atiWorkout.SetControlToWOD = schedule.WOD;
                atiWorkout.LastWorkoutType = WorkoutUtil.WorkoutType.CROSSFIT;
            }
            else
            {
                // try to find "any scheduled wod" from another group
                long[] gIds = entities.UserFriends.Where(f => f.SrcUserSettingKey == UserSettings.Id && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.DestUserSettingKey).ToArray();
                schedule = entities.WODSchedules.Include("WOD").Include("WOD.WODType").Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<WODSchedule, long>(w => w.UserSetting.Id, gIds)).FirstOrDefault(s => s.Date.CompareTo(DateTime.Today) == 0);
                if (schedule != null)
                {
                    atiWorkout.SetControlToWOD = schedule.WOD;
                    atiWorkout.LastWorkoutType = WorkoutUtil.WorkoutType.CROSSFIT;
                }
                else
                {
                    Workout lastWorkout = entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == UserSettings.Id).OrderByDescending(w => w.Id).FirstOrDefault();
                    if (lastWorkout != null)
                    {
                        atiWorkout.LastWorkoutType = Affine.Utils.WorkoutUtil.IntToWorkoutType((int)lastWorkout.WorkoutTypeKey);
                    }
                }
            }
        }


        private void LoadChartData()
        {         
            const int numDays = 7;

            DateTime endDate = DateTime.Now.ToUniversalTime();
            DateTime startDate = endDate.AddDays(-(numDays-1)).ToUniversalTime();
           
            aqufitEntities entities = new aqufitEntities();
            IList<Affine.Data.json.Workout> ret = new List<Affine.Data.json.Workout>();
            IList<Workout> workoutList = entities.UserStreamSet.Include("WOD").Include("WOD.WODType").Include("UserSetting").OfType<Workout>().
                                                Where(w => w.UserSetting.Id == ProfileSettings.Id  
                                                    && w.Date.CompareTo(endDate) <= 0 && w.Date.CompareTo(startDate) > 0).OrderByDescending(w => w.Date).ToList();
            workoutList = workoutList.Reverse().ToList();
            // So we want to display a possible list of 10 strength and 10 cardio workouts.

            //atiHighCharts.SeriesList.Add(new Data.json.HighChartSeries() { name = "test", data = new double[] { 10.0, 33.9, 52.7, 89.3, 5.5, 4.2, 0.0 } });            
            
            Data.json.HighChartSeries cardio = new Data.json.HighChartSeries(){ name = "Cardio", data = new double[numDays] };
            Data.json.WorkoutData[] strenght = new Data.json.WorkoutData[numDays]; 
          //  Data.json.HighChartSeries strength = new Data.json.HighChartSeries(){ name = "Strength", data = new double[numDays] };
            startDate = startDate.ToLocalTime();
            startDate = startDate.AddHours(-startDate.Hour).AddMinutes(-startDate.Minute);
            DateTime itr = startDate;
            int dayCount = 0;
            int cCount = 0;
            int sCount = 0;
            double dist = 0.0;
            double cal = 0.0;
            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;

            while( dayCount <  numDays ){
                if (itr.DayOfYear == DateTime.Today.DayOfYear)
                {
                    atiHighCharts.Categories.Add("<span style=\"color: #fcaf17; font-weight: bolder;\">" + itr.ToString("MMM dd") + "</span>");
                }
                else
                {
                    atiHighCharts.Categories.Add("" + itr.ToString("MMM dd"));
                }
                Workout[] wouts = workoutList.Where( w => w.Date.ToLocalTime().DayOfYear == itr.DayOfYear ).ToArray(); 
                cardio.data[dayCount] = 0;
                strenght[dayCount] = new Data.json.WorkoutData() { Id = -1 };
                //strength.data[dayCount] = 0;
                foreach( Workout w in wouts ){
                    if( w.WorkoutTypeKey == (long)Utils.WorkoutUtil.WorkoutType.CROSSFIT || w.WorkoutTypeKey == (long)Utils.WorkoutUtil.WorkoutType.WEIGHTS ){
     //                   strength.data[dayCount] += 5.0;
                        if (w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED)
                        {   // time in sec
                            strenght[dayCount] = new Affine.Data.json.WorkoutData() {Id = w.Id, T = (int)Affine.Utils.WorkoutUtil.WodType.TIMED, UId = w.Title, S = (double)w.Duration, Un = w.UserSetting.UserName, D = w.Date.ToLocalTime().ToShortDateString(), Rx = (w.RxD.HasValue ? w.RxD.Value : false) };
                        }
                        else if (w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT)
                        {
                            strenght[dayCount] = new Affine.Data.json.WorkoutData() {Id = w.Id, T = (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT, UId = w.Title, S = Math.Round(Affine.Utils.UnitsUtil.systemDefaultToUnits(Convert.ToDouble(w.Max), WeightUnits), 2), Un = w.UserSetting.UserName, D = w.Date.ToLocalTime().ToShortDateString(), Rx = (w.RxD.HasValue ? w.RxD.Value : false) };
                        }
                        else
                        {
                            strenght[dayCount] = new Affine.Data.json.WorkoutData() {Id = w.Id, T = (int)Affine.Utils.WorkoutUtil.WodType.SCORE, UId = w.Title, S = Math.Round(Convert.ToDouble(w.Score), 2), Un = w.UserSetting.UserName, D = w.Date.ToLocalTime().ToShortDateString(), Rx = (w.RxD.HasValue ? w.RxD.Value : false) };
                        } 
                        //strenght[dayCount] = dataMan.workoutToJsonWorkout(w);
                        sCount++;
                        cal += Convert.ToDouble(w.Calories);
                    }else{
                        cardio.data[dayCount] += Utils.UnitsUtil.systemDefaultToUnits( Convert.ToDouble( w.Distance ), UnitsUtil.MeasureUnit.UNIT_KM );
                        cCount++;
                        dist += Convert.ToDouble(w.Distance);
                        cal += Convert.ToDouble(w.Calories);
                    }
                }
                cardio.data[dayCount] = Math.Round(cardio.data[dayCount], 2);
                itr = startDate.AddDays(++dayCount);                
            }
            atiWorkoutSummaryHead.Distance = dist;
            if (UserSettings != null)
            {   // if user is loged in default to their units
                atiWorkoutSummaryHead.Units = UserSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit(Convert.ToInt32(UserSettings.DistanceUnits)) : Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
            }
            else
            {   // otherwise use the viewing profile default
                atiWorkoutSummaryHead.Units = ProfileSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit(Convert.ToInt32(ProfileSettings.DistanceUnits)) : Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
            }
            atiWorkoutSummaryHead.Calories = cal;
            atiWorkoutSummaryHead.NumCardioWorkouts = cCount;
            atiWorkoutSummaryHead.NumStrength = sCount;
            atiWorkoutSummaryHead.StartDate = startDate;
            atiWorkoutSummaryHead.EndDate = endDate;
            atiHighCharts.SeriesList.Add(cardio);
            atiStrengthGraph.WorkoutArray = strenght;
     //       atiHighCharts.SeriesList.Add(strength);
            
            WorkoutTotal totals = entities.WorkoutTotals.FirstOrDefault(wt => wt.UserKey == ProfileSettings.UserKey && wt.PortalKey == ProfileSettings.PortalKey && wt.WorkoutTypeKey == 0);   // When workout type key == 0 This is the "Entire" totals for all workout types            
            atiHighCharts.CardioColor =  Affine.Utils.WorkoutUtil.getRgbForWorkoutDistance(Affine.Utils.UnitsUtil.systemDefaultToUnits((totals != null ? totals.Distance : 0), UnitsUtil.MeasureUnit.UNIT_KM));    // Note: colors are calculated in KM
            atiTotalDistColors.TotalDistance = Math.Round(Affine.Utils.UnitsUtil.systemDefaultToUnits((totals != null ? totals.Distance : 0), UnitsUtil.MeasureUnit.UNIT_KM), 2);

        }

        protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (ddlGroupSchedule.Items.Count > 0)
            {
                long gId = Convert.ToInt64(ddlGroupSchedule.SelectedValue);
                aqufitEntities entities = new aqufitEntities();
                RadGrid2.DataSource = entities.WODSchedules.Where(w => w.UserSetting.Id == gId).Select(w => new { Id = w.WOD.Id, Name = w.WOD.Name, Date = w.Date, WODType = w.WOD.WODType.Name, WODTypeKey = w.WOD.WODType.Id }).OrderByDescending(w => w.Date).ToArray();
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
                    case "AddFriend":
                        try
                        {
                            status = dataManager.sendFriendRequest(UserSettings.Id, this.ProfileSettings.Id);
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiProfile.ShowOk('A Friend request has been sent to " + ProfileSettings.UserName + " ');");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;
                    case "AddSuggestFriend":
                        try
                        {
                            long usid = Convert.ToInt64(hiddenAjaxValue.Value);
                            status = dataManager.sendFriendRequest(UserSettings.Id, usid);
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiProfile.ShowOk('A Friend request has been sent');");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;
                    case "AddFollow":
                        try
                        {
                            status = ss.FollowUser(UserSettings.Id, ProfileSettings.Id);
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiProfile.ShowOk('You are now following " + ProfileSettings.UserName + "');");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;
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
                    case "StreamPost":
                    case "AddComment":
                        try
                        {
                            long toUserSettingsKey = -1;
                            long photoKey = 0;
                            Affine.Data.json.PageMetaData linkJson = null;
                            if (hiddenAjaxAction.Value == "StreamPost")
                            {
                                toUserSettingsKey = ProfileSettings.Id;
                                photoKey = atiStreamPostAttachment.PhotoKey;
                                linkJson = atiStreamPostAttachment.LinkJson;
                            }
                            else
                            {
                                photoKey = atiCommentAttachment.PhotoKey;
                                linkJson = atiCommentAttachment.LinkJson;
                            }
                            Shout shoutRet = dataManager.SaveShout(-1, this.UserId, this.PortalId, toUserSettingsKey, hiddenAjaxValue.Value);
                            bool dirty = false;
                            if (photoKey  > 0)
                            {
                                dataManager.AddAttachmentToStream(shoutRet.Id, photoKey);
                                dirty = true;
                            }
                            else if (linkJson != null)
                            {
                                dirty = true;
                                dataManager.SaveLinkAttachment(UserSettings.Id, shoutRet.Id, linkJson);
                            }
                            if (dirty)
                            {
                                aqufitEntities entities = new aqufitEntities();
                                shoutRet = entities.UserStreamSet.OfType<Shout>().Include("UserSetting").Include("UserAttachments").FirstOrDefault(s => s.Id == shoutRet.Id);
                            }
                            // get the json serializable version of the object.
                            Affine.Data.json.StreamData sd = _IStreamManager.UserStreamEntityToStreamData(shoutRet, null);
                            // RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.Controls.atiStreamPanel.prependJson(" + _jserializer.Serialize(sd) + "); })();"); 
                            string json = _jserializer.Serialize(sd);
                            RadAjaxManager1.ResponseScripts.Add(" (function(){ if(Aqufit.Page.atiStreamPostAttachment)Aqufit.Page.atiStreamPostAttachment.clear(); if(Aqufit.Page.atiCommentAttachment)Aqufit.Page.atiCommentAttachment.clear(); Aqufit.Page.atiStreamScript.prependJson('" + json + "'); Aqufit.Page.atiComment.clear(); })();");
                            //Affine.WebService.StreamService service = new WebService.StreamService();
                            //RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.atiStreamScript.prependJson('" + service.SaveStreamShout(-1, this.UserId, this.PortalId, hiddenAjaxValue.Value) + "'); Aqufit.Page.atiComment.clear(); })();");
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + ex.InnerException.Message + ex.InnerException.StackTrace.Replace("\r", "").Replace("\n", "").Replace("'", "") + "');");                           
                        }
                        break;
                    case "remStepByStep":
                        try
                        {
                            aqufitEntities entities = new aqufitEntities();
                            SiteSetting introSettings = ProfileSettings.SiteSettings.FirstOrDefault(s => s.Name == "SiteIntro");
                            if (introSettings != null)
                            {
                                entities.DeleteObject(entities.SiteSettings.FirstOrDefault(s => s.Id == introSettings.Id));
                                entities.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");  
                        }
                        break;
                    case "remStreamTutorial":
                        try
                        {
                            aqufitEntities entities = new aqufitEntities();
                            SiteSetting StreamTutorial = ProfileSettings.SiteSettings.FirstOrDefault(s => s.Name == "StreamTutorial");
                            if (StreamTutorial != null)
                            {
                                entities.DeleteObject(entities.SiteSettings.FirstOrDefault(s => s.Id == StreamTutorial.Id));
                                entities.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");  
                        }
                        break;
                    case "test":
                        try
                        {
                            aqufitEntities entities = new aqufitEntities();
                            Workout newWorkout = entities.UserStreamSet.OfType<Workout>().Include("WOD").Include("UserSetting").Where(w => w.UserSetting.Id == UserSettings.Id).OrderByDescending(w => w.Id).First();
                            DisplayWorkoutStatistics(newWorkout);
                        }
                        catch (Exception ex) { RadAjaxManager1.ResponseScripts.Add("alert(\"" + ex.StackTrace.Replace("\n","").Replace("'","") + "\");"); }
                       
                        break;
                    case "SaveWorkout":
                        try
                        {
                            
                            aqufitEntities entities = new aqufitEntities();
                            UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
                            // Check if we have a map record accociated with the workout.   
                            
                            if (atiWorkout.WorkoutType.Id == (int)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT)
                            {
                                dataManager.SaveWorkout(UserSettings, atiWorkout.WorkoutType.Id, (int)WorkoutUtil.DataSrc.MANUAL_NO_MAP, atiWorkout.Date, atiWorkout.Time, atiWorkout.Notes, atiWorkout.IsRxD, atiWorkout.WODId, atiWorkout.Score, (int)atiWorkout.MaxWeightUnit);
                            }
                            else
                            {
                                dataManager.SaveWorkout(UserSettings, atiWorkout.WorkoutType.Id, (int)WorkoutUtil.DataSrc.MANUAL_NO_MAP, atiWorkout.Date, atiWorkout.Time, atiWorkout.Distance, atiWorkout.SelectedMapRouteId, (short)atiWorkout.Feeling, (short)atiWorkout.Weather, (short)atiWorkout.Terrain, atiWorkout.Title, atiWorkout.Notes);
                            }
                            // get the workout we just saved
                            Workout newWorkout = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == UserSettings.Id).OrderByDescending(w => w.Id).First();
                            bool dirty = false;
                            if (atiWorkoutAttachment.PhotoKey > 0)
                            {
                                dataManager.AddAttachmentToStream(newWorkout.Id, atiWorkoutAttachment.PhotoKey);
                                dirty = true;
                            }
                            else if (atiWorkoutAttachment.LinkJson != null)
                            {
                                dirty = true;
                                dataManager.SaveLinkAttachment(UserSettings.Id, newWorkout.Id, atiWorkoutAttachment.LinkJson);
                            }
                            if (dirty)
                            {
                                newWorkout = entities.UserStreamSet.OfType<Workout>().Include("UserSetting").Include("UserAttachments").FirstOrDefault(s => s.Id == newWorkout.Id);
                            }                           
                            // get the json serializable version of the object.
                            Affine.Data.json.StreamData sd = _IStreamManager.UserStreamEntityToStreamData(newWorkout, null);
                            // TOOD: "Aqufit.Page.atiWorkout.clear();" was in the response script but it messes up the Ajaax after the first postback..
                            RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.atiStreamScript.prependJson('" + _jserializer.Serialize(sd) + "'); Aqufit.Page.atiWorkout.clear(); })();");
                            status = "Workout has been saved.";
                            // display the stats...
                            DisplayWorkoutStatistics(newWorkout);
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + ex.StackTrace.Replace("\n","").Replace("'","") +  "');");                           
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


        private void DisplayWorkoutStatistics(Workout workout)
        {
            string html = string.Empty;
            string title = string.Empty;
            string statUrl = string.Empty;
            hiddenWorkoutKey.Value = "" + workout.Id;
            aqufitEntities entities = new aqufitEntities();
            // First lets find out "How" we want to compare/display the workout based on the type that it is
            Affine.Utils.WorkoutUtil.WorkoutType wt = WorkoutUtil.IntToWorkoutType( (int)workout.WorkoutTypeKey );
            html += "<ul>";
            if (wt == WorkoutUtil.WorkoutType.CROSSFIT)
            {
                WOD wod = entities.WODs.Include("WODType").First(w => w.Id == workout.WOD.Id);
                // crossfit stats are going to be a bit different... we want to do things like this..
                // say you just did Fran ..
                // 1) how many times have you done it... was it your best time?
                // 2) If it was a group posted workout.. how many people have also posted it so far..
                // 3) How many people in your group did you do better than?
                // 4) If not a group post... how many friends/compare pallet people did you do better then?
                IList<long> friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id || f.DestUserSettingKey == this.ProfileSettings.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FRIEND ).Select(f => (f.SrcUserSettingKey == this.ProfileSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToList();
                friendIds.Add(workout.UserSetting.Id); // add you into the list
                IQueryable<Workout> allFriendsWorkouts = entities.UserStreamSet.OfType<Workout>().Where(w => w.WOD != null && w.WOD.Id == workout.WOD.Id).Where(LinqUtils.BuildContainsExpression<Workout, long>(w => w.UserSetting.Id, friendIds));
                IQueryable<Workout> doneBeforeQuery = allFriendsWorkouts.Where(w => w.UserSetting.Id == workout.UserSetting.Id); 
                int doneBeforeCount = doneBeforeQuery.Count();
                title = workout.Title;
                statUrl = ResolveUrl("~/") + UserSettings.UserName + "/workout/" + workout.Id;

                html += "<li>This is the " + UnitsUtil.numberToNumberPlaceString(doneBeforeCount) + " time you have done this workout.</li>";
                if (doneBeforeCount > 1)
                {   // need to know if this is timed or scored ect ..
                    if (wod.WODType.Id == (long)WorkoutUtil.WodType.TIMED)
                    {
                        int place = doneBeforeQuery.OrderBy(w => w.Duration).Where(w => w.Duration < workout.Duration).Count() + 1;
                        html += "<li>This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best time for this workout.</li>";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.AMRAP || wod.WODType.Id == (long)WorkoutUtil.WodType.SCORE )
                    {
                        int place = doneBeforeQuery.OrderBy(w => w.Score).Where(w => w.Score > workout.Score).Count() + 1;
                        html += "<li>This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best score for this workout.</li>";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.MAX_WEIGHT)
                    {
                        int place = doneBeforeQuery.OrderBy(w => w.Max).Where(w => w.Max >= workout.Max).Count();
                        html += "<li>This is the " + UnitsUtil.numberToNumberPlaceString(place) + " highest weight for this workout.</li>";
                    }
                }                
                // 2) If it was a group posted workout.. how many people have also posted it so far..
                // check if this is a group workout..
                if (wod.UserSettingsKey != 1)
                {
                    Group group = entities.UserSettings.OfType<Group>().FirstOrDefault(u => u.Id == wod.UserSettingsKey);
                    if (group != null)
                    {   // we need to get all the member ids...
                        long[] groupFriendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == group.Id || f.DestUserSettingKey == group.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER ).Select(f => (f.SrcUserSettingKey == group.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();
                        IQueryable<Workout> allGroupEntries = entities.UserStreamSet.OfType<Workout>().Where(w => w.WOD != null && w.WOD.Id == workout.WOD.Id).Where(LinqUtils.BuildContainsExpression<Workout, long>(w => w.UserSetting.Id, groupFriendIds));
                        int gcount = allGroupEntries.Count();
                        html += "<li>So far " + UnitsUtil.numberToNumberPlaceString(gcount) + " members of " + group.UserName + " have posted this workout.</li>";
                        // TODO: how well are you doing in the group..
                    }
                }
                // next we report on friends stats
                // TODO: we really only want to count peoples best times... right now 1 person could be in the query having done the workout 5 times..
                IQueryable<Workout> justFriends = allFriendsWorkouts.Where(w => w.UserSetting.Id != workout.UserSetting.Id);
                int friendsCount = justFriends.Count();
                html += "<li>"+friendsCount + " of your friends have post results for the workout.<br />";
                if (friendsCount > 0)
                {
                    if (wod.WODType.Id == (long)WorkoutUtil.WodType.TIMED)
                    {
                        int place = allFriendsWorkouts.OrderBy(w => w.Duration).Where(w => w.Duration < workout.Duration).Count() + 1;
                        html += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best time out of your firends.<br />";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.AMRAP || wod.WODType.Id == (long)WorkoutUtil.WodType.SCORE)
                    {
                        int place = allFriendsWorkouts.OrderBy(w => w.Score).Where(w => w.Score > workout.Score).Count() + 1;
                        html += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best score out of your friends.<br />";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.MAX_WEIGHT)
                    {
                        int place = allFriendsWorkouts.OrderBy(w => w.Max).Where(w => w.Max >= workout.Max).Count();
                        html += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " highest weight out of your friends.<br />";
                    }
                }
                html += "</li>";

                List<long> watchIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id ) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => f.DestUserSettingKey).ToList();
                
                watchIds.Add(workout.UserSetting.Id); // add you into the list
                IQueryable<Workout> allWatchListWorkouts = entities.UserStreamSet.OfType<Workout>().Where(w => w.WOD != null && w.WOD.Id == workout.WOD.Id).Where(LinqUtils.BuildContainsExpression<Workout, long>(w => w.UserSetting.Id, watchIds));
                IQueryable<Workout> justWatchList = allWatchListWorkouts.Where(u => u.UserSetting.Id != UserSettings.Id);
                int watchListCount = justWatchList.Count();
                html += "<li>"+watchListCount + " of the athletes in your Compare Pallet have results for this workout.<br />";
                if (watchListCount > 0)
                {
                    if (wod.WODType.Id == (long)WorkoutUtil.WodType.TIMED)
                    {
                        int place = allWatchListWorkouts.OrderBy(w => w.Duration).Where(w => w.Duration < workout.Duration).Count() + 1;
                        html += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best time out of your Compare Pallet.<br />";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.AMRAP || wod.WODType.Id == (long)WorkoutUtil.WodType.SCORE)
                    {
                        int place = allWatchListWorkouts.OrderBy(w => w.Score).Where(w => w.Score > workout.Score).Count() + 1;
                        html += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best score out of your Compare Pallet.<br />";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.MAX_WEIGHT)
                    {
                        int place = allWatchListWorkouts.OrderBy(w => w.Max).Where(w => w.Max >= workout.Max).Count();
                        html += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " highest weight out of your Compare Pallet.<br />";
                    }
                }
                html += "</li>";

                // report on stats if it is a "standard workout" (report the levels) 
                if (wod.Standard > 0)
                {
                    // need to know if this is timed or scored ect ..
                    /*
                    if (wod.WODType.Id == (long)WorkoutUtil.WodType.TIMED)
                    {
                        int place = doneBeforeQuery.OrderBy(w => w.Duration).Where(w => w.Duration < workout.Duration).Count() + 1;
                        litDebug.Text += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best time for this workout.<br />";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.AMRAP || wod.WODType.Id == (long)WorkoutUtil.WodType.SCORE)
                    {
                        int place = doneBeforeQuery.OrderBy(w => w.Score).Where(w => w.Score < workout.Score).Count() + 1;
                        litDebug.Text += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " best score for this workout.<br />";
                    }
                    else if (wod.WODType.Id == (long)WorkoutUtil.WodType.MAX_WEIGHT)
                    {
                        int place = doneBeforeQuery.OrderBy(w => w.Max).Where(w => w.Max < workout.Max).Count() + 1;
                        litDebug.Text += "This is the " + UnitsUtil.numberToNumberPlaceString(place) + " highest weight for this workout.<br />";
                    }
                    */
                }

            }
            else if (wt == WorkoutUtil.WorkoutType.WEIGHTS)
            {
                // TODO: this section needs a lot of work...
            }
            else
            {   // TODO: we will want to break these appart.. but for now all cardio has the "same" stats
                // get all workouts of that type ... say "running"
                IQueryable<Workout> allWorkouts = entities.UserStreamSet.OfType<Workout>().Where(w => w.UserSetting.Id == UserSettings.Id && w.WorkoutTypeKey == workout.WorkoutTypeKey);
                // 1) Get some distance metrics
                IQueryable<Workout> allByDist = allWorkouts.Where(w => w.Distance.HasValue ).OrderBy(w => w.Distance);
                int longestRank = allByDist.Where(w => w.Distance >= workout.Distance).Count();

                // get the distance metric (5K, 10K, 15K, Half, Full)
                UnitsUtil.DistanceMetric dmet = UnitsUtil.distanceMetricRounder(workout.Distance.Value, Utils.WorkoutUtil.IntToWorkoutType( (int)workout.WorkoutTypeKey ));
                double lowBound = UnitsUtil.distanceMetricToDistance(dmet);
                double hightBound = UnitsUtil.distanceMetricToDistance(UnitsUtil.ToDistanceMetric((int)dmet - 1));
                IQueryable<Workout> allInRange = allByDist.Where(w => (w.Distance < hightBound && w.Distance >= lowBound));
                int rangeSize = allInRange.Count();
                int fastest = allInRange.Where(w => (w.Duration.HasValue && w.Duration <= workout.Duration)).Count();

                title = workout.Title + (workout.AvrPace.HasValue ? " ("+Utils.UnitsUtil.durationToTimeString( (long)workout.AvrPace.Value ) +")" : "");
                statUrl = ResolveUrl("~/") + UserSettings.UserName + "/workout/" + workout.Id;
                
                DateTime weekAgo = DateTime.Today.AddDays(-7);
                string wtName = Utils.WorkoutUtil.WorkoutTypeKeyToString((int)wt, false);
                if (workout.Date.CompareTo(weekAgo) >= 0)
                {
                    int thisWeek = allByDist.Where(w => w.Date.CompareTo(weekAgo) >= 0).Count();

                    html += "<li>This was your " + UnitsUtil.numberToNumberPlaceString(thisWeek) + " " + wtName + " in the last <span class=\\\"calendar\\\">&nbsp;7</span> days</li>";
                }
                DateTime monthAgo = DateTime.Today.AddDays(-30);
                if (workout.Date.CompareTo(monthAgo) >= 0)
                {
                    int thisMonth = allByDist.Where(w => w.Date.CompareTo(monthAgo) >= 0).Count();
                    html += "<li>This was your " + UnitsUtil.numberToNumberPlaceString(thisMonth) + " " + wtName + " in the last <span class=\\\"calendar\\\">30</span> days</li>";
                }
                // need to do pace info (pace for 5m, 10km, ect.. )
           //     IQueryable<Workout> allByPace = allInRange.Where(w => w.AvrPace.HasValue).OrderBy(w => w.AvrPace);
           //     int paceRank = allInRange.Where(w => w.AvrPace < workout.AvrPace).Count();

                // TODO: award badges based on when a person first gets to the above (1Mile, 5K, 10K, 15K, Half, Full)
                html += "<li>This was your " + UnitsUtil.numberToNumberPlaceString(longestRank) + " longest " + wtName + "</li>";
                html += "<li>This was your " + UnitsUtil.numberToNumberPlaceString(fastest) + " fastest time out of " + rangeSize + " " + wtName + "<br /> between range of " + UnitsUtil.DistanceMetricToString(dmet) + " and " + UnitsUtil.DistanceMetricToString(UnitsUtil.ToDistanceMetric((int)dmet - 1)) + ".</li>";
            //    html += "Your pace of " + workout.AvrPace + " was " + UnitsUtil.numberToNumberPlaceString(paceRank) + "<br />";

                // TODO: compare with some friends..
            }
            html += "</ul>";
            // TODO: this is temp
            var json = new { html = html, title = title, statUrl = statUrl };
            JavaScriptSerializer serial = new JavaScriptSerializer();
            RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.WorkoutStatsDialog.open('" + serial.Serialize(json) + "');  ");
        }
       
        protected void bSubmitWorkout_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string status = string.Empty;
                try
                {
                    aqufitEntities entities = new aqufitEntities();
                    UserSettings settings = entities.UserSettings.FirstOrDefault(us => us.UserKey == this.UserId && us.PortalKey == this.PortalId);
                    // Check if we have a map record accociated with the workout.

                    Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;
                    if(atiWorkout.WorkoutType.Id == (int)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT)
                    {
                        dataManager.SaveWorkout(UserSettings, atiWorkout.WorkoutType.Id, (int)WorkoutUtil.DataSrc.MANUAL_NO_MAP, atiWorkout.Date, atiWorkout.Time, atiWorkout.Notes, atiWorkout.IsRxD, atiWorkout.WODId, atiWorkout.Score, (int)atiWorkout.MaxWeightUnit);
                    }else{
                        dataManager.SaveWorkout(UserSettings, atiWorkout.WorkoutType.Id, (int)WorkoutUtil.DataSrc.MANUAL_NO_MAP, atiWorkout.Date, atiWorkout.Time, atiWorkout.Distance, atiWorkout.SelectedMapRouteId, (short)atiWorkout.Feeling, (short)atiWorkout.Weather, (short)atiWorkout.Terrain, atiWorkout.Title, atiWorkout.Notes);
                    }                   
                    
                    // get the workout we just saved
                    Workout newWorkout = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == UserSettings.Id).OrderByDescending(w => w.Id).First();
                    // get the json serializable version of the object.
                    Affine.Data.json.StreamData sd = _IStreamManager.UserStreamEntityToStreamData(newWorkout, null);
                    RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.atiStreamScript.prependJson('" + _jserializer.Serialize(sd) + "'); Aqufit.Page.atiWorkout.clear(); })();");
                    status = "Workout has been saved.";
                    
                    // display the stats...
                    DisplayWorkoutStatistics(newWorkout);                  
                }
                catch (Exception ex)
                {
                    status = "ERROR: problem saving comment (" + ex.Message + ")";
                }                
                
                //RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
            }          
        }

        
        protected void atiWorkout_RouteItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboBoxCrossfitWorkouts = (RadComboBox)sender;
            atiRadComboBoxCrossfitWorkouts.Items.Clear();
            const int TAKE = 9;
            aqufitEntities entities = new aqufitEntities();
            IQueryable<MapRoute> mapRoutesQuery = string.IsNullOrEmpty(e.Text) ? 
                            entities.User2MapRouteFav.Include("MapRoutes").Where(r => r.UserSettingsKey == UserSettings.Id ).Select( r => r.MapRoute ).OrderBy(w => w.Name) : 
                            entities.User2MapRouteFav.Include("MapRoutes").Where(r => r.UserSettingsKey == UserSettings.Id ).Select( r => r.MapRoute ).Where(r => r.Name.ToLower().StartsWith(e.Text)).OrderBy(r => r.Name);
            int itemOffset = e.NumberOfItems > 0 ? e.NumberOfItems-1 : 0;
            List<MapRoute> mapRoutes = mapRoutesQuery.Skip(itemOffset).Take(TAKE).ToList();
            string mapIcon = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMap.png");
            if (itemOffset == 0)
            {
                RadComboBoxItem item = new RadComboBoxItem("Add New Map");
                item.Value = "{'Id':0, 'Dist':'0'}";
                item.ImageUrl = mapIcon;
                atiRadComboBoxCrossfitWorkouts.Items.Add(item);
            }
            Affine.Utils.UnitsUtil.MeasureUnit unit = base.UserSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit(Convert.ToInt32(base.UserSettings.DistanceUnits)) : UnitsUtil.MeasureUnit.UNIT_MILES;
            string unitName = Affine.Utils.UnitsUtil.unitToStringName(unit);
            foreach (MapRoute mr in mapRoutes)
            {
                double dist = Affine.Utils.UnitsUtil.systemDefaultToUnits( mr.RouteDistance, unit);
                dist = Math.Round(dist, 2);
                RadComboBoxItem item = new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString( mr.Name ) + " (" + dist + " " +unitName+  ")");
                item.Value = "{ 'Id':" + mr.Id + ", 'Dist':" + mr.RouteDistance + "}";
                item.ImageUrl = Affine.Utils.ImageUtil.GetGoogleMapsStaticImage(mr, 200, 150);
                atiRadComboBoxCrossfitWorkouts.Items.Add(item);
            }                            
            int length = mapRoutesQuery.Count();
            int endOffset = Math.Min(itemOffset + TAKE+1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);    
        }
        

        protected void atiWorkout_WodItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboBoxCrossfitWorkouts = (RadComboBox)sender;
            const int TAKE = 10;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                RadComboBoxItem item = new RadComboBoxItem("Create a New Workout");
                item.Value = "{'Id':0, 'Type':'0'}";
                atiRadComboBoxCrossfitWorkouts.Items.Add(item);
            }
            IQueryable<WOD> wods = entities.User2WODFav.Where(w => w.UserSetting.Id == UserSettings.Id).Select(w => w.WOD);
            wods = wods.Union<WOD>(entities.WODs.Where(w => w.Standard > 0));
            wods.Select(w => w.WODType).ToArray();  // hydrate WODTypes
         
            long[] groupIds = null;
            groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == UserSettings.Id || f.DestUserSettingKey == UserSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.SrcUserSettingKey == UserSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            // OK this is a bit of a trick... this query hydrates only the "WODSchedule" in the above WOD query.. so we will get the wods we are looking for..
            IEnumerable< WODSchedule >[] workoutSchedule = entities.UserSettings.OfType<Group>().Where(LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, groupIds)).Select(g => g.WODSchedules.Where(ws => ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0)).ToArray();

            string lowerTxt = e.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(e.Text))
            {
                wods = wods.Where(w => w.Name.ToLower().StartsWith(lowerTxt) || w.WODSchedules.Where(ws => ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0).Any(ws => ws.HiddenName.ToLower().StartsWith(lowerTxt))).OrderBy(w => w.Name);
            }
            else
            {
                wods = wods.OrderByDescending(w => w.CreationDate);
            }
            int length = wods.Count();
            wods = wods.Skip(itemOffset).Take(TAKE);
            WOD[] wodList = wods.ToArray();            
            int endOffset = Math.Min(itemOffset + TAKE,  length);
            e.EndOfItems = endOffset == length;
            for (int i = 0; i < wodList.Length; i++)
            {
                WOD w = wodList[i];
                if (w.WODSchedules != null && w.WODSchedules.Count > 0)
                {
                    WODSchedule ws = w.WODSchedules.OrderByDescending( s => s.HideTillDate ).First();
                    if (ws.HideTillDate.HasValue && DateTime.Now.CompareTo(ws.HideTillDate.Value) < 0)
                    {
                        if (string.IsNullOrWhiteSpace(e.Text))
                        {
                            atiRadComboBoxCrossfitWorkouts.Items.Add(new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString(ws.HiddenName), "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}"));
                            if (w.Standard > 0 || w.WODSchedules.Count > 1)
                            {   // CA - here is what is going on here.  If the workout is suppost to be hidden until a date (common for crossfit gyms) then we just put a date name like up
                                // top.  But if it is a standard WOD (or a wod that they have done before (w.WODSchedules.Count > 1) then we still need to add the WOD
                                atiRadComboBoxCrossfitWorkouts.Items.Add(new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}"));
                            }
                        }
                        else if (ws.HiddenName.ToLower().StartsWith(lowerTxt))
                        {
                            atiRadComboBoxCrossfitWorkouts.Items.Add(new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString(ws.HiddenName), "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}"));
                        }                        
                    }
                    else
                    {
                        atiRadComboBoxCrossfitWorkouts.Items.Add(new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}"));
                    }                    
                }
                else
                {
                    atiRadComboBoxCrossfitWorkouts.Items.Add(new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString(w.Name), "{ 'Id':" + w.Id + ", 'Type':" + w.WODType.Id + "}"));
                }
            }
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);     
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

