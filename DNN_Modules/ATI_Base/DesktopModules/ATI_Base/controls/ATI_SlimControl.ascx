<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_SlimControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_SlimControl" %>
    
<script type="text/javascript">    

    $(function () {        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiSlimForm('<%=this.ID %>','<%=atiTxtPostal.ClientID%>','<%=atiAddress.ClientID %>','<%=atiLocLat.ClientID %>','<%=atiLocLng.ClientID %>' );
        Aqufit.Page.<%=this.ID %>.gmapLoad();
        $('#<%=atiTxtPostal.ClientID%>').change(function (event) {
            Aqufit.Page.<%=this.ID %>.storeLatLngHome();
            event.stopPropagation();
        }).focusout(function (event) {
            Aqufit.Page.<%=this.ID %>.storeLatLngHome();
            event.stopPropagation();
        });               
    });       
    
</script>


<asp:HiddenField ID="atiLocLat" runat="server" />
<asp:HiddenField ID="atiLocLng" runat="server" />
<asp:HiddenField ID="atiAddress" runat="server" />
<dl>
    <dt id="dtFirstName" runat="server" Visible="false"><asp:Label id="plFirstName" runat="server" controlname="txtFirstName" text="First&nbsp;Name:" /></dt>
    <dd id="ddFirstName" runat="server" Visible="false">
        <asp:TextBox ID="atiTxtFirstName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="63" />
        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtFirstName" ErrorMessage="<a href='javascript: focusOnErrorControl(this);'>First Name</a> required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
    </dd>
    <dt id="dtLastName" runat="server" Visible="false"><asp:Label id="plLastName" runat="server" controlname="txtLastName" text="Last&nbsp;Name:" /></dt>
    <dd id="ddLastName" runat="server" Visible="false">
        <asp:TextBox ID="atiTxtLastName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="63" />
        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtLastName" ErrorMessage="Last Name required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
    </dd>
    <dt id="dtFullName" runat="server"><asp:Label id="plFullName" runat="server" controlname="txtFullName" text="Full&nbsp;Name:" /></dt>
    <dd id="ddFullName" runat="server" style="margin: 0; padding: 0;">
        <asp:TextBox ID="atiTxtFullName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="127" Text="eg: Harry Potter" />
        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtFullName" ErrorMessage="Your Name required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
    </dd>
    <dt id="dtPostal" runat="server"><asp:Label id="plPostal" runat="server" controlname="txtPostal" text="Zip/Postal:" /></dt>	
	<dd id="ddPostal" runat="server">
        <asp:TextBox ID="atiTxtPostal" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="16" />
	    <asp:RequiredFieldValidator ID="rfvPostal" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtPostal" ErrorMessage="Zip/Postal required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
    </dd>
    <dt id="dtEmail" runat="server"><asp:Label id="plEmail" runat="server" controlname="txtEmail" text="Email&nbsp;Address:" /></dt>	
    <dd id="ddEmail" runat="server">
        <asp:TextBox ID="atiTxtEmail" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="128" Text="eg: harry.potter@gmail.com" />
        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtEmail" ErrorMessage="Email is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
        <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtEmail" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
        <!-- ^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$ -->   
    </dd>    
</dl>