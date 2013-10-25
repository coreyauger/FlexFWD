<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamScript" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<script type="text/javascript">
    Aqufit.Windows.FacebookWin = {
        win: null,
        open: function (url) {
            this.win = window.radopen(url, null);
            this.win.set_modal(true);
            this.win.setSize(747, 600);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };

    $(function () {     
       Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiStreamPanel('<%=this.ID %>', '<%=atiStreamPanel.ClientID %>', <%=this.IsFollowMode ? 1 : 0 %>, $('#<%=atiStateSkip.ClientID %>').val( ), <%=this.DefaultTake %>, <%=this.IsSearchMode ? "true":"false" %>, '<%=this.EditUrl %>', <%=this.ShowTopPager? "true":"false" %>, <%=this.ShowBottomPager? "true":"false" %>, <%=this.ShowStreamSelect? "true":"false" %>, '<%=atiStateSkip.ClientID %>', '<%=atiStreamLoading.ClientID %>');       
    });  
    
</script>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" />
<asp:HiddenField ID="atiStateSkip" runat="server" EnableViewState="true" Value="0" />
<div id="atiStreamTarget">
    <div id="atiStreamLoading" runat="server" style="text-align: center;">
        <img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading_blue_circle.gif") %>" />
    </div>  
    <asp:Panel ID="atiStreamPanel" runat="server">           
    </asp:Panel>
    <div style="clear: both;"></div>
</div>

