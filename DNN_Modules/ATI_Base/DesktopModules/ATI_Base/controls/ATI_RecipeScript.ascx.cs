using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using DotNetNuke.Entities.Users;
using DotNetNuke;
using DotNetNuke.Common.Lists;
using DotNetNuke.Services.Localization;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_RecipeScript : DotNetNuke.Framework.UserControlBase
{
    public string RecipeId { get; set; }
    public bool ShowLogin
    {
        get
        {
            if (ViewState["ShowLogin"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["ShowLogin"]);
        }
        set
        {
            ViewState["ShowLogin"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                linkLogin.Visible = this.ShowLogin;
            }
        }
        catch (DotNetNuke.Services.Exceptions.ModuleLoadException mlex)
        {

        }
    }

    protected void linkLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.LoginTabId, "Login", new string[] { "returnUrl=" + Request.Url.PathAndQuery }));
        //Response.Redirect(ResolveUrl("~/Login.aspx?ReturnUrl="+Request.Url.PathAndQuery), true);
    }
}
