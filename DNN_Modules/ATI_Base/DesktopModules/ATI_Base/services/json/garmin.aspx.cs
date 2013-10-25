using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using System.Drawing.Imaging;
using Affine.Data;

public partial class services_garmin : DotNetNuke.Framework.PageBase //System.Web.UI.Page
{

    private static byte[] UnGzip(byte[] data, int start)
    {
        int size = BitConverter.ToInt32(data, data.Length - 4);
        byte[] uncompressedData = new byte[size];
        MemoryStream memStream = new MemoryStream(data, start, (data.Length - start));
        memStream.Position = 0;
        GZipStream gzStream = new GZipStream(memStream, CompressionMode.Decompress);

        try
        {
            gzStream.Read(uncompressedData, 0, size);
        }
        catch (Exception)
        {
            throw;
        }

        gzStream.Close();
        return uncompressedData;
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        try
        {
            //   byte[] ret2 = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/profileSmall" + pid + "M.jpg"));
            //   Response.OutputStream.Write(ret2, 0, ret2.Length);
            if (Request.Form["fileType"] != null && Request.Form["gps_data"] != null)
            {
                string gps_data = Request.Form["gps_data"];
                // gps_data an xml file that has been gzip compressed then base64 encoded
                // first we need to remove the first and last line...
                gps_data = gps_data.Substring(gps_data.IndexOf("\n") + 1);      // remove first line
                gps_data = gps_data.Substring(0, gps_data.LastIndexOf("\n"));   // remove last line
                byte[] decbuff = Convert.FromBase64String(gps_data);
                
                byte[] buffer = UnGzip(decbuff, 0);
                string xml = System.Text.Encoding.UTF8.GetString(buffer);
                
                // we now need to parse the xml document for what we need..
                switch (Request.Form["fileType"])
                {
                    case "FitnessHistoryDirectory":
                        Response.Write(ParseFitnessHistoryDirectory(xml));
                        break;
                    case "FitnessHistoryDetail":
                        ParseFitnessHistoryDetail(xml);
                        break;
                }
            }

            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + ex.StackTrace);
            Response.Flush();
            Response.End();
        }
        //base.PortalSettings.UserId + "::"
    }


    public long GetNumberFromStr(string str)
    {
        str = str.Trim();
        string ret = string.Empty;
        foreach (char c in str)
        {
            if (c <= '9' && c >= '0')
            {
                ret += c;
            }
        }
        return Convert.ToInt64( ret );
    }

    /*
    private IList<WorkoutSample> DeviceSamplesForWorkout(WorkoutExtended workoutExted)
    {       
            IList<WorkoutSample> workoutSamples = new List<WorkoutSample>();
            int sequenceNum = 0;
            double minLat = double.MaxValue;
            double maxLat = double.MinValue;
            double minLng = double.MaxValue;
            double maxLng = double.MinValue;
            foreach (XElement sample in samples)
            {
                WorkoutSample workoutSample = DeviceSampleToWorkoutSampleAdaptor(sample);
                if (workoutSample.Lat != null && workoutSample.Lng != null)
                {
                    double lat = Convert.ToDouble(workoutSample.Lat);
                    double lng = Convert.ToDouble(workoutSample.Lng);
                    if (lat < minLat)
                    {
                        minLat = lat;
                    }
                    if (lat > maxLat)
                    {
                        maxLat = lat;
                    }
                    if (lng < minLng)
                    {
                        minLng = lng;
                    }
                    if (lng > maxLng)
                    {
                        maxLng = lng;
                    }
                }
                workoutSample.SampleNumber = sequenceNum++;
                workoutSample.WorkoutExtended = workoutExted;
                workoutSamples.Add(workoutSample);
            }
            workoutExted.LatMin = minLat;
            workoutExted.LatMax = maxLat;
            workoutExted.LngMin = minLng;
            workoutExted.LngMax = maxLng;

            return workoutSamples;
       
    }
     */
    
    private void ParseFitnessHistoryDetail(string xml){
        XElement doc = XElement.Parse(xml);
        XElement activity = doc.Descendants().FirstOrDefault(n => n.Name.LocalName == "Activity");

        aqufitEntities entities = new aqufitEntities();
        User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.UserKey == PortalSettings.UserId && u.PortalKey == PortalSettings.PortalId);

        Affine.Utils.WorkoutUtil.WorkoutType workoutType = Affine.Utils.WorkoutUtil.WorkoutType.RUNNING;
        string atype = (activity.Attribute("Sport").Value).ToLower();
        string title = string.Empty;
        switch (atype)
        {
            case "walking":
                workoutType = Affine.Utils.WorkoutUtil.WorkoutType.WALKING;
                title = "Walking ";
                break;
            case "cycling":
                workoutType = Affine.Utils.WorkoutUtil.WorkoutType.CYCLING;
                title = "Cycling ";
                break;
            case "swimming":
                workoutType = Affine.Utils.WorkoutUtil.WorkoutType.SWIMMING;
                title = "Swimming ";
                break;
            case "running":
            default:
                title = "Running ";
                workoutType = Affine.Utils.WorkoutUtil.WorkoutType.RUNNING;
                break;
        }
        string activityId = activity.Descendants().FirstOrDefault( n => n.Name.LocalName == "Id" ).Value;
        long thirdParyId = GetNumberFromStr(activityId);

        IEnumerable<XElement> laps = activity.Descendants().Where(n => n.Name.LocalName == "Lap");
        if (laps.Count() > 0)
        {   // we must have a lap
            // get the start time.
            XElement lap1 = laps.ElementAt(0);
            DateTime startTime = Convert.ToDateTime(lap1.Attribute("StartTime").Value);
            
            // total time
            double[] timesInSec = activity.Descendants().Where(n => n.Name.LocalName == "TotalTimeSeconds").Select(n => Convert.ToDouble( n.Value )).ToArray();
            double totalSec = 0.0;
            foreach (double t in timesInSec)
            {
                totalSec += t;
            }
            long duration = (long)Affine.Utils.UnitsUtil.unitsToSystemDefualt(totalSec, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_SEC);

            // total cal
            double[] calArray = activity.Descendants().Where(n => n.Name.LocalName == "Calories").Select(n => Convert.ToDouble( n.Value)).ToArray();
            double totalCal = 0.0;
            foreach (double c in calArray)
            {
                totalCal += c;
            }

            // total dist
            double[] distArray = activity.Descendants().Where(n => n.Name.LocalName == "DistanceMeters").Where(n => n.Parent.Name.LocalName == "Lap").Select(n => Convert.ToDouble(n.Value)).ToArray();
            double totalDist = 0.0;
            foreach (double d in distArray)
            {               
                totalDist += d;
            }
            
            Workout workout = new Workout()
            {
                Date = startTime.ToUniversalTime(),
                TimeStamp = DateTime.Now,
                Calories = totalCal,
                AccoutType = (short)Affine.Services.ThirdParty.AccountTypes.GARMIN,
                Description = "",
                Distance = Affine.Utils.UnitsUtil.unitsToSystemDefualt(totalDist, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M),
                Duration = duration,
                Emotion = null,
                PortalKey = (int)PortalSettings.PortalId,
                UserSetting = user,
                Terrain = null,
                ThirdPartyId = thirdParyId,
                Title = title,
                Weather = null,
                DataSrc = (int)Affine.Utils.WorkoutUtil.DataSrc.GARMIN,
                WorkoutTypeKey = (long)workoutType
            };
            // now we need to add the extended data ...
            
            // get all the track points
            XElement[] trackpoints = activity.Descendants().Where(n => n.Name.LocalName == "Trackpoint").ToArray();
          //  IList<WorkoutSample> workoutSamples = new List<WorkoutSample>();
            WorkoutExtended extended = new WorkoutExtended()
            {
                UserStream = workout              
            };
            entities.AddToWorkoutExtendeds(extended);
            int sequenceNum = 0;
            double minLat = double.MaxValue;
            double maxLat = double.MinValue;
            double minLng = double.MaxValue;
            double maxLng = double.MinValue;
            foreach (XElement point in trackpoints)
            {
                try
                {
                    double lat = Convert.ToDouble(point.Descendants().FirstOrDefault(n => n.Name.LocalName == "LatitudeDegrees").Value);
                    double lng = Convert.ToDouble(point.Descendants().FirstOrDefault(n => n.Name.LocalName == "LongitudeDegrees").Value);
                    DateTime time = Convert.ToDateTime(point.Descendants().FirstOrDefault(n => n.Name.LocalName == "Time").Value);
                    double distance = Convert.ToDouble(point.Descendants().FirstOrDefault(n => n.Name.LocalName == "DistanceMeters").Value);
                    distance = Affine.Utils.UnitsUtil.unitsToSystemDefualt(distance, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M);
                    XElement el = point.Descendants().FirstOrDefault(n => n.Name.LocalName == "AltitudeMeters");
                    double? elivation = null;
                    if (el != null)
                    {
                        elivation = Convert.ToDouble(el.Value);
                        elivation = Affine.Utils.UnitsUtil.unitsToSystemDefualt(elivation.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M);
                    }
                    if (lat < minLat)
                    {
                        minLat = lat;
                    }
                    if (lat > maxLat)
                    {
                        maxLat = lat;
                    }
                    if (lng < minLng)
                    {
                        minLng = lng;
                    }
                    if (lng > maxLng)
                    {
                        maxLng = lng;
                    }
                    WorkoutSample sample = new WorkoutSample()
                    {
                        Lat = lat,
                        Lng = lng,
                        SampleNumber = sequenceNum++,
                        WorkoutExtended = extended,
                        Date = time,
                        Elevation = elivation,
                        Distance = distance
                    };
                    entities.AddToWorkoutSamples(sample);
                }
                catch (NullReferenceException ex) { }   // just goto the next full sample
            }
            extended.LatMax = maxLat;
            extended.LatMin = minLat;
            extended.LngMax = maxLng;
            extended.LngMin = minLng;
            Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
            dataMan.SaveWorkout(entities, workout);
        }

    }

    private string ParseFitnessHistoryDirectory(string xml)
    {
        string ret = string.Empty;
        XElement doc = XElement.Parse(xml);
        string[] activityIds = doc.Descendants().Where( n => n.Name.LocalName == "Id" ).Select( n => n.Value ).ToArray();
        // now we only want to Ids that have not been imported.
        aqufitEntities entities = new aqufitEntities();
        var IdMap = activityIds.Select(s => new { ThirdPartyId = GetNumberFromStr(s), ActivityId = s }).ToArray();

        IdMap = IdMap.OrderByDescending(s => s.ThirdPartyId).ToArray();

        long? lastId = entities.UserStreamSet.OfType<Workout>().Where(w => 
            w.UserSetting.UserKey == PortalSettings.UserId && 
            w.UserSetting.PortalKey == PortalSettings.PortalId &&
            w.ThirdPartyId.HasValue).OrderByDescending(w => w.ThirdPartyId.Value).Select(w => w.ThirdPartyId.Value).FirstOrDefault();

        IList<string> newActivityIds = new List<string>();
        if (lastId != null)
        {
            foreach (var id in IdMap)
            {
                if (id.ThirdPartyId == lastId)
                {
                    break;
                }
                newActivityIds.Add(id.ActivityId);
            }
        }

        ret = "[";
        if (newActivityIds.Count > 0)
        {
            for (int i = 0; i < newActivityIds.Count - 1; i++)
            {
                ret += "'" + newActivityIds[i] + "', ";
            }
            ret += "'" + newActivityIds[newActivityIds.Count - 1] + "'";
        }
        ret += "]";
        return ret;   
    }
}