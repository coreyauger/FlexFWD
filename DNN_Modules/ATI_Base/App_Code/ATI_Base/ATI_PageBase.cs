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
using Affine.Data.EventArgs;
using Affine.Utils.Linq;

namespace Affine.Dnn.Modules
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
    public class ATI_PageBase : PortalModuleBase
    {

        #region Private Members
        #endregion

        #region Public Methods
        #endregion            
  
        protected Affine.Data.json.FaceBookUser fbUser { get; set; }

        protected bool _PerformFBLoginCheck = true;

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
                base.OnInit(e);
                if (_PerformFBLoginCheck)
                {
                    try
                    {                        
                        string sessionKey = FaceBookSessionKey();
                        if (!string.IsNullOrWhiteSpace(sessionKey) && this.UserId <= 0)
                        {   // we have a facebook session but the user is logged out of our site.
                            sessionKey = sessionKey.Substring(1, sessionKey.Length - 2);    // remove the " from front and back....
                            System.Collections.Specialized.NameValueCollection nvc = HttpUtility.ParseQueryString(sessionKey);
                            if (nvc.Get("uid") != null)
                            {
                                string access_token = nvc.Get("access_token");
                                WebRequest request = WebRequest.Create("https://graph.facebook.com/me?access_token=" + access_token);
                                WebResponse res = request.GetResponse();
                                System.IO.Stream ReceiveStream = res.GetResponseStream();

                                System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                                System.IO.StreamReader readStream = new System.IO.StreamReader(ReceiveStream, encode);
                                string json = readStream.ReadToEnd();
                                
                                System.Web.Script.Serialization.JavaScriptSerializer serial = new System.Web.Script.Serialization.JavaScriptSerializer();
                                fbUser = serial.Deserialize<Affine.Data.json.FaceBookUser>(json);

                                if (!string.IsNullOrWhiteSpace(fbUser.email))
                                {
                                    aqufitEntities entities = new aqufitEntities();
                                    User siteUser = null;
                                    siteUser = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.PortalKey == PortalId && u.FBUid == fbUser.id );
                                    if (siteUser == null)
                                    {
                                        siteUser = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.PortalKey == PortalId && u.UserEmail.ToLower() == fbUser.email);
                                    }
                                    if (siteUser != null)
                                    {   // we can log the user in 
                                        if (siteUser.FBUid == null)
                                        {
                                            siteUser.FBUid = fbUser.id;
                                            entities.SaveChanges();
                                        }
                                        else if (siteUser.FBUid.Value != fbUser.id)
                                        {   // something strange is going on here.. these should be equal
                                            throw new Exception("Facebook Login Exception.");
                                        }
                                        UserInfo uinfo = UserController.GetUser((int)siteUser.PortalKey, (int)siteUser.UserKey, true);
                                        UserController.UserLogin((int)siteUser.PortalKey, uinfo, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);
                                        string url = Request.RawUrl;
                                        if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
                                        {
                                            url = Request["returnurl"];
                                        }
                                        else if (url.Contains("/Login") || url.EndsWith("flexfwd.com/") || url.EndsWith("flexfwd.com/Home.aspx"))
                                        {
                                            url = ResolveUrl("~/") + siteUser.UserName;
                                        }
                                        Response.Redirect(url, true);
                                    }
                                    else if( Request["sl"] == null )    // sl (stop loop) is not defined..
                                    {
                                        // TODO: this should just bring them to the info that they still need to fill out.
                                        Response.Redirect(ResolveUrl("~/RPX?sl=0"), true);
                                    }
                                }
                            }
                        }
                        else if (Request.Cookies["FlexLogout"] != null)
                        {
                            Request.Cookies.Remove("FlexRM");
                            Request.Cookies.Remove("FlexLogout");
                        }
                        else if (Request.Cookies["FlexRM"] != null && this.UserId <= 0)
                        {
                            aqufitEntities entities = new aqufitEntities();
                            User siteUser = null;
                            Guid test = Guid.Parse(Request.Cookies["FlexRM"].Value);
                            siteUser = entities.UserSettings.OfType<User>().FirstOrDefault(u => u.PortalKey == PortalId && u.Guid == test);
                            if (siteUser != null)
                            {   // we can log the user in                                 
                                UserInfo uinfo = UserController.GetUser((int)siteUser.PortalKey, (int)siteUser.UserKey, true);
                                UserController.UserLogin((int)siteUser.PortalKey, uinfo, PortalSettings.PortalName, DotNetNuke.Services.Authentication.AuthenticationLoginBase.GetIPAddress(), true);
                                string url = Request.RawUrl;
                                if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
                                {
                                    url = Request["returnurl"];
                                }
                                else if (url.Contains("/Login") || url.EndsWith("flexfwd.com/") || url.EndsWith("flexfwd.com/Home.aspx"))
                                {
                                    url = ResolveUrl("~/") + siteUser.UserName;
                                }
                                Response.Redirect(url, true);
                            }
                        }                        
                    }
                    catch (Exception)
                    {   // do nothing here.
                    }
                }
            
        }


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
            try
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "PageSettings", GetPageClientScript(), true);
                if (!Page.IsPostBack && !Page.IsCallback)
                {

                    // do a marketing tracker check 
                    try
                    {
                        if (Request["_mt"] != null)
                        {
                            long _mt = Convert.ToInt64(Request["_mt"]);
                            aqufitEntities entities = new aqufitEntities();
                            Marketing marketing = entities.Marketings.Include("MarketingCampaign").FirstOrDefault(m => m.Id == _mt);
                            MarketingTracker track = new MarketingTracker()
                            {
                                Marketing = marketing,
                                Date = DateTime.Now,
                                TrackUrl = Request.Url.AbsoluteUri,
                                HostAddress = Request.UserHostAddress,
                                Referer = Request.UrlReferrer.AbsoluteUri
                            };
                            entities.AddToMarketingTrackers(track);
                            entities.SaveChanges();

                            string trackerJs = "if( _gaq ){ "+
                                                    "_gaq.push(['_trackEvent', 'MarketingTrack', 'Track: "+marketing.MarketingCampaign.Name +"', '"+Request.Url.AbsoluteUri+"', "+marketing.Id+"]); "+
                                                "}";

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "EventTracker", GetPageClientScript(), true);
                        }
                    }
                    catch (Exception) { }   // never fail because we are tracking...                                                      
                }

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public string FaceBookSessionKey()
        {
            if (System.Web.HttpContext.Current == null)
                throw new ApplicationException("HttpContext cannot be null.");

            string fullName = "fbs_" + ConfigurationManager.AppSettings["facebookAppId"];
            if (System.Web.HttpContext.Current.Request.Cookies[fullName] == null)
            {
                return null;
            }
            return System.Web.HttpContext.Current.Request.Cookies[fullName].Value;
        }       


        private string GetPageClientScript()
        {
            string js = string.Empty;
            js += "Aqufit.Page.UserId=" + this.UserId + ";";
            js += "Aqufit.Page.PortalId=" + this.PortalId + ";";
            js += "Aqufit.Page.PageBase='" + ResolveUrl("~/") + "';";
            js += "Aqufit.Page.SiteUrl = 'http://" + HttpContext.Current.Request.Url.Host + System.Web.VirtualPathUtility.ToAbsolute("~/") + "'; ";
            js += "Aqufit.Page.LoginUrl = '" + DotNetNuke.Common.Globals.NavigateURL(PortalSettings.LoginTabId, "Login") + "'; ";
            return js;
        }

        #endregion
    }
}
