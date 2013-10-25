using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_ThemeEditor : DotNetNuke.Framework.UserControlBase
{


    public FileUpload FileUpload { get { return fileUpload; } }

    public System.Drawing.Color BackgroundColor { get { return RadColorPicker1.SelectedColor; } set { RadColorPicker1.SelectedColor = value; } }

    public bool IsTiled { get { return cbTileBackground.Checked; } set { cbTileBackground.Checked = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!Page.IsPostBack )
        {
            
        }
    }



    
}
