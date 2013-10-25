<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_Workout.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_Workout" ClassName="ATI_Workout" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ati" TagName="WorkoutTypes" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutTypes.ascx" %>
<%@ Register TagPrefix="ati" TagName="IconSelector" Src="~/DesktopModules/ATI_Base/controls/ATI_IconSelector.ascx" %>
<%@ Register TagPrefix="ati" TagName="TimeSpan" Src="~/DesktopModules/ATI_Base/controls/ATI_TimeSpan.ascx" %>

<script type="text/javascript">   

    Aqufit.Windows.SyncWin = {
        win: null,
        open: function(arg){
            this.win = window.radopen('<%=ResolveUrl("~/FitnessProfile/WorkoutSync.aspx") %>?a=' + arg, null);
            this.win.set_modal(true);
            this.win.setSize(747, 500);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };        

    Aqufit.Windows.MapWin = {
        win: null,
        open: function () {
            this.win = window.radopen('<%=ResolveUrl("~/FitnessProfile/MapRoute.aspx") %>', null);
            this.win.set_modal(true);
            this.win.setSize(747, 600);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Resize + Telerik.Web.UI.WindowBehaviors.Maximize);
            this.win.center();
            this.win.show();        
            return false;
        }
    };


    Aqufit.Page.Controls.atiTxtDistance = {
        control: null
    };

    Aqufit.Page.Controls.atiDistanceUnits = {
        control: null,
        SwitchDistance: function (sender, args) {
            var ddlUnitsType = Aqufit.$(sender);
            var units = ddlUnitsType[ddlUnitsType.selectedIndex].value;
            if (units == Aqufit.Units.UNIT_KM) {   // metric
                var mi = Aqufit.Page.Controls.atiTxtDistance.control.value == '' ? 0 : parseFloat(Aqufit.Page.Controls.atiTxtDistance.control.value);
                var km = Aqufit.Units.convert(Aqufit.Units.UNIT_MILES, mi, Aqufit.Units.UNIT_KM);
                Aqufit.Page.Controls.atiTxtDistance.control.value = Aqufit.Utils.round(km, 2);
            } else if (units == Aqufit.Units.UNIT_MILES) {    // english
                var km = Aqufit.Page.Controls.atiTxtDistance.control.value == '' ? 0 : parseFloat(Aqufit.Page.Controls.atiTxtDistance.control.value);
                var mi = Aqufit.Units.convert(Aqufit.Units.UNIT_KM, km, Aqufit.Units.UNIT_MILES);
                Aqufit.Page.Controls.atiTxtDistance.control.value = Aqufit.Utils.round(mi, 2);
            }
        }
    };

    Aqufit.Page.Controls.atiRouteSelector = {
        radControl: null,
        control: null,
        OnClientSelectedIndexChangedEventHandler: function (sender, args) {
            var item = args.get_item();
            var mr = eval( '(' + item.get_value() + ')' );
            if (mr.Id == 0) {    // User has selected the "add new map"
                Aqufit.Windows.MapWin.open();
            }else{
                Aqufit.Page.Controls.atiTxtDistance.control.value = Aqufit.Utils.round( Aqufit.Units.convert(Aqufit.Units.UNIT_M, mr.Dist, Aqufit.Page.DistanceUnits), 2 );
            }
        },
        AddNewItem: function (value, txt, dist, img) {
            var comboItem = new Telerik.Web.UI.RadComboBoxItem();            
            comboItem.set_text(txt);
            comboItem.set_value('{ Id:'+value+', Dist:'+ dist +'}');
            comboItem.set_imageUrl('<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMap.png") %>');
            //the method below indicates that client-changes are to be made
            Aqufit.Page.Controls.atiRouteSelector.radControl.trackChanges();
            //add the newly created item to the Items collection of the combobox
            Aqufit.Page.Controls.atiRouteSelector.radControl.get_items().add(comboItem);
            //select the newly added item
            comboItem.select();
            //the methods below submits the client changes to the serverso that these changes are persisted after postback
            var combo = $find('<%=atiRouteSelector.ClientID%>'); 
            combo.commitChanges();

          //  Aqufit.Page.Controls.atiRouteSelector.radControl.commitChanges();
            Aqufit.Page.Controls.atiTxtDistance.control.value = Aqufit.Utils.round( Aqufit.Units.convert(Aqufit.Units.UNIT_M, dist, Aqufit.Page.DistanceUnits), 2 );
        }
    };

    Aqufit.Page.Controls.atiWodSelector = {
        OnClientSelectedIndexChangedEventHandler: function (sender, args) {
            var item = args.get_item();               
            Aqufit.Page.Controls.atiWodSelector.SetupWOD(item.get_value());       
        },
        SetSelected: function(id, name, type){
            var combo = $find("<%= atiRadComboBoxCrossfitWorkouts.ClientID %>");
            var comboItem = new Telerik.Web.UI.RadComboBoxItem();
            comboItem.set_text(name);
            var json = '{"Id":'+id+', "Type":'+type+'}';
            comboItem.set_value(json);
            combo.trackChanges();
            combo.get_items().add(comboItem);
            comboItem.select();
            combo.commitChanges();   
            Aqufit.Page.Controls.atiWodSelector.SetupWOD(json);
        },
        SetupWOD: function(json){
            if( json != '' ){
                var wod = eval( '(' + json + ')' ); 
                if (wod.Id == 0) {    // User has selected the "add new WOD"
                    // TODO: this could open a Modal?
                    top.location.href = '<%=ResolveUrl("~/Profile/WorkoutBuilder") %>';
                }else{
                    //Aqufit.Page.Controls.atiTxtDistance.control.value = Aqufit.Utils.round( Aqufit.Units.convert(Aqufit.Units.UNIT_M, mr.Dist, Aqufit.Page.DistanceUnits), 2 );
                    $('#<%= atiMaxWeightUnits.ClientID%>').hide();
                    $('#<%=atiTxtScore.ClientID %>').val('');
                    if( wod.Type == Aqufit.WODTypes.TIMED ){
                        $('#atiTimePanel').show();
                        $('#atiScorePanel').hide();
                    }else{
                        $('#atiTimePanel').hide();
                        $('#atiScorePanel').show();
                        if( wod.Type == Aqufit.WODTypes.MAXWEIGHT ){
                            // TODO: need to add a units in
                            $('#<%= atiMaxWeightUnits.ClientID%>').show();
                        }
                    }                
                }
            }
        }
    };


    Aqufit.Page.Controls.ATI_Workout = function () {
        this.workoutMode = null;
        this.atiRadComboBoxCrossfitWorkouts = null;
        this.viewScheduleClickHandler = null;
        this.ERR_NO_WORKOUT = 1;
        this.ERR_NO_SCORE = 2;
    };
    
    Aqufit.Page.Controls.ATI_Workout.prototype = {
        configureFormView: function (wt) {
            this.workoutMode = wt;
            if (wt == Aqufit.WorkoutTypes.CROSSFIT) {                
                $('.rShow').hide();
                $('.cfShow').show();
                $('#atiTimePanel').hide();
                $('#atiScorePanel').hide();
                if( !this.atiRadComboBoxCrossfitWorkouts ){
                    this.atiRadComboBoxCrossfitWorkouts = $find("<%= atiRadComboBoxCrossfitWorkouts.ClientID %>");                    
                }
                if(this.atiRadComboBoxCrossfitWorkouts ){
                    // IE does not find the control in time.. 
                    var item = this.atiRadComboBoxCrossfitWorkouts.get_selectedItem();                    
                    if( item ){
                        Aqufit.Page.Controls.atiWodSelector.SetupWOD( item.get_value() );
                    }
                }
            } else {
                $('.rShow').show();
                $('.cfShow').hide();    
                $('#atiTimePanel').show();    
                SwitchLayoutTitleRoute($('#<%=atiRouteMode.ClientID %>').val());              
            }
        },
        clear: function(){
            $('#<%=atiTxtTitle.ClientID %>').val('');
            $('#<%=atiTxtDistance.ClientID %>').val('');
            $('#<%=atiTxtScore.ClientID %>').val('');
            Aqufit.Page.atiTimeSpan.clear();
            $('#<%=txtNote.ClientID %>').val('');
            Aqufit.Page.atiFeltIconSelector.clear();
            Aqufit.Page.atiWeatherIconSelector.clear();
            Aqufit.Page.atiTerrainIconSelector.clear();
        },
        validate: function(){
            if( this.workoutMode == Aqufit.WorkoutTypes.CROSSFIT) {  
                var item = this.atiRadComboBoxCrossfitWorkouts.get_selectedItem();
                if( !item ){
                    throw this.ERR_NO_WORKOUT;              
                }
            }           
        }
    };

    function SwitchLayoutTitleRoute( mode ){
        if( mode == 'title' ){
            $('#<%=atiRouteSelector.ClientID %>').hide();
            $('#<%=atiTxtTitle.ClientID %>').show();
            $('#<%=atiRouteMode.ClientID %>').val('title');
        }else{
            $('#<%=atiRouteSelector.ClientID %>').show();
            $('#<%=atiTxtTitle.ClientID %>').hide();
            $('#<%=atiRouteMode.ClientID %>').val('route');
        }
    }

    function OnClientWorkoutTypeChanged(sel){
         Aqufit.Page.<%=this.ID %>.configureFormView(sel);           
    }

    function atiRadCombo_OnClientItemsRequesting(sender, eventArgs) {
        var context = eventArgs.get_context();
        context["UserSettingsId"] = Aqufit.Page.UserSettingsId;
    }



    $(function () {
        $('.dull').focus(function(){
            $(this).removeClass('dull');
        });
        
        Aqufit.Page.Controls.atiRouteSelector.control = Aqufit.$('<%=atiRouteSelector.ClientID%>');
        Aqufit.Page.Controls.atiDistanceUnits.control = Aqufit.$('<%=atiDistanceUnits.ClientID%>');
        Aqufit.Page.Controls.atiTxtDistance.control = Aqufit.$('<%=atiTxtDistance.ClientID%>');        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_Workout();
        $('#bGroupSchedule').button({icons:{primary:'ui-icon-calendar'}}).click(function(event){
            if( Aqufit.Page.<%=this.ID %>.viewScheduleClickHandler != null ){
                Aqufit.Page.<%=this.ID %>.viewScheduleClickHandler();
            }
            event.stopPropagation();
            return false;
        });      
    });           
    
    Aqufit.addLoadEvent(function(){
        // depending on the last workout type setup the right view
        var lastType = <%=(int)LastWorkoutType %>;
        if( lastType < 1 ){
            lastType = Aqufit.WorkoutTypes.RUNNING;
        }
        Aqufit.Page.Controls.atiRouteSelector.radControl = $find('<%=atiRouteSelector.ClientID%>');
        Aqufit.Page.atiWorkoutTypes.setSelected(lastType);
        Aqufit.Page.<%=this.ID %>.configureFormView(lastType);        
    }); 
   


    
