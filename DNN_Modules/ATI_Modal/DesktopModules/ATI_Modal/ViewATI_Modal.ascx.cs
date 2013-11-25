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


using Telerik.Web.UI;
using Telerik.Web.UI.Upload;


using Affine.Data;

namespace Affine.Dnn.Modules.ATI_Modal
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
    partial class ViewATI_Modal : ATI_PermissionPageBase, IActionable
    {

        #region Private Members
        private string serverRoot;
        private string userPhotoPath;
        private string urlPath;

       
        #endregion       

        #region Public Methods
        Album _album = null;
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
                serverRoot = Server.MapPath("~");
                userPhotoPath = serverRoot + @"\Portals\0\Users\" + base.UserSettings.UserName;
                urlPath = "/Portals/0/Users/" + base.UserSettings.UserName;                
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    h3UploadTitle.InnerHtml = "Upload a new profile picture.";
                    hlAlbum.HRef = "javascript: top.location.href = '"+ResolveUrl("~") + UserSettings.UserName+"/photos';";
                    AsyncUpload1.AllowedMimeTypes = new string[] { "image/jpeg", "image/png", "image/jpg", "image/gif" };
                    AsyncUpload1.RegisterWithScriptManager = true;
                    AsyncUpload1.TemporaryFolder = Server.MapPath(ResolveUrl("~/Portals/0/Temp"));            
                    if (Request["a"] != null)
                    {
                        atiUploadAlbumPhotos.Visible = true;
                        atiUploadProfileImgPanel.Visible = false;
                    }
                    if (Request["sap"] != null) // this is a stream attachment "photo"
                    {
                        panelAlbumChoose.Visible = false;
                        h3UploadTitle.InnerHtml = "Upload a new image to share.";
                    }
                    
                }               
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        protected void bAsyncUpload_Click(object sender, EventArgs e)
        {
            // New albums are a result of ?a=0
            try
            {
                // First thing we do is check if we need to create or retrieve and album
                if (_album == null && Request["a"] != null)
                {

                    long aId = Convert.ToInt64(Request["a"]);
                    aqufitEntities entities = new aqufitEntities();
                    if (aId == 0)   // this is a new album
                    {
                        Album album = new Album()
                        {
                            Name = txtAlbumName.Text,
                            CreationDate = DateTime.Now.ToUniversalTime(),
                            UserSetting = entities.UserSettings.FirstOrDefault(us => us.Id == UserSettings.Id)
                        };
                        entities.AddToAlbums(album);
                        entities.SaveChanges();
                        _album = entities.Albums.OrderByDescending(a => a.Id).FirstOrDefault(a => a.UserSetting.Id == UserSettings.Id);
                    }
                    else
                    {
                        _album = entities.Albums.OrderByDescending(a => a.Id).FirstOrDefault(a => a.Id == aId);
                    }
                }
                else if (Request["a"] == null)
                {
                    // should never see this ... sanity check..
                //    RadAjaxManager1.ResponseScripts.Add("alert('No Album specified for the photos'); ");
                  //  return;
                }
                int u = 0;
                foreach (UploadedFile file in AsyncUpload1.UploadedFiles)
                {
                    if (file.ContentLength < 10072000)
                    {
                        byte[] buffer = new byte[file.ContentLength];
                        using (Stream str = file.InputStream)
                        {
                            buffer = ReadFully(str);
                        }                        
                        MemoryStream ms = new MemoryStream(buffer);
                        Affine.Utils.ImageUtil.SavePhoto(Affine.Utils.ImageUtil.AlbumType.USER, ms, UserSettings.Id, _album.Id, userPhotoPath, urlPath, _album.Photo == null);
                        u++;
                    }
                }
                RadAjaxManager1.ResponseScripts.Add(" parent.Aqufit.Windows.UploadWin.close(); top.location.href='/" + UserSettings.UserName + "/photos?a=" + _album.Id + "'; ");
               // Response.Write("<h1>TEST " + u + "</h1>");
            }
            catch (Exception ex)
            {
             //   Response.Write("<h1>" + ex.Message + "</h1>");
            }
        }

        protected void AsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            try
            {
                // First thing we do is check if we need to create or retrieve and album
                if (_album == null && Request["a"] != null)
                {
                    
                    long aId = Convert.ToInt64(Request["a"]);
                    aqufitEntities entities = new aqufitEntities();
                    if (aId == 0)   // this is a new album
                    {
                        Album album = new Album()
                        {
                            Name = txtAlbumName.Text,
                            CreationDate = DateTime.Now.ToUniversalTime(),
                            UserSetting = entities.UserSettings.FirstOrDefault(us => us.Id == UserSettings.Id)
                        };
                        entities.AddToAlbums(album);
                        entities.SaveChanges();
                        _album = entities.Albums.OrderByDescending(a => a.Id).FirstOrDefault(a => a.UserSetting.Id == UserSettings.Id);
                    }
                    else
                    {
                        _album = entities.Albums.OrderByDescending(a => a.Id).FirstOrDefault(a => a.Id == aId);
                    }                    
                }
                else if (Request["a"] == null)
                {
                    // should never see this ... sanity check..
                    RadAjaxManager1.ResponseScripts.Add("alert('No Album specified for the photos'); ");
                    return;
                }


                if (e.File.ContentLength < 10072000)
                {
                    e.IsValid = true;
                    byte[] buffer = new byte[e.File.ContentLength];
                    using (Stream str = e.File.InputStream)
                    {                            
                        str.Read(buffer, 0, e.File.ContentLength);
                        MemoryStream ms = new MemoryStream(buffer);
                        Affine.Utils.ImageUtil.SavePhoto(Affine.Utils.ImageUtil.AlbumType.USER, ms, UserSettings.Id, _album.Id, userPhotoPath, urlPath, _album.Photo == null);
                    }                        
                }
                
                         
            }
            catch (Exception ex)
            {
                RadAjaxManager1.ResponseScripts.Add("alert('"+ex.Message+"'); ");
            }
        }
      

        protected void bUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                try
                {
                    long usId = this.UserSettings.Id;
                    if (GroupSettings != null)
                    {
                        aqufitEntities entities = new aqufitEntities();
                        UserFriends uf = entities.UserFriends.FirstOrDefault(f => f.SrcUserSettingKey == UserSettings.Id && f.DestUserSettingKey == GroupSettings.Id || f.SrcUserSettingKey == GroupSettings.Id && f.DestUserSettingKey == UserSettings.Id);
                        if (uf != null && (uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_OWNER || uf.Relationship == (short)Affine.Utils.ConstsUtil.Relationships.GROUP_ADMIN))
                        {   // Relation
                            usId = GroupSettings.Id;
                        }
                        else
                        {
                            lStatus.Text = "You must be a group admin to change the profile picture.";
                        }
                    }

                    if (fileUpload.PostedFile.ContentType == "image/jpeg" || fileUpload.PostedFile.ContentType == "image/png" || fileUpload.PostedFile.ContentType == "image/jpg" || fileUpload.PostedFile.ContentType == "image/gif" || fileUpload.PostedFile.ContentType == "image/pjpeg" || fileUpload.PostedFile.ContentType == "image/x-png")
                    {
                        if (fileUpload.PostedFile.ContentLength < 10072000)
                        {
                            MemoryStream ms = new MemoryStream(fileUpload.FileBytes);                            
                            Affine.Utils.ImageUtil.AlbumType type = Utils.ImageUtil.AlbumType.PROFILE;
                            bool cover = true;
                            if (Request["sap"] != null)
                            {
                                type = Utils.ImageUtil.AlbumType.STREAM;
                                cover = false;
                            }
                            else
                            {
                                Affine.Utils.ImageUtil.MakeImageProfilePic(ms, usId);
                            }
                            long pId = Affine.Utils.ImageUtil.SavePhoto(type, ms, usId, -1, userPhotoPath, urlPath, cover);
                            string control = Request["c"];  
                            aqufitEntities entities = new aqufitEntities();
                            Photo photo = entities.UserAttachments.OfType<Photo>().FirstOrDefault(p => p.Id == pId);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "atiProfileRefresh", " parent.Aqufit.Page." + control + ".ImageUploadSuccess(" + usId + "," + pId + ", '" + photo.ThumbUri + "');", true);
                        }
                        else
                        {
                            lStatus.Text = "Upload status: The file has to be less than 9 MB!";
                        }
                    }
                    else
                    {
                        lStatus.Text = "Upload status: Only JPEG, PNG, GIF files are accepted! (" + fileUpload.PostedFile.ContentType+")";
                    }
                }
                catch (Exception ex)
                {
                    lStatus.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
                
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

