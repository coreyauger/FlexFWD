<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_BugReporter.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_BugReporter Settings Design Table">   
   <tr>
        <td><asp:Label id="lUserMustHaveAccount" runat="server" controlname="cbUserMustHaveAccount" Text="User Must have account (NOT IMPLEMENTED):" /></td>
        <td><asp:CheckBox ID="cbUserMustHaveAccount" runat="server" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lConfigAsContact" runat="server" controlname="cbConfigAsContact" Text="Configure as contact form:" /></td>
        <td><asp:CheckBox ID="cbConfigAsContact" runat="server" /></td>
   </tr>   
</table>
