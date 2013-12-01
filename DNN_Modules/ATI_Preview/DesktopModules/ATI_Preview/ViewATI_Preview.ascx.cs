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

using System.Net;


using Affine.Data;

namespace Affine.Dnn.Modules.ATI_Preview
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
    partial class ViewATI_Preview : ATI_PageBase, IActionable
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
            base.Page_Load(sender, e);
            try
            {
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    imgPhone.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Preview/resources/images/iPhone.png");
                    imgWelcome.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Preview/resources/images/welcome.png");
                    imgInvestor.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Preview/resources/images/investor.png");
                    imgBeta.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Preview/resources/images/betatest.png");
                    imgThankyou.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Preview/resources/images/thankyou.png");
                    imgThankyouCheck.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Preview/resources/images/iCheck.png");
                    if (Settings["SalesPhone"] != null)
                    {
                        lSalesPhone.Text = Convert.ToString( Settings["SalesPhone"] );
                    }
                    else
                    {
                        panelSales.Visible = false;
                    }
                    divBlog.Visible = Settings["BlogUrl"] != null;
                    if (Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "WelcomeText") != null)
                    {
                        lWelcomeContent.Text = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "WelcomeText");
                    }
                    if (Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "InvestorText") != null)
                    {
                        lInvestorText.Text = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "InvestorText");
                    }
                    divInvestor.Visible = Settings["LandingPage"] != null;
                    if (Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "BetaTestText") != null)
                    {
                        lBetaTestText.Text = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "BetaTestText");
                    }
                    else
                    {
                        divBetaTest.Visible = false;
                    }
                    if (Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "ThankYouText") != null)
                    {
                        lThankYouText.Text = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(Settings, TabModuleId, "ThankYouText");
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        protected void bPostBack_Click(object sender, EventArgs e)
        {
            panelWelcom.Visible = false;
            panelLogin.Visible = false;
            panelBeta.Visible = false;
            panelThankYou.Visible = false;
            if (hiddenMode.Value == "beta")
            {
                panelBeta.Visible = true;
            }
            else if (hiddenMode.Value == "investor")
            {
                panelLogin.Visible = true;
            }
            else if (hiddenMode.Value == "welcom")
            {
                panelWelcom.Visible = true;
            }
        }

        private Preview CreatePreview(string fname, string lname, string email, string comments)
        {
            Preview preview = new Preview();
            if (fname != string.Empty)
            {
                preview.FirstName = fname;
            }
            else
            {
                // TODO: error
            }
            if (lname != string.Empty)
            {
                preview.LastName = lname;
            }
            else
            {
                // TODO: error
            }
            if (email != string.Empty)
            {
                preview.Email = email;
            }
            else
            {
                // TODO: error
            }
            preview.Comments = comments;
            return preview;
        }

        protected void bPreview_Click(object sender, EventArgs e)
        {
            aqufitEntities aqufitEntities = new aqufitEntities();
            Preview p = CreatePreview(atiPreview.FirstName, atiPreview.LastName, atiPreview.Email, atiPreview.Comments);
            p.Ip = Request.ServerVariables["REMOTE_ADDR"];
            p.IsBetaTester = false;
            p.EnteredByPortalId = PortalId;
            p.EnteredByUserId = UserId;
            p.EnteredByUserName = UserInfo.Username;
            aqufitEntities.AddToPreview(p);
            aqufitEntities.SaveChanges();
            string body = "User Added to Mail List<br />First Name: " + p.FirstName + "<br />Last Name: " + p.LastName + "<br />Email: " + p.Email + "<br />IP: " + p.Ip + "<br />Comments: <br />"+p.Comments;
            DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "coreyauger@gmail.com", "", "User Added to Mail List", body, "", "HTML", "", "", "", "");
            DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "kristindarguzas@gmail.com", "", "User Added to Mail List", body, "", "HTML", "", "", "", "");
            panelWelcom.Visible = false;
            panelThankYou.Visible = true;
            RadAjaxManager1.ResponseScripts.Add("AjaxResponseSuccess();");
        }
        protected void bLogin_Click(object sender, EventArgs e)
        {
   //         Response.Redirect(Convert.ToString(Settings["LandingPage"]), true);
        }
        protected void lbBlog_Click(object sender, EventArgs e)
        {
            Response.Redirect(Convert.ToString(Settings["BlogUrl"]), true);
        }
        protected void bBetaTest_Click(object sender, EventArgs e)
        {
            aqufitEntities aqufitEntities = new aqufitEntities();
            Preview p = CreatePreview(atiBetaTest.FirstName, atiBetaTest.LastName, atiBetaTest.Email, atiBetaTest.Comments);
            p.Ip = Request.ServerVariables["REMOTE_ADDR"];
            p.IsBetaTester = true;
            p.EnteredByPortalId = PortalId;
            p.EnteredByUserId = UserId;
            p.EnteredByUserName = UserInfo.Username;
            aqufitEntities.AddToPreview(p);
            aqufitEntities.SaveChanges();
            string body = "New Beta Tester signed up<br />First Name: " + p.FirstName + "<br />Last Name: " + p.LastName + "<br />Email: " + p.Email + "<br />IP: " + p.Ip;
            DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "coreyauger@gmail.com", "", "Beta Tester Signed up!", body, "", "HTML", "", "", "", "");
            DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "kristindarguzas@gmail.com", "", "Beta Tester Signed up!", body, "", "HTML", "", "", "", "");
            panelBeta.Visible = false;
            panelThankYou.Visible = true;
            RadAjaxManager1.ResponseScripts.Add("AjaxResponseSuccess();");
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

