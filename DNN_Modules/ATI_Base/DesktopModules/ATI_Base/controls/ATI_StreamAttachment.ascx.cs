using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_StreamAttachment : DotNetNuke.Framework.UserControlBase
{

    public long PhotoKey
    {
        get { return (string.IsNullOrWhiteSpace(hiddenAttachmentPhotoKey.Value) ? 0 : Convert.ToInt64(hiddenAttachmentPhotoKey.Value)); }
        set { hiddenAttachmentPhotoKey.Value = "" + value; }
    }

    public Affine.Data.json.PageMetaData LinkJson
    {
        get {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            return (string.IsNullOrWhiteSpace(hiddenAttachmentLinkKey.Value) ? null : serialize.Deserialize<Affine.Data.json.PageMetaData>(hiddenAttachmentLinkKey.Value));
        }
        set {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            hiddenAttachmentLinkKey.Value =  serialize.Serialize( value ); 
        }
    }

    public long VideoKey
    {
        get { return (string.IsNullOrWhiteSpace(hiddenAttachmentVideoKey.Value) ? 0 : Convert.ToInt64(hiddenAttachmentVideoKey.Value)); }
        set { hiddenAttachmentVideoKey.Value = "" + value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            string baseUrl = ResolveUrl("~");
            imgPhoto.Src = baseUrl + "DesktopModules/ATI_Base/resources/images/iAddPhoto.png";
            imgLink.Src = baseUrl + "DesktopModules/ATI_Base/resources/images/iAddLink.png";
        }
    }
}
