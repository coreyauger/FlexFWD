<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Redirect.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_Redirect Settings Design Table">   
   <tr>
        <td><asp:Label id="lProfileRedirect" runat="server" controlname="txtProfileRedirect" Text="Redirect&nbsp;Profile:" /></td>
        <td><asp:CheckBox ID="cbProfileRedirect" runat="server" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lFriendsRedirect" runat="server" controlname="txtFriendRedirect" Text="Redirect&nbsp;Friends:" /></td>
        <td><asp:CheckBox ID="cbFriendsRedirect" runat="server" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lRedirectUrl" runat="server" controlname="txtRedirectUrl" Text="Destination&nbsp;Url:" /></td>
        <td><asp:TextBox ID="txtRedirectUrl" runat="server" MaxLength="511" /></td>
   </tr>   
</table>