</script>
<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" />

<div style="position: absolute; right: 100px; top: 80px; padding: 4px 7px; border: 1px solid #ccc;" class="grad-FFF-EEE ui-corner-all rShow"><a href="javascript: ;" onclick="Aqufit.Windows.SyncWin.open('Nike');" style="display: block;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iNike.png") %>" /></a></div><div style="position: absolute; right: 20px; top: 80px; padding: 5px 7px; border: 1px solid #ccc;" class="grad-FFF-EEE ui-corner-all rShow"><a href="javascript: ;" onclick="Aqufit.Windows.SyncWin.open('Garmin');"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iGarmin.png") %>" /></a></div>            
<asp:Panel ID="atiRunningPanel" runat="server" CssClass="workoutForm" Style="padding: 20px;">
    <fieldset>   
    <asp:HiddenField ID="atiRouteMode" runat="server" Value="route" />   
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
    <tr valign="middle">
        <td style="width: 50%;">
            <asp:label id="plWorkoutType" runat="server" controlname="txtWorkoutDate" CssClass="ati_Form_TextLeft" text="Workout&nbsp;Type:" /><br />
            <ati:WorkoutTypes ID="atiWorkoutTypes" runat="server" TypeDisplayMode="DROP_DOWN" CssClass="ui-corner-all ui-widget-content atiTxtBox" OnClientWorkoutTypeChanged="OnClientWorkoutTypeChanged" />           
        </td> 
        <td style="width: 50%">
            <asp:label id="plDate" runat="server" controlname="txtWorkoutDate" CssClass="ati_Form_TextLeft" text="Date:" /><br />
            <telerik:RadDatePicker CssClass="ui-corner-all ui-widget-content atiTxtBox" DateInput-BorderStyle="None" DateInput-BackColor="Transparent" ID="atiRadDatePicker" runat="server" />           
        </td>  
    </tr>  
    <tr>
        <td colspan="2">            
            <div id="atiRoutePanel" runat="server" class="rShow">
                <a href="javascript: ;" onclick="SwitchLayoutTitleRoute('route');">Select&nbsp;a&nbsp;Route&nbsp;/&nbsp;</a> <a href="javascript: ;" onclick="SwitchLayoutTitleRoute('title')" id="plWorkoutTitle" runat="server" CssClass="ati_Form_TextLeft" text="" >Title</a>
                <asp:TextBox id="atiTxtTitle" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" Style="display: none;" Width="100%" MaxLength="128" />
                <telerik:RadComboBox ID="atiRouteSelector" runat="server" Height="190px" Width="100%"
                    EmptyMessage="Select a Route" EnableLoadOnDemand="True" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                    OnItemsRequested="RadComboBox1_ItemsRequested"
                    OnClientSelectedIndexChanged="Aqufit.Page.Controls.atiRouteSelector.OnClientSelectedIndexChangedEventHandler">                    
                </telerik:RadComboBox>
            </div>
            <div id="atiWodPanel" runat="server" class="cfShow">
            <asp:label id="plSelectWorkout" runat="server" controlname="atiRadComboBoxCrossfitWorkouts" CssClass="ati_Form_TextLeft" text="Select a Workout:" />&nbsp;(<a href="<%=ResolveUrl("~/Profile/WorkoutBuilder") %>">Create a Workout</a>)<br />
            <telerik:RadComboBox ID="atiRadComboBoxCrossfitWorkouts" runat="server" Width="50%" Height="140px"
                EmptyMessage="Select a WOD" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                EnableVirtualScrolling="true" OnClientItemsRequesting="atiRadCombo_OnClientItemsRequesting"
                OnClientSelectedIndexChanged="Aqufit.Page.Controls.atiWodSelector.OnClientSelectedIndexChangedEventHandler">
                <WebServiceSettings Method="GetWorkoutsOnDemand" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
            </telerik:RadComboBox> - OR - <button id="bGroupSchedule">View Group Workout Schedule</button>          
            </div>
            
        </td>     
      </tr>  
    <tr valign="top">
        <td align="right">  
            <div id="atiDistancePanel" class="rShow">
                <asp:label id="plDistance" runat="server" controlname="txtWorkoutDistance" CssClass="ati_Form_TextLeft" text="Distance:" style="display: block;" /> 
                <asp:TextBox id="atiTxtDistance" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" />
                <ati:UnitControl id="atiDistanceUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ClientOnChange="Aqufit.Page.Controls.atiDistanceUnits.SwitchDistance" UnitType="distance" />
            </div>     
            <span style="position: relative; top: 22px; right: 20px;">
            <asp:label id="plWorkoutRxd" runat="server" controlname="cbWorkoutRx" CssClass="ati_Form_TextLeft cfShow" text="Workout&nbsp;Rx:" />
            <asp:CheckBox id="cbWorkoutRx" runat="server" Checked="true" CssClass="cfShow" />  
            </span>                          
        </td>
        <td>            
            <div id="atiScorePanel" class="cfShow">
                <asp:label id="plScore" runat="server" controlname="txtWorkoutScore" CssClass="ati_Form_TextLeft" text="Score:" /><asp:RegularExpressionValidator ID="revReal" runat="server" ControlToValidate="atiTxtScore" ValidationExpression="\d{0,}.\d{0,2}." Display="Static" SetFocusOnError="true" ErrorMessage="Must be Real number (eg: 3.14159)" Text="* Must be Real number (eg: 3.14159)" /> <br />
                <asp:TextBox id="atiTxtScore" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" />                                       
                <ati:UnitControl id="atiMaxWeightUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" />                
            </div>
            <div id="atiTimePanel">
                <ati:TimeSpan id="atiTimeSpan" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" ShowPace="true" /> 
            </div>           
        </td>
    </tr>  
    </table>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" id="atiModeTable" class="rShow">
    <tr>
        <td style="width: 33%">      
            <ati:IconSelector id="atiFeltIconSelector" runat="server" 
                        Title="How you Felt" 
                        BaseImgUrl="~/DesktopModules/ATI_Fitness/resources/images/"
                        ImgArray="iFeltGood, iFeltSoSo, iFeltBad, iFeltSick"
                        StatusArray="Good, So So, Not Good, Injured"
                        ValueArray="1,2,3,4" />
        </td>
        <td style="width: 33%">         
            <ati:IconSelector id="atiWeatherIconSelector" runat="server" 
                        Title="Weather" 
                        BaseImgUrl="~/DesktopModules/ATI_Fitness/resources/images/"
                        ImgArray="iWeatherSun, iWeatherCloud, iWeatherRain, iWeatherSnow"
                        StatusArray="Sunny, Cloudy, Rain, Snow"
                        ValueArray="1,2,3,4" />
        </td>
        <td style="width: 33%">  
           <ati:IconSelector id="atiTerrainIconSelector" runat="server" 
                        Title="Terrain" 
                        BaseImgUrl="~/DesktopModules/ATI_Fitness/resources/images/"
                        ImgArray="iTerrainRoad, iTerrainTrail, iTerrainTred, iTerrainTrack"
                        StatusArray="Road, Trail, Treadmill, Track"
                        ValueArray="1,2,3,4" />
        </td>
    </tr>
    </table>
    <asp:label id="plNote" CssClass="ati_Form_TextLeft" runat="server" controlname="txtNote" text="Note / Substitutions:" /><br />
    <asp:TextBox id="txtNote" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="512" Width="660px" />    
    </fieldset>                    
</asp:Panel>