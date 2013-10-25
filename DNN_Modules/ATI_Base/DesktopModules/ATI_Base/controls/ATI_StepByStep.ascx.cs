using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

public partial class DesktopModules_ATI_Base_controls_ATI_StepByStep : DotNetNuke.Framework.UserControlBase
{


    public bool Step1Check { get; set; }
    public bool Step2Check { get; set; }
    public bool Step3Check { get; set; }
    public bool Step4Check { get; set; }

    public string Step1Link { get; set; }
    public string Step2Link { get; set; }
    public string Step3Link { get; set; }
    public string Step4Link { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            imgStep1Check.Visible = this.Step1Check;
            imgStep2Check.Visible = this.Step2Check;
            imgStep3Check.Visible = this.Step3Check;
            imgStep4Check.Visible = this.Step4Check;

            linkStep1.HRef = this.Step1Link;
            linkStep2.HRef = this.Step2Link;
            linkStep3.HRef = this.Step3Link;
            linkStep4.HRef = this.Step4Link;
        }
    }    
}
