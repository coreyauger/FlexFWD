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

public partial class DesktopModules_ATI_Base_controls_ATI_FriendFinder : DotNetNuke.Framework.UserControlBase
{
    public IList<long> SelectedItems
    {
        get {
            IList<long> ret = new List<long>();
            string[] split = atiUserIdArray.Value.Split(',');
            foreach( string i in split ){
                try{
                    ret.Add( Convert.ToInt64(i) );
                }catch(Exception){
                }
            }
            return ret;           
        }
    }

    public UserSettings[] UserSettings
    {
        set;
        private get;
    }

    public string CssClass
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack && !Page.IsPostBack)
            {
                atiUserNames.Attributes["class"] = this.CssClass;               
                if (this.UserSettings != null)
                {
                    atiUserIdArray.Value = string.Empty;
                    foreach (UserSettings friendProfile in this.UserSettings)
                    {
                       // litScript.Text = "t5.add('" + friendProfile.UserName + " (" + friendProfile.UserFirstName + "," + friendProfile.UserLastName + ")','" + friendProfile.UserKey + "');";
                        ListItem option = new ListItem(){
                            Text =  friendProfile.UserName + " (" + friendProfile.UserFirstName + "," + friendProfile.UserLastName + ")",
                            Value = "" + friendProfile.Id
                        };
                        atiUserIdArray.Value += friendProfile.Id + ",";
                        option.Attributes["class"] = "selected";
                        atiUserNames.Items.Add(option);
                    }
                }
            }               
        }
        catch (DotNetNuke.Services.Exceptions.ModuleLoadException mlex)
        {

        }
    }
}
