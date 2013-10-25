<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_HeightControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_HeightControl" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>

<script type="text/javascript">

    function atiHeightSetUnits(mode) {
        if (mode == 7) {    // english
            atiHeightFeet.show();
            atiHeightInches.show();           
        } else if (mode == 2 ) {    // metric
            atiHeightFeet.hide();
            atiHeightInches.show();     
        }
    }
    
</script>

<asp:TextBox ID="atiHeightFeet" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="8" />
<asp:TextBox ID="atiHeightInches" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="8" />
<asp:HiddenField ID="atiHeightMode" Value="7" runat="server" />
<ati:UnitControl id="atiHeightUnits" runat="server" CssClass="ati_Form_TextBoxSmall" />