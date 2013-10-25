using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DotNetNuke;

using Affine.Data;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

public partial class DesktopModules_ATI_Base_controls_ATI_FindInvite : DotNetNuke.Framework.UserControlBase
{

    public UserSettings UserSettings { get; set; }
    public Group GroupSettings { get; set; }
    public int TabId { get; set; }

    // TODO: this could be named better
    // right now this is what sets up the control as a group invite and not a friend finder..
    private bool _IsInviteOnly = false;
    public bool IsInviteOnly{ get{ return _IsInviteOnly;} set{ _IsInviteOnly = value; } }

    public bool IsOAuthPostback
    {
        get
        {
            if (this.ViewState["IsOAuthPostback"] != null)
            {
                return Convert.ToBoolean(this.ViewState["IsOAuthPostback"]);
            }
            return false;
        }
        set
        {
            this.ViewState["IsOAuthPostback"] = value;
        }
    }
       

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!Page.IsPostBack && !Page.IsPostBack)
        {
            if (this.UserSettings == null || this.TabId == null)
            {
                throw new Exception("Find Invite Control missing parameters");
            }
            if (this.IsInviteOnly)
            {
                panelManualSearch.Visible = false;
                atiFoundPanel.Visible = true;
                //   ScriptManager.RegisterStartupScript(this, Page.GetType(), "ContactInviteList", "$(function(){ Aqufit.Page.atiContactInviteScript.generateStreamDom('{}'); });", true);
            }
            if (this.IsInviteOnly && this.GroupSettings != null)
            {
                atiContactInviteScript.UserSettings = this.GroupSettings;
                atiFoundFriendListScript.ControlMode = DesktopModules_ATI_Base_controls_ATI_FriendListScript.Mode.GROUP_INVITE;
            }
            else
            {
                atiContactInviteScript.UserSettings = this.UserSettings;
                atiFoundFriendListScript.ControlMode = DesktopModules_ATI_Base_controls_ATI_FriendListScript.Mode.FRIEND_REQUEST;
            }
            if (Request[Parameters.OAuth_Token] != null && Request[Parameters.OAuth_Verifier] != null && Context.Items["s"] != null)
            {
                this.IsOAuthPostback = true;
                string serviceType = ((string)Context.Items["s"]).ToLower();

                string returnUri = string.Empty;
                if (this.IsInviteOnly && this.GroupSettings != null)
                {
                    atiContactInviteScript.UserSettings = this.GroupSettings;
                    returnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + TabId + "/" + GroupSettings.UserName + "/" + serviceType + "/contacts";
                }
                else
                {
                    atiContactInviteScript.UserSettings = this.UserSettings;
                    returnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + TabId + "/" + UserSettings.UserName + "/" + serviceType + "/contacts";
                }
                IOAuthSession oauthSession = OAuthSessionFactory.createSession(serviceType, returnUri);

                string requestTokenString = Request[Parameters.OAuth_Token];
                string verifier = Request[Parameters.OAuth_Verifier];
                IToken requestToken = (IToken)Session[requestTokenString];

                IToken accessToken;

                try
                {
                    accessToken = oauthSession.ExchangeRequestTokenForAccessToken(requestToken, verifier);
                    oauthSession.AccessToken = accessToken;
                    Session[requestTokenString] = null;
                    Session[accessToken.Token] = accessToken;
                    //Response.Write(test);
                    ContactSearchResults(oauthSession.GetContactsEmails());
                }
                catch (OAuthException authEx)
                {
                    Session["problem"] = authEx.Report;
                    Response.Write(authEx.Message + "<br/><br/>" + authEx.StackTrace);
                    return;
                }
            }
            else if (Request["ConsentToken"] != null)      // We have a windows live login ( OMFG I wish they had OAuth sorted out like the rest of the world )
            {
                string ConsentToken = Request["ConsentToken"];
                System.Collections.Specialized.NameValueCollection consent = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(ConsentToken));
                string uri = "https://livecontacts.services.live.com/@L@" + consent["lid"] + "/rest/LiveContacts/Contacts/";
                //string uri2 = "https://livecontacts.services.live.com/users/@L@" + consent["lid"] + "/rest/livecontacts";
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(uri);
                request.Method = "GET";
                request.Headers.Add("UserAgent", "Windows Live Data Interactive SDK");
                request.ContentType = "application/xml; charset=utf-8";
                request.Headers.Add("Authorization", "DelegatedToken dt=\"" + consent["delt"] + "\"");
                request.Headers["Cookie"] = Response.Headers["Set-Cookie"];
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream);
                string resString = streamReader.ReadToEnd();
                System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(resString);
                //<Contacts>
                //  <Contact><ID>b41c2cec-e5c7-426c-b8e6-0043e7cdaeb0</ID>
                //  <Profiles><Personal><FirstName>Krista</FirstName><LastName>Greeves</LastName><UniqueName>bluegirl</UniqueName><SortName>Greeves,Krista</SortName><DisplayName>bluegirl</DisplayName></Personal></Profiles>
                //  <Emails>
                //      <Email><ID>1</ID><EmailType>Personal</EmailType><Address>bluegirl@lycos.com</Address><IsIMEnabled>false</IsIMEnabled><IsDefault>true</IsDefault></Email>
                //  </Emails></Contact>

              //  Affine.Data.json.Contact[] contactList = doc.Element("Contacts").Descendants("Email").Select(c => new Affine.Data.json.Contact() { Email = c.Element("Address").Value }).OrderBy(c => c.Email).ToArray();
                // We need to find all the contacts that are in the system now.
                //ContactSearchResults(contactList);
                ContactSearchResults(doc.Element("Contacts").Descendants("Email").Select(c => c.Element("Address").Value).ToArray());
            }
                
            SetupOauth();     
        }               
       
    }

    private void ContactSearchResults(string[] emails)
    {
        // TODO: make this an asyn call to store the contacts
        //string[] emails = contactList.Select( c => c.Email ).ToArray();
        //foreach (Affine.Data.json.Contact c in contactList)
        //{                
        //}
        Affine.Data.json.Contact[] contactList = emails.Select(e => new Affine.Data.json.Contact() { Email = e }).ToArray();
        contactList = contactList.OrderBy(c => c.Email).ToArray();
        Affine.Data.Managers.IStreamManager streamMan = Affine.Data.Managers.LINQ.StreamManager.Instance;
        string json = string.Empty;
        Affine.Data.json.UserSetting[] foundUsers = streamMan.FindFriendsFromContacts(UserSettings.Id, contactList);
        if (this.GroupSettings == null)
        {
            json = streamMan.ToJsonWithPager(foundUsers);
        }
        else
        {   // we have a group
            Affine.Data.json.UserSetting[] foundMembers = streamMan.GetGroupMembersOfRelationship(this.GroupSettings.Id, Affine.Utils.ConstsUtil.Relationships.GROUP_MEMBER, 0, 250);
            Affine.Data.json.UserSetting[] setMinus = foundUsers.Except(foundMembers.AsEnumerable()).ToArray();
            json = streamMan.ToJsonWithPager(setMinus);
        }

        atiFoundPanel.Visible = true;
        atiFindFriendPanel.Visible = false;
        //            atiFriendPanel.Visible = false;

        System.Web.Script.Serialization.JavaScriptSerializer serial = new System.Web.Script.Serialization.JavaScriptSerializer();

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "ContactInviteList", "$(function(){ Aqufit.Page.atiContactInviteScript.generateStreamDom('" + serial.Serialize(contactList) + "'); });", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "ContactList", "$(function(){ Aqufit.Page.atiFoundFriendListScript.generateStreamDom('" + json + "'); });", true);
    }

    private void SetupOauth()
    {
        //  bFacebook.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/bFacebook.png");

        string returnUri = string.Empty;
        if (this.IsInviteOnly && this.GroupSettings != null)
        {
            returnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + TabId + "/" + GroupSettings.UserName;
        }
        else
        {
            returnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + TabId + "/" + UserSettings.UserName;
        }

        //
        // Yahoo Setup ...
        // 
        imgYahoo.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/bYahoo.png");
        string yahooReturnUri = returnUri + "/yahoo/contacts";

        var yahooSession = OAuthSessionFactory.createSession("yahoo", yahooReturnUri);
        IToken yahooRequestToken = yahooSession.GetRequestToken();
        if (string.IsNullOrEmpty(yahooRequestToken.Token))
        {
            throw new Exception("The request token was null or empty");
        }
        Session[yahooRequestToken.Token] = yahooRequestToken;
        string yahooAuthorizationUrl = yahooSession.GetUserAuthorizationUrlForToken(yahooRequestToken);
        aYahoo.HRef = yahooAuthorizationUrl;


        // 
        // GMail setup ...
        //
        imgGoogle.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/bGmail.png");
        string googleReturnUri = returnUri + "/google/contacts";
        var googleSession = OAuthSessionFactory.createSession("google", googleReturnUri);
        IToken googleRequestToken = googleSession.GetRequestToken();
        if (string.IsNullOrEmpty(googleRequestToken.Token))
        {
            throw new Exception("The request token was null or empty");
        }
        Session[googleRequestToken.Token] = googleRequestToken;
        string googleAuthorizationUrl = googleSession.GetUserAuthorizationUrlForToken(googleRequestToken);
        aGoogle.HRef = googleAuthorizationUrl;

        //
        // MSN setup ...
        //
        imgLive.Src = ResolveUrl("~/DesktopModules/ATI_Base/resources/images/bMsn.png");
        // string LiveReturnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + base.ProfileSettings.UserName + "/live/contacts";
        // var LiveSession = OAuthSessionFactory.createSession("Live", LiveReturnUri);
        // IToken LiveRequestToken = LiveSession.GetRequestToken();
        //  if (string.IsNullOrEmpty(LiveRequestToken.Token))
        //  {
        //      throw new Exception("The request token was null or empty");
        //  }
        //  Session[LiveRequestToken.Token] = LiveRequestToken;
        // string LiveAuthorizationUrl = "http://login.live.com/wlogin.srf?appid=" + ConfigurationManager.AppSettings["wll_appid"] + "&alg=" + ConfigurationManager.AppSettings["wll_securityalgorithm"];
        // string liveReturnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + base.ProfileSettings.UserName + "/live/contacts";

        string extraArg = string.Empty;
        if (this.IsInviteOnly && this.GroupSettings != null)
        {
            extraArg = "&g=" + GroupSettings.UserName;
        }

        string liveReturnUri = "http://" + Request.Url.Host + ResolveUrl("~/") + "?tabid=" + TabId + extraArg;
        string privacyUri = "http://" + Request.Url.Host + ResolveUrl("~/Privacy.aspx");
        //string LiveAuthorizationUrl = "https://consent.live.com/delegation.aspx?ru=" + HttpUtility.UrlEncode( liveReturnUri) + "&ps=Contacts.Invite&pl=" + HttpUtility.UrlEncode( privacyUri) + "&app=" + HttpUtility.UrlEncode( "appid=" + ConfigurationManager.AppSettings["wll_appid"] + "&ts=" + DateTime.Now.Ticks );
        string LiveAuthorizationUrl = "https://consent.live.com/delegation.aspx?ru=" + HttpUtility.UrlEncode(liveReturnUri) + "&ps=Contacts.View&pl=" + HttpUtility.UrlEncode(privacyUri);
        aLive.HRef = LiveAuthorizationUrl;

    }
}
