using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

using Affine.Data;
using Affine.Utils;

namespace Aqufit.Helpers
{
    public class RouteItem
    {
        public long Id { get; set; }
        public double Dist { get; set; }
    }

    public class WodItem
    {
        public long Id { get; set; }
        public short Type { get; set; }
    }
}

public partial class DesktopModules_ATI_Base_controls_ATI_Workout : DotNetNuke.Framework.UserControlBase
{
    private System.Web.Script.Serialization.JavaScriptSerializer _serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

    public WorkoutType WorkoutType
    {
        get { return atiWorkoutTypes.Selected; }
    }

    public WorkoutUtil.Feeling Feeling
    {
        get
        {
            int val = 0;
            try
            {
                val = Convert.ToInt32(atiFeltIconSelector.Value);
            }catch(Exception){}
            return WorkoutUtil.IntToFeeling(val);
        }
    }

    public WorkoutUtil.Weather Weather
    {
        get
        {
            int val = 0;
            try
            {
                val = Convert.ToInt32(atiWeatherIconSelector.Value);
            }
            catch (Exception) { }
            return WorkoutUtil.IntToWeather(val);
        }
    }

    public WorkoutUtil.Terrain Terrain
    {
        get
        {
            int val = 0;
            try
            {
                val = Convert.ToInt32(atiTerrainIconSelector.Value);
            }
            catch (Exception) { }
            return WorkoutUtil.IntToTerrain(val);
        }
    }

    public double Distance
    {
        get { 
            double distance = 0.0;
            try
            {
                distance = Convert.ToDouble(atiTxtDistance.Text);
            }
            catch (FormatException) { }
            return Affine.Utils.UnitsUtil.unitsToSystemDefualt(distance, atiDistanceUnits.Value); 
        }
        set
        {
            atiTxtDistance.Text = "" + value;
        }
    }

    public Affine.Utils.UnitsUtil.MeasureUnit MaxWeightUnit
    {
        get {
            return atiMaxWeightUnits.Value;
        }
    }

    public long WODId
    {
        get
        {
            Aqufit.Helpers.WodItem wi = _serializer.Deserialize<Aqufit.Helpers.WodItem>(atiRadComboBoxCrossfitWorkouts.SelectedValue);
            return wi.Id;           
        }
    }

    public Affine.Utils.WorkoutUtil.WodType WodType
    {
        get
        {
            Aqufit.Helpers.WodItem wi = _serializer.Deserialize<Aqufit.Helpers.WodItem>(atiRadComboBoxCrossfitWorkouts.SelectedValue);
            return Affine.Utils.WorkoutUtil.IntToWODType(wi.Type);
        }
    }

    public bool IsRxD
    {
        get { return cbWorkoutRx.Checked; }
    }

    public string Notes
    {
        get { return txtNote.Text; }
    }

    public WOD SetControlToWOD { set; get; }

    public MapRoute SetControlToMapRoute { get; set; }

    public double Score
    {
        get { return string.IsNullOrEmpty(atiTxtScore.Text) ? 0.0 : Convert.ToDouble( atiTxtScore.Text ); }
    }

    public long SelectedMapRouteId
    {
        get { try{
            Aqufit.Helpers.RouteItem ri = _serializer.Deserialize<Aqufit.Helpers.RouteItem>(atiRouteSelector.SelectedValue);
            return ri.Id;
        }
        catch (Exception) { return -1; }
        } 
    }      

    public long Time
    {
        get
        {
            return atiTimeSpan.Time;
        }
    }
    public DateTime Date
    {
        get { 
            DateTime date = DateTime.Now;
            if( atiRadDatePicker.SelectedDate != null ){
                DateTime days = Convert.ToDateTime(atiRadDatePicker.SelectedDate);
                date = new DateTime(days.Year, days.Month, days.Day, date.Hour, date.Minute, date.Second );
            }            
            return date;
        }
    }

    public Affine.Utils.WorkoutUtil.WorkoutType LastWorkoutType { get; set; }

