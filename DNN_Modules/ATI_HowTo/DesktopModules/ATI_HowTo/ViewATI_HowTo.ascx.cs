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
using System.IO;

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
using Telerik.Web.UI.Upload;


using Affine.Data;

namespace Affine.Dnn.Modules.ATI_HowTo
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
    partial class ViewATI_HowTo : ATI_PermissionPageBase, IActionable
    {

        #region Private Members
        private long _hId = 0;
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
                base.Page_Load(sender, e);
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    _hId = Request["h"] != null ? Convert.ToInt64(Request["h"]) : 1;                
                    aqufitEntities entities = new aqufitEntities();
                    HelpPage helpPage = entities.HelpPages.FirstOrDefault(h => h.Id == _hId);
                    litContent.Text = helpPage.Content;
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void RadTreeView1_DataBound(object sender, EventArgs e)
        {
            if (_hId > 0)
            {
                RadTreeNode node = RadTreeView1.FindNodeByValue("" + _hId);
                node.Selected = true;
                node.Expanded = true;
                node.ExpandParentNodes();
            }
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

