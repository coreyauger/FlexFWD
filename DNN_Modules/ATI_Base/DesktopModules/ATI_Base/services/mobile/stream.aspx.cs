using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_stream : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
    //$response[] = array($i, $name, null, '<img src="images/'. $filename . (file_exists('images/' . $filename . '.jpg') ? '.jpg' : '.png') .'" /> ' . $name);

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
        {
            aqufitEntities entities = new aqufitEntities();
            long uid = Convert.ToInt64( Request.Form["u"] );
            Guid token = Guid.Parse( Request.Form["t"] );
            User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid && u.Guid == token);
            string json = ss.getStreamData(0, user.UserKey, user.PortalKey, user.UserKey, 0, 0, 0, 30);
            json = json.Replace(";", "");   // this messes up the json parser .. frig..
            Response.Write(json);
            Response.Flush();
            Response.End();
        }
    }
}