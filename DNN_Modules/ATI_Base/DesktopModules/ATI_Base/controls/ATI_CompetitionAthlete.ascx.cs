using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;

using Telerik.Web.UI;

public partial class DesktopModules_ATI_Base_controls_ATI_CompetitionAthlete : DotNetNuke.Framework.UserControlBase
{

    public long CompetitionAthleteId
    {
        get;
        set;
    }

   
    protected void Page_Load(object sender, EventArgs e)
    {              
        aqufitEntities entities = new aqufitEntities();
        if (this.CompetitionAthleteId > 0)
        {
            CompetitionAthlete athlete = entities.CompetitionAthletes.FirstOrDefault(a => a.Id == CompetitionAthleteId);
            if (athlete != null)
            {
                litName.Text = athlete.AthleteName + " " + athlete.OverallRank + "(" + athlete.OverallScore + ")";
                imgAthlete.ImageUrl = athlete.ImgUrl;
                string html = "<ul>";
                if( athlete.Height.HasValue ){
                    double inch = Affine.Utils.UnitsUtil.systemDefaultToUnits(athlete.Height.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES );
                    html += "<li>Height: <em>" + Math.Floor(inch/12.0) + "' " + Math.Ceiling(inch%12.0) + "\"</em></li>";
                }
                if( athlete.Weight.HasValue ){
                    double lbs = Affine.Utils.UnitsUtil.systemDefaultToUnits(athlete.Weight.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
                    html += "<li>Weight: <em>" + Math.Round(lbs, 2) + "</em></li>";
                }
                html += "<li>&nbsp;</li>";
                html += "<li>Affiliate: <em>" + athlete.AffiliateName + "</em></li>";
                html += "<li>Region: <em>" + athlete.RegionName + "</em></li>";
                html += "<li>Hometown: <em>" + athlete.Hometown + "</em></li>";
                html += "<li>Country: <em>" + athlete.Country + "</em></li>";                
                html += "</ul>";
                litDetails.Text = html;
            }
        }
    }   

    /*
    protected void bAjaxPostback_Click(object sender, EventArgs e)
    {      
        OnWorkoutTypeChanged(e);
    }

    public delegate void WorkoutTypeCommandEventHandler(object sender,EventArgs e);
    public event WorkoutTypeCommandEventHandler WorkoutTypeChanged;
    protected virtual void OnWorkoutTypeChanged(EventArgs e)
    {
        if (WorkoutTypeChanged != null) WorkoutTypeChanged(ddlWorkoutTypeList, e);
    }   
    */
   
}
