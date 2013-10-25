using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_Uploadify : DotNetNuke.Framework.UserControlBase
{
    private bool _showInfo = false;

    public bool ShowInfoHeader
    {
        get { return _showInfo; }
        set { _showInfo = value; }
    }

    public string InfoHeaderText
    {
        get;
        set;
    }

    public string SucessCallbackFunction
    {
        get;
        set;
    }

    public string Action
    {
        get;
        set;
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (string.IsNullOrEmpty(this.SucessCallbackFunction))
            {
                this.SucessCallbackFunction = "null";
            }
            infoHeader.Visible = this.ShowInfoHeader;
            infoHeaderText.Text = this.InfoHeaderText;
          //  ScriptManager.RegisterStartupScript(this, Page.GetType(), "Uploadify"+this.ID, "(function(){Aqufit.Page."+this.ID+".init();})(); ", true);
        }
    }
}
