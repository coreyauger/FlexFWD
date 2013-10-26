<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_BugReporter.ViewATI_BugReporter" CodeFile="ViewATI_BugReporter.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script language="javascript" type="text/javascript">
    function RequestStart(sender, eventArgs) {
        $('#<%=txtDescription.ClientID %>').hide();
        $('#<%=bSend.ClientID %>').hide();
        $('#atiStatusWidget').hide();
    }

    function ResponseEnd(sender, eventArgs) {
        $('#atiStatusWidget').show();
    }

    $(function () {
        $('#atiStatusWidget').hide();
        $('#<%=bSend.ClientID %>').button({});
        var screenRes = screen.height + ' X ' + screen.width;
        $('#<%=hiddenScreenRes.ClientID %>').val(screenRes);
    });
</script>
</telerik:RadCodeBlock>

<style type="text/css">
.atiTxtArea
{
	width: 95%;
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
       <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>    
    </AjaxSettings>    
    <ClientEvents OnRequestStart="RequestStart" />
    <ClientEvents OnResponseEnd="ResponseEnd" />
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />  
    
<asp:Panel ID="atiAjaxPanel" runat="server">    
   <div class="ui-widget" style="margin: 9px 18px 9px 18px;">
   <asp:Panel ID="panelBugHead" runat="server">
       <asp:Image id="imgBug" runat="server" style="float: right;" />
       <h2>Ooops, it appears you have found a bug</h2>
       <p>Please provide a description of the error and/or what to do to reproduce the error.</p>   
   </asp:Panel>
   <asp:Panel ID="panelContactHead" runat="server" Visible="false">
       <asp:Image id="imgContact" runat="server" style="float: right;" />
       <h2>Contact Us</h2>
       <p>We will work to respond to your request as soon as possible.</p>   
   </asp:Panel>
   <div id="atiStatusWidget" class="ui-widget">
    	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
    		<span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
    		<asp:Literal ID="litStatus" runat="server" />        
    	</div>
    </div>        
    <asp:HiddenField id="hiddenScreenRes" runat="server" />
    <div id="divEmail" runat="server" visible="false">
    <label>Email</label><br />
    <asp:TextBox id="txtEmail" runat="server" MaxLength="90" CssClass="ui-corner-all ui-widget-content atiTxtBox"/>
    </div>
    <br />
   <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="ui-corner-all ui-widget-content atiTxtArea" Style="min-height: 200px;"  />
   <asp:Button ID="bSend" runat="server" Text="Send" OnClick="bSend_Click" />
   </div>
</asp:Panel>

<asp:Panel ID="atiAdminPanel" runat="server" Visible="false">
    <h1 id="adminTitle" runat="server">Open Bug Reports</h1>
    <p>TODO: right now bugs have a type string:status that is the state of the bug "Open"/"Closed".  We only get open bugs on this grid.  Could add a way to see all.. or closed ones as well.</p>
    <telerik:RadGrid ID="RadGrid1" runat="server"
        GridLines="None" AllowPaging="True" AllowAutomaticUpdates="True" AllowAutomaticInserts="True"
        AllowSorting="true" Width="100%" OnItemCreated="RadGrid1_ItemCreated" Skin="Office2007">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <MasterTableView AutoGenerateColumns="True"
            DataKeyNames="Id" CommandItemDisplay="Top">
            <Columns>
                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                </telerik:GridEditCommandColumn>
                
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" SortExpression="Id"
                    UniqueName="Id"  MaxLength="5" >
                </telerik:GridBoundColumn>

                <telerik:GridBoundColumn DataField="DateTime" HeaderText="DateTime" SortExpression="DateTime"
                    UniqueName="DateTime">
                </telerik:GridBoundColumn>    

                <telerik:GridBoundColumn DataField="AbsoluteUrlReferrer" HeaderText="Referer" SortExpression="AbsoluteUrlReferrer"
                    UniqueName="AbsoluteUrlReferrer">
                </telerik:GridBoundColumn>    

                <telerik:GridBoundColumn DataField="Description" HeaderText="Description" SortExpression="Description"
                    UniqueName="Description">
                </telerik:GridBoundColumn>                                                                  
            </Columns>
            
            <EditFormSettings>
              <EditColumn ButtonType="ImageButton" />
            </EditFormSettings>
            
        </MasterTableView>       
    </telerik:RadGrid>

   
</asp:Panel>
    

    

         
    
               



