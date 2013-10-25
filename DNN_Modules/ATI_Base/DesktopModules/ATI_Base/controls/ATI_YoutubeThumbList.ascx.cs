using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;




public partial class DesktopModules_ATI_Base_controls_ATI_YoutubeThumbList : DotNetNuke.Framework.UserControlBase
{
    public class YoutubeThumbItem{
        public string Title{get; set;}
        public string Url{ get; set; }
        public string Type { get; set; }
        public string ImageUrl{ get; set; }
        public string Duration { get; set; }
        public string VideoId { get; set; }
    };

    public Unit Width
    {
        get
        {
            if (ViewState["Width"] != null)
            {
                return (Unit)ViewState["Width"];
            }
            return Unit.Pixel(0);
        }
        set
        {
            ViewState["Width"] = value;
        }
    }
    public Unit Height
    {
        get
        {
            if (ViewState["Height"] != null)
            {
                return (Unit)ViewState["Height"];
            }
            return Unit.Pixel(0);
        }
        set
        {
            ViewState["Height"] = value;
        }
    }


    public enum Orientation { HORIZONTAL, VERTICAL };
    public Orientation Layout { get; set; }


    public IList<Video> VideoFeed { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && !Page.IsCallback)
        {
            if (this.Layout == Orientation.HORIZONTAL)
            {
                //mediaListWrap.Attributes["class"] = "hlist";
                if (this.VideoFeed != null)
                {
                    IList<YoutubeThumbItem> data = new List<YoutubeThumbItem>();
                    foreach (Video entry in this.VideoFeed)
                    {
                        int second = Convert.ToInt32(entry.YouTubeEntry.Duration.Seconds);
                        string time = Affine.Utils.UnitsUtil.durationToTimeString(second * 1000);
                        if (entry.YouTubeEntry.Media.Contents.Count > 0)
                        {
                            data.Add(new YoutubeThumbItem() { Title = entry.Title, VideoId = entry.YouTubeEntry.VideoId, Url = entry.YouTubeEntry.Media.Contents[0].Url, Type = entry.YouTubeEntry.Media.Contents[0].Type, ImageUrl = entry.Thumbnails[0].Url, Duration = time });
                        }                                          
                    }

                    mediaList.DataSource = data;
                    mediaList.DataBind();
                }
            }
        }
    }
}
