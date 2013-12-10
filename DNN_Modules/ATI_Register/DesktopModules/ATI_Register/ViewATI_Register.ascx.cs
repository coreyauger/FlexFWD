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
' TO THE WARRANTIES OF MERCHANTABILITY, REGISTER FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
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
using System.Web;
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
using Affine.Services.ThirdParty;

namespace Affine.Dnn.Modules.ATI_Register
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
    partial class ViewATI_Register : ATI_PermissionPageBase, IActionable
    {

        #region Private Members     
   
        private aqufitEntities entities = new aqufitEntities();
       
        #endregion       

        #region Public Methods

        public string BackgroundImageUrl { get; set; }
        public bool IsBeta
        {
            get
            {
                if (ViewState["IsBeta"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsBeta"]);
                }
                return false;
            }
            set {
                ViewState["IsBeta"] = value;
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            //base._PerformFBLoginCheck = false;
            base.OnInit(e);
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
                litFBConnect.Text = "<fb:login-button show-faces=\"true\" perms=\"email\" width=\"250\" max-rows=\"4\"></fb:login-button>";                    
                this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=0&p=0&bg=1";
                litStatus.Text = "";
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    imgfbConnect.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/fbplatform.gif");
                    if (Request["t"] != null)
                    {
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Tabs.SwitchTab(" + Request["t"] + "); }); ");
                    }
                    if (Request["g"] != null)
                    {
                        hiddenGroupKey.Value = Request["g"];
                    }

                    imgError.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png");
                    imgCheck.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
                    if (Settings["Beta"] != null)
                    {
                        this.IsBeta = Convert.ToBoolean(Settings["Beta"]);
                        panelBeta.Visible = this.IsBeta;
                    }
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/RegisterService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    if (Settings["Configure"] != null && Convert.ToString(Settings["Configure"]) == "ConfigureLogin")
                    {
                        atiRegistrationPanel.Visible = false;
                        atiLoginPanel.Visible = true;

                        fbConnect.Text = "<fb:login-button show-faces=\"true\" perms=\"email\" width=\"250\" max-rows=\"4\"></fb:login-button>";                       
                    }
                    else if (Settings["Configure"] != null && Convert.ToString(Settings["Configure"]) == "ConfigureGroupRegister")
                    {
                        if (GroupSettings != null)   // This is a person trying to edit a group profile
                        {
                            UserFriends perm = entities.UserFriends.FirstOrDefault(w => w.SrcUserSettingKey == UserSettings.Id && w.DestUserSettingKey == GroupSettings.Id && w.Relationship <= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN);
                            // check that they have permissions first (need to be a group admin)
                            if (perm != null)
                            {
                                SetupAccountEdit(GroupSettings);
                            }
                            else
                            {
                                Response.Redirect(ResolveUrl("~") + UserSettings.UserName, true);
                            }

                        }
                        else
                        {
                            atiRegistrationPanel.Visible = false;
                            aqufitEntities entities = new aqufitEntities();
                            GroupType[] groupTypes = entities.GroupTypes.ToArray();
                        }
                    }
                    else if (Settings["RpxReciever"] != null && Convert.ToBoolean(Settings["RpxReciever"]))
                    {       // We just use FB login now... so may want to rename this setting in future..
                        if (base.fbUser == null || string.IsNullOrWhiteSpace(base.fbUser.email) || base.fbUser.id <= 0 )
                        {
                            throw new Exception("Problem with FaceBook Login");
                        }
                        atiRegistrationPanel.Visible = false;
                        atiRpxRecievePanel.Visible = true;

                        aqufitEntities entities = new aqufitEntities();                        
                        if (this.IsBeta)
                        {
                            Preview tester = entities.Preview.FirstOrDefault(p => p.EnteredByPortalId == this.PortalId && string.Compare(p.Email, fbUser.email, true) == 0);
                            if (tester == null)
                            {
                                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Sorry you are not on the Beta Test list.\"}');");
                                return;
                            }
                        }                        
                        // This is the first time in so we need to register them
                        hiddenEmail.Value = base.fbUser.email;
                        hiddenGivenName.Value = base.fbUser.first_name;
                        hiddenFamilyName.Value = base.fbUser.last_name;
                        hiddenIdentifier.Value = "" + base.fbUser.id;
                        // if we do not get a sex ... we assume male.. otherwise leader boards do not get processed ..
                        hiddenGender.Value = string.IsNullOrWhiteSpace( base.fbUser.gender ) ? "M" : base.fbUser.gender.ToLower().StartsWith("m") ? "M" : "F";
                        hiddenBirthday.Value = "";
                    }
                    else
                    {   // we need to track this and produce an error message
                        if (this.UserId > 0)
                        {
                            SetupAccountEdit(this.UserSettings);
                        }
                    }
                }
                else
                {
                    RadCaptcha1.EnableRefreshImage = true;
                    RadCaptcha1.CaptchaTextBoxLabel = "";
                    if (Request["r"] != null && !Request["r"].Equals(string.Empty))
                    {
                        int rid = Convert.ToInt32(Request["r"]);
                        DotNetNuke.Entities.Users.UserInfo user = DotNetNuke.Entities.Users.UserController.GetUser(PortalId, rid, true);
                        if (user != null)
                        {
                            Session["AffiliateId"] = rid;
                            Request.Cookies.Add(new HttpCookie("AffiliateId", Convert.ToString(rid)));
                        }
                        else
                        {
                            // TODO: log this
                            DotNetNuke.Services.Log.EventLog.ExceptionLogController exLogController = new ExceptionLogController();
                            exLogController.AddLog(new Exception("User linked to site with affliate id of a non existant user"), ExceptionLogController.ExceptionLogType.GENERAL_EXCEPTION);
                        }
                    }
                    
                    // We still need to configure which inputs we want from the module settings.
                    if (Settings["HideGender"] != null && Convert.ToBoolean(Settings["HideGender"]))
                    {
                        atiBodyComp.GenderVisible = false;
                    }
                    if (Settings["HideBirthday"] != null && Convert.ToBoolean(Settings["HideBirthday"]))
                    {
                        atiBodyComp.BirthDateVisible = false;
                    }
                    if (Settings["HideWeight"] != null && Convert.ToBoolean(Settings["HideWeight"]))
                    {
                        atiBodyComp.WeightVisible = false;
                    }
                    if (Settings["HideHeight"] != null && Convert.ToBoolean(Settings["HideHeight"]))
                    {
                        atiBodyComp.HeightVisible = false;
                    }
                    if (Settings["HideFitnessLevel"] != null && Convert.ToBoolean(Settings["HideFitnessLevel"]))
                    {
                        atiBodyComp.FitnessLevelVisible = false;
                    }                                      
                }
                
               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        private void SetupAccountEdit(UserSettings userSettings)
        {
            aqufitEntities entities = new aqufitEntities();
           // BodyComposition bodyComp = entities.BodyComposition.FirstOrDefault(b => b.Id == userSettings.Id);
            this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + userSettings.UserKey + "&p=" + userSettings.PortalKey + "&bg=1";
            bool isUser = userSettings is User;
            spanTerms.Visible = false;
            litRegistration.Text = "Account Settings";
            tabPassword.Visible = tabAppearance.Visible = tabWorkoutSettings.Visible = isUser;            
            h2Register.Visible = false;
            // might as well make the gender visible
            //atiBodyComp.GenderVisible = true;
            // TODO: add the address stuff in on an edit as well
            panelAthleteSettings.Visible = true;
            atiLeftPanel.Visible = false;
            atiRightPanel.Style["width"] = "690px";
            atiUserName.Visible = false;
            litUserName.Text = "<h2 style=\"color: #EB8F00;\">" + UserSettings.UserName + "</h2>";
            litUserName.Visible = true;
            RadCaptcha1.Visible = false;
            atiWebLinks.Visible = true;
            bUpdate.Visible = true;
            bRegister.Visible = false;
            atiThemeEditor.Visible = true;
            bUpload.Visible = true;
            panelProfilePic.Visible = true;
            atiProfileImage.Settings = userSettings;
            // TODO: in the future allow username edit ... (query for it is in bUpdate_Click)
            atiPassword.Visible = isUser;
            atiPassword.SetValidationGroup("updatepassword");
            bUpdatePass.Visible = isUser;
            bUpdatePass.Click += new EventHandler(bUpdatePass_Click);
            UserSettings settings = entities.UserSettings.Include("User2WorkoutType.WorkoutType").Include("UserSettingsExtendeds").FirstOrDefault(us => us.UserKey == userSettings.UserKey && us.PortalKey == userSettings.PortalKey);
            atiUserName.IsEditMode = atiBodyComp.IsEditMode = atiSlimControl.IsEditMode = atiPassword.IsEditMode = true;
            atiUserName.Text = settings.UserName;
            atiUserName.Enabled = false;
            atiSlimControl.FullName = settings.UserFirstName + " " + settings.UserLastName;
            atiSlimControl.Postal = this.UserInfo.Profile.PostalCode;
            atiSlimControl.Email = settings.UserEmail;
            panelAthleteSettings.Visible = isUser;
            atiWorkoutTypes.UncheckTypes = settings.User2WorkoutType.Select(u2w => u2w.WorkoutType).ToArray();

            SiteSetting[] ssArray = entities.SiteSettings.Where(s => s.UserSetting.Id == UserSettings.Id).ToArray();
            foreach (SiteSetting ss in ssArray)
            {
                if (ss.Name == "AllowHomeGroupEmail")
                {
                    cbAllowGroupEmails.Checked = false;
                }
            }

            BodyComposition comp = entities.BodyComposition.FirstOrDefault( b => b.UserSetting.Id == settings.Id );
            if (comp != null)
            {
                txtBio.Text = comp.Bio;
                txtTraining.Text = comp.Description;
            }

            long[] groupIds = null;            
            groupIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == settings.Id || f.DestUserSettingKey == settings.Id) && f.Relationship >= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER ).Select(f => f.SrcUserSettingKey == settings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey).ToArray();
            var gNamesIds = entities.UserSettings.OfType<Group>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<UserSettings, long>(us => us.Id, groupIds)).OrderBy(f => f.UserName).Select(g => new{ Name=g.UserName, Id = g.Id} ).ToArray();
            foreach( var gi in gNamesIds ){
                ListItem item = new ListItem(gi.Name, "" + gi.Id);
                if( settings.MainGroupKey.HasValue && gi.Id == settings.MainGroupKey.Value ){
                    item.Selected = true;
                }
                ddlHomeGroup.Items.Add(item);
            }

            // Next we do the appearance settings...
            if (!string.IsNullOrWhiteSpace(settings.CssStyle))
            {
                const string findMe = "background-color:";
                int bgInd = settings.CssStyle.IndexOf(findMe);
                if (bgInd >= 0)
                {
                    string color = settings.CssStyle.Substring(bgInd + findMe.Length, settings.CssStyle.IndexOf(";", bgInd + findMe.Length) - (bgInd + findMe.Length)).Replace("#", "");
                    atiThemeEditor.BackgroundColor = System.Drawing.Color.FromName("#" + color.Trim());
                }
                if (settings.CssStyle.Contains("background-repeat: repeat;"))
                {
                    atiThemeEditor.IsTiled = true;
                }
            }
            
            atiBodyComp.Visible = isUser;
            if (atiBodyComp.BirthDateVisible && settings.BirthDate != null)
            {
                atiBodyComp.BirthDate = settings.BirthDate;
            }
            if (atiBodyComp.GenderVisible)
            {
                atiBodyComp.Gender = settings.Sex;
            }
            if (atiWebLinks.Visible)
            {
                UserSettingsExtended[] webLinks = settings.UserSettingsExtendeds.Where(s => s.Class == 1).ToArray();
                UserSettingsExtended facebook = webLinks.FirstOrDefault(w => w.Name == "Facebook");
                atiWebLinks.Facebook = facebook != null ? facebook.Value : null;
                UserSettingsExtended twitter = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "Twitter");
                atiWebLinks.Twitter = twitter != null ? twitter.Value : null;
                UserSettingsExtended linkedin = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "LinkedIn");
                atiWebLinks.LinkedIn = linkedin != null ? linkedin.Value : null;
                UserSettingsExtended youtube = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "YouTube");
                atiWebLinks.YouTube = youtube != null ? youtube.Value : null;
                UserSettingsExtended flickr = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "Flickr");
                atiWebLinks.Flickr = flickr != null ? flickr.Value : null;
                UserSettingsExtended personal = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "Personal");
                atiWebLinks.Peronsal = personal != null ? personal.Value : null;
            }
        }


        protected void bUpload_Click(object sender, EventArgs e)
        {
            FileUpload fileUpload = atiThemeEditor.FileUpload;
            if (fileUpload.HasFile)
            {
                try
                {
                    long usid = this.UserSettings.Id;
                    if (GroupSettings != null)
                    {
                        aqufitEntities entities = new aqufitEntities();
                        UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == GroupSettings.Id || f.SrcUserSettingKey == GroupSettings.Id && f.DestUserSettingKey == UserSettings.Id);
                        if (uf != null && (uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER || uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN) )
                        {   // Relation
                            usid = GroupSettings.Id;    // allow the edit of the group profile pic if the user is an admin
                        }
                    }
                    if (fileUpload.PostedFile.ContentType == "image/jpeg" || fileUpload.PostedFile.ContentType == "image/png" || fileUpload.PostedFile.ContentType == "image/jpg" || fileUpload.PostedFile.ContentType == "image/gif")
                    {
                        if (fileUpload.PostedFile.ContentLength < 3072000)
                        {
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(fileUpload.FileBytes);
                            Affine.Utils.ImageUtil.SaveBackgroundImage(ms, usid);
                      //      ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiProfileRefresh", " parent.Aqufit.Page.Controls.atiProfileImage.ImageUploadSuccess();", true);
                        }
                        else
                        {
                        //    lStatus.Text = "Upload status: The file has to be less than 3 MB!";
                        }
                    }
                    else
                    {
                        //lStatus.Text = "Upload status: Only JPEG, PNG, GIF files are accepted!";
                    }
                }
                catch (Exception ex)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"" + ex.Message + "\"}');");
                   // lStatus.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }

            }
        }



        protected void bLogin_Click(object sender, EventArgs e)
        {
            string uname = txtUserNameEmail.Text;
            if (uname.Contains("@"))
            {   // this is an email login
                /*
                int numRec = 0;                
                ArrayList uinfoArray = UserController.GetUsersByEmail(this.PortalId, uname, 0, 1, ref numRec);
                if (numRec != 1)
                {
                    litStatus.Text = "Error: Incorect Username/Password combination.";
                    return;
                }
                uname = ((UserInfo)uinfoArray[0]).Username;
                 */
                User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserEmail == uname);
                if (user == null)
                {
                    litStatus.Text = "Error: Wrong email address.";
                        
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Error: Wrong email address.\"}');");
                    return;
                }
                uname = user.UserName;
            }
            DotNetNuke.Security.Membership.UserLoginStatus status = DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_FAILURE;
            //UserController.UserLogin(this.PortalId, uname, txtLoginPassword.Text, null, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), ref status, cbRememberMe.Checked);
            UserController.UserLogin(this.PortalId, uname, txtLoginPassword.Text, null, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), ref status, true);
            if (status == DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_SUCCESS || status == DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_SUPERUSER )
            {
                aqufitEntities entities = new aqufitEntities();
                UserSettings usersettings = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserName == uname && u.PortalKey == PortalSettings.PortalId);
                
                if (usersettings != null)
                {
                    if (cbRememberMe.Checked)
                    {
                        if (!usersettings.Guid.HasValue)
                        {
                            usersettings.Guid = Guid.NewGuid();
                        }
                        Response.Cookies["FlexRM"].Value = usersettings.Guid.Value.ToString();
                        Response.Cookies["FlexRM"].Expires = DateTime.Now.AddYears(1);
                        Response.Cookies["FlexRM"].Domain = this.PortalAlias.HTTPAlias;
                    }
                    usersettings.LastLoginDate = DateTime.Now;
                    entities.SaveChanges();
                }
                //string url = Convert.ToString( Settings["LandingPage"] );
                string url = ResolveUrl("~") + uname;
                if (!string.IsNullOrEmpty(Request["ReturnUrl"]) && !Request["ReturnUrl"].Contains("Home.aspx") && !Request["ReturnUrl"].Contains("Login"))
                {
                    string decode = Server.UrlDecode(Request["ReturnUrl"]);
                    if (!string.IsNullOrWhiteSpace(decode) && !decode.EndsWith("?") && decode != "/")
                    {
                        url = Request["ReturnUrl"];
                    }
                }
                Response.Redirect(url, true);
            }
            else
            {
                litStatus.Text = "Login Failed. Please remember that passwords are case sensitive. " + status;
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Login Failed. Please remember that passwords are case sensitive. " + status+"\"}');");
            }
        }

       
        protected void bUserSetup_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int UID = -1;
                UserInfo objUserInfo = UserController.GetUserByName(PortalId, atiUserNameSetup.Text);
                // if a user is found with that username, error. this prevents you from adding a username with the same name as a superuser.
                if (objUserInfo != null)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"We already have an entry for the User Name.\"}');");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(atiSlimFBSetup.Postal))
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Postal/Zip is required to give you accurate gym, route, and user information, in your location.\"}');");
                    return;
                }
                else if (atiPasswordSetup.Password != atiPasswordSetup.Confirm)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Password and Confirmation do not match.\"}');");
                    return;
                }
                Affine.WebService.RegisterService regService = new WebService.RegisterService();
                UserInfo objNewUser = regService.InitialiseUser(PortalId); // TODO: this method

                // TODO: need a TimeZone control

                string fname = hiddenGivenName.Value;
                string lname = hiddenFamilyName.Value;
                string fullname = fname + " " + lname;                
                //string uuid = Guid.NewGuid().ToString().Substring(0, 15);
                string pass = atiPasswordSetup.Password;
                string email = hiddenEmail.Value;
                regService.populateDnnUserInfo(PortalId, ref objNewUser, fname, lname, atiUserNameSetup.Text, email, pass, atiSlimFBSetup.Postal, atiSlimFBSetup.Address, null);

                DotNetNuke.Security.Membership.UserCreateStatus userCreateStatus = UserController.CreateUser(ref objNewUser);
                if (userCreateStatus == DotNetNuke.Security.Membership.UserCreateStatus.Success)
                {
                    UID = objNewUser.UserID;
                    // DNN3 BUG
                    DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                    objEventLog.AddLog(objNewUser, PortalSettings, objNewUser.UserID, atiSlimControl.Email, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.USER_CREATED);

                    // send notification to portal administrator of new user registration                       
                    //            DotNetNuke.Services.Mail.Mail.SendMail(objNewUser, DotNetNuke.Services.Mail.MessageType.UserRegistrationAdmin, PortalSettings);
                    //            DotNetNuke.Services.Mail.Mail.SendMail(objNewUser, DotNetNuke.Services.Mail.MessageType.UserRegistrationPublic, PortalSettings);                   
                    DataCache.ClearUserCache(PortalId, objNewUser.Username);
                    UserController.UserLogin(PortalId, objNewUser, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);

                    User us = new Data.User();
                    us = (User)populateUserSettings(us, objNewUser);
                    try
                    {
                        us.FBUid = Convert.ToInt64(hiddenIdentifier.Value);
                    }
                    catch (Exception) { }
                    entities.AddToUserSettings(us);
                    entities.SaveChanges();

                    // TODO: should have a populate function for these
                    BodyComposition bc = new BodyComposition()
                    {
                        UserSetting = us
                    };
                    if (atiBodyComp.Visible && atiBodyComp.FitnessLevelVisible)
                    {
                        bc.FitnessLevel = atiBodyComp.FitnessLevel;
                    }

                    // need height and weight conversions
                    if (atiBodyComp.Visible && atiBodyComp.WeightVisible)
                    {
                        bc.Weight = atiBodyComp.UserWeightInSystemDefault;
                    }
                    if (atiBodyComp.Visible && atiBodyComp.HeightVisible)
                    {
                        bc.Height = atiBodyComp.UserHeightInSystemDefault;
                    }
                    entities.AddToBodyComposition(bc);
                    entities.SaveChanges();

                    string body = "User registration recorded\n\n";
                    body += "ID: " + objNewUser.UserID + "\n";
                    body += "User: " + objNewUser.Username + "\n";
                    body += "First Name: " + objNewUser.FirstName + "\n";
                    body += "Last Name: " + objNewUser.LastName + "\n";
                    body += "Email: " + objNewUser.Email + "\n";
                    body += "Portal: " + objNewUser.PortalID + "\n";
                    //           DotNetNuke.Services.Mail.Mail.SendMail("aqufit@gmail.com", "corey@aqufit.com", "", "NEW aqufit.com USER", body, "", "HTML", "", "", "", "");
                    Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
                    gmail.Send("coreyauger@gmail.com", "User Signup: " + objNewUser.Username, body);

                    // See if this person was invited by anyone.
                    ContactInvite invite = entities.ContactInvites.Include("UserSetting").FirstOrDefault(i => i.Email == us.UserEmail);
                    if (invite != null)
                    {   // this person was invited by someone so lets make them friends....
                        string stat = Affine.Data.Managers.LINQ.DataManager.Instance.AcceptFriendRequests(invite.UserSetting.Id, us.Id);
                        // TODO: assume success send for now
                    }
                    Response.Redirect(Convert.ToString(Settings["LandingPage"]), true);
                }
                else
                { // registration error
                    string body = "User registration FAILED \n\n";
                    body += "ID: " + objNewUser.UserID + "\n";
                    body += "User: " + objNewUser.Username + "\n";
                    body += "First Name: " + fname + "\n";
                    body += "Last Name: " + lname + "\n";
                    body += "Email: " + email + "\n";
                    body += "Status: " + userCreateStatus.ToString();
                    //  string errStr = userCreateStatus.ToString() + "\n\n" + atiSlimControl.ToString();
                    //  DotNetNuke.Services.Mail.Mail.SendMail("aqufit@gmail.com", "corey@aqufit.com", "", "FAILED REGISTRATION", lErrorText.Text + errStr, "", "HTML", "", "", "", "");
                    Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
                    gmail.Send("coreyauger@gmail.com", "**FAIL** RPX User Signup: " + objNewUser.Username, body);                    
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"" + userCreateStatus.ToString() + "\"}');");                                        
                }
            }
            else
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"\"}');");
            }            
        }

        private void Login(int PortalKey, int UserKey)
        {
            // This is a returning user... so lets log them in.
            UserInfo uinfo = UserController.GetUser(PortalKey, UserKey, true);
            UserController.UserLogin(PortalKey, uinfo, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);
            //string url = Convert.ToString(Settings["LandingPage"]);
            string url = ResolveUrl("~") + uinfo.Username;
            if (Request["ReturnUrl"] != null)
            {
                url = Request["ReturnUrl"];
            }
            Response.Redirect(url, true);            
        }

        protected void bUpdatePass_Click(object sender, EventArgs e)
        {           
            if (Page.IsValid)
            {
                string current = atiPassword.Original;
                string update = atiPassword.Password;
                try
                {
                    if (UserController.ChangePassword(this.UserInfo, current, update))
                    {
                        litStatus.Text = "SUCCESS... your password has been set.";
                    }
                    else // Failed to set the new password
                    {
                        litStatus.Text = "ERROR. Your current password was incorrect.";
                    }
                }
                catch (Exception ex)
                {
                    litStatus.Text = "ERROR. " + ex.Message;
                }
            }
            //ErrorDialog.Visible = !Page.IsValid;
        }
        

        protected void bUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Get the DNN user.
                try
                {
                    UserInfo ui = UserController.GetUserById(this.PortalId, this.UserId);
                    string fullname = atiSlimControl.FullName;
                    string[] nameparts = fullname.Split(' ');
                    string fname = string.Empty;
                    string lname = string.Empty;
                    if (nameparts.Length >= 2)
                    {
                        fname = nameparts[0];
                        lname = nameparts[1];
                    }
                    // TODO: we check if the user is trying to change their username
                    // Here is the query for it.  Right not we disable the UserName control
                    // update users set username='newusername' where username='oldusername'
                    // update aspnet_Users set username='newusername' ,loweredusername='newusername' where username='oldusername'
                    Affine.WebService.RegisterService regService = new WebService.RegisterService();

                    // TODO: need timezone control

                    regService.populateDnnUserInfo(PortalId, ref ui, fname, lname, atiUserName.Text, atiSlimControl.Email, atiPassword.Password, atiSlimControl.Postal, atiSlimControl.Address, null);
                    aqufitEntities entities = new aqufitEntities();
                    User us = entities.UserSettings.OfType<User>().Include("User2WorkoutType").Include("User2WorkoutType.WorkoutType").FirstOrDefault(s => s.UserKey == this.UserId && s.PortalKey == this.PortalId);
                    us = (User)populateUserSettings(us, ui);
                    if (!string.IsNullOrEmpty( hiddenIdentifier.Value ))
                    {
                        us.FBUid = Convert.ToInt64(hiddenIdentifier.Value);
                    }


                    // TODO: make this the same as the "GroupAdmin"
               //     UploadThemeBackground();
                    us.CssStyle = string.Empty;

                    if (!atiThemeEditor.BackgroundColor.IsEmpty)
                    {
                        us.CssStyle += "background-color: #" + atiThemeEditor.BackgroundColor.Name.Substring(2) + ";";
                    }
                    if (atiThemeEditor.IsTiled)
                    {
                        us.CssStyle += "background-repeat: repeat;";
                    }
                    else
                    {
                        us.CssStyle += "background-repeat:no-repeat; background-attachment:fixed;";
                    }
                    this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + us.UserKey + "&p=" + us.PortalKey + "&bg=1";
                  //  this.ProfileCSS = us.CssStyle;
                    

                    foreach (CheckBox cb in atiWorkoutTypes.CheckBoxList)
                    {
                        if (!cb.Checked)
                        {
                            WorkoutType wt = entities.WorkoutType.First(w => w.Name == cb.Text);
                            User2WorkoutType u2wt = new User2WorkoutType() { WorkoutType = wt, UserSetting = us };
                            entities.AddToUser2WorkoutType(u2wt);
                        }
                        else
                        {
                            User2WorkoutType wt = us.User2WorkoutType.FirstOrDefault(w => w.WorkoutType.Name == cb.Text);
                            if (wt != null)
                            {
                                entities.DeleteObject(wt);
                            }
                        }
                    }
                    if (!cbAllowGroupEmails.Checked)
                    {
                        SiteSetting ss = new SiteSetting()
                        {
                            Name = "AllowHomeGroupEmail",
                            Value = "true",
                            UserSetting = entities.UserSettings.FirstOrDefault( u => u.Id == UserSettings.Id )
                        };
                        entities.AddToSiteSettings(ss);                       
                    }else{
                        // delete it if it exists
                        SiteSetting ss = entities.SiteSettings.FirstOrDefault(s => s.UserSetting.Id == UserSettings.Id && s.Name == "AllowHomeGroupEmail");
                        if (ss != null)
                        {
                            entities.DeleteObject(ss);
                        }
                    }
                    UserController.UpdateUser(this.PortalId, ui);
                    entities.SaveChanges();
                    litStatus.Text = "Your Changes have been saved.";
                  //  RadAjaxManager1.ResponseScripts.Add("alert('Your Changes have been saved');");
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.SuccessDialog.open('{\"html\":\"Your Changes have been saved.\"}');");
                }
                catch (Exception ex)
                {
                    RadAjaxManager1.ResponseScripts.Add("alert('" + ex.InnerException.Message.Replace("'", "").Replace("\"", "").Replace("\n", "") + "');");
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"" + ex.Message + "\"}');");
                }
            }
            else
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\" Page Not Valid \"}');");
            }
            
        }

        protected void bRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (this.IsBeta)
                {
                    Preview tester = entities.Preview.FirstOrDefault(p => p.EnteredByPortalId == this.PortalId && string.Compare(p.Email, atiSlimControl.Email, true) == 0);
                    if (tester == null)
                    {
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Sorry you are not on the Beta Test list.\"}');");
                        return;
                    }
                }
                User emailTest = entities.UserSettings.OfType<User>().FirstOrDefault(us => us.UserEmail == atiSlimControl.Email && us.PortalKey == this.PortalId );
                if (emailTest != null)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"Email already exists\"}');");
                    return;
                }
                if (Register())
                {
                    Response.Redirect(Convert.ToString(Settings["LandingPage"]), true);          
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"There was a problem with your registration.  Please contact support.\"}');");
                }
            }
            else
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"" + RadCaptcha1.ErrorMessage + "\"}');");
                litStatus.Text = RadCaptcha1.ErrorMessage;
                RadCaptcha1.CaptchaImage.RenderImage();
               // RadAjaxManager1.ResponseScripts.Add("AjaxResponseFail();");
            }            
        }

        private bool Register()
        {
            //Try
            // Only attempt a save/update if all form fields on the page are valid
            if (Page.IsValid)
            {
                // check required fields               
                int UID = -1;                               
                UserInfo objUserInfo = UserController.GetUserByName(PortalId, atiUserName.Text);                                                
                // if a user is found with that username, error. this prevents you from adding a username with the same name as a superuser.
                if (objUserInfo != null)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"We already have an entry for the User Name.\"}');");
                    return false;
                }
                Affine.WebService.RegisterService regService = new WebService.RegisterService();
                double? timezone = null;
                if (!string.IsNullOrEmpty(atiSlimControl.LocLat))
                {
           //         timezone = regService.GetTimeZone(Convert.ToDouble(atiSlimControl.LocLat), Convert.ToDouble(atiSlimControl.LocLng));
                }
                UserInfo objNewUser = regService.InitialiseUser(PortalId);
                string fullname = atiSlimControl.FullName;
                string[] nameparts = fullname.Split(' ');
                string fname = string.Empty;
                string lname = string.Empty;
                if (nameparts.Length >= 2)
                {
                    fname = nameparts[0];
                    lname = nameparts[1];
                }
                
                regService.populateDnnUserInfo(PortalId, ref objNewUser, fname, lname, atiUserName.Text, atiSlimControl.Email, atiPassword.Password, atiSlimControl.Postal, atiSlimControl.Address, timezone);
                
                    
                DotNetNuke.Security.Membership.UserCreateStatus userCreateStatus = UserController.CreateUser(ref objNewUser);
                if (userCreateStatus == DotNetNuke.Security.Membership.UserCreateStatus.Success)
                {
                    UID = objNewUser.UserID;
                    // DNN3 BUG
                    DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                    objEventLog.AddLog(objNewUser, PortalSettings, objNewUser.UserID, atiSlimControl.Email, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.USER_CREATED);

                    // send notification to portal administrator of new user registration                       
        //            DotNetNuke.Services.Mail.Mail.SendMail(objNewUser, DotNetNuke.Services.Mail.MessageType.UserRegistrationAdmin, PortalSettings);
        //            DotNetNuke.Services.Mail.Mail.SendMail(objNewUser, DotNetNuke.Services.Mail.MessageType.UserRegistrationPublic, PortalSettings);                   
                    DataCache.ClearUserCache(PortalId, objNewUser.Username);
                    UserController.UserLogin(PortalId, objNewUser, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);

                    User us = new Data.User();
                    us = (User)populateUserSettings(us, objNewUser);
                    if (atiBodyComp.BirthDateVisible)
                    {
                        us.BirthDate = atiBodyComp.BirthDate;
                    }
                    SiteSetting tutorial = new SiteSetting()
                    {
                        UserSetting = us,
                        Name = "SiteIntro",
                        Value = "1"
                    };
                    entities.AddToUserSettings(us);
                    entities.SaveChanges();

                    us = entities.UserSettings.OfType<User>().FirstOrDefault( u => u.UserKey == objNewUser.UserID && u.PortalKey == objNewUser.PortalID );

                    // TODO: should have a populate function for these
                    BodyComposition bc = new BodyComposition()
                    {
                        UserSetting = us                       
                    };
                    if (atiBodyComp.FitnessLevelVisible)
                    {
                        bc.FitnessLevel = atiBodyComp.FitnessLevel;
                    }

                    // need height and weight conversions
                    if (atiBodyComp.WeightVisible)
                    {
                        bc.Weight = atiBodyComp.UserWeightInSystemDefault;
                    }
                    if( atiBodyComp.HeightVisible )
                    {
                        bc.Height = atiBodyComp.UserHeightInSystemDefault;                     
                    }
                    entities.AddToBodyComposition(bc);
                    entities.SaveChanges();

                   
                    

                    string body = "User registration recorded\n\n";
                           body += "ID: " + objNewUser.UserID + "\n";
                           body += "User: " + objNewUser.Username + "\n";
                           body += "First Name: " + objNewUser.FirstName + "\n";
                           body += "Last Name: " + objNewUser.LastName + "\n";
                           body += "Email: " + objNewUser.Email + "\n";
                           body += "Portal: " + objNewUser.PortalID + "\n";
                           body += "User-Agent: " + Request.UserAgent + "\n\n";
         //           DotNetNuke.Services.Mail.Mail.SendMail("aqufit@gmail.com", "corey@aqufit.com", "", "NEW aqufit.com USER", body, "", "HTML", "", "", "", "");
                    Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
                    gmail.Send("coreyauger@gmail.com", "User Signup: " + objNewUser.Username, body);

                    // where they brought here by a group?
                    long gid = Convert.ToInt64(hiddenGroupKey.Value);
                    if (gid > 0)
                    {   // if so then we auto add them to the group.
                        Data.Managers.LINQ.DataManager.Instance.JoinGroup(us.Id, gid, Utils.ConstsUtil.Relationships.GROUP_MEMBER);
                    }

                    // See if this person was invited by anyone.
                    ContactInvite invite = entities.ContactInvites.Include("UserSetting").FirstOrDefault(i => i.Email == us.UserEmail);
                    if (invite != null)
                    {   // this person was invited by someone so lets make them friends....
                        string stat = Affine.Data.Managers.LINQ.DataManager.Instance.AcceptFriendRequests(invite.UserSetting.Id, us.Id);
                        // TODO: assume success send for now
                    }
                    // TODO: look through a list of stored contacts to get a sugested friends list...
                }
                else
                { // registration error                   
                    string body = "User Registration Form FAILED (Us\n\n";
                    body += "ID: " + objNewUser.UserID + "\n";
                    body += "User: " + objNewUser.Username + "\n";
                    body += "First Name: " + objNewUser.FirstName + "\n";
                    body += "Last Name: " + objNewUser.LastName + "\n";
                    body += "Email: " + objNewUser.Email + "\n";
                    body += "REGISTRATION ERROR: " + userCreateStatus.ToString() + "\n";
                    //  string errStr = userCreateStatus.ToString() + "\n\n" + atiSlimControl.ToString();
                  //  DotNetNuke.Services.Mail.Mail.SendMail("aqufit@gmail.com", "corey@aqufit.com", "", "FAILED REGISTRATION", lErrorText.Text + errStr, "", "HTML", "", "", "", "");
                    Affine.Utils.GmailUtil gmail = new Utils.GmailUtil();
                    gmail.Send("coreyauger@gmail.com", "**FAIL** REGISTRATION FORM: " + objNewUser.Username, body);

                    litStatus.Text = "REGISTRATION ERROR: " + userCreateStatus.ToString();
                    return false;
                }
            }
            return true;
        }

        private UserSettings populateUserSettings(UserSettings us, UserInfo ui)
        {
            aqufitEntities entities = new aqufitEntities();
                
            long usId = us.Id;
            us.UserKey = ui.UserID;
            us.PortalKey = ui.PortalID;
            us.UserEmail = ui.Email;
            us.UserName = ui.Username;
            us.UserFirstName = ui.FirstName;
            us.UserLastName = ui.LastName;
            //us.TimeZone = ui.Profile.TimeZone;
            if (atiBodyComp.Visible)
            {
                if (atiBodyComp.GenderVisible)
                {
                    us.Sex = atiBodyComp.Gender;
                }
                if (atiBodyComp.BirthDateVisible && atiBodyComp.BirthDate.HasValue)
                {
                    us.BirthDate = Convert.ToDateTime(atiBodyComp.BirthDate);
                }
                if (atiBodyComp.WeightVisible)
                {
                    us.WeightUnits = Convert.ToInt16(atiBodyComp.WeightUnits);
                }
                if (atiBodyComp.HeightVisible)
                {
                    us.HeightUnits = Convert.ToInt16(atiBodyComp.HeightUnits);
                }
            }
            if (ddlHomeGroup.Items.Count > 0 )
            {
                us.MainGroupKey = Convert.ToInt64( ddlHomeGroup.SelectedValue );
            }
            if (atiHeightWeight.Visible)
            {
                BodyComposition bc = entities.BodyComposition.FirstOrDefault(b => b.UserSetting.Id == us.Id);
                if( bc == null ){
                    bc = new BodyComposition(){
                        UserSetting = entities.UserSettings.FirstOrDefault( u => u.Id == us.Id ),                        
                    };
                    entities.AddToBodyComposition(bc);
                }
                if (atiHeightWeight.WeightVisible)
                {
                    bc.Weight = atiHeightWeight.UserWeightInSystemDefault;                    
                }
                if (atiHeightWeight.HeightVisible)
                {
                    bc.Height = atiHeightWeight.UserHeightInSystemDefault;                   
                }
                bc.Bio = txtBio.Text;
                bc.Description = txtTraining.Text;
                entities.SaveChanges();
            }
            ASP.desktopmodules_ati_base_controls_ati_slimcontrol_ascx slim = atiSlimControl.Visible ? atiSlimControl : (atiSlimFBSetup.Visible ? atiSlimFBSetup : null);
            if (slim != null)
            {
                if (!string.IsNullOrEmpty(slim.LocLat))
                {
                    us.DefaultMapLat = Convert.ToDouble(slim.LocLat);
                    us.LatHome = us.DefaultMapLat;
                }
                if (!string.IsNullOrEmpty(slim.LocLng))
                {
                    us.DefaultMapLng = Convert.ToDouble(slim.LocLng);
                    us.LngHome = us.DefaultMapLng;
                }
                us.RemoteAddress = slim.Address;
            }
            UserSettingsExtended[] webLinks = entities.UserSettingsExtendeds.Where(s => s.UserSetting.Id == usId && s.Class == 1).ToArray();
            if (atiWebLinks.Visible)
            {
                // Facebook
                UserSettingsExtended facebook = webLinks.FirstOrDefault(s => s.Name == "Facebook");
                if (!string.IsNullOrEmpty(atiWebLinks.Facebook))
                {
                    if (facebook == null)
                    {
                        facebook = new UserSettingsExtended()
                        {
                            Class = 1,
                            Name = "Facebook",
                            Value = atiWebLinks.Facebook
                        };
                        us.UserSettingsExtendeds.Add(facebook);
                    }
                    else
                    {
                        facebook.Value = atiWebLinks.Facebook;
                    }
                }
                else if (facebook != null)
                {
                    entities.DeleteObject(facebook);
                }

                // Twitter
                UserSettingsExtended twitter = webLinks.FirstOrDefault(s => s.Name == "Twitter");
                if (!string.IsNullOrEmpty(atiWebLinks.Twitter))
                {
                    if (twitter == null)
                    {
                        twitter = new UserSettingsExtended()
                        {
                            Class = 1,
                            Name = "Twitter",
                            Value = atiWebLinks.Twitter
                        };
                        us.UserSettingsExtendeds.Add(twitter);
                    }
                    else
                    {
                        twitter.Value = atiWebLinks.Twitter;
                    }
                }
                else if (twitter != null)
                {
                    entities.DeleteObject(twitter);
                }

                // YouTube
                UserSettingsExtended youtube = webLinks.FirstOrDefault(s => s.Name == "YouTube");
                if (!string.IsNullOrEmpty(atiWebLinks.YouTube))
                {

                    if (youtube == null)
                    {
                        youtube = new UserSettingsExtended()
                        {
                            Class = 1,
                            Name = "YouTube",
                            Value = atiWebLinks.YouTube
                        };
                        us.UserSettingsExtendeds.Add(youtube);
                    }
                    else
                    {
                        youtube.Value = atiWebLinks.YouTube;
                    }
                }
                else if (youtube != null)
                {
                    entities.DeleteObject(youtube);
                }

                // LinkedIn
                UserSettingsExtended linkedin = webLinks.FirstOrDefault(s => s.Name == "LinkedIn");
                if (!string.IsNullOrEmpty(atiWebLinks.LinkedIn))
                {
                    if (linkedin == null)
                    {
                        linkedin = new UserSettingsExtended()
                        {
                            Class = 1,
                            Name = "LinkedIn",
                            Value = atiWebLinks.LinkedIn
                        };
                        us.UserSettingsExtendeds.Add(linkedin);
                    }
                    else
                    {
                        linkedin.Value = atiWebLinks.LinkedIn;
                    }
                }
                else if (linkedin != null)
                {
                    entities.DeleteObject(linkedin);
                }

                // Flickr
                UserSettingsExtended flickr = webLinks.FirstOrDefault(s => s.Name == "Flickr");
                if (!string.IsNullOrEmpty(atiWebLinks.Flickr))
                {
                    if (flickr == null)
                    {
                        flickr = new UserSettingsExtended()
                        {
                            Class = 1,
                            Name = "Flickr",
                            Value = atiWebLinks.Flickr
                        };
                        us.UserSettingsExtendeds.Add(flickr);
                    }
                    else
                    {
                        flickr.Value = atiWebLinks.Flickr;
                    }
                }
                else if (flickr != null)
                {
                    entities.DeleteObject(flickr);
                }

                // Personal
                UserSettingsExtended personal = webLinks.FirstOrDefault(s => s.Name == "Personal");
                if (!string.IsNullOrEmpty(atiWebLinks.Peronsal))
                {
                    if (personal == null)
                    {
                        personal = new UserSettingsExtended()
                        {
                            Class = 1,
                            Name = "Personal",
                            Value = atiWebLinks.Peronsal
                        };
                        us.UserSettingsExtendeds.Add(personal);
                    }
                    else
                    {
                        personal.Value = atiWebLinks.Peronsal;
                    }
                }
                else if (personal != null)
                {
                    entities.DeleteObject(personal);
                }

                entities.SaveChanges();
            }
            return us;
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

