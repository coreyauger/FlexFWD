<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Workouts.ViewATI_Workouts" CodeFile="ViewATI_Workouts.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="WorkoutListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_Profile.ascx" %>
<%@ Register TagPrefix="ati" TagName="TimeSpan" Src="~/DesktopModules/ATI_Base/controls/ATI_TimeSpan.ascx" %>
<%@ Register TagPrefix="ati" TagName="ShareLink" Src="~/DesktopModules/ATI_Base/controls/ATI_ShareLink.ascx" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="YoutubeThumbList" Src="~/DesktopModules/ATI_Base/controls/ATI_YoutubeThumbList.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutHighChart" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutHighChart.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">
<style type="text/css">           
    div.profileImage
    {
    	position: absolute; top: 60px;
    }
    
    div.profileImage h3
    {
    	margin: 5px 0px 0px 0px;
    	text-decoration: underline;
    }
    div.findPeoplePanel
    {
    	padding: 10px 0px;
    	height: 80px;
    }
     div.findPeopleByMetric
    {
    	position: absolute; 
    	left: 190px; 
    	top: 35px;
    }
    div.findPeoplePanel ul li
    {
    	list-style: none;
    	list-style: none outside none;
    }
    
    hr.controlDiv
    {
    	background-color: #fff;
    	color: #fff;
    	border: 1px solid #ccc;
    }
    
    ul.atiTopButtons
    /*button#atiRouteCreate*/
    {
    	position: absolute;
    	z-index: 999;
    	right: 6px;
    	top: 6px;
    }
    
    div#workoutInfo
    {
    
    	min-height: 40px;
    }
    div#workoutInfo h2
    {
    	font-size: 24px;
    	color: #0095CD;
    	margin: 0;
    }    
    ul.leaderBoard li
    {
    	font-size: 13px;
    	font-weight: bold;
    	list-style: none;
    	list-style: none outside none;
    }
    div#workoutAddedPanel div
    {
    	padding: 10px 20px 5px 20px;
    }
    
    div#workoutAddedPanel div span
    {
    	font-size: 16px;
    } 
    button#bCloseDialog
    {
    	position: absolute;
    	right: 25px;
    	bottom: 40px;
    }
       
    .atiMainFeed div.atiStreamItemRight
    {
	    width: 460px;
    }
   
    div.infoScroll
    {
        background-color: #FFF;
        height: 50px;
        padding: 10px;
        overflow: auto;
        border: 1px solid #ccc;
        margin-top: 70px;
    }
    span.wodSet
    {
    	display: block;
    }
    span.wodEx
    {
    	padding-left: 20px;
    }
    
    
