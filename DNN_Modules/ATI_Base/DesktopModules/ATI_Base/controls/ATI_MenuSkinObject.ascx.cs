using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Skins.Controls;

using Affine.Data;
using Telerik.Web.UI;

namespace Affine.Web.Controls
{

    public partial class ATI_MenuSkinObject : DotNetNuke.UI.Skins.SkinObjectBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                if (PortalSettings.UserId <= 0)
                {   // if no one is logged in then remove the searh
                    atiMenuSkinObject.Visible = false;
                    atiMenuSkinJoin.Visible = true;
                }
                else
                {
                    pImg.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + PortalSettings.UserId + "&p=" + PortalSettings.PortalId;
                 //   pLog.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iLogWorkout.png");
                    
                    // TODO: put this in a base skin class..
                    aqufitEntities entities = new aqufitEntities();
                    User ProfileSettings = entities.UserSettings.OfType<User>().First(u => u.UserKey == PortalSettings.UserId && u.PortalKey == PortalSettings.PortalId);

                    // check if we need to remove a notification (because someone selected it from the menu skin object)
                    if (Request["rn"] != null)
                    {
                        long nId = Convert.ToInt64(Request["rn"]);
                        Notification notification = entities.UserStreamSet.OfType<Notification>().FirstOrDefault(n => n.UserSetting.Id == ProfileSettings.Id && n.Id == nId);
                        if (notification != null)
                        {
                            entities.DeleteObject(notification);
                            entities.SaveChanges();
                        }
                    }

                    Notification[] notifications = entities.UserStreamSet.OfType<Notification>().Include("WOD").Include("Message").Where(n => n.UserSetting.Id == ProfileSettings.Id && n.PublishSettings != (int)Affine.Utils.ConstsUtil.PublishSettings.NOTIFICATION_READ).OrderByDescending( n => n.Id ).Take(25).ToArray();
                    int notificationCount = notifications.Count();
                    
                    string baseUrl = ResolveUrl("~");
                    string notificationUrl = ResolveUrl("~/fitnessprofile/requests");
                    // Request Status are as follow ( 0 == Accepted, 1 == Request, 2 == Rejected )
                    long friendRequestCount = entities.UserRequestSet.OfType<FriendRequest>().Where(r => r.FriendRequestSettingsId == ProfileSettings.Id && r.Status == 1 ).Count();
                    litDebug.Text += "<a href=\"" + notificationUrl + "\">View Notifications</a>";
                    if (friendRequestCount > 0)
                    {
                        litDebug.Text += "<a href=\"" + notificationUrl + "\">You have " + friendRequestCount + " friend requests</a>";
                        notificationCount++;
                    }
                    long inviteRequestCount = entities.UserRequestSet.OfType<GroupInviteRequest>().Where(r => r.FriendRequestSettingsId == ProfileSettings.Id && r.Status != 0).Count();
                    if (inviteRequestCount > 0)
                    {
                        litDebug.Text += "<a href=\"" + notificationUrl + "\">" + inviteRequestCount + " group invites</a>";
                        notificationCount++;
                    }
                    lNumUnread.Text = "" + notificationCount;
                    foreach (Notification n in notifications)
                    {
                        string linkUrl = notificationUrl + "?n=" + n.Id;
                        if (n.NotificationType == (int)Affine.Utils.ConstsUtil.NotificationTypes.NEW_MESSAGE)
                        {
                            linkUrl = baseUrl + "Profile/Inbox?m=" + n.Message.Id + "&rn="+n.Id;
                        }
                        else if (n.NotificationType == (int)Affine.Utils.ConstsUtil.NotificationTypes.WORKOUT_SCHEDULED)
                        {
                            linkUrl = baseUrl + "workout/"+n.WOD.Id + "?rn=" + n.Id;
                        }
                        litDebug.Text += "<a href=\"" + linkUrl+"\">" + n.Title + "</a>";
                        //litDebug.Text += "<br /><br />";
                    }

                    hlProfile.HRef = ResolveUrl("~/") + ProfileSettings.UserName;
                    hlGroups.HRef = ResolveUrl("~/Groups");
                    hlInbox.HRef = ResolveUrl("~/Profile/Inbox");
                    hlFriends.HRef = ResolveUrl("~/" + ProfileSettings.UserName + "/friends");
                    hlRoutes.HRef = ResolveUrl("~/Profile/MyRoutes");
                    hlWorkouts.HRef = ResolveUrl("~/" + ProfileSettings.UserName + "/workout-history");
                }
            }
        }


    }
}
