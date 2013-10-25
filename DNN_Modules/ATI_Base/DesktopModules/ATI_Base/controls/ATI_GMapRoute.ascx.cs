using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_GMapRoute : DotNetNuke.Framework.UserControlBase
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

    public double? Lat{ get; set; }
    public double? Lng{ get; set; }
    public short? Zoom { get; set; }
    public Affine.Utils.UnitsUtil.MeasureUnit Units { get; set; }

    public int FitZoom { get { return Convert.ToInt32(atiFitZoomLevel.Value); } }
    public int MapWidth { get { return Convert.ToInt32(atiMapWidth.Value); } }
    public int MapHeight { get { return Convert.ToInt32(atiMapHeight.Value); } }
    public string RouteTitle { get { return (!string.IsNullOrWhiteSpace(atiRouteTitle.Value) ? atiRouteTitle.Value : "Untitled"); } }
    public double RouteDistance { get { return Convert.ToDouble( atiRouteDistance.Value )*1000; } } // return in meters
    public int WorkoutTypeKey { get { return Convert.ToInt32(atiRouteWorkoutType.Value); } }
    public Affine.Data.json.MapRoutePoint[] MapRoutePoints
    {
        get {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Deserialize<Affine.Data.json.MapRoutePoint[]>(atiLatLngArray.Value);
        }
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback )
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "AtiGMapRoute", GetGMapData(), true);
            atiWorkoutTypes.SelectedType = Affine.Utils.WorkoutUtil.WorkoutType.RUNNING;
        }
    }

    private string GetGMapData()
    {
        System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        string js = string.Empty;
        js += "Aqufit.Page." + this.ID + " = new Aqufit.Page.Controls.ATI_GMapRoute('" + this.ID + "', '" + map_canvas.ClientID + "', " + (int)this.Units + ");";
        if (this.Lat == null || this.Lng == null)
        {   // center on North America
            this.Lat = 39.6395;
            this.Lng = -95.4492;
            this.Zoom = 2;
        }
        else if( this.Zoom == null )
        {
            this.Zoom = 16;
        }
        js += "Aqufit.Page."+this.ID+".setup('"+jsSerializer.Serialize(new { Zoom = this.Zoom, Lat = this.Lat, Lng = this.Lng }) + "');";
        js += "$('#dialog button').css({'float':'right'}).button().click(function (event) {";
        js += "if( Aqufit.Page." + this.ID + " != null ){ Aqufit.Page." + this.ID + ".routeName = $('#atiRouteName').val(); var address =  $('#atiAddress').val(); ";
        js += "if( address != '' ){  Aqufit.Page."+this.ID+".gotoAddress(address);}$('#dialog').dialog('close');} event.stopPropagation(); return false; }); ";
        return js;
    }
}
