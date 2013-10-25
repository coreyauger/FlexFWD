using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing.Imaging;
using Affine.Data;

public partial class services_images_profile : System.Web.UI.Page
{
    private void SendProfileImageResponse(UserSettings settings, bool large)
    {
        if (large)
        {
            if (settings != null && settings.Image != null)
            {
                Response.ContentType = settings.Image.ContentType;
                Response.OutputStream.Write(settings.Image.Bytes, 0, settings.Image.Bytes.Length);
                Response.Flush();
                Response.End();
            }
            else if (settings != null)
            {
                Response.ContentType = "image/jpg";
                char sex = string.IsNullOrWhiteSpace(settings.Sex) ? 'M' : settings.Sex[0];
                byte[] ret = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/profileLarge0" + sex + ".jpg"));
                Response.OutputStream.Write(ret, 0, ret.Length);
                Response.Flush();
                Response.End();
            }
        }
        else
        {
            if (settings != null && settings.Image1 != null)
            {
                Response.ContentType = settings.Image1.ContentType;
                Response.OutputStream.Write(settings.Image1.Bytes, 0, settings.Image1.Bytes.Length);
                Response.Flush();
                Response.End();
            }
            else if (settings != null)
            {
                Response.ContentType = "image/jpg";
                byte[] ret = null;
                if (string.IsNullOrEmpty(settings.Sex))
                {
                    ret = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/profileSmall0" + "M.jpg"));
                }
                else
                {
                    ret = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/profileSmall0" + settings.Sex[0] + ".jpg"));
                }

                Response.OutputStream.Write(ret, 0, ret.Length);
                Response.Flush();
                Response.End();
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        long pid = 0;
        try
        {
            if (Request["us"] != null)
            {
                long uid = Convert.ToInt64(Request["us"]);
                aqufitEntities entities = new aqufitEntities();
                UserSettings settings = entities.UserSettings.Include("BackgroundImage").Include("Image").Include("Image1").FirstOrDefault(us => us.Id == uid);
                if (Request["bg"] != null)
                {
                    if (settings != null && settings.BackgroundImage != null)
                    {
                        Response.ContentType = settings.BackgroundImage.ContentType;
                        Response.OutputStream.Write(settings.BackgroundImage.Bytes, 0, settings.BackgroundImage.Bytes.Length);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        Response.ContentType = "image/jpg";
                        byte[] ret = null;
                        ret = System.IO.File.ReadAllBytes(Server.MapPath("~/Portals/_default/Skins/Aqufit/standardBG.jpg"));
                        Response.OutputStream.Write(ret, 0, ret.Length);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    SendProfileImageResponse(settings, Request["f"] != null);
                }
            }
            else if (Request["u"] != null && Request["p"] != null)
            {
                long uid = Convert.ToInt64(Request["u"]);
                pid = Convert.ToInt64(Request["p"]);
                aqufitEntities entities = new aqufitEntities();
                if (Request["f"] != null)
                {
                    // TODO: cache
                    UserSettings settings = entities.UserSettings.Include("Image").FirstOrDefault(us => us.UserKey == uid && us.PortalKey == pid);
                    SendProfileImageResponse(settings,true);
                }
                else if (Request["bg"] != null)
                {
                    // TODO: cache
                    UserSettings settings = entities.UserSettings.Include("BackgroundImage").FirstOrDefault(us => us.UserKey == uid && us.PortalKey == pid);
                    if (settings != null && settings.BackgroundImage != null)
                    {
                        Response.ContentType = settings.BackgroundImage.ContentType;
                        Response.OutputStream.Write(settings.BackgroundImage.Bytes, 0, settings.BackgroundImage.Bytes.Length);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        Response.ContentType = "image/jpg";
                        byte[] ret = null;
                        ret = System.IO.File.ReadAllBytes(Server.MapPath("~/Portals/_default/Skins/Aqufit/standardBG.jpg"));
                        Response.OutputStream.Write(ret, 0, ret.Length);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    // TODO: cache
                    UserSettings settings = entities.UserSettings.Include("Image1").FirstOrDefault(us => us.UserKey == uid && us.PortalKey == pid);
                    SendProfileImageResponse(settings, false);
                }
            }
            
        }
        catch (Exception)
        {
            // fall through            
        }
        Response.ContentType = "image/jpg";
        byte[] ret2 = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/profileSmall" + pid + "M.jpg"));
        Response.OutputStream.Write(ret2, 0, ret2.Length);
        Response.Flush();
        Response.End();
    }
}