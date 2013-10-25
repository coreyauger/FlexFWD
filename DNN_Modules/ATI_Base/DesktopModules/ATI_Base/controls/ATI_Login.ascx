<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_Login.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_Login" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<fieldset>
    <dl>
        <dt><asp:Label id="plUserName" runat="server" controlname="txtUserName" text="Email/User&nbsp;Name:" /></dt>
        <dd>
            <asp:TextBox ID="txtUserName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="127" />
            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ValidationGroup="Login" ControlToValidate="txtUserName" ErrorMessage="<a href='javascript: focusOnErrorControl(this);'>User Name</a> required!" Text="*" />
        </dd>
        <dt><asp:Label id="plPassword" runat="server" controlname="txtUserName" text="Password:" /></dt>
        <dd>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="63" TextMode="Password" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ValidationGroup="Login" ControlToValidate="txtPassword" ErrorMessage="<a href='javascript: focusOnErrorControl(this);'>Password</a> required!" Text="*" />
        </dd>        
    </dl>
</fieldset>
