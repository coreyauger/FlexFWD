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

public partial class DesktopModules_ATI_Base_controls_ATI_FriendListScript : DotNetNuke.Framework.UserControlBase
{
    public enum Mode { FRIEND_LIST=0, FRIEND_REQUEST, FRIEND_RESPONSE, FOLLOWING_LIST, GROUP_LIST, MEMBER_ADMIN, MEMBERADMIN_ADMIN, GROUP_INVITE, GROUP_JOIN };

    public Mode ControlMode { get; set; }

    public bool IsOwner
    {
        get
        {
            if (ViewState["IsOwner"] != null)
            {
                return Convert.ToBoolean(ViewState["IsOwner"]);
            }
            return false;
        }
        set
        {
            ViewState["IsOwner"] = value;
        }
    }

    public string Title { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {

        }
    }      
}
