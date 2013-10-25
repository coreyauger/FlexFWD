using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_ProfileLinks : DotNetNuke.Framework.UserControlBase
{
    public UserSettings ProfileSettings { get; set; }

    private bool _IsOwner = false;
    public bool IsOwner { get { return _IsOwner; } set { _IsOwner = value; } }

    private bool _IsFriend = false;
    public bool IsFriend { get { return _IsFriend; } set { _IsFriend = value; } }

    private bool _ShowGettingSarted = true;
    public bool ShowGettingSarted { get { return _ShowGettingSarted; } set { _ShowGettingSarted = value; } }
    
    private Affine.Utils.ConstsUtil.ProfileMode _ProfileMode = Affine.Utils.ConstsUtil.ProfileMode.NORMAL;
    public Affine.Utils.ConstsUtil.ProfileMode Mode { get { return _ProfileMode; } set { _ProfileMode = value; } }

    private string urlBase;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback && this.ProfileSettings != null)
        {
            urlBase = ResolveUrl("~/");            
            hlStats.HRef = urlBase + ProfileSettings.UserName + "/achievements";
            hlGroups.HRef = ResolveUrl("~/Groups");
            hlMyFriends.HRef = urlBase + ProfileSettings.UserName + "/friends";
            hlWorkouts.HRef = urlBase + ProfileSettings.UserName + "/workout-history";
            hlInbox.HRef = ResolveUrl("~/Profile/Inbox");
            hlViewPhotos.HRef = ResolveUrl("~/" + ProfileSettings.UserName + "/photos");
            liEdit.Visible = liEdit2.Visible = false;
            if (ProfileSettings is Group)
            {
                hlMembers.HRef = urlBase + ProfileSettings.UserName + "/friends";
            }
            else
            {
                liMembers.Visible = false;
            }
            
            if (this.IsOwner)
            {
                liWorkout.Visible = true;
                liEdit.Visible = liEdit2.Visible = true;
                liRoutes.Visible = true;
                hlRoutes.HRef = ResolveUrl("~/Profile/MyRoutes");
                hlMyWorkouts.HRef = ResolveUrl("~/Profile/MyWorkouts");
                bEditProfile.HRef = ResolveUrl("~/Home/Register");
                aGettingStarted.HRef = ResolveUrl("~/Profile/GettingStarted");
                liGettingStarted.Visible = this.ShowGettingSarted;
            }
            else
            {
                liGettingStarted.Visible = false;
                liFriends.Visible = false;
                divExtraLinks.Visible = false;
                liWorkout.Visible = false;
            }

            if (this.Mode == Affine.Utils.ConstsUtil.ProfileMode.NORMAL)
            {                
                if (this.ProfileSettings is Group)
                {
                    if (this.IsOwner)
                    {
                        liCreateWorkout.Visible = true;
                        bEditProfile.HRef = ResolveUrl("~/") + "group/"+ProfileSettings.UserName+"/settings";
                    }                    
                    lMyFriends.Text = "Members";
                    liFriends.Visible = false;
                    divExtraLinks.Visible = false;
                    litStats.Text = "Leader Board";
                    liWorkout.Visible = false;
                    liFriends.Visible = false;
                    liPhotos.Visible = false;
                    litEditProfile.Text = "Edit Group Settings";
                    liGettingStarted.Visible = false;
                }               
            }
            else if (this.Mode == Affine.Utils.ConstsUtil.ProfileMode.BIO)
            {
                liEdit.Visible = false;
                liEdit2.Visible = false;
                liFriends.Visible = false;
                liMessages.Visible = false;
                liSpace.Visible = true;
                liWorkout.Visible = false;
                if (this.IsOwner)
                {
                    liAddBio.Visible = true;
                    hlAddBio.HRef = this.urlBase + "Home/Register?t=2";
                }
                
            }
            if (this.IsFriend) // You are viewing a friends profile... so should the "Send Message Button";
            {
                hlMessages.HRef = "javascript: void();";
                hlMessages.Attributes["onclick"] = "Aqufit.Windows.MessageWin.open();";
            }
            else
            {
                liMessages.Visible = false;                
            }            

        }
    }
}
