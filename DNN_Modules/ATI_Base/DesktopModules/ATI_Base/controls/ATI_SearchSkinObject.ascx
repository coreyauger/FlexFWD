<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_SearchSkinObject.ascx.cs" Inherits="Affine.Web.Controls.ATI_SearchSkinObject" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">

    function atiRadComboBoxSearch_OnClientSelectedIndexChangedEventHandler(sender, args) {
        var item = args.get_item();
        var json = eval('(' + item.get_value() + ')');
        if (json.Type == "USER") {
            top.location.href = '<%=ResolveUrl("~/") %>' + json.Val;
        } else if (json.Type == "GROUP") {
            top.location.href = '<%=ResolveUrl("~/") %>group/' + json.Val;
        } else if (json.Type == "WORKOUT") {
            // find out the workout type
            var wtype = json.WType;
            if (wtype == Aqufit.WorkoutTypes.CROSSFIT) {
                top.location.href = '<%=ResolveUrl("~/") %>workout/' + json.WODKey;
            } else {    // running, rowing, cycling, swimming ..
                top.location.href = '<%=ResolveUrl("~/") %>' + json.User + '/Workout/' + json.Val;
            } 
        }
    }

    function atiRadComboBoxSearch_OnClientItemsRequesting(sender, eventArgs) {
        var context = eventArgs.get_context();
        context["UserSettingsId"] = Aqufit.Page.UserSettingsId;
    }

</script>

 <telerik:RadComboBox ID="atiRadComboBoxSearch" runat="server" Width="220px" Height="340px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                    EmptyMessage="Search (Users, Groups)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true" OnClientItemsRequesting="atiRadComboBoxSearch_OnClientItemsRequesting"
                    OnClientSelectedIndexChanged="atiRadComboBoxSearch_OnClientSelectedIndexChangedEventHandler">
                    <WebServiceSettings Method="GetFlexFWDSearch" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
</telerik:RadComboBox>