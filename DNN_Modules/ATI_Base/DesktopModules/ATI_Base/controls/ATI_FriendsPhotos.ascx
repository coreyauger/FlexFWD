<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FriendsPhotos.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FriendsPhotos" %>
<style type="text/css">
    #atiFriendPhotoPanel img
    {
    	padding-top: 4px;    	
    }
#atiFriendPhotoPanel a
{
	font-size: 9px;
	font-weight: bold;
	padding-left: 8px;
	
}
#atiFriendPhotoPanel table a
{
	width: 50px;
	overflow: hidden;
	display: block;
}

</style>
<div id="atiFriendPhotoPanel">
<a id="hlNumFriends" runat="server" style="float:left; padding-left: 8px; padding-bottom: 4px; font-size: 9px;">? Friends</a>
<a ID="hlSeeAll" runat="server" style="float:right; padding-right: 16px; font-size: 9px;">See All</a>
<br style="clear: both;" />
<table width="100%" border="0" cellpadding="0" cellspacing="0" runat="server" id="friendPhotoGrid">
<tr>
    <td width="33%"><a runat="server" id="img1LinkA"><asp:Image ID="img1" visible="false" runat="server" /></a><a runat="server" id="img1LinkB"></a></td>
    <td width="33%"><a runat="server" id="img2LinkA"><asp:Image ID="img2" visible="false" runat="server" /></a><a runat="server" id="img2LinkB"></a></td>
    <td width="33%"><a runat="server" id="img3LinkA"><asp:Image ID="img3" visible="false" runat="server" /></a><a runat="server" id="img3LinkB"></a></td>
</tr>
<tr>
    <td width="33%"><a runat="server" id="img4LinkA"><asp:Image ID="img4" runat="server" visible="false" /></a><a runat="server" id="img4LinkB"></a></td>
    <td width="33%"><a runat="server" id="img5LinkA"><asp:Image ID="img5" runat="server" visible="false" /></a><a runat="server" id="img5LinkB"></a></td>
    <td width="33%"><a runat="server" id="img6LinkA"><asp:Image ID="img6" runat="server" visible="false" /></a><a runat="server" id="img6LinkB"></a></td>
</tr>
</table>
</div>