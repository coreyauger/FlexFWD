<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Base.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="ATI_Base Settings Design Table" style="height: 97px">
   <tr>
   <td><dnn:label id="lblProductId" runat="server" controlname="txtProductId" suffix=":" /></td>
   <td><asp:TextBox ID="txtProductId" runat="server" MaxLength="31" /></td>
   </tr>
   <tr>
   <td><dnn:label id="lblCartUrl" runat="server" controlname="txtCartUrl" suffix=":" /></td>
   <td><asp:TextBox ID="txtCartUrl" runat="server" MaxLength="511" /></td>
   </tr>
   
   <tr>
   <td><dnn:label id="lblLandingPage" runat="server" controlname="txtLandingPage" suffix=":" /></td>
   <td><asp:TextBox ID="txtLandingPage" runat="server" MaxLength="511" /></td>
   </tr>
</table>
