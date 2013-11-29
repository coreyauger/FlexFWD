<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Player.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_Player Settings Design Table">   
   <tr>
        <td><asp:Label id="lModuleMode" runat="server" controlname="ddlModuleMode" Text="Module&nbsp;Mode:" /></td>
        <td><asp:DropDownList ID="ddlModuleMode" runat="server">
                <asp:ListItem Text="Profile Image" Value="PROFILE_IMG" />
                <asp:ListItem Text="Photo Album" Value="PHOTO_ALBUM" />
            </asp:DropDownList>
        </td>
   </tr>   
</table>
