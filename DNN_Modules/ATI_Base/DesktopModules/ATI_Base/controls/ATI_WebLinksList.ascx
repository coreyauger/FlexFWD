<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WebLinksList.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_WebLinksList" %>

<ul class="webList"> 
    <li runat="server" id="liTwitter"><a id="aTwitter" target="_blank" runat="server"><asp:Image runat="server" ID="imgWebTwitter" /></a></li>                         
    <li runat="server" id="liFaceBook"><a id="aFacebook" target="_blank" runat="server"><asp:Image runat="server" ID="imgWebFaceBook" /></a></li>
    <li runat="server" id="liLinkedIn"><a id="aLinkedIn" target="_blank" runat="server"><asp:Image runat="server" ID="imgWebLinkedIn" /></a></li>
    <li runat="server" id="liYouTube"><a id="aYouTube" target="_blank" runat="server"><asp:Image runat="server" ID="imgWebYouTube" /></a></li>
    <li runat="server" id="liFlickr"><a id="aFlickr" target="_blank" runat="server"><asp:Image runat="server" ID="imgWebFlickr" /></a></li>
</ul> 
<div style="text-align: center; padding: 3px;"><a id="aAddSites" runat="server" visible="false">add my sites</a><a id="aPersonSite" runat="server">Website</a> </div>