<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_SendMessage.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_SendMessage" %>
<%@ Register TagPrefix="ati" TagName="FriendFinder" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendFinder.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">   
<style  type="text/css">    
    /* The following CSS needs to be copied to the page to produce textbox-like RadEditor */
    
    .reLeftVerticalSide,
    .reRightVerticalSide,
    .reToolZone,
    .reToolCell
    {
        background: white !important;
    }
    
    .reContentCell
    {
        border-width: 0 !important;
    }
    
    .formInput
    {
       border: solid 1px black;
    }
    
    .RadEditor
    {
        filter: chroma(color=c2dcf0);
    }
    
    .reWrapper_corner,
    .reWrapper_center
    {
        display: none !important; /* for FF */            
    }
    
    td.reWrapper_corner,
    td.reWrapper_center
    {
        display: block\9 !important; /* for all versions of IE */            
    }
    
    </style>

<script type="text/javascript">

    Aqufit.Page.Controls.atiSendMessage = function(id) {
        this.id = id;
    };

    Aqufit.Page.Controls.atiSendMessage.prototype = {
        refresh: function(){
            Aqufit.Page.atiFriendFinder.init();
        }
    };
    
    $(function(){
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiSendMessage('<%=this.ID %>');        
    });

</script>
</telerik:RadCodeBlock>

<div id="atiSendMessageForm" class="atiFormPanel" style="clear: both;">
<fieldset>
    <dl>
        <dt><asp:Label id="plTo" runat="server" controlname="txtTo" text="To:" /></dt>
        <dd><ati:FriendFinder id="atiFriendFinder" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>         
        
        <dt><asp:Label id="plSubject" runat="server" controlname="txtSubject" text="Subject:" /></dt>
        <dd><asp:TextBox ID="atiTxtSubject" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="255" /></dd>
        
        <dt><asp:Label id="plMessage" runat="server" controlname="txtMessage" text="Message:" /></dt>       
        <dd><asp:TextBox ID="atiTxtMessage" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" Width="100%" Height="250px" MaxLength="65536" TextMode="MultiLine" /></dd>		 	          
   </dl>  
</fieldset>
</div>
