using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

public partial class DesktopModules_ATI_Base_controls_ATI_TotalDistColors : DotNetNuke.Framework.UserControlBase
{
    public double TotalDistance { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
              ScriptManager.RegisterStartupScript(this, Page.GetType(), "totalDistanceColors", "Aqufit.Page."+this.ID+" = new Aqufit.Page.Controls.ATI_TotalDistColors('"+this.ID+"'); Aqufit.Page."+this.ID+".setupDistance("+this.TotalDistance+");", true);    
        }
    }    
}
