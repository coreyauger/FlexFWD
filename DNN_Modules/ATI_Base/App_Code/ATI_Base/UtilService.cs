using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Affine.Data;

namespace Affine.WebService
{

    /// <summary>
    /// Summary description for UtilService
    /// </summary>
    [WebService(Namespace = "http://aqufit.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UtilService : System.Web.Services.WebService
    {
        private aqufitEntities aqufitEntities = new aqufitEntities();

        public UtilService()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string SaveUnitChangeSetting(long uid, int pId, string type, int units)
        {
            UserSettings aqufitSettings = aqufitEntities.UserSettings.FirstOrDefault<UserSettings>(us => us.UserKey == uid && us.PortalKey == pId);
            if (aqufitSettings != null)
            {
                Affine.Utils.UnitsUtil.MeasureUnit u = (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), units);
                switch (type.ToLower())
                {
                    case "weight":
                        if (u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS || u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG)  // check units
                        {
                            aqufitSettings.WeightUnits = (short)u;
                        }
                        break;
                    case "height":                        
                        if (u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN || u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM)  // check units
                        {
                            aqufitSettings.HeightUnits = (short)u;
                        }
                        break;
                    case "distance":
                        if (u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES || u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KM)  // check units
                        {
                            aqufitSettings.DistanceUnits = (short)u;
                        }
                        break;
                    case "bodymeasure":
                        if (u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES || u == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM) // check units
                        {
                            aqufitSettings.BodyMeasureUnits = (short)u;
                        }
                        break;                    
                }
                aqufitEntities.SaveChanges();
                return "SUCESS";
            }
            // TODO: log an alerts (iphone alert)
            //DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
            //objEventLog.AddLog(objNewUser, PortalSettings, objNewUser.UserID, atiSlimControl.Email, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);
            string body = "UtilService.SaveUnitChangeSetting: No UserSettings for user Id: " + uid + " Portal: " + pId;
            DotNetNuke.Services.Mail.Mail.SendMail("corey@aqufit.com", "coreyauger@gmail.com", "", "App ERROR", body, "", "HTML", "", "", "", "");


            return "FAIL";
        }

    }
}

