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
using System.Xml.Linq;
using System.Net;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

using Affine.Data;

using Telerik.Web.UI;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace Affine.Dnn.Modules.ATI_Routes
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
    partial class ViewATI_Routes : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
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
            base.Page_Load(sender, e);            
            try
            {
                ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                service.InlineScript = true;
                ScriptManager.GetCurrent(Page).Services.Add(service);
                imgAd.Src = ResolveUrl("~/images/iphoneAd.png");
                imgCheck.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
                
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    imgSearch.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iSearch.png");
                    imgCenter.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCenter.png");
                    aqufitEntities entities = new aqufitEntities();
                    long rId = 0;
                    if (HttpContext.Current.Items["r"] != null)
                    {
                        rId = Convert.ToInt64(HttpContext.Current.Items["r"]);
                    }
                    else if (Request["r"] != null)
                    {
                        rId = Convert.ToInt64(Request["r"]);
                    }
                    else if (HttpContext.Current.Items["w"] != null)
                    {
                        long workoutId = Convert.ToInt64(HttpContext.Current.Items["w"]);
                        rId = entities.UserStreamSet.OfType<Workout>().Where(w => w.Id == workoutId).Select(w => w.WorkoutExtendeds.FirstOrDefault().MapRoute.Id).FirstOrDefault();
                    }
                    else if (Request["w"] != null)
                    {
                        long workoutId = Convert.ToInt64(Request["w"]);
                        rId = entities.UserStreamSet.OfType<Workout>().Where(w => w.Id == workoutId).Select(w => w.WorkoutExtendeds.FirstOrDefault().MapRoute.Id).FirstOrDefault();
                    }
                    // Are we viewing a specific route ?
                    if (rId > 0)
                    {
                        hiddenRouteKey.Value = "" + rId;
                        
                        MapRoute route = entities.MapRoutes.Include("UserSetting").Include("MapRoutePoints").FirstOrDefault(m => m.Id == rId);
                        if( base.UserSettings != null ){                            
                            bAddRoute.Visible = entities.User2MapRouteFav.FirstOrDefault(mr => mr.MapRoute.Id == route.Id && mr.UserSettingsKey == UserSettings.Id) == null;
                        }
                        Affine.Utils.UnitsUtil.MeasureUnit unit = this.UserSettings != null && this.UserSettings.DistanceUnits != null ? Affine.Utils.UnitsUtil.ToUnit( this.UserSettings.DistanceUnits.Value ) : Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
                        string dist = Math.Round( Affine.Utils.UnitsUtil.systemDefaultToUnits(route.RouteDistance, unit), 2 ) + " " + Affine.Utils.UnitsUtil.unitToStringName(unit);
                        lRouteTitle.Text = route.Name + " (" + dist + ")";
                        lRouteInfo.Text = "<span>A " + dist + " route posted by <a class=\"username\" href=\"/" + route.UserSetting.UserName + "\">" + route.UserSetting.UserName + "</a> on "+route.CreationDate.ToShortDateString()+"</span>";
                        double centerLat = (route.LatMax + route.LatMin) / 2;
                        double centerLng = (route.LngMax + route.LngMin) / 2;
                        atiGMapView.Lat = centerLat;
                        atiGMapView.Lng = centerLng;
                        if (route.ThumbZoom.HasValue)
                        {
                            atiGMapView.Zoom = (short)(route.ThumbZoom.Value + 2);
                        }
                        atiGMapView.Route = route;
                        atiProfileImg.Settings = route.UserSetting;
                        atiRoutePanel.Visible = false;
                        atiRouteViewer.Visible = true;

                        string js = string.Empty;

                        //js += "Affine.WebService.StreamService.GetRoutes(" + centerLat + ", " + centerLng + ", 10, 0, 5, 'distance', function (json) { ";
                        js += "Aqufit.Page.atiSimilarRouteListScript.dataBinder = function(skip, take){ \n";
                        js += "     Affine.WebService.StreamService.GetSimilarRoutes(" + rId + ", 10, skip, take, 'distance', function (json) { \n";
                        js += "         Aqufit.Page.atiSimilarRouteListScript.generateStreamDom(json); \n";
                        js += "     }); \n";
                        js += "};";
                        js += " Aqufit.Page.atiSimilarRouteListScript.dataBinder(0,5); ";
                        atiShareLink.ShareLink = "http://"+Request.Url.Host + "/route/" + route.Id;
                        atiShareLink.ShareTitle = "FlexFWD.com Mapped Route: \"" + route.Name +"\"";

                        routeTabTitle.Text = "&nbsp;" + (string.IsNullOrWhiteSpace(route.Name) ? "Untitled" : route.Name);

                        Affine.WebService.StreamService ss = new WebService.StreamService();
                        string json = ss.getStreamDataForRoute(route.Id, 0, 5);
                        //generateStreamDom

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "SimilarRouteList", "$(function(){ " + js + " Aqufit.Page.atiStreamScript.generateStreamDom('" + json + "'); });", true);
                    }
                    else
                    {
                        if (Settings["Configure"] != null && Convert.ToString( Settings["Configure"] ).Equals("ConfigureMyRoutes") )
                        {
                            atiMyRoutePanel.Visible = true;
                            mapContainer.Visible = false; 
                            atiRoutePanel.Visible = true;
                            routeTabTitle.Text = "My Routes";
                            liMyRoutes.Visible = false;
                            liFindRoute.Visible = true;
                            WebService.StreamService streamService = new WebService.StreamService();
                            string json = streamService.GetMyRoutes(base.UserSettings.Id, 0, 10, "date");
                            string js = string.Empty;
                            //js += "Affine.WebService.StreamService.GetRoutes(" + centerLat + ", " + centerLng + ", 10, 0, 5, 'distance', function (json) { ";
                            js += "$(function(){ Aqufit.Page.atiRouteListScript.isMyRoutes = true; Aqufit.Page.atiRouteListScript.generateStreamDom('" + json + "'); \n";
                            js += "     Aqufit.Page.atiRouteListScript.dataBinder = function(skip, take){ \n";
                            js += "         Affine.WebService.StreamService.GetMyRoutes(" + base.UserSettings.Id + ", skip, take, 'date', function (json) { \n";
                            js += "             Aqufit.Page.atiRouteListScript.generateStreamDom(json); \n";
                            js += "         }); \n";
                            js += "     } \n";
                            js += " }); \n";

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "RouteList", js, true);
                        }
                        else
                        {
                            routeTabTitle.Text = "Routes";
                            atiRoutePanel.Visible = true;
                            atiRouteViewer.Visible = false;
                            SetupPage();        // do normal page setup  
                        }
                          
                    }              
                }
                           
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }       

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                aqufitEntities entities = new aqufitEntities();
                switch (hiddenAjaxAction.Value)
                {
                    case "addRoute":
                        // few things to do here... 
                        // 1) check to see if this route is already in the fav.
                        long rid = Convert.ToInt64(hiddenAjaxValue.Value);
                        User2MapRouteFav check = entities.User2MapRouteFav.FirstOrDefault(f => f.UserSettingsKey == this.UserSettings.Id && f.MapRoute.Id == rid);
                        if (check != null)
                        {
                            // TODO: postback a message saying that the route is already there.
                        //    status = "Route already in list.  TODO: dialog with a way to 'view my routes' | 'record workout for route'";
                        }
                        else
                        {   // add the route.
                            MapRoute mr = entities.MapRoutes.First(r => r.Id == rid);
                            User2MapRouteFav fav = new User2MapRouteFav()
                            {
                                UserSettingsKey = this.UserSettings.Id,
                                MapRoute = mr
                            };
                            entities.AddToUser2MapRouteFav(fav);
                            entities.SaveChanges();
                            //TODO: dialog with a way to 'view my routes' | 'record workout for route'";
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Windows.RouteAddedDialog.open(); ");                            
                        }
                        break;
                    case "remRoute":
                        // few things to do here... 
                        long remid = Convert.ToInt64(hiddenAjaxValue.Value);
                        User2MapRouteFav toRem = entities.User2MapRouteFav.FirstOrDefault(f => f.UserSettingsKey == this.UserSettings.Id && f.MapRoute.Id == remid);
                        if (toRem != null)
                        {
                            // remove the route from fav... any workouts will still be logged though.
                            entities.DeleteObject(toRem);
                            entities.SaveChanges();
                         //   status = "TODO:";
                        }                       
                        break;
                    case "delStream":
                        long sid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteStream(UserSettings, sid);
                        break;
                    case "delComment":
                        long cid = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Data.Managers.LINQ.DataManager.Instance.deleteComment(UserSettings, cid);
                        break;
                }
            }
            catch (Exception ex)
            {
                // TODO: better error handling
                RadAjaxManager1.ResponseScripts.Add( " alert('" + ex.Message + "');");
            }
        }


        private void SetupPage()
        {
            // center USA if we dont have anything to go on
           // double Lat = 39.6395;
           // double Lng = -95.4492;
           // short Zoom = 4;
            double Lat = 49.2844;
            double Lng = -123.1258;
            short Zoom = 10;

            if (UserSettings != null && UserSettings.LatHome.HasValue && UserSettings.LngHome.HasValue)
            {
                Lat = UserSettings.LatHome.Value;
                Lng = UserSettings.LngHome.Value;
                Zoom = 10;
            }
            atiGMap.Lat = Lat;
            atiGMap.Lng = Lng;
            atiGMap.Zoom = Zoom;
            /*
            if (!string.IsNullOrEmpty(hiddenLat.Value))
            {
                atiGMap.Lat = Convert.ToDouble(hiddenLat.Value);
            }
            if (!string.IsNullOrEmpty(hiddenLng.Value))
            {
                atiGMap.Lng = Convert.ToDouble(hiddenLng.Value);
            }
            if (!string.IsNullOrEmpty(hiddenZoom.Value))
            {
                atiGMap.Zoom = Convert.ToInt16(hiddenZoom.Value);
            }
             */
            hiddenLat.Value = "" + Lat;
            hiddenLng.Value = "" + Lng;
            hiddenZoom.Value = "" + Zoom;
            WebService.StreamService streamService = new WebService.StreamService();
            string json = streamService.GetRoutes(Lat, Lng, 5.0, 0, 10, "date");
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "RouteList", "$(function(){ Aqufit.Page.atiRouteListScript.generateStreamDom('" + json + "'); });", true);
        }



        protected void atiRadComboBoxSearchRoutes_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboSearchWorkouts = (RadComboBox)sender;
            atiRadComboSearchWorkouts.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<MapRoute> routes = entities.User2MapRouteFav.Where(r => r.UserSettingsKey == UserSettings.Id).Select(w => w.MapRoute);
            routes = routes.OrderBy(r => r.Name);
            int length = routes.Count();
            routes = string.IsNullOrEmpty(e.Text) ? routes.Skip(itemOffset).Take(TAKE) : routes.Where(r => r.Name.ToLower().StartsWith(e.Text)).Skip(itemOffset).Take(TAKE);

            MapRoute[] routeArray = routes.ToArray();

            foreach (MapRoute r in routeArray)
            {
                RadComboBoxItem item = new RadComboBoxItem(r.Name);
                item.Value = "" + r.Id;
                atiRadComboSearchWorkouts.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }

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

