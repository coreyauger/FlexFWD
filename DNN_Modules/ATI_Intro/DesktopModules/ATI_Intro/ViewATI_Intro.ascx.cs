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

namespace Affine.Dnn.Modules.ATI_Intro
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
    partial class ViewATI_Intro : Affine.Dnn.Modules.ATI_PermissionPageBase, IActionable
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
                atiProfileImage.Settings = atiProfileLinks.ProfileSettings = base.UserSettings;
                atiProfileLinks.IsOwner = true;
                atiFindInvite.UserSettings = UserSettings;
                atiFindInvite.TabId = TabId;

                imgCheck.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
                ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                service.InlineScript = true;
                ScriptManager.GetCurrent(Page).Services.Add(service);

                atiGMap.Lat = UserSettings.LatHome;
                atiGMap.Lng = UserSettings.LngHome;
                atiGMap.Zoom = 10;
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    instruction1.Text = "Upload a profile picture, so people know who you are, then click the 'save' button.";
                    if (Request["step"] != null)
                    {
                        instruction1.Text += ".. or press the skip button in the bottom right.";
                        atiSidePanel.Visible = false;
                        atiMainPanel.Style["width"] = "100%";
                        atiGMap.Width = Unit.Pixel(605);
                        // check if we may have info on this person from the 2011 crossfit open.
                        aqufitEntities entities = new aqufitEntities();
                        UserFriends inDotCom = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == CROSSFIT_COM_ID);
                        if (inDotCom != null)
                        {
                            rbFollowDotCom.SelectedIndex = 0;
                        }
                        CompetitionAthlete[] found = entities.CompetitionAthletes.Where(a => string.Compare(a.AthleteName, UserSettings.UserFirstName + " " + UserSettings.UserLastName, true) == 0 && a.UserSetting == null).ToArray();
                        if (found.Length > 0)
                        {
                            foreach (CompetitionAthlete athlete in found)
                            {
                                Panel p = new Panel();
                                p.CssClass = "compAthleteIsMe";
                                p.Attributes["onclick"] = "Aqufit.Page.Actions.ConnectCompetitionAthlete(" + athlete.Id + ");";
                                Literal me = new Literal();
                                me.Text = "<span class=\"boldButton compAthleteIsMeButton\">This is Me !</span>";
                                p.Controls.Add(me);
                                Control ctrl = Page.LoadControl("~/DesktopModules/ATI_Base/controls/ATI_CompetitionAthlete.ascx");
                                ctrl.ID = "athlete" + athlete.Id;
                                p.Controls.Add(ctrl);
                                ASP.desktopmodules_ati_base_controls_ati_competitionathlete_ascx details = (ASP.desktopmodules_ati_base_controls_ati_competitionathlete_ascx)ctrl;
                                details.CompetitionAthleteId = athlete.Id;
                                p.Controls.Add(details);
                                phCompetitionAthlete.Controls.Add(p);
                            }
                            RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Actions.LoadStep(0); }); ");
                        }
                        else
                        {
                            RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Actions.LoadStep(" + Request["step"] + "); }); ");
                        }
                    }
                    else
                    {
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ $('#step0').hide(); }); ");
                        
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
                    case "toggleDotCom":
                        int join = Convert.ToInt32(hiddenAjaxValue.Value);
                        
                        if (join == 1)
                        {
                            dataMan.JoinGroup(UserSettings.Id, CROSSFIT_COM_ID, ConstsUtil.Relationships.GROUP_MEMBER);
                        }
                        else if (join == 0)
                        {
                            dataMan.LeaveGroup(UserSettings.Id, CROSSFIT_COM_ID);
                        }
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiLoading.remove(); ");
                        break;
                }
            }
            catch (Exception ex)
            {
                status = "ERROR: There was a problem with the action (" + ex.Message + ")";
            }
            RadAjaxManager1.ResponseScripts.Add("UpdateStatus('" + status + "'); ");
        }

        protected void bUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                try
                {
                    long usId = this.UserSettings.Id;
                    if (GroupSettings != null)
                    {
                        aqufitEntities entities = new aqufitEntities();
                        UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == GroupSettings.Id || f.SrcUserSettingKey == GroupSettings.Id && f.DestUserSettingKey == UserSettings.Id);
                        if (uf != null && (uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER || uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN))
                        {   // Relation
                            usId = GroupSettings.Id;
                        }
                    }

                    if (fileUpload.PostedFile.ContentType == "image/jpeg" || fileUpload.PostedFile.ContentType == "image/png" || fileUpload.PostedFile.ContentType == "image/jpg" || fileUpload.PostedFile.ContentType == "image/gif")
                    {
                        if (fileUpload.PostedFile.ContentLength < 10072000)
                        {
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(fileUpload.FileBytes);
                            Affine.Utils.ImageUtil.MakeImageProfilePic(ms, usId);
                            Affine.Utils.ImageUtil.SavePhoto(Affine.Utils.ImageUtil.AlbumType.PROFILE, ms, usId, -1, userPhotoPath, urlPath, true);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiProfileRefresh", " parent.Aqufit.Page.Controls.atiProfileImage.ImageUploadSuccess();", true);
                        }
                        else
                        {
                       //     lStatus.Text = "Upload status: The file has to be less than 9 MB!";
                        }
                    }
                    else
                    {
                    //    lStatus.Text = "Upload status: Only JPEG, PNG, GIF files are accepted!";
                    }
                }
                catch (Exception ex)
                {
                 //   lStatus.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
                

            }
            if (Request["step"] != null)
            {
                Response.Redirect(ResolveUrl("~/Profile/GettingStarted") + "?step=2", true);
                return;
            }
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

