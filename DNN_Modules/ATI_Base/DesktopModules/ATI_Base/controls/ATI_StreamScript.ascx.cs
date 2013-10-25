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

public partial class DesktopModules_ATI_Base_controls_ATI_StreamScript : DotNetNuke.Framework.UserControlBase
{
    public string EditUrl { get; set; }

    public bool ShowTopPager
    {
        get
        {
            return ViewState["ShowTopPager"] != null ? Convert.ToBoolean(ViewState["ShowTopPager"]) : true;
        }
        set { ViewState["ShowTopPager"] = value; }
    }
    public bool ShowBottomPager
    {
        get
        {
            return ViewState["ShowBottomPager"] != null ? Convert.ToBoolean(ViewState["ShowBottomPager"]) : true;
        }
        set { ViewState["ShowBottomPager"] = value; }
    }
    public bool ShowStreamSelect
    {
        get
        {
            return ViewState["ShowStreamSelect"] != null ? Convert.ToBoolean(ViewState["ShowStreamSelect"]) : false;
        }
        set { ViewState["ShowStreamSelect"] = value; }
    }
    

    public int DefaultTake
    {
        get
        {
            if (this.ViewState["DefaultTake"] == null)
            {
                return 15;
            }
            return Convert.ToInt32(this.ViewState["DefaultTake"]);
        }
        set
        {
            this.ViewState["DefaultTake"] = value;
        }
    }

    public bool IsSearchMode
    {
        get
        {
            if (this.ViewState["IsSearchMode"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(this.ViewState["IsSearchMode"]);
        }
        set
        {
            this.ViewState["IsSearchMode"] = value;
        }
    }

    public bool IsFollowMode
    {
        get
        {
            if (this.ViewState["IsFollowMode"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(this.ViewState["IsFollowMode"]);
        }
        set
        {
            this.ViewState["IsFollowMode"] = value;
        }
    }
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {
            
        }        
    }      
}
