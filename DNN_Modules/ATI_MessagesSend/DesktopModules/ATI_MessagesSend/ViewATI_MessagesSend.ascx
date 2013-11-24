<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_MessagesSend.ViewATI_MessagesSend" CodeFile="ViewATI_MessagesSend.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="SendMessage" Src="~/DesktopModules/ATI_Base/controls/ATI_SendMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script language="javascript" type="text/javascript">
    function RequestStart(sender, eventArgs) {
        $('#atiStatusWidget').hide();
    }

    function ResponseEnd(sender, eventArgs) {
        $('#atiStatusWidget').fadeIn('slow');
        $('#<%=bClose.ClientID %>').css({ 'float': 'right', 'margin-top': '10px' }).button().click(function(event) {
            self.close();
            event.stopPropagation();
            return false;
        });
    }

    $(function() {
        $('#<%=bSend.ClientID %>').css({ 'float': 'right', 'margin-top': '10px' }).button();
        $('#atiStatusWidget').hide();
    });
</script>
</telerik:RadCodeBlock>



<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="atiAjaxPanel">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="atiAjaxPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>    
    <ClientEvents OnRequestStart="RequestStart" />
    <ClientEvents OnResponseEnd="ResponseEnd" />
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />

<div style="width: 728; height: 90px; background-color: #CCCCCC; border: 1px solid #666666; text-align: center;">
    AD UNIT
</div>
<asp:Panel ID="atiAjaxPanel" runat="server" style="padding: 20px;">
    <div id="atiStatusWidget" class="ui-widget">
	    <div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		    <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		    <asp:Literal ID="litStatus" runat="server" />
	    </div>
    </div>
    <ati:SendMessage Id="atiSendMessage" runat="server" />
    <asp:Button ID="bSend" runat="server" Text="Send" OnClick="bSend_Click" />
    <asp:Button ID="bClose" runat="server" Text="Close" Visible="false" />
</asp:Panel>
    
   


    

    

         
    
               



