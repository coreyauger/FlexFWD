using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

using Telerik.Web.UI;

public partial class DesktopModules_ATI_Base_controls_ATI_PhotoTagger : DotNetNuke.Framework.UserControlBase
{

    public object DataSource { get; set; } 
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (this.DataSource != null)
            {
                RadListBox1.DataSource = this.DataSource;
                RadListBox1.DataValueField = "Id";
                RadListBox1.DataTextField = "UserName";
                RadListBox1.DataBind();
            }
        }
    }    
}
