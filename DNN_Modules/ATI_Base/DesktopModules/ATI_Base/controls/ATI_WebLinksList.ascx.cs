using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Users;

using DotNetNuke;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_WebLinksList : DotNetNuke.Framework.UserControlBase
{
     

    public string ValidationGroupName
    {
        get;
        set;
    }

    public UserSettings ProfileSettings
    {
        get;
        set;
    }

    public bool HidePersonalLink { get; set; }

    public bool IsOwner
    {
        get
        {
            if (ViewState["IsOwner"] == null)
            {
                return false;
            }
            return Convert.ToBoolean(ViewState["IsOwner"]);
        }
        set
        {
            ViewState["IsOwner"] = value;
        }

    }

    public string AddSitesUrl { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && this.ProfileSettings != null)
        {
            UserSettingsExtended[] webLinks = this.ProfileSettings.UserSettingsExtendeds.Where(s => s.Class == 1).ToArray();
            if (webLinks.Length == 0 && this.IsOwner)
            {
                aAddSites.Visible = true;
                string toAddUrl = this.ProfileSettings is Group ? ResolveUrl("~/") + "group/" + this.ProfileSettings.UserName + "/settings?t=2" : ResolveUrl("~/Register");
                aAddSites.HRef = string.IsNullOrEmpty(this.AddSitesUrl) ? toAddUrl : this.AddSitesUrl;
            }        
            UserSettingsExtended twitter = webLinks.FirstOrDefault(s => s.Name == "Twitter");
            if (twitter != null)
            {
                imgWebTwitter.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iTwitter.png");
                aTwitter.HRef = twitter.Value;
            }
            else
            {
                liTwitter.Visible = false;
            }
            UserSettingsExtended facebook = webLinks.FirstOrDefault(s => s.Name == "Facebook");
            if (facebook != null)
            {
                imgWebFaceBook.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFaceBook.png");
                aFacebook.HRef = facebook.Value;
            }
            else
            {
                liFaceBook.Visible = false;
            }
            UserSettingsExtended linkedin = webLinks.FirstOrDefault(s => s.Name == "LinkedIn");
            if (linkedin != null)
            {
                imgWebLinkedIn.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iLinkedIn.png");
                aLinkedIn.HRef = linkedin.Value;
            }
            else
            {
                liLinkedIn.Visible = false;
            }
            UserSettingsExtended youtube = webLinks.FirstOrDefault(s => s.Name == "YouTube");
            if (youtube != null)
            {
                imgWebYouTube.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iYouTube.png");
                aYouTube.HRef = youtube.Value;
            }
            else
            {
                liYouTube.Visible = false;
            }
            UserSettingsExtended flickr = webLinks.FirstOrDefault(s => s.Name == "Flickr");
            if (flickr != null)
            {
                imgWebFlickr.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFlickr.png");
                aFlickr.HRef = flickr.Value;
            }
            else
            {
                liFlickr.Visible = false;
            }
            UserSettingsExtended personal = webLinks.FirstOrDefault(s => s.Name == "Personal");
            if (personal != null && ! this.HidePersonalLink )
            {
                aPersonSite.HRef = personal.Value.Contains("http://") ? personal.Value : "http://" + personal.Value;
                aPersonSite.InnerHtml = "Visit " + this.ProfileSettings.UserName + "'s Personal Site";
            }
            else
            {
                aPersonSite.Visible = false;
            }            
        }
    }
  
   
}
