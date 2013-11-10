<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_CompLiveScore.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_CompLiveScore Settings Design Table">   
   <tr>
        <td><asp:Label id="lLoginDestination" runat="server" controlname="txtLandingPage" Text="Module&nbsp;Mode:" /></td>
        <td>
            <asp:DropDownList ID="ddlMode" runat="server">
                <asp:ListItem Text="Score Board" Value="Score" />
                <asp:ListItem Text="Admin" Value="Admin" />
            </asp:DropDownList>
        </td>
   </tr>    
</table>
