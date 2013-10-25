using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_MapRoute : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["u"]) && !string.IsNullOrEmpty(Request["json"]))
            {
                string jsonString = Request["json"];
                
                Affine.Data.json.MapRoute mapRoute = serializer.Deserialize<Affine.Data.json.MapRoute>(jsonString);

                long userKey = Convert.ToInt64(Request["u"]);
                Affine.Data.MapRoute route = new MapRoute()
                {
                    Name = mapRoute.Name,
                    City = mapRoute.City,
                    Region = mapRoute.Region,
                    Rating = mapRoute.Rating,                    
                    PortalKey = 0,                  // TODO: send this in
                    UserKey = userKey,
                    RouteDistance = mapRoute.RouteDistance,
                    LatMax = mapRoute.LatMax,
                    LatMin = mapRoute.LatMin,
                    LngMax = mapRoute.LngMax,
                    LngMin = mapRoute.LngMin,
                    CreationDate = DateTime.Now                   
                };
                Affine.Data.MapRoutePoint[] points = mapRoute.MapRoutePoints.Select(mp => new Affine.Data.MapRoutePoint() { Lat = mp.Lat, Lng = mp.Lng, Order = mp.Order, MapRoute = route }).ToArray<Affine.Data.MapRoutePoint>();               
                route.MapRoutePoints.Concat(points);
                aqufitEntities entities = new aqufitEntities();
                entities.AddToMapRoutes(route);
                long id = entities.SaveChanges();
                MapRoute passback = entities.MapRoutes.Where(m => m.UserKey == userKey ).OrderByDescending( m => m.Id).FirstOrDefault();
                Affine.Data.json.MapRoute mr = new Affine.Data.json.MapRoute() { Id = passback.Id, Name = passback.Name, RouteDistance = passback.RouteDistance };
                string json = serializer.Serialize(mr);
                Response.Write(json);
            }
            else
            {
                Affine.Data.json.MapRoute route = new Affine.Data.json.MapRoute() { Id = -1 };
                Response.Write(serializer.Serialize(route));
            }
        }
        catch (Exception)
        {
            Affine.Data.json.MapRoute route = new Affine.Data.json.MapRoute() { Id = -1 };
            Response.Write(serializer.Serialize(route));
        }
    }
}