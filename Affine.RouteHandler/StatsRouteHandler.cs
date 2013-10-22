using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Routing;
using System.Web.Compilation;
using System.Web.SessionState;

using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Tabs;

namespace Affine.RouteHandler
{
    public class StatsRouteHandler : RouteHandler
    {
        // static constructor should get executed "ONE" time when class loads
        static StatsRouteHandler()
        {
            if (RouteTable.Routes["StatsRoute"] == null)
            {
                RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
                RouteTable.Routes.Add("StatsRoute", new Route("{u}/workout/{w}", new StatsRouteHandler("~/Default.aspx"))
                {
                    //Defaults = new RouteValueDictionary { { "tabid", "57" }, { "u", "-1" } }
                });
            }
        }

        // explicit call to make sure that we init the handler
        public static void init()
        {
            if (RouteTable.Routes["StatsRoute"] == null)
            {
                RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
                RouteTable.Routes.Add("StatsRoute", new Route("{u}/workout/{w}", new StatsRouteHandler("~/Default.aspx"))
                {
                    //Defaults = new RouteValueDictionary { { "tabid", "57" }, { "u", "-1" } }
                });
            }
        }


        public StatsRouteHandler(string virtualPath)
        {
            base.VirtualPath = virtualPath;
        }


        public override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return GetDnnHttpHandler(requestContext, 0, 64, new string[] { "u", "w" });    // 64 is the stats tab. TODO: get by page name and not hardcoded id                        
        }
    }
}
