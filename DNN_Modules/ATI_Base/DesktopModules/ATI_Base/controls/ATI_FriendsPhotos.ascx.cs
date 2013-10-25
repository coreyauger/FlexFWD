using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;
using Affine.Data.EventArgs;

namespace Affine
{
    public enum Relationship { FRIEND, FOLLOWING };   
}

public partial class DesktopModules_ATI_Base_controls_ATI_FriendsPhotos : DotNetNuke.Framework.UserControlBase
{       
    public IList<UserSettings> FriendKeyList
    {
        get
        {
            if (ViewState["FriendKeyList"] == null)
            {
                return null;
            }
            return (IList<UserSettings>)ViewState["FriendKeyList"];
        }
        set
        {
            ViewState["FriendKeyList"] = value;
        }
    }

    private string _FriendTerm = "Friend";
    public string FriendTerm
    {
        get { return _FriendTerm; }
        set { _FriendTerm = value; }
    }
    private string _FriendTermPlural = "Friends";
    public string FriendTermPlural
    {
        get { return _FriendTermPlural; }
        set { _FriendTermPlural = value; }
    }

    private Affine.Relationship _Relationship = Affine.Relationship.FRIEND;
    public Affine.Relationship RelationshipType
    {
        get { return _Relationship; }
        set { _Relationship = value; }
    }

    public UserSettings User
    {
        get;
        set;
    }

    public int FriendCount
    {
        get;
        set;
    }

    public string FriendListLink { get; set; }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack || !Page.IsCallback)
        {
            if (this.User != null)
            {
                if ( !string.IsNullOrEmpty(this.FriendListLink) &&  this.FriendListLink.StartsWith("javascript:"))
                {
                    hlSeeAll.HRef = "javascript: ;";
                    hlNumFriends.HRef = "javascript: ;";
                    string js = this.FriendListLink.Replace("javascript:","");
                    hlSeeAll.Attributes["onclick"] = js;
                    hlNumFriends.Attributes["onclick"] = js;
                }
                else
                {
                    hlSeeAll.HRef = this.FriendListLink;
                    hlNumFriends.HRef = hlSeeAll.HRef;
                }
            }
            if (this.FriendCount <= 0)
            {
                hlSeeAll.Visible = false;
                hlNumFriends.Visible = false;
            }
            else
            {
                if (this.RelationshipType == Affine.Relationship.FRIEND)
                {
                    hlNumFriends.InnerHtml = this.FriendCount + " " + (this.FriendCount > 1 ? this.FriendTermPlural : this.FriendTerm);
                }
                else
                {
                    hlNumFriends.InnerHtml = "Following " + this.FriendCount + (this.FriendCount > 1 ? " " + this.FriendTermPlural : " " + this.FriendTerm);
                }
            }
            if (this.FriendKeyList != null)
            {

                if( this.FriendKeyList.Count > 0 ){
                    img1.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?u=" + this.FriendKeyList[0].UserKey + "&p="+this.PortalSettings.PortalId);
                    img1LinkB.InnerHtml =  this.FriendKeyList[0].UserName;
                    img1LinkB.HRef = ResolveUrl("~/" + this.FriendKeyList[0].UserName);
                    img1LinkA.HRef = img1LinkB.HRef;
                    img1.Visible = true;
                }
                if (this.FriendKeyList.Count > 1)
                {
                    img2.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?u=" + this.FriendKeyList[1].UserKey + "&p=" + this.PortalSettings.PortalId);
                    img2LinkB.InnerHtml = this.FriendKeyList[1].UserName;
                    img2LinkB.HRef = ResolveUrl("~/" + this.FriendKeyList[1].UserName);
                    img2LinkA.HRef = img2LinkB.HRef;
                    img2.Visible = true;
                }
                if (this.FriendKeyList.Count > 2)
                {
                    img3.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?u=" + this.FriendKeyList[2].UserKey + "&p=" + this.PortalSettings.PortalId);
                    img3LinkB.InnerHtml = this.FriendKeyList[2].UserName;
                    img3LinkB.HRef = ResolveUrl("~/" + this.FriendKeyList[2].UserName);
                    img3LinkA.HRef = img3LinkB.HRef;
                    img3.Visible = true;
                }
                if (this.FriendKeyList.Count > 3)
                {
                    img4.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?u=" + this.FriendKeyList[3].UserKey + "&p=" + this.PortalSettings.PortalId);
                    img4LinkB.InnerHtml = this.FriendKeyList[3].UserName;
                    img4LinkB.HRef = ResolveUrl("~/" + this.FriendKeyList[3].UserName);
                    img4LinkA.HRef = img4LinkB.HRef;
                    img4.Visible = true;
                }
                if (this.FriendKeyList.Count > 4)
                {
                    img5.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?u=" + this.FriendKeyList[4].UserKey + "&p=" + this.PortalSettings.PortalId);
                    img5LinkB.InnerHtml = this.FriendKeyList[4].UserName;
                    img5LinkB.HRef = ResolveUrl("~/" + this.FriendKeyList[4].UserName);
                    img5LinkA.HRef = img5LinkB.HRef;
                    img5.Visible = true;
                }
                if (this.FriendKeyList.Count > 5)
                {
                    img6.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?u=" + this.FriendKeyList[5].UserKey + "&p=" + this.PortalSettings.PortalId);
                    img6LinkB.InnerHtml = this.FriendKeyList[5].UserName;
                    img6LinkB.HRef = ResolveUrl("~/" + this.FriendKeyList[5].UserName);
                    img6LinkA.HRef = img6LinkB.HRef;
                    img6.Visible = true;
                }
            }
           
           
        }
    }          

}
