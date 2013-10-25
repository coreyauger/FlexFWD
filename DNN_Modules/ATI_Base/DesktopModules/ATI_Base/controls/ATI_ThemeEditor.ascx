<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ThemeEditor.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ThemeEditor" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<dl>
    <dt><asp:Label ID="lBackgroundImage" runat="server" Text="Background Image" /></dt>
    <dd><asp:FileUpload ID="fileUpload" runat="server" /></dd>

    <dt><asp:CheckBox ID="cbTileBackground" runat="server" />&nbsp;<asp:Label ID="Label1" runat="server"  Text="Tile background" /></dt>
    <dd></dd>

    <dt><span style="display: inline-block;"><telerik:RadColorPicker ToolTip="Background Color" runat="server" ID="RadColorPicker1" PaletteModes="HSV" ShowEmptyColor="true" ShowIcon="true" CssClass="ColorPickerPreview"></telerik:RadColorPicker>&nbsp;Background Color</span></dt>
    <dd></dd>
</dl>
