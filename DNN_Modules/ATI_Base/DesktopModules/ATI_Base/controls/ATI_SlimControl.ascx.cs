using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

using DotNetNuke;

public partial class DesktopModules_ATI_Base_controls_ATI_SlimControl : DotNetNuke.Framework.UserControlBase
{
    public bool IsEditMode
    {
        get
        {
            if (ViewState["SlimIsEditMode"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["SlimIsEditMode"]);
        }
        set
        {
            ViewState["SlimIsEditMode"] = value;
        }
    }

    public string FullName
    {
        get { return atiTxtFullName.Text; }
        set { atiTxtFullName.Text = value; }
    }

    public string Address
    {
        get { return atiAddress.Value; }
        set { atiAddress.Value = value; }
    }

    public string LocLat
    {
        get { return atiLocLat.Value; }
        set { atiLocLat.Value = value; }
    }

    public string LocLng
    {
        get { return atiLocLng.Value; }
        set { atiLocLng.Value = value; }
    }

    public string Postal
    {
        get { return atiTxtPostal.Text; }
        set { atiTxtPostal.Text = value; }
    }

    public string Email
    {
        get { return atiTxtEmail.Text; }
        set { atiTxtEmail.Text = value; }
    }   
    
    public string FirstName
    {
        get { return atiTxtFirstName.Text; }
        set { atiTxtFullName.Text = value; }
    }
    public string LastName
    {
        get { return atiTxtLastName.Text; }
        set { atiTxtLastName.Text = value; }
    }       

    public string ValidationGroupName
    {
        get;
        set;
    }

    private bool _ShowFullName = true;
    public bool ShowFullName
    {
        get { return _ShowFullName; }
        set { _ShowFullName = value; }
    }

    private bool _ShowEmail = true;
    public bool ShowEmail
    {
        get { return _ShowEmail; }
        set { _ShowEmail = value; }
    }
    private bool _ShowPostal = true;
    public bool ShowPostal
    {
        get { return _ShowPostal; }
        set { _ShowPostal = value; }
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
            if (!this.ShowFullName)
            {
                dtFullName.Visible = false;
                ddFullName.Visible = false;
            }
            if (!this.ShowEmail)
            {
                dtEmail.Visible = false;
                ddEmail.Visible = false;
            }
            if (!this.ShowPostal)
            {
                dtPostal.Visible = false;
                ddPostal.Visible = false;
            }

            if ( !string.IsNullOrEmpty( this.ValidationGroupName))
            {
                AddignValidationGroup(this);           
            }            
        }
    }

    public string ToString()
    {
        string ret = string.Empty;
        ret += "First Name: " + this.FirstName + "<br />";
        ret += "Last Name: " + this.LastName + "<br />";
        ret += "Email: " + this.Email + "<br />";       
        return ret;
    }
}
