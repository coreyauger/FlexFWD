<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FriendList.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FriendList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

    <script type="text/javascript">
        function sendFriendRequest(id) {
            alert(id);
        }
    </script>

</telerik:RadCodeBlock>

<style type="text/css">

.friendPanel
{
	border-bottom: 1px solid #666666; 
	width: 100%;
}
.friendPanel0
{
	display: none;
}
.sentRequestPanel
{
	display: none;
}
.sentRequestPanel0
{
	display: block;
	background-color: #90d6b2;	
}

</style>

<asp:Panel ID="atiFriendPanel" runat="server">
    <asp:HiddenField ID="hiddenFriendKey" runat="server" />
    <asp:Button ID="bPostBack" runat="server" style="display: none;" />
        <asp:DataList runat="server" 
            DataKeyField="Id" 
            ID="atiFriendList"
            OnItemCreated="atiFriendList_ItemCreated"
            OnDeleteCommand="atiFriendList_DeleteCommand"
            >             
            <EditItemTemplate>
                <div class="thumb vcard author">
                    <a class="url fn" href="/people/suroot">
                        <telerik:RadBinaryImage runat="server" ID="atiStreamAvitar" Width="70px" AutoAdjustImageControlSize="false" DataValue='<%# Eval("Image")%>' />
                        <span class="author_name">Corey A.</span> 
                    </a><span class="workout-sport run">RUN</span>
                </div>
              
                <asp:LinkButton ID="lbUpdate" runat="server" CommandName="update" Text="Save" />                
                &nbsp;
                <asp:LinkButton ID="lbCancel" runat="server" CommandName="cancel" Text="Cancel" />                 
            </EditItemTemplate>
            <ItemTemplate>
                <div class="friendPanel friendPanel<%# Eval("Id")%>">
                   <div class="thumb vcard author" style="float: left; width: 70px; padding-right: 15px; padding-bottom: 15px; padding-top: 15px;">
                        <a class="url fn" href="<%=ResolveUrl("~/")%><%# Eval("UserName")%>">
                            <telerik:RadBinaryImage runat="server" ID="atiStreamAvitar" Width="70px" AutoAdjustImageControlSize="false" DataValue='<%# Eval("Image")%>' />                        
                            <span class="author_name" style="display: block; font-size: 9px;">[SCORE]</span> 
                        </a>
                   </div> 
                   <div class="entry-content" style="position: relative; float: right; width: 497px; padding-bottom: 15px; padding-top: 15px;">
                        <strong class="entry-title"><asp:Label ID="lTitle" runat="server" Text='' /></strong><br />
                        <div class="entry-description">     
                            <a href="<%=ResolveUrl("~/")%><%# Eval("UserName")%>" style="display: block; font-size: 13px; font-weight: bold;"><%# Eval("UserName")%></a>                        
                            <p><asp:Label ID="lDescription" runat="server" Text='<%# Eval("Email") %>' /></p>
                            <div style="position: absolute; right: 0px; top: 30px;">
                                
                                <input type="button" value="<%=this.Text %>" onclick="sendFriendRequest(<%# Eval("Id") %>);" style='display: <%# ((int)Eval("Status") == 0) ? "block" : "none" %>' />
                                <span style='display: <%# ((int)Eval("Status") == 1) ? "block" : "none" %>'>Friend Request Pending</span>                               
                            </div>
                        </div>                                                
                    </div>                    
                </div>   
                <div class="sentRequestPanel sentRequestPanel<%# Eval("Id")%>">      
                    <span>Friend Request sent to <%# Eval("UserName")%> (<%# Eval("Email") %>)</span>
                </div>                           
            </ItemTemplate>
            <FooterTemplate>
                <strong>TODO: Data Pager</strong> 
            </FooterTemplate>
        </asp:DataList>        

</asp:Panel>