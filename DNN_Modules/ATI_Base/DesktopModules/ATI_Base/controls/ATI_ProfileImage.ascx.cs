using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_ProfileImage : DotNetNuke.Framework.UserControlBase
{
    public Unit Width
    {
        get
        {
            if (ViewState["Width"] != null)
            {
                return (Unit)ViewState["Width"];
            }
            return Unit.Pixel(0);
        }
        set
        {
            ViewState["Width"] = value;
        }
    }
    public Unit Height
    {
        get
        {
            if (ViewState["Height"] != null)
            {
                return (Unit)ViewState["Height"];
            }
            return Unit.Pixel(0);
        }
        set
        {
            ViewState["Height"] = value;
        }
    }
    public bool Small
    {
        get
        {
            if (ViewState["Small"] != null)
            {
                return (bool)ViewState["Small"];
            }
            return false;
        }
        set
        {
            ViewState["Small"] = value;
        }
    }

    public UserSettings Settings
    {
        get;
        set;
    }

    public string GroupUserName
    {
        get;
        set;
    }

    public bool IsOwner
    {
        set { atiProfileImgEdit.Visible = value; }

    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.GroupUserName = null;
            panelProfileImageSmall.Visible = this.Small;
            panelProfileImageLarge.Visible = !this.Small;
            imgProfileLarge.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/noProfilePic.png");
        }
        if (this.Settings != null)
        {
            if (this.Settings is Group)
            {
                this.GroupUserName = this.Settings.UserName;
            }
            else if (!atiProfileImgEdit.Visible)
            {
                imgProfileLarge.Attributes["onclick"] = "top.location.href = '" + ResolveUrl("~/") + "compare/" + this.Settings.UserName + "';";
                imgProfileLarge.Style["cursor"] = "pointer";
            }
            imgProfileSmall.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?us=" + Settings.Id + (this.Small ? "" : "&f=1");
            hrefImgSmall.HRef = ResolveUrl("~/") + this.Settings.UserName;
            hrefNameSmall.HRef = hrefImgSmall.HRef;
            hrefNameSmall.InnerHtml = this.Settings.UserName;
            imgProfileLarge.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?us=" + Settings.Id + (this.Small ? "" : "&f=1");            
        }
        
        
        
        
    }
}
