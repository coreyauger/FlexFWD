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

namespace Affine.Dnn.Modules.ATI_CompRegister
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
    partial class ViewATI_CompRegister : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
    {

        #region Private Members    
        
        private string baseUrl;
        private string userPhotoPath;
        private string urlPath;
        private readonly long CROSSFIT_COM_ID = 516;
        #endregion       

        #region Public Methods
        
        #endregion        

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
        //    Page.Title = "FlexFWD: " + (this.ProfileSettings != null ? this.ProfileSettings.UserName : "" );
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
                RadAjaxManager1.AjaxSettings.AddAjaxSetting(panelAjax, panelAjax, RadAjaxLoadingPanel2);
                atiProfileImage.Settings = base.UserSettings;                

                imgCheck.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
                ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                service.InlineScript = true;
                ScriptManager.GetCurrent(Page).Services.Add(service);

                atiGMap.Lat = UserSettings.LatHome;
                atiGMap.Lng = UserSettings.LngHome;
                atiGMap.Zoom = 10;
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    aqufitEntities entities = new aqufitEntities();
                    // get all the users group ids 
                    long[] groupIdArray = entities.UserFriends.Where(g => (g.DestUserSettingKey == UserSettings.Id || g.SrcUserSettingKey == UserSettings.Id) && g.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(g => (g.DestUserSettingKey == UserSettings.Id ? g.SrcUserSettingKey : g.DestUserSettingKey)).ToArray();
                    string groupKeyString = new string(groupIdArray.SelectMany(s => Convert.ToString( s ) + ",").ToArray());
                    if (!string.IsNullOrWhiteSpace(groupKeyString))
                    {
                        groupKeyString = groupKeyString.Substring(0, groupKeyString.Length - 1);
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Groups = new Array(" + groupKeyString + "); }); "); 
                    }
                    if ( UserSettings.MainGroupKey.HasValue && UserSettings.MainGroupKey.Value > 1)
                    {
                        RadAjaxManager1.ResponseScripts.Add("$('#panelFindAffiliate').hide();");
                        Group homeGroup = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == UserSettings.MainGroupKey.Value);
                        litAffiliateName.Text = homeGroup.UserFirstName;
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Actions.selectedPlace = {'Address': '', 'GroupKey':" + homeGroup.Id + ", 'Lat':" + homeGroup.DefaultMapLat + ", 'Lng':" + homeGroup.DefaultMapLng + " , 'Name':'" + homeGroup.UserFirstName + "', 'UserName':'" + homeGroup.UserName.Replace("'", "") + "', 'UserKey':" + homeGroup.UserKey + ", 'ImageId':0, 'Description':''}; }); ");
                        bNotMyAffiliate.Visible = true;
                    }
                    // fill in know form fields
                    atiSlimControl.IsEditMode = true;
                    atiSlimControl.FullName = UserSettings.UserFirstName + " " + UserSettings.UserLastName;
                    atiSlimControl.Email = UserSettings.UserEmail;
                    txtTeamEmail.Text = UserSettings.UserEmail;
                    atiSlimControl.ShowPostal = false;
                    atiBodyComp.IsEditMode = true;
                    atiBodyComp.Gender = UserSettings.Sex;
                    txtTeamCaptinName.Text = atiSlimControl.FullName;
                    txtTeamCaptinEmail.Text = atiSlimControl.Email;
                    if (UserSettings.BirthDate.HasValue)
                    {
                        atiBodyComp.BirthDate = UserSettings.BirthDate.Value;
                    }
                    // setup known times for Standard WODS
                    Workout fran = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.FRAN_ID && w.IsBest == true && w.Duration.HasValue);
                    if (fran != null)
                    {
                        atiTimeSpanFran.Time = fran.Duration.Value;
                        hiddenFranId.Value = "" + fran.Id;
                    }
                    Workout helen = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.HELEN_ID && w.IsBest == true && w.Duration.HasValue);
                    if (helen != null)
                    {
                        atiTimeSpanHelen.Time = helen.Duration.Value;
                        hiddenHelenKey.Value = "" + helen.Id;
                    }
                    Workout grace = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.GRACE_ID && w.IsBest == true && w.Duration.HasValue);
                    if (grace != null)
                    {
                        atiTimeSpanGrace.Time = grace.Duration.Value;
                        hiddenGraceKey.Value = "" + grace.Id;
                    }
                    Workout ff = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.FILTHY50_ID && w.IsBest == true && w.Duration.HasValue);
                    if (ff != null)
                    {
                        atiTimeSpanFilthyFifty.Time = ff.Duration.Value;
                        hiddenFilthyFiftyKey.Value = "" + ff.Id;
                    }
                    Workout fgb = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.FGB_ID && w.IsBest == true && w.Score.HasValue);
                    if (fgb != null)
                    {
                        txtFGB.Text = "" + fgb.Score.Value;
                        hiddenFGBKey.Value = "" + fgb.Id;
                    }

                    Workout maxPullups = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.MAX_PULLUPS_ID && w.IsBest == true && w.Score.HasValue);
                    if (maxPullups != null)
                    {
                        txtMaxPullups.Text = "" + maxPullups.Score.Value;
                        hiddenMaxPullupKey.Value = "" + maxPullups.Id;
                    }

                    atiMaxBSUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    atiMaxBSUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                    atiMaxBSUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                    Workout maxBS = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.MAX_BACK_SQUAT_ID && w.IsBest == true && w.Max.HasValue);
                    if (maxBS != null)
                    {
                        txtMaxBS.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits( maxBS.Max.Value, UnitsUtil.MeasureUnit.UNIT_LBS );
                        hiddenMaxBackSquatKey.Value = "" + maxBS.Id;
                    }

                    atiMaxCleanUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    atiMaxCleanUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                    atiMaxCleanUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                    Workout maxClean = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.MAX_CLEAN_ID && w.IsBest == true && w.Max.HasValue);
                    if (maxClean != null)
                    {
                        txtMaxClean.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits(maxClean.Max.Value, UnitsUtil.MeasureUnit.UNIT_LBS);
                        hiddenMaxCleanKey.Value = "" + maxClean.Id;
                    }

                    atiMaxDeadliftUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    atiMaxDeadliftUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                    atiMaxDeadliftUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                    Workout maxDead = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.MAX_DEADLIFT_ID && w.IsBest == true && w.Max.HasValue);
                    if (maxDead != null)
                    {
                        txtMaxDeadlift.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits(maxDead.Max.Value, UnitsUtil.MeasureUnit.UNIT_LBS);
                        hiddenMaxDeadliftKey.Value = "" + maxDead.Id;
                    }

                    atiMaxSnatchUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    atiMaxSnatchUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
                    atiMaxSnatchUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                    Workout maxSnatch = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.UserSetting.Id == UserSettings.Id && w.WOD.Id == Affine.Utils.ConstsUtil.MAX_SNATCH_ID && w.IsBest == true && w.Max.HasValue);
                    if (maxSnatch != null)
                    {
                        txtMaxSnatch.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits(maxSnatch.Max.Value, UnitsUtil.MeasureUnit.UNIT_LBS);
                        hiddenMaxSnatchKey.Value = "" + maxSnatch.Id;
                    }

                    if (Request["step"] != null)
                    {
                        atiSidePanel.Visible = false;
                        atiMainPanel.Style["width"] = "100%";
                        atiGMap.Width = Unit.Pixel(605);
                        // check if we may have info on this person from the 2011 crossfit open.                                                                       
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Actions.LoadStep(" + Request["step"] + "); }); ");                        
                    }                    
                    this.baseUrl = ResolveUrl("~/");   
                    userPhotoPath = this.baseUrl + @"\Portals\0\Users\" + base.UserSettings.UserName;
                    urlPath = "/Portals/0/Users/" + base.UserSettings.UserName;                                        
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
           
        }

        protected void atiRadComboBoxSearchGroups_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboBoxSearchGroups = (RadComboBox)sender;
            atiRadComboBoxSearchGroups.Items.Clear();
            const int TAKE = 15;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<Group> friends = entities.UserSettings.OfType<Group>().OrderBy(w => w.UserName);      
            if( !string.IsNullOrEmpty(e.Text) ){
                friends = friends.Where(w => w.UserName.ToLower().Contains(e.Text) || w.UserFirstName.ToLower().Contains(e.Text));
            }
            int length = friends.Count();
            friends = friends.Skip(itemOffset).Take(TAKE);
            Group[] groups = friends.ToArray();

            foreach (Group g in groups)
            {
                RadComboBoxItem item = new RadComboBoxItem(g.UserFirstName);
                item.Value = " { 'Address': '', 'GroupKey':" + g.Id + ", 'Lat':" + g.DefaultMapLat + ", 'Lng':" + g.DefaultMapLng + " , 'Name':'" + g.UserFirstName + "', 'UserName':'" + g.UserName.Replace("'","") + "', 'UserKey':" + g.UserKey + ", 'ImageId':0, 'Description':'' }";
             //   item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                atiRadComboBoxSearchGroups.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }



        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                aqufitEntities entities = new aqufitEntities();                        
                switch (hiddenAjaxAction.Value)
                {
                    case "saveRegistration":
                        CompetitionRegistration registration = null;
                        if (string.IsNullOrWhiteSpace(hiddenAjaxRegistrationId.Value))
                        {
                            registration = new CompetitionRegistration();
                        }
                        else
                        {
                            long rId = Convert.ToInt64(hiddenAjaxRegistrationId.Value);
                            registration = entities.CompetitionRegistrations.FirstOrDefault(r => r.Id == rId);
                        }
                        registration.Competition = entities.Competitions.FirstOrDefault( c => c.Id == 1 ); // ********** TMP
                        User test = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == UserSettings.Id);
                        registration.UserSetting = test;
                        // we have a chance to correct user "miss information"
                        int compType = Convert.ToInt32( ddlCompetitionType.SelectedValue );
                        registration.RegistrationType = compType;
                        registration.DateTime = DateTime.Now;
                        if (!string.IsNullOrWhiteSpace(hiddenAjaxValue.Value))
                        {
                            long affilId = Convert.ToInt64(hiddenAjaxValue.Value);
                            registration.GroupAffiliate = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == affilId);
                        }
                        if (compType == 1)
                        {
                            if (atiSlimControl.Email != test.UserEmail) test.UserEmail = atiSlimControl.Email;
                            if (atiBodyComp.Gender != test.Sex) test.Sex = atiBodyComp.Gender;
                            if (!test.BirthDate.HasValue || atiBodyComp.BirthDate != test.BirthDate.Value) test.BirthDate = atiBodyComp.BirthDate;
                            registration.RegistrationType = 1;  // individual
                            registration.MailingAddress = txtAddress.Text;
                            registration.ContactPhone = txtPhone.Text;
                            registration.ContactEmail = test.UserEmail;
                            registration.EmergContactName = txtEmergName.Text;
                            registration.EmergContactPhone = txtEmergPhone.Text;
                            registration.EmergContactRelation = txtEmergRelation.Text;
                            registration.ExtraInfo = txtMedical.Text;
                            // save and load the new registration ID
                            entities.SaveChanges();
                            registration = entities.CompetitionRegistrations.OrderByDescending(r => r.Id).FirstOrDefault(r => r.UserSetting.Id == UserSettings.Id);
                            hiddenAjaxRegistrationId.Value = "" + registration.Id;

                            // now we need to add the users PR's to their profiles...
                            SaveCompWorkoutPR();
                        }
                        else
                        {   // team registration...
                            registration.MailingAddress = txtTeamAddress.Text;
                            registration.ContactPhone = txtTeamPhone.Text;
                            registration.TeamName = txtTeamName.Text;
                            registration.ContactEmail = txtTeamEmail.Text;
                            registration.RegistrationType = 2;  // team

                            // save and load the new registration ID
                            entities.SaveChanges();
                            registration = entities.CompetitionRegistrations.OrderByDescending(r => r.Id).FirstOrDefault(r => r.UserSetting.Id == UserSettings.Id);
                            hiddenAjaxRegistrationId.Value = "" + registration.Id;
                        }                                                                    
                        
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.Actions.LoadStep(6); "); 
                        break;
                    case "checkTeamLimit":
                        long gId = Convert.ToInt64(hiddenAjaxValue.Value);                       
                        int count = entities.CompetitionRegistrations.Where(r => r.GroupAffiliate.Id == gId && r.RegistrationType == 2 ).Count(); // more the 2 affiliate teams alreay ??
                        if (count >= 2)
                        {
                            Group check = entities.UserSettings.OfType<Group>().FirstOrDefault( g => g.Id == gId );
                            RadAjaxManager1.ResponseScripts.Add(" alert('Sorry the affiliate [ " + check.UserFirstName + " ] already has 2 teams signed up.' );" );
                        }
                        else
                        {
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.Actions.LoadStep(3);");
                        }
                        break;
                    case "connectCompAthlete":
                        long aid = Convert.ToInt64(hiddenAjaxValue.Value);
                        CompetitionAthlete athlete = entities.CompetitionAthletes.Include("CompetitionWODs").Include("CompetitionWODs.WOD").Include("CompetitionWODs.WOD.WODType").Include("CompetitionAffiliate").Include("CompetitionAffiliate.CFAffiliate").Include("CompetitionAffiliate.CFAffiliate.UserSetting").FirstOrDefault(a => a.Id == aid);
                        // first lets get any info we can off the profile (weight, height)
                        User thisUser = entities.UserSettings.OfType<User>().Include("BodyCompositions").FirstOrDefault( u => u.Id == UserSettings.Id );
                        BodyComposition bc = thisUser.BodyCompositions.FirstOrDefault();
                        if (bc == null)
                        {
                            bc = new BodyComposition();
                            bc.UserSetting = thisUser;
                            entities.AddToBodyComposition(bc);
                        }
                        bc.Height = athlete.Height;
                        bc.Weight = athlete.Weight;  
                        entities.SaveChanges();
                        bool loadImg = true;
                        try
                        {
                            Utils.ImageUtil.MakeImageProfilePic(Utils.ImageUtil.ReadImageDataFromUrl(athlete.ImgUrl), UserSettings.Id);
                        }
                        catch (Exception)
                        {
                            loadImg = false;
                        }
                        // next see if we can add them to any groups (via the affiliate)
                        if (athlete.CompetitionAffiliate != null && athlete.CompetitionAffiliate.CFAffiliate != null && athlete.CompetitionAffiliate.CFAffiliate.UserSetting != null)
                        {
                            dataMan.JoinGroup(thisUser.Id, athlete.CompetitionAffiliate.CFAffiliate.UserSetting.Id, ConstsUtil.Relationships.GROUP_MEMBER);
                        }
                        // now add all the games workouts to the site.
                        foreach (CompetitionWOD cw in athlete.CompetitionWODs)
                        {
                            // so far we think they are all AMRAPS                            
                            dataMan.SaveWorkout(thisUser, (int)Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "", true, cw.WOD.Id, cw.Score, null);
                        }
                        if (loadImg)
                        {
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.Actions.LoadStep(2); ");
                        }
                        else
                        {
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.Actions.LoadStep(1); ");
                        }
                        break;
                    case "friendRequest":
                        long fid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.sendFriendRequest(UserSettings.Id, fid);
                        status = "Friend request has been sent.";
                        break;
                    case "joinGroup":                        
                        long gid = Convert.ToInt64(hiddenAjaxValue.Value);
                        bool isOwner = hiddenAjaxValue2.Value == "true" ? true : false;
                        bool sendRequestToAll = hiddenAjaxValue3.Value == "1" ? true : false;
                        dataMan.JoinGroup(UserSettings.Id, gid, ConstsUtil.Relationships.GROUP_MEMBER);
                        Group group = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == gid);
                        if (sendRequestToAll)
                        {                            
                            long[] memberIdArray = entities.UserFriends.Where(g => (g.DestUserSettingKey == gid || g.SrcUserSettingKey == gid) && g.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER).Select(g => (g.DestUserSettingKey == gid ? g.SrcUserSettingKey : g.DestUserSettingKey)).ToArray();
                            int numMembers = memberIdArray.Length;                            
                            foreach (long mid in memberIdArray)
                            {
                                dataMan.sendFriendRequest(UserSettings.Id, mid);
                            }
                        }
                        if (isOwner)
                        {
                            dataMan.makeGroupOwnerRequest(UserSettings as User, group);
                        }
                        if (Request["step"] != null)
                        {   // todo: store this owne                      
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.JoinGroupDialog.close(); Aqufit.Page.Actions.LoadStep(4); ");
                        }
                        else
                        {
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.JoinGroupDialog.close(); Aqufit.Windows.SuccessDialog.open(\"{'html':'You have been added to the group.'}\");");
                        }                        
                        break;                   
                }
            }
            catch (Exception ex)
            {
                status = "ERROR: There was a problem with the action (" + ex.Message + ex.StackTrace.Replace("'","").Replace("\n","")+")";
                RadAjaxManager1.ResponseScripts.Add("alert('" + status + "'); ");
            }
            
        }       


        #endregion


        private void SaveCompWorkoutPR()
        {
            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
            aqufitEntities entities = new aqufitEntities();
            int exCount = 0;
            try
            {
                if (atiTimeSpanFran.Time > 0)
                {   // houston we have a fran time...
                    bool addFran = true;
                    if (!string.IsNullOrWhiteSpace(hiddenFranId.Value))
                    {
                        long wId = Convert.ToInt64(hiddenFranId.Value);
                        Workout fran = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (fran.Duration.Value == atiTimeSpanFran.Time) addFran = false;
                    }
                    if (addFran)
                    {
                        WOD fran = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.FRAN_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, atiTimeSpanFran.Time, "Entered PR for Competition data", true, fran.Id, null, null);
                    }
                }
            }
            catch (Exception) { exCount++;  }

            try
            {
                if (atiTimeSpanHelen.Time > 0)
                {   // houston we have a Helen time...
                    bool addHelen = true;
                    if (!string.IsNullOrWhiteSpace(hiddenHelenKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenHelenKey.Value);
                        Workout Helen = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (Helen.Duration.Value == atiTimeSpanHelen.Time) addHelen = false;
                    }
                    if (addHelen)
                    {
                        WOD Helen = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.HELEN_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, atiTimeSpanHelen.Time, "Entered PR for Competition data", true, Helen.Id, null, null);
                    }
                }
            }
            catch (Exception) { exCount++; }

            try
            {
                if (atiTimeSpanFilthyFifty.Time > 0)
                {   // houston we have a Helen time...
                    bool addFF = true;
                    if (!string.IsNullOrWhiteSpace(hiddenFilthyFiftyKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenFilthyFiftyKey.Value);
                        Workout FF = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (FF.Duration.Value == atiTimeSpanFilthyFifty.Time) addFF = false;
                    }
                    if (addFF)
                    {
                        WOD FF = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.FILTHY50_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, atiTimeSpanFilthyFifty.Time, "Entered PR for Competition data", true, FF.Id, null, null);
                    }
                }
            }
            catch (Exception) { exCount++; }

            try
            {
                if (atiTimeSpanGrace.Time > 0)
                {   // houston we have a Helen time...
                    bool addGrace = true;
                    if (!string.IsNullOrWhiteSpace(hiddenGraceKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenGraceKey.Value);
                        Workout grace = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (grace.Duration.Value == atiTimeSpanGrace.Time) addGrace = false;
                    }
                    if (addGrace)
                    {
                        WOD grace = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.GRACE_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, atiTimeSpanGrace.Time, "Entered PR for Competition data", true, grace.Id, null, null);
                    }
                }
            }
            catch (Exception) { exCount++; }

            try
            {
               
                if (!string.IsNullOrWhiteSpace(txtFGB.Text))
                {  
                    bool addFGB = true;
                    double score = Convert.ToDouble(txtFGB.Text);
                    if (!string.IsNullOrWhiteSpace(hiddenFGBKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenFGBKey.Value);
                        Workout fgb = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (fgb.Score.Value == score) addFGB = false;
                    }
                    if (addFGB)
                    {
                        WOD fgb = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.FGB_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "Entered PR for Competition data", true, fgb.Id, score, null);
                    }
                }
            }
            catch (Exception ) { exCount++;}

            try
            {
                if (!string.IsNullOrWhiteSpace(txtMaxPullups.Text))
                {   // houston we have a Helen time...
                    bool addMaxPullups = true;
                    double score = Convert.ToDouble(txtMaxPullups.Text);
                    if (!string.IsNullOrWhiteSpace(hiddenMaxPullupKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenMaxPullupKey.Value);
                        Workout maxPullups = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (maxPullups.Score.Value == score) addMaxPullups = false;
                    }
                    if (addMaxPullups)
                    {
                        WOD maxPullups = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.MAX_PULLUPS_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "Entered PR for Competition data", true, maxPullups.Id, score, null);
                    }
                }
            }
            catch (Exception) { exCount++;}

            try
            {
                if (!string.IsNullOrWhiteSpace(txtMaxBS.Text))
                {   // houston we have a Helen time...
                    bool addMaxBS = true;
                    double w1 = Convert.ToDouble(txtMaxBS.Text);
                    double weight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(w1, atiMaxBSUnits.Value);
                    if (!string.IsNullOrWhiteSpace(hiddenMaxBackSquatKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenMaxBackSquatKey.Value);
                        Workout maxBS = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (maxBS.Max.Value == weight) addMaxBS = false;
                    }
                    if (addMaxBS)
                    {
                        WOD maxBS = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.MAX_BACK_SQUAT_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "Entered PR for Competition data", true, maxBS.Id, w1, (int)atiMaxBSUnits.Value);
                    }
                }
            }
            catch (Exception) { exCount++; }

            try
            {
                if (!string.IsNullOrWhiteSpace(txtMaxClean.Text))
                {   // houston we have a Helen time...
                    bool addMaxClean = true;
                    double w1 = Convert.ToDouble(txtMaxClean.Text);
                    double weight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(w1, atiMaxCleanUnits.Value);
                    if (!string.IsNullOrWhiteSpace(hiddenMaxCleanKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenMaxCleanKey.Value);
                        Workout maxClean = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (maxClean.Max.Value == weight) addMaxClean = false;
                    }
                    if (addMaxClean)
                    {
                        WOD maxClean = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.MAX_CLEAN_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "Entered PR for Competition data", true, maxClean.Id, w1, (int)atiMaxCleanUnits.Value);
                    }
                }
            }
            catch (Exception) { exCount++; }

            try
            {
                if (!string.IsNullOrWhiteSpace(txtMaxDeadlift.Text))
                {   // houston we have a Helen time...
                    bool addMaxDeadlift = true;
                    double w1 = Convert.ToDouble(txtMaxDeadlift.Text);
                    double weight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(w1, atiMaxDeadliftUnits.Value);
                    if (!string.IsNullOrWhiteSpace(hiddenMaxDeadliftKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenMaxDeadliftKey.Value);
                        Workout maxDeadlift = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (maxDeadlift.Max.Value == weight) addMaxDeadlift = false;
                    }
                    if (addMaxDeadlift)
                    {
                        WOD maxDeadlift = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.MAX_DEADLIFT_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "Entered PR for Competition data", true, maxDeadlift.Id, w1, (int)atiMaxDeadliftUnits.Value);
                    }
                }
            }
            catch (Exception) { exCount++; }

            try
            {
                if (!string.IsNullOrWhiteSpace(txtMaxSnatch.Text))
                {   // houston we have a Helen time...
                    bool addMaxSnatch = true;
                    double w1 = Convert.ToDouble(txtMaxSnatch.Text);
                    double weight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(w1, atiMaxSnatchUnits.Value);
                    if (!string.IsNullOrWhiteSpace(hiddenMaxSnatchKey.Value))
                    {
                        long wId = Convert.ToInt64(hiddenMaxSnatchKey.Value);
                        Workout maxSnatch = entities.UserStreamSet.OfType<Workout>().FirstOrDefault(w => w.Id == wId);
                        if (maxSnatch.Max.Value == weight) addMaxSnatch = false;
                    }
                    if (addMaxSnatch)
                    {
                        WOD maxSnatch = entities.WODs.FirstOrDefault(w => w.Id == Affine.Utils.ConstsUtil.MAX_SNATCH_ID);
                        dataMan.SaveWorkout(UserSettings, (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, (int)Affine.Utils.WorkoutUtil.DataSrc.MANUAL_NO_MAP, DateTime.Now, null, "Entered PR for Competition data", true, maxSnatch.Id, w1, (int)atiMaxSnatchUnits.Value);
                    }
                }
            }
            catch (Exception) { exCount++; }

        //    RadAjaxManager1.ResponseScripts.Add("alert('ex Count: "+exCount+"');");
        }

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

