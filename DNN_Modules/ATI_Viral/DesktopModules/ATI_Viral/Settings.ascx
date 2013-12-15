<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Viral.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_Viral Settings Design Table">   
   <tr>
        <td><asp:Label id="lModuleMode" runat="server" controlname="txtModuleMode" Text="Module&nbsp;Mode:" /></td>
        <td>
            <asp:ListBox ID="lbModuleMode" runat="server" SelectionMode="Single" Rows="1" AutoPostBack="false">
                <asp:ListItem Text="Normal" Value="Normal" />
                <asp:ListItem Text="Modal" Value="Modal" />                        
            </asp:ListBox>
        </td>
   </tr>   
</table>
