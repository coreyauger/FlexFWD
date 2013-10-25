<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_InviteList.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_InviteList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
    
    </script>
</telerik:RadCodeBlock>

<asp:Panel ID="atiInvitePanel" runat="server">
        <telerik:RadGrid ID="atiRadInviteGrid" SelectedItemStyle-BackColor="Silver" runat="server" AllowPaging="False" Width="100%" AllowSorting="True" AllowMultiRowSelection="True" GridLines="None" OnNeedDataSource="atiRadInviteGrid_NeedDataSource">
            <ClientSettings EnableRowHoverStyle="true">
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView>
                <Columns>
                    <telerik:GridClientSelectColumn>
                    </telerik:GridClientSelectColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>   

</asp:Panel>