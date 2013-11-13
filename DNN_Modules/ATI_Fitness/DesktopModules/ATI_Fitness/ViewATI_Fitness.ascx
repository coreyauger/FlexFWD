<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Fitness.ViewATI_Fitness" CodeFile="ViewATI_Fitness.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_Profile.ascx" %>
<%@ Register TagPrefix="ati" TagName="Workout" Src="~/DesktopModules/ATI_Base/controls/ATI_Workout.ascx" %>
<%@ Register TagPrefix="ati" TagName="Comment" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamComment.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="HighChart" Src="~/DesktopModules/ATI_Base/controls/ATI_HighChart.ascx" %>
<%@ Register TagPrefix="ati" TagName="TotalDistColors" Src="~/DesktopModules/ATI_Base/controls/ATI_TotalDistColors.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutSummaryHead" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutSummaryHead.ascx" %>
<%@ Register TagPrefix="ati" TagName="NameValueGrid" Src="~/DesktopModules/ATI_Base/controls/ATI_NameValueGrid.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="StrengthGraph" Src="~/DesktopModules/ATI_Base/controls/ATI_StrengthGraph.ascx" %>
<%@ Register TagPrefix="ati" TagName="StepByStep" Src="~/DesktopModules/ATI_Base/controls/ATI_StepByStep.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamTutorial" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamTutorial.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamAttachment" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamAttachment.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">

<style type="text/css">
html
{
    background-color: #cdcdcd; 
    background-image:url(<%=this.BackgroundImageUrl%>);
    background-repeat:no-repeat;
    background-attachment:fixed;
    <%=this.ProfileCSS%>
}

html body
{
    background-color: #cdcdcd; 
    background-image:url(<%=this.BackgroundImageUrl%>);
    background-repeat:no-repeat;
    background-attachment:fixed;
    <%=this.ProfileCSS%>
}
  #workoutList
  {
  	float: right;
  }
    
.atiGadgetContainer
{
	padding-bottom: 10px;
}

.atiMainFeed div.atiStreamItemRight
{
	width: 460px;
}

.faChooseDiv ul
{
	padding-top: 10px;
}
.faChooseDiv ul li
{
	list-style: disc !important;
	margin: 2px 25px;
}

#statContent
{
	background-color: #fafafa;
}

#statContent ul li
{
	list-style: none;
	border-bottom: 1px solid #ccc;
	padding: 5px 20px;
	font-size: 14px;
}

span.calendar
{
	background-image:url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCalendar.png")%>);
    background-repeat:no-repeat;
    width: 43px;
    height: 44px;
    display: inline-block;
    font-size: 16px;
    padding: 12px 0px 0px 10px;
}

