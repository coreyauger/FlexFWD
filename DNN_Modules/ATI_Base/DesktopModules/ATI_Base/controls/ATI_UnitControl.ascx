<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_UnitControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_UnitControl" %>

<script type="text/javascript">
    Aqufit.Page.Controls.Ati_UnitsControl = function (id, list) {
        this.id = id;
        this.list = list;
    };

    Aqufit.Page.Controls.Ati_UnitsControl.prototype = {
        getUnits: function () {
            return parseInt( $('#'+ this.list + ' option:selected').val() );
        }
    };

    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.Ati_UnitsControl('<%=this.ID %>', '<%=ddlUnitsType.ClientID %>');
    });
</script>

<asp:DropDownList ID="ddlUnitsType" runat="server">
</asp:DropDownList>