</style>
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
<script language="javascript" type="text/javascript">        

    Aqufit.Windows.MapWin = {
        win: null,
        open: function () {
            Aqufit.Windows.MapWin.win = window.radopen('<%=ResolveUrl("~/FitnessProfile/MapRoute.aspx") %>', null);
            Aqufit.Windows.MapWin.win.set_modal(true);
            Aqufit.Windows.MapWin.win.setSize(747, 600);
            Aqufit.Windows.MapWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Resize + Telerik.Web.UI.WindowBehaviors.Maximize);
            Aqufit.Windows.MapWin.win.center();
            Aqufit.Windows.MapWin.win.show();
            return false;
        }
    };

    Aqufit.Windows.WorkoutAddedDialog = {
        win: null,
        open: function (json) {
            Aqufit.Windows.WorkoutAddedDialog.win = $find('<%=WorkoutAddedDialog.ClientID %>');
            Aqufit.Windows.WorkoutAddedDialog.win.show();
        },
        close: function () {
            Aqufit.Windows.WorkoutAddedDialog.win.close();
        }
    };


    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        exerciseArray: [],        
        toggleMapView: function () {
            var $mapContainer = $('#atiMapContainer');
            if ($mapContainer.is(':visible')) {
                $mapContainer.hide('fast');
            } else {
                $mapContainer.show('fast');
            }
        },
        gotoMyRoutes: function () {
            top.location.href = '<%=ResolveUrl("~/Profile/MyRoutes") %>';
        },
        OnWorkoutClientSelectedIndexChanged: function (sender, args) {
            var item = args.get_item();
            if (item.get_value() != '') { 
                top.location.href = Aqufit.Page.PageBase + 'workout/' + item.get_value();
            }
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
        SendSuggestedFriendRequest: function (usid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddSuggestFriend');
            $('#<%=hiddenAjaxValue.ClientID %>').val(usid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
        }
    };

    function OnResponseEnd(){
        Aqufit.Page.atiLoading.remove();
    }

    $(function () {
        $('#tabs').tabs();

        $('.dull').focus(function () {
            $(this).val('').removeClass('dull').unbind('focus');
        });
        $('button#atiWorkoutCreate').button().click(function (event) {
            if (Aqufit.Page.UserSettingsId > 0) {
                top.location.href = '<%=ResolveUrl("~/Profile/WorkoutBuilder") %>';
            } else {
                self.location.href = '<%=ResolveUrl("~/Login.aspx") %>';
            }
            event.stopPropagation();
            return false;
        });
        $('#atiWorkoutFind').button().click(function (event) {
            top.location.href = '<%=ResolveUrl("~/Community/Workouts") %>';
            event.stopPropagation();
            return false;
        });
        $('#bCloseDialog').button().click(function (event) {
            Aqufit.Windows.WorkoutAddedDialog.close();
            event.stopPropagation();
            return false;
        });
        $('#bMyWorkouts, #bMyWorkouts2').button().click(function (event) {
            top.location.href = '<%=ResolveUrl("~/Profile/MyWorkouts") %>';
            event.stopPropagation();
            return false;
        });        
        $('#bShowUserSearch').click(function (event) {
            if ($('#panelAthleteSearch').is(':visible')) {
                $('#panelAthleteSearch').hide('fast');
                $(this).html('+');
            } else {
                $('#panelAthleteSearch').show('fast');
                $(this).html('-');
            }
            event.stopPropagation();
            return false;
        });
        $('#bShowYourProgress').click(function (event) {
            if ($('#panelYourProgress').is(':visible')) {
                $('#panelYourProgress').hide('fast');
                $(this).html('+');
            } else {
                $('#panelYourProgress').show('fast');
                $(this).html('-');
           }
            event.stopPropagation();
            return false;
        });
        $('#bShowQuickView').click(function (event) {
            if ($('#panelQuickView').is(':visible')) {
                $('#panelQuickView').hide('fast');
                $(this).html('+');
            } else {
                $('#panelQuickView').show('fast');
                $(this).html('-');
           }
            event.stopPropagation();
            return false;
        });
        $('#bDoAdvancedSearch').button().click(function (event) {
            var listBox = $find('<%=RadListBoxExcerciseDestination.ClientID %>');
            var exerciseArray = [];
            for (var i = 0; i < listBox.get_items().get_count(); i++) {
                exerciseArray.push(listBox.getItem(i).get_value());
            }
            Aqufit.Page.atiLoading.addLoadingOverlay('advancedSearchPanel');
            // now we need to relaod the workout list...
            Aqufit.Page.atiWorkoutListScript.showLoading();
            var order = $('#<%=orderPopular.ClientID %>').attr('checked') ? 'popular' : 'date';
            var uid = <%if( this.IsMyWorkouts ){ %> Aqufit.Page.UserSettingsId <%}else{ %> -1 <%} %>;

            Affine.WebService.StreamService.GetWorkouts(uid, 0, 30, order, exerciseArray, function (json) {
                Aqufit.Page.atiLoading.remove();
                Aqufit.Page.atiWorkoutListScript.generateStreamDom(json);
                Aqufit.Page.hideLoading();
            }, function (err) {
                alert('error');
            });

            event.stopPropagation();
            return false;
        });
        $('#bLogWorkout, #bLogWorkout2').button().click(function (event) {
            if (Aqufit.Page.UserSettingsId > 0) {
                top.location.href = '<%=ResolveUrl("~/")%>' + Aqufit.Page.UserName + '?w=' + $('#<%=hiddenWorkoutKey.ClientID %>').val();
            } else {
                top.location.href = '<%=ResolveUrl("~/Login")%>?returnUrl=' + top.location.href;
            }
            event.stopPropagation();
            return false;
        });
        $('.workoutOrder').change(function () {
            if ($(this).is(':checked')) {
                $('#bDoAdvancedSearch').trigger('click');
            }
        });
        $('#bMoreVideos').button().click(function (event) {
            Aqufit.Windows.MediaWin.openWOD('<%=hiddenWorkoutKey.Value %>');
            event.stopPropagation();
            return false;
        });
        <% if( !string.IsNullOrWhiteSpace(hiddenWorkoutKey.Value) ){ %>
            $('#bSearchPeople').button().click(function (event) {
                var v1 = 0;
                var v2 = 9999999999;
                if ($('#<%=atiTimeSpanePanel.ClientID %>').size() > 0 ) {
                    v1 = Aqufit.Page.atiTimeSpanStart.getMilliDuration();
                    v2 = Aqufit.Page.atiTimeSpanFinish.getMilliDuration();                    
                } else if($('#<%=atiScoreRangePanel.ClientID %>').size() > 0 ){
                    var test1 = parseInt( $('#<%=atiScoreStart.ClientID %>').val() );
                    var test2 = parseInt( $('#<%=atiScoreFinish.ClientID %>').val() );
                    if( !isNaN(test1) ){
                        v1 = test1;
                    }
                    if( !isNaN(test2) ){
                        v2 = test2;
                    }
                }else if( $('#<%=atiMaxRangePanel.ClientID %>').size() > 0 ){
                    var test1a = parseInt( $('#<%=atiMaxFirst.ClientID %>').val() );
                    var test2a = parseInt( $('#<%=atiMaxLast.ClientID %>').val() );
                    if( !isNaN(test1a) ){
                        v1 = Aqufit.Units.convert(Aqufit.Page.atiMaxWeightUnitsFirst.getUnits(), test1a, Aqufit.Units.UNIT_KG);
                    }
                    if( !isNaN(test2a) ){
                        v2 = Aqufit.Units.convert(Aqufit.Page.atiMaxWeightUnitsLast.getUnits(), test2a, Aqufit.Units.UNIT_KG);
                    } 
                }
                if (v2 < v1) {  // swap..
                    var tt = v2;
                    v2 = v1;
                    v1 = tt;
                }
                // get male, female, rxd
                var male = $('#findPeopleSexM').is(':checked');
                var female = $('#findPeopleSexF').is(':checked');
                var rxd = $('#findPeopleRxs').is(':checked') ? 1 : 0;

                // stream service
                Aqufit.Page.atiEveryoneStreamScript.clearAndShowLoad();
                Affine.WebService.StreamService.getStreamDataForWOD(<%=hiddenWorkoutKey.Value %>, -1, 0, 30, male, female, rxd, v1, v2,  function (json) {
                    Aqufit.Page.atiEveryoneStreamScript.generateStreamDom(json);
                },function(err){
                    alert('error: 0x423911');
                });
                event.stopPropagation();
                return false;
            });
        <% } %>
        $('#<%=bAddWorkout.ClientID %>').css('z-index', '999').button().click(function (event) {
            if (Aqufit.Page.UserSettingsId > 0) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('addWorkout');
                $('#<%=hiddenAjaxValue.ClientID %>').val('<%=hiddenWorkoutKey.Value %>');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workout","ATI_Workout") %>', '');
                $(this).fadeOut('normal');
            } else {
                top.location.href = '<%=ResolveUrl("~/Login") %>?returnURL=' + top.location.href;
            }
            event.stopPropagation();
            return false;
        });
        $('#<%=bRemoveWorkout.ClientID %>').css('z-index', '999').button().click(function (event) {
            if (Aqufit.Page.UserSettingsId > 0) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('remWorkout');
                $('#<%=hiddenAjaxValue.ClientID %>').val('<%=hiddenWorkoutKey.Value %>');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workout","ATI_Workout") %>', '');
                $(this).fadeOut('normal');
            } else {
                top.location.href = '<%=ResolveUrl("~/Login") %>?returnURL=' + top.location.href;
            }
            event.stopPropagation();
            return false;
        });
        
        $('#bMyWorkouts').button().click(function(event){
            top.location.href = '<%=ResolveUrl("~/") %>Profile/MyWorkouts';
            event.stopPropagation();
            return false;
        });
        $('#bShareWorkout').button().click(function(event){
            alert('TODO:');
            event.stopPropagation();
            return false;
        });
        $('#bChartPageNext').button({icons:{primary:'ui-icon-circle-arrow-e'},text: false}).click(function(event){
            $('#bStreamNext2').trigger('click');
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
        if (Aqufit.Page.atiWorkoutListScript) {
            Aqufit.Page.atiWorkoutListScript.dataBinder = function (skip, take) {
                Aqufit.Page.atiWorkoutListScript.showLoading();
                var order = $('#<%=orderPopular.ClientID %>').attr('checked') ? 'popular' : 'date';
                var uid = <%if( this.IsMyWorkouts ){ %> Aqufit.Page.UserSettingsId <%}else{ %> -1 <%} %>;
                Affine.WebService.StreamService.GetWorkouts(uid, skip, take, order, [], function (json) {
                    Aqufit.Page.atiWorkoutListScript.generateStreamDom(json);
                    Aqufit.Page.atiWorkoutListScript.hideLoading();
                }, function (err) {
                    alert(err);
                });                
            };
            Aqufit.Page.atiWorkoutListScript.addWorkoutToFavCallback = function (id) {
                if (Aqufit.Page.UserSettingsId > 0) {
                    $('#<%=hiddenAjaxAction.ClientID %>').val('addWorkout');
                    $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
                } else {
                    self.location.href = '<%=ResolveUrl("~/Login") %>?returnUrl=' + self.location.href;
                }
            };
            Aqufit.Page.atiWorkoutListScript.remWorkoutToFavCallback = function (id, sel) {
                if (Aqufit.Page.UserSettingsId > 0) {
                    if (confirm("Are you sure you want to remove this workout?")) {
                        $('#<%=hiddenAjaxAction.ClientID %>').val('remWorkout');
                        $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                        __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
                        $(sel).hide("slow").children().remove();
                    }

                } else {
                    self.location.href = '<%=ResolveUrl("~/Login") %>?returnUrl=' + self.location.href;
                }
            };
        }
        if( Aqufit.Page.atiYouStreamScript ){
            Aqufit.Page.atiYouStreamScript.streamDeleteCallback = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delStream');
                $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
            };
            Aqufit.Page.atiYouStreamScript.onDeleteComment = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delComment');
                $('#<%=hiddenAjaxValue.ClientID %>').val(id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
            };
        }
        if( Aqufit.Page.atiEveryoneStreamScript ){
            Aqufit.Page.atiEveryoneStreamScript.streamDeleteCallback = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delStream');
                $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
            };
            Aqufit.Page.atiEveryoneStreamScript.onDeleteComment = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delComment');
                $('#<%=hiddenAjaxValue.ClientID %>').val(id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Workouts","ATI_Workouts") %>', '');
            };
            Aqufit.Page.atiEveryoneStreamScript.onNeedData = function(skip, take){
                 Affine.WebService.StreamService.getStreamDataForWOD($('#<%=hiddenWorkoutKey.ClientID %>').val(), -1, skip, take, true, true, -1, -1, -1, function(json){
                   // alert(json);
                    Aqufit.Page.atiWorkoutHighChart.fromStreamData(json);
                    Aqufit.Page.atiWorkoutHighChart.drawChart();
                    Aqufit.Page.atiEveryoneStreamScript.generateStreamDom(json);
                 }, function(err){});
            }
        }
    });
    
