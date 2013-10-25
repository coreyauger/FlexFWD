using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DesktopModules_ATI_Base_controls_ATI_StreamComment : System.Web.UI.UserControl
{
    public string CssClass { get; set; }

    public string Comment
    {
        get
        {
            this.EnsureChildControls();
            return atiTxtComment.Text;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
         
    }
}
