<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StepByStep.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StepByStep" %>

<style type="text/css">

    span.stepbox
    {
    	border: 1px solid #ccc;
    	width: 112px;
    	
    	display: inline-block;
    	padding-top: 10px;
    	padding-left: 20px;
    	height: 60px;  
    	background-color: #EEE;	
    	cursor: pointer;    	
    }
    
    span.stepdone
    {
    	background-color: #cee3ce !important;
    }
    
    span.stepbox em
    {
    	color: #3a95cd;
    	
    }
    
    span.stepbox img
    {
    	position: absolute;
    	z-index: 999;
    	top: 20px;
    	left: 25px;
    }
    
    span.stepbox:hover
    {
    	border: 1px solid #fbcb09;
    	background-color: #fdf9e1;
    }
    
    span.stepbox div
    {    
    	position: absolute;
    	width: 90px;
    }
    
    img.stepNext
    {
    	position: absolute; 
    	top: 60px; 
    	margin-left: -1px;
    }
    
</style>

<script type="text/javascript">      

    Aqufit.Page.Controls.ATI_StepByStep = function (id) {
        this.id = id;
        this.onCloseCallback = null;   
        this.init();        
    }

    Aqufit.Page.Controls.ATI_StepByStep.prototype = {
        init: function () {
            var that = this;
            $('#bCloseSteps').button().click(function (event) {
                $('#stepWrap').slideUp(500, function () {
                    $(this).remove();
                });
                if( that.onCloseCallback != null ){
                    that.onCloseCallback();
                }
                event.stopPropagation();
                return false;
            });
            $('.stepbox').hover(function () {
                $(this).parent().find('img.stepNext').attr('src', '/DesktopModules/ATI_Base/resources/images/stepNextHi.png');
            }, function () {
                $(this).parent().find('img.stepNext').attr('src', '/DesktopModules/ATI_Base/resources/images/stepNext.png');
            }); 
        },
        setupDistance: function (dist) {            
        }
    };

    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_StepByStep('<%=this.ID %>');
    });    
</script>

<div id="stepWrap" style="position: relative; margin-bottom: 10px;">
<div class="profileHeading grad-FFF-EEE ui-corner-all">
    <button id="bCloseSteps" style="float: right;">X</button>
    <h2>Start Here.</h2>
    <p>Complete these steps to learn all the features this site has to offer.</p>    
</div>
<ul class="hlist steps">
    <li>
        <a id="linkStep1" runat="server">
        <span class="ui-corner-all stepbox">
        <em>1. Find Friends</em>
        <div>Find Athletes and Friends. <img id="imgStep1Check" runat="server" visible="false"  src="/DesktopModules/ATI_Base/resources/images/iCheck.png" /></div>
        </span><img class="stepNext" src="/DesktopModules/ATI_Base/resources/images/stepNext.png" />
        </a>
    </li>    
    <li>
        <a id="linkStep2" runat="server">
        <span class="ui-corner-all stepbox stepbox">
        <em>2. Join Groups</em>
        <div>Join the community and achieve results.<img id="imgStep2Check" runat="server" visible="false"  src="/DesktopModules/ATI_Base/resources/images/iCheck.png" /></div>
        </span><img src="/DesktopModules/ATI_Base/resources/images/stepNext.png" class="stepNext" />
        </a>
    </li>
    <li>
        <a id="linkStep3" runat="server">
        <span class="ui-corner-all stepbox">
        <em>3. Log Workouts</em>
        <div>See how to log and track all workouts.<img id="imgStep3Check" runat="server" visible="false"  src="/DesktopModules/ATI_Base/resources/images/iCheck.png" /></div>
        </span><img src="/DesktopModules/ATI_Base/resources/images/stepNext.png" class="stepNext" /></a></li>
    <li>
        <a id="linkStep4" runat="server">
        <span class="ui-corner-all stepbox">
        <em>4. View Stats</em>
        <div>Monitor your progress. Compare.<img id="imgStep4Check" runat="server" visible="false"  src="/DesktopModules/ATI_Base/resources/images/iCheck.png" /></div>
        </span>
        </a>
    </li>   
</ul>
</div>