<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_CompLeaderBoard.ViewATI_CompLeaderBoard" CodeFile="ViewATI_CompLeaderBoard.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="FeaturedProfile" Src="~/DesktopModules/ATI_Base/controls/ATI_FeaturedProfile.ascx" %>
<%@ Register TagPrefix="ati" TagName="ShareLink" Src="~/DesktopModules/ATI_Base/controls/ATI_ShareLink.ascx" %>
<%@ Register TagPrefix="ati" TagName="CompetitionAthlete" Src="~/DesktopModules/ATI_Base/controls/ATI_CompetitionAthlete.ascx" %>
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
    .compAthlete a
    {
    	color: #faa01b !important;
    	font-size: 12px;
    	text-decoration: underline;
    }
    .compAthlete a:hover
    {
    	text-decoration: none;
    }
    .compAthete ul
    {
    	padding: 10px 40px;
    }
    .compAthete ul li
    {
    	list-style: none;
    }
    
</style>
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
<script language="javascript" type="text/javascript">

    Aqufit.Windows.CompAthleteDialog = {
        win: null,
        open: function (json) {
            Aqufit.Windows.CompAthleteDialog.win = $find('<%=CompAthleteDialog.ClientID %>');
            Aqufit.Windows.CompAthleteDialog.win.show();
        },
        close: function () {
            Aqufit.Windows.CompAthleteDialog.win.close();
        }
    };


    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        RowClick: function (sender, args) {
            Aqufit.Page.atiCompAthlete.selectedAthlete($('#' + args.get_id() + ' .compId').html());
            Aqufit.Windows.CompAthleteDialog.open();
        },
        RefreshGrid: function () {
            var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
            masterTable.rebind();
        },
        OnAffiliateClientSelectedIndexChanged: function (sender, args) {
            var item = args.get_item();
            if (item.get_value() != '') {
                //  top.location.href = Aqufit.Page.PageBase + 'workout/' + item.get_value();
                Aqufit.Page.Actions.RefreshGrid();
            }
        },
        TableCreated: function () {
            var athletes = [];
            var heights = [];
            var aHeight = 0;
            var weights = [];
            var aWeight = 0;
            var ages = [];
            var aAge = 0;
            $('.rgMasterTable tr').each(function (index, r) {
                var score = parseInt($(r).find('.compScore').html());
                if (!isNaN(score)) {
                    //  alert(score);
                    athletes.push({ Score: score, WorkoutType: Aqufit.WorkoutTypes.CROSSFIT, UserName: $(r).find('.compAthlete').html(), Id: $(r).find('.compId').html() });
                    var h = parseFloat($(r).find('.compHeight').html());
                    if (!isNaN(h)) {
                        heights.push({ UserName: $(r).find('.compAthlete').html(), Height: h });
                        aHeight += h;
                    }
                    var w = parseFloat($(r).find('.compWeight').html());
                    if (!isNaN(w)) {
                        weights.push({ UserName: $(r).find('.compAthlete').html(), Height: w });
                        aWeight += w;
                    }
                    var a = parseFloat($(r).find('.compAge').html());
                    if (!isNaN(a) && a > 0) {
                        ages.push({ UserName: $(r).find('.compAthlete').html(), Age: a });
                        aAge += a;
                    }
                }

            });
            aHeight = aHeight / heights.length;
            aWeight = aWeight / weights.length;
            aAge = aAge / ages.length;

            var inches = Aqufit.Units.convert(Aqufit.Units.UNIT_M, aHeight, Aqufit.Units.UNIT_INCHES);
            var feet = Math.floor(inches / 12);
            inches = Math.ceil(inches % 12);
            $('#aHeight').html('' + feet + "' " + inches + '"');
            var lbs = Aqufit.Units.convert(Aqufit.Units.UNIT_KG, aWeight, Aqufit.Units.UNIT_LBS);
            lbs = Math.round(lbs * 100) / 100;
            $('#aWeight').html(lbs + ' lbs');
            $('#aAge').html(''+ Math.round( aAge ));
            Aqufit.Page.atiCompHighChart.title = 'Overall Scores';
            Aqufit.Page.atiCompHighChart.genChart(athletes);
            Aqufit.Page.atiCompHighChart.drawChart();
            Aqufit.Page.atiCompHighChart.handlePointClick = function (name, x, y) {
                Aqufit.Page.atiCompAthlete.selectedAthlete(Aqufit.Page.atiCompHighChart.getData(name, x, y).Id);
                Aqufit.Windows.CompAthleteDialog.open();
            }
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

        $('#<%=rblSex.ClientID %>, #<%=ddlRegion.ClientID %>').change(function () {
            Aqufit.Page.Actions.RefreshGrid();
        });



        $('#bChartPageNext').button({ icons: { primary: 'ui-icon-circle-arrow-e' }, text: false }).click(function (event) {
            $('#bStreamNext2').trigger('click');
            event.stopPropagation();
            return false;
        });
    });

    Aqufit.addLoadEvent(function () {
                     
    });

     function onRequestStart(sender, args)
        {
            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToWordButton") >= 0 ||
                    args.get_eventTarget().indexOf("ExportToCsvButton") >= 0)
            {
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
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting> 
        <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
            </UpdatedControls>
        </telerik:AjaxSetting>    
        
        </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   



<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" EnableShadow="true">
    <Windows>    
        <telerik:RadWindow ID="CompAthleteDialog" runat="server" Skin="Black" Title="Competition Athlete" Width="400" Height="450" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
                <ContentTemplate>
                    <ati:CompetitionAthlete ID="atiCompAthlete" runat="server" />
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
        <div id="divMain" style="width: 100%; position: relative; min-height: 1300px;">            
            <!-- Tabs -->            
    		<div id="tabs">
    			<ul>
    				<li id="tabInbox"><a href="#pageViewWorkouts"><asp:Literal ID="workoutTabTitle" runat="server" /></a></li>    				              
                </ul>
    			<div id="pageViewWorkouts" style="padding: 0px; background-color: White; _clear:both;">                                                                                                        
                    <asp:Panel ID="atiWorkoutPanel" runat="server">                        
                        <div id="atiWorkoutSearch" runat="server">
                            <div class="atiSearchControls grad-FFF-EEE">                                                          
                                <div style="border: 2px solid #ccc;">
                                    <ati:WorkoutHighChart ID="atiCompHighChart" runat="server" Width="100%" Height="200px" />                                                       
                                </div>
                                <ul class="hlist">
                                    <li>Average Height: <em id="aHeight"></em></li>
                                    <li>&nbsp;&nbsp;&nbsp;&nbsp;</li>
                                    <li>Average Weight: <em id="aWeight"></em></li>
                                    <li>&nbsp;&nbsp;&nbsp;&nbsp;</li>
                                    <li>Average Age: <em id="aAge"></em></li>
                                </ul>
                            </div>                            
                        </div>
                        <div id="orderByPanel" class="atiSearchControls grad-FFF-EEE" style="position: relative; display: block; height: 85px;">   
                            <asp:RadioButtonList ID="rblSex" runat="server">
                                <asp:ListItem Text="Men" Value="M" Selected="True" />
                                <asp:ListItem Text="Women" Value="W" />
                                <asp:ListItem Text="Masters Men" Value="Y" />
                                <asp:ListItem Text="Masters Women" Value="X" />
                                <asp:ListItem Text="All" Value="A" />
                            </asp:RadioButtonList>
                            <div style="position: absolute; top: 10px; left: 150px;">                           
                                <ul>
                                    <li style="list-style: none;">Region:&nbsp;
                                    <asp:DropDownList ID="ddlRegion" runat="server" Width="250px">
                                    </asp:DropDownList></li>
                                    
                                    <li style="list-style: none;">Affiliate:
                                    <telerik:RadComboBox ID="atiRadComboSearchAffiliates" runat="server" Width="250px" Height="200px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                    EmptyMessage="Affiliate Name" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                    EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchAffiliate_ItemsRequested"
                                    OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnAffiliateClientSelectedIndexChanged">
                                    </telerik:RadComboBox></li>
                                </ul>
                            </div>
                            
                            <div style="position: absolute; top: 10px; right: 240px;" class="normalButton" onclick="top.location.href='/';"> 
                                Explore FlexFWD
                            </div>
                            <div style="position: absolute; top: 50px; right: 240px;" class="normalButton" onclick="top.location.href='/Register';"> 
                                Sign up
                            </div>
                            <div style="position: absolute; top: 10px; right: 10px; cursor: pointer;" onclick="top.location.href='/haypye';">                                
                                <h3>Featured FlexFWD Athlete</h3>
                                <asp:Literal ID="litRank" runat="server" />
                                <ati:FeaturedProfile ID="atiFeaturedProfile" runat="server" Small="true" />                                
                            </div>                            
                        </div>   
                        <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="LeaveTargetAndToolTip"
                            Width="350" Height="250" runat="server" EnableShadow="true" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element"
                            Position="MiddleRight">
                        </telerik:RadToolTipManager>
                        <telerik:RadGrid ID="RadGrid1" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="true" ShowGroupPanel="True"  OnItemCommand="RadGrid1_ItemCommand"
                             AllowPaging="True" PageSize="60" runat="server" AllowSorting="true" AllowFilteringByColumn="true" OnItemDataBound="RadGrid1_ItemDataBound"
                             OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="None"
                             ClientSettings-ClientEvents-OnRowClick="Aqufit.Page.Actions.RowClick" 
                             ClientSettings-ClientEvents-OnMasterTableViewCreated="Aqufit.Page.Actions.TableCreated">
                             <ExportSettings HideStructureColumns="false" />
                          <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="Top">
                          <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true" ShowAddNewRecordButton="false" />
                            <Columns> 
                                <telerik:GridTemplateColumn ItemStyle-CssClass="compFlex" HeaderText="" SortExpression="FlexId" HeaderButtonType="TextButton" DataField="FlexId" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:Label ID="numberLabel" runat="server" />
                                    <%# DisplayFlexIcon(Container)%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn ItemStyle-CssClass="compId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                                <telerik:GridBoundColumn SortExpression="RegionKey" HeaderText="RegionKey" HeaderButtonType="TextButton" DataField="RegionKey" Display="false" />
                                <telerik:GridBoundColumn SortExpression="Sex" HeaderText="Sex" HeaderButtonType="TextButton" DataField="Sex" Display="false" />                                
                                <telerik:GridBoundColumn ItemStyle-CssClass="compHeight"  SortExpression="Height" HeaderText="Height" HeaderButtonType="TextButton" DataField="Height" Display="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compWeight"  SortExpression="Weight" HeaderText="Weight" HeaderButtonType="TextButton" DataField="Weight" Display="false" />
                                <telerik:GridBoundColumn SortExpression="Country" HeaderText="Country" HeaderButtonType="TextButton" DataField="Country" Display="false" />
                                <telerik:GridBoundColumn SortExpression="Hometown" HeaderText="Hometown" HeaderButtonType="TextButton" DataField="Hometown" Display="false" />
                                <telerik:GridBoundColumn SortExpression="UId" HeaderText="UId" HeaderButtonType="TextButton" DataField="UId" AllowFiltering="false" Display="false" />        
                                <telerik:GridTemplateColumn ItemStyle-CssClass="compAthlete" HeaderText="Athlete" SortExpression="AthleteName" HeaderButtonType="TextButton" DataField="AthleteName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='<%# Eval("AthleteName") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn ItemStyle-CssClass="compAge"  SortExpression="Age" HeaderText="Age" HeaderButtonType="TextButton" DataField="Age" Display="true" HeaderStyle-Width="25px" AllowFiltering="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compAffilate" SortExpression="AffiliateName" HeaderText="Affiliate" HeaderButtonType="TextButton" DataField="AffiliateName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  />
                                <telerik:GridBoundColumn SortExpression="RegionName" HeaderText="Region" HeaderButtonType="TextButton" DataField="RegionName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  />                                
                                <telerik:GridBoundColumn ItemStyle-CssClass="compRank" SortExpression="OverallRank" HeaderText="Rank" HeaderButtonType="TextButton" DataField="OverallRank" AllowFiltering="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compScore" SortExpression="OverallScore" HeaderText="Score" HeaderButtonType="TextButton" DataField="OverallScore" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W1Rank" HeaderText="W1 Rank" HeaderButtonType="TextButton" DataField="W1Rank" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W1Score" HeaderText="W1 Score" HeaderButtonType="TextButton" DataField="W1Score" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W2Rank" HeaderText="W2 Rank" HeaderButtonType="TextButton" DataField="W2Rank" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W2Score" HeaderText="W2 Score" HeaderButtonType="TextButton" DataField="W2Score" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W3Rank" HeaderText="W3 Rank" HeaderButtonType="TextButton" DataField="W3Rank" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W3Score" HeaderText="W3 Score" HeaderButtonType="TextButton" DataField="W3Score" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W4Rank" HeaderText="W4 Rank" HeaderButtonType="TextButton" DataField="W4Rank" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W4Score" HeaderText="W4 Score" HeaderButtonType="TextButton" DataField="W4Score" AllowFiltering="false" />  
                                <telerik:GridBoundColumn SortExpression="W5Rank" HeaderText="W5 Rank" HeaderButtonType="TextButton" DataField="W5Rank" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W5Score" HeaderText="W5 Score" HeaderButtonType="TextButton" DataField="W5Score" AllowFiltering="false" /> 
                                <telerik:GridBoundColumn SortExpression="W6Rank" HeaderText="W6 Rank" HeaderButtonType="TextButton" DataField="W6Rank" AllowFiltering="false" />
                                <telerik:GridBoundColumn SortExpression="W6Score" HeaderText="W6 Score" HeaderButtonType="TextButton" DataField="W6Score" AllowFiltering="false" />                                                                          
                            </Columns>                                
                          </MasterTableView>
                          <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="True" AllowColumnsReorder="False">
                            <Selecting AllowRowSelect="True"></Selecting>
                         </ClientSettings>
                          <PagerStyle Mode="NextPrevAndNumeric" Position="TopAndBottom" />    
                          <GroupingSettings CaseSensitive="false" />                                             
                        </telerik:RadGrid>                    
                    </asp:Panel> 

                </div>
          			
    		</div>
            <!-- END Tabs -->                       
        </div>        
    </div>   
    <div style="clear:both;"></div>            