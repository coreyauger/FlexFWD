using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DesktopModules_ATI_Base_controls_ATI_HeightControl : System.Web.UI.UserControl
{
    public string CssClass
    {
        get;
        set;
    }

    public Affine.Utils.UnitsUtil.MeasureUnit HeightUnits
    {
        get { return atiHeightUnits.Value; }
        set { 
            atiHeightUnits.Selected = value;
            if (value == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN)
            {
            //    atiHeightFeet.Style["display"] = "block";
            //    atiHeightInches.Style["display"] = "block";
            }
            else if (value == Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN)
            {
            //    atiHeightFeet.Style["display"] = "none";
            //    atiHeightInches.Style["display"] = "none";
            }
        }
    }    

    private string GetAtiTextBoxJs()
    {
        string ret = string.Empty;
        //atiHeightControl(ftId, inId, cmId, modeId, checkId, toolId, errId)
      //  ret += "var " + this.ID + " = new atiHeightControl('" + atiHeightFeet.ClientID + "','" + atiHeightInches.ClientID + "','" + atiHeightCm.ClientID + "','" + atiHeightMode.ClientID + "', '" + atiHeight_checkImg.ClientID + "','" + RadToolTip1.ClientID + "','" + atiLabel.ClientID + "',true,true,-1);";       
        //ret += "_atiTextObjectCollection['" + atiHeightFeet.ClientID + "'] = " + this.ID + ";";
        return ret;
    }
   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            atiHeightFeet.CssClass = this.CssClass;
            atiHeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN);
            atiHeightUnits.UnitList.Add(Affine.Utils.UnitsUtil.MeasureUnit.UNIT_CM);
            //string js = "atiHightSetUnits("+(int)this.HeightUnits+");";
           // string js = " atiHeightSetUnits(" + (int)Affine.Utils.UnitsUtil.MeasureUnit.UNIT_FT_IN + "); ";
           // ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiHeightSetMode"+this.ID, js,true);
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiTxtValidation" + this.ID, GetAtiHeightJs(), true);
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiTxtValidation" + this.ID, GetAtiTextBoxJs(), true);
    }
}
