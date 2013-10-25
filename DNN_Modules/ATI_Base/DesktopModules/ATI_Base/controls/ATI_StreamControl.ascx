<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamControl" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

    <script type="text/javascript">
        function RowDblClick(sender, eventArgs) {
            sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
        }

    </script>

</telerik:RadCodeBlock>

<asp:Panel ID="atiStreamPanel" runat="server">
        <asp:DataList runat="server" 
            DataKeyField="Id" 
            ID="atiDataListStream"
            OnEditCommand="atiDataListStream_EditCommand" 
            OnCancelCommand="atiDataListStream_CancelCommand" 
            OnUpdateCommand="atiDataListStream_UpdateCommand"
            OnItemCreated="atiDataListStream_ItemCreated"
            OnDeleteCommand="atiDataListStream_DeleteCommand"
             >
             
            <EditItemTemplate>
                <div class="thumb vcard author">
                    <a class="url fn" href="<%# Eval("UserName")%>">                        
                        <span class="author_name">Corey A.</span> 
                    </a><span class="workout-sport run">RUN</span>
                </div>
                
                Title: <asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>' />                  
                <br />
                Distance: <asp:TextBox ID="textCategoryName" runat="server" Text='<%# Eval("Distance") %>' />                     
                <br />
                Duration: <asp:TextBox ID="textDescription" runat="server" Text='<%# Eval("Duration") %>' />
                <br />
                <asp:LinkButton ID="lbUpdate" runat="server" CommandName="update" Text="Save" />                
                &nbsp;
                <asp:LinkButton ID="lbCancel" runat="server" CommandName="cancel" Text="Cancel" />                 
            </EditItemTemplate>         
            <ItemTemplate>            
                <div style="border-bottom: 1px dashed #666666; width: 100%;">
                    <div class="thumb vcard author" style="float: left; width: 70px; padding-right: 15px; padding-bottom: 15px; padding-top: 15px; padding-left: 17px;">
                        <a class="url fn" href="<%# Eval("UserName")%>">
                                               
                            <span class="author_name" style="display: block; font-size: 9px;"><%# Eval("UserName")%></span> 
                        </a>
                    </div> 
                   <div class="entry-content" style="float: right; width: 440px; padding-bottom: 15px; padding-top: 15px;" onclick="compareWorkoutData(<%# Eval("Id") %>, 16, 'test');">
                        <strong class="entry-title"><asp:Label ID="lTitle" runat="server" Text='<%# Eval("Title") %>' /></strong><br />
                        <span><asp:Label id="lDate" runat="server" Text='<%# Eval("Date") %>' /> </span><br />
                        <span><asp:Label ID="lDistance" runat="server" Text='<%# Eval("Distance") %>' /><span class="workout-distance-units"> km</span></span>
                        <span><asp:Label ID="lDuration" runat="server" Text='<%# Eval("Duration") %>' /></span>                       
                        <div class="entry-description">                            
                            <span><asp:Label ID="lDescription" runat="server" Text='<%# Eval("Description") %>' /></span>
                        </div>                        
                        <span class="byline" style="position: relative; bottom: 0px;">
                            <abbr class="published" title=''><asp:Label ID="lDaysAgo" runat="server" Text='' /></abbr>                        
                        </span>
                        <br />
                        Comments: <%# Eval("Comments") %>
                        <div style="float: right;">
                            <asp:LinkButton runat="server" ID="lbShare" CommandName="share" Text="Share"  />
                            <asp:LinkButton runat="server" ID="lbComment" CommandName="comment" Text="Comment"  />                           
                            <asp:LinkButton runat="server" ID="lbEdit" CommandName="edit" Text="Add Details"  />
                            <asp:LinkButton runat="server" ID="lbDelete" CommandName="delete" Text="Delete"  />   
                        </div>
                    </div>                    
                </div>                            
            </ItemTemplate>
            <FooterTemplate>
                <strong>TODO: Data Pager</strong> 
            </FooterTemplate>
        </asp:DataList>        

</asp:Panel>
