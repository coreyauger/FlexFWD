<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_People.ViewATI_People" CodeFile="ViewATI_People.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="NameValue" Src="~/DesktopModules/ATI_Base/controls/ATI_NameValueGrid.ascx" %>
<%@ Register TagPrefix="ati" TagName="FeaturedStreamItem" Src="~/DesktopModules/ATI_Base/controls/ATI_FeaturedStreamItem.ascx" %>
<%@ Register TagPrefix="ati" TagName="PeopleList" Src="~/DesktopModules/ATI_Base/controls/ATI_PeopleListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutTypes" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutTypes.ascx" %>
<%@ Register TagPrefix="ati" TagName="ShareLink" Src="~/DesktopModules/ATI_Base/controls/ATI_ShareLink.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="LeaderBoard2" Src="~/DesktopModules/ATI_Base/controls/ATI_LeaderBoard2.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">
<style type="text/css">

    div.atiRouteListControls,
    div.atiGMapControls
    {
	    border-bottom: 1px solid #CCC;
	    position: relative;
    }
    
    div.workoutGhost
    {
    	border: 4px dashed #CCC; margin: 10px; text-align: center; vertical-align: middle;
    }
    div.workoutGhost:hover
    {
    	background-color: #eee;
    }   
    
    div.unameHover
    {
    	position: absolute; 
    	left: 230px;
    }
    div.unameHover a
    {
    	font-size: 22px;
    	color: #F9A01B;
    	display: inline;
    }
    div.unameHover span
    {
    	margin-left: 10px;
    	color: #666;
    	font-size: 10px;
    }
            
    div#routeInfo
    {
    	position: absolute;
    	left: 200px;
    }
    div#routeInfo h2
    {
    	font-size: 24px;
    	color: #0095CD;
    	margin: 0;
    }  
    
    div.profileImgWrap
    {
    	width: 200px;
    }
    
    div.profileImgWrap img
    {
    	border: 1px solid #0095CD
    } 
    
    div#cfAchievements table
    {
    	width: 100%;
    } 
    table.nvGrid
    {
    	color: #666;
    	font-size: 12px;
    }
    td.nvValue
    {
    	font-weight: bold;
    }
    
    table.nvGrid td
    {
    	padding-left: 20px;
    	height: 30px;
    	
    }
    table.nvGrid tr:nth-child(even)
    {
	    background-color: #FAFAFA;
    }
    
    div.featuredTitle
    {
        padding: 4px 16px;
        background-color: #EFEFEF;
        
    }
    div.featuredTitle span
    {
    	color: #0095CD;
    	font-weight: bold;
    	font-size: 14px;
    } 
    
    h2.hSplitTitle
    {
    	display: block;
    	background-color: #fafafa;
    	margin: 0px;
    	font-size: 14px;
    	font-weight: bold;
    	color: #1C94C4;
    	padding: 12px 20px 4px 30px;
    }  
    
    div.WorkoutTypeList ul li
    {
    	padding: 14px 20px;
    } 
    div.WorkoutTypeList ul li img
    {
    	position: relative;
    	top: 10px;
    }
    
     div.WorkoutTypeList li
     {
     	border-right: 1px solid #CCC;
     	border-top: 1px solid #CCC;
     	border-bottom: 1px solid #CCC;
     }
    
    div.WorkoutTypeList  li.workoutTypeSelected
    {
    	border: 0;
    	border-right: 1px solid #CCC;
    	background-color: #FFF;
    }
    
    li.workoutTypeSelected a
    {
    	color: Red;
    }
    
    div.atiStreamItemRight
    {
	    width: 670px !important;
    }
    
    .bMinMax
    {
    	position: absolute;
    	top: 5px; 
    	right: 5px;
    }

