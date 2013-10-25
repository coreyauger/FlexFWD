using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_HighChart : DotNetNuke.Framework.UserControlBase
{
    public Unit Width
    {
        get
        {
            if (ViewState["Width"] != null)
            {
                return (Unit)ViewState["Width"];
            }
            return Unit.Percentage(100.0);
        }
        set
        {
            ViewState["Width"] = value;
        }
    }
    public string CardioColor { get; set; }

    public Unit Height
    {
        get
        {
            if (ViewState["Height"] != null)
            {
                return (Unit)ViewState["Height"];
            }
            return Unit.Percentage(100.0);
        }
        set
        {
            ViewState["Height"] = value;
        }
    }

    private IList<string> _Categories = new List<string>();
    public IList<string> Categories
    {
        get { return _Categories; }
    }

    private IList<Affine.Data.json.HighChartSeries> _SeriesList = new List<Affine.Data.json.HighChartSeries>();
    public IList<Affine.Data.json.HighChartSeries> SeriesList
    {
        get { return _SeriesList; }
    }
   
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            atiWorkoutHighChartPanel.Height = this.Height;
            atiWorkoutHighChartPanel.Width = this.Width;

            if (this.Visible)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "AtiWorkoutHighChart", GetChartData(), true);
            }
        }
    }

    private string GetChartData()
    {
        System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        string js = string.Empty;
        js += "Aqufit.Page." + this.ID + " = new Aqufit.Page.Controls.ATI_WorkoutHighChart('"+this.ID+"'); ";
        /*
        foreach (Affine.Data.json.HighChartSeries series in SeriesList)
        {
            js += "Aqufit.Page." + this.ID + ".pushSeries(" + jsSerializer.Serialize(series) + "); ";
        }
        js += "Aqufit.Page."+this.ID+".categories = "+jsSerializer.Serialize(this.Categories.ToArray())+"; ";
        */
        if (string.IsNullOrEmpty(this.CardioColor))
        {
            this.CardioColor = "#0095cd";
        }
        js += "Aqufit.Page." + this.ID + ".colors = ['" + this.CardioColor + "','#004a71'];";
        //js += "Aqufit.Page."+this.ID+".drawChart(); ";
        return js;
    }
}
