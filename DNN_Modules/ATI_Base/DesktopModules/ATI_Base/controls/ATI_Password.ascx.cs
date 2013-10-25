using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

using DotNetNuke;

public partial class DesktopModules_ATI_Base_controls_ATI_Password : DotNetNuke.Framework.UserControlBase
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
    public string Original
    {
        get { return atiTxtCurrentPassword.Text; }
    }
   
    public string Password
    {
        get { return atiTxtPassword.Text; }        
    }     
    public string Confirm
    {
        get { return atiTxtConfirm.Text; }
    }    

    public string ValidationGroupName
    {
        get;
        set;
    }
    /*
    public delegate void UpdatePasswordEventHandler(object sender, EventArgs e);
    public event UpdatePasswordEventHandler UpdatePassword;       
    */
    private void AssignValidationGroup(Control con)
    {
        foreach (Control c in con.Controls)
        {
            if (c is BaseValidator)
            {
                ((BaseValidator)c).ValidationGroup = this.ValidationGroupName;
            }
            else
            {
                AssignValidationGroup(c);
            }
        }
    }

    public void SetValidationGroup(string group)
    {
        this.ValidationGroupName = group;
        AssignValidationGroup(this);  
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if ( !string.IsNullOrEmpty( this.ValidationGroupName))
            {
                AssignValidationGroup(this);           
            }

            if (this.IsEditMode)
            {
                dtCurrentPassword.Visible = true;
                ddCurrentPassword.Visible = true;
                plPassword.Text = "New Password";
            }
        }
    }
  
   
}
