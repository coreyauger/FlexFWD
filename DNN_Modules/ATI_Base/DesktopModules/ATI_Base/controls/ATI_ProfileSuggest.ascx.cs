using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_ProfileSuggest : DotNetNuke.Framework.UserControlBase
{
    public UserSettings ProfileSettings { get; set; }
    private bool _IsOwner = false;
    private string urlBase;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback && this.ProfileSettings != null)
        {
            _IsOwner = (this.ProfileSettings.UserKey == (long)PortalSettings.UserId);
            urlBase = ResolveUrl("~/");
            if (!_IsOwner)
            {
                this.Visible = false;
            }
            if (this.ProfileSettings.MainGroupKey != null && this.Visible)
            {
                Affine.WebService.StreamService ss = new Affine.WebService.StreamService();
                string json = ss.getFriendSuggestions(ProfileSettings.Id, this.ProfileSettings.MainGroupKey.Value, null, 2);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "SugestedFriends", "$(function(){ Aqufit.Page." + this.ID + ".geneateListDom('"+json+"') });", true);
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}
