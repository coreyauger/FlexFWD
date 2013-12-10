<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Register.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="ATI_Register Settings Design Table">   
<tr>
    <td width="300"><dnn:label id="lbConfigureLogin" runat="server" controlname="bcConfigureLogin" Text="Configure As" suffix=":" /></td>
    <td>
        <asp:DropDownList ID="ddlConfigure" runat="server">
            <asp:ListItem Text="User Register" Value="ConfigureUserRegister" />
            <asp:ListItem Text="Login" Value="ConfigureLogin" />
            <asp:ListItem Text="Group Register" Value="ConfigureGroupRegister" />            
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td><dnn:label id="lblBetaMode" runat="server" controlname="cbBeta" Text="Beta Test Signup only" suffix=":" /></td>
    <td><asp:CheckBox ID="cbBeta" runat="server" /></td>
</tr>
<tr>
    <td width="300"><dnn:label id="lblLandingPage" runat="server" controlname="txtLandingPage" Text="Landing Page" suffix=":" /></td>
    <td><asp:TextBox ID="txtLandingPage" runat="server" MaxLength="511" /></td>
</tr>   
<tr>
    <td><dnn:label id="lblHideBirthday" runat="server" controlname="txtHideBirthday" Text="Hide Birthday" suffix=":" /></td>
    <td><asp:CheckBox ID="cbHideBirthday" runat="server" /></td>
</tr>
<tr>
    <td><dnn:label id="lbHideGender" runat="server" controlname="txtHideGender" Text="Hide Gender" suffix=":" /></td>
    <td><asp:CheckBox ID="cbHideGender" runat="server" /></td>
</tr>
<tr>
    <td><dnn:label id="lbHideHeight" runat="server" controlname="txtHideHeight" Text="Hide Height" suffix=":" /></td>
    <td><asp:CheckBox ID="cbHideHeight" runat="server" /></td>
</tr>
<tr>
    <td><dnn:label id="lbHideWeight" runat="server" controlname="txtHideWeight" Text="Hide Weight" suffix=":" /></td>
    <td><asp:CheckBox ID="cbHideWeight" runat="server" /></td>
</tr>
<tr>
    <td><dnn:label id="lbHideFitnessLevel" runat="server" controlname="txtHideFitnessLevel" Text="Hide Fitness Level" suffix=":" /></td>
    <td><asp:CheckBox ID="cbHideFitnessLevel" runat="server" /></td>
</tr>
</table>


<table cellspacing="0" cellpadding="2" border="0" summary="RPX Login settings">   
<tr>
    <td colspan="2">
    Notes: To use RPX login.  It must be enabled on both the "registration"/"login" pages as well an RPX reciever page that captures the "token" for "auth_onfo".
    <a href="https://rpxnow.com/overview">https://rpxnow.com/overview</a>
    </td>
</tr>
<tr>
    <td><dnn:label id="lbRpxReciever" runat="server" controlname="txtRpxReciever" Text="Configure Module as RPX Reciever" suffix=":" /></td>
    <td><asp:CheckBox ID="cbRpxReciever" runat="server" /></td>
</tr>
<tr>
    <td><dnn:label id="lbRpxReturnUrl" runat="server" controlname="txtRpxReturnUrl" Text="Rpx Return Url" suffix=":" /></td>
    <td><asp:TextBox ID="txtRpxReturnUrl" runat="server" MaxLength="256" /></td>
</tr>
<tr>
    <td><dnn:label id="lbRpxApiToken" runat="server" controlname="txtRpxApiToken" Text="Rpx API Token" suffix=":" /></td>
    <td><asp:TextBox ID="txtRpxApiToken" runat="server" MaxLength="128" /></td>
</tr>
</table>