</style>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<script type="text/javascript" >

    Aqufit.Windows.MessageWin = {
        open: function (arg) {
            this.win = radopen('<%=ResolveUrl("~/FitnessProfile/MesageSend.aspx") %>?u=' + Aqufit.Page.UserName, 'MessageWin');
            this.win.set_modal(true);
            this.win.setSize(747, 600);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };

    Aqufit.Windows.PhotoWin = {
        open: function (pid, uname) {
            this.win = radopen('<%=ResolveUrl("~/") %>'+uname+'/viewphoto?p=' + pid, 'PhotoWin');
            this.win.set_modal(true);
            this.win.setSize(800, 700);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };

    Aqufit.Windows.WorkoutProblem ={
        win: null,
        open: function (showProblem) {
            if( showProblem ){
                $('#wpTitle, #wpInfo').show();
            }else{
                $('#wpTitle, #wpInfo').hide();
            }
            Aqufit.Windows.WorkoutProblem.win = $find('<%=WorkoutProblem.ClientID %>');
            Aqufit.Windows.WorkoutProblem.win.show();
        },
        close: function () {
            Aqufit.Windows.WorkoutProblem.win.close();
        }
    };

    Aqufit.Windows.WorkoutStatsDialog = {
        win: null,
        open: function (json) {
            var oJson = eval('(' + json + ')');
            $('#statContent').html(oJson['html']);
            $('#wsTitle').html(oJson['title']);
            $('#wsCompareUrl').attr('href',oJson['statUrl']);
            Aqufit.Windows.WorkoutStatsDialog.win = $find('<%=WorkoutStatsDialog.ClientID %>');
            Aqufit.Windows.WorkoutStatsDialog.win.show();
        },
        close: function () {
            Aqufit.Windows.WorkoutStatsDialog.win.close();
        }
    };

    Aqufit.Windows.FollowAthleteModal = {
        win: null,
        open: function () {            
            Aqufit.Windows.FollowAthleteModal.win = $find('<%=FollowAthleteModal.ClientID %>');
            Aqufit.Windows.FollowAthleteModal.win.show();
        },
        close: function () {
            if(Aqufit.Windows.FollowAthleteModal.win){
                Aqufit.Windows.FollowAthleteModal.win.close();
            }
        }
    };



    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        },
        SwitchToLogWorkoutTab: function () {
            Aqufit.Page.Tabs.SwitchTab(1);
        }

    };

    Aqufit.Page.Actions = {
        SendFriendRequest: function () {
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddFriend');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        },
        SendSuggestedFriendRequest: function(usid){
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddSuggestFriend');
            $('#<%=hiddenAjaxValue.ClientID %>').val(usid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        },
        ShowFail: function (msg) {
            Aqufit.Windows.FollowAthleteModal.close();            
            radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #fff !important;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
        },
        SetWorkout: function (sender, eventArgs) {
            var grid = sender;
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
            var cellId = MasterTable.getCellByColumnUniqueName(row, "Id");
            var cellName = MasterTable.getCellByColumnUniqueName(row, "Name");
            var cellType = MasterTable.getCellByColumnUniqueName(row, "WODTypeKey");
            Aqufit.Page.Controls.atiWodSelector.SetSelected(cellId.innerHTML, cellName.innerHTML, cellType.innerHTML);
            Aqufit.Windows.WorkoutProblem.close();
        },
        test: function(){
            $('#<%=hiddenAjaxAction.ClientID %>').val('test');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        }         
    };

    function OnResponseEnd(){
        Aqufit.Page.atiLoading.remove();
    }

    $(function () {
        $('#atiStatusWidget').hide();
        $('#workoutList').button().click(function (event) {
            self.location.href = '<%= ResolveUrl("~/") %>' + Aqufit.Page.UserName + '/workout-history';
            event.stopPropagation();
            return false;
        });
        $('#bViewStats').button().click(function (event) {
            top.location.href = '<%= ResolveUrl("~/") %>' + Aqufit.Page.UserName + '/workout/' + $('#<%=hiddenWorkoutKey.ClientID%>').val();
            event.stopPropagation();
            return false;
        }); 
        $('#bClose').button().click(function(event){
            Aqufit.Windows.WorkoutStatsDialog.close();
            event.stopPropagation();
            return false;
        }); 
        $('#bStreamPost').button().click(function(event){
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=panelStreamPost.ClientID %>');
            $('#<%=hiddenAjaxAction.ClientID %>').val('StreamPost');
            var comment = $('#<%=txtStreamPostTxt.ClientID %>').val();
            $('#<%=hiddenAjaxValue.ClientID %>').val(comment);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');          
            event.stopPropagation();
            return false;
        });      

        $('.linksList #bViewPhotos').click(function () {
            top.location.href = '<%= ResolveUrl("~/") %>' + Aqufit.Page.UserName + '/photos'; 
        });

        var loading = '<div class="loadingPanel"><img style="margin-top: 100px;" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif") %>" /><br /><h2 style="color: #000;">Loading ...</h2></div>';
        $('#bSubmitComment').click(function (event) {
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=pageViewComment.ClientID %>');
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddComment');
            var comment = Aqufit.Page.atiComment.getTxt();
            $('#<%=hiddenAjaxValue.ClientID %>').val(comment);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');            
            event.stopPropagation();
            return false;
        });
        $('#bSubmitWorkoutAjax').click(function (event) {
            try{                
                Aqufit.Page.atiWorkout.validate()
                Aqufit.Page.atiLoading.addLoadingOverlay('<%=pageViewWorkout.ClientID %>');
                $('#<%=hiddenAjaxAction.ClientID %>').val('SaveWorkout');            
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
            }catch(err){
                if( err == Aqufit.Page.atiWorkout.ERR_NO_WORKOUT ){
                    Aqufit.Windows.WorkoutProblem.open(true);
                }else if( err == Aqufit.Page.atiWorkout.ERR_NO_SCORE ){
                    Aqufit.Page.Actions.ShowFail('Error: You must have a score or a time for the workout');
                }
            }
            event.stopPropagation();
            return false;
        });
        $('#bAddToFriends').click(function () {
            $('#faModalContainer').append(loading);
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddFriend');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        });
        $('#bAddToFollow').click(function () {
            $('#faModalContainer').append(loading);
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddFollow');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        });
        $('#<%=ddlGroupSchedule.ClientID %>').change(function(){
            var masterTable = $find('<%=RadGrid2.ClientID %>').get_masterTableView();;
            masterTable.fireCommand("Cancel", "");
        });
        $('#bCreateWorkout').button().click(function(event){
            top.location.href = '<%=ResolveUrl("~/Profile/WorkoutBuilder") %>';
            event.stopPropagation();
            return false;
        });
        $('#bFindGroups').button().click(function(event){
            top.location.href= '<%=ResolveUrl("~/Community/Groups.aspx") %>';
            event.stopPropagation();
            return false;
        });
    });

    Aqufit.addLoadEvent(function () {
        <%if( Request["s"] != null ){ %>
            Aqufit.Page.atiStreamScript.getStreamItem(<%=Request["s"] %>, 0, '');
        <% }else{ %>
            Aqufit.Page.atiStreamScript.getStreamData(0, '');
        <% } %>
        Aqufit.Page.atiStreamScript.noDataHtml = '<h3>Workouts that you and your friends post will show up here.  When you are ready to log your first workout, click the log workout tab or log workout link in your left hand menu.</h3>';
        Aqufit.Page.atiStreamScript.streamDeleteCallback = function (id) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('delStream');
            $('#<%=hiddenAjaxValue.ClientID %>').val(id);           
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        };   
        Aqufit.Page.atiStreamScript.onDeleteComment = function(id){
            $('#<%=hiddenAjaxAction.ClientID %>').val('delComment');
            $('#<%=hiddenAjaxValue.ClientID %>').val(id);           
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
        }; 
        if( Aqufit.Page.atiStepByStep ){
            Aqufit.Page.atiStepByStep.onCloseCallback = function(){
                $('#<%=hiddenAjaxAction.ClientID %>').val('remStepByStep');
                $('#<%=hiddenAjaxValue.ClientID %>').val(0);           
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
            };
        }   
        if( Aqufit.Page.atiStreamTutorial ){
            Aqufit.Page.atiStreamTutorial.onCloseCallback = function(){
                $('#<%=hiddenAjaxAction.ClientID %>').val('remStreamTutorial');
                $('#<%=hiddenAjaxValue.ClientID %>').val(0);           
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Fitness") %>', '');
            };
        }
        if( Aqufit.Page.atiWorkout ){
            Aqufit.Page.atiWorkout.viewScheduleClickHandler = function(){
                Aqufit.Windows.WorkoutProblem.open(false);
            }     
        }
    });
          
</script>    
</telerik:RadCodeBlock>

    <telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>
        <telerik:RadWindow runat="server" ID="MessageWin" />
        <telerik:RadWindow runat="server" ID="WatchList" />
        <telerik:RadWindow runat="server" ID="UploadWin" />
        <telerik:RadWindow runat="server" ID="PhotoWin" />
        
        <telerik:RadWindow ID="WorkoutProblem" runat="server" Skin="Black" Title="Workout Helper" Width="600" Height="450" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div id="wpTitle" class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3>There was a problem saving your workout.</h3>                        
                    </div>
                    <div id="wpInfo" style="padding: 10px;">
                         <div class="ui-widget">
			                <div style="padding: 10px;" class="ui-state-error ui-corner-all"> 
				                <p style="color: #fff;"><span style="float: left; margin-right: .3em;" class="ui-icon ui-icon-alert"></span> 
				                <strong>Alert:</strong> There is no workout in the system with that name.</p>
			                </div>
                            <br />
                            Click here to <button id="bCreateWorkout">Create this Workout</button> OR perhaps you are looking for a scheduled group workout that is listed below.
		                </div>         
                    </div>
                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3> Workout Schedule for: <asp:DropDownList ID="ddlGroupSchedule" runat="server" /></h3>                        
                    </div>
                    <telerik:RadGrid ID="RadGrid2" runat="server" PageSize="10" ShowGroupPanel="false" ShowStatusBar="false" AllowPaging="True" AllowSorting="true"
                            OnNeedDataSource="RadGrid2_NeedDataSource" ClientSettings-ClientEvents-OnRowClick="Aqufit.Page.Actions.SetWorkout"
                            AllowFilteringByColumn="true" AutoGenerateColumns="False" GridLines="None">
                        <PagerStyle Mode="NumericPages"></PagerStyle>  
                        <ClientSettings AllowGroupExpandCollapse="True" ReorderColumnsOnClient="false" AllowColumnsReorder="false" AllowDragToGroup="false" Selecting-AllowRowSelect="true"></ClientSettings>
                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed">  
                            <Columns>
                                <telerik:GridNumericColumn SortExpression="Id" HeaderText="Id" UniqueName="Id" HeaderButtonType="TextButton" DataField="Id" Display="false">                                                                      
                                </telerik:GridNumericColumn>   
                                <telerik:GridBoundColumn SortExpression="WODTypeKey" HeaderText="WODTypeKey" UniqueName="WODTypeKey" DataField="WODTypeKey" Display="false">                                        
                                </telerik:GridBoundColumn>                                
                                                                        
                                <telerik:GridDateTimeColumn SortExpression="Date" HeaderText="Date" UniqueName="Date" HeaderButtonType="TextButton" DataField="Date" DataFormatString="{0:MMM dd, yyyy}">
                                </telerik:GridDateTimeColumn>                                    

                                <telerik:GridBoundColumn SortExpression="Name" AutoPostBackOnFilter="true" HeaderText="Name" UniqueName="Name" HeaderButtonType="TextButton" DataField="Name" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn SortExpression="Type" HeaderText="Type" UniqueName="WODType" HeaderButtonType="TextButton" DataField="WODType" AllowFiltering="false">                                        
                                </telerik:GridBoundColumn>                                                                       
                                 
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Action" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                    <a href="javascript: ;">Log</a>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>                                     
                            </Columns>
                        </MasterTableView>                                                                        
                    </telerik:RadGrid>
                    <asp:Panel ID="panelNoGroups" runat="server" Visible="false" style="padding: 20px;">
                        <h3>You are currently not a member of any groups.  Click here to <button id="bFindGroups">Find Groups</button></h3>
                    </asp:Panel>         
                </div>                    
            </ContentTemplate>
        </telerik:RadWindow>
    
        <telerik:RadWindow ID="WorkoutStatsDialog" runat="server" Skin="Black" Title="Workout Statistics" Width="600" Height="450" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div id="workoutStatusPanel" style="width: 100%; height: 100%; background-color: white;">
                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3 id="wsTitle">Workout Title</h3>
                        <ul class="hlist" style="position: absolute; right: 10px; top: 4px;">                                
                            <li><a id="wsCompareUrl" title="Compare Stats"><span class="grad-FFF-EEE ui-corner-all" style="border: 1px solid #ccc; padding: 5px;">View Graph Stats&nbsp;&nbsp;<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iStats_s.png") %>" /></span></a></li>
                        </ul>
                    </div>
                    <div id="statContent"></div>                    
                       
                    <button id="bClose" style="float: right;">Close</button>                 
                    <button id="bViewStats" style="float: right;">View Stats</button>
                    
                    <!-- This needs to be here -->
                    <asp:Literal ID="litDebug" runat="server" />
                </div>                    
            </ContentTemplate>
        </telerik:RadWindow>

        <telerik:RadWindow ID="FollowAthleteModal" runat="server" Skin="Black" Title="Add Athlete to ..." Width="700" Height="300" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div id="faModalContainer" style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div class="faChooseDiv grad-FFF-EEE" style="width: 50%; height: 100%; float: right; border-left: 1px solid #CCC;">
                        <div style="padding: 15px;">
                            <div id="bAddToFriends" class="normalButton"><img id="imgAddToFriends" runat="server" align="absmiddle" /> Send a Friend Request</div>
                            <span>
                                <p>This is an Athlete that you know on a personal level.  They will be sent a friend request that they will have to approve before you become friends.</p>
                                <ul>
                                    <li>Your workouts will be visible to that user.</li>
                                    <li>Their workouts will be visible to you.</li>
                                    <li>You can send messages to this athlete.</li>
                                    <li>Compare your workouts.</li>
                                </ul>
                            </span>
                        </div>
                    </div>
                    <div class="faChooseDiv grad-FFF-EEE" style="width: 50%; height: 100%;">
                        <div style="padding: 15px;">
                            <div id="bAddToFollow" class="normalButton"><img id="imgAddToFollow" runat="server" align="absmiddle" /> Add to Watch List</div>
                            <span>
                                <p>This is an Athlete that you want to track. They do not have to approve this request.  You would follow athletes that you don't know on a personal level, but are still people who's training your interested in following and comparing.</p>
                                <ul>
                                    <li>Follow their posts and comments in your own stream.</li>
                                    <li>Compare your workouts.</li>
                                </ul>
                            </span>
                        </div>
                    </div>                                                
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
    </Windows>
</telerik:radwindowmanager>
  
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnResponseEnd="OnResponseEnd">     
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Office2007" />

<ati:LoadingPanel ID="atiLoading" runat="server" />

<asp:Panel ID="atiFitness" runat="server">
   <!-- <a href="javascript: Aqufit.Page.Actions.test();">.</a> -->
    <asp:Panel ID="panelAjax" runat="server" >
        <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
        <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
        <asp:HiddenField ID="hiddenWorkoutKey" runat="server" />    
        <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
    </asp:Panel>
    <div>
        <div id="ati_sidePanel" style="height: 100%; float: left;">        
            <!-- ** Left Panel Zone -->          
            <ati:Profile ID="atiProfile" runat="server" />                               
            <!-- ** END Left Panel Zone -->    
        </div>
        <div style="width: 728px; float: right;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr valign="top">
                <td colspan="2" style="padding-bottom: 7px;">           
                    <div class="profileHeading grad-FFF-EEE ui-corner-all">
                        <asp:Literal ID="lUserName" runat="server" />
                        <ul class="hlist">
                            <li><a id="aGroups" runat="server" title="My Groups"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iGroup_s.png")%>" /></span></a></li>
                            <li><a id="aPhotos" runat="server" title="View Photos"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCamera_s.png")%>" /></span></a></li>
                            <li><a id="aHistory" runat="server" title="Workout History"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iHistory_s.png")%>" /></span></a></li>
                            <li><a id="aStats" runat="server" title="Workout Stats"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iStats_s.png")%>" /></span></a></li>                          
                        </ul>
                    </div>                   
                    <!-- Tabs -->
            		<div id="tabs">
            			<ul>
            				<li id="tabHistory" runat="server"><a href="#<%=pageViewHistory.ClientID %>">View History</a></li>
            				<li id="tabWorkout" runat="server"><a href="#<%=pageViewWorkout.ClientID %>">Log Workout</a></li>
            				<li id="tabComment" runat="server"><a href="#<%=pageViewComment.ClientID %>">Shout</a></li>
            			</ul>
            			
                        <div id="pageViewHistory" runat="server" style="padding: 0px;">
                            <div style="width: 722px; height: 240px; text-align: center; padding-top: 10px; position: relative;" class="grad-panel">                                    
                                <ati:WorkoutSummaryHead ID="atiWorkoutSummaryHead" runat="server" />    
                                <ati:TotalDistColors id="atiTotalDistColors" runat="server" />
                                <ati:StrengthGraph ID="atiStrengthGraph" runat="server" />  
                                <ati:HighChart ID="atiHighCharts" runat="server" Height="180px" />                                                                                	
                           </div>
                        </div>

            			<div id="pageViewWorkout" runat="server" style="padding: 0;">
                            <asp:Panel ID="atiWorkoutPanel" runat="server" CssClass="grad-panel">                
                                <ati:Workout ID="atiWorkout" runat="server" OnWodItemsRequested="atiWorkout_WodItemsRequested" OnRouteItemsRequested="atiWorkout_RouteItemsRequested" />
                                <div style="position: relative; left: 20px; bottom: 10px;">
                                    <ati:StreamAttachment ID="atiWorkoutAttachment" runat="server" />
                                </div>
                                <br />
                                <button id="bSubmitWorkoutAjax" class="ati_Form_Button" style="float: right; margin-top: -20px; margin-right: 20px;">Post</button> 
                                <br />                                                               
                            </asp:Panel> 
                        </div>

            			<div id="pageViewComment" runat="server" style="padding: 0;">
                            <asp:Panel ID="atiCommentPanel" runat="server" CssClass="grad-panel">   
                                <ati:Comment ID="atiComment" runat="server" />   
                                <div style="position: relative; left: 20px; bottom: 10px;">
                                    <ati:StreamAttachment ID="atiCommentAttachment" runat="server" />
                                </div>
                                <br />
                                <button Id="bSubmitComment" class="ati_Form_Button" style="float: right; margin-top: -20px; margin-right: 20px;">Post</button>          
                                <br />         
                            </asp:Panel>
                        </div>
            		</div>
                    <!-- END Tabs -->                                                                                                                                      
                </td>   
            </tr>            
            <tr valign="top">
                <td style="width: 550px; padding-right: 16px;">              
                    <div>
                        <telerik:RadListView ID="RadListViewTags" runat="server" OnNeedDataSource="RadListViewTags_NeedDataSource" ItemPlaceholderID="PlaceHolder1" AllowPaging="true" DataKeyNames="Id">
                            <LayoutTemplate>
                                <telerik:RadDataPager ID="RadDataPager1" runat="server" PageSize="4" Skin="Default"  >
                                    <Fields>
                                        <telerik:RadDataPagerSliderField />
                                        <telerik:RadDataPagerButtonField FieldType="Numeric" />                                        
                                    </Fields>
                                </telerik:RadDataPager>
                                <asp:Panel ID="PhotoPanel" runat="server">                                        
                                    <ul class="hlist">
                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                    </ul>
                                    &nbsp;
                                </asp:Panel>                                                               
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="category atiPhoto">                                     
                                    <a href="<%=this.PhotoUrl %>?p=<%#Eval("Id")%>"><img ID="photoNorm" runat="server" src='<%#Eval("ThumbUri")%>' /></a>                                        
                                </li>
                            </ItemTemplate>
                            <SelectedItemTemplate>
                                <li class="selected atiPhoto">                                       
                                    <a href="<%=this.PhotoUrl %>?p=<%#Eval("Id")%>"><img ID="photoSelected" runat="server" src='<%#Eval("ThumbUri") %>' /></a>                                    
                                </li>
                            </SelectedItemTemplate>
                            <EmptyDataTemplate>
                                <% if( Permissions == AqufitPermission.OWNER ){
                                       RadListViewTags.Visible = false;
                                } %>
                            </EmptyDataTemplate>
                        </telerik:RadListView>
                        <ati:StepByStep ID="atiStepByStep" runat="server" Visible="false" />
                        <ati:StreamTutorial ID="atiStreamTutorial" runat="server" Visible="false" />
                     </div>
                     <asp:Panel ID="panelStreamPost" runat="server" Visible="false" style="position: relative;">   
                        <asp:Literal ID="litStreamPostTitle" runat="server" />
                        <span style="font-size: 9px; position: absolute; top: 10px; right: 60px;">max (1024 characters)</span>
                        <asp:TextBox ID="txtStreamPostTxt" runat="server" TextMode="MultiLine" Width="465px" MaxLength="1024" Height="18px"  CssClass="ui-corner-all ui-widget-content atiTxtBox" />                  
                        <ati:StreamAttachment ID="atiStreamPostAttachment" runat="server" />
                        <button id="bStreamPost" style="position: absolute; right: 0px; top: 30px;">Post</button>                                                                                 
                    </asp:Panel>

                    <button id="workoutList">Workout History</button>                    
                    <!-- ** MAIN FEED Area -->
                    <asp:Panel ID="atiStreamPanelAjax" runat="server" CssClass="atiMainFeed">
                        <ati:StreamScript ID="atiStreamScript" runat="server" ShowTopPager="false" DefaultTake="20" />                                 
                    </asp:Panel>
                    <!-- ** END MAIN FEED Area -->
                </td>
                <td align="right">                    
                    <a href="http://tastypaleo.com"><img id="imgAdRight" runat="server" /></a>
                </td>
            </tr>
            </table>
       </div>
       <div style="clear:both;"></div> 
    </div>    
    </asp:Panel>
         
    
               



