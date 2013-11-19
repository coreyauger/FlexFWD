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

namespace Affine.Dnn.Modules.ATI_Groups
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
    partial class ViewATI_Groups : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
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

                    imgSearch.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iSearch.png");
                    if (GroupSettings == null)
                    {       // Let people search for a group in this case..
                        atiGroupListPanel.Visible = true;
                        atiGroupProfile.Visible = false;

                        if (this.UserSettings == null)
                        {
                            pageMyGroups.Visible = false;
                            tabMyGroups.Visible = false;
                        }

                        if (this.UserSettings != null && (this.UserSettings.LatHome != null && this.UserSettings.LatHome.Value > 0.0))
                        {
                            atiGMap.Lat = this.UserSettings.LatHome.Value;
                            atiGMap.Lng = this.UserSettings.LngHome.Value;
                            atiGMap.Zoom = 13;
                        }
                        imgAd.Src = ResolveUrl("~/portals/0/images/adTastyPaleo.jpg");
                        WebService.StreamService streamService = new WebService.StreamService();
                        if (this.UserSettings != null)
                        {   // TODO: need to hide the "My Group" section
                            string json = streamService.getGroupListData(this.UserSettings.Id, 0, 25);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "MyGroups", "$(function(){ Aqufit.Page.atiMyGroupList.generateStreamDom('" + json + "'); });", true);
                        }
                       // string search = streamService.searchGroupListData(PortalId, null, 0, 15);

                        atiMyGroupList.IsOwner = true;

                        SetupFeaturedGroups();

                        // we need to setup for a location based group search                    
                       // ScriptManager.RegisterStartupScript(this, Page.GetType(), "GroupSearch", "$(function(){ Aqufit.Page.atiGroupSearch.generateStreamDom('" + search + "'); });", true);
                    }
                    else
                    {
                        atiWebLinksList.ProfileSettings = GroupSettings;
                        aHistory.HRef = baseUrl + GroupSettings.UserName + "/workout-history";
                        aLeaders.HRef = baseUrl + GroupSettings.UserName + "/achievements";
                        aMembers.HRef = baseUrl + GroupSettings.UserName + "/friends";
                        atiProfile.ProfileSettings = GroupSettings;
                        this.ProfileCSS = GroupSettings.CssStyle;
                        atiWorkoutScheduler.ProfileSettings = GroupSettings;
                       
                        litGroupName.Text = "<h2>" + GroupSettings.UserFirstName + "</h2>";
                        imgAdRight.Src = ResolveUrl("/portals/0/images/adTastyPaleo.jpg");
                        LoadChartData();

                        if (Request["w"] != null)
                        {
                            RadAjaxManager1.ResponseScripts.Add("$(function(){ Aqufit.Page.Tabs.SwitchTab(1); });");
                            long wId = Convert.ToInt64(Request["w"]);
                            WOD wod = entities.WODs.Include("WODType").FirstOrDefault(w => w.Id == wId);
                            if (wod != null)
                            {
                                atiWorkoutScheduler.SetControlToWOD = wod;
                            }
                        }

                        long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.GroupSettings.Id || f.DestUserSettingKey == this.GroupSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(f => (f.SrcUserSettingKey == this.GroupSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();
                        IQueryable<Affine.Data.User> friends = entities.UserSettings.OfType<User>().Where(LinqUtils.BuildContainsExpression<User, long>(s => s.Id, friendIds)).OrderBy(s => s.Id);
                        int fcount = friends.Count();
                        UserSettings[] firendSettings = null;
                        if (fcount > 6)
                        {
                            Random rand = new Random((int)DateTime.Now.Millisecond);
                            int skip = rand.Next(fcount - 6);
                            firendSettings = friends.Skip(skip).Take(6).ToArray();
                        }
                        else
                        {
                            firendSettings = friends.Take(6).ToArray();
                        }
                        // PERMISSIONS: The action panel is only visible to OWNERS                     
                        if (GroupPermissions == ConstsUtil.Relationships.GROUP_OWNER || GroupPermissions == ConstsUtil.Relationships.GROUP_ADMIN)  // Need to find if user is an admin
                        {
                            atiProfile.IsOwner = true;
                            tabWorkout.Visible = true;
                            pageScheduleWOD.Visible = true;
                            bJoinGroup.Visible = false;     // for now owners can never leave the group ... mu ha ha ha
                            RadAjaxManager1.AjaxSettings.AddAjaxSetting(atiCommentPanel, atiCommentPanel, RadAjaxLoadingPanel1);
                        }
                        else if (GroupPermissions == ConstsUtil.Relationships.GROUP_MEMBER)
                        {
                            bJoinGroup.Text = "Leave Group";
                            RadAjaxManager1.AjaxSettings.AddAjaxSetting(atiCommentPanel, atiCommentPanel, RadAjaxLoadingPanel1);
                            RadAjaxManager1.AjaxSettings.AddAjaxSetting(bJoinGroup, bJoinGroup, RadAjaxLoadingPanel1);
                        }
                        else
                        {
                            tabComment.Visible = false;
                            pageViewComment.Visible = false;
                            RadAjaxManager1.AjaxSettings.AddAjaxSetting(bJoinGroup, bJoinGroup, RadAjaxLoadingPanel1);
                        }                      
                        // settup the users web links                    
                        this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?us=" + GroupSettings.Id + "&bg=1";
                        this.ProfileCSS = GroupSettings.CssStyle;                         
                        
                    }
                }
                
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }         
        }

        private void SetupFeaturedGroups()
        {
            aqufitEntities entities = new aqufitEntities();
            // CA - We have encoded the number of members in a Groups::MainGroupKey ... this is an easy way to pick out featured groups..
            IQueryable<Group> featureQuery = entities.UserSettings.OfType<Group>().Include("Image").Include("Places").Where(g => g.MainGroupKey > 10 );
            int gcount = featureQuery.Count();
            int skip = new Random().Next(gcount-1);
            Group feature = featureQuery.OrderBy( g => g.Id ).Skip(skip).First();
            atiFGProfileImg.Settings = feature;
            hrefGroupName.HRef  = hrefGroupLink2.HRef = ResolveUrl("~/") + "Group/" + feature.UserName;
            hrefGroupName.InnerHtml = feature.UserFirstName;

            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
            Affine.Data.json.LeaderBoardWOD[] all = dataMan.CalculatCrossFitLeaderBoard(feature.Id);
            System.Web.Script.Serialization.JavaScriptSerializer jsSerial = new System.Web.Script.Serialization.JavaScriptSerializer();
            RadAjaxManager1.ResponseScripts.Add("$(function(){ Aqufit.Page." + atiLeaderBoard2.ID + ".loadLeaderBoardFromJson('" + jsSerial.Serialize(all) + "'); });");

            // TODO: cache this grabbing of the friend ids from the stream service
            long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == feature.Id || f.DestUserSettingKey == feature.Id) && (f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER)).Select(f => (f.SrcUserSettingKey == feature.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();
            IQueryable<Affine.Data.User> friends = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<User, long>(s => s.Id, friendIds)).OrderBy(s => s.Id);
            int fcount = friends.Count();
            UserSettings[] firendSettings = null;
            if (fcount > 6)
            {
                Random rand = new Random((int)DateTime.Now.Millisecond);
                skip = rand.Next(fcount - 6);
                firendSettings = friends.Skip(skip).Take(6).ToArray();
            }
            else
            {
                firendSettings = friends.Take(6).ToArray();
            }            
            atiFriendsPhotos.FriendKeyList = firendSettings;
            atiFriendsPhotos.User = feature;
            atiFriendsPhotos.FriendCount = fcount;
            atiFriendsPhotos.FriendListLink = ResolveUrl("~/") + feature.UserName + "/Friends";

            if( feature.Places.Count > 0 ){
                Place place = feature.Places.FirstOrDefault();
                litGroupDescription.Text += "<span>" + place.Street + "<br />"+place.City+", "+place.Region+", "+place.Country+"</span>";
                litGroupDescription.Text += "<span>"+place.Description+"</span>";
            }            
            WebService.StreamService streamService = new WebService.StreamService();            
            string json = streamService.getActiveGroups(0, 10);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "Groups", "$(function(){ Aqufit.Page.atiGroupList.generateStreamDom('" + json + "'); });", true);                        
        }


        private void LoadChartData()
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            // need a list of people in the group
            aqufitEntities entities = new aqufitEntities();
            long[] memberIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == GroupSettings.Id || f.DestUserSettingKey == GroupSettings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER ).Select(f => f.SrcUserSettingKey == GroupSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            // How many members are there in the group.. this will determin how far back in time we will look for workouts
            int numMembers = memberIds.Length;
            // For now we will always look over the last 7 days..
            DateTime today = DateTime.Today;
            DateTime past = today.AddDays(-7);

            IQueryable<UserStream> streamSet = entities.UserStreamSet
                                        .Include("UserSetting").Include("WOD")
                                        .Where(w => w.Date.CompareTo(past) > 0)
                                        .Where(LinqUtils.BuildContainsExpression<UserStream, long>(w => w.UserSetting.Id, memberIds));          
            // TODO: we need to get a better idea of who is going to need hydrating before we do it.

            streamSet.Select(w => w.UserSetting).ToArray();     // THIS IS BAD.. Hydrating a lot of users profiles..
            IQueryable<IGrouping<long?, Workout>> groupWorkouts = streamSet.OfType<Workout>().GroupBy(w => w.WorkoutTypeKey);
            // Lets find out what this group is all about ... IE what type of workouts dominate
            long ukey = -1;
            long key = -1;
            int usize = 0;
            int size = 0;
            foreach (IGrouping<long?, Workout> g in groupWorkouts)
            {
                UserSettings us = g.Select(w => w.UserSetting).First();
                // first see if we can find anything that the current user is in
                if (UserSettings != null && g.FirstOrDefault(w => (long)w.UserSettingReference.EntityKey.EntityKeyValues[0].Value == UserSettings.Id) != null)
                {
                    int c = g.Count();
                    if ( c > size)
                    {
                        ukey = g.Key.Value;
                        usize = c;
                    }
                }else{
                    int c = g.Count();
                    if ( c > size)
                    {
                        key = g.Key.Value;
                        size = c;
                    }
                }
            }   
            if( ukey > 0 ){ // we found something that this user is in
                key = ukey;
                size = usize;
            }
            IGrouping<long?, Workout> workouts = groupWorkouts.Where(g => g.Key == key).FirstOrDefault();
            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
            Affine.Data.json.LeaderBoardWOD[] leaderBoard = dataMan.CalculatCrossFitLeaderBoard(base.GroupSettings.Id);
            atiLeaderBoard.Visible = true;


            WODSchedule scheduled = entities.WODSchedules.Include("WOD").Where(w => w.UserSetting.Id == GroupSettings.Id).OrderByDescending(w => w.Id).FirstOrDefault();
            litTodaysWOD.Text = "<h3>Last Scheduled Workout:";
            if (scheduled != null)
            {
                WOD wod = scheduled.WOD;
                litTodaysWOD.Text += " <a style=\"font-size: 16px; color: #0095cd; font-weight: bold;\" href=\"/workouts/" + wod.Id + "\">" + wod.Name + "</a></h3>";
                Affine.WebService.StreamService ss = new WebService.StreamService();
                string jsonEveryone = ss.getStreamDataForWOD(wod.Id, -1, 0, 25, true, true, -1, -1, -1);

                string jsonYou = string.Empty;
                string js = string.Empty;
                js += " Aqufit.Page." + atiWorkoutHighChart.ID + ".fromStreamData('" + jsonEveryone + "'); ";
                if (base.UserSettings != null)
                {
                    jsonYou = ss.getStreamDataForWOD(wod.Id, base.UserSettings.Id, 0, 10, true, true, -1, -1, -1);
                    js += " Aqufit.Page." + atiWorkoutHighChart.ID + ".fromYourStreamData('" + jsonYou + "'); ";
                }


                js += " Aqufit.Page." + atiWorkoutHighChart.ID + ".drawChart(); ";

                RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page." + atiLeaderBoard.ID + ".loadLeaderBoardFromJson('" + jsSerializer.Serialize(leaderBoard) + "'); " + js + " });");
            }
            else
            {
                litTodaysWOD.Text += " <em>Unavailble</em></h3>";
                atiWorkoutHighChart.Visible = false;
            }
            // so now KEY is the most (type) of workout
            // There are now a couple special cases... (crossfit workouts)
         /*
            if (key == (int)Utils.WorkoutUtil.WorkoutType.CROSSFIT)
            {
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                Affine.Data.json.LeaderBoardWOD[] females = dataMan.CalculatCrossFitLeaderBoard(base.GroupSettings.Id, "F");
                Affine.Data.json.LeaderBoardWOD[] males = dataMan.CalculatCrossFitLeaderBoard(base.GroupSettings.Id, "M");
                atiLeaderBoardMale.Visible = true;
                atiLeaderBoardMale.LeaderWODList = males;
                atiLeaderBoardFemale.Visible = true;
                atiLeaderBoardFemale.LeaderWODList = females;
            }
            else
            {
                if (workouts != null)
                {
                    // TODO: now for some reason we can not hydrate the "UserSettings" so we need to get them now
                    long[] ids = workouts.Select(w => (long)w.UserSettingReference.EntityKey.EntityKeyValues[0].Value).ToArray();

                    // for now the other types of workout are just a quick grab...

                }
            }
          */
          //  ScriptManager.RegisterStartupScript(this, Page.GetType(), "wt", "alert('" +  jsSerializer.Serialize( title ) + "');", true);
        }

        protected void bJoinGroup_Click(object sender, EventArgs e)
        {
            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
            string status = string.Empty;
            // if the person is in the group... then this request is to be removed..
            if (GroupPermissions >= Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER)
            {
                dataMan.LeaveGroup(UserSettings.Id, GroupSettings.Id);
                status = "You have been successfully removed from the group: \"" + GroupSettings.UserName + "\"";
                bJoinGroup.Text = "Join Group";
            }
            else if (GroupPermissions < Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER )
            {
                dataMan.JoinGroup(UserSettings.Id, GroupSettings.Id, Affine.Utils.ConstsUtil.Relationships.GROUP_MEMBER);
                status = "You have been successfully added to the group: \"" + GroupSettings.UserName + "\"";
                bJoinGroup.Text = "Leave Group";                
            }
            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowSuccess('" + status + "'); UpdateStatus('" + status + "'); $('#" + bJoinGroup.ClientID + "').button();  ");
        }


        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {
                    case "delStream":
                        long sid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.deleteStream(UserSettings, sid);
                        break;
                    case "delComment":
                        long cid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.deleteComment(UserSettings, cid);
                        break;
                    case "scheduleWOD":                        
                        dataMan.ScheduleGroupWOD(UserSettings.Id, GroupSettings.Id, atiWorkoutScheduler.WODId, atiWorkoutScheduler.Date, atiWorkoutScheduler.HideDate, atiWorkoutScheduler.HiddenName, atiWorkoutScheduler.Notes);
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowSuccess('Workout has been scheduled for all members.');");
                        break;                    
                }
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('ERROR: There was a problem with the action (" + ex.Message.Replace("'","").Replace("\r","").Replace("\n","") + ")');");
            }
           // RadAjaxManager1.ResponseScripts.Add(" UpdateStatus('" + status + "'); ");
        }

        protected void bSubmitComment_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string status = string.Empty;
                try
                {
                    Affine.WebService.StreamService service = new WebService.StreamService();
                    RadAjaxManager1.ResponseScripts.Add(" (function(){ Aqufit.Page.atiStreamScript.prependJson('" + service.SaveStreamShout(this.GroupSettings.Id, this.UserId, this.PortalId, atiComment.Comment) + "'); })();");
                    status = "Your comment has been saved.";
                }
                catch (Exception ex)
                {
                    status = "ERROR: problem saving comment (" + ex.Message + ")";
                }
                RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
            }
        }

        protected void atiRadComboBoxSearchGroups_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboBoxSearchGroups = (RadComboBox)sender;
            atiRadComboBoxSearchGroups.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.UserSettings.Id || f.DestUserSettingKey == this.UserSettings.Id)).Select(f => f.SrcUserSettingKey == this.UserSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            int itemOffset = e.NumberOfItems;
            IQueryable<Group> friends = entities.UserSettings.OfType<Group>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<Group, long>(w => w.Id, friendIds)).OrderBy(w => w.UserName);
            int length = friends.Count();
            friends = string.IsNullOrEmpty(e.Text) ? friends.Skip(itemOffset).Take(TAKE) : friends.Where(w => w.UserName.ToLower().StartsWith(e.Text) || w.UserFirstName.ToLower().StartsWith(e.Text) || w.UserLastName.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            Group[] groups = friends.ToArray();

            foreach (Group g in groups)
            {
                RadComboBoxItem item = new RadComboBoxItem(g.UserName);
                item.Value = "" + g.UserName;
                item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                atiRadComboBoxSearchGroups.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
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
            IQueryable<WOD> wods = entities.User2WODFav.Where(w => w.UserSetting.Id == GroupSettings.Id || w.UserSetting.Id == UserSettings.Id).Select(w => w.WOD);
            wods = wods.Union<WOD>(entities.WODs.Where(w => w.Standard > 0));
            wods.Select(w => w.WODType).ToArray();  // hydrate WODTypes
            wods = wods.OrderByDescending(w => w.CreationDate);
            wods = string.IsNullOrEmpty(e.Text) ? wods.OrderByDescending(w => w.CreationDate) : wods.Where(w => w.Name.ToLower().StartsWith(e.Text)).OrderBy(w => w.Name);
            int length = wods.Count();
            wods = wods.Skip(itemOffset).Take(TAKE);
            WOD[] wodList = wods.ToArray();
            int endOffset = Math.Min(itemOffset + TAKE, length);
            e.EndOfItems = endOffset == length;
            for (int i = 0; i < wodList.Length; i++)
            {
                atiRadComboBoxCrossfitWorkouts.Items.Add(new RadComboBoxItem(wodList[i].Name, "{ 'Id':" + wodList[i].Id + ", 'Type':" + wodList[i].WODType.Id + "}"));
            }
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
            //    Actions.Add(this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), ModuleActionType.AddContent, "", "", this.EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }
}

