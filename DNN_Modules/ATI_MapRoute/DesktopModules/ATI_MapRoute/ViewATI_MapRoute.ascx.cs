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

namespace Affine.Dnn.Modules.ATI_MapRoute
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
    partial class ViewATI_MapRoute : ATI_PermissionPageBase, IActionable
    {

        #region Private Members
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
                 //   atiMapRoute.Text = GetFlexEmbed();                    
                    if ((base.UserSettings.LatHome != null && base.UserSettings.LatHome.Value > 0.0))
                    {
                        atiGMapRoute.Lat = base.UserSettings.LatHome.Value;
                        atiGMapRoute.Lng = base.UserSettings.LngHome.Value;
                        atiGMapRoute.Zoom = 13;
                    }  
                    // set the users prefered units
                    atiGMapRoute.Units = base.UserSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit( Convert.ToInt32( base.UserSettings.DistanceUnits) ) : Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }

        private IList<Affine.Data.MapRoutePoint> CullPoints(IList<Affine.Data.MapRoutePoint> pointCopy, int len)
        {
            int i = 0;
            while (pointCopy.Count > len)
            {
                if (i++ % 2 == 0)
                {
                    pointCopy.RemoveAt(i);
                }
                if (i >= pointCopy.Count-1)
                {
                    i = 0;
                }
            }
            return pointCopy;
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            switch (hiddenAjaxAction.Value)
            {
                case "saveRoute":
                    aqufitEntities entities = new aqufitEntities();                    
                    Affine.Data.MapRoute route = new MapRoute()
                    {
                        Name = atiGMapRoute.RouteTitle,
                        PortalKey = this.PortalId,                  // TODO: send this in
                        UserSetting = entities.UserSettings.First( u => u.Id == UserSettings.Id ),
                        RouteDistance = atiGMapRoute.RouteDistance,                    
                        CreationDate = DateTime.Now,
                        WorkoutTypeId = atiGMapRoute.WorkoutTypeKey
                    };

                    entities.AddToMapRoutes(route);
                    double zoom = atiGMapRoute.FitZoom;
                    double w = atiGMapRoute.MapWidth/200.0;
                    double h = atiGMapRoute.MapHeight/100.0;
                    double r = w > h ? w : h;
                    zoom = r > 0 ? zoom - Math.Sqrt(r) : zoom + Math.Sqrt(r);
                    route.ThumbZoom = (short)Math.Floor(zoom);

                    Affine.Data.MapRoutePoint[] points = atiGMapRoute.MapRoutePoints.Select(mp => new Affine.Data.MapRoutePoint() { Lat = mp.Lat, Lng = mp.Lng, Order = mp.Order, MapRoute = route }).ToArray<Affine.Data.MapRoutePoint>();               
                    route.MapRoutePoints.Concat(points);
                    double minLat = double.MaxValue;
                    double minLng = double.MaxValue;
                    double maxLat = double.MinValue;
                    double maxLng = double.MinValue;
                    foreach( Affine.Data.MapRoutePoint p in points )
                    {
                        if (p.Lat < minLat)
                        {
                            minLat = p.Lat;
                        }
                        if (p.Lng < minLng)
                        {
                            minLng = p.Lng;
                        }
                        if (p.Lat > maxLat)
                        {
                            maxLat = p.Lat;
                        }
                        if (p.Lng > maxLng)
                        {
                            maxLng = p.Lng;
                        }
                    }
                    route.LatMax = maxLat;
                    route.LatMin = minLat;
                    route.LngMax = maxLng;
                    route.LngMin = minLng;
                    // Cull points until there are 100 or less points to make the encoded polyline fit the 512 bytes
                    int len = 100;
                    IList<Affine.Data.MapRoutePoint> pointCopy = atiGMapRoute.MapRoutePoints.Select(mp => new Affine.Data.MapRoutePoint() { Lat = mp.Lat, Lng = mp.Lng, Order = mp.Order }).ToList<Affine.Data.MapRoutePoint>();
                    pointCopy = CullPoints(pointCopy, len);
                    route.PolyLineEncoding = Affine.Utils.PolylineEncoder.createEncodings(pointCopy, 17, 1)["encodedPoints"].ToString();
                    while (route.PolyLineEncoding.Length > 512)
                    {
                        len = len/2;
                        pointCopy = CullPoints(pointCopy, len);
                        route.PolyLineEncoding = Affine.Utils.PolylineEncoder.createEncodings(pointCopy, 17, 1)["encodedPoints"].ToString();
                    }
                    entities.SaveChanges();
                    MapRoute passback = entities.MapRoutes.Where(m => m.UserSetting.Id == this.UserSettings.Id && m.PortalKey == this.PortalId ).OrderByDescending( m => m.Id).FirstOrDefault();
                    // routes creater needs the route to show up in their route list now.
                    User2MapRouteFav fav = new User2MapRouteFav()
                    {
                        MapRoute = passback,
                        UserSettingsKey = UserSettings.Id
                    };
                    entities.AddToUser2MapRouteFav(fav);
                    entities.SaveChanges();

                    Affine.Data.json.MapRoute mr = new Affine.Data.json.MapRoute() { Id = passback.Id, Name = passback.Name, RouteDistance = passback.RouteDistance };                    
                    RadAjaxManager1.ResponseScripts.Add(" setSelectedMap(" + passback.Id + ", '" + passback.Name + "', " + passback.RouteDistance + ", ''); ");
                    break;
            }
            // save and close  RadAjaxManager1.ResponseScripts.Add();  /  
        }      

        /*
        private string GetFlexEmbed()
        {
            string ret = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" id=\"ATI_MapRoute\" width=\"100%\" height=\"100%\" codebase=\"http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab\">";
            ret += "<param name=\"movie\" value=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/ATI_Aqufit.swf") + "\" />";
            ret += "<param name=\"quality\" value=\"high\" /><param name=\"bgcolor\" value=\"#FFFFFF\" />";
            ret += "<param name=\"flashvars\" value=\"u=" + this.UserId + "&m=design&url=http://flexfwd.com/\" />";
            ret += "<param name=\"allowScriptAccess\" value=\"sameDomain\" />";
            ret += "<param name=\"wmode\" VALUE=\"transparent\">";
            ret += "<embed src=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/ATI_Aqufit.swf") + "\" wmode=\"transparent\" quality=\"high\" bgcolor=\"#FFFFFF\" width=\"100%\" height=\"100%\" name=\"ATI_MapRoute\" align=\"middle\" play=\"true\" loop=\"false\" quality=\"high\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\"";
            ret += " flashvars=\"u=" + this.UserId + "&m=design&url=http://flexfwd.com/\"";
            ret += " pluginspage=\"http://www.adobe.com/go/getflashplayer\"></embed></object>";
            return ret;
        }
            */          

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

