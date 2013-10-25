using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Affine.Web.Controls
{
    /// <summary>
    /// Summary description for ATI_ValidationBaseControl
    /// </summary>
    public class ATI_ValidationBaseCompositeControl : DotNetNuke.Entities.Modules.PortalModuleBase
    {

        private string GetJsValidation()
        {
            string ret = string.Empty;
            ret += "var " + this.ID + "Array = [];";
            int i = 0;
            foreach (Control c in this.Controls)
            {
                if (c is ATI_ValidationBaseControl)
                {
                    ret += this.ID + "Array["+(i++)+"] = " + c.ID + ";";
                }
            }
            ret += "var " + this.ID + " = new AtiValidationGroup(" + this.ID + "Array);";
            return ret;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiValidation" + this.ID, GetJsValidation(), true);
        }        

    }
}