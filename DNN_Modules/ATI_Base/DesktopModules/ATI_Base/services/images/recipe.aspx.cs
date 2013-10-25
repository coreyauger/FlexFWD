using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing.Imaging;
using Affine.Data;

public partial class services_images_recipe : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request["r"] != null )
            {
                long rid = Convert.ToInt64(Request["r"]);               
                aqufitEntities entities = new aqufitEntities();
                RecipeExtended re = entities.RecipeExtendeds.Include("Image").FirstOrDefault(r => r.UserStream.Id == rid);
                if (Request["f"] != null)
                {
                    // TODO: cache                                        
                    if (re.Image != null )
                    {
                        Affine.Data.Image image = entities.Image.FirstOrDefault(i => i.Id == re.Image.ImageLargeKey);
                        Response.ContentType = re.Image.ContentType;
                        if (image != null)
                        {
                            Response.OutputStream.Write(image.Bytes, 0, image.Bytes.Length);
                        }
                        else
                        {
                            Response.OutputStream.Write(re.Image.Bytes, 0, re.Image.Bytes.Length);
                        }
                    }
                    else
                    {
                        Response.ContentType = "image/jpg";
                        byte[] ret = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/meal.jpg"));
                        Response.OutputStream.Write(ret, 0, ret.Length);
                    }
                    Response.Flush();
                    Response.End();
                    return;
                }
                else
                {
                    // TODO: cache                                        
                    if (re.Image != null)
                    {
                        Response.ContentType = re.Image.ContentType;
                        Response.OutputStream.Write(re.Image.Bytes, 0, re.Image.Bytes.Length);
                    }
                    else
                    {
                        Response.ContentType = "image/jpg";
                        byte[] ret = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/meal.jpg"));
                        Response.OutputStream.Write(ret, 0, ret.Length);
                    }
                    Response.Flush();
                    Response.End();
                    return;
                }                
            }
        }
        catch (Exception)
        {
            // fall through
        }
        Response.ContentType = "image/jpg";
        byte[] ret2 = System.IO.File.ReadAllBytes(Server.MapPath("~/DesktopModules/ATI_Base/resources/images/meal.jpg"));
        Response.OutputStream.Write(ret2, 0, ret2.Length);
        Response.Flush();
        Response.End();
    }
}