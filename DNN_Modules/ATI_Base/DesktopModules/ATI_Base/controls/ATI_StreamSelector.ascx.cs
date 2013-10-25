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

public partial class DesktopModules_ATI_Base_controls_ATI_StreamSelector : StreamControlBase
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        base.atiDataListStream = atiDataListStream;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {

            atiDataListStream.ShowFooter = this.AllowPaging;

            // TODO: make a view button (me & friends) (just me) that sets the stream view

            // TODO: make the "take" configurable

        }
    }      
}