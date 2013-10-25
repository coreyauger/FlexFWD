using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

/// <summary>
/// TODO: this is a TEMP webservice class until I make a WCF service.
/// </summary>
public partial class services_json_LeaderBoard : System.Web.UI.Page
{
    private Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;

    protected void Page_Load(object sender, EventArgs e)
    {
        try{
           // Response.ContentType = "application/json";
            if (!string.IsNullOrEmpty(Request["g"]))
            {
                aqufitEntities entities = new aqufitEntities();
                string gname = Request["g"];
                Group group = entities.UserSettings.OfType<Group>().FirstOrDefault(g => string.Compare(g.UserName, gname, true) == 0);
                Affine.Data.json.LeaderBoardWOD[] leaderBoard = dataMan.CalculatCrossFitLeaderBoard(group.Id);
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                string jsfile = System.IO.File.ReadAllText(Server.MapPath("~/DesktopModules/ATI_Base/resources/scripts/leaders.js"));
                Response.Write("var __ati_group = {Name:'"+group.UserFirstName+"', UserName:'"+group.UserName+"', Id:"+group.Id+"}; var __ati_json = '"+jserializer.Serialize(leaderBoard)+"';");
                Response.Write(jsfile);
            }        
            else
            {
                Response.Write("{ERROR:'invalid request'}");
            }
        }catch(Exception){
            Response.Write("{ERROR:'exception in request'}");
        }
    }
}