<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_MapRoute.ViewATI_MapRoute" CodeFile="ViewATI_MapRoute.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="GMapRoute" Src="~/DesktopModules/ATI_Base/controls/ATI_GMapRoute.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<script type="text/javascript">
    function setSelectedMap(id, name, dist, img) {
        if (window.parent.Aqufit.Page.Controls.atiRouteSelector) {
            window.parent.Aqufit.Page.Controls.atiRouteSelector.AddNewItem(id, name, dist, img);
        }
        closeWin();

    }
    function closeWin() {
        self.close();
    }

    Aqufit.addLoadEvent(function () {
        Aqufit.Page.atiGMapRoute.saveButtonClick = function () {
            $('#<%=hiddenAjaxAction.ClientID %>').val('saveRoute');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$MapRoute","ATI_MapRoute") %>', '');
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
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007"></telerik:RadAjaxLoadingPanel>   

<asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>
<asp:Literal ID="atiMapRoute" runat="server" />  
<ati:GMapRoute id="atiGMapRoute" runat="server" Width="100%" Height="100%" />  

    

         
    
               



