<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Player.ViewATI_Player" CodeFile="ViewATI_Player.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<div style="width: 728; height: 90px; background-color: #CCCCCC; border: 1px solid #666666; text-align: center;">
    AD UNIT
</div>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">  
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="RadProgressArea1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadProgressArea1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>   
     </AjaxSettings>      
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<!-- <script src='<%=ResolveUrl("~/DesktopModules/ATI_Player/AC_RunActiveContent.js") %>' language='javascript'></script> -->
<script language="javascript" type="text/javascript">
  /*
    AC_FL_RunContent(
        'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0',
        'width', '400',
        'height', '325',
        'src', ((!DetectFlashVer(9, 0, 0) && DetectFlashVer(8, 0, 0)) ? 'OSplayer' : 'OSplayer'),
        'pluginspage', 'http://www.macromedia.com/go/getflashplayer',
        'id', 'flvPlayer',
        'allowFullScreen', 'true',
        'allowScriptAccess', 'always',
        'movie', ((!DetectFlashVer(9, 0, 0) && DetectFlashVer(8, 0, 0)) ? 'OSplayer' : 'OSplayer'),
        'FlashVars', 'movie=<%=ResolveUrl("~/DesktopModules/ATI_Player/mario.flv") %>&btncolor=0x333333&accentcolor=0x31b8e9&txtcolor=0xdddddd&volume=30&autoload=on&autoplay=off&vTitle=Super Mario Brothers Lego Edition&showTitle=yes');
*/
</script>
</telerik:RadCodeBlock>

 <asp:Panel ID="panelPlayer" runat="server">
     <object width="800" height="600" id="flvPlayer">
      <param name="allowFullScreen" value="true">
      <param name="allowScriptAccess" value="always"> 
      <param name="movie" value="<%=ResolveUrl("~/DesktopModules/ATI_Player/OSplayer.swf")%>?movie=<%=this.FlvPath %>&btncolor=0x333333&accentcolor=0x31b8e9&txtcolor=0xdddddd&volume=80&autoload=on&autoplay=off&vTitle=&showTitle=no">
      <embed 
            src="<%=ResolveUrl("~/DesktopModules/ATI_Player/OSplayer.swf")%>?movie=<%=this.FlvPath %>&btncolor=0x333333&accentcolor=0x31b8e9&txtcolor=0xdddddd&volume=80&autoload=on&autoplay=off&vTitle=&showTitle=no" 
            width="800" height="600" 
            allowFullScreen="true" 
            type="application/x-shockwave-flash"
            allowScriptAccess="always">
     </object>
 </asp:Panel>


    
   


    

    

         
    
               



