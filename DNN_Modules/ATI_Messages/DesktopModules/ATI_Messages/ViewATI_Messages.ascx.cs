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

using Telerik.Web.UI;

using Affine.Data;

namespace Affine.Dnn.Modules.ATI_Messages
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
    partial class ViewATI_Messages : ATI_PermissionPageBase, IActionable
    {

        #region Private Members      
        private const int DEFAULT_TAKE = 25;
        #endregion       

        #region Public Methods      
        #endregion

        #region Event Handlers

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
            base.Page_Load(sender, e);
            try
            {               
                if (!Page.IsPostBack && !Page.IsCallback)
                {
                    if (base.Permissions == AqufitPermission.PUBLIC)
                    {   // You need to be logged in to see this page.. redirect to login
                        Response.Redirect(ResolveUrl("~/Login.aspx"), true);
                        return;
                    }
                    imgAd.Src = ResolveUrl("~/Portals/0/images/adTastyPaleo.jpg");
                    ServiceReference service = new ServiceReference("~/DesktopModules/ATI_Base/resources/services/StreamService.asmx");
                    service.InlineScript = true;
                    ScriptManager.GetCurrent(Page).Services.Add(service);
                    atiProfile.ProfileSettings = UserSettings;
                    atiProfile.IsOwner = true;
                   /*
                    if (Request["m"] != null)
                    {
                        Affine.WebService.StreamService ss = new WebService.StreamService();
                        string json = ss.getMessage(UserSettings.Id, Convert.ToInt64( Request["m"] ) );
                        json = json.Replace("'", "").Replace("\n","");
                        RadAjaxManager1.ResponseScripts.Add("Aqufit.addLoadEvent(function(){alert('"+json+"');  Aqufit.Page.MessageSentListScript.generateMessageDom('"+json+"'); });");
                    }
                    */
                }                        
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        private void deleteMessageForUser(long mid)
        {            
            aqufitEntities entities = new aqufitEntities();
            MessageRecipiant mr = entities.MessageRecipiants.Where(m => m.Message.Id == mid && m.UserSettingsKey == this.UserSettings.Id).First();
            entities.DeleteObject(mr);
            entities.SaveChanges();
            litStatus.Text = "Message has been deleted.";           
        }

        protected void bMessageListDelete_Click(object sender, EventArgs e)
        {
            try
            {
                deleteMessageForUser(Convert.ToInt64(hiddenMessageListDeleteId.Value));
            }
            catch (Exception ex)
            {
                litStatus.Text = "ERROR: There was a problem deleting your message. " + ex.Message;
            }
        }
        protected void bMessageSentDelete_Click(object sender, EventArgs e)
        {
            try
            {
                deleteMessageForUser(Convert.ToInt64(hiddenMessageListDeleteId.Value));
            }
            catch (Exception ex)
            {
                litStatus.Text = "ERROR: There was a problem deleting your message. " + ex.Message;
            }
        }

        protected void atiRadComboBoxSearchMessageInbox_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            // TODO: we need to search "reply" text and add those messages to the results.
            RadComboBox atiRadComboBoxSearchMessageInbox = (RadComboBox)sender;
            atiRadComboBoxSearchMessageInbox.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<Message> messagesQuery = entities.MessageRecipiants.Where( m => m.UserSettingsKey == this.UserSettings.Id ).Select( m => m.Message ).OrderBy(m => m.DateTime);
            int length = messagesQuery.Count();
            messagesQuery = string.IsNullOrEmpty(e.Text) ? messagesQuery.Where(m => m.UserSetting.Id != this.UserSettings.Id).Skip(itemOffset).Take(TAKE) : messagesQuery.Where(m => m.UserSetting.Id != this.UserSettings.Id && m.Subject.ToLower().Contains(e.Text) || m.Text.ToLower().Contains(e.Text)).Skip(itemOffset).Take(TAKE);

            Message[] messages = messagesQuery.ToArray();

            foreach (Message m in messages)
            {
                RadComboBoxItem item = new RadComboBoxItem(m.Subject);
                item.Value = "" + m.Id;
               // item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                atiRadComboBoxSearchMessageInbox.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }

        protected void atiRadComboBoxSearchMessageSent_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            // TODO: we need to search "reply" text and add those messages to the results.
            RadComboBox atiRadComboBoxSearchMessageInbox = (RadComboBox)sender;
            atiRadComboBoxSearchMessageInbox.Items.Clear();
            const int TAKE = 5;
            aqufitEntities entities = new aqufitEntities();
            int itemOffset = e.NumberOfItems;
            IQueryable<Message> messagesQuery = entities.MessageRecipiants.Where(m => m.UserSettingsKey == this.UserSettings.Id).Select(m => m.Message).OrderBy(m => m.DateTime);
            int length = messagesQuery.Count();
            messagesQuery = string.IsNullOrEmpty(e.Text) ? messagesQuery.Where(m => m.UserSetting.Id == this.UserSettings.Id).Skip(itemOffset).Take(TAKE) : messagesQuery.Where(m => m.UserSetting.Id == this.UserSettings.Id && m.Subject.ToLower().Contains(e.Text) || m.Text.ToLower().Contains(e.Text)).Skip(itemOffset).Take(TAKE);

            Message[] messages = messagesQuery.ToArray();

            foreach (Message m in messages)
            {
                RadComboBoxItem item = new RadComboBoxItem(m.Subject);
                item.Value = "" + m.Id;
                // item.ImageUrl = ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") + "?u=" + g.UserKey + "&p=" + g.PortalKey;
                atiRadComboBoxSearchMessageInbox.Items.Add(item);
            }
            int endOffset = Math.Min(itemOffset + TAKE + 1, length);
            e.EndOfItems = endOffset == length;
            e.Message = (length <= 0) ? "No matches" : String.Format("Items <b>1</b>-<b>{0}</b> of {1}", endOffset, length);
        }



        protected void bAjaxPostback_Click(object sender, EventArgs e)
        {
            string status = string.Empty;
            try
            {
                Affine.Data.Managers.IDataManager dataManager = Affine.Data.Managers.LINQ.DataManager.Instance;
                switch (hiddenAjaxAction.Value)
                {                    
                    case "AddSuggestFriend":
                        try
                        {
                            long usid = Convert.ToInt64(hiddenAjaxValue.Value);
                            status = dataManager.sendFriendRequest(UserSettings.Id, usid);
                            RadAjaxManager1.ResponseScripts.Add(" Aqufit.Page.atiProfile.ShowOk('A Friend request has been sent');");                          
                        }
                        catch (Exception ex)
                        {
                            RadAjaxManager1.ResponseScripts.Add("Aqufit.Page.Actions.ShowFail('Error: " + ex.Message + "');");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //status = "ERROR: There was a problem with the action (" + ex.Message + ")";
                RadAjaxManager1.ResponseScripts.Add(" alert('" + ex.Message + "'); ");
            }
        }



        protected void bSearchPeople_Click(object sender, EventArgs e)
        {
            /*
            string nameOrEmail = txtSearchPeople.Text;
            aqufitEntities entities = new aqufitEntities();            
            IList<UserSettings> people = entities.UserSettings.Include("Image").Where( us => (us.UserFirstName + us.UserLastName).Contains(nameOrEmail) || us.UserEmail.Contains(nameOrEmail) ).ToList();
            
            if (people.Count > 0)
            {
                atiFoundPanel.Visible = true;
                atiNotFound.Visible = false;
                atiFoundFriendList.DataSource = people;
                atiFoundFriendList.DataBind();
            }
            else
            {
                atiFoundPanel.Visible = false;
                atiNotFound.Visible = true;
                lNotFound.Text = "Sorry we did not find \"<em>" + nameOrEmail + "</em>\".";
            }
             */            
        }        

        /*
        protected void bReply_Click(object sender, EventArgs e)
        {
            // make sure that the person can reply to this message
            aqufitEntities entities = new aqufitEntities();
            // 1) TODO: we need the friend "multi select" control.
            // for now this is just the single user name (TEMP: we will just use the request var)
            Message replyTo = atiMessageSend.ReplyToMessage;

            // check if these users are really friends before we send the message.
            string txt = atiMessageSend.Message;
            string subject = string.IsNullOrEmpty(atiMessageSend.Subject) ? "(No Subject)" : atiMessageSend.Subject;
            DateTime dt = DateTime.Now.ToUniversalTime();
            UserSettings settings = entities.UserSettings.FirstOrDefault( us => us.UserKey == this.UserId );
            Message message = new Message()
            {
                UserSetting = settings,
                PortalKey = this.PortalId,
                Status = 0, // TODO: make nice message status ( 0 = unread )
                DateTime = dt,
                ParentKey = replyTo.Id,
                Subject = subject,
                Text = txt
            };
            // This is how we handle repys ... we want to be able to query the messages easy and know what has changed... so we store some
            // data about the last reply in the "source" message.  That way we can update and show that there is a new message for people
            // but not have to search the entire message history to do this.
            Message reply = entities.Messages.FirstOrDefault(m => m.Id == replyTo.Id);
            reply.LastUserKey = this.UserId;
            reply.LastText = txt.Length > 128 ? txt.Substring(128) + "..." : txt;          
            reply.LastDateTime = dt;
            entities.AddToMessages(message);
            entities.SaveChanges();

        }
         */
                     
        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Registers the module actions required for interfacing with the portal framework
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();
                Actions.Add(this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), ModuleActionType.AddContent, "", "", this.EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }
}

