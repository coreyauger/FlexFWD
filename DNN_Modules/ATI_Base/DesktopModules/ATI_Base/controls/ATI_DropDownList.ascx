<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_DropDownList.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_DropDownList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
 <asp:textbox id="atiTextBox" runat="server"></asp:textbox>
 <asp:Image runat="server" ID="atiTextBox_checkImg" align="absmiddle" style="visibility: hidden;" />
 <telerik:RadToolTip runat="server" RenderInPageRoot="true" Skin="Outlook"
        ID="RadToolTip1" Position="TopRight" HideEvent="FromCode" ShowEvent="FromCode"
        Animation="Fade" RelativeTo="element" TargetControlID="atiTextBox">
  <asp:Image runat="server" ID="imgError" align="absmiddle" />&nbsp;<asp:Label ID="atiLabel" runat="server" Text="" />      
</telerik:RadToolTip>