</style>
<script language="javascript" type="text/javascript">
    Aqufit.Windows = {
        WorkoutSelectDialog: {
            win: null,
            open: function (json) {
                Aqufit.Windows.WorkoutSelectDialog.win = $find('<%=WorkoutSelectDialog.ClientID %>');
                Aqufit.Windows.WorkoutSelectDialog.win.show();
            },
            close: function () {
                Aqufit.Page.atiLoading.remove();
                Aqufit.Windows.WorkoutSelectDialog.win.close();
            }
        }
    };
  
    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        WorkoutTypeChanged: function (id) {
            top.location.href = '?wt=' + id;
        },
        RestoreDefault: function () {
            if (confirm("Are you sure you want to revert back to the default leader board?  All your changes will be lost")) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('RestoreDefault');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
            }
        },
        atiPeopleList1_ItemsLoad: function (sender, page, size) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('page1');
            $('#<%=hiddenAjaxValue.ClientID %>').val(page * size);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
        },
        atiPeopleList2_ItemsLoad: function (sender, page, size) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('page2');
            $('#<%=hiddenAjaxValue.ClientID %>').val(page * size);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
        },
        atiPeopleList3_ItemsLoad: function (sender, page, size) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('page3');
            $('#<%=hiddenAjaxValue.ClientID %>').val(page * size);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
        },
        atiPeopleListFast1_ItemsLoad: function (sender, page, size) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('pageFast1');
            $('#<%=hiddenAjaxValue.ClientID %>').val(page * size);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
        },
        atiPeopleListFast2_ItemsLoad: function (sender, page, size) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('pageFast2');
            $('#<%=hiddenAjaxValue.ClientID %>').val(page * size);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
        }
    };

    function atiRadCombo_OnClientItemsRequesting(sender, eventArgs) {
        var context = eventArgs.get_context();
        context["UserSettingsId"] = Aqufit.Page.UserSettingsId;
    }

    Aqufit.Page.Controls.atiWodSelector = {
        OnClientSelectedIndexChangedEventHandler: function (sender, args) {
            var item = args.get_item();
            var wod = eval('(' + item.get_value() + ')');
            $('#<%=hiddenWodKey.ClientID %>').val(wod.Id);
            $('#infoError').html('');            
        }
    }

    $(function () {
        $('#atiStatusWidget').hide();
        $('#tabs').tabs();

        $('.bMinMax').button({ text: false, icons: { primary: 'ui-icon-minus'} }).click(function (event) {
            var opts = $(this).button("option");
            if (opts.icons.primary == 'ui-icon-minus') {
                $(this).button("option", "icons", { primary: 'ui-icon-plus' }).parent().next().hide();

            } else {
                $(this).button("option", "icons", { primary: 'ui-icon-minus' }).parent().next().show();
            }
            event.stopPropagation();
            return false;
        });

        $('div.WorkoutTypeList li').addClass('grad-FFF-EEE');
        $('div.WorkoutTypeList  li.workoutTypeSelected').removeClass('grad-FFF-EEE');
        $('.dull').focus(function () {
            $(this).val('').removeClass('dull');
        });
        $('#bConfigLeaderBoard').button({ icons: { primary: 'ui-icon-wrench'} }).click(function (event) {
            top.location.href = "?c=1";
            event.stopPropagation();
            return false;
        });
        $('#bGroupProfile').button({ icons: { primary: 'ui-icon-home'} }).click(function (event) {
            top.location.href = "/group/" + Aqufit.Page.ProfileUserName;
            event.stopPropagation();
            return false;
        });
        $('#bAddOpenWorkout').button({ icons: { primary: 'ui-icon-plusthick'} }).click(function (event) {
            Aqufit.Windows.WorkoutSelectDialog.open();
            event.stopPropagation();
            return false;
        });
        $('#bBack').button({ icons: { primary: 'ui-icon-arrowreturnthick-1-w'} }).click(function (event) {
            top.location.href = '/' + Aqufit.Page.ProfileUserName + '/achievements';
            event.stopPropagation();
            return false;
        });
        $('#bAddWorkout').button({ icons: { primary: 'ui-icon-plusthick'} }).click(function (event) {
            var val = $('#<%=hiddenWodKey.ClientID %>').val();
            if (val == '') {
                $('#infoError').html('* You must select a workout from the drop down.');
            } else {
                Aqufit.Page.atiLoading.addLoadingOverlay('workoutSelectPanel');
                $('#<%=hiddenAjaxAction.ClientID %>').val('configAddWorkout');
                $('#<%=hiddenAjaxValue.ClientID %>').val($('#numAthletes option:selected').val());
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
            }
            event.stopPropagation();
            return false;
        });
    });

    Aqufit.addLoadEvent(function () {
        if (Aqufit.Page.atiLeaderBoard2Config) {
            Aqufit.Page.atiLeaderBoard2Config.deleteCallback = function (wodKey) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('configDelWorkout');
                $('#<%=hiddenAjaxValue.ClientID %>').val(wodKey);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$People","ATI_People") %>', '');
            }
        }
    });
    
</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   

 <asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue2" runat="server" />
    <asp:HiddenField ID="hiddenWodKey" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>


