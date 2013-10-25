using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_comment : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
    //$response[] = array($i, $name, null, '<img src="images/'. $filename . (file_exists('images/' . $filename . '.jpg') ? '.jpg' : '.png') .'" /> ' . $name);

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
        {
            object json = new { Status = "FAIL", Msg = "" };
            try
            {
                aqufitEntities entities = new aqufitEntities();
                long uid = Convert.ToInt64(Request.Form["u"]);
                string token = Convert.ToString(Request.Form["t"]);     // our limited security model ;)
                long sk = Convert.ToInt64(Request.Form["sk"]);
                long profileKey = Convert.ToInt64(Request.Form["p"]);
                string comment = Convert.ToString(Request.Form["c"]);
                Guid guid = Guid.Parse(token);
                User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid && u.Guid == guid);

                Affine.Data.Managers.LINQ.DataManager.Instance.AddComment(user.UserKey, user.PortalKey, profileKey, sk, comment);
                json = new { Status = "SUCCESS", Msg = "" };
            }
            catch (Exception ex)
            {
                json = new { Status = "FAIL", Msg = ex.Message.Replace("'", "") };
            }
            Response.Write(serializer.Serialize(json));
            Response.Flush();
            Response.End();
        }
    }
}