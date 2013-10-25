using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_workoutCreate : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;
    
    /*
    <u>{userId}</u>
	<t>{token}</t>
	<ds>{dateSrc}</ds>
    <d>{description}</d>
	<n>{workoutName}</n>
	<wt>{WODType}</wt>
	<at>{amrapTime}</at>
     */

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
        {
            object json = new{ Id = -1, Status = "FAIL", Msg = "" };
            try
            {
                aqufitEntities entities = new aqufitEntities();
                long uid = Convert.ToInt64(Request.Form["u"]);
                string token = Convert.ToString(Request.Form["t"]);     // our limited security model ;)
                int wodType = Convert.ToInt32(Request.Form["wt"]);
                string description = Convert.ToString(Request.Form["d"]);
                string name = Convert.ToString(Request.Form["n"]);
                long dataSrc = Convert.ToInt64(Request.Form["ds"]);
                long amrapTime = Convert.ToInt64(Request.Form["at"]);

                Guid gToken = Guid.Parse(token);
                User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid && u.Guid == gToken);

                Affine.Data.json.Exercise[] none = new Affine.Data.json.Exercise[0];
                long wodId = dataManager.CreateWOD(user.Id, name, description, wodType, amrapTime, none);               
                json = new { Id = wodId, Status = "SUCCESS", Msg = "" };
            }
            catch (Exception ex)
            {
                json = new { Id = -1, Status = "FAIL", Msg = ex.Message.Replace("'", "") };
            }
            Response.Write(serializer.Serialize(json));
            Response.Flush();
            Response.End();
        }
    }
}