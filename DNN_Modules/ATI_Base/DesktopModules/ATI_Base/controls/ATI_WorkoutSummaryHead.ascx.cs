using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

public partial class DesktopModules_ATI_Base_controls_ATI_WorkoutSummaryHead : DotNetNuke.Framework.UserControlBase
{

    public double Distance { get; set; }
    public Affine.Utils.UnitsUtil.MeasureUnit Units { get; set; }
    public int NumCardioWorkouts { get; set; }
    public double Calories { get; set; }
    public int NumStrength { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            double dist = Math.Round( Affine.Utils.UnitsUtil.systemDefaultToUnits(this.Distance, this.Units), 2);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "WorkoutSummaryHead", "\n Aqufit.Page." + this.ID + "  = new Aqufit.Page.Controls.ATI_WorkoutSummaryHead('" + this.ID + "'); \nAqufit.Page." + this.ID + ".set(" + dist + ", " + (int)this.Units + ", " + this.Calories + ", " + this.NumCardioWorkouts + ", " + this.NumStrength + ", '" + this.StartDate.ToShortDateString() + "', '" + this.EndDate.ToShortDateString() + "');", true);    
        }
    }    
}