</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>    
        </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   



<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" EnableShadow="true">
    <Windows>    
        <telerik:RadWindow ID="WorkoutAddedDialog" runat="server" Skin="Black" Title="Workout Added" Width="400" Height="200" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
                <ContentTemplate>
                    <div id="routeAddedPanel" style="width: 100%; height: 100%; background-color: white;">
                        <div>
                            <span><asp:Image ID="imgCheck" runat="server" />Workout has been added.</span>
                            <ul class="hlist">
                                <li><button id="bMyWorkouts">view my workouts</button></li>
                                <li><button id="bLogWorkout2">Log Workout</button></li>
                                <li><button id="bShareWorkout">share workout</button></li>
                            </ul>                               
                            <button id="bCloseDialog">Close</button>                          
                        </div>
                    </div>
                </ContentTemplate>
        </telerik:RadWindow>
        </Windows>
</telerik:radwindowmanager>

<ati:LoadingPanel ID="atiLoading" runat="server" />


 <asp:Panel ID="panelAjax" runat="server" >   
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenWorkoutKey" runat="server" />   
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>    
    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" style="position: relative;">
        <div id="ati_sidePanel" style="height: 100%; float: left;">        
            <!-- ** Left Panel Zone -->          
            <ati:Profile ID="atiProfile" runat="server" />                               
            <div id="divMainLinks" runat="server" style="margin-top: 10px;" visible="false">                            
                <ul class="linksList">
                    <li runat="server" id="liWorkout"><a id="hlLogWorkout" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iWorkout_s.png")%>" /> Log this Workout</a></li> 
                    <li runat="server" id="liEdit2" style="border-bottom: 0;">&nbsp;</li>
                    <li runat="server" id="liStats"><a href="" id="hlGraph" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iStats_s.png")%>" /> <asp:Literal ID="litStats" runat="server" Text="Workout Graph" /></a></li> 
                    <li runat="server" id="liWorkoutHistory"><a id="hlWorkouts" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iHistory_s.png")%>" /> Workout History</a></li>                  
                    <li runat="server" id="liCreateWOD"><a id="hlCreateWOD" runat="server"> Create New Workout</a></li>   
                    <li runat="server" id="liMyWorkouts2"><a runat="server" id="hlMyWorkouts"> My Workouts</a></li>
                </ul>                            
            </div>
            <!-- ** END Left Panel Zone -->                           
        </div>
        <div id="divMain" style="width: 554px; float: right; margin-right: 175px; position: relative; min-height: 1300px;">            
            <!-- Tabs -->
            <ul class="hlist atiTopButtons">                
                <li id="liFindWorkout" runat="server" visible="false"><button id="atiWorkoutFind">Find Workout</button></li>
                <li id="liMyWorkouts" runat="server"><button id="bMyWorkouts2">My Workouts</button></li>               
                <li><button id="atiWorkoutCreate">Create a Workout</button></li>
            </ul>
    		<div id="tabs">
    			<ul>
    				<li id="tabInbox"><a href="#pageViewWorkouts"><asp:Literal ID="workoutTabTitle" runat="server" /></a></li>    				              
                </ul>
    			<div id="pageViewWorkouts" style="padding: 0px; background-color: White; _clear:both;">                                                                                                        
                    <asp:Panel ID="atiWorkoutPanel" runat="server">                        
                        <div id="atiWorkoutSearch" runat="server">
                            <div class="atiSearchControls grad-FFF-EEE">                            
                                <telerik:RadComboBox ID="atiRadComboBoxSearchWorkouts" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                    EmptyMessage="Search Workouts by Name" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                    EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchWorkouts_ItemsRequested"
                                    OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnWorkoutClientSelectedIndexChanged">
                                </telerik:RadComboBox>                                                          
                            </div>
                            <div id="advancedSearchPanel" class="atiSearchControls grad-FFF-EEE" style="position: relative; display: block;">                                    
                                <span style="font-weight:bold; display: block;">Search for workouts containing exercises.</span><br />
                                <ul class="hlist">
                                    <li><span style="padding-right: 200px;">All Exercises</span></li>
                                    <li>Wanted Exercises</li>
                                </ul>
                                    <input id="txtExerciseFilter" type="text" maxlength="32" value="filter exercises" class="dull" style="display: block; width: 170px; height: 20px;" />
                                    <telerik:RadListBox
                                        runat="server" ID="RadListBoxExcerciseSource"
                                        Height="100px" Width="200px" TransferMode="Move" DataTextField="Text" DataValueField="Value"
                                        AllowTransfer="true" TransferToID="RadListBoxExcerciseDestination" AllowDelete="false" AllowReorder="false" ButtonSettings-ShowTransferAll="false">                                      
                                    </telerik:RadListBox>
                                       
                                    <telerik:RadListBox
                                        runat="server" ID="RadListBoxExcerciseDestination" DataTextField="Text" DataValueField="Value"
                                        Height="100px" Width="200px" AllowTransferDuplicates="false" AllowDelete="true" AllowReorder="false">
                                    </telerik:RadListBox>
                                <button id="bDoAdvancedSearch" style="position: absolute; right: 16px; bottom: 16px;">Search</button>
                            </div>
                        </div>
                        <div id="orderByPanel" class="atiSearchControls grad-FFF-EEE" style="position: relative; display: block;">   
                            <span>Order results by:</span>
                            <ul class="hlist" style="position: absolute; right: 16px; bottom: 10px;" >
                                <li>Popularity <input type="radio" name="workoutOrder" runat="server" id="orderPopular" value="popular" checked="true" class="workoutOrder" /></li>
                                <li>&nbsp;</li>
                                <li>Creation Date <input type="radio" name="workoutOrder" runat="server" id="orderDate" value="date" class="workoutOrder" /></li>
                            </ul>
                        </div>
                        <ati:WorkoutListScript id="atiWorkoutListScript" runat="server" />
                    </asp:Panel> 


                    <asp:Panel ID="atiWorkoutViewer" runat="server" Visible="false"> 
                        <div class="atiControlPanel grad-FFF-EEE">
                            <div style="position: absolute; right: 10px; top: 80px; z-index: 999;">
                                <ul class="hlist"> 
                                    <li><button id="bLogWorkout">Log this workout</button></li>                                                                
                                    <li><button id="bAddWorkout" runat="server" style="width: 115px;">Add to My Workouts</button></li>
                                    <li><button id="bRemoveWorkout" runat="server" visible="false" style="width: 115px;">Remove from My Workouts</button></li>                                    
                                </ul>
                            </div>
                            <div id="workoutInfo">
                                <h2><asp:Literal ID="lWorkoutTitle" runat="server" /></h2>                                                                                   
                            </div>   
                            <div class="infoScroll">
                                    <asp:Literal ID="lWorkoutInfo" runat="server" />         
                                </div>                          
                            <div class="profileImage">
                                <ati:ProfileImage small="true" id="atiProfileImg" runat="server" />   
                                <img id="imgCrossFit" runat="server" visible="false" />                                
                            </div>     
                        </div>  
                        <div style="padding-top: 15px;" class="atiControlPanel grad-FFF-EEE">                            
                            <ati:ShareLink ID="atiShareLink" runat="server" TextBoxWidth="300px" TextBoxCssClass="ui-corner-all ui-widget-content atiTxtBox shareLink" />                                
                        </div>    
                        
                        <asp:Panel ID="atiPanelYourProgress" runat="server">                  
                            <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                                <h3>Your Progress</h3>
                                <div id="bShowYourProgress" class="ui-corner-all ui-widget-content grad-FFF-EEE bMin">-</div>
                            </div>
                            <div id="panelYourProgress" class="atiMainFeed">
                                <asp:Panel ID="atiStreamPanelAjax" runat="server">
                                    <ati:StreamScript ID="atiYouStreamScript" runat="server" ShowTopPager="false" ShowBottomPager="false" />                       
                                </asp:Panel>
                            </div>
                        </asp:Panel>  
                        
                                
                        <asp:Panel ID="atiPanelQuickView" runat="server">   
                            <div class="atiListHeading grad-FFF-EEE" style="position: relative;">                            
                                <h3>Quick View</h3>
                                <div id="bShowQuickView" class="ui-corner-all ui-widget-content grad-FFF-EEE bMin">-</div>
                            </div>
                            <div id="panelQuickView" class="atiMainFeed" style="position: relative;">                                
                                <ati:WorkoutHighChart ID="atiWorkoutHighChart" runat="server" Width="98%" Height="200px" />
                                <button id="bChartPageNext" style="position: absolute; right: 5px; top: 100px; z-index: 999999;">&gt;</button>
                            </div>
                        </asp:Panel>

                        
                        <div class="atiListHeading grad-FFF-EEE" style="position: relative;">                            
                            <h3>Workouts Logged</h3>
                            <div id="bShowUserSearch" class="ui-corner-all ui-widget-content grad-FFF-EEE bMin">-</div>
                        </div>
                        <div id="panelAthleteSearch" class="atiListHeading grad-FFF-EEE" style="position: relative; display: block;">
                        
                            <h3>Find people who recorded this workout and ...</h3>
                            <div class="findPeoplePanel">
                                <div style="position: absolute; left: 10px; top: 35px;">
                                Are:
                                <ul class="vlist" style="padding-left: 30px;">
                                    <li><input type="checkbox" name="findPeopleSexM" id="findPeopleSexM" value="M" checked="checked" /> Male</li>
                                    <li><input type="checkbox" name="findPeopleSexF" id="findPeopleSexF" value="F" checked="checked" /> Female</li>
                                    <li><input type="checkbox" name="findPeopleRxs" id="findPeopleRxs" value="Rxd" checked="checked" /> RxD</li>
                                </ul>
                                </div>

                                <div id="atiTimeSpanePanel" runat="server" class="findPeopleByMetric" visible="false">
                                    <span>With times between:</span>
                                    <ul class="vlist">
                                        <li><ati:TimeSpan ID="atiTimeSpanStart" runat="server" ShowPace="false" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" /></li>
                                        <li><span>and</span></li>
                                        <li><ati:TimeSpan ID="atiTimeSpanFinish" runat="server" ShowPace="false" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" /></li>
                                    </ul>
                                </div>
                                <div id="atiScoreRangePanel" runat="server" class="findPeopleByMetric" visible="false">
                                    <span>With scores between:</span>
                                    <ul class="vlist">
                                        <li><asp:TextBox ID="atiScoreStart" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" style="width: 150px;" /></li>
                                        <li><span>and</span></li>
                                        <li><asp:TextBox ID="atiScoreFinish" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" style="width: 150px;" /></li>
                                    </ul>
                                </div>
                                <div id="atiMaxRangePanel" runat="server" class="findPeopleByMetric" visible="false">
                                    <span>With max weight between:</span>
                                    <ul class="vlist">
                                        <li><asp:TextBox ID="atiMaxFirst" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" style="width: 100px;" /><ati:UnitControl id="atiMaxWeightUnitsFirst" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" /></li>
                                        <li><span>and</span></li>
                                        <li><asp:TextBox ID="atiMaxLast" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox dull" style="width: 100px;" /><ati:UnitControl id="atiMaxWeightUnitsLast" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" /></li>
                                    </ul>
                                </div>
                                <button id="bSearchPeople" style="position: absolute; top: 70px; right: 10px;">Search</button>
                            </div>
                        </div>   
                        <div class="atiMainFeed">
                            <asp:Panel ID="Panel1" runat="server">
                                <ati:StreamScript ID="atiEveryoneStreamScript" runat="server" ShowTopPager="false" />                       
                            </asp:Panel>
                        </div>                 
                        
                    </asp:Panel>
                </div>
          			
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; position: absolute; right: 0;">
            <img runat="server" id="imgAd" />
            <asp:Panel ID="atiVideoPanel" runat="server">
                <br /><br />
                <h3>Related Videos</h3>                            
                <ati:YoutubeThumbList id="atiYoutubeThumbList" runat="server" />
                <button id="bMoreVideos" style="float: right; margin-top: 10px">More ...</button>
                <br style="clear: both;" />
            </asp:Panel>
        </div>
    </div>   
    <div style="clear:both;"></div>            