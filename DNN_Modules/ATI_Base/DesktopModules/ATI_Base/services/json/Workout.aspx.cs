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
public partial class services_json_Workout : System.Web.UI.Page
{
    private const int TAKE = 20;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["w"]))
        {
            aqufitEntities entities = new aqufitEntities();
            long wid = Convert.ToInt64(Request["w"]);
            // TODO: need security
            Workout workout = entities.UserStreamSet.Include("WorkoutExtendeds").Include("UserSetting").OfType<Workout>().FirstOrDefault(w => w.Id == wid);
            if (workout != null)
            {
                Affine.Data.json.Workout json = new Affine.Data.json.Workout()
                {
                    Calories = workout.Calories,
                    Date = workout.Date.ToShortDateString(),
                    Description = workout.Description,
                    Distance = workout.Distance,
                    Duration = workout.Duration,
                    Emotion = workout.Emotion,
                    Id = workout.Id,
                    Title = workout.Title,                   
                    PortalKey = workout.PortalKey,
                    UserKey = workout.UserSetting.UserKey,
                    Weather = workout.Weather,                    
                };
                if (workout.WorkoutExtendeds.FirstOrDefault() != null)
                {
                    WorkoutExtended ext = workout.WorkoutExtendeds.First();
                    Affine.Data.json.WorkoutExtended jsonWorkoutExtended = new Affine.Data.json.WorkoutExtended()
                    {
                        Id = ext.Id,
                        LatMax = ext.LatMax,
                        LatMin = ext.LatMin,
                        LngMax = ext.LngMax,
                        LngMin = ext.LngMin
                    };
                    Affine.Data.json.WorkoutSample[] samples = entities.WorkoutSamples.Where(s => s.WorkoutExtended.Id == ext.Id).OrderBy(s => s.SampleNumber)
                                                                .Select(s => new Affine.Data.json.WorkoutSample()
                                                                {
                                                                    Date = s.Date,  
                                                                    Time = 0,
                                                                    Distance = s.Distance,
                                                                    Elevation = s.Elevation,
                                                                    HeartRate = s.HeartRate,
                                                                    Id = s.Id,
                                                                    Lat = s.Lat,
                                                                    Lng = s.Lng,
                                                                    SampleNumber = s.SampleNumber
                                                                }).ToArray();
                    foreach (Affine.Data.json.WorkoutSample s in samples)
                    {
                        s.Time = s.Date.Ticks;
                    }
                    jsonWorkoutExtended.WorkoutSamples = samples;
                    json.WorkoutExtended = jsonWorkoutExtended;
                }
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                Response.Write( jserializer.Serialize(json) );
            }
        }
        else if (Request["ed"] != null && Request["u"] != null && Request["sd"] != null)
        {
            long uid = Convert.ToInt64(Request["u"]);
            DateTime endDate = DateTime.Parse(Request["ed"]);
            DateTime startDate = DateTime.Parse(Request["sd"]).ToUniversalTime();
            endDate = endDate.AddDays(1).ToUniversalTime();
            aqufitEntities entities = new aqufitEntities();
            IList<Affine.Data.json.Workout> ret = new List<Affine.Data.json.Workout>();
            IList<Workout> workoutList = entities.UserStreamSet.Include("UserSetting").OfType<Workout>().
                                                Where(w => w.UserSetting.UserKey == uid && w.Date.CompareTo(endDate) <= 0 && w.Date.CompareTo(startDate) >= 0 ).OrderByDescending(w => w.Date).ToList();
            workoutList = workoutList.Reverse().ToList();
            foreach (Workout workout in workoutList)
            {
                Affine.Data.json.Workout json = new Affine.Data.json.Workout()
                {
                    Calories = workout.Calories,
                    TimeStamp = workout.TimeStamp,
                    Date = workout.Date.ToShortDateString(),
                    Description = workout.Description,
                    Distance = workout.Distance,
                    Duration = workout.Duration,
                    Emotion = workout.Emotion,
                    Id = workout.Id,
                    Title = workout.Title,
                    PortalKey = workout.PortalKey,
                    UserKey = workout.UserSetting.UserKey,
                    Weather = workout.Weather,
                    DataSrc = workout.DataSrc
                };
                ret.Add(json);
            }
            // TODO: need a case for empty data
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            Response.Write(jserializer.Serialize(ret.ToArray()));
        }
        else
        {
            Response.Write("{ERROR:'invalid request'}");
        }
    }
}