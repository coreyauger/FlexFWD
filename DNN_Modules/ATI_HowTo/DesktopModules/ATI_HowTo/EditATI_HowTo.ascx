<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_HowTo.EditATI_HowTo" CodeFile="EditATI_HowTo.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">
 div.atiMainFeed
 {
 	width: 660px;
 	padding: 10px 20px;
 }
    
.atiMainFeed div.atiStreamItemRight
{
	width: 570px;
}
</style>
<script type="text/javascript">

    Aqufit.Page.Actions = {
        onNodeClicking: function (sender, args) {
            //alert("OnClientNodeClicking: " + args.get_node().get_value());
            top.location.href = '?h=' + args.get_node().get_value();
        }
    };

    $(function () {
        $('#streamerTabs').tabs();
        $('#menuTabs').tabs();
    });

</script>
</telerik:RadCodeBlock>


<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">    
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
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>


<div style="position: relative;">
<div style="width: 729px; float: right;">
    <div id="streamerTabs">
        <ul>
            <li id="tabActivity"><a href="#pageViewActivity">Site Help / How to</a></li>            				
        </ul>
            			
        <div id="pageViewActivity" style="padding: 0px; background-color: White;">
            <div class="atiMainFeed">
                <dnn:TextEditor ID="TextEditor" runat="server" Height="700px" ></dnn:TextEditor>
                <asp:Button ID="bSave" runat="server" Text="Save" OnClick="bSave_Click" />
            </div>
        </div>
    </div>
</div>
<div style="width: 200px; float: left;">
    <div id="menuTabs">
        <ul>
            <li><a href="#pageViewMenu">Help Menu</a></li>            				
        </ul>
        <div id="pageViewMenu" style="padding: 0px; background-color: White;">
            <telerik:RadTreeView runat="server" ID="RadTreeView1" Style="margin: 15px;" Skin="Office2007" Width="200px"
                DataSourceID="EntityDataSource1" DataTextField="Title" DataValueField="Id" OnDataBound="RadTreeView1_DataBound"
                DataFieldID="Id" DataFieldParentID="ParentKey" OnClientNodeClicking="Aqufit.Page.Actions.onNodeClicking">       
            </telerik:RadTreeView>
            <asp:EntityDataSource runat="server" ID="EntityDataSource1" ContextTypeName="Affine.Data.aqufitEntities" EntitySetName="HelpPages">
            </asp:EntityDataSource>
            
        </div>
    </div>
</div>
    
</div>
<br style="clear: both;" />
    

         
    
               



