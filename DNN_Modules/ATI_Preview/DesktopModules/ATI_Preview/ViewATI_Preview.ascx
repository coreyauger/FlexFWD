<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Preview.ViewATI_Preview" CodeFile="ViewATI_Preview.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="Preview" Src="~/DesktopModules/ATI_Base/controls/ATI_Preview.ascx" %>
<%@ Register TagPrefix="ati" TagName="Login" Src="~/DesktopModules/ATI_Base/controls/ATI_Login.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
.simpleDiv
{
	border-top: 1px solid #d9d9d9;
	font-family:  verdana, arial, sans-serif;
	padding-top: 5px;
	padding-bottom: 3px;
	
}
.simpleDiv H2
{
    font-size: 11px;
    font-weight: bold;
    color: #333333;
    text-transform: uppercase;
    padding-bottom: 0px;
}

.simpleDiv A
{
    font-size: 11px;
    font-weight: bold;
    color: #004477;   
}
.simpleDiv A:HOVER
{
    font-size: 11px;
    font-weight: bold;
    color: #004477;   
}
.topImgPad
{
	/*margin-top: -14px;*/
	position: relative;
	top: -14px;
	z-index: 100;
}
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script language="javascript" type="text/javascript">
    function RequestStart(sender, eventArgs) {
        // do form input checks        

        //var atiTopMarker = document.getElementById('atiTopMarker');
        //var atiBottomMarker = document.getElementById('atiBottomMarker');
        //var topPos = findPos(atiTopMarker);
        //var bottomPos = findPos(atiBottomMarker);
        var hiddenMode = document.getElementById('<%=hiddenMode.ClientID %>');
        if (hiddenMode.value == 'welcome') {
            if (!atiPreview.validate()) {
                return false;
            }
        }
        document.body.className = document.body.className.replace("Normal", "Wait");        // TODO: this style is not being found       

    }

    function ResponseEnd(sender, eventArgs) {
        document.body.className = document.body.className.replace("Wait", "Normal");
    }

    function DoAjaxPostback(mode) {
        var hiddenMode = document.getElementById('<%=hiddenMode.ClientID %>');
        hiddenMode.value = mode;
        __doPostBack('<%=bPostBack.ClientID.Replace("_","$").Replace("ATI$Preview","ATI_Preview") %>', '');
    }

    function AjaxResponseSuccess() {
    
    }
</script>
</telerik:RadCodeBlock>



<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax" LoadingPanelID="RadAjaxLoadingPanel1" />
                <telerik:AjaxUpdatedControl ControlID="bPostBack" />
            </UpdatedControls>            
        </telerik:AjaxSetting>
    </AjaxSettings>
    <ClientEvents OnRequestStart="RequestStart" />
    <ClientEvents OnResponseEnd="ResponseEnd" />
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />

<div id="atiTopMarker" style="width: 100%; height: 1px;"></div>
<div id="imgPreload" style="position: absolute; top: -1000px; left: -2000px; overflow: hidden;">
    <asp:Button ID="Button1" runat="server" CssClass="ati_Form_Button2" style="margin-right: 182px;" />       
</div>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
<tr valign="top">
  <td colspan="2" style="height: 30px; width: 100%; background-color: #d9d9d9; font-size: 1px;">&nbsp;</td>
