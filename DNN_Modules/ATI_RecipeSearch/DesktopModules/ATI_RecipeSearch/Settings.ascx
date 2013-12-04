<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_RecipeSearch.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_RecipeSearch Settings Design Table">   
   <tr>
        <td><asp:Label id="lTakeSize" runat="server" controlname="txtTakeSize" Text="Default&nbsp;Take&nbsp;Size:" /></td>
        <td><asp:TextBox ID="txtTakeSize" runat="server" MaxLength="4" /></td>
   </tr> 
   <tr>
        <td><asp:Label id="lShowSearchBar" runat="server" controlname="txtShowSearchBar" Text="Show&nbsp;Search&nbsp;Bar:" /></td>
        <td><asp:CheckBox ID="cbShowSearchBar" runat="server" /></td>
   </tr> 
</table>
