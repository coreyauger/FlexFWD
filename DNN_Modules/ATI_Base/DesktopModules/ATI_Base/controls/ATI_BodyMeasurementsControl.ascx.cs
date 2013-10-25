using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_BodyMeasurements : DotNetNuke.Framework.UserControlBase
{
    public string Neck
    {
        get { return atiTxtNeck.Text; }
        set { atiTxtNeck.Text = value; }
    }

    public string Shoulders
    {
        get { return atiTxtShoulders.Text; }
        set { atiTxtShoulders.Text = value; }
    }

    public string Chest
    {
        get { return atiTxtChest.Text; }
        set { atiTxtChest.Text = value; }
    }

    public string Stomach
    {
        get { return atiTxtStomach.Text; }
        set { atiTxtStomach.Text = value; }
    }

    public string Waist
    {
        get { return atiTxtWaist.Text; }
        set { atiTxtWaist.Text = value; }
    }

    public string Hips
    {
        get { return atiTxtHips.Text; }
        set { atiTxtHips.Text = value; }
    }

    public string BicepLeft
    {
        get { return atiTxtBicepLeft.Text; }
        set { atiTxtBicepLeft.Text = value; }
    }

    public string BicepRight
    {
        get { return atiTxtBicepRight.Text; }
        set { atiTxtBicepRight.Text = value; }
    }

    public string ForearmLeft
    {
        get { return atiTxtForearmLeft.Text; }
        set { atiTxtForearmLeft.Text = value; }
    }

    public string ForearmRight
    {
        get { return atiTxtForearmRight.Text; }
        set { atiTxtForearmRight.Text = value; }
    }

    public string ThighLeft
    {
        get { return atiTxtThighLeft.Text; }
        set { atiTxtThighLeft.Text = value; }
    }

    public string ThighRight
    {
        get { return atiTxtThighRight.Text; }
        set { atiTxtThighRight.Text = value; }
    }

    public string CalfLeft
    {
        get { return atiTxtCalfLeft.Text; }
        set { atiTxtCalfLeft.Text = value; }
    }

    public string CalfRight
    {
        get { return atiTxtCalfRight.Text; }
        set { atiTxtCalfRight.Text = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack )
        {
            atiBmUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES);
            atiBmUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM);
            if (this.PortalSettings.UserInfo.UserID != -1)
            {
                aqufitEntities aqufitEntities = new aqufitEntities();
                UserSettings aqufitSettings = aqufitEntities.UserSettings.FirstOrDefault<UserSettings>(us => us.UserKey == this.PortalSettings.UserInfo.UserID && us.PortalKey == this.PortalSettings.PortalId);
                if (aqufitSettings != null)
                {
                    if (aqufitSettings.BodyMeasureUnits != null)
                    {
                        aqufitSettings.BodyMeasureUnits = (short)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES;
                        aqufitEntities.SaveChanges();
                        atiBmUnits.Selected = (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), aqufitSettings.BodyMeasureUnits);
                    }
                }
            }
        }
    }

    
}
