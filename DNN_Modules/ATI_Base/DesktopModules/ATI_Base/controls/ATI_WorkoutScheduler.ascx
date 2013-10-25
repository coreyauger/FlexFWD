<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WorkoutScheduler.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_WorkoutScheduler" ClassName="ATI_WorkoutScheduler" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">   

    Aqufit.Page.Controls.atiWodSelector = {
        OnClientSelectedIndexChangedEventHandler: function (sender, args) {
            var item = args.get_item();               
            Aqufit.Page.Controls.atiWodSelector.SetupWOD(item.get_value());       
        },
        SetupWOD: function(json){            
            if( json != '' ){
                var wod = eval( '(' + json + ')' ); 
                if (wod.Id == 0) {    // User has selected the "add new WOD"
                    // TODO: this could open a Modal?
                    top.location.href = '<%=ResolveUrl("~/Profile/WorkoutBuilder") %>?g=<%=this.ProfileSettings.UserName %>';
                }
            }
        }
    };


    Aqufit.Page.Controls.ATI_WorkoutScheduler = function () {
        this.atiRadComboBoxCrossfitWorkouts = null;
    };
    
    Aqufit.Page.Controls.ATI_WorkoutScheduler.prototype = {
        
    };   

    $(function () {
        $('.dull').focus(function(){
            $(this).removeClass('dull');
        });        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_WorkoutScheduler();       
    });       
    
</script>

<asp:Panel ID="atiRunningPanel" runat="server"  Style="padding: 20px;">
    <fieldset>   
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
    <tr valign="middle">        
        <td style="width: 50%">
            <asp:label id="plDate" runat="server" controlname="txtWorkoutDate" CssClass="ati_Form_TextLeft" text="Workout Day:" /><br />
            <telerik:RadDatePicker CssClass="ui-corner-all ui-widget-content atiTxtBox" DateInput-BorderStyle="None" DateInput-BackColor="Transparent" ID="atiRadDatePicker" runat="server" />                        
        </td> 
        <td style="width: 50%;">
            <asp:label id="plHideDate" runat="server" controlname="txtWorkoutHideDate" CssClass="ati_Form_TextLeft" text="Hide Workout Details Until:" /><br />
            <telerik:RadDatePicker CssClass="ui-corner-all ui-widget-content atiTxtBox" DateInput-BorderStyle="None" DateInput-BackColor="Transparent" ID="atiRadDatePickerHide" runat="server" />
        </td>  
    </tr>  
    <tr>
        <td colspan="2">                       
            <div id="atiWodPanel" runat="server" class="cfShow">
            <asp:label id="plSelectWorkout" runat="server" controlname="atiRadComboBoxCrossfitWorkouts" CssClass="ati_Form_TextLeft" text="Select a Workout:" />&nbsp;(<a href="<%=ResolveUrl("~/Profile/WorkoutBuilder") %>?g=<%=this.ProfileSettings.UserName %>">Create a Workout</a>)
            <telerik:RadComboBox ID="atiRadComboBoxCrossfitWODs" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                EmptyMessage="Select a WOD" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                EnableVirtualScrolling="true" OnItemsRequested="RadComboBox2_ItemsRequested"
                OnClientSelectedIndexChanged="Aqufit.Page.Controls.atiWodSelector.OnClientSelectedIndexChangedEventHandler">
            </telerik:RadComboBox>            
            </div>  
        </td>     
      </tr>
      <tr>
        <td colspan="2">    
            <asp:Label ID="plHiddenName" runat="server" CssClass="ati_Form_TextLeft" text="Hidden Workout Name:" /><br />
            <asp:TextBox ID="txtHiddenName" runat="server" MaxLength="62" CssClass="ui-corner-all ui-widget-content atiTxtBox" />
        </td>  
      <tr>
        <td colspan="2">
            <asp:label id="plNote" CssClass="ati_Form_TextLeft" runat="server" controlname="txtNote" text="Additional Notes:" /><br />
            <asp:TextBox id="txtNote" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" TextMode="MultiLine" Width="100%" />
        </td>
      </tr>  
    </table>    
    </fieldset>                    
</asp:Panel>