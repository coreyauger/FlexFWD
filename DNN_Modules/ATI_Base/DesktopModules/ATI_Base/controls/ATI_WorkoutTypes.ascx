<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WorkoutTypes.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_WorkoutTypes" %>
<script type="text/javascript">
    Aqufit.Page.Controls.Ati_WorkoutTypes = function(id, changeCallback){
        this.id = id;
        this.changeCallback = changeCallback;
    };

    Aqufit.Page.Controls.Ati_WorkoutTypes.prototype = {
        setSelected: function( sel ){
            $('#<%=panelWorkoutTypeList.ClientID %> select').val(sel);
        },
        getSelectedDropDownVal: function(){
            return $('#<%=panelWorkoutTypeList.ClientID %> select').find(':selected').val();
        },
        setupEvents: function () {
            var that = this;
            $('#<%=panelWorkoutTypeList.ClientID %> select').change(function () {
                var sel = $(this).find(':selected').val();                              
                if( typeof that.changeCallback == 'function' ){
                    that.changeCallback(sel);
                }               
            });
        }
    };
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.Ati_WorkoutTypes('<%=this.ID %>',<%=this.OnClientWorkoutTypeChanged != null ? this.OnClientWorkoutTypeChanged : "function(){}" %>);
        Aqufit.Page.<%=this.ID %>.setupEvents();
    });    
</script>
<asp:Panel ID="panelWorkoutTypeList" class="WorkoutTypeList" runat="server">
    <asp:DropDownList ID="ddlWorkoutTypeList" runat="server" Visible="true" />		    
</asp:Panel>