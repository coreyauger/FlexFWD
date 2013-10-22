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
    public class WODRouteHandler : RouteHandler
    {
        private static int PortalId;
        private static int TabId;

        public static void init(int portal, int tab)
        {
            PortalId = portal;
            TabId = tab;
            if (RouteTable.Routes["WODRoute"] == null)
            {
                RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
                RouteTable.Routes.Add("WODRoute", new Route("workout/{w}", new WODRouteHandler("~/Default.aspx"))
                {
                    //Defaults = new RouteValueDictionary { { "tabid", "57" }, { "u", "-1" } }
                });
            }
        }


        public WODRouteHandler(string virtualPath)
        {
            base.VirtualPath = virtualPath;
        }

        public override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return GetDnnHttpHandler(requestContext, WODRouteHandler.PortalId, WODRouteHandler.TabId, new string[] {"w" });            
        }
    }
}