<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>
        <telerik:RadWindow ID="WorkoutSelectDialog" runat="server" Skin="Black" Title="Select Workout" Width="500" Height="250" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
                <ContentTemplate>
                    <div id="workoutSelectPanel" style="width: 100%; height: 100%; background-color: white;">
                        <div style="width: 400px; margin: 0px auto; padding-top: 30px;">
                        <dl>
                            <dt>Select a Workout:</dt>
                            <dd>
                            <telerik:RadComboBox ID="atiRadComboBoxCrossfitWorkouts" runat="server" Width="400px" Height="140px"
                                EmptyMessage="Start typing a workout name (eg: fran)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true" OnClientItemsRequesting="atiRadCombo_OnClientItemsRequesting"
                                OnClientSelectedIndexChanged="Aqufit.Page.Controls.atiWodSelector.OnClientSelectedIndexChangedEventHandler">

                                <WebServiceSettings Method="GetStandardWorkoutsOnDemand" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
                            </telerik:RadComboBox>
                            </dd>

                            <dt>Number of Athletes to display</dt>
                            <dd>
                                <select id="numAthletes">
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5" selected="selected">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                </select>
                            </dd>                            
                        </dl>
                        <br /><br />
                        <span id="infoError" style="color: Red;"></span>
                        <button id="bAddWorkout" style="float: right;">Add</button>
                        </div>                        
                    </div>                    
                </ContentTemplate>
        </telerik:RadWindow>
    </Windows>
</telerik:radwindowmanager>

