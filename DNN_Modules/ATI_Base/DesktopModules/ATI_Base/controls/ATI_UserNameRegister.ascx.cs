using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

namespace Affine.Web.Controls
{
    public enum NameRegistrationMode { USER_NAME = 0, GROUP_NAME };
}

public partial class DesktopModules_ATI_Base_controls_ATI_UserNameRegister : DotNetNuke.Framework.UserControlBase
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

    public Affine.Web.Controls.NameRegistrationMode Mode
    {
        get
        {
            if (ViewState["Mode"] == null)
            {
                return Affine.Web.Controls.NameRegistrationMode.USER_NAME;
            }
            return (Affine.Web.Controls.NameRegistrationMode)(ViewState["Mode"]);
        }
        set
        {
            ViewState["Mode"] = value;
        }
    }

    public string CssClass
    {
        get { return atiTxtUsername.CssClass; }
        set { atiTxtUsername.CssClass = value; }
    }

    public int MaxLength
    {
        get { return atiTxtUsername.MaxLength; }
        set { atiTxtUsername.MaxLength = value; }
    }    

    public string Text
    {
        get { return atiTxtUsername.Text; }
        set { atiTxtUsername.Text = value; }
    }    
 
    public string ValidationGroupName
    {
        get;
        set;
    }

    public Unit Width { get; set; }

    private bool _Enabled = true;
    public bool Enabled { get { return _Enabled; } set { _Enabled = value; } }

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
            if (this.Width != null)
            {
                atiTxtUsername.Width = this.Width;
            }
            if (this.Mode == Affine.Web.Controls.NameRegistrationMode.USER_NAME)
            {
                plUserName.Text = "User Name:";
                cvUserName.ClientValidationFunction = "Aqufit.Page."+this.ID+".atiUserNameValidate";
                cvUserName.ServerValidate += new ServerValidateEventHandler(cvUserName_ServerValidate);
            }
            else if (this.Mode == Affine.Web.Controls.NameRegistrationMode.GROUP_NAME)
            {
                plUserName.Text = "Group Url:";
                litHelp.Visible = true;
                litHelp.Text = "<span style=\"display: block;\">http://flexfwd.com/group/<strong>[Group Url]</strong></span>";
                cvUserName.ClientValidationFunction = "Aqufit.Page." + this.ID + ".atiGroupNameValidate";
                cvUserName.ServerValidate += new ServerValidateEventHandler(cvGroupName_ServerValidate);
            }

            if (!string.IsNullOrEmpty(this.ValidationGroupName))
            {
                AddignValidationGroup(this);
            }
            if (!this.Enabled)
            {
                aitLabelUsername.Text = atiTxtUsername.Text;
                atiTxtUsername.Visible = false;
                aitLabelUsername.Visible = true;
                litHelp.Text = "<span style=\"display: block;\">http://flexfwd.com/group/<strong>" + aitLabelUsername.Text + "</strong></span>";
            }
            //imgError.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png");
            //atiTextBox_checkImg.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
        }
    }

    protected void cvUserName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        UserInfo objUserInfo = UserController.GetUserByName(this.PortalSettings.PortalId, args.Value.ToLower());
        if (this.IsEditMode)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = objUserInfo == null;
        }
    }

    protected void cvGroupName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        //Affine.Data.aqufitEntities entities = new Affine.Data.aqufitEntities();
        //if (this.IsEditMode)
        //{
            args.IsValid = true;
        //}
        //else
        //{
        //    args.IsValid = objUserInfo == null;
        //}
    } 
}
