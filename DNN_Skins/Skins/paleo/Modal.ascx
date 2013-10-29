<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<!-- 
Affine Technology Inc. 
Custom Skin Design

contact: coreyauger(at)gmail.com 
-->
<link rel="icon" type="image/vnd.microsoft.icon" href="favicon.ico"/>
<script src="<%=ResolveUrl("~/Resources/Shared/scripts/jquery/jquery-ui-1.8.custom.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/base.js")%>" type="text/javascript"></script>

<table cellspacing="0" cellpadding="0" width="100%" border="0">     
<tr>
    <td id="TopImagePane" class="TopImage-Pane" valign="top" runat="server">    
    <!-- Pane to put some section header graphic into -->               
    </td> 
</tr>                
<tr>
    <td valign="top">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td id="LeftPane" class="Left-Pane" valign="top" runat="server">
            <!-- Container Pane -->                      
            </td>
            <td id="ContentPane" class="Middle-Pane" valign="top" runat="server">
            <!-- Container Pane -->
            </td>
            <td id="RightPane" class="Right-Pane" valign="top" runat="server">
            <!-- Container Pane -->                      
            </td>
        </tr>
        </table>
    </td>
</tr>       
<tr>
    <td id="BottomPane" class="Bottom-Pane" valign="top" runat="server">
    <!-- Container Pane -->                
    </td>
</tr>
</table>      











