using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DesktopModules_ATI_Base_controls_ATI_Preview : Affine.Web.Controls.ATI_ValidationBaseCompositeControl
{
    public string FirstName
    {
        get { return atiTxtFirstName.Text; }
    }
    public string LastName
    {
        get { return atiTxtLastName.Text; }
    }
    public string Email
    {
        get { return atiTxtEmail.Text; }
    }
    public string Comments
    {
        get { return txtComments.Text; }
    }

    private bool _ShowComments = true;
    public bool ShowComments
    {
        get { return _ShowComments; }
        set { _ShowComments = value; txtComments.Visible = _ShowComments; plComments.Visible = _ShowComments; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
