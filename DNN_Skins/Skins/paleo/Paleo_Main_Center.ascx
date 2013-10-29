<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="CURRENTDATE" Src="~/Admin/Skins/CurrentDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!-- 
Affine Technology Inc. 
Custom Skin Design

contact: coreyauger(at)gmail.com 
-->
<div id="atiBeta"></div>
<link rel="icon" type="image/vnd.microsoft.icon" href="favicon.ico"/>
<telerik:RadCodeBlock id="radcodeblock2" runat="server">
<script src="<%=ResolveUrl("~/Resources/Shared/scripts/jquery/jquery-ui-1.8.custom.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/base.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        var left = ($('body').width() - $('div#atiSiteBG').width()) / 2;
        $('div#atiSiteBG').css('left', left + 'px');
        $(window).resize(function () {
            var left = ($('body').width() - $('div#atiSiteBG').width()) / 2;
            $('div#atiSiteBG').css('left', left + 'px');
        });
        //   $('#IconBar.ascx_cmdVisibility').click(function () {
        //       $('div#atiSiteBG').hide();
        //   });
        $('table.ControlPanel').css('position', 'absolute');
        $('table.ControlPanel').css('z-index', '9999');
        $('#bSearchRecipes').button({
            icons: {
                primary: 'ui-icon-search'
            }
        }).click(function (event) {
            var s = $('#atiTxtSearch').val();
            self.location.href = '<%=ResolveUrl("~/Search.aspx") %>?s=' + s;
            event.stopPropagation();
            return false;
        });

        $("#atiTxtSearch").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#bSearchRecipes").trigger('click');
                event.stopPropagation();
                return false;
            }
        });
        $('#atiBeta').click(function () {
            $(this).hide();
        });

    });


    Aqufit.BugWin = {
        win: null,
        open: function (arg) {
            this.win = window.radopen('<%=ResolveUrl("~/MyPaleo/ReportaBug.aspx") %>?u=' + self.location.href , null);
            this.win.set_modal(true);
            this.win.setSize(640, 400);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };

    Aqufit.addLoadEvent(function () {
        if (navigator.appName.toLowerCase().indexOf("internet explorer") == -1) {
            setTimeout(function () {
                $('.dropshadow, .mainMenu span.txt').dropShadow();
            }, 1500);
        } else {
            $('#atiMainRaised').removeClass('atiMainRaised').addClass('atiMainRaisedIE');
            $('#atiLegal').css('position', 'relative').css('top', '400px');
        }
        $('#atiBugReport').click(function (event) {
            Aqufit.BugWin.open();
            event.stopPropagation();
            return false;
        });       
    });

</script>
</telerik:RadCodeBlock>

<style type="text/css">
.atiMainRaisedIE
{
	position: relative; 
	left: -18px; 
	margin-right: 18px; 
	background-color: white; 
	padding-left: 18px; 
	margin-bottom: 18px; 
	padding: 18px;
}
.atiMainRaised
{
	margin-right: 18px; 
	background-color: white; 
	margin-left: -18px; 
	margin-bottom: 18px; 
	padding: 18px;
}
</style>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" />

  <center>
<div ID="paleoMainPanel" style="margin: auto; width: 948px; position: relative; z-index: 1000;">   
    <div id="ati_topPanelSearch">
        <input type="text" id="atiTxtSearch" class="ui-corner-all ui-widget-content" style="height: 20px; background: none; background-color: #EEEEEE; color: #666666; width: 150px;" /><button id="bSearchRecipes">Search</button>
    </div>
    <div id="ati_topPanel">
         
         <div style="float: right; margin-right: 20px;">
            <a href="javascript: ;" id="atiBugReport" style="font-weight: normal; font-size: smaller;">Report a bug</a>
            <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>	
            <dnn:USER runat="server" id="dnnUser"  />
            <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>	
            <dnn:LOGIN runat="server" id="dnnLogin"  />
        </div>
    </div>

    <div style="position: absolute; top: 55px; left: 19px;z-index: 99; padding-top: 9px;">
        <dnn:LOGO runat="server" id="dnnLOGO" BorderWidth="0" />
    </div>
        
    <div style="position: absolute; top: 100px; right: 0px; color: White; z-index: 99;">
        <dnn:NAV runat="server" id="dnnNAV"  ProviderName="DNNMenuNavigationProvider" IndicateChildren="false" ControlOrientation="Horizontal" CSSControl="mainMenu" CSSNodeSelectedRoot="main_dnnmenu_rootitem_selected" />
    </div>     
    
    
    <div style="position: relative; z-index: 99; top: 155px; left: 18px; background-color: #dedede; padding-top: 18px; margin-bottom: 36px;">
       <div id="atiMainRaised" class="atiMainRaised">
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
   
        </div>
        <br />
        <div id="atiLegal" style="float: left; padding-bottom: 30px; padding-top: 4px;">
            <dnn:PRIVACY runat="server" id="dnnPRIVACY" CssClass="ati_standardSpan ati_standard_lightTxt" />
        	<span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>
        	<dnn:TERMS runat="server" id="dnnTERMS" CssClass="ati_standardSpan ati_standard_lightTxt" />	
            <span class="ati_standardSpan ati_standard_lightTxt">&nbsp;&nbsp;|&nbsp;&nbsp;</span>				                        
            <dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" CssClass="ati_standardSpan ati_standard_lightTxt" />   
        </div>
    
    </div>        

</div>
</center>
<div id="atiSiteBG"></div>
  









