using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing.Imaging;
using Affine.Data;

public partial class services_images_image : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request["i"] != null )
            {
                long iid = Convert.ToInt64(Request["i"]);               
                aqufitEntities entities = new aqufitEntities();
                Affine.Data.Image img = entities.Image.FirstOrDefault(i => i.Id == iid);
                if (Request["f"] != null)
                {                   
                    Affine.Data.Image image = entities.Image.FirstOrDefault(i => i.Id == img.ImageLargeKey);
                    Response.ContentType = image.ContentType;
                    Response.OutputStream.Write(image.Bytes, 0, image.Bytes.Length);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    Response.ContentType = img.ContentType;
                    Response.OutputStream.Write(img.Bytes, 0, img.Bytes.Length);
                    Response.Flush();
                    Response.End();
                }                
            }
        }
        catch (Exception)
        {
            // fall through
        }
        Response.Flush();
        Response.End();
    }
}