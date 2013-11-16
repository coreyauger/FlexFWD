<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Friends.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_Friends Settings Design Table">   
   <tr>
        <td><asp:Label id="lModuleMode" runat="server" controlname="txtModuleMode" Text="Module&nbsp;Mode:" /></td>
        <td>
            <asp:ListBox ID="lbModuleMode" runat="server" SelectionMode="Single" Rows="1" AutoPostBack="false">
                <asp:ListItem Text="Friend" Value="Friend" />
                <asp:ListItem Text="Follow" Value="Follow" />
                <asp:ListItem Text="Friend and Follow" Value="FriendFollow" />             
            </asp:ListBox>
        </td>
   </tr>
   <tr>
        <td><asp:Label id="lProfileMode" runat="server" controlname="txtProfileMode" Text="Profile Mode:" /></td>
        <td>
            <asp:ListBox ID="lbProfileMode" runat="server" SelectionMode="Single" Rows="1" AutoPostBack="false">
                <asp:ListItem Text="Small" Value="Small" />
                <asp:ListItem Text="Large" Value="Large" />    
                <asp:ListItem Text="None" Value="None" />            
            </asp:ListBox>
        </td>
   </tr>
   <tr>
        <td><asp:Label id="lShowInvite" runat="server" controlname="txtProfileMode" Text="Show Invite Tab:" /></td>
        <td>
            <asp:ListBox ID="lbShowInvite" runat="server" SelectionMode="Single" Rows="1" AutoPostBack="false">
                <asp:ListItem Text="Yes" Value="Yes" />
                <asp:ListItem Text="No" Value="No" />                             
            </asp:ListBox>
        </td>
   </tr>
</table>
