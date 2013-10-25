using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_Login : DotNetNuke.Framework.UserControlBase
{
    public bool ShowForgotPassword { get; set; }

    public string UserNameField { get; set; }
    public string PasswordField { get; set; }

    public string UserName
    {
        get { return txtUserName.Text; }
    }

    public string Password
    {
        get { return txtPassword.Text; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (this.UserNameField != null)
            {
                plUserName.Text = this.UserNameField;
            }
            if (this.PasswordField != null)
            {
                plPassword.Text = this.PasswordField;
            }
        }
    }

}
