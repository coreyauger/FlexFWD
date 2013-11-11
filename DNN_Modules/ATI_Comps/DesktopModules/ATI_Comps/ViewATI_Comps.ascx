<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Comps.ViewATI_Comps" CodeFile="ViewATI_Comps.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="GMap" Src="~/DesktopModules/ATI_Base/controls/ATI_GMap.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnGroupResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>   
        <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>               
    </AjaxSettings>        
</telerik:RadAjaxManager>

<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" /> 
<ati:LoadingPanel ID="atiLoading" runat="server" />

<asp:Panel ID="panelAjax" runat="server">
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxButton" runat="server" OnClick="bAjaxPostback_Click" style="display: none;" />
</asp:Panel>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" />

<asp:Panel ID="atiGroupListPanel" runat="server" Visible="false">

    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server"> 
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>

    <script type="text/javascript" >

        Aqufit.Page.Tabs = {
            SwitchTab: function (ind) {
                $('#tabs').tabs('select', ind);
            }
        };

        Aqufit.Page.Actions = {
            OnClientSelectedIndexChangedEventHandler: function (sender, args) {
                var item = args.get_item();
                if (item.get_value() != '') {    // User has selected the "add new map"
                    var val = eval('(' + item.get_value() + ')');
                    self.location.href = Aqufit.Page.PageBase + 'group/' + val.UserName;
                }
            },
            RowClick: function (sender, args) {
                var cid =  $('#' + args.get_id() + ' .compId').html();
                if( cid == 1 ){
                    self.location.href = '<%=ResolveUrl("~/Community/CompetitionResults") %>?c=' + cid;
                }else{
                    self.location.href = '<%=ResolveUrl("~/Community/CompetitionScoring") %>?c=' + cid;
                }
            },
            SearchRoutes: function () {
                var address = $('#<%=atiRouteSearch.ClientID %>').val();
                Aqufit.Page.atiGMap.gotoAddress(address, null, function () {
                    // if we could not find a location ... do a postback for route names
                });
            },
            ShowFail: function (msg) {
                radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
            },
            ShowSuccess: function (msg) {
                radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
            }
        };
        function OnGroupResponseEnd(sender, eventArgs) { } // dummy function

        $(function () {
            $('#atiStatusWidget').hide();
            $('.dull').focus(function () {
                $(this).val('').removeClass('dull').unbind('focus');
            });
            $('#tabs').tabs({
                show: function (event, ui) {
                    if (ui.panel.id == "<%=pageFindComps.ClientID %>") {
                        Aqufit.Page.atiGMap.refresh();
                    }
                }
            });
            $('#bCreateCompetition').button().click(function (event) {
                alert('Feature comming soon');
                event.stopPropagation();
                return false;
            });
            if (Aqufit.Page.atiGMap) {
                Aqufit.Page.atiGMap.onPlacesResponse = function (places) {
                    Aqufit.Page.atiGroupSearch.generateStreamDom(places);
                }
            }
            //       Aqufit.Page.atiGMap.onMarkerClick = function (place) {
            //    Aqufit.Page.Actions.CenterPlace(place);
            //     }
        });        

        
    </script>    
    <style type="text/css">    
    div.atiStreamItemRight
    {
	    width: 380px !important;
    }
    </style>
    </telerik:RadCodeBlock>

  
    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" style="width: 100%">
    
        <div id="divMain" style="width: 760px; float: left;">            
            <!-- Tabs -->
    		<div id="tabs">
    			<ul>
                    <li id="tabCompes"><a href="#pageCompetitions">Competitions</a></li>
                    <li id="tabMyGroupComps" runat="server" visible="false"><a href="#<%=pageMyGroupComps.ClientID %>">My Group Competitions</a></li> 
    				<li id="tabFindComps" runat="server" visible="false"><a href="#<%=pageFindComps.ClientID %>">Find by location</a></li>                                
                </ul>
                <button id="bCreateCompetition" style="position: absolute; right: 6px; top: 6px;">New Competition</button>                      			
                
                <div id="pageCompetitions" style="padding: 0px; background-color: White;"> 
                    <div class="atiSearchControls grad-FFF-EEE">
                        <telerik:RadComboBox ID="atiRadComboBoxSearchComps" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                EmptyMessage="Search Competitions (eg: Taranis Winter Challenge)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true"
                                OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                                <WebServiceSettings Method="GetCompSearch" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
                        </telerik:RadComboBox>                        
                    </div>
                    <div class="groupFeature">
                        <div class="atiSearchControls grad-FFF-EEE" style=" position: relative;">                                                
                            <div style="width: 200px; min-height: 220px;">
                                <a id="hrefGroupLink2" runat="server"><ati:ProfileImage ID="atiFGProfileImg" runat="server" IsOwner="false" /></a>                            
                            </div>
                            <div style="width: 500px; position: absolute; right: 10px; top: 0px;">
                                <h2><a id="hrefCompName" runat="server">2011 Taranis Winter Challenge</a></h2>
                                <asp:Literal ID="litGroupDescription" runat="server" />
                                <div style="margin-top: 5px; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc; background: #fff;">
                                    <div class="atiListHeading grad-FFF-EEE">
                                    Freatured Competition
                                    </div>
                                    <div style="padding: 10px;">
                                        The games season is all about competition. Our Winter Challenge event is a nice, fun, team building competition at the halfway point to the games season.  Last year we had competitors from Kelowna to Bellingham and everywhere in between, and all had a great time.  This year is looking to be bigger better, and a whole lot of fun!
                                    </div>
                                </div>
                                <h1>Registration is closed.  Event is full.</h1>
                                <div>                                    
                                    <a href="/Community/CompetitionScoring?c=3"><span class="boldButton" style="float: right;">View Live Results</span></a>&nbsp;&nbsp;
                                    <a href="/Community/CompetitionRegistration.aspx?step=1&c=3&l=1"><span class="boldButton" style="float: right;">See who is registered</span></a>
                                </div>
                            </div>
                            
                        </div>                        
                    </div>
                    <div id="panelActiveGroups">
                        <div class="atiListHeading grad-FFF-EEE">
                            Past Competitions
                        </div>
                        <telerik:RadGrid ID="RadGrid1" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="true" ShowGroupPanel="False" OnItemCommand="RadGrid1_ItemCommand"
                                AllowPaging="True" PageSize="60" runat="server" AllowSorting="true" AllowFilteringByColumn="true" OnItemDataBound="RadGrid1_ItemDataBound"
                                OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="None"
                                ClientSettings-ClientEvents-OnRowClick="Aqufit.Page.Actions.RowClick">
                                <ExportSettings HideStructureColumns="false" />
                            <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="Top">
                            <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true" ShowAddNewRecordButton="false" />
                            <Columns>                                         
                                <telerik:GridBoundColumn ItemStyle-CssClass="compId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                                <telerik:GridBoundColumn ItemStyle-CssClass="compName" SortExpression="Name" HeaderText="Competition Name" HeaderButtonType="TextButton" DataField="Name" Display="true" AutoPostBackOnFilter="true"  CurrentFilterFunction="Contains" ShowFilterIcon="false" />                                
                                <telerik:GridBoundColumn ItemStyle-CssClass="compGroup"  SortExpression="Organizer" HeaderText="Group" HeaderButtonType="TextButton" DataField="Group" Display="true" AutoPostBackOnFilter="true"  CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compStart"  SortExpression="StartDate" HeaderText="StartDate" HeaderButtonType="TextButton" DataField="StartDate" Display="true" AutoPostBackOnFilter="true"  AllowFiltering="true" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compEnd"  SortExpression="EndDate" HeaderText="EndDate" HeaderButtonType="TextButton" DataField="EndDate" Display="true" AutoPostBackOnFilter="true"  AllowFiltering="true" />
                                        
                                <telerik:GridTemplateColumn ItemStyle-CssClass="action" HeaderText="Actions" HeaderButtonType="TextButton" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='View Results'></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                    
                            </Columns>                                
                            </MasterTableView>
                            <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="false" AllowColumnsReorder="False">
                            <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" Position="TopAndBottom" />    
                            <GroupingSettings CaseSensitive="false" />                                             
                        </telerik:RadGrid>  
                        
                    </div>
                </div>

                <div id="pageMyGroupComps" style="padding: 0px; background-color: White;" runat="server" visible="false">        
                    <div class="atiSearchControls grad-FFF-EEE">
                        
                    </div>
                </div>

    			<div id="pageFindComps" style="padding: 0px; background-color: White;" runat="server" visible="false">            
                    <div class="atiSearchControls grad-FFF-EEE">
                        <ul class="hlist">
                            <li><asp:TextBox ID="atiRouteSearch" runat="server" Width="450px"  CssClass="ui-corner-all ui-widget-content atiTxtBox dull" Text="search by address." /></li>
                            <li><a href="javascript: Aqufit.Page.Actions.SearchRoutes();" title="Search"><span class="grad-FFF-EEE rounded seachButton"><img id="imgSearch" runat="server" /></span></a></li>
                        </ul>
                    </div>
                    <ati:GMap id="atiGMap" runat="server" Mode="GROUP_FINDER" Height="300px" />
                    <ati:FriendListScript id="atiGroupSearch" runat="server" ControlMode="GROUP_LIST" Title="Competitions located on map" />                  
                </div>            			
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; float: right;">
            <a href="http://tastypaleo.com"><img runat="server" id="imgAd" /></a>
        </div>
    
    </div>   
    <div style="clear:both;"></div>  

</asp:Panel>        








    

    

         
    
               



