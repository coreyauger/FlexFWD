using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;
using Affine.Data.EventArgs;

public partial class DesktopModules_ATI_Base_controls_ATI_InviteList : DotNetNuke.Framework.UserControlBase
{   

   
    public IList<object> DataSource
    {
        get
        {
            if (ViewState["DataSource"] == null)
            {
                return null;
            }
            return (IList<object>)ViewState["DataSource"];
        }
        set
        {
            ViewState["DataSource"] = value;
        }
    }   

    public override void DataBind()
    {
        base.DataBind();
        this.EnsureChildControls();
        if (this.DataSource != null)
        {
            atiRadInviteGrid.DataSource = this.DataSource;
            atiRadInviteGrid.Rebind();
            
            
        }
    }

    protected void atiRadInviteGrid_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (this.DataSource != null)
        {
            atiRadInviteGrid.DataSource = this.DataSource;
            atiRadInviteGrid.Rebind();
        }  
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {

        }
    }

}
