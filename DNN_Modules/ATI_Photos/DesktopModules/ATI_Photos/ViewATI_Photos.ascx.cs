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
using System.Xml.Linq;
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

using Affine.Data;

using Telerik.Web.UI;


namespace Affine.Dnn.Modules.ATI_Photos
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
    partial class ViewATI_Photos : Affine.Dnn.Modules.ATI_PermissionPageBase , IActionable
    {

        #region Private Members              
               
        protected string StartupFunction{ get; set;} 
        #endregion       

        #region Public Methods    
        public string PageUrl { get; set; }                

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
            base.Page_Load(sender, e);
            
            try
            {
                // ***************
                // TODO: security ... make sure firends ... ect ...

                ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                service.InlineScript = true;
                ScriptManager.GetCurrent(Page).Services.Add(service);
                imgAd.Src = ResolveUrl("~/images/iphoneAd.png");
                
                if (!Page.IsPostBack && !Page.IsCallback)
                {

                    if (UserSettings == null)
                    {   // Users must be signed in to view photos
                        panelAll.Visible = false;
                        string retUrl = ResolveUrl("~") + ProfileSettings.UserName + "/photos";
                        if (Request["p"] != null)
                        {
                            retUrl += "?p=" + Request["p"];
                        }
                        RadAjaxManager1.ResponseScripts.Add(" top.location.href='" + ResolveUrl("~/Login") + "?returnurl=" + Server.UrlEncode(retUrl) + "'; ");
                        Response.End();
                        return;
                    }

                    if (Settings["Configure"] != null && string.Compare(Settings["Configure"].ToString(), "ConfigurePhotoView", true) == 0)
                    {
                        atiPhotosTitle.InnerText = "Photos";
                        imgAd.Visible = false;
                        this.PageUrl = ResolveUrl("~") + ProfileSettings.UserName + "/ViewPhoto";
                        atiWorkoutAlbum.Visible = false;
                    }
                    else
                    {
                        if (ProfileSettings == null && UserSettings != null)
                        {
                            Response.Redirect(ResolveUrl("~") + UserSettings.UserName + "/photos", true);
                            return;
                        }
                        atiPhotosTitle.InnerText = "Photos of " + ProfileSettings.UserName;
                        this.PageUrl = ResolveUrl("~") + ProfileSettings.UserName + "/photos";
                    }
                    photoTabTitle.Text = "Photos of " + ProfileSettings.UserName;



                    hlBackAlbums.HRef = "javascript: top.location.href='" + ResolveUrl("~") + ProfileSettings.UserName + "/photos" + "';";
                    atiWorkoutAlbum.Visible = base.Permissions == AqufitPermission.OWNER;
                    long pId = 0;
                    aqufitEntities entities = new aqufitEntities();
                    if (HttpContext.Current.Items["p"] != null)
                    {
                        pId = Convert.ToInt64(HttpContext.Current.Items["p"]);
                    }
                    else if (Request["p"] != null)
                    {
                        pId = Convert.ToInt64(Request["p"]);
                    }
                    if (Request["a"] != null)
                    {
                        
                        RadListViewTags.Visible = false;
                        hiddenAlbumKey.Value = Request["a"];
                        long aId = Convert.ToInt64(hiddenAlbumKey.Value);
                        Album album = entities.Albums.Include("UserSetting").FirstOrDefault(a => a.Id == aId);
                        bDeleteAlbum.Visible = (album.UserSetting.Id == UserSettings.Id && album.AlbumType == (int)Affine.Utils.ImageUtil.AlbumType.USER);
                        int total = entities.UserAttachments.OfType<Photo>().Where(p => p.Album.Id == album.Id).Count();
                        atiPhotosTitle.InnerText = "Photos: " + total + " photos in album (" + album.Name + ")";
                        litPhotoNav.Text = "<a href=\"" + ResolveUrl("~") + ProfileSettings.UserName + "/photos\">Back to photos</a>";
                        litAlbumName.Text = "<a style=\"border-bottom: 0;\" href=\"?a=" + album.Id + "\">" + album.Name + "</a>";
                        atiAlbumList.Visible = false;
                    }
                    else
                    {
                        RadListViewAlbumPhotos.Visible = false;
                    }
                    // Are we viewing a specific photo
                    if (pId > 0)
                    {
                        // TODO: security ... make sure firends ... ect ...
                        imgPhoto.Src = SetupPhoto(pId);
                        imgThisUser.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + UserSettings.UserKey + "&p=" + UserSettings.PortalKey;
                        long[] friendIds = entities.UserFriends.Where(f => (f.SrcUserSettingKey == UserSettings.Id || f.DestUserSettingKey == UserSettings.Id) && f.Relationship != (int)Affine.Utils.ConstsUtil.Relationships.FOLLOW).Select(f => (f.SrcUserSettingKey == UserSettings.Id ? f.DestUserSettingKey : f.SrcUserSettingKey)).ToArray();
                        IQueryable<User> userQuery = entities.UserSettings.OfType<User>().Where(Affine.Utils.Linq.LinqUtils.BuildContainsExpression<User, long>(s => s.Id, friendIds));
                        userQuery = userQuery.Concat(entities.UserSettings.OfType<User>().Where(us => us.Id == UserSettings.Id));
                        atiPhotoTagger.DataSource = userQuery.OrderBy(us => us.UserName).Select( us => new{UserName = us.UserName + " ("+us.UserFirstName + " " + us.UserLastName +")", Id = us.Id  });
                    }
                    else
                    {
                        panelPhotoList.Visible = true;
                        panelPhoto.Visible = false;
                        hiddenUserSettingsKey.Value = "" + ProfileSettings.Id;                       
                    }              
                }                           
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }            
        }

        private string SetupPhoto(long pId)
        {
            string ret = string.Empty;
            aqufitEntities entities = new aqufitEntities();
            hiddenNextPhotoId.Value = "-1";
            hiddenPrevPhotoId.Value = "-1";
            panelPhotoList.Visible = false;
            panelPhoto.Visible = true;
            Photo[] photos = null;
            Photo photoPrev = null;
            if (hiddenAlbumKey.Value == string.Empty)
            {
                // lets first check that the photo is not in any album?
                Album album = entities.UserAttachments.OfType<Photo>().Include("Album").FirstOrDefault(p => p.Id == pId).Album;
                if (album != null)
                {
                    hiddenAlbumKey.Value = "" + album.Id;
                }                
            }
            if (hiddenAlbumKey.Value != string.Empty)
            {
                long aId = Convert.ToInt64(hiddenAlbumKey.Value);            
                IQueryable<Photo> photoQuery = entities.UserAttachments.OfType<Photo>().Include("UserSetting").OrderBy(p => p.Id).Where(p => p.Album.Id == aId);
                int total = photoQuery.Count();
                litPhotoCount.Text = "" + total;                
                photoQuery = photoQuery.Where(p => p.Id >= pId);
                total++;    // avoid saying 0 of 4 ....
                lPhotoNum.Text = "" + (total - photoQuery.Count());
                photos = photoQuery.Take(2).ToArray();
                photoPrev = entities.UserAttachments.OfType<Photo>().Include("UserSetting").OrderByDescending(p => p.Id).FirstOrDefault(p => p.Id < pId && p.Album.Id == aId);
            }
            else
            {
                IQueryable<Photo> photoQuery = entities.User2Photo.OrderBy(p => p.Photo.Id).Where(p => p.UserSettingsKey == ProfileSettings.Id).Select(p => p.Photo).OfType<Photo>();
                int total = photoQuery.Count();
                litPhotoCount.Text = "" + photoQuery.Count();
                photoQuery = photoQuery.Where(p => p.Id >= pId);
                total++;    // avoid saying (0 of 4) ....
                lPhotoNum.Text = "" + (total - photoQuery.Count());
                photos = photoQuery.Take(2).ToArray();
                User2Photo u2p = entities.User2Photo.Include("Photo").OrderByDescending(p => p.Photo.Id).FirstOrDefault(p => p.Photo.Id < pId);
                if (u2p != null)
                {
                    photoPrev = (Photo)u2p.Photo;
                }
            }
            if (photos.Length > 0)
            {
                if (photos[0].UserSetting.Id == UserSettings.Id)
                {
                    liDeletePhoto.Visible = true;
                }

                ret = photos[0].ImageUri;
                hiddenPhotoId.Value = "" + photos[0].Id;       
                // setup tags...
                pId = photos[0].Id; // yes this is needed 
                User2Photo[] photoTags = entities.User2Photo.Where(p => p.Photo.Id == pId).ToArray();
                string js = string.Empty;
                js += "Aqufit.Page.atiPhotoTagger.clearTags();";
                foreach (User2Photo u2p in photoTags)
                {
                    User person = entities.UserSettings.OfType<User>().FirstOrDefault( u => u.Id == u2p.UserSettingsKey );
                    js += "Aqufit.Page.atiPhotoTagger.addTagElement("+person.Id+ "," + u2p.Id + ",'"+person.UserName+" ("+person.UserFirstName+" "+person.UserLastName+")'," + u2p.Top + ", " + u2p.Left + ", " + u2p.Width + ", " + u2p.Height+ ");";
                }
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    this.StartupFunction = "Aqufit.Page.Actions.onLoadHelper = function(){ " + js + " };";
                }
                else
                {
                    RadAjaxManager1.ResponseScripts.Add("$(function(){ " + js + " });");
                }
            }
            if (photos.Length > 1)
            {
                hiddenNextPhotoId.Value = "" + photos[1].Id;
            }
            
            if (photoPrev != null)
            {
                hiddenPrevPhotoId.Value = "" + photoPrev.Id;
            }
            return ret;
        }

        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            try
            {
                aqufitEntities entities = new aqufitEntities();
                Affine.Data.Managers.IDataManager dataMan = Affine.Data.Managers.LINQ.DataManager.Instance;
                long pId = -1;
                switch (hiddenAjaxAction.Value)
                {
                    case "loadPhoto":
                        pId = Convert.ToInt64(hiddenAjaxValue.Value);
                        string url = SetupPhoto(pId);
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.LoadImage('" + url + "', " + lPhotoNum.Text + ");");                                
                        break;   
                    case "makeProfile":
                        pId = Convert.ToInt64(hiddenAjaxValue.Value);
                        Affine.Utils.ImageUtil.MakeProfileFromPhoto(UserSettings.Id, pId, Server.MapPath("~"));
                        string profileUrl = ResolveUrl("~") + UserSettings.UserName;
                        RadAjaxManager1.ResponseScripts.Add("top.location.href='" + profileUrl + "'; Aqufit.Page.atiLoading.remove();");  
                        break;
                    case "tagPhoto":
                        Affine.Data.Helpers.PhotoTag json = JsonExtensionsWeb.FromJson<Affine.Data.Helpers.PhotoTag>(hiddenAjaxValue.Value);
                        pId = Convert.ToInt64(hiddenPhotoId.Value);
                        // TODO: security
                        dataMan.TagPhoto(UserSettings.Id, json.FriendId, pId, json.Top, json.Left, json.Width, json.Height);
                        RadAjaxManager1.ResponseScripts.Add("radalert('<div style=\"width: 100%; height: 100%; padding: 0px;\"><span style=\"color: #FFF;\"><img src=\""+ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png")+"\" align=\"absmiddle\"/> Photo has been tagged</span></div>', 330, 100, 'Success');");  
                        break;
                    case "addComment":
                        pId = Convert.ToInt64(hiddenPhotoId.Value);
                        string comment = hiddenAjaxValue.Value;
                        AttachmentComment pc = dataMan.SavePhotoComment(UserSettings.Id, pId, comment);
                        string j = new { UserName = pc.UserSetting.UserName, UserKey = pc.UserSetting.UserKey, Comment = pc.Comment }.ToJson();
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.appendComment('"+j+"'); ");  
                        break;
                    case "deletePhoto":
                        pId = Convert.ToInt64(hiddenPhotoId.Value);
                        dataMan.DeletePhoto(UserSettings.Id, pId);
                        Response.Redirect(ResolveUrl("~") + UserSettings.UserName +"/photos", true );
                        //RadAjaxManager1.ResponseScripts.Add("alert('"+pId+"');");
                        break;
                    case "deleteAlbum":
                        long aId = Convert.ToInt64(hiddenAlbumKey.Value);
                        dataMan.DeleteAlbum(UserSettings.Id, aId);
                        Response.Redirect(ResolveUrl("~") + UserSettings.UserName + "/photos", true);
                        //RadAjaxManager1.ResponseScripts.Add("alert('"+pId+"');");
                        break;
                    case "deleteTag":
                        long tId = Convert.ToInt64(hiddenAjaxValue.Value);
                        User2Photo u2p = entities.User2Photo.FirstOrDefault( p => p.Id == tId && (p.UserSettingsKey == UserSettings.Id || p.TaggerUserSettingsKey == UserSettings.Id ));
                        if (u2p != null)
                        {
                            entities.DeleteObject(u2p);
                            entities.SaveChanges();
                            RadAjaxManager1.ResponseScripts.Add("radalert('<div style=\"width: 100%; height: 100%; padding: 0px;\"><span style=\"color: #FFF;\"><img src=\"" + ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png") + "\" align=\"absmiddle\"/> Tag has been removed</span></div>', 330, 100, 'Success');"); 
                        }
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.atiLoading.remove(); ");
                        break;
                }
            }
            catch (Exception ex)
            {
                // TODO: better error handling
                RadAjaxManager1.ResponseScripts.Add( " alert('" + ex.Message + "'); ");
            }
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

