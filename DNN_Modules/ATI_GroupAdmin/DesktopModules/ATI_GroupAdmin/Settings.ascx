<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_GroupAdmin.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="ATI_Register Settings Design Table">   
<tr>
    <td width="300"><dnn:label id="lbConfigureLogin" runat="server" controlname="bcConfigureLogin" Text="Configure As" suffix=":" /></td>
    <td>
        <asp:DropDownList ID="ddlConfigure" runat="server">          
            <asp:ListItem Text="Group Register" Value="ConfigureGroupRegister" />            
        </asp:DropDownList>
    </td>
</tr>
</table>
