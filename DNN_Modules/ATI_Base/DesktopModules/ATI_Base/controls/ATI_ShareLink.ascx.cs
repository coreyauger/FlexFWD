using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_ShareLink : DotNetNuke.Framework.UserControlBase
{
    public Unit TextBoxWidth { get; set; }
    public string ShareLink { get; set; }

    public string ShareTitle { get; set; }

    public string TextBoxCssClass { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            imgTwitter.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iTwitter.png");
            aTweetShare.HRef = "http://twitter.com/share?url=" + this.ShareLink + "&related=flexfwd&text=" + this.ShareTitle;
            txtShareLink.CssClass = this.TextBoxCssClass;
            txtShareLink.Width = this.TextBoxWidth;
            txtShareLink.Text = this.ShareLink;
        }
    }
}
