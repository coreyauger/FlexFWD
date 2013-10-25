using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_GMap : DotNetNuke.Framework.UserControlBase
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
            mapWrap.Style["Width"] = this.Width.ToString();
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
            mapWrap.Style["height"] = this.Height.ToString();
        }
    }

    public double? Lat{ get; set; }
    public double? Lng{ get; set; }
    public short? Zoom { get; set; }

    public MapRoute Route { get; set; }

    public enum MapModes { GROUP_FINDER = 1, ROUTE_FINDER, ROUTE_VIEWER };
    public MapModes Mode
    {
        get
        {
            if (ViewState["MapModes"] != null)
            {
                return (MapModes)ViewState["MapModes"];
            }
            return MapModes.GROUP_FINDER;
        }
        set
        {
            ViewState["MapModes"] = value;
        }
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback )
        {
            if (this.Visible)
            {
                
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "AtiGMap", GetGMapData(), true);
            }
        }
    }

    private string GetGMapData()
    {
        System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        string js = string.Empty;
        js += "Aqufit.Page."+this.ID+" = new Aqufit.Page.Controls.ATI_GMap('"+this.ID+"', '"+map_canvas.ClientID+"');";
        if (this.Lat == null || this.Lng == null)
        { // this always get set now in the PermissionPageBase..
            this.Lat = 39.6395;
            this.Lng = -95.4492;
            this.Zoom = 4;            
        }
        else if( this.Zoom == null )
        {
            this.Zoom = 16;
        }       
        string mode = "group";
        if (this.Mode == MapModes.ROUTE_FINDER)
        {
            mode = "route";
        }
        else if (this.Mode == MapModes.ROUTE_VIEWER)
        {
            mode = "routeview";
            mapWrap.Style["height"] = "450px";
        }
        if (this.Route != null)
        {
            MapRoute m = this.Route;
            var RouteData = new { Id = m.Id, Name = m.Name, LatMin = m.LatMin, LngMin = m.LngMin, LatMax = m.LatMax, LngMax = m.LngMax, Distance = m.RouteDistance, Rating = m.Rating, Zoom = m.ThumbZoom, UserKey = m.UserSetting.Id, UserName = m.UserSetting.UserName, RoutePoints = m.MapRoutePoints.OrderBy(p => p.Order).Select(p => new { Lat = p.Lat, Lng = p.Lng }).ToArray() };
            js += "Aqufit.Page." + this.ID + ".setup('" + jsSerializer.Serialize(new { Zoom = this.Zoom, Lat = this.Lat, Lng = this.Lng, RouteData = RouteData }) + "', '" + mode + "');";
        }
        else
        {
            js += "Aqufit.Page." + this.ID + ".setup('" + jsSerializer.Serialize(new { Zoom = this.Zoom, Lat = this.Lat, Lng = this.Lng, RouteData = "" }) + "', '" + mode + "');";
        }
        return js;
    }
}
