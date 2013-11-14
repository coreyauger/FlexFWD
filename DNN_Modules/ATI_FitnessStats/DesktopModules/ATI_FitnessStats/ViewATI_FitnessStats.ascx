<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_FitnessStats.ViewATI_FitnessStats" CodeFile="ViewATI_FitnessStats.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="NameValue" Src="~/DesktopModules/ATI_Base/controls/ATI_NameValueGrid.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    div.atiRouteListControls,
    div.atiGMapControls
    {
	    padding: 15px 10px;
	    border-bottom: 1px solid #CCC;
	    position: relative;	    
    }
    div.atiGMapControls
    {
    	min-height: 150px;
    }
    
    div#orkoutTotalsPanel
    {
    	position: absolute; 
    	right: 10;
    	top: 10;
    }
    span.name
    {
    	padding: 10px 20px;
    	color: #999;
    	font-size: 13px;
    }
    span.value
    {
    	padding: 10px 20px;
    	color: #666;
    	font-size: 13px;
    	font-weight: bold;
    }
    
    .pdfButton,
    .excelButton
    {
    	width: 50%;    	
    }
    
.atiGadgetContainer
{
	padding-bottom: 7px;
}

div.atiControlsContainer ul li
{
	list-style-type:none;
}
DIV.panelProfileImage
{
	overflow: hidden; 
}
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">                    
<script type="text/javascript" >       

    function AjaxResponseFail() {
        alert("FAIL");
    }
    function onRequestStart(sender, args) {
        if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0) {
            args.set_enableAjax(false);
        }
    }
</script>
</telerik:RadCodeBlock>

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">  
    <ClientEvents OnRequestStart="onRequestStart" />
    <AjaxSettings>               
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>    
        <telerik:AjaxSetting AjaxControlID="RadGrid2">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>     
    </AjaxSettings>
    </telerik:RadAjaxManager>
   
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" BackColor="Transparent" runat="server" Skin="Office2007" />    
 
 
 <telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>
        <telerik:RadWindow ID="LoadingModal" runat="server" Skin="Black" Height="200px" Title="Loading Workout" VisibleOnPageLoad="false" Behaviors="Move" EnableShadow="true" Modal="true">
                <ContentTemplate>
                    <div id="workoutStatusPanel" style="width: 100%; height: 100%; background-color: #eee;">
                        <div style="padding: 20px;">
                        <center><img id="imgLoading" runat="server" />
                        <h2 class="loading">Loading Workout</h2>
                        </center>
                        </div>
                    </div>                    
                </ContentTemplate>
        </telerik:RadWindow>       
    </Windows>
