using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DesktopModules_ATI_Base_controls_ATI_UnitControl : DotNetNuke.Entities.Modules.PortalModuleBase
{
    private IList<Affine.Utils.UnitsUtil.MeasureUnit> mUnitList = new List<Affine.Utils.UnitsUtil.MeasureUnit>();

    public Unit Width
    {
        set { ddlUnitsType.Width = value; }
        get { return ddlUnitsType.Width; }
    }

    public override string ClientID
    {
        get
        {
            return ddlUnitsType.ClientID;
        }
    }

    public string UnitType
    {
        get;
        set;
    }

    public string ClientOnChange
    {
        get;
        set;
    }

    public string CssClass
    {
        get { return ddlUnitsType.CssClass; }
        set { ddlUnitsType.CssClass = value; }
    }

    public IList<Affine.Utils.UnitsUtil.MeasureUnit> UnitList
    {
        get { return mUnitList; }
        set { mUnitList = value; }
    }

    public Affine.Utils.UnitsUtil.MeasureUnit Selected
    {
        get;
        set;
    }

    public Affine.Utils.UnitsUtil.MeasureUnit Value
    {
        get { return (Affine.Utils.UnitsUtil.MeasureUnit)Enum.ToObject(typeof(Affine.Utils.UnitsUtil.MeasureUnit), Convert.ToInt32( ddlUnitsType.SelectedValue )); }
    }

    private void SetupUnit(Affine.Utils.UnitsUtil.MeasureUnit unit)
    {
        switch (unit)
        {
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM:
                ddlUnitsType.Items.Add(new ListItem("cm", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES:
                ddlUnitsType.Items.Add(new ListItem("in", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_INCHES)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG:
                ddlUnitsType.Items.Add(new ListItem("Kg", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KG)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KM:
                ddlUnitsType.Items.Add(new ListItem("KM", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_KM)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS:
                ddlUnitsType.Items.Add(new ListItem("lbs", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_LBS)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M:
                ddlUnitsType.Items.Add(new ListItem("M", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_M)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES:
                ddlUnitsType.Items.Add(new ListItem("Mi", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MILES)); 
                break;
            case Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN:
                ddlUnitsType.Items.Add(new ListItem("ft/in", "" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN));
                break;
            default:
                throw new Affine.Utils.InvalidUnitsException("Unknown Units: ATI_UnitControl.SetupUnit");
        }
    }

    private void SetupSelectedUnit()
    {
        ListItem selected = ddlUnitsType.Items.FindByValue("" + (int)this.Selected);
        if (selected != null)
        {
            selected.Selected = true;
        }        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/UtilService.asmx");
        service.InlineScript = true;
        ScriptManager.GetCurrent(Page).Services.Add(service);
        // script service requires the user id and portal id            
        ddlUnitsType.Attributes["onchange"] = "Aqufit.Units.SaveUnitChangeSetting('" + ddlUnitsType.ClientID + "','" + this.UnitType + "'); " + this.ClientOnChange + "('" + ddlUnitsType.ClientID + "','" + this.UnitType + "');";
        if (!Page.IsPostBack)
        {            
            foreach (Affine.Utils.UnitsUtil.MeasureUnit u in this.UnitList)
            {
                SetupUnit(u);
            }
            SetupSelectedUnit();
        }
    }
}
