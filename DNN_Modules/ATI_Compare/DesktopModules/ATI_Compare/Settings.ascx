<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Compare.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="ATI_Compare Settings Design Table">   
<tr>
    <td><label><dnn:label id="lRequestsPage" runat="server" controlname="txtRequestsPage" Text="Requests Page" suffix=":" /></label></td>
    <td><asp:TextBox ID="txtRequestsPage" runat="server" MaxLength="511" /></td>
</tr>
<tr>    
    <td><label><dnn:label id="lFriendsPage" runat="server" controlname="txtFriendsPage" Text="Friends Page" suffix=":" /></label></td>
    <td><asp:TextBox ID="txtFriendsPage" runat="server" MaxLength="511" /></td>
</tr>
<tr>    
    <td><label><dnn:label id="lMessagePage" runat="server" controlname="txtMessagePage" Text="Messages Page" suffix=":" /></label></td>
    <td><asp:TextBox ID="txtMessagePage" runat="server" MaxLength="511" /></td>
</tr>
</table>
