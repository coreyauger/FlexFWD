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

public partial class DesktopModules_ATI_Base_controls_ATI_PeopleListScript : DotNetNuke.Framework.UserControlBase
{
    public string Client_OnItemsLoad { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!Page.IsPostBack || !Page.IsCallback)
        {

        }
    }      
}
