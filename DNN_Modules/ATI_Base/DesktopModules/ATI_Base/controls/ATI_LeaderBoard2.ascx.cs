using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

public partial class DesktopModules_ATI_Base_controls_ATI_LeaderBoard2 : DotNetNuke.Framework.UserControlBase
{

    public bool EditMode { get; set; }
    public bool NoPics { get; set; }

    private int _Cols = 2;
    public int Cols
    {
        get { return _Cols; }
        set { _Cols = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {

        }
    }    
}
