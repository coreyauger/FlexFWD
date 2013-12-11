<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Routes.ViewATI_Routes" CodeFile="ViewATI_Routes.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="RouteListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_RouteListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="GMap" Src="~/DesktopModules/ATI_Base/controls/ATI_GMap.ascx" %>
<%@ Register TagPrefix="ati" TagName="ShareLink" Src="~/DesktopModules/ATI_Base/controls/ATI_ShareLink.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">
<style type="text/css">

    div.atiRouteListControls,
    div.atiGMapControls
    {
	    padding: 15px 10px;
	    border-bottom: 1px solid #CCC;
	    position: relative;
    }
    
    
    
    div#routeInfo
    {
    	position: absolute;
    	right: 10px;
    	top: 0px;
    }
    div#routeInfo h2
    {
    	font-size: 24px;
    	color: #0095CD;
    	margin: 0;
    }    

    div#routeAddedPanel div
    {
    	padding: 10px 20px 5px 20px;
    }
    
    div#routeAddedPanel div span
    {
    	font-size: 16px;
    }
  /*  div#routeAddedPanel div > ul
    {
    	position: relative;
    	top: 60px;
    }
*/
    
.routeFeed div.atiStreamItemRight
{
	width: 660px;
}

    button#bCloseDialog
    {
    	position: absolute;
    	right: 25px;
    	bottom: 40px;
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

    Aqufit.Windows.RouteAddedDialog = {
        win: null,
        open: function (json) {
            Aqufit.Windows.RouteAddedDialog.win = $find('<%=RouteAddedDialog.ClientID %>');
            Aqufit.Windows.RouteAddedDialog.win.show();
        },
        close: function () {
            Aqufit.Windows.RouteAddedDialog.win.close();
        }
    };


    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        OnRoutesClientSelectedIndexChanged: function (sender, args) {
            var item = args.get_item();
            if (item.get_value() != '') {
                top.location.href = Aqufit.Page.PageBase + 'route/' + item.get_value();
            }
        },
        SearchRoutes: function () {
            var address = $('#<%=atiRouteSearch.ClientID %>').val();
            Aqufit.Page.atiGMap.gotoAddress(address, null, function () {
                // if we could not find a location ... do a postback for route names
            });
        },
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
        }
    };

    $(function () {
        $('#tabs').tabs();

        $('.dull').focus(function () {
            $(this).val('').removeClass('dull').unbind('focus');
        });
        $('button#atiRouteCreate').button().click(function (event) {
            if (Aqufit.Page.UserSettingsId > 0) {
                Aqufit.Windows.MapWin.open();
            } else {
                self.location.href = '<%=ResolveUrl("~/Login.aspx") %>';
            }
            event.stopPropagation();
            return false;
        });
        $('#atiRouteFind').button().click(function (event) {
            top.location.href = '<%=ResolveUrl("~/Community/Routes.aspx") %>';
            event.stopPropagation();
            return false;
        });
        $('#bCloseDialog').button().click(function (event) {
            Aqufit.Windows.RouteAddedDialog.close();
            event.stopPropagation();
            return false;
        });
        $('#bMyRoutes, #bMyRoutes2').button().click(function (event) {
            top.location.href = '<%=ResolveUrl("~/Profile/MyRoutes") %>';
            event.stopPropagation();
            return false;
        });
        $('#<%=bAddRoute.ClientID %>').css('z-index', '999').button().click(function (event) {
            if (Aqufit.Page.UserSettingsId > 0) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('addRoute');
                $('#<%=hiddenAjaxValue.ClientID %>').val('<%=hiddenRouteKey.Value %>');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Route","ATI_Route") %>', '');
                $(this).fadeOut('normal');
            } else {
                top.location.href = '<%=ResolveUrl("~/Login") %>?returnURL=' + top.location.href;
            }
            event.stopPropagation();
            return false;
        });
        $('#bLogRoute').css('z-index', '999').button().click(function (event) {            
            top.location.href = '<%=ResolveUrl("~/")%>' + Aqufit.Page.UserName + '?r='+$('#<%=hiddenRouteKey.ClientID %>').val();
            event.stopPropagation();
            return false;
        });
        $('#bShareRoute').button().click(function (event) {
            alert("TODO:");
            event.stopPropagation();
            return false;
        });
        Aqufit.EnterKeyHandler = function () {
            Aqufit.Page.Actions.SearchRoutes();
        };
    });

    Aqufit.addLoadEvent(function () {
        if (Aqufit.Page.atiRouteListScript) {
            Aqufit.Page.atiRouteListScript.addRouteToFavCallback = function (id) {
                // TODO: check if loged in .. if not redirect to login... would be nice if they could bounce back to the route after login.
                if (Aqufit.Page.UserSettingsId > 0) {
                    $('#<%=hiddenAjaxAction.ClientID %>').val('addRoute');
                    $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Route","ATI_Route") %>', '');
                } else {
                    self.location.href = '<%=ResolveUrl("~/Login") %>?returnUrl=' + self.location.href;
                }
            };
            Aqufit.Page.atiRouteListScript.remRouteToFavCallback = function (id, sel) {
                // TODO: check if loged in .. if not redirect to login... would be nice if they could bounce back to the route after login.
                if (Aqufit.Page.UserSettingsId > 0) {
                    if (confirm("Are you sure you want to remove this route?")) {
                        $('#<%=hiddenAjaxAction.ClientID %>').val('remRoute');
                        $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                        __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Route","ATI_Route") %>', '');
                        $(sel).hide("slow").children().remove();
                    }

                } else {
                    self.location.href = '<%=ResolveUrl("~/Login") %>?returnUrl=' + self.location.href;
                }
            };
        }
        if (Aqufit.Page.atiStreamScript) {
            Aqufit.Page.atiStreamScript.streamDeleteCallback = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delStream');
                $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Route","ATI_Route") %>', '');
            };
            Aqufit.Page.atiStreamScript.onDeleteComment = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delComment');
                $('#<%=hiddenAjaxValue.ClientID %>').val(id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Route","ATI_Route") %>', '');
            };
        }
        if (Aqufit.Page.atiGMap) {
            var $lat = $('#<%=hiddenLat.ClientID %>');
            var $lng = $('#<%=hiddenLng.ClientID %>');
            var $zoom = $('#<%=hiddenZoom.ClientID %>');
            Aqufit.Page.atiRouteListScript.dataBinder = function (skip, take) {
                Aqufit.Page.atiRouteListScript.showLoading();
                Affine.WebService.StreamService.GetRoutes($lat.val(), $lng.val(), Aqufit.Page.atiGMap.circleRadius, skip, take, "date", function (json) {
                    Aqufit.Page.atiRouteListScript.generateStreamDom(json);
                }, function () { alert('err'); });
            }
            Aqufit.Page.atiGMap.locationChangeCallback = function (lat, lng) {
                // TODO: need to store map viewstate when someone views a route then returns back... 
                $lat.val(lat);
                $lng.val(lng);
                $zoom.val(Aqufit.Page.atiGMap.getMap().getZoom());
                Aqufit.Page.atiRouteListScript.showLoading();
                Affine.WebService.StreamService.GetRoutes(lat, lng, Aqufit.Page.atiGMap.circleRadius, 0, 10, "date", function (json) {
                    Aqufit.Page.atiRouteListScript.generateStreamDom(json);
                });
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



<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" EnableShadow="true">
    <Windows>    
        <telerik:RadWindow ID="RouteAddedDialog" runat="server" Skin="Black" Title="Route Added" Width="400" Height="200" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
                <ContentTemplate>
                    <div id="routeAddedPanel" style="width: 100%; height: 100%; background-color: white;">
                        <div>
                            <span><asp:Image ID="imgCheck" runat="server" />Route has been added.</span>
                            <ul class="hlist">
                                <li><button id="bMyRoutes">view my routes</button></li>
                                <li><button id="bShareRoute">share route</button></li>
                            </ul>                               
                            <button id="bCloseDialog">Close</button>                          
                        </div>
                    </div>
                </ContentTemplate>
        </telerik:RadWindow>
        </Windows>
</telerik:radwindowmanager>

 <asp:Panel ID="panelAjax" runat="server" >   
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

    <asp:HiddenField ID="hiddenRouteKey" runat="server" />
    <asp:HiddenField ID="hiddenLat" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hiddenLng" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hiddenZoom" runat="server" EnableViewState="true" />
    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper">
        <div id="divMain" style="width: 764px; float: left; margin-right: 8px; position: relative;">            
            <!-- Tabs -->
            <ul class="hlist atiTopButtons">                
                <li id="liFindRoute" runat="server" visible="false"><button id="atiRouteFind">Find Route</button></li>
                <li id="liMyRoutes" runat="server"><button id="bMyRoutes2">My Routes</button></li>               
                <li><button id="atiRouteCreate">Create Route</button></li>
            </ul>
    		<div id="tabs">
    			<ul>
    				<li id="tabInbox"><a href="#pageViewRoutes"><asp:Literal ID="routeTabTitle" runat="server" /></a></li>    				              
                </ul>
    			<div id="pageViewRoutes" style="padding: 0px; background-color: White;">     
                    <asp:Panel ID="atiMyRoutePanel" runat="server" Visible="false"> 
                        <div class="atiRouteListControls grad-FFF-EEE">
                            <telerik:RadComboBox ID="atiRadComboBoxSearchRoutes" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                    EmptyMessage="Search My Routes by Name" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                    EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchRoutes_ItemsRequested"
                                    OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnRoutesClientSelectedIndexChanged">
                                </telerik:RadComboBox>  
                        </div>
                    </asp:Panel>                                                                                                  
                    <asp:Panel ID="atiRoutePanel" runat="server">                        
                        <div id="mapContainer" runat="server">                            
                            <div id="atiMapContainer">
                                <div class="atiGMapControls grad-FFF-EEE">
                                    <div style="position: absolute; right: 10px;">
                                        <ul class="hlist">
                                            <li><a href="javascript: Aqufit.Page.atiGMap.centerRouteMarker();" title="Center Map"><span class="grad-FFF-EEE rounded seachButton"><img id="imgCenter" runat="server" /></span></a></li>
                                            <li><span class="toolDiv">|</span></li>
                                            <li><a href="javascript: Aqufit.Page.atiGMap.adjustSearchRadius(2.5);" title="Small Radius"><span class="grad-FFF-EEE rounded seachButton" style="font-size: 6px;">S</span></a></li>
                                            <li><a href="javascript: Aqufit.Page.atiGMap.adjustSearchRadius(5.0);" title="Medium Radius"><span class="grad-FFF-EEE rounded seachButton" style="font-size: 10px;">M</span></a></li>
                                            <li><a href="javascript: Aqufit.Page.atiGMap.adjustSearchRadius(10.0);" title="Large Radius"><span class="grad-FFF-EEE rounded seachButton" style="font-size: 14px;">L</span></a></li>
                                        </ul>
                                    </div>
                                    <ul class="hlist">
                                        <li><asp:TextBox ID="atiRouteSearch" runat="server" Width="450px"  CssClass="ui-corner-all ui-widget-content atiTxtBox dull" Text="search by address." /></li>
                                        <li><a href="javascript: Aqufit.Page.Actions.SearchRoutes();"><span class="grad-FFF-EEE rounded seachButton"><img id="imgSearch" runat="server" /></span></a></a></li>
                                    </ul>
                                </div>
                                <ati:GMap id="atiGMap" runat="server" Mode="ROUTE_FINDER" Width="100%" Height="300px" />      
                            </div>                      
                            <div class="atiRouteListControls grad-FFF-EEE">                            
                                <!--
                                <div style="position: absolute; right: 10px;">
                                    <ul class="hlist">
                                        <li>Date</li>
                                        <li>Distance</li>
                                        <li>Popular?</li>
                                    </ul>
                                </div>
                                -->
                                <ul class="hlist">
                                    <li><a href="javascript: Aqufit.Page.Actions.toggleMapView();">hide/show map</a></li>
                                </ul>
                            </div>
                        </div>  
                        <ati:RouteListScript id="atiRouteListScript" runat="server" />
                    </asp:Panel> 
                    <asp:Panel ID="atiRouteViewer" runat="server" Visible="false"> 
                        <div class="atiRouteListControls grad-FFF-EEE">
                            <div id="routeInfo">
                                <h2><asp:Literal ID="lRouteTitle" runat="server" /></h2>
                                <asp:Literal ID="lRouteInfo" runat="server" />
                            </div>                            
                            <div style="position: absolute; right: 10px; bottom: 10px;">
                                <ul class="hlist">
                                    <li><button id="bLogRoute">Log this Route</button></li>                                    
                                    <li><span class="toolDiv">|</span></li>
                                    <li><button id="bAddRoute" runat="server">Add to My Routes</button></li>                                   
                                </ul>
                            </div>
                            <ati:ProfileImage small="true" id="atiProfileImg" runat="server" />        
                        </div>  
                        <div style="padding-top: 15px;" class="atiRouteListControls grad-FFF-EEE">
                            <ati:ShareLink ID="atiShareLink" runat="server" TextBoxWidth="500px" TextBoxCssClass="ui-corner-all ui-widget-content atiTxtBox shareLink" />      
                        </div>            
                        
                        <div id="mapWrapper">
                            <ati:GMap id="atiGMapView" runat="server" Mode="ROUTE_VIEWER" Width="100%" Height="500px" />      
                        </div>
                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Route Action</h3>
                        </div>
                        <div>
                            <asp:Panel ID="atiStreamPanelAjax" runat="server" CssClass="routeFeed">
                                <ati:StreamScript ID="atiStreamScript" runat="server" ShowTopPager="false" />                       
                            </asp:Panel>
                        </div>

                        <div class="atiListHeading grad-FFF-EEE">
                            <h3>Routes near by</h3>
                        </div>

                        <ati:RouteListScript id="atiSimilarRouteListScript" runat="server" />
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
               










    

    

         
    
               



