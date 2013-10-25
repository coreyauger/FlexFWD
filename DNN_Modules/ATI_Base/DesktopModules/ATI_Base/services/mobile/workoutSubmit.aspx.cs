using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_workoutSubmit : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;

    /*
    <u>{userId}</u>
    <t>{token}</t>
    <d>{dateStr}</d>
    <wt>{workoutType}</wt>
    <w>{WODId}</w>
    <s>{score}</s>
    <l>{distance}</l>
    <n>{notes}</n>
     <h>{title}</h>
     <r>{rx}</r>
     */

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
        {
            object json = new{ Status = "FAIL", Msg = "" };
            try
            {
                aqufitEntities entities = new aqufitEntities();
                long uid = Convert.ToInt64(Request.Form["u"]);
                string token = Convert.ToString(Request.Form["t"]);     // our limited security model ;)
                int workoutType = Convert.ToInt32(Request.Form["wt"]);
                DateTime date = Convert.ToDateTime(Request.Form["d"]);
                long wodKey = Convert.ToInt64(Request.Form["w"]);
                double score = Convert.ToDouble(Request.Form["s"]);
                double distance = Convert.ToDouble(Request.Form["l"]);
                string notes = Convert.ToString(Request.Form["n"]);
                string title = Convert.ToString(Request.Form["h"]);
                int rx = Convert.ToInt32(Request.Form["r"]);
                Guid gToken = Guid.Parse(token);
                User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid && u.Guid == gToken);
                if (workoutType == 6)
                {
                    dataManager.SaveWorkout((UserSettings)user, 
                                                (long)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT, 
                                                (int)Affine.Utils.WorkoutUtil.DataSrc.MOBILE, 
                                                date, 
                                                (long)score, 
                                                notes,
                                                (rx != 0), 
                                                wodKey, 
                                                score, 
                                                (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);                           
                }
                else
                {
                    dataManager.SaveWorkout((UserSettings)user, 
                                            (long)Affine.Utils.WorkoutUtil.IntToWorkoutType(workoutType),
                                            (int)Affine.Utils.WorkoutUtil.DataSrc.MOBILE, 
                                            date, 
                                            (long)score,
                                            distance, 
                                            0, 
                                            0, 
                                            0,
                                            0,
                                            title, 
                                            notes);
                }
                //Affine.Data.Managers.LINQ.DataManager.Instance.AddComment(user.UserKey, user.PortalKey, profileKey, sk, comment);
                json = new { Status = "SUCCESS", Msg = "" + (long)score };
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