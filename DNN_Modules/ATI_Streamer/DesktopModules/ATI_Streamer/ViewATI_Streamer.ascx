<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Streamer.ViewATI_Streamer" CodeFile="ViewATI_Streamer.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">
<style type="text/css">
 div.atiMainFeed
 {
 	width: 660px;
 }
    
.atiMainFeed div.atiStreamItemRight
{
	width: 570px;
}
</style>
<script type="text/javascript">

    $(function () {
        $('#streamerTabs').tabs();
        // TODO: need the comment add delete handlers...
    });

    Aqufit.addLoadEvent(function () {
         Aqufit.Page.atiStreamScript.streamDeleteCallback = function (id) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('delStream');
            $('#<%=hiddenAjaxValue.ClientID %>').val(id);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Streamer","ATI_Streamer") %>', '');
        };
        Aqufit.Page.atiStreamScript.onDeleteComment = function (id) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('delComment');
            $('#<%=hiddenAjaxValue.ClientID %>').val(id);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Streamer","ATI_Streamer") %>', '');
        };            
    });

</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>    
        </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   
    
<asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenWorkoutKey" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<div style="width: 660px;">
<div id="streamerTabs">
    <ul>
        <li id="tabActivity"><a href="#pageViewActivity">Recent Athlete Activity</a></li>            				
    </ul>
            			
    <div id="pageViewActivity" style="padding: 0px; background-color: White;">
        <div class="atiMainFeed">
            <ati:StreamScript ID="atiStreamScript" runat="server" ShowBottomPager="false" ShowTopPager="false" /> 
        </div>
    </div>
</div>
</div>
    

         
    
               



