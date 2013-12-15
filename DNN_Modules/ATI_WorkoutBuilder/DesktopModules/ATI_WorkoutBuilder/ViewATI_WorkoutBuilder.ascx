<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_WorkoutBuilder.ViewATI_WorkoutBuilder" CodeFile="ViewATI_WorkoutBuilder.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="ExerciseListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_ExerciseListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="TimeSpan" Src="~/DesktopModules/ATI_Base/controls/ATI_TimeSpan.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutList" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<style type="text/css">
    div.builderWrapper
    {
        padding: 9px;    
        background-color: #FFF;   
    }
    div.atiWodContainer
    {
    	border-top: 1px solid #CCC;
    	background-color: #262626;
    	padding: 5px 16px;
    }
    
    div#workoutContainer
    {
    	margin-top: 10px;
    }
    
    div.workoutSet
    {
    	padding: 4px;
    	border: 1px solid #CCCCCC;    	
    	clear: both;
    	margin-bottom: 10px;
    }
   
    button.atiMinMax,
    button.atiClone,
    button.atiDelete
    {
    	position: relative;
    	float: right;
    	top: -30px;
    }
     
    .builderWrapper > ul
    {
    	display: table;
    }
    
    .builderWrapper > ul li
    {
    	display: table-cell;
    }
    	
    
    ul li,
    div.workoutSet ul li
    {
    	list-style: none;
    	list-style: none outside none;
    	
    }
    div.workoutSet div.exerciseBox ul li
    {
    	line-style:none;       
        font-size: 12px;
        display: list-item;
    }    
    
    span.cbutton
    {
    	text-align: center;    
    	width: 100px;
    	border: 1px solid #CCC;
    	padding: 12px;
    }
   
    ul.notes
    {
    	float: right;
    	list-style: none;
    	list-style: none outside none;
    	position: relative;
    	top: -15px;
    }
    div.exerciseBox
    {
    	padding: 9px;
    	min-height: 100px;
    	position: relative;
    }
    div.exerciseBox > h2
    {
    	color: #C77405;
    	font-size: 14px;
    	position: absolute;
    	top: 0px;
    	left: 10px;
    }  
    div.exerciseBox ul li input
    {
    	width: 50px;
    }
    ul.atiExDist
    {
        position: absolute;
     	top: 20px;
     	left: 300px;	
    }
    ul.atiExRx
    {
     	position: absolute;
     	top: 20px;
     	left: 150px;
    }
    
    ul.atiExReps
    {
    	position: absolute;
    	top: 45px;
    }
     ul.atiExReps span
     {
     	margin-left: 15px;
     	font-size: 25px;
     	color: #CCCCCC;
     }
    
    ul.atiExDist li > span,
    ul.atiExRx li > span
    {
    	width: 130px;
    	display: block;
    	color:#1C94C4;
        font-weight:bold;
    }
    ul.notes 
    {
    	position: absolute;
    	top: 20px;
    	right: 10px;
    }
    
    div.exerciseBox ul li ul.notes li
    {
    	display: inherit;
    }
    ul.notes li > textarea
    {
    	height: 30px;
    	width: 200px;
    }
    div.exerciseBox ul
    {
    	display: inline-table;
    }
    div.exerciseBox ul li
    {
    	display: table-cell;
    }
    div.exerciseBox ul li.atiExReps
    {
    	vertical-align: middle;
    }
    
    button#bSave
    {
    	float: right;
    }
    
    div.setHeading button
    {
    	margin-top: 3px;
    }
    
    div.setHeading
    {
    	background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#009ad6), to(#0077b4));
	    background: -moz-linear-gradient(-90deg, #009ad6, #0077b4);
	    filter: progid:DXImageTransform.Microsoft.gradient(enabled='false',startColorstr=#009ad6, endColorstr=#0077b4);
	    margin: 0px;
	    padding: 5px;
	    position: relative;
    }
    div.setHeading h3
    {
    	font-family:Arial;
    	font-size: 14px;
    	font-weight: bold;
    	padding-left: 2px;  
    	color: #FFF;
    }
    
    div.addSpacer
    {
    	text-align: center;
    	border: 3px dashed #CCC;
    	height: 30px;
    	margin-top: 5px;
    	font-size: 14px;
    	color: #CCC;
    	font-weight: bold;
    }
    
    div.postSaveOptions
    {
    	padding: 20px 0px;
    	text-align: center;
    }
    div.postSaveOptions a:hover
    {
    	text-decoration: none;
    }





    /*   NEW WC */
    
    
    div.atiWcStep
    {
    	background-color: White;
    	min-height: 100px;
    	padding: 40px;
    	position: relative;
    	display: none;
    }
    
    div.atiWcStep lable
    {
    	font-size: 20px;
    	color: #666;
    }
    .atiBack
    {
    	position: absolute;
    	left: 20px;
    	bottom: 40px;
    }
    #bSaveWOD,
    .atiNext
    {
    	position: absolute;
    	right: 20px;
    	bottom: 40px;
    }
    
    #workoutReview
    {
    	margin: 0px auto; 
    	width: 300px;
    	padding: 20px;
    	border: 1px solid #CCC;
    	position: relative;
    }
    #workoutReview ul
    {
        margin: 5px 10px; 
        border-bottom: 1px soliid #ccc;
    }
    #rExercises ul li
    {
    	list-style: none;
    	border-bottom: 1px soliid #ccc;
    	padding: 2px 0px;
    }
    #rExercises ul li:nth-child(odd)
    {
    	background-color: #fff;
    }
    #rExercises ul li:nth-child(even)
    {
    	background-color: #eee;
    }
    
</style>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<script language="javascript" type="text/javascript">
    
    Aqufit.Page.WOD = {};
    
    // Adding some data models that we can json serialize
    Aqufit.Data.WODExercise = function(id, order){
        this.Id = id;
        this.ExcercisKey = null;
        this.Reps = null;
        this.Order = order;
        this.MenRx = 0;
        this.MenRxUnits = null;
        this.WomenRx = 0;
        this.WomenRxUnits = null;
        this.MenDist = 0;
        this.MenDistUnits = null;
        this.WomenDist = 0;
        this.WomenDistUnits = null;
        this.Notes = null;
    };
    
    Aqufit.Data.WODSet = function(id, order){
        this.Id = id;
        this.Order = order;
        this.WODExercises = [];
    };
    
    Aqufit.Data.WOD = function(id, name, type, difficulty){
        this.Id = id;
        //this.Name = name;
        //this.WODTypeKey = type;
        this.Difficulty = difficulty;
        this.WODSets = [];
    };
    // End Aqufit.Data
    
    Aqufit.Page.Events = {
        OnLoad: function () {       // This is called after the jquery ready event
            $('#atiExerciseListScriptatiExerciseList li').draggable({
                revert: 'invalid',
                helper: 'clone',
                cursor: 'move',
                opacity: 0.7
            });
            Aqufit.Page.<%=this.ID %>.setupDragDrop();
        }
    }
    
    Aqufit.Page.Actions = {
        AssignEvents: function(that){                    
            $('#workoutContainer .atiClone:last').click(function(event){
                that.cloneElement(event, this);
                event.stopPropagation();
                return false;
            }); 
            $('#workoutContainer .atiDelete:last').click(function(event){
                if( confirm("Are you sure you want to remove this set?" ) ){
                    $(this).parents('li').remove();
                    Aqufit.Page.SetCounter--;
                }
                event.stopPropagation();
                return false;               
            }); 
            $('#workoutContainer .atiMinMax').click(function(event){
                if( $(this).parent().next('ul').is(':visible') ){
                    $(this).parent().next('ul').hide("slide", { direction: "up" }, 500);
                }else{
                    $(this).parent().next('ul').show("slide", { direction: "up" }, 500);
                }
                event.stopPropagation();
                return false;
            }); 
        },
        OnClientSelectedIndexChangedEventHandler: function(sender, args) {
            var item = args.get_item();
            var val = item.get_value();    
            alert(val);    
        },
        filterExerciseList: function(filter){
            filter = filter.toLowerCase();
            var listbox = $find("<%= RadListBoxExcerciseSource.ClientID %>");
            listbox.get_items().forEach(function(item){
                if( ! item.get_text().toLowerCase().indexOf(filter) || filter == '' ){
                    item.ensureVisible();
                }
            });                   
        },
        HaveNoGroup: function(){
            Aqufit.Page.WOD.GroupKey = 0;
            Aqufit.Page.WOD.Date = null;
            $('#atiWcStep-1').hide();
            $('#atiWcStep1').show();
        }
    };

    
    Aqufit.Page.Controls.atiWorkoutBuilder = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;      
        this.numExercises = 0;        
        this.init();    
    };

    Aqufit.Page.Controls.atiWorkoutBuilder.prototype = {        
        test: function(){
            alert("my id: " + this.id );
        },        
        init: function(){
            var that = this;
            // CA - Uncomment this to allow for the sorting of set
         //   $('#workoutContainer > ul').sortable({
         //               placeholder: 'ui-state-highlight'
         //           }).disableSelection(); 
            $('.atiClone').button({ icons: { primary: 'ui-icon-copy' }}).click(function(event){
                that.cloneElement(event, this);
                event.stopPropagation();
                return false;
            });   
            $('.atiDelete').button({ icons: {primary: 'ui-icon ui-icon-close'},text: false }).click(function(event){
                if( confirm("Are you sure you want to remove this set?" ) ){
                    $(this).parents('li').remove();
                    Aqufit.Page.SetCounter--;
                }
                event.stopPropagation();
                return false;
            }); 
             $('.atiMinMax').button({ icons: {primary: 'ui-icon ui-icon-arrow-2-n-s'},text: false }).click(function(event){
                if( $(this).parent().next('ul').is(':visible') ){
                    $(this).parent().next('ul').hide("slide", { direction: "up" }, 500);
                }else{
                    $(this).parent().next('ul').show("slide", { direction: "up" }, 500);
                }
                event.stopPropagation();
                return false;
            });  
                         
        },
        cloneElement: function(event, elm){
            var that = this;
            $('#workoutContainer > ul').append('<li></li>');            
            $(elm).parent().parent('div.workoutSet:first').clone(false).appendTo('#workoutContainer > ul li:last');       
            Aqufit.Page.Actions.AssignEvents(that);             
            $('h3.setNum:last').html('Set ' + (Aqufit.Page.SetCounter++));
            that.setupDragDrop();
            event.stopPropagation();                
            return false;
        },
        setupDragDrop: function(){
            $('#workoutContainer div.workoutSet').droppable({
                accept: '#atiExerciseListScriptatiExerciseList li',
                //"activeClass: 'ui-state-highlight',
                hoverClass: 'ui-state-highlight',
                drop: function (ev, ui) {
                    var eid = ui.draggable.attr('id');
                    eid = eid.substr(eid.indexOf('-e') + 2);
                    Aqufit.Page.<%=this.ID %>.generateCrossFitExercise( $(this).children('ul'), Aqufit.Page.atiExerciseListScript.getExercise(eid));                  
                }
            });
            
            
            $('.droppable').droppable({
                accept: '#atiExerciseListScriptatiExerciseList li',
                //"activeClass: 'ui-state-highlight',
                hoverClass: 'ui-state-highlight',
                drop: function (ev, ui) {
                    var eid = ui.draggable.attr('id');
                    eid = eid.substr(eid.indexOf('-e') + 2);
                    Aqufit.Page.<%=this.ID %>.generateCrossFitExercise( $(this).children('ul'), Aqufit.Page.atiExerciseListScript.getExercise(eid));                   
                }
            });
        },
        genWeightSelect: function(css){
            return '<select class="ui-corner-all ui-widget-content atiTxtBox '+css+'"><option value="'+Aqufit.Units.UNIT_LBS+'" selected="true">lb</option><option value="'+Aqufit.Units.UNIT_KG+'">kg</option></select>';
        },
        genDistanceSelect: function(css){
            return '<select class="ui-corner-all ui-widget-content atiTxtBox '+css+'"><option value="'+Aqufit.Units.UNIT_IN+'">Inches</option><option value="'+Aqufit.Units.UNIT_M+'" selected="true">Meters</option><option value="'+Aqufit.Units.UNIT_KM+'">km</option><option value="'+Aqufit.Units.UNIT_MILES+'">Miles</option></select>';
        },        
        generateCrossFitExercise: function( $ul, e ){
            //var $wc = $(this.controlId);
            $ul.append('<li class="ui-state-default ui-corner-all">'+
                        '<div class="exerciseBox">'+
                        '<input type="hidden" class="exId" value="'+e["Id"]+'" />'+
                        '<h2>'+e["Name"] +'</h2>'+
                        '<ul class="atiExReps">'+                       
                            '<li>Reps: <input type="texbox" class="ui-corner-all ui-widget-content atiTxtBox exReps" /><span>X</span></li>'+                            
                            (e["HasWeight"] ?
                            '</ul><ul class="atiExRx">'+
                            '<li><span>Mens RxD: </span><input type="texbox" class="ui-corner-all ui-widget-content atiTxtBox exMenRx" />'+this.genWeightSelect('exMenRxUnits')+'</li>'+
                            '<li><span>Womens RxD: </span><input type="texbox" class="ui-corner-all ui-widget-content atiTxtBox exWomenRx" />'+this.genWeightSelect('exWomenRxUnits')+'</li>'+
                            '</ul>'
                            :
                            ''
                            )+                            
                            (e["HasDistance"] ?
                            '</ul><ul class="atiExDist">'+
                            '<li><span>Mens Distance: </span><input type="texbox" class="ui-corner-all ui-widget-content atiTxtBox exMenDist" />'+this.genDistanceSelect('exMenDistUnits')+'</li>'+
                            '<li><span>Womens Distance: </span><input type="texbox" class="ui-corner-all ui-widget-content atiTxtBox exWomenDist" />'+this.genDistanceSelect('exWomenDistUnits')+'</li>'+
                            '</ul>'
                            :
                            ''
                            )+
                        '</ul>'+
                        '<ul class="notes">'+
                            '<li>Notes<br /><textarea class="ui-corner-all ui-widget-content atiTxtBox exNotes"" style="height: 75px;"></textarea></li>'+
                        '</ul>'+
                        '</div></li>'
                        );
                // CA - uncomment this to allow for exercise sorting..        
//            $ul.sortable({
  //                      placeholder: 'ui-state-highlight'
    //                }).disableSelection();      
                                                
        }       
    };
    
    function OnResponseEnd(sender, args){
        Aqufit.Page.atiLoading.remove();           
    }       
    function OnRequestStart(sender, args){
                //        
    }

     Aqufit.Windows = {
        WorkoutSavedDialog : {
            win: null,
            open: function (wid, groupname) {
                if( typeof( groupname ) == 'string' && groupname != '' ){
                    $('#youSchedule').show();
                    $('#boxName').html(groupname);
                }else{
                    $('#youSchedule').hide();
                    $('#boxName').html('');
                }
                $('#aLogLink1, #aLogLink2').attr('href',Aqufit.Page.PageBase + Aqufit.Page.UserName + '?w='+wid);
                Aqufit.Windows.WorkoutSavedDialog.win = $find('<%=WorkoutSavedDialog.ClientID %>');
                Aqufit.Windows.WorkoutSavedDialog.win.show();
            },
            close: function () {
                Aqufit.Windows.WorkoutSavedDialog.win.close();
            }
        },
        WorkoutScheduleDialog : {
            win: null,
            open: function (wid) {
                Aqufit.Windows.WorkoutScheduleDialog.win = $find('<%=WorkoutScheduleDialog.ClientID %>');
                Aqufit.Windows.WorkoutScheduleDialog.win.show();
            },
            close: function () {
                Aqufit.Windows.WorkoutScheduleDialog.win.close();
            }
        },
        AddExerciseDialog : {
            win: null,
            open: function (wid) {
                Aqufit.Windows.AddExerciseDialog.win = $find('<%=AddExerciseDialog.ClientID %>');
                Aqufit.Windows.AddExerciseDialog.win.show();
            },
            close: function () {
                Aqufit.Windows.AddExerciseDialog.win.close();
            }
        }
    };

        
    Aqufit.Page.SetCounter = 2;

    
    
    $(function () {
        $('#bWcNo').button().click(function(event){
            Aqufit.Page.WOD.GroupKey = 0;
            $(this).parent().hide();
            $('#atiWcStep1').show();
            event.stopPropagation();
            return false;
        });
        $('#bWcYes').button().click(function(event){            
            var $opt = $('#<%=lbGroups.ClientID %> option:selected');
            var gid = $opt.val();
            Aqufit.Page.WOD.GroupKey = gid;
            Aqufit.Page.WOD.GroupName = $opt.html();            
            $('#atiWcStep-1').hide();
            $('#atiWcStep0').show();
            event.stopPropagation();
            return false;
        });
        $('#bAddExercise').button({icons:{primary:'ui-icon-plus'}}).click(function(event){
            $('#<%=txtExerciseName.ClientID %>').val( $('#txtExerciseFilter').val() );
            Aqufit.Windows.AddExerciseDialog.open();
            event.stopPropagation();
            return false;
        });
        $('#bCloseAndAdd').button().click(function(event){
            var listBox = $find("<%= RadListBoxExcerciseDest.ClientID %>");
            var item = new Telerik.Web.UI.RadListBoxItem();
            item.set_text($('#<%=txtExerciseName.ClientID %>').val());
            item.set_value(0);
            listBox.get_items().add(item);
            Aqufit.Windows.AddExerciseDialog.close();
            event.stopPropagation();
            return false;
        });
        $('.atiBack').button({icons:{primary:'ui-icon-triangle-1-w'}}).click(function(event){            
            var $step =$(this).parent();
            var stepId = $step.attr('id');
            var num = parseInt( stepId.substr(stepId.length-1));
            $step.hide();
            $('#atiWcStep'+(num-1)).show();
            event.stopPropagation();
            return false;
        });
        $('.atiNext').button({icons:{primary:'ui-icon-triangle-1-e'}}).click(function(event){
            var $step = $(this).parent();
            var stepId = $step.attr('id');
            if( stepId == "atiWcStep0" ){   
                Aqufit.Page.atiLoading.addLoadingOverlay('atiWcStep0');
                var datePicker = $find("<%= atiRadDatePicker.ClientID %>");
                var date = datePicker.get_selectedDate(); 
                Aqufit.Page.WOD.Date = date.toUTCString();
                Affine.WebService.StreamService.GetScheduledWorkouts(Aqufit.Page.WOD.GroupKey, date.toUTCString(), function(json){
                    var wods = eval('('+json+')');
                    if( wods.length > 0 ){
                        Aqufit.Page.atiWorkoutList.generateStreamDom(json);
                        Aqufit.Windows.WorkoutScheduleDialog.open();
                    }
                    $('#atiWcStep0').hide();
                    $('#atiWcStep1').show();
                    Aqufit.Page.atiLoading.remove();
                });                
            }else if( stepId == "atiWcStep1" ){                
                Aqufit.Page.WOD.Type = $('#<%=ddlWODType2.ClientID %> option:selected').val();
                if( Aqufit.Page.WOD.Type == Aqufit.WODTypes.AMRAP ){
                    var milli = Aqufit.Page.atiAMRAPTime2.getMilliDuration();
                    // we need to get the AMRAP Duration ... make sure it has a value.
                    if( milli <= 0 ){
                        // throw error
                        alert('Your AMRAP must have a time limit');
                        return false;
                    }
                    Aqufit.Page.WOD.AMRAPDuration = milli;
                }
                Aqufit.Page.WOD.TypeName = $('#<%=ddlWODType2.ClientID %> option:selected').html();
                $('#atiWcStep1').hide();
                $('#atiWcStep2').show();
            }else if(stepId == "atiWcStep2"){
                var listBox = $find("<%= RadListBoxExcerciseDest.ClientID %>");
                var Exercises = [];
                for( var i=0; i<listBox.get_items().get_count(); i++){
                    var item = listBox.get_items().getItem(i);
                    Exercises.push({ Name:item.get_text(), Id:item.get_value() });
                }
                Aqufit.Page.WOD.Exercises = Exercises;
                $('#atiWcStep2').hide();
                $('#atiWcStep3').show();
            }else if(stepId == "atiWcStep3"){
                var wcDescription = $('#<%=txtDescription2.ClientID %>').val();
                if( wcDescription == '' ){
                    // throw error
                    alert('You must provide a description for your workout.');
                    return false;
                }
                Aqufit.Page.WOD.Description = wcDescription;
                var wcName = $('#<%=txtWorkoutName2.ClientID %>').val();
                if( wcName == '' ){
                    alert('You must provide a name for your workout.');
                    return false;
                }
                // we need to make sure this in NOT a standard WOD name...
                Aqufit.Page.atiLoading.addLoadingOverlay('atiWcStep3');
                Affine.WebService.StreamService.WODNameCheck(wcName, function(json){
                    var ret = eval( '('+json+')' ); 
                    if( ret.Status == 'ERROR' ){
                        // error this is the same name as a standard wod..
                        alert('You can not call your workout one of the standard workout names (fran, helen, ect..)  Please select another name.');
                    }else{
                        Aqufit.Page.WOD.Name = wcName;
                    }
                    Aqufit.Page.atiLoading.remove();
                    $('#rName').html('<em>'+Aqufit.Page.WOD.Name+'</em>');
                    $('#rType').html('<em>'+Aqufit.Page.WOD.TypeName+'</em>');
                    $('#rDescription').html('<em>'+Aqufit.Page.WOD.Description+'</em>');
                    var exhtml = '<ul style="border: 1px solid #ccc;">';
                    for( var i=0; i<Aqufit.Page.WOD.Exercises.length; i++){
                        exhtml += '<li>'+Aqufit.Page.WOD.Exercises[i].Name+'</li>';
                    }
                    exhtml += '</ul>';
                    $('#rExercises').html(exhtml);
                    $('#atiWcStep3').hide();
                    $('#atiWcStep4').show();
                    if(Aqufit.Page.WOD.GroupKey > 0){
                        $('#rGroup').html('<em>'+Aqufit.Page.WOD.GroupName+'</em>');
                    }else{
                        $('#liGroup').hide();
                    }
                });
                
            }
            event.stopPropagation();
            return false;
        });
        $('.winClose').button().click(function(event){
            Aqufit.Windows.WorkoutScheduleDialog.close();
            event.stopPropagation();
            return false;
        });
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiWorkoutBuilder('<%=this.ID %>', 'workoutContainer'); 
        $(window).scroll(function () { 
                $('#scrollingExerciseTool')
				.stop()
				.animate({"marginTop": ($(window).scrollTop() + 30) + "px"}, "slow" );	
               // var position = $(that.controlId).offset();
               // alert(position.top);
            }); 

        $('#addSet').button().click(function (event) {
            $('#workoutContainer > ul').append('<div class="workoutSet droppable grad-FFF-EEE ui-corner-all"><div class="setHeading ui-corner-all"><h3 class="setNum">Set '+(Aqufit.Page.SetCounter++)+'</h3><button class="atiDelete">delete</button><button class="atiClone">clone set</button><button class="atiMinMax">min</button></div><ul></ul><div class="addSpacer">Drag and Drop Exercise Here</div></div>');
            Aqufit.Page.<%=this.ID %>.setupDragDrop();
            $('#workoutContainer .atiClone:last').button({ icons: { primary: 'ui-icon-copy' }});
            $('#workoutContainer .atiDelete:last').button({ icons: {primary: 'ui-icon ui-icon-close'},text: false });
            $('#workoutContainer .atiMinMax').button({ icons: {primary: 'ui-icon ui-icon-arrow-2-n-s'},text: false });
            Aqufit.Page.Actions.AssignEvents(Aqufit.Page.<%=this.ID %>); 
            event.stopPropagation();
            return false;
        });
        $('#<%=ddlWODType.ClientID %>, #<%=ddlWODType2.ClientID %>').change(function(){
            var wodType = $(this).find(':selected').val();
            if( wodType == Aqufit.WODTypes.AMRAP ){
                $('.atiAMRAPTime').show();
            }else{
                $('.atiAMRAPTime').hide();
            }
        });
        // Tabs        
        $('#tabs').tabs();
        $('#bSaveWOD').button( {icons:{primary:'ui-icon-disk'}} ).click(function(event){
            Aqufit.Page.atiLoading.addLoadingOverlay($(this).parent().attr('id'));
            $('#<%=hiddenAjaxAction.ClientID %>').val('SaveWOD');
            var wodJson = Aqufit.Serialize( Aqufit.Page.WOD );
            $('#<%=hiddenAjaxValue.ClientID %>').val(wodJson);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$WorkoutBuilder","ATI_WorkoutBuilder") %>',''); 
            event.stopPropagation();
            return false;          
        });
        $('.dull').focus(function () {
                    if ($(this).hasClass('dull')) {
                        $(this).removeClass('dull').val('');
                    }
                });
        $('#bSave').button( {icons:{primary:'ui-icon-disk'}} ).click(function(event){
            $(this).button( "option", "disabled", true );
            Aqufit.Page.atiLoading.addLoadingOverlay($(this).attr('id'));
        //$('#bSave').click(function(event){
            // here we have to start collecting all the data that the user dynamicly assembled.
            var wod = new Aqufit.Data.WOD(0, $('#<%=txtWorkoutName.ClientID %>').val(), 1, 1);
            $('.workoutSet').each(function(ind){
                //alert('set has ' + $(this).find('.exerciseBox').size() + ' exercises');
                var wodSet = new Aqufit.Data.WODSet(0, ind);
                $(this).find('.exerciseBox').each(function(index){
                    var $ex = $(this);
                    var newEx = new Aqufit.Data.WODExercise(0, index);
                    newEx.ExcercisKey = $ex.find('input.exId').val();
                    newEx.Reps = $ex.find('input.exReps').val();
                    newEx.MenRx = $ex.find('input.exMenRx').size() ? $ex.find('input.exMenRx').val() : -1;
                    newEx.MenRxUnits = $ex.find('.exMenRxUnits').size() ? $ex.find('.exMenRxUnits').find(':selected').val(): -1;
                    newEx.WomenRx = $ex.find('input.exWomenRx').size() ? $ex.find('input.exWomenRx').val() : -1;
                    newEx.WomenRxUnits = $ex.find('.exWomenRxUnits').size() ? $ex.find('.exWomenRxUnits').find(':selected').val(): -1;
                    newEx.MenDist = $ex.find('input.exMenDist').size() ? $ex.find('input.exMenDist').val() : -1;
                    newEx.MenDistUnits = $ex.find('.exMenDistUnits').size() ? $ex.find('.exMenDistUnits').find(':selected').val(): -1;
                    newEx.WomenDist = $ex.find('input.exWomenDist').size() ? $ex.find('input.exWomenDist').val() : -1;
                    newEx.WomenDistUnits = $ex.find('.exWomenDistUnits').size() ? $ex.find('.exWomenDistUnits').find(':selected').val() : -1;
                    newEx.Notes = $ex.find('.exNotes').val();
                    if( newEx.MenRx == '' ){
                        newEx.MenRx = 0;
                    }
                    if( newEx.WomenRx == '' ){
                        newEx.WomenRx = 0;
                    }
                    if( newEx.Reps == '' ){
                        newEx.Reps = 0;
                    }
                    if( newEx.MenDist == '' ){
                        newEx.MenDist = 0;
                    }
                    if( newEx.WomenDist == '' ){
                        newEx.WomenDist = 0;
                    }
                    wodSet.WODExercises.push( newEx );
                });
                wod.WODSets.push(wodSet);                
            });     
            var wodJson = Aqufit.Serialize( wod );
            $('#<%=atiWodJson.ClientID %>').val(wodJson);
            __doPostBack('<%=bSaveWOD.ClientID.Replace("_","$").Replace("ATI$WorkoutBuilder","ATI_WorkoutBuilder") %>',''); 
            event.stopPropagation();
            return false;           
        });
    });


    Aqufit.addLoadEvent(function () {
        if( $('#txtExerciseFilter').size() > 0 ){
            $('#txtExerciseFilter').keyup(function(event) {
                Aqufit.Page.Actions.filterExerciseList( $(this).val() );   
            });
        }
    });

</script>
</telerik:RadCodeBlock>


<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="bSaveWOD">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="bSaveWOD" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>   
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>                      
    </AjaxSettings>        
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" /> 

<ati:LoadingPanel ID="atiLoading" runat="server" />

 <asp:Panel ID="panelAjax" runat="server" >   
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel> 

 <telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" EnableShadow="true">
    <Windows>    
    <telerik:RadWindow ID="WorkoutSavedDialog" runat="server" Skin="Black" Title="Workout Saved" Width="600" Height="320" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div id="workoutStatusPanel" style="width: 100%; height: 100%; background-color: white; position: relative;">
                <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                    <h3 id="wsTitle" runat="server">Your Workout has been saved.  It will now show up in your Workout List</h3>
                    <ul class="hlist" style="position: absolute; right: 10px; top: 4px;">                                
                        <li><a id="aLogLink1" href="javascript: ;" title="Log Workout"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iWorkout_s.png") %>" /></a></li>
                    </ul>
                </div>    
                <div id="youSchedule" style="text-align: center; display: none; margin: 20px 10px;">
                    <h3><em style="color: Orange;">Success !</em> You just scheduled the workout for all your friends at <em id="boxName" style="color: Orange;"></em></h3>
                    <br />
                    <a href="<%=ResolveUrl("~/Profile/FindInvite")%>"><span class="grad-FFF-EEE ui-corner-all cbutton"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iGroup_s.png")%>" />Invite more Members</span></a>
                </div>           
                <div class="postSaveOptions">
                    <a id="aLogLink2" href="javascript: ;" title="Log Workout"><span class="grad-FFF-EEE ui-corner-all cbutton"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iWorkout_s.png")%>" /> Log this workout</span></a>
                    <a id="aMyWorkouts" runat="server" title="My Workouts"><span class="grad-FFF-EEE ui-corner-all cbutton"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iHistory_s.png")%>" /> View my workouts</span></a>                                            
                </div>
                <div class="postSaveOptions" style="float: right; padding-right: 10px;">
                    <a id="aReturn" runat="server" title="Log Workout"><span class="grad-FFF-EEE ui-corner-all cbutton"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iReply.png")%>" /> Return to profile</span></a>
                </div>
            </div>                    
        </ContentTemplate>
    </telerik:RadWindow>
     <telerik:RadWindow ID="WorkoutScheduleDialog" runat="server" Skin="Black" Title="Group Workouts" Width="600" Height="350" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div style="background-color: white;">
            <div style="width: 100%; height: 100%; background-color: white; position: relative;">
                <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                    <h3>Are you looking to log one of these workouts?</h3> 
                    <ul class="hlist" style="position: absolute; right: 10px; top: 4px;">                                
                        <li><button class="winClose">No mine is new</button></li>
                    </ul>                   
                </div>                
                <ati:WorkoutList ID="atiWorkoutList" runat="server" />
                <div class="postSaveOptions" style="float: right; padding-right: 10px;">
                    <button class="winClose">No mine is new</button>
                </div>
            </div>    
            </div>                
        </ContentTemplate>
    </telerik:RadWindow>
    <telerik:RadWindow ID="AddExerciseDialog" runat="server" Skin="Black" Title="Add New Exercise" Width="600" Height="250" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div id="Div2" style="width: 100%; height: 100%; background-color: white; position: relative;">
                <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                    <h3>Add a new Exercise for your workout</h3>                                        
                </div>    
                <div style="text-align: center; padding: 20px;">  
                    <span>Exercise Name:</span><asp:TextBox ID="txtExerciseName" runat="server" MaxLength="64" CssClass="ui-corner-all ui-widget-content atiTxtBox" />     
                </div>     
                <div class="postSaveOptions" style="float: right; padding-right: 10px;">
                    <button id="bCloseAndAdd">Done</button>
                </div>
            </div>                    
        </ContentTemplate>
    </telerik:RadWindow>

    
    </Windows>
    
    </telerik:radwindowmanager>

    <!-- Start of a 2 col box layout -->    
    <div id="divCenterWrapper" style="width: 733px; float: left;">
    
        <!-- Tabs -->
		<div id="tabs">
			<ul>
				<li><a href="#tabWorkoutBuilder">Workout Builder</a></li>               
			</ul>

			<div id="tabWorkoutBuilder" style="padding: 0px;">
                <asp:Panel ID="atiTemp" runat="server">
                <asp:Panel ID="atiBuildPanel" CssClass="builderWrapper" runat="server">
                    <asp:HiddenField ID="atiWodJson" runat="server" />
                    <h3>Workout Builder</h3>
                    <h4>Construct your workout.  Drag and Drop Exercises into sets.</h4>
                    <ul>
                        <li><asp:label id="plName" runat="server" controlname="txtWorkoutName" CssClass="ati_Form_TextLeft" text="Name:" /><br />
                        <asp:TextBox id="txtWorkoutName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox"  MaxLength="128" /></li>
                    
                        <li><asp:label id="plType" runat="server" controlname="ddlWODType" CssClass="ati_Form_TextLeft" text="Type:" /><br />
                        <asp:DropDownList id="ddlWODType" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ></asp:DropDownList></li>
                    
                        <li class="atiAMRAPTime" style="display: none;"><asp:label id="plTimeLimit" runat="server" controlname="atiTimeSpan" CssClass="ati_Form_TextLeft" text="AMRAP Time limit:" /><br />
                        <ati:TimeSpan ID="atiTimeSpan" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ShowPace="false" /></li>                                        
                    </ul>
                    <asp:label id="plDescription" runat="server" controlname="txtDescription" CssClass="ati_Form_TextLeft" text="Description:" /><br />
                    <asp:TextBox id="txtDescription" runat="server" TextMode="MultiLine" MaxLength="1023" CssClass="ui-corner-all ui-widget-content atiTxtBox" style="width: 100%; height: 75px;" />
                </asp:Panel>
                <div class="atiListHeading grad-FFF-EEE">
                    <h3>Easy Workout Creator</h3>
                </div>
                <asp:Panel ID="atiWodContainer" runat="server" CssClass="atiWodContainer">
                    <div id="workoutContainer">
                        <ul>                        
                            <li>
                                <div class="workoutSet droppable grad-FFF-EEE ui-corner-all">
                                    <div class="setHeading ui-corner-all">
                                        <h3 class="setNum">Set 1</h3><button class="atiDelete">delete</button><button class="atiClone">clone set</button><button class="atiMinMax">min</button>
                                    </div>
                                    <ul>
                                
                                    </ul>
                                    <div class="addSpacer">Drag and Drop Exercise Here</div>
                                </div>
                            </li>                                                               
                        </ul>
                    <button id="addSet">New Set</button>
                    <asp:Button ID="bSaveWOD" runat="server" OnClick="bSaveWOD_Click" style="display: none;" />
                    <button id="bSave">Save</button>
                    <br style="clear: both;" />
                    </div>
                </asp:Panel>    
                </asp:Panel>     
                <asp:Panel ID="atiNew" runat="server" Visible="false">
                    <div class="atiListHeading grad-FFF-EEE">
                        <h3>Easy Workout Creator</h3>
                    </div>
                    <div id="atiWcStep-1" class="atiWcStep" style="display: block; text-align:center; padding-top: 100px;">
                        <lable>Did you do this workout at: </lable><asp:ListBox ID="lbGroups" runat="server" SelectionMode="Single" Rows="1" CssClass="ui-corner-all ui-widget-content atiTxtBox"></asp:ListBox>
                        <button id="bWcNo">No</button><button id="bWcYes">Yes</button>
                    </div>
                    <div id="atiWcStep0" class="atiWcStep" style=" text-align:center; padding-top: 100px;">
                        <lable>What day did you do it on ? </lable><telerik:RadDatePicker CssClass="ui-corner-all ui-widget-content atiTxtBox" DateInput-BorderStyle="None" DateInput-BackColor="Transparent" ID="atiRadDatePicker" runat="server" /> 
                        <button class="atiBack">Back</button>
                        <button class="atiNext">Next</button>
                    </div>
                    <div id="atiWcStep1" class="atiWcStep" style="text-align:center; padding-top: 100px;">                        
                        <lable>Select a Workout Type: </lable>                 
                        <asp:DropDownList id="ddlWODType2" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ></asp:DropDownList>
                        <br />
                        <div class="atiAMRAPTime" style="display: none;"><lable>AMRAP Time limit:</lable><ati:TimeSpan ID="atiAMRAPTime2" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ShowPace="false" /></div>                                        
                        <button class="atiBack">Back</button>
                        <button class="atiNext">Next</button>
                    </div>
                    <div id="atiWcStep2" class="atiWcStep">
                        <lable>Add Exercises: </lable>          
                        <div style="margin-left: 100px;">
                            <ul class="hlist">
                                <li><input id="txtExerciseFilter" type="text" maxlength="32" value="filter exercises" class="dull" style="width: 220px; height: 20px;" /></li> 
                                <li><button id="bAddExercise">Add New Exercise</button></li>
                            </ul>                                   
                            <telerik:RadListBox ID="RadListBoxExcerciseSource" runat="server" Width="250px" Height="150px" AllowTransferDuplicates="false" TransferMode="Move"
                                SelectionMode="Multiple" AllowTransferOnDoubleClick="true" AllowTransfer="true" TransferToID="RadListBoxExcerciseDest" AutoPostBackOnTransfer="false"
                                AllowReorder="false" EnableDragAndDrop="true">  
                                <ButtonSettings ShowTransferAll="false" />                          
                            </telerik:RadListBox>
                            <telerik:RadListBox ID="RadListBoxExcerciseDest" runat="server" Width="250px" Height="150px" AllowDelete="true" TransferMode="Move"  AllowTransferDuplicates="false"
                                                SelectionMode="Multiple" AllowReorder="false" AutoPostBackOnReorder="false" EnableDragAndDrop="true">           
                            </telerik:RadListBox>
                        </div>
                        <button class="atiBack">Back</button>
                        <button class="atiNext">Next</button>
                    </div>
                    <div id="atiWcStep3" class="atiWcStep">
                        <div style="margin: 0px auto; width: 400px;">
                            <ul class="hlist">
                                <li><lable>Workout Name: </lable></li>
                                <li><asp:TextBox id="txtWorkoutName2" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" Text="eg: Fran"  MaxLength="128" /></li>                                
                            </ul>
                            <lable style="display: block;">Description:</lable>
                            <asp:TextBox id="txtDescription2" runat="server" TextMode="MultiLine" MaxLength="1023" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" style="width: 390px; height: 75px;">eg: For time
21 Thrusters, 21 Pullups
15 Thrusters, 15 Pullups
9 Thrusters, 9 Pullups</asp:TextBox>                                                         
                        </div>
                        <button class="atiBack">Back</button>
                        <button class="atiNext">Next</button>
                    </div>
                    <div id="atiWcStep4" class="atiWcStep">                        
                        <div id="workoutReview" class="grad-FFF-EEE rounded">                            
                            <img src="/DesktopModules/ATI_Base/resources/images/notificationType7.png" class="dropShadow" style="position:absolute; right: 20px;" >
                            <h3 style="margin-bottom: 20px; font-size: 24px; color: #3f95cd;" id="rName">Name</h3>
                            <span>Created By:</span><ati:ProfileImage small="true" id="atiProfileImg" runat="server" />  
                       
                            <ul>
                                <li id="liGroup"><span>Location:</span><span id="rGroup"></span></li>
                                <li>&nbsp;</li>
                                <li><span>Type:</span><span id="rType"></span></li>
                                <li><span>Description:</span><br /><span id="rDescription"></span></li>
                                <li>&nbsp;</li>
                                <li><span>Exercises:</span><span id="rExercises"></span></li>                      
                            </ul>
                        </div>
                        
                        <button class="atiBack">Back</button>
                        <button id="bSaveWOD">Save</button>
                    </div>
                </asp:Panel>                                                                                                                                                                        
            </div>
		</div>                                   
    
    </div>
    <div id="divLeftNav" style="width: 190px; float: right;" class="">
        
    </div>
    <div style="clear:both;"></div>   
        







    

    

         
    
               



