using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_StrengthGraph : DotNetNuke.Framework.UserControlBase
{
    public string IconUrl { get; set; }

    public Affine.Data.json.WorkoutData[] WorkoutArray { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (this.WorkoutArray != null)
            {
                System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "StrengthGraph" + this.UniqueID, "Aqufit.addLoadEvent(function(){ Aqufit.Page." + this.ID + ".setup('" + jsSerializer.Serialize(this.WorkoutArray) + "'); });", true);
            }
        }
    }
}