</tr>
<tr valign="top">
    <td style="padding-right: 7px; padding-left: 7px; width: 700px;">        
        <asp:Panel ID="panelAjax" runat="server">
            <asp:Panel ID="panelWelcom" runat="server" Visible="true">
                <asp:Image ID="imgWelcome" runat="server" CssClass="topImgPad" /><br />               
                <asp:Literal ID="lWelcomeContent" runat="server" />
                <p><br /></p>
                <h2>Join our mailing list</h2>
                <p>Stay up to date with development.  Join our mail list or send us comments. We will be sure to get back to you.  Thank you for your interest.</p>
                <ati:Preview ID="atiPreview" runat="server" />
                                 
                <asp:Button ID="bPreview" runat="server" Text="Send" OnClick="bPreview_Click" CssClass="ati_Form_Button" style="margin-right: 182px;" />  
                <p><br /></p>
            </asp:Panel>
            
            <asp:Panel ID="panelLogin" runat="server" Visible="false">                
                <asp:Image ID="imgInvestor" runat="server" CssClass="topImgPad" /><br />
                <asp:Literal ID="lInvestorText" runat="server" />
                <p><br /></p>             
                <ati:Login ID="atiLogin" runat="server" />  
                <asp:Button ID="bLogin" runat="server" Text="Login" OnClick="bLogin_Click" CssClass="ati_Form_Button" style="margin-right: 182px;" />  
                <p><br /></p>
            </asp:Panel>          
            
            <asp:Panel ID="panelBeta" runat="server" Visible="false">
                <asp:Image ID="imgBeta" runat="server" CssClass="topImgPad" /><br />
                <asp:Literal ID="lBetaTestText" runat="server" />
                <p><br /></p>
                <ati:Preview ID="atiBetaTest" runat="server" ShowComments="false" />
                <asp:Button ID="bBetaTest" runat="server" Text="Sign Up" OnClick="bBetaTest_Click" CssClass="ati_Form_Button" style="margin-right: 182px;"  />  
                <p><br /></p>
            </asp:Panel>
            
             <asp:Panel ID="panelThankYou" runat="server" Visible="false">
                <asp:Image ID="imgThankyou" runat="server" CssClass="topImgPad" /><br />
                <asp:Literal ID="lThankYouText" runat="server" />
                <p><br /><br /><br /><br /></p>
                <table align="center" border="0" cellpadding="0" cellspacing="0" style="background-color: #9ed89e; border: 1px solid #009900;">
                <tr>
                    <td style="padding: 10px 10px 10px 10px;"><asp:Image ID="imgThankyouCheck" runat="server" /> Information sent.  Thank you for your input !</td>
                </tr>
                </table>
             </asp:Panel>
            
            <asp:HiddenField ID="hiddenMode" runat="server" Value="welcome" />
            <asp:Button ID="bPostBack" runat="server" OnClick="bPostBack_Click" CssClass="ati_Form_Button2" style="display: none;" />
        </asp:Panel>
    </td>
    <td style="width: 250px; border-bottom: 1px solid #d9d9d9; border-left: 1px solid #d9d9d9; border-right: 1px solid #d9d9d9; padding: 7px; background-color: #ededed;">
        <div class="simpleDiv" style="border-top: 0; padding-bottom: 5px;">
            <h2>More Info</h2>
            <p>Feel free to contact our team with any requests that you may have.  Also join our mail list to receive the latest updates regarding the status of development.</p>
            
            <asp:Panel ID="panelSales" runat="server" Visible="true">
                <h2>Sales and Marketing</h2>
                <asp:Image id="imgPhone" runat="server" align="middle" /> <asp:Label ID="lSalesPhone" runat="server" /><br />
               Contact our sales team for advertisment opportunities.                
            </asp:Panel>
        </div>
        <div id="divInvestor" runat="server" class="simpleDiv">
            <h2><a href="javascript: DoAjaxPostback('investor');">Investors</a></h2>            
        </div>
        <div id="divBetaTest" runat="server" class="simpleDiv">
            <h2><a href="javascript: DoAjaxPostback('beta');">Beta Test sign up</a></h2>
        </div>        
        <div id="divBlog" runat="server" class="simpleDiv">
            <h2><asp:LinkButton ID="lbBlog" runat="server" Text="Our Blog" OnClick="lbBlog_Click" /></h2>
        </div>
        
    </td>
</tr>
</table>
<div id="atiBottomMarker" style="width: 100%; height: 1px;"></div>

    

    

         
    
               



