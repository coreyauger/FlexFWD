<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_SendMessageScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_SendMessageScript" %>

<script type="text/javascript">

    Aqufit.Page.Controls.atiSendMessage = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#' + control;
        this.toUserIdArray = [];
    };        

    Aqufit.Page.Controls.atiSendMessage.prototype = {        
        setUserIdArray: function( json ){
            this.toUserIdArray = eval('('+json+')');
        },
        Send: function(){
            var atiTxtSubject = $('#atiTxtSubject').val();
            var atiTxtMessage = $('#atiTxtMessage').val();
            $('#atiSendMessageForm').hide();
            $('#sendMessageStatus').show();                                    
            Affine.WebService.StreamService.SendMessage(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.<%=this.ID %>.toUserIdArray, atiTxtSubject, atiTxtMessage,
                function(){
                    $('#sendMessageStatus').hide();    
                    $('#sendMessageSent').show();       // TODO: this is not showing (frig... not sure why yet)             
                }, function(){
                    $('#sendMessageStatus').hide();  
                    $('#sendMessageFail').show();
                    $('#atiSendMessageForm').show();
                });                         
        }
    };


    Aqufit.Page.Controls.SendMessage = {
        init: function(){
            Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiSendMessage('<%=this.ID %>', 'atiSendMessageForm');     
            Aqufit.Page.<%=this.ID %>.setUserIdArray('<%= this.Json %>');    
            // $('#sendMessageStatus').addClass('ui-state-highlight').children('span.ui-icon').removeClass('ui-icon-info').addClass('ui-icon-check').next().show();    
        }
    };  

</script>

<div id="sendMessageFail" class="ui-widget" style="display: none;">
	<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;"> 
		<p><span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span> 
		<strong>Alert:</strong> Message Send Failed.</p>
	</div>
</div>
<div id="atiSendMessageForm" class="atiFormPanel" style="clear: both;">
<fieldset>
    <ul>
        <li>
            <label><asp:Label id="plTo" runat="server" controlname="txtTo" text="To:" /></label>
            <em><asp:Label id="lUserNames" runat="server" /></em>      
        </li>
        <li>
            <label><asp:Label id="plSubject" runat="server" controlname="txtSubject" text="Subject:" /></label>
            <input type="textbox" ID="atiTxtSubject" class="ati_Form_TextBox" maxlength="255" />
        </li>      
        <li>
            <label><asp:Label id="plMessage" runat="server" controlname="txtMessage" text="Message:" /></label>       
            <textarea id="atiTxtMessage" maxlength="16384" style="width: 100%; height: 180px;"></textarea>           	 
        </li>		          
   </ul>   
   <input id="bSendMessage" onclick="Aqufit.Page.<%=this.ID %>.Send();" type="button" value="Send" />
</fieldset>
</div>
<div id="sendMessageSent" class="ui-widget" style="display: none;">
	<div class="ui-corner-all ui-state-highlight" style="margin-top: 20px; padding: 0 .7em;"> 
		<p><span class="ui-icon ui-icon-check" style="float: left; margin-right: .3em;"></span>
		Message Sent :)</p>
	</div>
</div>
<div id="sendMessageStatus" class="ui-widget" style="display: none;">
	<div class="ui-corner-all" style="margin-top: 20px; padding: 0 .7em;"> 
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		Sending your message.</p>
	</div>
</div>



