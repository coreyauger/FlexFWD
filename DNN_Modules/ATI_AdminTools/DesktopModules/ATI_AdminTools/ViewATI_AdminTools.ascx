<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_AdminTools.ViewATI_AdminTools" CodeFile="ViewATI_AdminTools.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script language="javascript" type="text/javascript">

    Aqufit.Page.Actions = {
        RowSelected: function () {
        },
        BecomeUser: function (uid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('su');
            $('#<%=hiddenAjaxValue.ClientID %>').val(uid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$AdminTools","ATI_AdminTools") %>', '');
        }
    }

    $(function () {
       
    });
</script>
</telerik:RadCodeBlock>

<style type="text/css">
.atiTxtArea
{
	width: 100%;
}
.radItemStyle
{
	background-color: White;
}
</style>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="atiAjaxPanel">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="atiAjaxPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
       <telerik:AjaxSetting AjaxControlID="RadGridUsers">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridUsers" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>    
    </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />  
    

<asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<asp:Panel ID="atiAdminPanel" runat="server">
    <h1 id="adminTitle" runat="server">User Tools</h1>    
                                                                                     
    <telerik:RadGrid ID="RadGridUsers" runat="server" PageSize="15" ShowStatusBar="true" ClientSettings-ClientEvents-OnRowSelected="Aqufit.Page.Actions.RowSelected" AllowFilteringByColumn="true" ShowGroupPanel="true"  OnNeedDataSource="RadGridUsers_NeedDataSource" GridLines="None" AllowPaging="True" AllowAutomaticUpdates="False" AllowAutomaticInserts="False" AllowAutomaticDeletes="false" AllowSorting="true" Width="100%" Skin="Office2007">
        <PagerStyle Mode="NumericPages"></PagerStyle>  
                            
        <ClientSettings AllowGroupExpandCollapse="True" ReorderColumnsOnClient="True" AllowColumnsReorder="True" AllowDragToGroup="true" Selecting-AllowRowSelect="true"></ClientSettings>
        <GroupingSettings ShowUnGroupButton="true" />
        <MasterTableView AutoGenerateColumns="True" DataKeyNames="Id" CommandItemDisplay="Top">
            <Columns>                                
                     <telerik:GridTemplateColumn>
                        <HeaderTemplate>
                            Actions
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="javascript: Aqufit.Page.Actions.BecomeUser( <%#Eval("Id")%> );">Login</a>
                        </ItemTemplate>
                     </telerik:GridTemplateColumn>                                                          
            </Columns>            
        </MasterTableView>       
    </telerik:RadGrid>

   
</asp:Panel>
    

    

         
    
               



