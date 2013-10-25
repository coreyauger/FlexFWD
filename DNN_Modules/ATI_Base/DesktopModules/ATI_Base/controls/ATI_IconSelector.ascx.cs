using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_IconSelector : DotNetNuke.Framework.UserControlBase
{
    public string Title
    {
        get;
        set;
    }

    public string BaseImgUrl
    {
        get;
        set;
    }

    public string ImgArray
    {
        get;
        set;
    }

    public string StatusArray
    {
        get;
        set;
    }

    public string ValueArray
    {
        get;
        set;
    }

    public string Value
    {
        get { return hiddenValue.Value; }        
    }  
    
    protected void Page_Load(object sender, EventArgs e)
    {
        // get the img urls and the status strings
        string[] imgUrlArray = this.ImgArray.Split(',');
        string[] statusArray = this.StatusArray.Split(',');
        string[] valueArray = this.ValueArray.Split(',');
        if ( (imgUrlArray.Length != statusArray.Length) && (statusArray.Length != valueArray.Length) )
        {
            throw new ArgumentException("Number of img does not equal the number of status strings");
        }
        lTitle.Text = this.Title;
        // we need to construct all the icon controls every postback
        string baseUrl = ResolveUrl(this.BaseImgUrl);
        for (int i = 0; i < statusArray.Length; i++)
        {
            string imgId = imgUrlArray[i].Trim();
            string status = statusArray[i].Trim();
            string value = valueArray[i].Trim();
            System.Web.UI.HtmlControls.HtmlImage img = new System.Web.UI.HtmlControls.HtmlImage()
            {
                ID = imgId,
                Src = baseUrl + imgId + "_0.png"
            };
            img.Attributes["onclick"] = "Aqufit.Page." + this.ID + ".selectIcon('" + imgId + "','" + status + "', '" + value + "');";
            img.Attributes["onmouseover"] = "Aqufit.Page." + this.ID + ".switchIcon('" + imgId + "',1);";
            img.Attributes["onmouseout"] = "Aqufit.Page." + this.ID + ".switchIcon('" + imgId + "',0);";
            iconHolder.Controls.Add(img);

            System.Web.UI.HtmlControls.HtmlImage img2 = new System.Web.UI.HtmlControls.HtmlImage()
            {
                Src = baseUrl + imgId + "_1.png"
            };
            imgPreload.Controls.Add(img2);
        }        
        //System.Web.UI.HtmlControls.HtmlImage 
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            
        }
    }
    
}
