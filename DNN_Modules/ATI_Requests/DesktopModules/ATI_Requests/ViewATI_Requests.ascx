<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Requests.ViewATI_Requests" CodeFile="ViewATI_Requests.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_Profile.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<link href="<%=ResolveUrl("~/DesktopModules/ATI_Routes/resources/css/friends.css")%>" type="text/css" rel="stylesheet" />
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">
<style type="text/css">
    div.atiFriendItemRight
    {
    	width: 360px !important;
    }
    
div.atiStreamItemRight
    {
	    width: 430px !important;	    
    }
</style>
<script language="javascript" type="text/javascript">

    $(function () {
        $('#atiStatusWidget').hide();
        $('#tabs').tabs();
        $('#<%=bRemoveAll.ClientID %>').button();
    });

    Aqufit.Page.Actions = {
        SendSuggestedFriendRequest: function (usid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddSuggestFriend');
            $('#<%=hiddenAjaxValue.ClientID %>').val(usid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Requests","ATI_Requests") %>', '');
        }
    };

    Aqufit.addLoadEvent(function () {
        Aqufit.Page.atiFriendRequestScript.acceptFriendAction = function (fid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('friendAccept');
            $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Requests","ATI_Requests") %>', '');
        };
        Aqufit.Page.atiFriendRequestScript.denyFriendAction = function (fid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('friendReject');
            $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Requests","ATI_Requests") %>', '');
        };
        Aqufit.Page.atiStreamScript.streamDeleteCallback = function (sid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('deleteNotification');
            $('#<%=hiddenAjaxValue.ClientID %>').val(sid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Requests","ATI_Requests") %>', '');
        };
        if (Aqufit.Page.atiGroupJoinRequest) {
            Aqufit.Page.atiGroupJoinRequest.acceptFriendAction = function (fid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('joinGroup');
                $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Requests","ATI_Requests") %>', '');
            }
            Aqufit.Page.atiGroupJoinRequest.denyFriendAction = function (fid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('deleteRequest');
                $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Requests","ATI_Requests") %>', '');
            }
        }
    });
    
</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>    
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>              
    </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007"></telerik:RadAjaxLoadingPanel>  

 <asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<div id="atiStatusWidget" class="ui-widget">
	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<span id="statusMsg"></span>
	</div>
</div>

    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" style="width: 743px; float: right; ">        
        <div id="divMain" style="width: 538px; float: left; margin-left: 20px; margin-right: 20px;">                        
            <div id="tabs">
            	<ul>
            		<li id="tabRequests" runat="server"><a href="#<%=pageViewRequests.ClientID %>">Notifications / Requests</a></li>            		
            	</ul>
            			
                <div id="pageViewRequests" runat="server" style="padding: 0px; background-color: White;">            
                   <ati:FriendListScript id="atiFriendRequestScript" runat="server" ControlMode="FRIEND_RESPONSE" />
                   <ati:FriendListScript id="atiGroupJoinRequest" runat="server" ControlMode="GROUP_JOIN" Visible="false" />
                   <br /><br />
                   <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3>Notifications</h3>
                        <asp:Button id="bRemoveAll" runat="server" style="position: absolute; right: 10px; top: 3px;" OnClick="bRemoveAll_Click" OnClientClick="return confirm('Are you sure you want to remove ALL notifications?')" Text="Remove All Notifications" />
                    </div>
                    <div>
                        <ati:StreamScript id="atiStreamScript" runat="server" ShowTopPager="false" ShowBottomPager="true" />     
                   </div>                                            
               </div>
            </div>                                                                                                                  
        </div>
        <div id="divRightAdUnit" style="width: 160px; float: right;">
            <asp:Image ID="imgIphoneAd" runat="server"/>            
        </div>
    
    </div>
    <div id="divLeftNav" style="width: 192px; float: left;">
        <ati:Profile id="atiProfile" runat="server" IsSmall="true" />                                   
    </div>
    <div style="clear:both;"></div>   
    








    

    

         
    
               



