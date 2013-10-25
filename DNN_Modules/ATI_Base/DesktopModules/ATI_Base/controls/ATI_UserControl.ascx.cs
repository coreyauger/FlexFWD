using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke;

public partial class DesktopModules_ATI_Base_controls_ATI_UserControl : DotNetNuke.Framework.UserControlBase
{
    public string Email
    {
        get { return txtEmail.Text; }
    }

    public string FirstName
    {
        get { return txtFirstName.Text; }
    }

    public string LastName
    {
        get { return txtLastName.Text; }
    }

    public string Password
    {
        get { return txtPassword.Text; }
    }

    public string Confirm
    {
        get { return txtConfirm.Text; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ToString()
    {
        string ret = string.Empty;
        ret += "Email: " + this.Email + "<br />";
        ret += "FirstName: " + this.FirstName + "<br />";
        ret += "LastName: " + this.LastName + "<br />";
        ret += "Password: " + this.Password + "<br />";
        ret += "Confirm: " + this.Confirm + "<br />";
        return ret;
    }
}
