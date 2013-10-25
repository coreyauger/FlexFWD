using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class services_json_history : System.Web.UI.Page
{
    private JavaScriptSerializer serializer = new JavaScriptSerializer();
    private Affine.WebService.StreamService ss = new Affine.WebService.StreamService();

    protected class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Date { get; set; }
        public string Notes { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "application/json";
        if (!string.IsNullOrWhiteSpace(Request.Form["u"]))
        {
            aqufitEntities entities = new aqufitEntities();
            long uid = Convert.ToInt64( Request.Form["u"] );
            Guid token = Guid.Parse( Request.Form["t"] );
            User user = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.Id == uid && u.Guid == token);
            string search = Request["s"] != null ? Convert.ToString(Request["s"]) : "";

            if (!string.IsNullOrWhiteSpace(search))
            {
                IQueryable<Workout> wodsToDisplay = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == user.Id && w.Title.StartsWith(search));
                wodsToDisplay.Take(25);
                Workout[] timedWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED).OrderBy(w => w.Duration).ToArray();
                IList<NameValue> cfTotals = timedWods.Select(w => new NameValue() { Name = w.Title, Value = Affine.Utils.UnitsUtil.durationToTimeString(Convert.ToInt64(w.Duration)), Date = w.Date.ToShortDateString(), Notes = "" + w.Description }).ToList();
                // Now all the scored ones...
                Workout[] scoredWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.SCORE || w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.AMRAP).ToArray();
                cfTotals = cfTotals.Concat(scoredWods.Select(w => new NameValue() { Name = w.Title, Value = Convert.ToString(w.Score), Date = w.Date.ToShortDateString(), Notes = "" + w.Description }).ToList()).ToList();
                Workout[] maxWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT).ToArray();
                Affine.Utils.UnitsUtil.MeasureUnit WeightUnits = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                cfTotals = cfTotals.Concat(maxWods.Select(w => new NameValue() { Name = w.Title, Value = Affine.Utils.UnitsUtil.systemDefaultToUnits(w.Max.Value, WeightUnits) + " " + Affine.Utils.UnitsUtil.unitToStringName(WeightUnits), Date = w.Date.ToShortDateString(), Notes = "" + w.Description }).ToList()).ToList();
                Response.Write(serializer.Serialize(cfTotals.OrderBy(t => t.Name).ToArray()));
            }
            else
            {
                IQueryable<Workout> crossfitWorkouts = entities.UserStreamSet.OfType<Workout>().Include("WOD").Where(w => w.UserSetting.Id == user.Id && w.IsBest == true);
                int numDistinct = crossfitWorkouts.Select(w => w.WOD).Distinct().Count();
                IQueryable<Workout> wodsToDisplay = null;
                wodsToDisplay = crossfitWorkouts.OrderByDescending(w => w.Id);
                // We need to split up into WOD types now...
                Workout[] timedWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.TIMED).OrderBy(w => w.Duration).ToArray();
                IList<NameValue> cfTotals = timedWods.Select(w => new NameValue() { Name = w.Title, Value = Affine.Utils.UnitsUtil.durationToTimeString(Convert.ToInt64(w.Duration)), Date = w.Date.ToShortDateString(), Notes = "" + w.Description }).ToList();
                // Now all the scored ones...
                Workout[] scoredWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.SCORE || w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.AMRAP).ToArray();
                cfTotals = cfTotals.Concat(scoredWods.Select(w => new NameValue() { Name = w.Title, Value = Convert.ToString(w.Score), Date = w.Date.ToShortDateString(), Notes = "" + w.Description }).ToList()).ToList();
                Workout[] maxWods = wodsToDisplay.Where(w => w.WOD.WODType.Id == (int)Affine.Utils.WorkoutUtil.WodType.MAX_WEIGHT).ToArray();
                Affine.Utils.UnitsUtil.MeasureUnit WeightUnits = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;
                cfTotals = cfTotals.Concat(maxWods.Select(w => new NameValue() { Name = w.Title, Value = Affine.Utils.UnitsUtil.systemDefaultToUnits(w.Max.Value, WeightUnits) + " " + Affine.Utils.UnitsUtil.unitToStringName(WeightUnits), Date = w.Date.ToShortDateString(), Notes = "" + w.Description }).ToList()).ToList();
                Response.Write(serializer.Serialize(cfTotals.OrderBy(t => t.Name).ToArray()));
            }
            Response.Flush();
            Response.End();
        }
    }
}