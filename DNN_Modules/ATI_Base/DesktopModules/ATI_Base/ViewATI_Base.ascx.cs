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

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

using System.Web;
using System.Net;


using Affine.Data;

namespace Affine.Dnn.Modules.ATI_Base
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
    partial class ViewATI_Base : PortalModuleBase, IActionable
    {

        #region Private Members

        
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
            try
            {
                lErrorText.Text = "";
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    atiStepSelect.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/stepSelect.png");
                    if (string.IsNullOrEmpty((string)Settings["LandingPage"]))
                    {
                        ShowError("WARNING: No Landing Page set.  Contact Administrator");
                    }
                    //CATALooK.ProductsDB productDB = new CATALooK.ProductsDB();
                    //string product = productDB.GetProductbyProductID(Convert.ToInt32(Settings["ProductId"]), PortalId);
                    //if (product == null || product.Equals(string.Empty))
                    //{
                    //    throw new Exception("No Monthly Membership Product Setup. Contact Administrator");
                    //}
                    if (Request["r"] != null && !Request["r"].Equals(string.Empty))
                    {
                        int rid = Convert.ToInt32(Request["r"]);
                        DotNetNuke.Entities.Users.UserInfo user = DotNetNuke.Entities.Users.UserController.GetUser(PortalId, rid, true, true);
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
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

       
        private void ShowError( string msg )
        {
            lErrorText.Text = msg;      
        }

        protected void bPostBack_Click(object sender, EventArgs e)
        {
        }

        protected void bSubmitRegister_Click(object sender, EventArgs e)
        {
            if (Register())
            {      
                //Response.Redirect((string)Settings["LandingPage"], true);
            }
        
        }

        private UserInfo InitialiseUser()
        {

            UserInfo newUser = new UserInfo();
            // Initialise the ProfileProperties Collection
            newUser.Profile.InitialiseProfile(PortalId);
            return newUser;

        }

        protected void bRegister_Click(object sender, EventArgs e)
        {
            if (!atiSlimControl.RadCaptcha.IsValid)
            {
                RadAjaxManager1.ResponseScripts.Add("AjaxResponseFail('" + atiSlimControl.RadCaptcha.ErrorMessage + "');");
                return;
            }
            if (!Register())
            {
                // TODO: Error check
                RadAjaxManager1.ResponseScripts.Add("AjaxResponseFail('" + lErrorText.Text + "');");
            }
            else
            {
                bRegister.Visible = false;
                bAddress.Visible = true;
                RadAjaxManager1.ResponseScripts.Add("AjaxResponseSuccess();");
            }
        }

        protected void bAddress_Click(object sender, EventArgs e)
        {
            // TODO: check inputs for error
            UserInfo.Profile.Street = atiAddress.Street;
            UserInfo.Profile.City = atiAddress.City;
            UserInfo.Profile.Region = atiAddress.Region;
            UserInfo.Profile.PostalCode = atiAddress.Postal;
            UserInfo.Profile.Country = atiAddress.Country;
            UserController.UpdateUser(PortalId, UserInfo);
            bAddress.Visible = false;
            bBodyComposition.Visible = true;
            RadAjaxManager1.ResponseScripts.Add("AjaxResponseSuccess();");
            // TODO: need to store the google map point
        }

        protected void bBodyComposition_Click(object sender, EventArgs e)
        {
            // TODO: check inputs for error
            UserSettings userSettings = new UserSettings();           
            if (atiBodyComposition.BirthDate == null)
            {
                lErrorText.Text = "You must enter your Birthdate.";
                return;
            }
            if (string.IsNullOrEmpty( atiBodyComposition.Weight ))
            {
                lErrorText.Text = "You must enter your weight.";
                return;
            }
            userSettings.BirthDate = Convert.ToDateTime(atiBodyComposition.BirthDate);
            //userSettings.DistanceUnits = (short)atiBodyComposition.DistanceUnits;
            userSettings.HeightUnits = (short)atiBodyComposition.HeightUnits;
            userSettings.WeightUnits = (short)atiBodyComposition.WeightUnits;
            userSettings.UserKey = UserId;
            userSettings.PortalKey = PortalId;
            userSettings.Sex = atiBodyComposition.Gender;

            aqufitEntities aqufitEntities = new aqufitEntities();
            aqufitEntities.AddToUserSettings(userSettings);


            BodyComposition bc = new BodyComposition()
            {
                UserKey = UserId,
                PortalKey = PortalId,
                FitnessLevel = atiBodyComposition.FitnessLevel
            };

            // need height and weight conversions
            bc.Modified = DateTime.Now;
            bc.BodyFatPercent = 0.0;
            bc.Weight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyComposition.Weight), atiBodyComposition.WeightUnits);

           // bc.Height = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyComposition.Height), atiBodyComposition.HeightUnits);
            aqufitEntities.AddToBodyComposition(bc);

            BodyMeasurments bm = new BodyMeasurments()
            {
                UserKey = UserId,
                PortalKey = PortalId
            };
            Affine.Utils.UnitsUtil.MeasureUnit bmUnits = atiBodyComposition.HeightUnits == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES ? Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES : Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM;
            if (!string.IsNullOrEmpty(atiBodyMeasurements.Chest)) bm.Chest = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.Chest), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.Neck)) bm.Neck = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.Neck), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.Shoulders)) bm.Shoulders = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.Shoulders), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.Stomach)) bm.Stomach = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.Stomach), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.Waist)) bm.Waist = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.Waist), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.Hips)) bm.Hips = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.Hips), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.BicepLeft)) bm.BicepLeft = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.BicepLeft), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.BicepRight)) bm.BicepRight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.BicepRight), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.ForearmLeft)) bm.ForearmLeft = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.ForearmLeft), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.ForearmRight)) bm.ForearmRight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.ForearmRight), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.ThighLeft)) bm.ThighLeft = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.ThighLeft), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.ThighRight)) bm.ThighRight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.ThighRight), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.CalfLeft)) bm.CalfLeft = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.CalfLeft), bmUnits);
            if (!string.IsNullOrEmpty(atiBodyMeasurements.CalfLeft)) bm.CalfRight = Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiBodyMeasurements.CalfRight), bmUnits);
            bm.Modified = DateTime.Now;
            aqufitEntities.AddToBodyMeasurments(bm);

            aqufitEntities.SaveChanges();

            bBodyComposition.Visible = false;
            bProfile.Visible = true;
            RadAjaxManager1.ResponseScripts.Add("AjaxResponseSuccess();");
        }

        protected void bProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect(Convert.ToString(Settings["LandingPage"]), true);
        }
        


        private bool Register()
        {
            //Try
            // Only attempt a save/update if all form fields on the page are valid
            if (Page.IsValid)
            {
                // check required fields               
                int UID = -1;
                //     Dim userCreateStatus As UserCreateStatus = userCreateStatus.AddUser               
                UserInfo objUserInfo = UserController.GetUserByName(PortalId, atiSlimControl.Email, false);
                // if a user is found with that username, error.
                // this prevents you from adding a username
                // with the same name as a superuser.
                if (objUserInfo != null)
                {
                    lErrorText.Text = "We already have an entry for the User Name.";
                    return false;
                }            
                UserInfo objNewUser = InitialiseUser(); // TODO: this method
                int affiliateId = 0;
                if (Request.Cookies["AffiliateId"] != null)
                {
                    affiliateId = Convert.ToInt32(Request.Cookies["AffiliateId"]);
                }
                objNewUser.PortalID = PortalId;
                objNewUser.Profile.FirstName = atiSlimControl.FirstName;
                objNewUser.Profile.LastName = atiSlimControl.LastName;
                objNewUser.FirstName = objNewUser.Profile.FirstName;
                objNewUser.LastName = objNewUser.Profile.LastName;
                objNewUser.DisplayName = atiSlimControl.Email;
                objNewUser.Profile.Unit = string.Empty;
                objNewUser.Profile.Street = string.Empty;
                objNewUser.Profile.City = string.Empty;
                objNewUser.Profile.Region = string.Empty;
                objNewUser.Profile.PostalCode = string.Empty;
                objNewUser.Profile.Country = string.Empty;                
                objNewUser.Email = atiSlimControl.Email;
                objNewUser.Username = atiSlimControl.UserName;
                objNewUser.Membership.Password = atiSlimControl.Password;
                objNewUser.Membership.Approved = true; // Convert.ToBoolean((PortalSettings.UserRegistration != PortalRegistrationType.PublicRegistration ? false : true));
                objNewUser.AffiliateID = affiliateId;                                    
                DotNetNuke.Security.Membership.UserCreateStatus userCreateStatus = UserController.CreateUser(ref objNewUser);
                if (userCreateStatus == DotNetNuke.Security.Membership.UserCreateStatus.Success)
                {
                    UID = objNewUser.UserID;
                    // DNN3 BUG
                    DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                    objEventLog.AddLog(objNewUser, PortalSettings, objNewUser.UserID, atiSlimControl.Email, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.USER_CREATED);

                    // send notification to portal administrator of new user registration                       
                    DotNetNuke.Services.Mail.Mail.SendMail(objNewUser, DotNetNuke.Services.Mail.MessageType.UserRegistrationAdmin, PortalSettings);
                    DotNetNuke.Services.Mail.Mail.SendMail(objNewUser, DotNetNuke.Services.Mail.MessageType.UserRegistrationPublic, PortalSettings);
                    if (affiliateId > 0)
                    {
                        DotNetNuke.Services.Vendors.AffiliateController objAffiliates = new DotNetNuke.Services.Vendors.AffiliateController();
                        objAffiliates.UpdateAffiliateStats(affiliateId, 0, 1);
                    }
                    DataCache.ClearUserCache(PortalId, objNewUser.Username);
                    UserController.UserLogin(PortalId, objNewUser, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);
                    string body = "<br/>ID: " + objNewUser.UserID;
                           body += "<br/>User: " + objNewUser.Username;
                           body += "<br/>First Name: " + objNewUser.FirstName;
                           body += "<br/>Last Name: " + objNewUser.LastName;
                    DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "corey@aqufit.com", "", "NEW aqufit.com USER", body, "", "HTML", "", "", "", "");
                }
                else
                { // registration error                   
                    string errStr = userCreateStatus.ToString() + "\n\n" + atiSlimControl.ToString();
                    DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "corey@aqufit.com", "", "FAILED REGISTRATION", lErrorText.Text + errStr, "", "HTML", "", "", "", "");
                    lErrorText.Text = "REGISTRATION ERROR: " + userCreateStatus.ToString();                   
                    return false;
                }
            }
            return true;
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

