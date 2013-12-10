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

namespace Affine.Dnn.Modules.ATI_Register
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
                    cbBeta.Checked = TabModuleSettings["Beta"] != null ? Convert.ToBoolean(TabModuleSettings["Beta"]) : false;
                    cbHideBirthday.Checked = TabModuleSettings["HideBirthday"] != null ? Convert.ToBoolean(TabModuleSettings["HideBirthday"]) : false;
                    cbHideGender.Checked = TabModuleSettings["HideGender"] != null ? Convert.ToBoolean(TabModuleSettings["HideGender"]) : false;
                    cbHideHeight.Checked = TabModuleSettings["HideHeight"] != null ? Convert.ToBoolean(TabModuleSettings["HideHeight"]) : false;
                    cbHideWeight.Checked = TabModuleSettings["HideWeight"] != null ? Convert.ToBoolean(TabModuleSettings["HideWeight"]) : false;
                    cbHideFitnessLevel.Checked = TabModuleSettings["HideFitnessLevel"] != null ? Convert.ToBoolean(TabModuleSettings["HideFitnessLevel"]) : false;
                    cbRpxReciever.Checked = TabModuleSettings["RpxReciever"] != null ? Convert.ToBoolean(TabModuleSettings["RpxReciever"]) : false;
                    txtRpxApiToken.Text = TabModuleSettings["RpxApiToken"] != null ? Convert.ToString(TabModuleSettings["RpxApiToken"]) : "";
                    txtRpxReturnUrl.Text = TabModuleSettings["RpxReturnUrl"] != null ? Convert.ToString(TabModuleSettings["RpxReturnUrl"]) : "";
                    ddlConfigure.SelectedValue = TabModuleSettings["Configure"] != null ? Convert.ToString(TabModuleSettings["Configure"]) : "ConfigureUserRegister";
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
                ModuleController objModules = new ModuleController();                
                objModules.UpdateTabModuleSetting(TabModuleId, "LandingPage", txtLandingPage.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "Beta", Convert.ToString(cbBeta.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "HideBirthday", Convert.ToString( cbHideBirthday.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "HideGender", Convert.ToString(cbHideGender.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "HideHeight", Convert.ToString(cbHideHeight.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "HideWeight", Convert.ToString(cbHideWeight.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "HideFitnessLevel", Convert.ToString(cbHideFitnessLevel.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "RpxReciever", Convert.ToString(cbRpxReciever.Checked));
                objModules.UpdateTabModuleSetting(TabModuleId, "RpxApiToken", txtRpxApiToken.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "RpxReturnUrl", txtRpxReturnUrl.Text);
                objModules.UpdateTabModuleSetting(TabModuleId, "Configure", Convert.ToString(ddlConfigure.SelectedValue));
                
                //objModules.UpdateTabModuleSetting(TabModuleId, "template", txtTemplate.Text);
                //refresh cache
                ModuleController.SynchronizeModule(this.ModuleId);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
   
}
}


