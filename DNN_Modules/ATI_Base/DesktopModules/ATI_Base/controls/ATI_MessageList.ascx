<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_MessageList.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_MessageList" %>
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
	border: 1px solid #858585;
}
</style>

<asp:Panel ID="atiStreamPanel" runat="server">
        <asp:DataList runat="server" 
            DataKeyField="Id" 
            ID="atiDataListStream"
            OnItemCreated="atiDataListStream_ItemCreated"
            OnDeleteCommand="atiDataListStream_DeleteCommand"
             >                             
            <ItemTemplate>            
                <div style="border-bottom: 1px solid #666666;">
                   <div class="thumb vcard author" style="float: left; width: 70px; padding-right: 15px; padding-bottom: 15px; padding-top: 15px; padding-left: 17px;">
                        <a class="url fn" href="<%# Eval("UserName")%>">
                            <img src="<%= ResolveUrl("~/DesktopModules/ATI_Base/images/profile.aspx") %>?u=<%# Eval("UserKey")%>" />
                            <span class="author_name" style="display: block; font-size: 9px;"><%# Eval("UserName")%></span> 
                        </a>
                   </div> 
                   <div class="entry-content" style="float: right; width: 545px; padding-bottom: 15px; padding-top: 15px;">
                        <%if (!this.IsMessageHistory)
                          { %><strong class="entry-title"><a href="javascript: void(0);" onclick="DoAjaxPostback('message',<%# Eval("Id") %>);"><asp:Label ID="lTitle" runat="server" Text='<%# Eval("Subject") %>' /></a></strong><br /><% } %>                        
                            <span><asp:Label ID="lText" runat="server" Text='<%# Eval("Text") %>' /></span><br />                                             
                            <span class="byline" style="position: relative; bottom: 0px;">
                            <a class="entry-bookmark" rel="bookmark" href="/people/suroot/entries/431152">
                                <abbr class="published" title='<%# Eval("Date") %>'><asp:Label ID="lDaysAgo" runat="server" Text='<%# Eval("DaysAgo") %>' /></abbr>
                            </a>
                        </span>
                        <br />                        
                        <asp:LinkButton runat="server" ID="lbDelete" CommandName="delete" Text="Delete"  />   
                    </div>                    
                </div>                            
            </ItemTemplate>
            <FooterTemplate>
                <strong>TODO: Data Pager</strong> 
            </FooterTemplate>
        </asp:DataList>        

</asp:Panel>
