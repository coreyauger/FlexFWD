<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_HowTo.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_HowTo Settings Design Table">   
   <tr>
        <td><asp:Label id="lNumToShow" runat="server" controlname="txtNumToShow" Text="Num to Show:" /></td>
        <td><asp:TextBox ID="txtNumToShow" runat="server" MaxLength="2" /></td>
   </tr>   
</table>