</telerik:radwindowmanager>   

    <asp:HiddenField ID="hiddenProfileJson" runat="server" />
    <asp:HiddenField ID="hiddenFriendJson" runat="server" />
    <asp:HiddenField ID="hiddenWatchJson" runat="server" />
    <asp:HiddenField ID="hiddenGroupJson" runat="server" />
    <asp:HiddenField ID="hiddenWorkoutHistory" runat="server" />
    <asp:HiddenField ID="hiddenWorkoutData" runat="server" />

    <asp:Panel ID="panelAjax" runat="server" >                
        <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
        <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
        <asp:HiddenField ID="hiddenAjaxContext" runat="server" />
        <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
    </asp:Panel>


    <asp:Panel ID="atiStatsPanel" runat="server" style="height: 500px;">          
    
        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">                    
            <script type="text/javascript" >

                // This function returns the appropriate reference,
                // depending on the browser.
                function getFlexApp(appName) {
                    if (navigator.appName.indexOf("Microsoft") != -1) {
                        return window[appName];
                    }
                    else {
                        return document[appName];
                    }
                }

                function pushProfileToFlex(json) {
                    var fx = getFlexApp('ATI_Aqufit');
                    fx.flex_showUserProfile(json);
                }

                function pushWorkoutToFlex(workoutData) {
                    var fx = getFlexApp('ATI_Aqufit');
                    fx.flex_loadWorkout(workoutData);
                }

                Aqufit.Page.ATI_FitnessStats = {
                    hiddenFriendJson: null,
                    hiddenGroupJson: null,
                    hiddenPastWorkoutListJson: null,
                    hiddenWorkoutData: null,
                    hiddenWorkoutHistory: null,
                    clear: function () {
                        Aqufit.Page.ATI_FitnessStats.hiddenFriendJson.val('');
                        Aqufit.Page.ATI_FitnessStats.hiddenGroupJson.val('');
                        Aqufit.Page.ATI_FitnessStats.hiddenPastWorkoutListJson.val('');
                        Aqufit.Page.ATI_FitnessStats.hiddenWorkoutData.val('');
                        Aqufit.Page.ATI_FitnessStats.hiddenWorkoutHistory.val('');
                        Aqufit.Page.ATI_FitnessStats.hiddenProfileJson.val('');
                    }
                };

                function flashInit(val) {
                    var fx = getFlexApp('ATI_Aqufit');
                    fx.flex_loadProfile(Aqufit.Page.ATI_FitnessStats.hiddenProfileJson.val());
                    fx.flex_loadFriendList(Aqufit.Page.ATI_FitnessStats.hiddenFriendJson.val());
                    fx.flex_loadWorkoutHistory(Aqufit.Page.ATI_FitnessStats.hiddenWorkoutHistory.val());
                    fx.flex_loadGroupList(Aqufit.Page.ATI_FitnessStats.hiddenGroupJson.val());
                    fx.flex_loadWorkout(Aqufit.Page.ATI_FitnessStats.hiddenWorkoutData.val());
                }
                function getWorkout(wid, context) {
                    //Aqufit.Page.ATI_FitnessStats.clear();
                    $('#<%=hiddenAjaxAction.ClientID %>').val('getWorkout');
                    $('#<%=hiddenAjaxValue.ClientID %>').val(wid);
                    $('#<%=hiddenAjaxContext.ClientID %>').val(context);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$FitnessStats","ATI_FitnessStats") %>', '');
                }
                function getProfile(uname) {
                    //Aqufit.Page.ATI_FitnessStats.clear();
                    $('#<%=hiddenAjaxAction.ClientID %>').val('getProfile');
                    $('#<%=hiddenAjaxValue.ClientID %>').val(uname);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$FitnessStats","ATI_FitnessStats") %>', '');
                }

                $(function () {
                    Aqufit.Page.ATI_FitnessStats.hiddenFriendJson = $('#<%=hiddenFriendJson.ClientID %>');
                    Aqufit.Page.ATI_FitnessStats.hiddenGroupJson = $('#<%=hiddenGroupJson.ClientID %>');
                    Aqufit.Page.ATI_FitnessStats.hiddenWorkoutData = $('#<%=hiddenWorkoutData.ClientID %>');
                    Aqufit.Page.ATI_FitnessStats.hiddenWorkoutHistory = $('#<%=hiddenWorkoutHistory.ClientID %>');
                    Aqufit.Page.ATI_FitnessStats.hiddenProfileJson = $('#<%=hiddenProfileJson.ClientID %>');
                    var visWidth = $('#atiStatsContainer').width();
                    $('#atiStatsRight').css('width', (visWidth - 210) + 'px');
                    var h = $(window).height() - 180;
                    if (h < 500) h = 500;
                    $('#<%=atiStatsPanel.ClientID%>').height(h);
                });

            </script>            
            </telerik:RadCodeBlock>
    
                                                                                              
        <!-- Start of rich media stats -->
        <asp:Literal ID="atiWorkoutVisualizer" runat="server" />                          	
        <!-- END Start of rich media stats -->                                                                                   
    </asp:Panel>

    <!-- Workout Lists -->


    <asp:Panel ID="atiWorkoutList" runat="server" Visible="false">
        
        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">          
        <link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">          
            <script type="text/javascript" >

                Aqufit.Page.Controls.atiWorkoutSelector = {
                    radControl: null,
                    control: null,
                    OnClientSelectedIndexChangedEventHandler: function (sender, args) {
                        var item = args.get_item();
                        self.location.href = '<%=ResolveUrl("~") %>' + Aqufit.Page.UserName + "/workout/" + item.get_value();                        
                    }
                };

                Aqufit.Page.Actions = {
                    RowSelected: function (sender, eventArgs) {
                        var win = $find('<%=LoadingModal.ClientID %>');
                        win.show();
                        var grid = sender;
                        var MasterTable = grid.get_masterTableView();
                        var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
                        var cell = MasterTable.getCellByColumnUniqueName(row, "Id");
                        var wodKey = MasterTable.getCellByColumnUniqueName(row, "WODKey");
                        var cell2 = MasterTable.getCellByColumnUniqueName(row, "WorkoutTypeKey");
                        var src = MasterTable.getCellByColumnUniqueName(row, "DataSrc");
                        if (cell2.innerHTML == "Crossfit") {
                            //top.location.href = '<%=ResolveUrl("~") %>workout/' + wodcell.innerHTML;
                            top.location.href = '<%=ResolveUrl("~") %>workout/' + wodKey.innerHTML;
                        } else {
                            if ($(src.innerHTML).attr('title') == Aqufit.DataSrc.MANUAL_WITH_MAP) {
                                // we want to goto the route page...
                                top.location.href = '<%=ResolveUrl("~") %>workout/' + cell.innerHTML + '/route/';
                            } else {
                                top.location.href = '<%=ResolveUrl("~") %>' + Aqufit.Page.UserName + '/workout/' + cell.innerHTML;
                            }
                        }
                        //here cell.innerHTML holds the value of the cell
                    },
                    RowSelected2: function (sender, eventArgs) {
                        var win = $find('<%=LoadingModal.ClientID %>');
                        win.show();
                        var grid = sender;
                        var MasterTable = grid.get_masterTableView();
                        var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
                        var cell = MasterTable.getCellByColumnUniqueName(row, "Id");
                       // var wodKey = MasterTable.getCellByColumnUniqueName(row, "WODKey");
                       // alert(wodKey.innerHTML);
                        //       alert("This should just be in the context of the group: " + cell.innerHTML);
                        top.location.href = '<%=ResolveUrl("~") %>workout/' + cell.innerHTML + '?g=';
                    }
                };


                $(function () {
                    $('#tabs').tabs();
                    $('.dull').focus(function () {
                        $(this).val('').removeClass('dull');
                    });                    
                });

            </script>            
            </telerik:RadCodeBlock>


        <div id="divCenterWrapper">
        <div id="divMain" style="width: 764px; float: left; margin-right: 8px; position: relative;">            
           <!-- TODO: a search box to search messages -->
            <!-- Tabs -->
            <div id="tabs">
    			<ul>
    				<li id="tabInbox"><a href="#pageViewRoutes"><asp:Literal ID="workoutTabTitle" runat="server" /></a></li>    				              
                </ul>
    			<div id="pageViewRoutes" style="padding: 0px; background-color: White;">                                                                                                        
                    <asp:Panel ID="atiWorkoutListPanel" runat="server">                        
                        <div id="mapContainer" runat="server">                            
                            <div id="atiMapContainer">
                                <div class="atiGMapControls grad-FFF-EEE">
                                   <div class="workoutTotals" style="position: absolute; right: 10px; top: 20px;">
                                        <ati:NameValue ID="atiWorkoutTotals" runat="server" />
                                   </div>
                                   <ati:ProfileImage small="true" id="atiProfileImg2" runat="server" /> 
                                </div>
                            </div>                      
                            <div class="atiRouteListControls grad-FFF-EEE">                            
                                <ul class="hlist">
                                    <li>
                                    <telerik:RadComboBox ID="atiWorkoutSelector" runat="server" Height="190px" Width="100%" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                        EmptyMessage="Select a Workout" EnableLoadOnDemand="True" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                        OnItemsRequested="atiWorkoutSelector_WorkoutItemsRequested"
                                        OnClientSelectedIndexChanged="Aqufit.Page.Controls.atiWorkoutSelector.OnClientSelectedIndexChangedEventHandler">
                                    </telerik:RadComboBox>
                                    </li>      
                                </ul>                                                          
                            </div>
                        </div>                      
                        <telerik:RadGrid ID="RadGrid1" runat="server" PageSize="50" OnNeedDataSource="RadGrid1_NeedDataSource" ShowStatusBar="true" 
                            AllowPaging="True" AllowSorting="true" ShowGroupPanel="true" 
                            AllowFilteringByColumn="true" AutoGenerateColumns="false" GridLines="None" ClientSettings-ClientEvents-OnRowSelected="Aqufit.Page.Actions.RowSelected"
                            OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand">                                                         
                            <PagerStyle Mode="NumericPages"></PagerStyle>                              
                            <ClientSettings AllowGroupExpandCollapse="True" AllowDragToGroup="true" Selecting-AllowRowSelect="true"></ClientSettings>
                            <GroupingSettings ShowUnGroupButton="true" />                             
                            <MasterTableView Width="100%" AllowFilteringByColumn="true" TableLayout="Fixed" CommandItemDisplay="Top" >   
                            <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true" ShowAddNewRecordButton="false" />                                                                                           
                                <Columns>
                                    <telerik:GridNumericColumn SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" AllowFiltering="false" />                                                                      
                                    <telerik:GridNumericColumn SortExpression="WODKey" HeaderText="WODKey" HeaderButtonType="TextButton" DataField="WODKey" Display="false" AllowFiltering="false" />                                                                      
                                    <telerik:GridBoundColumn SortExpression="WorkoutTypeKey" HeaderText="Workout Type" HeaderButtonType="TextButton" DataField="WorkoutTypeKey" AllowFiltering="false" />
                                    <telerik:GridDateTimeColumn SortExpression="Date" HeaderText="Date" HeaderButtonType="TextButton" DataField="Date" DataFormatString="{0:MMM dd, yyyy}" />
                                    <telerik:GridBoundColumn SortExpression="Title" HeaderText="Title" HeaderButtonType="TextButton" DataField="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />                                    
                                    <telerik:GridBoundColumn SortExpression="Distance" HeaderText="Distance" HeaderButtonType="TextButton" DataField="Distance" AllowFiltering="false" />                                    
                                    <telerik:GridBoundColumn SortExpression="Duration" HeaderText="Time/Score" HeaderButtonType="TextButton" DataField="Duration" AllowFiltering="false" />                                                                 
                                    <telerik:GridNumericColumn SortExpression="Notes" HeaderText="Notes" HeaderButtonType="TextButton" DataField="Notes" AllowFiltering="false" />  

                                    <telerik:GridNumericColumn SortExpression="Score" HeaderText="Score" HeaderButtonType="TextButton" DataField="Score" Display="false">                                                                      
                                    </telerik:GridNumericColumn>

                                    <telerik:GridNumericColumn SortExpression="Max" HeaderText="Max" HeaderButtonType="TextButton" DataField="Max" Display="false">                                                                      
                                    </telerik:GridNumericColumn>
                                                                                                                                           
                                    <telerik:GridImageColumn DataType="System.Int16" DataImageUrlFields="DataSrc"  DataImageUrlFormatString="~/DesktopModules/ATI_Base/Resources/images/src{0}.png" AlternateText="Date Src" DataAlternateTextField="DataSrc" ImageAlign="Middle" ImageHeight="32px" ImageWidth="32px" HeaderText="Src" FooterText="ImageColumn footer" AllowFiltering="false" >
                                        <HeaderStyle Width="40px" />
                                    </telerik:GridImageColumn>                                   
                                </Columns>
                            </MasterTableView>                                                           
                        </telerik:RadGrid>
                        

                        <telerik:RadGrid ID="RadGrid2" runat="server" PageSize="50" ShowGroupPanel="true" ShowStatusBar="true" AllowPaging="True" AllowSorting="true" Visible="false"
                            OnNeedDataSource="RadGrid2_NeedDataSource"   OnItemCommand="RadGrid1_ItemCommand"
                            AllowFilteringByColumn="true" AutoGenerateColumns="False" GridLines="None" ClientSettings-ClientEvents-OnRowSelected="Aqufit.Page.Actions.RowSelected2">

                            <PagerStyle Mode="NumericPages"></PagerStyle>  
                            <ClientSettings AllowGroupExpandCollapse="True" ReorderColumnsOnClient="True" AllowColumnsReorder="True" AllowDragToGroup="true" Selecting-AllowRowSelect="true"></ClientSettings>
                            <GroupingSettings ShowUnGroupButton="true" />                             
                            <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Fixed" CommandItemDisplay="Top">  
                            <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true" ShowAddNewRecordButton="false" />                              
                                <Columns>
                                    <telerik:GridNumericColumn SortExpression="Id" HeaderText="Id" UniqueName="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" AllowFiltering="false">                                                                      
                                    </telerik:GridNumericColumn>                                   
                                                                        
                                    <telerik:GridDateTimeColumn SortExpression="Date" HeaderText="Date" UniqueName="Date" HeaderButtonType="TextButton" DataField="Date" DataFormatString="{0:MMM dd, yyyy}">
                                    </telerik:GridDateTimeColumn>                                    

                                    <telerik:GridBoundColumn SortExpression="Name" AutoPostBackOnFilter="true" HeaderText="Name" UniqueName="Name" HeaderButtonType="TextButton" DataField="Name" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn SortExpression="Type" HeaderText="Type" UniqueName="WODType" HeaderButtonType="TextButton" DataField="WODType" AllowFiltering="false">                                        
                                    </telerik:GridBoundColumn>                                                                       
                                                                      
                                </Columns>
                            </MasterTableView>                                                           
                        </telerik:RadGrid>

                    </asp:Panel> 
                    <asp:Panel ID="atiWorkoutViewer" runat="server" Visible="false"> 
                        <div class="atiRouteListControls grad-FFF-EEE">
                            <div id="routeInfo">
                                <h2><asp:Literal ID="lRouteTitle" runat="server" /></h2>
                                <asp:Literal ID="lRouteInfo" runat="server" />
                            </div>                            
                            <div style="position: absolute; right: 10;">
                                <ul class="hlist">                                    
                                    <li><span class="toolDiv">|</span></li>
                                    <li>Add</li>                                   
                                </ul>
                            </div>
                            This is where workout info goes... youtube videos.. ect..     
                        </div>  
                        <div style="padding-top: 15px;" class="atiRouteListControls grad-FFF-EEE">
                            Share Link:
                            <asp:TextBox ID="txtRouteLink" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" Width="450px" Text="TODO: " />                             
                        </div>            
                        
                        <div id="workoutsWrapper">
                             <asp:Panel ID="atiStreamPanelAjax" runat="server">
                                <ati:StreamScript ID="atiStreamScript" runat="server" ShowTopPager="false" ShowBottomPager="false" />                       
                            </asp:Panel>
                        </div>                        
                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Others</h3>
                        </div>

                       TODO: based on workout type... we could pull in friends data here..
                    </asp:Panel>
                </div>
          			
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; float: right;">
            <img runat="server" id="imgAd" />
        </div>
    
    </div>   
    <div style="clear:both;"></div>  

    </asp:Panel>       
                         

    

         
    
               



