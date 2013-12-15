<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_WorkoutBuilder.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_WorkoutBuilder Settings Design Table">   
   <tr>
        <td><asp:Label id="lLoginDestination" runat="server" controlname="txtLandingPage" Text="Investor&nbsp;Login&nbsp;Dest:" /></td>
        <td><asp:TextBox ID="txtLandingPage" runat="server" MaxLength="511" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lBlogUrl" runat="server" controlname="txtLandingPage" Text="Blog&nbsp;Url:" /></td>
        <td><asp:TextBox ID="txtBlogUrl" runat="server" MaxLength="511" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lSalesPhone" runat="server" controlname="txtSalesPhone" Text="Sales&nbsp;Phone:" /></td>
        <td><asp:TextBox ID="txtSalesPhone" runat="server" MaxLength="32" /></td>
   </tr>  
</table>
