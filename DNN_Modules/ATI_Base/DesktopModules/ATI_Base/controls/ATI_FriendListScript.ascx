<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FriendListScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FriendListScript" %>
<%@ Register TagPrefix="ati" TagName="DataPager" Src="~/DesktopModules/ATI_Base/controls/ATI_DataPager.ascx" %>

<script type="text/javascript" >          
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiFriendList('<%=this.ID %>', '<%=atiFriendList.ClientID %>', '<%=this.ControlMode %>', <%=this.IsOwner?"true":"false" %>, '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif") %>', '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png") %>', '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png") %>', '<%=this.Title %>');        
    });
    
    Aqufit.addLoadEvent(function () {
        Aqufit.Page.<%=this.ID %>.setTopPager(Aqufit.Page.<%=atiDataPagerTop.UniqueID %>);
        Aqufit.Page.<%=this.ID %>.setBottomPager(Aqufit.Page.<%=atiDataPagerBottom.UniqueID %>);
    });              
</script>

<ati:DataPager id="atiDataPagerTop" runat="server" />     
<asp:Panel ID="atiFriendList" runat="server" CssClass="atiFriendList">   
</asp:Panel>  
<div style="clear: both;"></div>
<ati:DataPager id="atiDataPagerBottom" runat="server" />   
