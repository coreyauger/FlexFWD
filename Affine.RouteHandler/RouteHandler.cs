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
    public abstract class RouteHandler : IRouteHandler
    {
        public string VirtualPath { get; protected set; }

        public abstract IHttpHandler GetHttpHandler(RequestContext requestContext);       
        
        public IHttpHandler GetDnnHttpHandler(RequestContext requestContext, int portal, int tab, string[] passThrough)
        {
            PortalController pcontroller = new PortalController();
            PortalInfo pinfo = pcontroller.GetPortal(portal);
            PortalAliasController pacontroller = new PortalAliasController();
            PortalAliasCollection pacollection = pacontroller.GetPortalAliasByPortalID(portal);
            //pacollection.
            //PortalSettings psettings = new PortalSettings(pinfo);
            PortalSettings psettings = new PortalSettings(tab, portal);               // 64 is the stats tab. TODO: get by page name and not hardcoded id
            foreach (string key in pacollection.Keys)
            {
                psettings.PortalAlias = pacollection[key];
            }
            TabController tcontroller = new TabController();
            // psettings.ActiveTab = tcontroller.GetTab(57, 0, true);                  // 57 is the profile tab.
            requestContext.HttpContext.Items["PortalSettings"] = psettings;

            requestContext.HttpContext.Items["UrlRewrite:OriginalUrl"] = requestContext.HttpContext.Request.RawUrl;
            //UserInfo uinfo = requestContext.HttpContext.User == null ? new UserInfo() : UserController.GetUserByName(psettings.PortalId, requestContext.HttpContext.User.Identity.Name);
            UserInfo uinfo = requestContext.HttpContext.User == null ? new UserInfo() : UserController.GetCachedUser(psettings.PortalId, requestContext.HttpContext.User.Identity.Name);
            requestContext.HttpContext.Items["UserInfo"] = uinfo;
            foreach (string s in passThrough)
            {
                requestContext.HttpContext.Items[s] = requestContext.RouteData.Values[s];
            }
            IHttpHandler page = BuildManager.CreateInstanceFromVirtualPath(VirtualPath, typeof(DotNetNuke.Framework.PageBase)) as IHttpHandler;
            return page;
        }
    }
}
