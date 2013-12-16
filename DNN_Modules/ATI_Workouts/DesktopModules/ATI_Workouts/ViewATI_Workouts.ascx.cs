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


namespace Affine.Dnn.Modules.ATI_Workouts
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
    partial class ViewATI_Workouts : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
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
                imgAd.Src = ResolveUrl("/portals/0/images/adTastyPaleo.jpg");
                imgCheck.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
                
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    baseUrl = ResolveUrl("~/");
                    long wId = 0;
                    if (HttpContext.Current.Items["w"] != null)
                    {
                        wId = Convert.ToInt64(HttpContext.Current.Items["w"]);
                    }
                    else if (Request["w"] != null)
                    {
                        wId = Convert.ToInt64(Request["w"]);
                    }

                    // Are we viewing a specific workout ?
                    if (wId > 0)
                    {
                        divMainLinks.Visible = true;

                        atiProfile.Visible = false;
                        hiddenWorkoutKey.Value = "" + wId;
                        aqufitEntities entities = new aqufitEntities();
                        WOD wod = entities.WODs.Include("WODType").Include("WODSets").Include("WODSets.WODExercises").Include("WODSets.WODExercises.Exercise").FirstOrDefault(w => w.Id == wId);                                  
                        if( base.UserSettings != null && wod.Standard == 0){
                            User2WODFav fav = entities.User2WODFav.FirstOrDefault(mr => mr.WOD.Id == wod.Id && mr.UserSetting.Id == UserSettings.Id);
                            bAddWorkout.Visible = fav == null;
                            bRemoveWorkout.Visible = fav != null;
                        }                      
                        lWorkoutTitle.Text = wod.Name;
                        
                      //  lWorkoutDescription.Text = wod.Description;

                       // constructWorkoutInfo(wod);
                        lWorkoutInfo.Text = wod.Description;
                        if (wod.Standard > 0)
                        {
                            imgCrossFit.Visible = true;
                            imgCrossFit.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/xfit.png");
                            atiProfileImg.Visible = false;
                        }
                        else
                        {
                            atiProfileImg.Settings = entities.UserSettings.FirstOrDefault(us => us.Id == wod.UserSettingsKey);
                        }
                        atiWorkoutPanel.Visible = false;
                        atiWorkoutViewer.Visible = true;
                        
                        // Get the leader board
                        IQueryable<Workout> stream = entities.UserStreamSet.OfType<Workout>().Where(w => w.WOD.Id == wId);                        
                        long typeId = entities.WODs.Where(w => w.Id == wId).Select(w => w.WODType.Id).FirstOrDefault();
                        switch (typeId)
                        {
                            case (long)Affine.Utils.WorkoutUtil.WodType.AMRAP:
                            case (long)Affine.Utils.WorkoutUtil.WodType.SCORE:
                                atiScoreRangePanel.Visible = true;
                                stream = stream.OrderByDescending(w => w.Score);
                                break;
                            case (long)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT:
                                atiMaxRangePanel.Visible = true;
                                atiMaxWeightUnitsFirst.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                                atiMaxWeightUnitsFirst.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                                atiMaxWeightUnitsFirst.Selected = WeightUnits;
                                atiMaxWeightUnitsLast.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                                atiMaxWeightUnitsLast.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                                atiMaxWeightUnitsLast.Selected = WeightUnits;
                                stream = stream.OrderByDescending(w => w.Max);
                                break;
                            case (long)Affine.Utils.WorkoutUtil.WodType.TIMED:
                                atiTimeSpanePanel.Visible = true;
                                stream = stream.OrderBy(w => w.Duration);
                                break;
                            default:
                                stream = stream.OrderByDescending(w => w.TimeStamp);
                                break;
                        }                        
                        string js = string.Empty;
                        atiShareLink.ShareLink = "http://" + Request.Url.Host + "/workout/" + wod.Id;
                        atiShareLink.ShareTitle = "FlexFWD.com crossfit WOD " + wod.Name;

                        workoutTabTitle.Text = "&nbsp;" + (string.IsNullOrWhiteSpace(wod.Name) ? "Untitled" : wod.Name) + "&nbsp;";
                        Affine.WebService.StreamService ss = new WebService.StreamService();
                        string jsonEveryone = ss.getStreamDataForWOD(wod.Id, -1, 0, 15, true, true, -1, -1, -1);

                        string jsonYou = string.Empty;

                        js += " Aqufit.Page." + atiWorkoutHighChart.ID + ".fromStreamData('" + jsonEveryone + "'); ";                                               
                        if (base.UserSettings != null)
                        {
                            hlLogWorkout.HRef = baseUrl + UserSettings.UserName + "?w=" + wId;
                            hlWorkouts.HRef = baseUrl + UserSettings.UserName + "/workout-history";
                            jsonYou = ss.getStreamDataForWOD(wod.Id, base.UserSettings.Id, 0, 10, true, true, -1, -1, -1);
                            js += "Aqufit.Page.atiYouStreamScript.generateStreamDom('" + jsonYou + "');";
                            js += " Aqufit.Page." + atiWorkoutHighChart.ID + ".fromYourStreamData('" + jsonYou + "'); ";
                            // TODO: this could be improved on...
                            Workout thisWod = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == wId);
                            if (thisWod != null)
                            {   // graphs of this wod
                                hlGraph.HRef = ResolveUrl("~/") + UserSettings.UserName + "/workout/" + thisWod.Id;
                            }
                            else
                            {   // just grab any workout then..
                                Workout any = entities.UserStreamSet.OfType<Workout>().OrderByDescending( w => w.Id ).FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id);
                                if (any != null)
                                {
                                    hlGraph.HRef = ResolveUrl("~/") + UserSettings.UserName + "/workout/" + any.Id;
                                }
                                else
                                {   // no workouts ??? say what.. slack man :) hide graph
                                    hlGraph.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            hlGraph.HRef = ResolveUrl("~/Login");
                            hlLogWorkout.HRef = hlGraph.HRef;
                            hlWorkouts.HRef = hlGraph.HRef;
                            atiPanelYourProgress.Visible = false;
                        }
                        hlCreateWOD.HRef = baseUrl + "Profile/WorkoutBuilder";
                        hlMyWorkouts.HRef = baseUrl + "Profile/MyWorkouts";
                        js += " Aqufit.Page." + atiWorkoutHighChart.ID + ".drawChart(); ";
                        


                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "SimilarRouteList", "$(function(){ " + js + " Aqufit.Page.atiEveryoneStreamScript.generateStreamDom('" + jsonEveryone + "'); });", true);

                        

                        YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
                        //order results by the number of views (most viewed first)
                        query.OrderBy = "viewCount";                       
                        query.NumberToRetrieve = 3;
                        query.SafeSearch = YouTubeQuery.SafeSearchValues.Moderate;
                        
                        YouTubeRequestSettings settings = new YouTubeRequestSettings(ConfigurationManager.AppSettings["youtubeApp"], ConfigurationManager.AppSettings["youtubeKey"]);
                        YouTubeRequest request = new YouTubeRequest(settings);
                        const int NUM_ENTRIES = 50;
                        IList<Video> videos = new List<Video>();
                        IList<Video> groupVideo = new List<Video>();
                        // first try to find videos with regard to users group                        
                        Feed<Video> videoFeed = null;
                        
                        if (this.UserSettings != null)
                        {
                            long[] groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == UserSettings.Id || f.DestUserSettingKey == UserSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => f.SrcUserSettingKey == UserSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
                            Group business = entities.UserSettings.OfType<Group>().Where(Utils.Linq.LinqUtils.BuildContainsExpression<Group, long>(us => us.Id, groupIds)).FirstOrDefault(g => g.GroupType.Id == 1);
                            if (business != null)
                            {   // TODO: need the business name...
                                query.Query = business.UserName;
                            }
                            else
                            {
                                UserSettings creator = entities.UserSettings.FirstOrDefault(u => u.Id == wod.UserSettingsKey);
                                if (creator is Group )
                                {   // TODO: need the business name...
                                    query.Query = creator.UserFirstName + " " + creator.UserLastName;
                                }
                                else
                                {
                                    query.Query = UserSettings.UserFirstName + " " + UserSettings.UserLastName;
                                }
                            }
                            videoFeed = request.Get<Video>(query);
                            foreach (Video v in videoFeed.Entries)
                            {
                                groupVideo.Add(v);
                            }
                        }                                               
                        if (videos.Count < NUM_ENTRIES)
                        {   // now try the crossfit WOD name
                            query.NumberToRetrieve = NUM_ENTRIES - videos.Count;
                            query.Query = "crossfit wod " + wod.Name;
                            videoFeed = request.Get<Video>(query);
                            foreach (Video v in videoFeed.Entries)
                            {
                                videos.Add(v);
                            }
                            if (videos.Count < NUM_ENTRIES)
                            {   // this is last resort .. just get videos about crossfit...
                                query.NumberToRetrieve = NUM_ENTRIES - videos.Count;
                                query.Query = "crossfit wod";
                                videoFeed = request.Get<Video>(query);
                                foreach (Video v in videoFeed.Entries)
                                {
                                    videos.Add(v);
                                }
                            }
                        }

                        const int TAKE = 3;
                        if (videos.Count > TAKE)
                        {
                            Random random = new Random((int)DateTime.Now.Ticks);
                            int rand = random.Next(videos.Count - TAKE);
                            videos = videos.Skip(rand).Take(TAKE).ToList();
                            if( groupVideo.Count > 0 ){
                                // always replace one of the main videos with a gorup one.. (if possible)
                                rand = random.Next(groupVideo.Count - 1);
                                videos[0] = groupVideo[rand];
                            }
                            atiYoutubeThumbList.VideoFeed = videos;
                        }
                        else
                        {
                            atiVideoPanel.Visible = false;
                        }
                    }
                    else
                    {
                        atiVideoPanel.Visible = false;
                        atiPanelQuickView.Visible = false;
                        hlGraph.Visible = false;
                        aqufitEntities entities = new aqufitEntities();
                        var exerciseArray = entities.Exercises.OrderBy( x => x.Name ).Select( x => new{ Text = x.Name, Value = x.Id } ).ToArray();
                        RadListBoxExcerciseSource.DataSource = exerciseArray;
                        RadListBoxExcerciseSource.DataBind();
                        
                        string order = orderDate.Checked ? "date" : "popular";
                        if (Settings["Configure"] != null && Convert.ToString(Settings["Configure"]).Equals("ConfigureMyWorkouts"))
                        {
                            this.IsMyWorkouts = true;
                            atiProfile.ProfileSettings = base.UserSettings;
                            atiProfile.IsOwner = true;
                            atiProfile.IsSmall = true;
                            atiWorkoutPanel.Visible = true;
                            workoutTabTitle.Text = "My Workouts";
                            liMyWorkouts.Visible = false;                      
                            liFindWorkout.Visible = true;
                            WebService.StreamService streamService = new WebService.StreamService();
                            string json = streamService.GetWorkouts(base.UserSettings.Id, 0, 30, order, null);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "WorkoutList", "$(function(){ Aqufit.Page.atiWorkoutListScript.isMyRoutes = true; Aqufit.Page.atiWorkoutListScript.generateStreamDom('" + json + "'); });", true);
                        }
                        else
                        {
                            this.IsMyWorkouts = false;
                            atiProfile.Visible = false;
                            workoutTabTitle.Text = "Workouts";
                            atiWorkoutPanel.Visible = true;
                            atiWorkoutViewer.Visible = false;
                            WebService.StreamService streamService = new WebService.StreamService();
                            string json = streamService.GetWorkouts(-1, 0, 30, order, null);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "WorkoutList2", "$(function(){ Aqufit.Page.atiWorkoutListScript.generateStreamDom('" + json + "'); });", true);
                        }                          
                    }              
                }                           
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }

        private void constructWorkoutInfo(WOD wod)
        {
            lWorkoutInfo.Text = "";
            int i = 1;
            string timecap = string.Empty;
            if (wod.TimeSpan.HasValue)
            {
                timecap = " ("+ Utils.UnitsUtil.durationToTimeString(wod.TimeSpan.Value) + ")  hh:mm:ss";
            }
            lWorkoutInfo.Text = "Workout Style: <b>" + wod.WODType.Name + "</b>" + timecap + "<br /><br />";
            foreach (WODSet set in wod.WODSets)
            {                
                lWorkoutInfo.Text += "<span class=\"wodSet\">Set: " + (i++) + "</span>";
                foreach (WODExercise we in set.WODExercises)
                {
                    lWorkoutInfo.Text += "<span class=\"wodEx\">";
                    string additional = string.Empty;
                    if (we.MenDist.HasValue && we.MenDist.Value > 0)
                    {
                        additional += "Men: " + Affine.Utils.UnitsUtil.systemDefaultToUnits(we.MenDist.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M) + Affine.Utils.UnitsUtil.unitToStringName(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M);
                    }
                    if (we.WomenDist.HasValue && we.WomenDist.Value > 0)
                    {
                        additional += ", Women: " + Affine.Utils.UnitsUtil.systemDefaultToUnits(we.MenDist.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M) + Affine.Utils.UnitsUtil.unitToStringName(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M);
                    }
                    if (we.MenRx.HasValue && we.MenRx.Value > 0)
                    {
                        additional += "Men: " + Affine.Utils.UnitsUtil.systemDefaultToUnits(we.MenRx.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS) + Affine.Utils.UnitsUtil.unitToStringName(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    }
                    if (we.WomenRx.HasValue && we.WomenRx.Value > 0)
                    {
                        additional += ", Women: " + Affine.Utils.UnitsUtil.systemDefaultToUnits(we.WomenRx.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS) + Affine.Utils.UnitsUtil.unitToStringName(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    }

                    lWorkoutInfo.Text +=  we.Reps + " X " + we.Exercise.Name + (!string.IsNullOrWhiteSpace(additional) ? " (" + additional + ")" : "") + (!string.IsNullOrWhiteSpace(we.Notes) ? " Notes: " + we.Notes : "") + "<br />";
                    lWorkoutInfo.Text += "</span>";
                }
            }
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                aqufitEntities entities = new aqufitEntities();
                switch (hiddenAjaxAction.Value)
                {
                    case "addWorkout":
                        // few things to do here... 
                        // 1) check to see if this route is already in the fav.
                        long wid = Convert.ToInt64(hiddenAjaxValue.Value);
                        User2WODFav check = entities.User2WODFav.FirstOrDefault(w => w.UserSetting.Id == this.UserSettings.Id && w.WOD.Id == wid);
                        if (check != null)
                        {
                            // TODO: postback a message saying that the route is already there.
                        //    status = "Route already in list.  TODO: dialog with a way to 'view my routes' | 'record workout for route'";
                        }
                        else
                        {   // add the WOD.
                            WOD wod = entities.WODs.First(w => w.Id == wid);
                            UserSettings us = entities.UserSettings.First( u => u.Id == UserSettings.Id );
                            User2WODFav fav = new User2WODFav()
                            {
                                UserSetting = us,
                                WOD = wod
                            };
                            entities.AddToUser2WODFav(fav);
                            entities.SaveChanges();
                            //TODO: dialog with a way to 'view my workouts' | 'record this workout'";                                                        
                        }
                        // TODO: should not see this for when the check is null above..
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.WorkoutAddedDialog.open(); ");
                        break;
                    case "remWorkout":
                        long remid = Convert.ToInt64(hiddenAjaxValue.Value);
                        User2WODFav toRem = entities.User2WODFav.FirstOrDefault(w => w.UserSetting.Id == this.UserSettings.Id && w.WOD.Id == remid);
                        if (toRem != null)
                        {
                            // remove the route from fav... any workouts will still be logged though.
                            entities.DeleteObject(toRem);
                            entities.SaveChanges();
                        }                       
                        break;
                    case "delStream":
                        long sid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteStream(UserSettings, sid);
                        break;
                    case "delComment":
                        long cid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteComment(UserSettings, cid);
                        break;
                    case "AddSuggestFriend":
                        try
                        {
                            long usid = Convert.ToInt64(hiddenAjaxValue.Value);
                            string status = Affine.Data.Managers.LINQ.DataManager.Instance.sendFriendRequest(UserSettings.Id, usid);
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
                // TODO: better error handling
                RadAjaxManager1.ResponseScripts.Add( " alert('" + ex.Message + "');");
            }
        }


        protected void atiRadComboBoxSearchWorkouts_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboSearchWorkouts = (RadComboBox)sender;
            atiRadComboSearchWorkouts.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<WOD> wods = null;
            if (UserSettings != null && Settings["Configure"] != null && string.Compare( Convert.ToString( Settings["Configure"] ), "ConfigureMyWorkouts", true) == 0)
            {
                wods = entities.User2WODFav.Where(w => w.UserSetting.Id == UserSettings.Id).Select(w => w.WOD);
                wods = wods.Union(entities.WODs.Where(w => w.Standard > 0));
                wods = wods.OrderBy(w => w.Name);
            }
            else
            {
                wods = entities.WODs;
                wods = wods.OrderBy(w => w.Name);
            }
            int length = wods.Count();
            wods = string.IsNullOrEmpty(e.Text) ? wods.Skip(itemOffset).Take(TAKE) : wods.Where(w => w.Name.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            WOD[] wodArray = wods.ToArray();

            foreach (WOD w in wodArray)
            {
                RadComboBoxItem item = new RadComboBoxItem(w.Name);
                item.Value = "" + w.Id;
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

