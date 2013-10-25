<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ContactInviteScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ContactInviteScript" %>

<script type="text/javascript" >

    Aqufit.Page.Controls.atiContactInvite = function (id, usid) { // New object constructor         
        this.id = id;
        this.usid = usid;
        this.iter = 0;
        this.$contactLength = $('#' + this.id + 'contactLength');
        this.$list = $('#' + this.id + 'atiContactList');
        var cid = this.id;
        $('#' + this.id + 'selectAll').click(function () {
            $('#' + cid + 'atiContactList input:checkbox').attr("checked", true);
        });
        $('#' + cid + 'selectNone').click(function () {
            $('#' + cid + 'atiContactList input:checkbox').attr("checked", false);
        });
        var that = this;
        $('#bSendInvite').button().click(function (event) {
            var contacts = [];
            $('#' + cid + 'atiContactList input:checkbox').each(function () {
                if ($(this).attr("checked")) {
                    contacts.push($(this).val());
                }
            });

            var emails = $('#' + cid + 'txtEmail').val();
            var moreEmails = emails.split(',');
            contacts = contacts.concat(moreEmails);
            Affine.WebService.StreamService.SendInviteToContacts(that.usid, contacts, $('#' + cid + 'contactCustomText').val(), function () {
                alert("Invites have been sent.");   // TODO: need a proper message here
            }, function () {
                alert("fail");      // TODO: need a proper message here
            });
            event.stopPropagation();
            return false;
        });
    };
    
    Aqufit.Page.Controls.atiContactInvite.prototype = {
        generateStreamItem: function (sd, prepend) {    
            var html = '<li id="'+this.id+'atiStreamItem' + (this.iter++) + '">' +
                            '<input type="checkbox" id="'+this.id+'atiMessageCheck' + sd["Id"] + '" value="'+sd["Email"]+'" />&nbsp;'+sd["Email"] + 
                        '</li>';
            if (prepend) {
                this.$list.prepend(html);
                $('#'+this.id+'atiStreamItem' + sd["Id"]).hide();
                $('#'+this.id+'atiStreamItem' + sd["Id"]).show("slow");
            } else {
                this.$list.append(html);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////            
            var cid = this.id;  // (this.id) has new context iside the closures so save it under a new name
                    
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        },        
        generateStreamDom: function (json) {            
            this.json = eval("(" + json + ")");
            this.$list.children().remove();                                                    
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false);
            }
            $('#'+this.id+'contactLength').html(this.json.length+ ' found');
                        
        }          
    };
    
   
    
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiContactInvite('<%=this.ID %>',<%= this.UserSettings != null ? this.UserSettings.Id : 0 %>);       
    });  
        
    
</script>
<asp:Panel ID="atiContactInvite" runat="server" CssClass="atiContactInvite">      
    <div class="contactListHead"><h2>Your Contacts</h2>
        
        <div class="contactWrapper">
            <h3>How about inviting some of your friends to join?</h3>
            <textarea id="<%=this.ID %>txtEmail" style="width: 100%; height: 45px; margin-bottom: 20px;" class="dull">Enter emails separated by commas</textarea>
            <div class="contactLeft">
                <h4>These are your contacts</h4>
                <span>Select: <a href="javascript: ;" id="<%=this.ID %>selectAll">All</a>, <a href="javascript: ;" id="<%=this.ID %>selectNone">None</a><span style="float: right;" id="<%=this.ID %>contactLength">0 found</span></span>
                <div class="contactListScroll">
                    <ul id="<%=this.ID %>atiContactList"></ul>
                </div>
            </div>
            <div class="contactRight">
                <h4>Customize your Invite and send it.</h4>
                <div class="contactMessageWrapper">
                    <em>From:</em>&nbsp;<asp:Literal ID="litFromEmail" runat="server" /><br />
                    <em>Subject:</em>&nbsp;<asp:Literal ID="litSubject" runat="server" /><br /><br />
                    Hi,<br />
                    <textarea id="<%=this.ID %>contactCustomText"></textarea><br /><br />
                    Join ... it's free and always will be!<br />
                    <asp:Literal ID="litRegisterUrl" runat="server" /><br /><br />                    
                    Check out your sender's profile:<br />
                    <asp:Literal ID="litProfileUrl" runat="server" />
                </div>
            </div>
            <button id="bSendInvite">Send Invite</button>                                         
        </div>                                            
    </div>       
</asp:Panel>
<div style="clear: both;"></div>
