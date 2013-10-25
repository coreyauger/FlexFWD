<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Base.ViewATI_Base" CodeFile="ViewATI_Base.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="Slim" Src="~/DesktopModules/ATI_Base/controls/ATI_SlimControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="Address" Src="~/DesktopModules/ATI_Base/controls/ATI_AddressControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="BodyComposition" Src="~/DesktopModules/ATI_Base/controls/ATI_BodyComposition.ascx" %>
<%@ Register TagPrefix="ati" TagName="BodyMeasurements" Src="~/DesktopModules/ATI_Base/controls/ATI_BodyMeasurementsControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileControl.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">                    
    <script type="text/javascript" >            
        var atiStep = 0;

        function RequestStart(sender, eventArgs) {
            // do form input checks
            var pass = true;
            if (atiStep == 0) {
                pass = atiSlimControl.validate();
            } else if (atiStep == 1) {
                pass = atiAddress.validate();
            } else if (atiStep == 3) {
                pass = atiBodyComposition.validate();
            } else if (atiStep == 4) {

            }           
            if (!pass) {
                return false;
            }
           
            var loadingImage = document.getElementById('<%= RadAjaxLoadingPanel1.FindControl("Image1").ClientID %>');
            var panel1 = $get("<%=Panel1.ClientID %>");
            var loading = document.getElementById('loading');
            var atiTopMarker = document.getElementById('atiTopMarker');
            var atiBottomMarker = document.getElementById('atiBottomMarker');
            var topPos = findPos(atiTopMarker);
            var bottomPos = findPos(atiBottomMarker);
            var h = bottomPos[1] - topPos[1];
            loading.style.top = -500 + "px";
            
            loadingImage.style.top = (parseInt(panel1.style.height) / 2) + "px";
            loadingImage.style.position = "relative";
            //document.body.className = document.body.className.replace("Normal", "Wait");        // TODO: this style is not being found
        }

        function ResponseEnd(sender, eventArgs) {            
            document.body.className = document.body.className.replace("Wait", "Normal");
        }

        function AjaxResponseFail() {
            alert("FAIL");
        }

        function AjaxResponseSuccess() {
            alert('test');
            atiStep++;
            document.getElementById('atiTrack' + (atiStep - 1)).className = "atiStepTrackHi";
            var trackRem = document.getElementById('atiTrack' + (atiStep - 1));
            var imgNode = trackRem.childNodes[0];
            trackRem.removeChild(imgNode);
            document.getElementById('atiTrack' + atiStep).appendChild(imgNode);
            document.getElementById('atiStep' + (atiStep - 1)).className = "atiStepLow";
            document.getElementById('atiStep' + atiStep).className = "atiStepHi";
            document.getElementById('atiArrow' + (atiStep - 1)).className = "atiArrowLow";
            document.getElementById('atiArrow' + atiStep).className = "atiArrowHi";
            if (atiStep == 1) {
                document.getElementById('atiRegisterPanel').style.display = 'none';
                document.getElementById('atiAddressPanel').style.display = 'block';
                load();
            } else if (atiStep == 2) {
                document.getElementById('atiRegisterPanel').style.display = 'none';
                document.getElementById('atiAddressPanel').style.display = 'none';
                document.getElementById('atiBodyCompositionPanel').style.display = 'block';
            } else if (atiStep == 3) {
                document.getElementById('atiRegisterPanel').style.display = 'none';
                document.getElementById('atiAddressPanel').style.display = 'none';
                document.getElementById('atiBodyCompositionPanel').style.display = 'none';
                document.getElementById('atiProfilePanel').style.display = 'block';
            }
        }

        function showBodyMeasurements() {
            var toggle = document.getElementById('atiBodyMeasurements').style.display == 'block' ? 'none' : 'block';
            document.getElementById('atiBodyMeasurements').style.display = toggle;
        }
    </script>    
    
    
    </telerik:RadCodeBlock>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">  
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="Panel1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel1" />  
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
    <ClientEvents OnRequestStart="RequestStart" />
    <ClientEvents OnResponseEnd="ResponseEnd" />
    </telerik:RadAjaxManager>

<div id="imgPreload" style="position: absolute; top: -1000px; left: 2000px; overflow: hidden;">
    <asp:Button ID="Button1" runat="server" CssClass="ati_Form_Button2" style="margin-right: 182px;" /> 
    <asp:Image ID="Image2" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/loading2.gif" /> 
