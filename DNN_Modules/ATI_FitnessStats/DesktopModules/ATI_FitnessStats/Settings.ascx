<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_FitnessStats.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="ATI_FitnessStats Settings Design Table">   
<tr>
    <td width="300"><dnn:label id="lbConfigureStats" runat="server" controlname="ddlConfigure" Text="Configure As" suffix=":" /></td>
    <td>
        <asp:DropDownList ID="ddlConfigure" runat="server">
            <asp:ListItem Text="Workout Stats" Value="ConfigureStats" />
            <asp:ListItem Text="Workout Lists" Value="ConfigureLists" />
        </asp:DropDownList>
    </td>
</tr>
</table>
