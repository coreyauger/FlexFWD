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
    public class CompareRouteHandler : RouteHandler
    {
        private static int PortalId;
        private static int TabId;
        
        public static void init(int portal, int tab)
        {
            PortalId = portal;
            TabId = tab;
            if (RouteTable.Routes["CompareRoute"] == null)
            {
                RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
                RouteTable.Routes.Add(new Route("{resource}.aspx", new StopRoutingHandler()));
                RouteTable.Routes.Add(new Route("{resource}.txt", new StopRoutingHandler()));
                RouteTable.Routes.Add(new Route("{resource}.html", new StopRoutingHandler()));
                RouteTable.Routes.Add(new Route("recipe/{all*}", new StopRoutingHandler()));
                RouteTable.Routes.Add("CompareRoute", new Route("compare/{u}", new CompareRouteHandler("~/Default.aspx"))
                {
                    //Defaults = new RouteValueDictionary { { "tabid", "57" }, { "u", "-1" } }
                });
            }
        }

        public CompareRouteHandler(string virtualPath)
        {
            base.VirtualPath = virtualPath;
        }      

        public override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return GetDnnHttpHandler(requestContext, CompareRouteHandler.PortalId, CompareRouteHandler.TabId, new string[] { "u" });            
        }
    }
}
