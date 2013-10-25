using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

public partial class DesktopModules_ATI_Base_controls_ATI_TimeSpan : DotNetNuke.Framework.UserControlBase
{   
    public bool ShowPace
    {
        get
        {
            if (ViewState["ShowPace"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState["ShowPace"]);
        }
        set
        {
            ViewState["ShowPace"] = value;
            plTime.Visible = lPace.Visible = value;
        }
    }

    public long Time
    {
        get {
            // TODO: need validation
            long ticks = 0;
            double hour = 0.0;
            try
            {
                hour = Convert.ToDouble(atiTxtTimeHour.Text);
            }
            catch (FormatException) { }
            double min = 0.0;
            try
            {
                min = Convert.ToDouble(atiTxtTimeMin.Text);
            }
            catch (FormatException) { }
            double sec = 0.0;
            try
            {
                sec = Convert.ToDouble(atiTxtTimeSec.Text);
            }
            catch (FormatException) { }
            ticks += (long)Affine.Utils.UnitsUtil.unitsToSystemDefualt(hour, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_HOUR);
            ticks += (long)Affine.Utils.UnitsUtil.unitsToSystemDefualt(min, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_MIN);
            ticks += (long)Affine.Utils.UnitsUtil.unitsToSystemDefualt(sec, Affine.Utils.UnitsUtil.MeasureUnit.UNIT_SEC);
            return ticks;
        }

        set
        {
            long sec = value / 1000;
            int h = (int)Math.Floor( ((double)sec / 60.0) / 60 );
            atiTxtTimeHour.Text = "" + h;
            sec = sec - (h * 60 * 60);
            int m = (int)Math.Floor(((double)sec / 60.0));
            atiTxtTimeMin.Text = "" + m;
            sec = sec - (m * 60);
            atiTxtTimeSec.Text = (sec < 10  ? "0" + sec : "" + sec);
        }
    }

    private bool _ShowHour = true;
    public bool ShowHour
    {
        get { return _ShowHour; }
        set { _ShowHour = value; }
    }

    public string CssClass
    {
        get { return atiTxtTimeSec.CssClass; }
        set { atiTxtTimeSec.CssClass = atiTxtTimeMin.CssClass = atiTxtTimeHour.CssClass = value; }
    }
   
    public string ValidationGroupName
    {
        get;
        set;
    }

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
            if (!this.ShowHour)
            {
                atiTxtTimeHour.Visible = false;
                litHourSep.Visible = false;
            }
            if (!string.IsNullOrEmpty(this.ValidationGroupName))
            {
                AddignValidationGroup(this);
            }            
        }
    }    
}
