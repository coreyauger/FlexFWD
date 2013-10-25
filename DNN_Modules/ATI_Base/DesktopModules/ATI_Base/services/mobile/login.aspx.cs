using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

public partial class services_json_login : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        object json = new { Status = "Login Failed" };
        if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
        {
            aqufitEntities entities = new aqufitEntities();
            string uname = Request.Form["u"];
            string password = Request.Form["p"];
            if (uname.Contains("@"))
            {   // this is an email login                
                User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserEmail == uname);
                if (user == null)
                {
                    json = new { Status = "Email ERROR" };
                    Response.Write(json);
                    Response.Flush();
                    Response.End();
                    return;
                }
                uname = user.UserName;
            }
            uname = uname.ToLower();
            DotNetNuke.Security.Membership.UserLoginStatus status = DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_FAILURE;
            DotNetNuke.Entities.Portals.PortalController pc = new DotNetNuke.Entities.Portals.PortalController();
            DotNetNuke.Entities.Portals.PortalInfo pi = pc.GetPortal(0);
            UserInfo uinfo = UserController.UserLogin(0, uname, password, null, pi.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), ref status, true);            
            if (status == DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_SUCCESS || status == DotNetNuke.Security.Membership.UserLoginStatus.LOGIN_SUPERUSER)
            {
                UserSettings usersettings = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserKey == uinfo.UserID && u.PortalKey == 0);
                if (!usersettings.Guid.HasValue)
                {   // we only add a UUID if there was none before.. this is so the "remember me" on the desktop site will still work.
                    usersettings.Guid = Guid.NewGuid();
                    entities.SaveChanges();
                }
                json = new { Status = "SUCCESS", Token = usersettings.Guid.ToString(), UserId = usersettings.Id, Username = usersettings.UserName };
            }            
        }
        Response.Write(serializer.Serialize( json ));
        Response.Flush();
        Response.End();
    }
}