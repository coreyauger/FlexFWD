using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Affine.Data;

public partial class DesktopModules_ATI_Base_controls_ATI_FeaturedProfile : DotNetNuke.Framework.UserControlBase
{
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
    public bool Small
    {
        get
        {
            if (ViewState["Small"] != null)
            {
                return (bool)ViewState["Small"];
            }
            return false;
        }
        set
        {
            ViewState["Small"] = value;
        }
    }
  
    public UserSettings Settings
    {
        get;
        set;
    }

    public bool ShowNumFollowers { get; set; }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            panelProfileImageSmall.Visible = panelProfileStats.Visible = this.Small;
            panelProfileImageLarge.Visible = !this.Small;
            if (this.Settings != null)
            {
                Metric nr = this.Settings.Metrics.FirstOrDefault( m => m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_RECIPES );
                int numRecipies = nr == null ? 0 : Convert.ToInt32(nr.MetricValue);
                Metric nf = this.Settings.Metrics.FirstOrDefault(m => m.MetricType == (int)Affine.Utils.MetricUtil.MetricType.NUM_FOLLOWERS);
                int numFollowers = nf == null ? 0 : Convert.ToInt32(nf.MetricValue);

                imgProfileLarge.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?us=" + Settings.Id + (this.Small ? "" : "&f=1") );
                imgProfileSmall.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx?us=" + Settings.Id + (this.Small ? "" : "&f=1"));
                litProfileInfo.Text = "<a class=\"uname\" href=\"" + ResolveUrl("~/") + this.Settings.UserName + "\">" + this.Settings.UserName + "</a>";
                hrefProfile.HRef = ResolveUrl("~/") + this.Settings.UserName;
                // TODO: fix the style so it works in all browsers
              
                if(this.ShowNumFollowers){
                    litProfileInfo.Text += "<br /><span class=\"stat\">Num Recipies: <em>" + numRecipies + "</em><br />Followers: <em>" + numFollowers + "</em></span>";
                }
                //    litProfileInfo.Text += "<ul class=\"stat\">";
                //    litProfileInfo.Text += "<li>Num Recipies: <em>" + numRecipies + "</em></li>";
                //    litProfileInfo.Text += "<li>Followers: <em>" + numFollowers + "</em></li>";
                //    litProfileInfo.Text += "</ul>";
                
                
                if( this.Settings.UserStreams != null && this.Settings.UserStreams.Count > 0 ){
                    DisplayStreamFeature(this.Settings.UserStreams.First());                   
                }
            }
            else
            {
                imgProfileLarge.Src = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx");
            }
        }
    }


    private void DisplayStreamFeature(UserStream stream)
    {
        if (stream is Recipe)
        {
            Recipe r = (Recipe)stream;
            string urlTitle = r.Title.Replace(" ", "-");

            string aRecLink = "<a href=\"" + ResolveUrl("~/") + "recipe/" + r.Id + "/" + urlTitle  + "\" >";
            string buff = aRecLink  + r.Title + "</a>";
            buff += "<ul>";
            buff += "<li style=\"padding-bottom: 14px;\"><span style=\"float: left;\"><input type=\"radio\" class=\"rate\" value=\"0.5\"/><input type=\"radio\" class=\"rate\" value=\"1.0\"/><input type=\"radio\" class=\"rate\" value=\"1.5\"/><input type=\"radio\" class=\"rate\" value=\"2.0\"/><input type=\"radio\" class=\"rate\" value=\"2.5\"/><input type=\"radio\" class=\"rate\" value=\"3.0\"/><input type=\"radio\" class=\"rate\" value=\"3.5\"/><input type=\"radio\" class=\"rate\" value=\"4.0\"/><input type=\"radio\" class=\"rate\" value=\"4.5\"/><input type=\"radio\" class=\"rate\" value=\"5.0\"/>&nbsp;&nbsp;Rating</span></li>";
            buff += "<li><span style=\"float: left;\"><input type=\"radio\" class=\"strict\" value=\"1\"/><input type=\"radio\" class=\"strict\" value=\"2\"/><input type=\"radio\" class=\"strict\" value=\"3\"/>&nbsp;&nbsp;Paleo&nbsp;Strict</span></li>";
            buff += "</ul>";
            buff += "<br />";
            buff += "<div style=\"text-align: center; padding-top: 5px;\">" + aRecLink + "<img src=\"" + ResolveUrl("~/DesktopModules/ATI_Base/services/images/recipe.aspx") + "?r=" + r.Id + "\" /><br />more ...</a></div>";
            buff += "<span>" +  r.Description +"</span>";

            buff += "<script type=\"text/javascript\">";
            buff += "$(function () {$('input.rate').each(function() {if ($(this).val() == " + r.AvrRating + " ) {$(this).attr('checked', 'true');}}).rating({split: 2,readOnly: true});";
            buff += "$('input.strict').each(function() {if ($(this).val() == " + r.AvrStrictness + ") {$(this).attr('checked', 'true');}}).rating({readOnly: true});});"; 
            buff += "</script>";
            litStats.Text = buff;


            /*
             * <li id="atiRecipeRating"></li>
            <li>
                <input type="radio" class="rate" value="0.5"/>
                <input type="radio" class="rate" value="1"/>
                <input type="radio" class="rate" value="1.5"/>
                <input type="radio" class="rate" value="2"/>   
                <input type="radio" class="rate" value="2.5"/>                         
                <input type="radio" class="rate" value="3"/>
                <input type="radio" class="rate" value="3.5"/>                                
                <input type="radio" class="rate" value="4" />
                <input type="radio" class="rate" value="4.5"/>   
                <input type="radio" class="rate" value="5"/> 
            </li>
            <li id="atiRecipeStrict" style="float: right">&nbsp;&nbsp;Paleo&nbsp;Strict
                 <input type="radio" class="strict" value="1"/>
                 <input type="radio" class="strict" value="2" checked="checked"/>
                 <input type="radio" class="strict" value="3" />  
            </li>
             */

        }
    }
}
