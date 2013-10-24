<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="RADMENU" Src="~/DesktopModules/telerik.skinobjects/RadMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="ati" TagName="SEARCH" Src="~/DesktopModules/ATI_Base/controls/ATI_SearchSkinObject.ascx" %>
<%@ Register TagPrefix="ati" TagName="MENU" Src="~/DesktopModules/ATI_Base/controls/ATI_MenuSkinObject.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!-- 
Affine Technology Inc. 
Custom Skin Design

contact: coreyauger(at)gmail.com 
-->
<telerik:RadCodeBlock id="radcodeblock2" runat="server">
<script src="<%=ResolveUrl("~/Resources/Shared/scripts/jquery/jquery-ui-1.8.custom.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/base.js")%>" type="text/javascript"></script>

<script type="text/javascript">

    Aqufit.BugWin = {
        win: null,
        open: function(arg) {
            this.win = window.radopen('<%=ResolveUrl("~/FitnessProfile/ReportaBug.aspx") %>?u=' + self.location.href, null);
            this.win.set_modal(true);
            this.win.setSize(640, 400);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };


    function keyUpHandler(event) {
        if (!event) {
            event = window.event;
        }
        if (event.keyCode == 13) {
            if (typeof Aqufit.EnterKeyHandler == "function") {
                Aqufit.EnterKeyHandler();
            }
            event.stopPropagation();
            return false;
        }
    }

    $(function () {
        $('#atiBugReport').click(function (event) {
            Aqufit.BugWin.open();
            event.stopPropagation();
            return false;
        });
        $('body').bind('keypress', keyUpHandler);
        // We need a custom logout to handle the cookies we want to remove.
        $('#dnn_dnnLogin_cmdLogin').attr('href', 'javascript: ;').click(function (event) {
            //$.cookie('FlexRM', null, { path: '/', domain: '.flexfwd.com' });
            $.cookie('FlexLogout', '1');
            FB.getLoginStatus(function (response) {
                if (response.session) {
                    FB.logout(function (response) {
                        __doPostBack('dnn$dnnLogin$cmdLogin', '');
                    });
                } else {
                    __doPostBack('dnn$dnnLogin$cmdLogin', '');
                }
            });
            event.stopPropagation();
            return false;
        });
    });
    
    
</script>
</telerik:RadCodeBlock>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" />

<!--<script type="text/javascript" src="jquery-1.3.2.js"></script>-->
<table align="center" width="95%" border="0" cellspacing="0" cellpadding="0" id="ati_sitePanel">
<tr valign="bottom">
    <td id="ati_standard_TopSpace">       
        <a href="/ContactUs.aspx">Contact Us</a>
        <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span> 
        <a href="javascript: ;" id="atiBugReport" style="font-weight: normal; font-size: xx-small;">Report a bug</a>
        <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>
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
            <td class="ati_topBannerSpan" style="width: 220px; _vertical-align:middle"><dnn:LOGO runat="server" id="dnnLOGO" BorderWidth="0" CssClass="atiSiteLogo" /></td>
            <td class="ati_topBannerSpan" style="padding: 20px;"><ati:SEARCH ID="atiSEARCH" runat="server" /></td>
            <td class="ati_topBannerSpan"><div style="z-index: 999999; float:right;"><dnn:RADMENU runat="server" id= "dnnMENU" Skin="Black" /></div></td>
            <td class="ati_topBannerSpan" style="width: 150px;"><ati:MENU ID="atiMENU" runat="server" /></td>
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
            <td id="ati_mainTopLeft"></td>
            <td id="ati_mainTopSpan"></td>
            <td id="ati_mainTopRight"></td>
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
            <td id="ati_mainBottomLeft"></td>
            <td id="ati_mainBottomSpan"></td>
            <td id="ati_mainBottomRight"></td>
        </tr>
        </table>
    </td>
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




