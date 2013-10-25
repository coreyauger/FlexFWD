using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_BodyComposition : DotNetNuke.Framework.UserControlBase
{
    public bool IsEditMode
    {
        get
        {
            if (ViewState["SlimIsEditMode"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["SlimIsEditMode"]);
        }
        set
        {
            ViewState["SlimIsEditMode"] = value;
        }
    }

    private DateTime? setBirthday = null;
    public DateTime? BirthDate
    {
        get { DateTime ret = new DateTime(); if (DateTime.TryParse(ddlMonth.SelectedValue + "/" + ddlDay.SelectedValue + "/" + ddlYear.SelectedValue, out ret)) { return ret; } return null; }
        set
        {
            if (value != null)
            {
                setBirthday = value.Value;
            }
        }
    }
    public double? UserWeightInSystemDefault
    {
        
        get {
            try
            {
                return Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiTxtWeight.Text), this.WeightUnits);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
    public Affine.Utils.UnitsUtil.MeasureUnit WeightUnits
    {
        get { 
            return atiWeightUnits.Value; 
        }
        set { atiWeightUnits.Selected = value; }
    }
    public Affine.Utils.UnitsUtil.MeasureUnit HeightUnits
    {
        get { return atiHeightUnits.Value; }
        set { atiHeightUnits.Selected = value; }
    }

    public double? UserHeightInSystemDefault
    {
        get
        {
            try
            {
                if (this.HeightUnits == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN)
                {
                    double inches = (Convert.ToDouble(atiTxtHeightFeet.Text) * 12) + Convert.ToDouble(atiTxtHeightInch.Text);
                    return Affine.Utils.UnitsUtil.unitsToSystemDefualt(inches, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES);
                }
                else if (this.HeightUnits == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM)
                {
                    return Affine.Utils.UnitsUtil.unitsToSystemDefualt(Convert.ToDouble(atiTxtHeightInch.Text), Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM);
                }
                return 0.0;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
   
    public string Gender
    {
        get { return ddlGender.SelectedValue; }
        set { ddlGender.SelectedValue = value; }
    }
    public short FitnessLevel
    {
        get
        {
            return Convert.ToInt16(ddlFitnessLevel.SelectedValue);
        }
    }

    #region Show / Hide registration options
    public bool HeightVisible
    {
        get { return dtHeight.Visible; }
        set { dtHeight.Visible = ddHeight.Visible = value; }
    }
    public bool WeightVisible
    {
        get { return dtWeight.Visible; }
        set { dtWeight.Visible = ddWeight.Visible = value; }
    }
    public bool FitnessLevelVisible
    {
        get { return dtFitnessLevel.Visible; }
        set { dtFitnessLevel.Visible = ddFitnessLevel.Visible =value; }
    }
    public bool BirthDateVisible
    {
        get { return dtBirthDate.Visible; }
        set { dtBirthDate.Visible = ddBirthDate.Visible = value; }
    }
    public bool GenderVisible
    {
        get { return dtGender.Visible; }
        set { dtGender.Visible = ddGender.Visible = value; }
    }
    public string ValidationGroupName
    {
        get;
        set;
    }
    #endregion

    private void AddignValidationGroup(Control con)
    {    
        foreach (Control c in con.Controls)
        {
            if (c is BaseValidator)
            {
                ((BaseValidator)c).ValidationGroup = this.ValidationGroupName;
            }
            else
            {
                AddignValidationGroup(c);
            }
        }                         
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(this.ValidationGroupName))
            {
                AddignValidationGroup(this);
            }

            atiWeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS);
            atiWeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG);
            atiHeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN);
            atiHeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM);
            for (int m = 1; m <= 12; m++)
            {
                string month = m < 10 ? "0" + m : "" + m;
                ddlMonth.Items.Add(new ListItem(month, "" + m));
            }
            for (int d = 1; d <= 31; d++)
            {
                ddlDay.Items.Add(new ListItem("" + d, "" + d));
            }
            int thisYear = DateTime.Today.Year;
            for (int y = 0; y <= 100; y++)
            {
                ddlYear.Items.Add(new ListItem("" + (thisYear - y), "" + (thisYear - y)));
            }
            if (setBirthday != null)
            {
                ddlMonth.SelectedValue = "" + setBirthday.Value.Month;
                ddlDay.SelectedValue = "" + setBirthday.Value.Day;
                ddlYear.SelectedValue = "" + setBirthday.Value.Year;
            }

            if (this.PortalSettings.UserInfo.UserID != -1)
            {
                aqufitEntities aqufitEntities = new aqufitEntities();
                UserSettings aqufitSettings = aqufitEntities.UserSettings.FirstOrDefault<UserSettings>(us => us.UserKey == this.PortalSettings.UserInfo.UserID && us.PortalKey == this.PortalSettings.PortalId);
                if (aqufitSettings == null) // TODO: this really should never happen
                {
                    aqufitSettings = new User()
                    {
                        UserKey = this.PortalSettings.UserInfo.UserID,
                        PortalKey = this.PortalSettings.PortalId,
                        HeightUnits = (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN,
                        WeightUnits = (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS
                    };
                    aqufitEntities.AddToUserSettings(aqufitSettings);
                    aqufitEntities.SaveChanges();
                }

                if (aqufitSettings.WeightUnits != null)
                {
                    atiWeightUnits.Selected = (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), aqufitSettings.WeightUnits);
                }

                if (aqufitSettings.HeightUnits != null)
                {
                    atiHeightUnits.Selected = (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), aqufitSettings.HeightUnits);
                }

                if (atiHeightUnits.Selected == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM)
                {
                    atiTxtHeightFeet.Style["display"] = "none";
                    rfvHeightFeet.Style["display"] = "none";
                    revHeightFeet.Style["display"] = "none";
                }
                BodyComposition comp = aqufitEntities.BodyComposition.FirstOrDefault(b => b.UserSetting.Id == aqufitSettings.Id);
                if (comp != null)
                {
                    if (comp.Weight.HasValue)
                    {
                        atiTxtWeight.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits(comp.Weight.Value, atiWeightUnits.Selected);
                    }
                    if (comp.Height.HasValue)
                    {
                        if (atiHeightUnits.Selected == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN)
                        {
                            double inches = Affine.Utils.UnitsUtil.systemDefaultToUnits(comp.Height.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES);
                            atiTxtHeightFeet.Text = "" + inches / 12;
                            atiTxtHeightInch.Text = "" + (inches % 12);
                        }
                        else
                        {
                            atiTxtHeightInch.Text = "" + Affine.Utils.UnitsUtil.systemDefaultToUnits(comp.Height.Value, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM);
                        }
                    }                    
                }
            }                                    
        }
    }

    public string ToString()
    {
        string ret = string.Empty;
        ret += "Birth Date: " + (this.BirthDate != null ?  this.BirthDate.ToString() : "" ) + "<br />";
        //ret += "Weight: " + this.UserWeightInSystemDefault + "<br />";
        //ret += "Height: " + this.UserHeightFeet + "<br />";       
        ret += "Gender: " + this.ddlGender.SelectedItem.Text + "<br />";
        return ret;
    }
}
