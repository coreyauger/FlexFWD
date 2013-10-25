using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Telerik.Web.UI;

using Affine.Data;
using Affine.Data.EventArgs;

using Affine.Dnn.Modules.ATI_Base;

public partial class DesktopModules_ATI_Base_controls_ATI_ContactInviteScript : DotNetNuke.Framework.UserControlBase
{    
    public UserSettings UserSettings { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {
            if (UserSettings != null)
            {
                if (UserSettings is Group)
                {
                    litFromEmail.Text = UserSettings.UserEmail;
                    litProfileUrl.Text = "http://" + Request.Url.Host + ResolveUrl("~") + UserSettings.UserName;
                    litRegisterUrl.Text = "http://" + Request.Url.Host + ResolveUrl("~/Home/Register") + "?g=" + UserSettings.Id;
                    litSubject.Text = UserSettings.UserFirstName + " has invited you to join their group on FlexFWD";
                }
                else
                {
                    litFromEmail.Text = UserSettings.UserEmail;
                    litProfileUrl.Text = "http://" + Request.Url.Host + ResolveUrl("~") + UserSettings.UserName;
                    litRegisterUrl.Text = "http://" + Request.Url.Host + ResolveUrl("~/Home/Register");
                    litSubject.Text = UserSettings.UserFirstName + " " + UserSettings.UserLastName + " (" + UserSettings.UserName + ") has invited you to join FlexFWD";
                }
            }
        }
    }      
}
