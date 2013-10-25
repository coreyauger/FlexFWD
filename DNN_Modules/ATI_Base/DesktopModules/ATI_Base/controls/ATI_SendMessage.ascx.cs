using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;
using DotNetNuke;
using DotNetNuke.Common.Lists;
using DotNetNuke.Services.Localization;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_SendMessage : DotNetNuke.Framework.UserControlBase
{

    private int _StartTabIndex = 1;

    // TODO: this will be the user select cotnrol as soon as it is done
    public IList<long> To
    {
        get { return atiFriendFinder.SelectedItems; }
       // set { atiFriendFinder.Text = value; }
    }

    public UserSettings[] UserSettings
    {
        set { atiFriendFinder.UserSettings = value; }
    }

    public string Subject
    {
        get { return atiTxtSubject.Text; }
        set { atiTxtSubject.Text = value; }
    }

    public string Message
    {
        get { return atiTxtMessage.Text; }
        set { atiTxtMessage.Text = value; }
    }

    public Message ReplyToMessage
    {
        get { return (Message)this.ViewState["ReplyToMessage"]; }
        set
        {            
            this.ViewState["ReplyToMessage"] = value;
            if (value == null)
            {
                atiFriendFinder.Visible = true;
                atiTxtSubject.Visible = true;                
            }
            else
            {
                atiFriendFinder.Visible = false;
                atiTxtSubject.Visible = false;                
            }
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        }
        catch (DotNetNuke.Services.Exceptions.ModuleLoadException mlex)
        {

        }
    }
}
