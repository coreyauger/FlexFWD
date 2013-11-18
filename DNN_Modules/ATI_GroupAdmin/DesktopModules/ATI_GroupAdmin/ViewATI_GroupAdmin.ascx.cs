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
' TO THE WARRANTIES OF MERCHANTABILITY, REGISTER FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
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
using System.Web;
using System.Net;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Entities.Users;

using Telerik.Web.UI;
using Affine.Data;

namespace Affine.Dnn.Modules.ATI_GroupAdmin
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
    partial class ViewATI_GroupAdmin : ATI_PermissionPageBase, IActionable
    {

        #region Private Members     
   
       
        #endregion       

        #region Public Methods

        public string BackgroundImageUrl { get; set; }
        public string ProfileCSS { get; set; }

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            //base._PerformFBLoginCheck = false;
            base.OnInit(e);
        }

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
            base.Page_Load(sender, e);
            try
            {
                
                this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?us=0&bg=1";
                aqufitEntities entities = new aqufitEntities();
                GroupType[] gTypes = entities.GroupTypes.ToArray();
                
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/RegisterService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    ServiceReference service2 = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service2.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service2);     

                    if (Request.Url.AbsoluteUri.Contains("http://www."))
                    {
                        Response.Redirect(Request.Url.AbsoluteUri.Replace("http://www.", "http://"), true);
                        return;    
                    }
                    if (Request["t"] != null)
                    {
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Tabs.SwitchTab(" + Request["t"] + "); }); ");
                    }
                    if (Request["oauth_token"] != null)
                    {
                        RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.Tabs.SwitchTab(4); }); ");
                    }
                    atiFindInvite.UserSettings = UserSettings;
                    atiFindInvite.TabId = TabId;

                    ddlGroupType.DataSource = gTypes;
                    ddlGroupType.DataTextField = "TypeName";
                    ddlGroupType.DataValueField = "Id";
                    ddlGroupType.DataBind();

                    imgError.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png");
                    imgCheck.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png");
                                  

                    if (GroupSettings != null)   // This is a person trying to edit a group profile
                    {
                        atiFindInvite.GroupSettings = GroupSettings;
                        hrefBackToProfile.Visible = true;
                        hrefBackToProfile.HRef = ResolveUrl("~/") + "group/" + GroupSettings.UserName;
                        hrefBackToProfile.InnerHtml = "Back to " + GroupSettings.UserName + " page";
                        this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?us=" + GroupSettings.Id + "&bg=1";
                        this.ProfileCSS = GroupSettings.CssStyle;
                        bCreateGroup.Text = "Update";
                        atiProfileImage.Settings =  GroupSettings;
                        atiProfileImage.GroupUserName = GroupSettings.UserName;
                        UserFriends perm = entities.UserFriends.FirstOrDefault(w => w.SrcUserSettingKey == UserSettings.Id && w.DestUserSettingKey == GroupSettings.Id && w.Relationship <= (int)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN);
                        // check that they have permissions first (need to be a group admin)
                        if (perm != null )
                        {
                            SetupAccountEdit(GroupSettings);
                        }
                        else
                        {
                            Response.Redirect(ResolveUrl("~") + UserSettings.UserName, true);
                            return;
                        }

                    }
                    else
                    {   // This is a new group to register...
                        atiProfileImage.Visible = false;      
                        RadAjaxManager1.ResponseScripts.Add("$(function(){ $('#tabs').tabs('option', 'disabled', [1,2,3, 4]); });");
                    }
                   
                }                                              
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetupAccountEdit(Group group)
        {
            aqufitEntities entities = new aqufitEntities();
            group = entities.UserSettings.OfType<Group>().Include("GroupType").Include("UserSettingsExtendeds").Include("Places").FirstOrDefault(g => g.Id == GroupSettings.Id);
                    
            hiddenGroupKey.Value = "" + GroupSettings.Id;                            
            UserFriends ownerId = entities.UserFriends.FirstOrDefault(f => (f.SrcUserSettingKey == group.Id || f.DestUserSettingKey == group.Id) && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER);
            if (ownerId != null)
            {
                atiOwnerProfile.Settings = entities.UserSettings.FirstOrDefault(u => u.Id == (ownerId.SrcUserSettingKey == group.Id ? ownerId.DestUserSettingKey : ownerId.SrcUserSettingKey));
            }
            else
            {
                atiOwnerProfile.Visible = false;
            }

            
            // setup the Basic Info section
            txtGroupName.Text = group.UserFirstName;
            atiGroupName.Text = group.UserName;
            Place place = group.Places.FirstOrDefault();
            if (place != null)
            {
                atTxtGroupDescription.Text = place.Description;
            }
            atiGroupName.Enabled = false;
            atiTxtGroupEmail.Text = group.UserEmail;
            ddlGroupType.SelectedValue = "" + group.GroupType.Id;            

            atiAddress.Visible = false;
            // TODO: for now we don't allow a place edit.
            /*
            Place place = entities.Places.FirstOrDefault( p => p.UserSetting.Id == group.Id );
            if( place != null ){
                atiAddress.City = place.City;
                atiAddress.Region = place.Region;
                atiAddress.
            }
             **/
            UserSettingsExtended[] webLinks = group.UserSettingsExtendeds.Where(s => s.Class == 1).ToArray();
            UserSettingsExtended facebook = webLinks.FirstOrDefault(w => w.Name == "Facebook");
            atiWebLinks.Facebook = facebook != null ? facebook.Value : null;
            UserSettingsExtended twitter = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "Twitter");
            atiWebLinks.Twitter = twitter != null ? twitter.Value : null;
            UserSettingsExtended linkedin = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "LinkedIn");
            atiWebLinks.LinkedIn = linkedin != null ? linkedin.Value : null;
            UserSettingsExtended youtube = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "YouTube");
            atiWebLinks.YouTube = youtube != null ? youtube.Value : null;
            UserSettingsExtended flickr = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "Flickr");
            atiWebLinks.Flickr = flickr != null ? flickr.Value : null;
            UserSettingsExtended personal = webLinks.FirstOrDefault(s => s.Class == 1 && s.Name == "Personal");
            atiWebLinks.Peronsal = personal != null ? personal.Value : null;


            // setup the advanced settings
            UserFriends follow = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == group.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW);
            if (follow != null)
            {
                Group followedGroup = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == follow.DestUserSettingKey);
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = followedGroup.UserFirstName;
                item.Value = "" + followedGroup.Id;
                atiRadComboBoxSearchGroups.Items.Add(item);
                atiRadComboBoxSearchGroups.SelectedIndex = atiRadComboBoxSearchGroups.Items.Count - 1;
            }

            // Next we do the appearance settings...
            if (! string.IsNullOrWhiteSpace( group.CssStyle ))
            {
                const string findMe = "background-color:";
                int bgInd = group.CssStyle.IndexOf(findMe);
                if (bgInd >= 0)
                {
                    string color = group.CssStyle.Substring(bgInd + findMe.Length, group.CssStyle.IndexOf(";", bgInd + findMe.Length) - (bgInd + findMe.Length)).Replace("#", "");
                    atiThemeEditor.BackgroundColor = System.Drawing.Color.FromName("#"+color.Trim());
                }
                if (group.CssStyle.Contains("background-repeat: repeat;"))
                {
                    atiThemeEditor.IsTiled = true;
                }
            }

            // Next the member list
            Affine.WebService.StreamService ss = new WebService.StreamService();
            string json = ss.getMemberListDataOfRelationship(GroupSettings.Id, (int)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN, 0, 10);

            string json2 = ss.getMemberListDataOfRelationship(GroupSettings.Id, (int)Affine.Utils.ConstsUtil.Relationships.GROUP_MEMBER, 0, 25);
            RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.atiMemberListAdmin.generateStreamDom('" + json + "');  Aqufit.Page.atiMemberList.generateStreamDom('" + json2 + "');  }); ");

        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                Affine.WebService.StreamService ss = new WebService.StreamService();
                long mid = 0;
                long gid = Convert.ToInt64(hiddenGroupKey.Value);;
                switch (hiddenAjaxAction.Value)
                {
                    case "makeAdmin":
                        mid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.makeMemberGroupAdmin(UserSettings.Id, gid, mid);
                        string json = ss.getMemberListDataOfRelationship(gid, (int)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN, 0, 10);
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiMemberListAdmin.generateStreamDom('" + json + "'); ");
                        break;  
                    case "removeAdmin":
                        mid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.removeMemberGroupAdmin(UserSettings.Id, gid, mid);
                        string json2 = ss.getMemberListDataOfRelationship(gid, (int)Affine.Utils.ConstsUtil.Relationships.GROUP_MEMBER, 0, 25);
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiMemberList.generateStreamDom('" + json2 + "'); ");
                        break;
                    case "removeMember":
                        mid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.removeMemberGroup(UserSettings.Id, gid, mid);
                        break;
                    case "sendGroupInvite":
                        long usid = Convert.ToInt64(hiddenAjaxValue.Value);
                        dataMan.inviteUserToGroup(usid, gid);
                        break;
                    case "friendData":
                        int skip = Convert.ToInt32(hiddenAjaxValue.Value);
                        int take = Convert.ToInt32(hiddenAjaxValue2.Value);
                        json = ss.getFriendListData(UserSettings.UserKey, this.PortalId, Affine.Utils.ConstsUtil.FriendListModes.FRIEND, skip, take, gid);
                        RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiFriendList.generateStreamDom('" + json + "'); Aqufit.Page.atiLoading.remove(); ");
                        break;
                }
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog('" + ex.Message.Replace("'","") + "'); ");
            }           
        }



        protected void UploadThemeBackground()
        {
            FileUpload fileUpload = atiThemeEditor.FileUpload;
            if (fileUpload.HasFile)
            {                
                long usid = 0;
                if (GroupSettings != null)
                {
                    aqufitEntities entities = new aqufitEntities();
                    UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == GroupSettings.Id || f.SrcUserSettingKey == GroupSettings.Id && f.DestUserSettingKey == UserSettings.Id);
                    if (uf != null && (uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER || uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN))
                    {   // Relation
                        usid = GroupSettings.Id;    // allow the edit of the group profile pic if the user is an admin
                        if (fileUpload.PostedFile.ContentType == "image/jpeg" || fileUpload.PostedFile.ContentType == "image/png" || fileUpload.PostedFile.ContentType == "image/jpg" || fileUpload.PostedFile.ContentType == "image/gif")
                        {
                            if (fileUpload.PostedFile.ContentLength < 3072000)
                            {
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(fileUpload.FileBytes);
                                Affine.Utils.ImageUtil.SaveBackgroundImage(ms, usid);
                            }
                            else
                            {
                                throw new Exception("Upload status: The file has to be less than 3 MB!");
                            }
                        }
                        else
                        {
                            throw new Exception("Upload status: Only JPEG, PNG, GIF files are accepted!");
                        }
                    }
                    else
                    {
                        throw new Exception("You must be an admin to perform this operation.");
                    }
                }                            
            }
        }       

        protected void atiRadComboBoxSearchGroups_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            RadComboBox atiRadComboBoxSearchGroups = (RadComboBox)sender;
            atiRadComboBoxSearchGroups.Items.Clear();
            const int TAKE = 15;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<Group> friends = entities.UserSettings.OfType<Group>().OrderBy(w => w.UserName);
            if (!string.IsNullOrEmpty(e.Text))
            {
                friends = friends.Where(w => w.UserName.ToLower().Contains(e.Text) || w.UserFirstName.ToLower().Contains(e.Text));
            }
            int length = friends.Count();
            friends = friends.Skip(itemOffset).Take(TAKE);
            Group[] groups = friends.ToArray();

            foreach (Group g in groups)
            {
                RadComboBoxItem item = new RadComboBoxItem(g.UserFirstName);
                item.Value = "" + g.Id;
                //   item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                atiRadComboBoxSearchGroups.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }


        protected void bCreateGroup_Click(object sender, EventArgs e)
        {
            if( base.GroupSettings == null )
            {
                RegisterGroup();                
            }
            else
            {                
                // Update
                UpdateGroup();
            }
        }

        private void UpdateGroup()
        {
            if (Page.IsValid)
            {
                try
                {
                    // check required fields  
                    aqufitEntities entities = new aqufitEntities();
                    Group group = entities.UserSettings.OfType<Group>().Include("GroupType").Include("UserSettingsExtendeds").Include("Places").FirstOrDefault(g => g.Id == GroupSettings.Id);
                    
                    group.UserEmail = atiTxtGroupEmail.Text;
                    group.UserFirstName = txtGroupName.Text;
                    Place place = group.Places.FirstOrDefault();
                    if (place != null)
                    {
                        place.Description = atTxtGroupDescription.Text;
                    }
                    int gtype = Convert.ToInt32(ddlGroupType.SelectedValue);
                    
                    group.GroupType = entities.GroupTypes.FirstOrDefault(gt => gt.Id == gtype);

                    long followId = 0;
                    if (!string.IsNullOrWhiteSpace(atiRadComboBoxSearchGroups.SelectedValue))
                    {
                        followId = Convert.ToInt64(atiRadComboBoxSearchGroups.SelectedValue);
                        
                    }
                    // first check if the group is following any other group;
                    UserFriends follow = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == group.Id && f.Relationship == (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW);
                    if (follow != null)
                    {
                        // make sure it is a valid group ..
                        Group test = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == followId);
                        // just update the follow refrence ...
                        if (test != null)
                        {
                            follow.DestUserSettingKey = followId;
                        }
                    }
                    else
                    {
                        // make sure it is a valid group ..
                        Group test = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.Id == followId);
                        // just update the follow refrence ...
                        if (test != null)
                        {
                            UserFriends followGroup = new UserFriends()
                            {
                                SrcUserSettingKey = group.Id,
                                DestUserSettingKey = test.Id,
                                Relationship = (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW
                            };
                            entities.AddToUserFriends(followGroup);
                        }                        
                    }
                    

                    #region WebLinks settings
                    UserSettingsExtended[] webLinks = group.UserSettingsExtendeds.Where(s => s.Class == 1).ToArray();
                    if (atiWebLinks.Visible)
                    {
                        // Facebook
                        UserSettingsExtended facebook = webLinks.FirstOrDefault(s => s.Name == "Facebook");
                        if (!string.IsNullOrEmpty(atiWebLinks.Facebook))
                        {
                            if (facebook == null)
                            {
                                facebook = new UserSettingsExtended()
                                {
                                    Class = 1,
                                    Name = "Facebook",
                                    Value = atiWebLinks.Facebook
                                };
                                group.UserSettingsExtendeds.Add(facebook);
                            }
                            else
                            {
                                facebook.Value = atiWebLinks.Facebook;
                            }
                        }
                        else if (facebook != null)
                        {
                            entities.DeleteObject(facebook);
                        }

                        // Twitter
                        UserSettingsExtended twitter = webLinks.FirstOrDefault(s => s.Name == "Twitter");
                        if (!string.IsNullOrEmpty(atiWebLinks.Twitter))
                        {
                            if (twitter == null)
                            {
                                twitter = new UserSettingsExtended()
                                {
                                    Class = 1,
                                    Name = "Twitter",
                                    Value = atiWebLinks.Twitter
                                };
                                group.UserSettingsExtendeds.Add(twitter);
                            }
                            else
                            {
                                twitter.Value = atiWebLinks.Twitter;
                            }
                        }
                        else if (twitter != null)
                        {
                            entities.DeleteObject(twitter);
                        }

                        // YouTube
                        UserSettingsExtended youtube = webLinks.FirstOrDefault(s => s.Name == "YouTube");
                        if (!string.IsNullOrEmpty(atiWebLinks.YouTube))
                        {

                            if (youtube == null)
                            {
                                youtube = new UserSettingsExtended()
                                {
                                    Class = 1,
                                    Name = "YouTube",
                                    Value = atiWebLinks.YouTube
                                };
                                group.UserSettingsExtendeds.Add(youtube);
                            }
                            else
                            {
                                youtube.Value = atiWebLinks.YouTube;
                            }
                        }
                        else if (youtube != null)
                        {
                            entities.DeleteObject(youtube);
                        }

                        // LinkedIn
                        UserSettingsExtended linkedin = webLinks.FirstOrDefault(s => s.Name == "LinkedIn");
                        if (!string.IsNullOrEmpty(atiWebLinks.LinkedIn))
                        {
                            if (linkedin == null)
                            {
                                linkedin = new UserSettingsExtended()
                                {
                                    Class = 1,
                                    Name = "LinkedIn",
                                    Value = atiWebLinks.LinkedIn
                                };
                                group.UserSettingsExtendeds.Add(linkedin);
                            }
                            else
                            {
                                linkedin.Value = atiWebLinks.LinkedIn;
                            }
                        }
                        else if (linkedin != null)
                        {
                            entities.DeleteObject(linkedin);
                        }

                        // Flickr
                        UserSettingsExtended flickr = webLinks.FirstOrDefault(s => s.Name == "Flickr");
                        if (!string.IsNullOrEmpty(atiWebLinks.Flickr))
                        {
                            if (flickr == null)
                            {
                                flickr = new UserSettingsExtended()
                                {
                                    Class = 1,
                                    Name = "Flickr",
                                    Value = atiWebLinks.Flickr
                                };
                                group.UserSettingsExtendeds.Add(flickr);
                            }
                            else
                            {
                                flickr.Value = atiWebLinks.Flickr;
                            }
                        }
                        else if (flickr != null)
                        {
                            entities.DeleteObject(flickr);
                        }

                        // Personal
                        UserSettingsExtended personal = webLinks.FirstOrDefault(s => s.Name == "Personal");
                        if (!string.IsNullOrEmpty(atiWebLinks.Peronsal))
                        {
                            if (personal == null)
                            {
                                personal = new UserSettingsExtended()
                                {
                                    Class = 1,
                                    Name = "Personal",
                                    Value = atiWebLinks.Peronsal
                                };
                                group.UserSettingsExtendeds.Add(personal);
                            }
                            else
                            {
                                personal.Value = atiWebLinks.Peronsal;
                            }
                        }
                        else if (personal != null)
                        {
                            entities.DeleteObject(personal);
                        }

                    }
                    #endregion

                    UploadThemeBackground();
                    group.CssStyle = string.Empty;

                    if( !atiThemeEditor.BackgroundColor.IsEmpty )
                    {
                        group.CssStyle += "background-color: #" + atiThemeEditor.BackgroundColor.Name.Substring(2)+";";
                    }
                    if (atiThemeEditor.IsTiled)
                    {
                        group.CssStyle += "background-repeat: repeat;";
                    }else{
                        group.CssStyle += "background-repeat:no-repeat; background-attachment:fixed;";
                    }
                    this.BackgroundImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + group.UserKey + "&p=" + group.PortalKey + "&bg=1";
                    this.ProfileCSS = group.CssStyle;

                    entities.SaveChanges();


                    // Next the member list
                    Affine.WebService.StreamService ss = new WebService.StreamService();
                    string json = ss.getMemberListDataOfRelationship(GroupSettings.Id, (int)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN, 0, 10);
                    string json2 = ss.getMemberListDataOfRelationship(GroupSettings.Id, (int)Affine.Utils.ConstsUtil.Relationships.GROUP_MEMBER, 0, 25);
                    RadAjaxManager1.ResponseScripts.Add(" $(function(){ Aqufit.Page.atiMemberListAdmin.generateStreamDom('" + json + "');  Aqufit.Page.atiMemberList.generateStreamDom('" + json2 + "');  }); ");
                }
                catch (Exception ex)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\""+ex.Message.Replace("'","")+"\"}');");
                }

            }
            else
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"\"}');");
            }
        }


        private bool RegisterGroup()
        {
            if (Page.IsValid)
            {
                // check required fields  
                aqufitEntities entities = new aqufitEntities();
                Group test = entities.UserSettings.OfType<Group>().FirstOrDefault(g => g.UserName == atiGroupName.Text);
                // if a user is found with that username, error. this prevents you from adding a username with the same name as a superuser.
                if (test != null)
                {
                    RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"We already have an entry for the Group Name.\"}');");
                    return false;
                }
                Affine.WebService.RegisterService regService = new WebService.RegisterService();
                
                string fname = string.Empty;
                string lname = string.Empty;
                string pass = Guid.NewGuid().ToString();

                Group us = new Data.Group();
                us.UserKey = 0;
                us.PortalKey = PortalId;
                us.UserEmail = atiTxtGroupEmail.Text;
                us.UserName = atiGroupName.Text;
                us.UserFirstName = atiGroupName.Text;
                us.UserLastName = "";
                us.DefaultMapLat = atiAddress.Lat;
                us.DefaultMapLng = atiAddress.Lng;
                us.LngHome = atiAddress.Lat;
                us.LngHome = atiAddress.Lng;
                int gtype = Convert.ToInt32(ddlGroupType.SelectedValue);
                us.GroupType = entities.GroupTypes.FirstOrDefault(gt => gt.Id == gtype);
                    
                us.Places.Add(new Place()
                {
                    City = atiAddress.City,
                    Country = atiAddress.Country,
                    Email = atiTxtGroupEmail.Text,
                    Lat = atiAddress.Lat,
                    Lng = atiAddress.Lng,
                    Name = atiGroupName.Text,
                    Postal = atiAddress.Postal,
                    Region = atiAddress.Region,
                    Street = atiAddress.Street
                });
                entities.AddToUserSettings(us);
                entities.SaveChanges();
                // Now assign the creator of the group as an admin
                Group group = entities.UserSettings.OfType<Group>().First(g => g.UserName == us.UserName);
                UserFriends uToG = new UserFriends()
                {
                    SrcUserSettingKey = UserSettings.Id,
                    DestUserSettingKey = group.Id,
                    Relationship = (short)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER
                };
                entities.AddToUserFriends(uToG);
                entities.SaveChanges();
                Response.Redirect(ResolveUrl("~") + "group/" + group.UserName + "/settings", true);
            }
            else
            {
                RadAjaxManager1.ResponseScripts.Add("Aqufit.Windows.ErrorDialog.open('{\"html\":\"\"}');");
            }
            return true;
        }      


        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Registers the module actions required for interfacing with the portal framework
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), ModuleActionType.AddContent, "", "", this.EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }
}

