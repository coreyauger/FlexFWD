using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_workout : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
    //$response[] = array($i, $name, null, '<img src="images/'. $filename . (file_exists('images/' . $filename . '.jpg') ? '.jpg' : '.png') .'" /> ' . $name);

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        IList<object> json = new List<object>(); 
        json.Add( new { Id = 0, Name = "", Type =  1} );
        try
        {
            if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
            {
                aqufitEntities entities = new aqufitEntities();
                long uid = Convert.ToInt64(Request.Form["u"]);
                Guid token = Guid.Parse(Request.Form["t"]);
                User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid && u.Guid == token);
                DateTime date = Convert.ToDateTime(Request.Form["d"]);
                string search = Request.Form["s"];                
                if (string.IsNullOrWhiteSpace(search) && user.MainGroupKey.HasValue)
                {
                    WODSchedule schedule = entities.WODSchedules.Include("WOD").Include("WOD.WODType").FirstOrDefault(s => s.UserSetting.Id == user.MainGroupKey.Value && s.Date == date);
                    if (schedule != null)
                    {
                        json.RemoveAt(0);
                        json.Add(new { Id = schedule.WOD.Id, Name = schedule.WOD.Name, Type = schedule.WOD.WODType.Id });
                    }
                }
                else
                {
                    search = search.ToLower();
                    IQueryable<WOD> wods = entities.User2WODFav.Where(w => w.UserSetting.Id == user.Id).Select(w => w.WOD);
                    wods = wods.Union<WOD>(entities.WODs.Where(w => w.Standard > 0));
                    wods.Select(w => w.WODType).ToArray();  // hydrate WODTypes
                    wods = wods.Where(w => w.Name.ToLower().Contains(search)).OrderBy(w => w.Name).Take(10);
                    json = wods.Select(w => new { Id = w.Id, Name = w.Name, Type = w.WODType.Id }).ToArray();
                }
                
                
            }
        }
        catch (Exception ex)
        {
            json.RemoveAt(0);
            json.Add( new { WorkoutId = 0, Name = ex.Message } );
        }
        Response.Write(serializer.Serialize( json ));
        Response.Flush();
        Response.End();
    }
}