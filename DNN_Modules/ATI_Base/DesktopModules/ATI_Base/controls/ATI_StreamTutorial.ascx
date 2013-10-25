<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamTutorial.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamTutorial" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">

    div.streamTutorial
    {
    	width: 557px;
    	height: 267px;
    	background: #CDCDCD url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/streamTutorial.png")%>) no-repeat top left;	
    	position: relative; 
    	margin-bottom: 10px;
    	border: 3px dashed #3a95cd;
    }
    
    div.tutAction
    {
    	position: absolute;
    	border: 2px dashed #3a95cd;
    }
    
    div#tutPic
    {    	
    	top: 17px;
    	left: 17px;
    	width: 60px;
    	height: 60px;
    	
    }
    div#tutDetails
    {
    	top: 4px;
    	left: 86px;
    	width: 257px;
    	height: 90px;
    }
    div#tutScore
    {
    	top: 10px;
    	left: 355px;
    	width: 180px;
    	height: 30px;
    }
    
    div#tutControls
    {
    	top: 72px;
    	left: 375px;
    	width: 170px;
    	height: 30px;
    }
    
    div#tutComments
    {
    	top: 110px;
    	left: 75px;
    	width: 470px;
    	height: 140px;
    }
    
</style>

<script type="text/javascript">

    function showToolTip(element) {
        var tooltipManager = $find("<%= RadToolTipManager1.ClientID %>");

        //If the user hovers the image before the page has loaded, there is no manager created
        if (!tooltipManager) return;

        //Find the tooltip for this element if it has been created
        var tooltip = tooltipManager.getToolTipByElement(element);

        //Create a tooltip if no tooltip exists for such element
        if (!tooltip) {
            tooltip = tooltipManager.createToolTip(element);
            tooltip.set_value($(element).attr('id'));
        }

        //Let the tooltip's own show mechanism take over from here - execute the onmouseover just once
        element.onmouseover = null;

        //show the tooltip
        tooltip.show();
    }

    Aqufit.Page.Controls.ATI_StreamTutorial = function (id) {
        this.id = id;
        this.onCloseCallback = null;
        this.init();
    }

    Aqufit.Page.Controls.ATI_StreamTutorial.prototype = {
        init: function () {
            var that = this;
            $('#bCloseTutorial').button().click(function (event) {
                $('#tutWrap').slideUp(500, function () {
                    $(this).remove();
                });
                if( that.onCloseCallback ){
                    that.onCloseCallback();
                }
                event.stopPropagation();
                return false;
            });
            $('.tutAction').hover(function () {
                showToolTip(this);
            }, function () {

            });  
        }        
    };

    

    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_StreamTutorial('<%=this.ID %>');
    });    
</script>
</telerik:RadCodeBlock>
<telerik:RadToolTipManager ID="RadToolTipManager1" EnableShadow="true" Width="400px" Height="100px"
        HideEvent="LeaveTargetAndToolTip" RelativeTo="Element" Position="TopCenter" runat="server" Animation="Fade"
        IgnoreAltAttribute="true">
        <WebServiceSettings Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" Method="getTutorialToolTip" />       
    </telerik:RadToolTipManager>
<div id="tutWrap">
    <button id="bCloseTutorial" style="float: right;">X</button>
    <h1>Stream Tutorial <span style="font-size: 12px;">Hover your mouse over the example stream below.</span></h1>
    <div class="streamTutorial">
        <div id="tutPic" class="tutAction"></div>
        <div id="tutDetails" class="tutAction"></div>
        <div id="tutScore" class="tutAction"></div>
        <div id="tutControls" class="tutAction"></div>
        <div id="tutComments" class="tutAction"></div>
    </div>
</div>