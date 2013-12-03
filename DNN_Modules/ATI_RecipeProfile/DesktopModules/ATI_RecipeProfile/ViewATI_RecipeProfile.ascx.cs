/*
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Linq;
using System.IO;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;


using Affine.Data;
using Affine.Utils.Linq;

namespace Affine.Dnn.Modules.ATI_RecipeProfile
{
    

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewATI_Builder class displays the content
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    partial class ViewATI_RecipeProfile : Affine.Dnn.Modules.ATI_PermissionPageBase
    {

        #region Private Members
        #endregion       

        #region Public Methods
      
        #endregion

        #region Event Handlers

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
           try
            {
                base.Page_Load(sender, e);
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    if (ProfileSettings == null)
                    {
                        Response.Redirect(ResolveUrl( "~/" ), true);
                    }
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    atiProfileImage.Settings = base.ProfileSettings;
                    atiProfileImage.IsOwner = base.Permissions == AqufitPermission.OWNER;
                    atiStreamScript.IsFollowMode = true;
                    atiStreamScript.DefaultTake = 10;
                    bEditProfile.HRef = ResolveUrl("~/Register.aspx");
                    litFriendsTitle.Text = "Chefs " + ProfileSettings.UserName + " follows";
                    atiWebLinksList.ProfileSettings = base.ProfileSettings;  
                    if (Permissions == AqufitPermission.OWNER )
                    {
                        atiFollow.Visible = false;
                        atiWebLinksList.IsOwner = true;
                    }
                    else if (Permissions == AqufitPermission.PUBLIC)
                    {
                        string url = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.LoginTabId, "Login", new string[] { "returnUrl=/" + this.ProfileSettings.UserName });
                        atiFollow.OnClientClick = "self.location.href='" + url + "'; return false;";
                        atiFollow.Click -= atiFollow_Click;
                    }
                    else
                    {
                        atiFollow.Text = base.Following ? "Unfollow " : "Follow " + ProfileSettings.UserName;
                    }
                    aqufitEntities entities = new aqufitEntities();
                    IList<long> friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == this.ProfileSettings.Id) ).Select(f =>f.DestUserSettingKey).ToList();
                    UserSettings[] firendSettings = entities.UserSettings.Where(LinqUtils.BuildContainsExpression<UserSettings, long>(s => s.Id, friendIds)).Where( f => f.PortalKey == this.PortalId ).ToArray();
                    atiFriendsPhotos.RelationshipType = Affine.Relationship.FOLLOWING;
                    atiFriendsPhotos.FriendKeyList = firendSettings;
                    atiFriendsPhotos.User = base.ProfileSettings;
                    atiFriendsPhotos.FriendCount = friendIds.Count;

                    Metric numRecipes = ProfileSettings.Metrics.FirstOrDefault(m => m.MetricType == (int)Utils.MetricUtil.MetricType.NUM_RECIPES);
                    lNumCreations.Text = lNumCreations2.Text = ProfileSettings.UserName + "'s Creations (" + (numRecipes != null ? numRecipes.MetricValue : "0") + ")";
                    lNumFavorites.Text = lNumFavorites2.Text = "" + entities.User2StreamFavorites.Where(f => f.UserKey == ProfileSettings.UserKey && f.PortalKey == ProfileSettings.PortalKey).Count();
                    lUserName.Text = ProfileSettings.UserKey == this.UserId ? "you" : ProfileSettings.UserName;

                                     
                    

                    atiStreamScript.EditUrl = ResolveUrl("~/AddRecipe.aspx");                    
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        protected void atiFollow_Click(object sender, EventArgs e)
        {
            try
            {             
                Affine.WebService.StreamService ss = new WebService.StreamService();
                if (base.Following) // this is an unfollow then
                {
                    ss.UnFollowUser(this.UserId, this.PortalId, this.ProfileSettings.UserKey);
                    litStatus.Text = "You are <em>No longer following " + this.ProfileSettings.UserName + "</em>";
                    base.Following = false;
                }
                else  // we want to follow this user
                {
                    ss.FollowUser(this.UserId, this.PortalId, this.ProfileSettings.UserKey);
                    litStatus.Text = "You are now <em>following " + this.ProfileSettings.UserName + "</em>";
                    base.Following = true;
                }
                atiFollow.Text = base.Following ? "Unfollow " : "Follow " + ProfileSettings.UserName;
            
            }
            catch (Exception)
            {
                litStatus.Text = "There was an <em>Error</em> with the operation.  Please contact support.";
            }
        }
      

       

     
        protected void bPostBack_Click(object sender, EventArgs e)
        {

        }
          
                      

        #endregion

        #region Optional Interfaces

        #endregion

    }
}

