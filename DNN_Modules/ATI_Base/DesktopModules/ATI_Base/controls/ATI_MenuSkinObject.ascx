<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_MenuSkinObject.ascx.cs" Inherits="Affine.Web.Controls.ATI_MenuSkinObject" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<style type="text/css">

div.atiTmProfile
{
	float: left;	
	width: 86px;	
	height: 45px;
	z-index: 9999;
	position: relative;
}

div.atiTmProfile img
{
	width: 28px;
	height: 28px;
	border: 1px solid #000;
	margin-left: 25px;
	margin-top: 8px;
}

div.atiTmProfileBg
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmProfile.png) no-repeat top left !important;
}
div.atiTmProfileBg:hover
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmProfileHover.png) no-repeat top left !important;	
}
div.atiTmProfileClick
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmProfileClick.png) no-repeat top left !important;	
}

div#atiTmProfileMenu ul.linksList li
{
	list-style: none;
	list-style: none outside none;
}

div.atiTmMsg
{
	float: left;
	width: 62px;
	height: 45px;
	z-index: 9999;
}
div.atiTmJoin
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmJoin.png) no-repeat top right !important;	
	width: 148px;
	height: 45px;
	cursor: pointer;
}
div.atiTmMsgBg
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmMsg.png) no-repeat top right !important;		
}

div.atiTmMsgBg:hover
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmMsgHover.png) no-repeat top left !important;	
}
div.atiTmMsgClick
{
	background: #000 url(/DesktopModules/ATI_Base/resources/images/tmMsgClick.png) no-repeat top left !important;	
}

div.atiTmMsg span
{
	font-size: 11px;
	font-weight: bold;
	color: #FFF;
	position: relative;
	top: 12px;
	left: 27px;
}



div.atiTmHoverMenu
{
	width: 137px;
	background-color: #FFF;
	border-left: 1px solid #000;
	border-right: 1px solid #000;
	border-bottom: 1px solid #000;
	position: absolute;
	z-index: 99999;
	margin-top: 45px;
	left: 6px;
	padding: 6px;
}
ul.linksList li a
{
	display: block;
	list-style:none;
	list-style: none outside none;
}
ul.linksList li a:hover
{	
	text-decoration: none;
}

div#atiTmMsgMenu a
{
	display: block;
	padding: 4px 5px;
	text-decoration: none;
}
div#atiTmMsgMenu a:hover
{
	background-color: #eee;
}

</style>

<script type="text/javascript">

    $(function () {
        $('div.atiTmProfile').click(function (event) {
            $('div.atiTmMsg').removeClass('atiTmMsgClick').addClass('atiTmMsgBg');
            $(this).removeClass('atiTmProfileBg').addClass('atiTmProfileClick');
            $('div.atiTmHoverMenu').hide();
            $('div#atiTmProfileMenu').show();
            event.stopPropagation();
        });
        $('div.atiTmMsg').click(function (event) {
            $('div.atiTmProfile').removeClass('atiTmProfileClick').addClass('atiTmProfileBg');
            $(this).removeClass('atiTmMsgBg').addClass('atiTmMsgClick');
            $('div.atiTmHoverMenu').hide();
            $('div#atiTmMsgMenu').show();
            event.stopPropagation();
        });
        $('div.atiTmJoin').click(function (event) {
            top.location.href = "/Home/Register/tabid/63/Default.aspx";
            event.stopPropagation();
        });

        $(document).click(function () {
            $('div.atiTmProfile').removeClass('atiTmProfileClick').addClass('atiTmProfileBg');
            $('div.atiTmMsg').removeClass('atiTmMsgClick').addClass('atiTmMsgBg');
            $('div.atiTmHoverMenu').hide();
        });
    });

    

</script>

<asp:Panel ID="atiMenuSkinObject" runat="server" style="position: relative; z-index: 3;">
<ul class="hlist">
    <li><a href="javascript: ;" title="Actions"><div class="atiTmProfile atiTmProfileBg"><img src="" id="pImg" runat="server" width="28" height="28" /></div></a></li>    
    <li><a href="javascript: ;" title="Notifictions"><div class="atiTmMsg atiTmMsgBg"><asp:Label ID="lNumUnread" runat="server" /></div></a></li>
</ul>
<div id="atiTmProfileMenu" class="atiTmHoverMenu shadow" style="display: none;">
    <ul class="linksList">
        <li><a id="hlProfile" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iProfile.png")%>" /> Profile</a></li>
        <li><a id="hlGroups" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iGroup_s.png")%>" /> Groups</a></li>
        <li><a id="hlInbox" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMail_s.png")%>" /> Inbox</a></li>
        <li><a id="hlFriends" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFriend_s.png")%>" /> Friends</a></li> 
        <li><a id="hlRoutes" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iRoutes.png")%>" /> My Routes</a></li>    
        <li><a id="hlWorkouts" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iHistory_s.png")%>" /> Workout History</a></li>                                                                  
    </ul>
</div>
<div id="atiTmMsgMenu" class="atiTmHoverMenu shadow" style="display: none;">
    <ul>
        <asp:Literal ID="litDebug" runat="server" />
    </ul>
</div>
</asp:Panel>
<asp:Panel ID="atiMenuSkinJoin" runat="server" Visible="false" style="position: relative;">
<div class="atiTmJoin">
</div>
</asp:Panel>