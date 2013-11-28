<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Photos.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="0" cellpadding="2" border="0" summary="Settings Design Table">   
<tr>
    <td width="300"><dnn:label id="lbConfigureLogin" runat="server" controlname="bcConfigureLogin" Text="Configure As" suffix=":" /></td>
    <td>
        <asp:DropDownList ID="ddlConfigure" runat="server">
            <asp:ListItem Text="Default" Value="ConfigureDefault" />
            <asp:ListItem Text="Photo View Modal" Value="ConfigurePhotoView" />
        </asp:DropDownList>
    </td>
</tr>
</table>
