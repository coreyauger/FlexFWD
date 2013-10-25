<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FindInvite.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FindInvite" %>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ContactInviteScript" Src="~/DesktopModules/ATI_Base/controls/ATI_ContactInviteScript.ascx" %>
<script language="javascript" type="text/javascript">    
    
    
    function SendFriendRequest(id) {
        Aqufit.Page.Actions.SendFriendRequest(fid);
    }

    $(function () {
        $('.dull').focus(function () {
            $(this).val('').removeClass('dull');
        });
        // connect the button to webservice
        $('#bSearchPeople').button().click(function (event) {
            var that = this;
            $(this).hide();
            $('#imgSearchLoading').show();
            Affine.WebService.StreamService.searchUsers(Aqufit.Page.UserSettingsId, $('#<%=txtSearchPeople.ClientID %>').val(), 0, 25,
                function (json) {                   
                    Aqufit.Page.atiFriendRequestScript.generateStreamDom(json);
                    $('#imgSearchLoading').hide();
                    $(that).show();
                },
                 function (err) {
                     $('#imgSearchLoading').hide();
                     $(that).show();
                 });
            event.stopPropagation();
            return false;
        });
    });

    Aqufit.addLoadEvent(function () {
        if (Aqufit.Page.atiFriendRequestScript) {
            Aqufit.Page.atiFriendRequestScript.sendFriendRequest = function (fid) {
                Aqufit.Page.Actions.SendFriendRequest(fid);
            }
            Aqufit.Page.atiFriendRequestScript.onDataNeeded = function (skip, take) {
                $('#imgSearchLoading').show();
                Affine.WebService.StreamService.searchUsers(Aqufit.Page.UserSettingsId, $('#<%=txtSearchPeople.ClientID %>').val(), skip, take,
                function (json) {
                    Aqufit.Page.atiFriendRequestScript.generateStreamDom(json);
                    $('#imgSearchLoading').hide();
                    $(that).show();
                },
                 function (err) {
                     $('#imgSearchLoading').hide();
                     $(that).show();
                 });
            }
        }
        if (Aqufit.Page.atiFoundFriendListScript) {
            Aqufit.Page.atiFoundFriendListScript.sendFriendRequest = function (fid) {
                Aqufit.Page.Actions.SendFriendRequest(fid);
            }
            Aqufit.Page.atiFoundFriendListScript.acceptFriendAction = function (fid) {
                Aqufit.Page.Actions.SendGroupInvite(fid);
            }
        }

    });
    

</script>

<asp:Panel id="atiFindFriendPanel" runat="server" class="atiFormPanel">
    <div style="padding: 10px;">
     
        <h2>Find people you email</h2>
        <p>Searching your email account is the fastest way to find your friends on FlexFWD.</p>
        <div id="serviceOptions">
            <a id="aYahoo" runat="server"><img ID="imgYahoo" runat="server" /></a >
                
            <a id="aGoogle" runat="server"><img ID="imgGoogle" runat="server" /></a >
                
            <a id="aLive" runat="server"><img ID="imgLive" runat="server" /></a >
        </div>
        <asp:HiddenField ID="hiddenFriendKey" runat="server" />                                       
        <div style="clear: both;"></div>      
        
        <asp:Panel ID="panelManualSearch" runat="server">                               
            <h2>Search for People</h2>  
            <p>Type the name or email to search the system for that user.</p>              
            <asp:TextBox ID="txtSearchPeople" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" Width="450" runat="server" Text="Enter a name or email" />
            <button id="bSearchPeople">Search</button><img id="imgSearchLoading" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif") %>" style="display:none;" />
        </asp:Panel>
    </div>
    <ati:FriendListScript id="atiFriendRequestScript" runat="server" ControlMode="FRIEND_REQUEST" />                                                                                                             
</asp:Panel> 
            
<asp:Panel ID="atiFoundPanel" runat="server">
    <ati:FriendListScript id="atiFoundFriendListScript" runat="server" />   
    <div style="background-color: #FFF;"> 
        <ati:ContactInviteScript id="atiContactInviteScript" runat="server" /> 
    </div>                      
</asp:Panel>

