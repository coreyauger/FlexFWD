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
    public class ContactsRouteHandler : RouteHandler
    {
        private static int PortalId;
        public static void init(int portal)
        {
            PortalId = portal;
            if (RouteTable.Routes["ContactsRoute"] == null)
            {
                RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
                RouteTable.Routes.Add("ContactsRoute", new Route("{t}/{u}/{s}/contacts", new ContactsRouteHandler("~/Default.aspx"))
                {
                    //Defaults = new RouteValueDictionary { { "tabid", "57" }, { "u", "-1" } }
                });
            }
        }

        public ContactsRouteHandler(string virtualPath)
        {
            base.VirtualPath = virtualPath;
        }

        public override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.HttpContext.Items["oauth_token"] = requestContext.HttpContext.Request["oauth_token"];
            requestContext.HttpContext.Items["oauth_verifier"] = requestContext.HttpContext.Request["oauth_verifier"];
            int tab = Convert.ToInt32(requestContext.RouteData.Values["t"]);
            return GetDnnHttpHandler(requestContext, ContactsRouteHandler.PortalId, tab, new string[] { "u", "s" });            
        }
    }
}
