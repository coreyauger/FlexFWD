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
using System.IO;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DotNetNuke;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

namespace Affine.Dnn.Modules.ATI_NikePlus
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    partial class Settings : ModuleSettingsBase
    {
        private string pAlias;

        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    txtLandingPage.Text = TabModuleSettings["LandingPage"] != null ? Convert.ToString(TabModuleSettings["LandingPage"]) : "";
                    txtBlogUrl.Text = TabModuleSettings["BlogUrl"] != null ? Convert.ToString(TabModuleSettings["BlogUrl"]) : "";
                    txtSalesPhone.Text = TabModuleSettings["SalesPhone"] != null ? Convert.ToString(TabModuleSettings["SalesPhone"]) : "";

                    string welcomeText = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(TabModuleSettings, TabModuleId, "WelcomeText");
                    RadEditorWelcomeText.Content = welcomeText != null ? welcomeText : "";
                    string btestText = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(TabModuleSettings, TabModuleId, "BetaTestText");
                    RadEditorBetaTestText.Content = btestText != null ? btestText : "";
                    string investorText = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(TabModuleSettings, TabModuleId, "InvestorText");
                    RadEditorInvestorText.Content = investorText != null ? investorText : "";
                    string tyText = Affine.Dnn.ModuleController.ReadLargeTabModuleSetting(TabModuleSettings, TabModuleId, "ThankYouText");
                    RadEditorThankYouText.Content = tyText != null ? tyText : "";
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                Affine.Dnn.ModuleController objModules = new Affine.Dnn.ModuleController();
                objModules.UpdateTabModuleSetting(TabModuleId, "LandingPage", txtLandingPage.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "BlogUrl", txtBlogUrl.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "SalesPhone", txtSalesPhone.Text);
                objModules.UpdateLargeTabModuleSetting(TabModuleSettings, TabModuleId, "WelcomeText", RadEditorWelcomeText.Content);
                objModules.UpdateLargeTabModuleSetting(TabModuleSettings, TabModuleId, "BetaTestText", RadEditorBetaTestText.Content);
                objModules.UpdateLargeTabModuleSetting(TabModuleSettings, TabModuleId, "InvestorText", RadEditorInvestorText.Content);
                objModules.UpdateLargeTabModuleSetting(TabModuleSettings, TabModuleId, "ThankYouText", RadEditorThankYouText.Content);

                //objModules.UpdateTabModuleSetting(TabModuleId, "template", txtTemplate.Text);
                //refresh cache
                SynchronizeModule();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

    }
}


