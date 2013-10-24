<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTMENU" Src="~/Admin/Skins/SolPartMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<!-- 
Affine Technology Inc. 
Custom Skin Design

contact: coreyauger(at)gmail.com 
-->

<!--<script type="text/javascript" src="jquery-1.3.2.js"></script>-->
<table align="center" width="976" border="0" cellspacing="0" cellpadding="0" id="ati_sitePanel">
<tr valign="bottom">
    <td id="ati_standard_TopSpace">        
        <dnn:USER runat="server" id="dnnUser" CssClass="ati_standard_LoginRegister" />
        <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>	
        <dnn:LOGIN runat="server" id="dnnLogin" CssClass="ati_standard_LoginRegister" />	
    </td>
</tr>
<tr valign="top">
    <td>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" id="ati_topBanner">
        <tr valign="bottom">
            <td id="ati_topBannerLeft">&nbsp;</td>
            <td class="ati_topBannerSpan"><dnn:LOGO runat="server" id="dnnLOGO" BorderWidth="0"/></td>
            <td class="ati_topBannerSpan">
                <dnn:SOLPARTMENU
						runat="server"
						id="dnnSOLPARTMENU"
						menualignment="right"
						separatecss="true"
						userootbreadcrumbarrow="false"
						usesubmenubreadcrumbarrow="false"
						menueffectsmouseoverdisplay="none"						
						rootmenuitemcssclass="MainMenu_Idle"
						rootmenuitemactivecssclass="MainMenu_Active"
						rootmenuitemselectedcssclass="MainMenu_Selected"
						rootmenuitembreadcrumbcssclass="MainMenu_BreadcrumbActive"
						rootmenuitemlefthtml="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
						rootmenuitemrighthtml="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
						rightseparator="<DIV class='MainMenu_Seperator'>&nbsp;</DIV>"
						rightseparatoractive="<DIV class='MainMenu_Seperator'>&nbsp;</DIV>"
						rightseparatorbreadcrumb="<DIV class='MainMenu_Seperator'>&nbsp;</DIV>"
						/>	  
            </td>
            <td id="ati_topBannerRight">&nbsp;</td>
        </tr>
        </table>
    </td>
</tr>
<tr>
    <td class="ati_hSpacer" id="ati_mainTopSpace">&nbsp;</td>
</tr>
<tr valign="top">
    <td>
        <table border="0" cellpadding="0" cellspacing="0" id="ati_main">
        <tr>
            <td id="ati_mainTopLeft">&nbsp;</td>
            <td id="ati_mainTopSpan">&nbsp;</td>
            <td id="ati_mainTopRight">&nbsp;</td>
        </tr>
        <tr>
            <td id="ati_mainLeft">&nbsp;</td>
            <td id="ati_mainCenter">          
             <!-- Main Content Area -->    
                <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">     
                <tr>
                        <td id="TopImagePane" class="TopImage-Pane" valign="top" runat="server">    
                        <!-- Pane to put some section header graphic into -->
                       
                        </td> 
                </tr>         
                <tr>
                    <td>                                
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td id="TopLeftPane" class="Top-Left-Pane" valign="top" runat="server">                      
                            <!-- Container Pane -->
                            </td>
                            <td id="TopRightPane" class="Top-Right-Pane" valign="top" runat="server">
                            <!-- Container Pane -->                      
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" height="100%">
                        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="LeftPane" height="100%" class="Left-Pane" valign="top" runat="server">
                            <!-- Container Pane -->                      
                            </td>
                            <td id="ContentPane" height="100%" class="Middle-Pane" valign="top" runat="server">
                            <!-- Container Pane -->
                            </td>
                            <td id="RightPane" height="100%" class="Right-Pane" valign="top" runat="server">
                            <!-- Container Pane -->                      
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
                <tr>                          
                <tr valign="top">
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td id="BottomLeftPane" class="Bottom-Left-Pane" valign="top" runat="server">
                            <!-- Container Pane -->                      
                            </td>
                            <td id="BottomRightPane" class="Bottom-Right-Pane" valign="top" runat="server">
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
            </td>
            <td id="ati_mainRight">&nbsp;</td>
        </tr>
        <tr>
            <td id="ati_mainBottomLeft">&nbsp;</td>
            <td id="ati_mainBottomSpan">&nbsp;</td>
            <td id="ati_mainBottomRight">&nbsp;</td>
        </tr>
        </table>
    </td>
</tr>
<tr>
    <td class="ati_hSpacer" id="ati_mainBottomSpace">&nbsp;</td>
</tr>
<tr>
    <td class="ati_bottomInfo">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="30%" nowrap="nowrap">
                <dnn:PRIVACY runat="server" id="dnnPRIVACY" CssClass="ati_standard_LoginRegister" />
		        <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>
		        <dnn:TERMS runat="server" id="dnnTERMS" CssClass="ati_standard_LoginRegister" />					                        
            </td>
            <td width="70%" valign="top" align="right" class="copyright">
                <dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" CssClass="ati_standard_LoginRegister" />                  
            </td>                
        </tr>
        </table> 
    </td>
</tr>
</table>




