using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

using DotNetNuke.Entities.Users;
using DotNetNuke;
using DotNetNuke.Common.Lists;
using DotNetNuke.Services.Localization;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_SendMessageScript : DotNetNuke.Framework.UserControlBase
{
    // TODO: this will be the user select cotnrol as soon as it is done
    public string To
    {
        get { return ""; }
       // get { return atiFriendFinder.Text; }
       // set { atiFriendFinder.Text = value; }
    }

    public UserSettings[] UserSettings
    {
        set;
        //{
        //    atiFriendFinder.UserSettings = value; 

        //}
        private get;
    }

    public string Json
    {
        get;
        private set;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                if (this.UserSettings != null)
                {
                    foreach (UserSettings us in this.UserSettings)
                    {
                        lUserNames.Text = us.UserName;
                    }
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    this.Json = serializer.Serialize(this.UserSettings.Select(us => us.Id).ToArray());
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "MessageSend", "(function(){Aqufit.Page.Controls.SendMessage.init();})(); ", true);
            }
        }
        catch (DotNetNuke.Services.Exceptions.ModuleLoadException mlex)
        {

        }
    }
}