    public string Title
    {
        //Add New Map
        get { 
            string ret = atiRouteMode.Value == "route" ? atiRouteSelector.Text :  atiTxtTitle.Text;
            if( ret.ToLower().StartsWith("add new map") )ret = "Untitled";
            return ret;
        }
    }    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
           // imgNikePlus.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Fitness/resources/images/iSyncNikePlus.png");
            // set date control to today
            atiRadDatePicker.SelectedDate = DateTime.Now;
            
            // setup the units control
            atiDistanceUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES);
            atiDistanceUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KM);
            atiDistanceUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M);
            atiDistanceUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;

            atiMaxWeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
            atiMaxWeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
            atiMaxWeightUnits.Selected = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS;

            // Setup the units to the users last used units
            if (this.PortalSettings.UserInfo.UserID != -1)
            {
                aqufitEntities aqufitEntities = new aqufitEntities();
                UserSettings aqufitSettings = aqufitEntities.UserSettings.FirstOrDefault<UserSettings>(us => us.UserKey == this.PortalSettings.UserInfo.UserID && us.PortalKey == this.PortalSettings.PortalId);
                if (aqufitSettings != null) // TODO: this really should never happen
                {
                    if (aqufitSettings.DistanceUnits != null)
                    {
                        atiDistanceUnits.Selected = (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), aqufitSettings.DistanceUnits);
                    }                    
                    if (aqufitSettings.WeightUnits != null)
                    {
                        atiMaxWeightUnits.Selected = (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), aqufitSettings.WeightUnits);
                    }
                }                
            }

            if (this.SetControlToWOD != null)
            {
                atiWorkoutTypes.SelectedType = Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT;
                Aqufit.Helpers.WodItem wi = new Aqufit.Helpers.WodItem() { Id = this.SetControlToWOD.Id, Type = (short)this.SetControlToWOD.WODType.Id };
                string json = _serializer.Serialize( wi );
                RadComboBoxItem item = new RadComboBoxItem(this.SetControlToWOD.Name, json);
                item.Selected = true;
                atiRadComboBoxCrossfitWorkouts.Items.Add(item);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "InitialWOD", "Aqufit.addLoadEvent(function(){ Aqufit.Page."+this.ID+".configureFormView("+(int)Affine.Utils.WorkoutUtil.WorkoutType.CROSSFIT+"); Aqufit.Page.Controls.atiWodSelector.SetupWOD('" + json + "') });", true);
            }
            else if (this.SetControlToMapRoute != null)
            {
                Affine.Utils.UnitsUtil.MeasureUnit units = Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES;
                double dist = Affine.Utils.UnitsUtil.systemDefaultToUnits(this.SetControlToMapRoute.RouteDistance, units);
                dist = Math.Round(dist, 2);
                Aqufit.Helpers.RouteItem ri = new Aqufit.Helpers.RouteItem() { Dist = this.SetControlToMapRoute.RouteDistance, Id = this.SetControlToMapRoute.Id };
                string json = _serializer.Serialize(ri);
                RadComboBoxItem item = new RadComboBoxItem(Affine.Utils.Web.WebUtils.FromWebSafeString(this.SetControlToMapRoute.Name) + " (" + dist + " " + Affine.Utils.UnitsUtil.unitToStringName(units) + ")", json);
                item.Selected = true;                
                item.ImageUrl = Affine.Utils.ImageUtil.GetGoogleMapsStaticImage(this.SetControlToMapRoute, 200, 150);
                atiRouteSelector.Items.Add(item);
            }
        }
    }

    // delegate declaration 
    public delegate void WodItemsRequestedHandler(object sender, RadComboBoxItemsRequestedEventArgs e);

    // event declaration 
    public event WodItemsRequestedHandler WodItemsRequested;

    public virtual void OnWodItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (this.WodItemsRequested != null)
        {
            this.WodItemsRequested(sender, e);
        }
    }

    public void RadComboBox2_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        OnWodItemsRequested(sender, e);        
    }


    // delegate declaration 
    public delegate void RouteItemsRequestedHandler(object sender, RadComboBoxItemsRequestedEventArgs e);

    // event declaration 
    public event RouteItemsRequestedHandler RouteItemsRequested;

    public virtual void OnRouteItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (this.RouteItemsRequested != null)
        {
            this.RouteItemsRequested(sender, e);
        }
    }

    public void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        OnRouteItemsRequested(sender, e);
    }
}
