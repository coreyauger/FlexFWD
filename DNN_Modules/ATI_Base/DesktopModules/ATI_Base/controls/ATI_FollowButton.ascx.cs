using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_FollowButton : DotNetNuke.Framework.UserControlBase
{
    public string Text
    {
        get;
        set;
    }

    private bool _UnFollow = false;
    public bool UnFollow
    {
        get { return _UnFollow; }
        set { _UnFollow = value; }
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.UnFollow ? "UnFollow" : "Follow";
            }
        }
    }
}
