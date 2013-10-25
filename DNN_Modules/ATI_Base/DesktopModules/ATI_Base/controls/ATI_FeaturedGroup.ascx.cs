using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_FeaturedGroup : DotNetNuke.Framework.UserControlBase
{

    public Group Group { get; set; }
    public int NumMembers { get; set; }
    public string[] MemberNames { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            atiFeaturedProfile.Settings = this.Group;
            hrefAllMembers.HRef = "/" + this.Group.UserName+ "/Friends";
            if (this.MemberNames != null)
            {
                listMemberList.DataSource = this.MemberNames.Select(n => new { Username = n }).ToArray();
                listMemberList.DataBind();
            }
        }
    }
}
