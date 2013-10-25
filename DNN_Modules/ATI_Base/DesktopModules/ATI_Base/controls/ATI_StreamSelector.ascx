<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamSelector.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamSelector" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

    <script type="text/javascript">
        function RowDblClick(sender, eventArgs) {
            sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
        }
    </script>

</telerik:RadCodeBlock>

<style type="text/css">
.border
{
	border: 1px dashed #858585;
}

</style>

<asp:Panel ID="atiStreamPanel" runat="server">
        <asp:DataList runat="server" 
            DataKeyField="Id" 
            ID="atiDataListStream"
            OnItemCreated="atiDataListStream_ItemCreated"
            >                              
            <ItemTemplate>            
                <div style="border-bottom: 1px solid #666666;" onclick="switchWorkoutData(<%# Eval("Id")%>);">
                   <div class="thumb vcard author" style="float: left; width: 20px; padding-right: 15px; padding-bottom: 15px; padding-top: 15px;">
                        <a class="url fn" href="<%# Eval("UserName")%>">                            
                            <span class="author_name" style="display: block; font-size: 9px;"><%# Eval("UserName")%></span> 
                        </a><span class="workout-sport run">RUN</span>
                   </div> 
                   <div class="entry-content" style="float: right; width: 159px; padding-bottom: 15px; padding-top: 15px;">
                        <strong class="entry-title"><asp:Label ID="lTitle" runat="server" Text='<%# Eval("Title") %>' /></strong><br />
                        <span><asp:Label id="lDate" runat="server" Text='<%# Eval("Date") %>' /> </span><br />
                        <span><asp:Label ID="lDistance" runat="server" Text='<%# Eval("Distance") %>' /><span> km</span></span>
                        <span><asp:Label ID="lDuration" runat="server" Text='<%# Eval("Duration") %>' /></span>
                        <span>(<asp:Label ID="lPace" runat="server" Text='' /> pace)</span>                                       
                        <span class="byline" style="position: relative; bottom: 0px;">
                            <abbr class="published" title=''><asp:Label ID="lDaysAgo" runat="server" Text='' /></abbr>                        
                        </span>                        
                    </div>                    
                </div>                                              
            </ItemTemplate>
            <FooterTemplate>
                <strong>TODO: Data Pager</strong> 
            </FooterTemplate>
        </asp:DataList>        
              
</asp:Panel>
