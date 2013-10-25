<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_Password.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_Password" %>
   
<dl>    
    <dt id="dtCurrentPassword" runat="server" Visible="false"><asp:Label id="plCurrentPassword" runat="server" controlname="txtPassword" text="Current Password:" /></dt>
    <dd id="ddCurrentPassword" runat="server" Visible="false">
        <asp:TextBox ID="atiTxtCurrentPassword" TextMode="Password" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="63" />
        <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtCurrentPassword" ErrorMessage="Current Password is required!" Text="*" /> 
        <asp:RegularExpressionValidator ID="revCurrentPassword" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtCurrentPassword" ValidationExpression=".{4}.*" ErrorMessage="Password min 4 characters" Text="*" />	
    </dd>
    <dt id="dtPassword" runat="server"><asp:Label id="plPassword" runat="server" controlname="txtPassword" text="Password:" /></dt>
    <dd id="ddPassword" runat="server">
        <asp:TextBox ID="atiTxtPassword" TextMode="Password" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" MaxLength="63" />
        <asp:RequiredFieldValidator ID="rfvPassword1" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtPassword" ErrorMessage="Password is required!" Text="*" /> 
        <asp:RegularExpressionValidator ID="revPassword1" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtPassword" ValidationExpression=".{4}.*" ErrorMessage="Password min 4 characters" Text="*" />	
    </dd>
    <dt id="dtConfirm" runat="server"><asp:Label id="plConfirm" runat="server" controlname="txtConfirm" text="Confirm:" /></dt>
    <dd id="ddConfirm" runat="server">
        <asp:TextBox ID="atiTxtConfirm" TextMode="Password" MinLength="5" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" />		
        <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" ValidationGroup="Slim" ControlToValidate="atiTxtConfirm" ErrorMessage="Password is required!" Text="*" /> 
        <asp:CompareValidator id="cvPassword1" runat="server" ValidationGroup="Slim" ErrorMessage="Passwords do not match!" Text="*" ControlToValidate="atiTxtConfirm" ControlToCompare="atiTxtPassword" /> 
	</dd>
</dl>