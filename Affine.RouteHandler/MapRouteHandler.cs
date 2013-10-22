﻿using System;
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
    public class WorkoutMapRouteHandler : RouteHandler
    {
        private static int PortalId;
        private static int TabId;

        public static void init(int portal, int tab)
        {
            PortalId = portal;
            TabId = tab;
            if (RouteTable.Routes["WorkoutMapRoute"] == null)
            {
                RouteTable.Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
                RouteTable.Routes.Add("WorkoutMapRoute", new Route("workout/{w}/route/", new WorkoutMapRouteHandler("~/Default.aspx"))
                {
                    //Defaults = new RouteValueDictionary { { "tabid", "57" }, { "u", "-1" } }
                });
            }
        }


        public WorkoutMapRouteHandler(string virtualPath)
        {
            base.VirtualPath = virtualPath;
        }

        public override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return GetDnnHttpHandler(requestContext, WorkoutMapRouteHandler.PortalId, WorkoutMapRouteHandler.TabId, new string[] { "w" });            
        }
    }
}