</div>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr valign="middle">
    <td id="atiTrack0" class="atiStepTrackLow"><img runat="server" id="atiStepSelect" /></td>
    <td id="atiTrack1" class="atiStepTrackLow"></td>
    <td id="atiTrack2" class="atiStepTrackLow"></td>
    <td id="atiTrack3" class="atiStepTrackLow"></td>
    <td id="aitTrack4" class="atiStepTrackLow"></td>
</tr>
<tr>
    <td colspan="4" style="height: 10px; font-size: 1px;">&nbsp;</td>
</tr>
<tr valign="top">
    <td id="atiStep0" class="atiStepHi">
        <div id="atiArrow0" class="atiArrowHi">&nbsp;<b>Register</b></div>
        <ul>
            <li>Testing</li>
            <li>Another test</li>
            <li>Last one</li>
        </ul>
    </td>
    <td id="atiStep1" class="atiStepLow">
        <div id="atiArrow1" class="atiArrowLow">&nbsp;<b>Regional</b></div>
        <ul>
            <li>Testing</li>
            <li>Another test</li>
            <li>Last one</li>
        </ul>
    </td>
    <td id="atiStep2" class="atiStepLow">
        <div id="atiArrow2" class="atiArrowLow">&nbsp;<b>Body&nbsp;Comp</b></div>      
        <ul>
            <li>Testing</li>
            <li>Another test</li>
            <li>Last one</li>
        </ul>
    </td>
    <td id="atiStep3" class="atiStepLow">
        <div id="atiArrow3" class="atiArrowLow">&nbsp;<b>Profile</b></div>    
        <ul>
            <li>Testing</li>
            <li>Another test</li>
            <li>Last one</li>
        </ul>    
    </td>
    <td id="atiStep4" class="atiStepLow" style="border: 0;">
        <div id="atiArrow4" class="atiArrowLow">&nbsp;<b>Goals/Plan</b></div>  
        <ul>
            <li>Testing</li>
            <li>Another test</li>
            <li>Last one</li>
        </ul>      
    </td>
</tr>
</table>

<h1>Registration Process</h1>
<span>Please compleate the registration process to begin.</span>

<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="80">
    <div id="loading" style="position: relative; overflow: hidden; text-align: center;">
        <table border="0" cellpadding="0" cellspacing="0">
        <tr valign="middle">
            <td align="center" style="width: 950px; height: 500px; background-color: #FFFFFF;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/loading2.gif" />
                loading ...
            </td>
        </tr>
        </table>
    </div>
</telerik:RadAjaxLoadingPanel> 

    <div id="atiTopMarker" style="width: 100%; height: 1px;"></div>
    <div id="atiRegisterPanel">
        <ati:Slim ID="atiSlimControl" runat="server" EnableValidation="false" />        
    </div>
    <div id="atiAddressPanel" style="display: none;">
        <ati:Address ID="atiAddress" runat="server" />        
    </div>
    <div id="atiBodyCompositionPanel" style="display: block;">
        <ati:BodyComposition ID="atiBodyComposition" runat="server" />
        <a href="javascript: showBodyMeasurements();">advanced</a>
        <div id="atiBodyMeasurements" style="display: none;">
        <ati:BodyMeasurements ID="atiBodyMeasurements" runat="server" /> 
        </div>       
    </div>
    <div id="atiProfilePanel" style="display: none;">
        <ati:Profile ID="atiProfile" runat="server" />
    </div>

<asp:Panel ID="Panel1" runat="server" Width="100%">    
    <asp:Label ID="lErrorText" runat="server" Visible="true" Text="" />  
    <asp:Button ID="bRegister" runat="server" Text="Next" OnClick="bRegister_Click" CssClass="ati_Form_Button" />
    <asp:Button ID="bAddress" runat="server" Text="Next" OnClick="bAddress_Click" Visible="false" CssClass="ati_Form_Button" />    
    <asp:Button ID="bBodyComposition" runat="server" Text="Next" OnClick="bBodyComposition_Click" Visible="false" CssClass="ati_Form_Button" />
    <asp:Button ID="bProfile" runat="server" Text="Next" OnClick="bProfile_Click" Visible="false" CssClass="ati_Form_Button" />
</asp:Panel>

<div id="atiBottomMarker" style="width: 100%; height: 1px;"></div>

    

         
    
               