<ati:LoadingPanel ID="atiLoading" runat="server" />

    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper">
        <div id="divMain" style="width: 764px; float: left; margin-right: 8px; position: relative;">            
           <!-- TODO: a search box to search messages -->
            <!-- Tabs -->            
    		<div id="tabs">
    			<ul>
    				<li><a href="#pageViewProple"><asp:Literal ID="peopleTabTitle" runat="server" /></a></li>
                    <li id="liFindFriends" runat="server"><a href="#<%=pageViewFindFriends.ClientID %>" onclick="top.location.href='/Profile/FindInvite';">Find Friends</a></li>   				              
                </ul>
    			<div id="pageViewProple" style="padding: 0px; background-color: White;">                                                                                                        
                    <asp:Panel ID="atiPeoplePanel" runat="server">                        
                        <div id="mapContainer" runat="server">                            
                            <div id="atiMapContainer">
                                <div class="atiGMapControls grad-FFF-EEE">                                    
                                    <ul class="hlist">
                                        <li></li>
                                        <li>&nbsp;</li>
                                    </ul>
                                </div>
                                  
                            </div>                            
                        </div>  
                        <div id="workoutTypeTabBar">
                            <ati:WorkoutTypes id="atiWorkoutTypes" runat="server" TypeDisplayMode="ICONS" OnClientWorkoutTypeChanged="Aqufit.Page.Actions.WorkoutTypeChanged"  />
                        </div>
                        <asp:Panel ID="panelLiveNearMe" runat="server">
                            <h2 class="hSplitTitle">Live Near Me</h2>
                            <ati:PeopleList ID="atiPeopleList1" runat="server" Client_OnItemsLoad="Aqufit.Page.Actions.atiPeopleList1_ItemsLoad" />
                        </asp:Panel>

                        <h2 class="hSplitTitle">Recently Active</h2>
                        <ati:PeopleList ID="atiPeopleList2" runat="server" Client_OnItemsLoad="Aqufit.Page.Actions.atiPeopleList2_ItemsLoad" />

                        <h2 class="hSplitTitle">Most Watched</h2>
                        <ati:PeopleList ID="atiPeopleList3" runat="server" Client_OnItemsLoad="Aqufit.Page.Actions.atiPeopleList3_ItemsLoad" />

                        <h2 class="hSplitTitle" id="hFastTime1" runat="server">Fastest</h2>
                        <ati:PeopleList ID="atiPeopleListFast1" runat="server" Client_OnItemsLoad="Aqufit.Page.Actions.atiPeopleListFast1_ItemsLoad" />

                        <h2 class="hSplitTitle" id="hFastTime2" runat="server">Fastest</h2>
                        <ati:PeopleList ID="atiPeopleListFast2" runat="server" Client_OnItemsLoad="Aqufit.Page.Actions.atiPeopleListFast2_ItemsLoad" />                        
                    </asp:Panel> 

                    <asp:Panel ID="atiPeopleViewer" runat="server" Visible="false"> 
                            <div class="atiRouteListControls grad-FFF-EEE">
                                <div class="atiListHeading grad-FFF-EEE" style="position: relative;">                            
                                <h3>Leader Board: <asp:Literal ID="litUserName" runat="server" /></h3>
                                <ul class="hlist" style="position: absolute; right: 5px; top: 5px;">
                                    <li id="liConfig" runat="server" visible="false"><button id="bConfigLeaderBoard">Configure Leader Board</button></li>
                                    <li><button id="bGroupProfile">Profile</button></li>   
                                </ul>                            
                            </div>
                            <div style="padding: 10px;">
                                <div style="float:right; margin: 50px 0px 10px 0px; width: 520px;">                                
                                    <div class="workoutTotals">
                                        <ati:NameValue ID="atiWorkoutTotals" runat="server" />
                                    </div>
                                </div>
                                <div class="profileImgWrap" style="min-height: 200px;">
                                    <ati:ProfileImage small="false" id="atiProfileImg" runat="server" />                                  
                                </div>
                            </div>
                        </div>  
                        <div style="padding-top: 15px;" class="atiRouteListControls grad-FFF-EEE">
                            <ati:ShareLink ID="atiShareLink" runat="server" TextBoxWidth="500px" TextBoxCssClass="ui-corner-all ui-widget-content atiTxtBox shareLink" />      
                        </div>            
                        
                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>CrossFit</h3>
                            <button class="bMinMax">&nbsp;</button>
                        </div>
                        <div id="cfAchievements">
                            <ati:NameValue ID="nvgCrossfit" runat="server" Cols="2" CssClass="nvGrid" /> 
                            <ati:LeaderBoard2 ID="atiLeaderBoard2" runat="server" />                        
                        </div>

                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Running</h3>
                            <button class="bMinMax">&nbsp;</button>
                        </div>
                        <div id="runningAchievements">
                            <ati:FeaturedStreamItem id="atiFeaturedRunning" runat="server" Title="" />                            
                        </div>
                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Cycling</h3>
                            <button class="bMinMax">&nbsp;</button>
                        </div>
                        <div id="cyclingAchievements">
                            <ati:FeaturedStreamItem id="atiFeaturedStreamCycling" runat="server" Title="" />                            
                        </div> 
                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Swimming</h3>
                            <button class="bMinMax">&nbsp;</button>
                        </div>
                        <div id="swimmingAchievements">
                            <ati:FeaturedStreamItem id="atiFeaturedSwimming" runat="server" Title="" />                            
                        </div>  
                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Rowing</h3>
                            <button class="bMinMax">&nbsp;</button>
                        </div>
                        <div id="rowingAchievements">
                            <ati:FeaturedStreamItem id="atiFeaturedRowing" runat="server" Title="" />                            
                        </div>                        
                    </asp:Panel>

                    <asp:Panel ID="atiLeaderBoardConfig" runat="server" Visible="false" style="min-height: 500px;">
                        <div class="atiListHeading grad-FFF-EEE" style="position: relative;">                            
                            <h3>Leader Board Configuration</h3>
                            <ul class="hlist" style="position: absolute; right: 5px; top: 5px;">
                                <li><button id="bBack" >Back</button></li>
                                <li><button id="bAddOpenWorkout" >Add Workout</button></li>
                            </ul>                            
                        </div>
                        <div style="background-color: #eee; border-bottom: 1px solid #ccc; padding: 10px;">
                            <span><em>Instructions:</em> Click on the "Add Workout" to select from the list of standard workouts.  You will see how the leader board will look as you add the workouts.  At anytime you can 
                            revert back to the "default" leader board (which includes all standards) by clicking this "<a href="javascript: ;" onclick="Aqufit.Page.Actions.RestoreDefault();">restore default</a>" link.</span>
                        </div>
                        
                        <a href="javascript: ;" onclick="Aqufit.Windows.WorkoutSelectDialog.open();">
                            <div class="workoutGhost">
                                <h3>Add Workout</h3>
                            </div>
                        </a>
                        <ati:LeaderBoard2 ID="atiLeaderBoard2Config" EditMode="true" runat="server" Cols="2" />
                        
                    </asp:Panel>
                </div>
          		<div id="pageViewFindFriends" style="padding: 0px; background-color: White;" runat="server"> 
                    <div style="text-align: center;margin-top: 50px;">
                        <h1>Loading ...</h1>
                    </div>
                
                </div>	
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; float: right;">
            <img runat="server" id="imgAd" />
        </div>
    
    </div>   
    <div style="clear:both;"></div>     
               










    

    

         
    
               



