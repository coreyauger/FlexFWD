using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;
using Affine.Utils.Linq;

public partial class services_json_Friends : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();

    //$response[] = array($i, $name, null, '<img src="images/'. $filename . (file_exists('images/' . $filename . '.jpg') ? '.jpg' : '.png') .'" /> ' . $name);

    protected void Page_Load(object sender, EventArgs e)
    {                        
        try
        {
            Items["UserId"] = 8;
            if (Items["UserId"] != null && Request["search"] != null)
            {
                string search = Request["search"];

                aqufitEntities entities = new aqufitEntities();
                long uid = Convert.ToInt64(Items["UserId"]);
                IList<long> friendIds = entities.UserFriends.Where(f => (f.PortalKey == 0) && (f.SrcUserKey == uid || f.DestUserKey == uid)).Select(f => (f.SrcUserKey == uid ? f.DestUserKey : f.SrcUserKey)).ToList();
                UserSettings[] firendSettings = entities.UserSettings.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(s => s.UserKey, friendIds)).Where(f => f.UserName.ToLower().Contains(search) || f.UserFirstName.ToLower().Contains(search) || f.UserLastName.ToLower().Contains(search)).ToArray();
                //
                object[] response = firendSettings.Select(f => new object[] { "" + f.UserKey, f.UserName + " (" + f.UserFirstName + "," + f.UserLastName + ")", null, "<img src=\"" + ResolveUrl("~/services/images/profile.aspx?u=" + f.UserKey) + "\" align=\"middle\"/>&nbsp;&nbsp;" + f.UserName + " (" + f.UserFirstName + "," + f.UserLastName + ")" }).ToArray();                
                //object[] response = {new object[]{ "5", "fdsafda dfsa fdsa", null, null } };
                Response.Write(serializer.Serialize(response));
                Response.End();
            }
        }
        catch (Exception)
        {
            //Affine.Data.json.MapRoute route = new Affine.Data.json.MapRoute() { Id = -1 };
            //Response.Write(serializer.Serialize(route));
        }
    }
}