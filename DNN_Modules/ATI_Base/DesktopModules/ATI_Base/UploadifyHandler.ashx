<%@ WebHandler Language="C#" Class="UploadifyHandler" %>

using System;
using System.Web;
using System.IO;

public class UploadifyHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            HttpPostedFile file = context.Request.Files["Filedata"];
           
            // TODO: security + make sure the file type is correct

            long uid = Convert.ToInt64(context.Request["uid"]);
            long pid = Convert.ToInt64(context.Request["pid"]);
            string type = context.Request["t"];

            BinaryReader reader = new BinaryReader(file.InputStream);
            byte[] binData = reader.ReadBytes((int)file.InputStream.Length);
            MemoryStream ms = new MemoryStream(binData);
            if (type == "profile")
            {
          //      Affine.Utils.ImageUtil.MakeImageProfilePic(ms, uid, pid);
            }
            else if( type == "recipe" )
            {
                long rid = Convert.ToInt64(context.Request["rid"]);               
                Affine.Utils.ImageUtil.MakeRecipeImages(ms, uid, pid, rid);
                // TODO: small and reg size of the image.
            }
            context.Response.Write("1");
            context.Response.Flush();
            context.Response.End();
        }
        catch (Exception ex)
        {            
        }
        context.Response.Write("0");
        context.Response.Flush();
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}