<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_GMapRoute.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_GMapRoute" %>
<%@ Register TagPrefix="ati" TagName="WorkoutTypes" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutTypes.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style type="text/css">
.trans80 {
	filter:alpha(opacity=80);
	-moz-opacity:0.8;
	-khtml-opacity: 0.8;
	opacity: 0.8;
}
a.atiGroupNameLink
{
	color: f58220;
	font-size: 16px;
	font-weight: bold;
	text-decoration: underline;
}
a.atiGroupNameLink:hover
{
	text-decoration: none;
}
div.atiOdometer
{
	width: 100%;
	text-align: center;
	position: absolute;	
}
div.atiOdometer span
{
	font-size: 18px;
	font-weight: bold;
}
ul.atiMapTools li a.mapToolSelected
{
	background: #FFFFFF url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/mapTools.png")%>) fixed no-repeat 0px 0px; 
}

div.atiMapToolsPanel
{
	width: 52px; 
	height: 210px;
	border-right: 1px solid #666666;
	border-bottom: 1px solid #666666;
}

ul.atiMapTools
{
	background: #EFEFEF url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/mapTools.png")%>) no-repeat top left; 
	list-style:none;	
}
ul.atiMapTools li
{
	list-style:none;
}
ul.atiMapTools li a
{
	height: 35px;
	display: block;
}
ul.atiMapTools li a:hover
{
	background: #FFFFFF url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/mapTools.png")%>) fixed no-repeat 0px 0px; 
}

ul.atiMapTools li a.down
{
	background: #e87824 url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/mapTools.png")%>) fixed no-repeat 0px 0px; 
}
#bSave
{
	font-size: 12px;
	margin: 10px 20px;
}

</style>
 <telerik:RadCodeBlock ID="codeBlock1" runat="server">    
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
        
    <script type="text/javascript">

        Aqufit.Page.Controls.ATI_GMapRoute = function (id, map_canvas, units) {
            this.id = id;
            this.map = null;
            this.map_canvas = map_canvas;
            this.units = units;
            this.placeLocation = null;
            this.menuBar = null;
            this.json = null;
            this.allowGeolocation = false;
            this.usersGeoLocation = null;
            this.directionsService = new google.maps.DirectionsService();
            this.geocoder = new google.maps.Geocoder();
            this.directionsEnabled = true;
            this.ctime = null;
            this.timeout = 1000;
            this.markers = [],
            this.clickLatLng = [];
            this.poly = null;
            this.dist = 0;
            this.startMarker = null;
            this.endMarker = null;
            this.initalized = false;
            this.saveButtonClick = null;
        }

        function de_ra(d) {
            var pi = Math.PI;
            return (d * (pi / 180));           
        }

        Aqufit.Page.Controls.ATI_GMapRoute.prototype = {
            constructToolBar: function () {
                var toolBar = document.createElement('DIV');
                $(toolBar).addClass('atiMapToolsPanel')
                                .append('<ul class="atiMapTools">' +
                                            '<li><a href="javascript: ;" id="bUndo" title="Undo">&nbsp;</a></li>' +
                                            '<li><a href="javascript: ;" id="bCenterToPoly" title="Center">&nbsp;</a></li>' +
                                            '<li><a href="javascript: ;" id="bDeletePoly" title="Delete">&nbsp;</a></li>' +
                                            '<li><a href="javascript: ;" id="bLoopPoly" title="Loop Path">&nbsp;</a></li>' +
                                            '<li><a href="javascript: ;" id="bThereBack" title="There and Back">&nbsp;</a></li>' +
                                            '<li><a href="javascript: ;" class="mapToolSelected" id="bFollowRoads" title="Follow Roads">&nbsp;</a></li>' +
                                        '</ul>');
                toolBar.index = 5;
                return toolBar;
            },
            constructOdometer: function () {
                var od = document.createElement('DIV');
                if (this.units == Aqufit.Units.UNIT_MILES) {
                    $(od).addClass('atiOdometer').append('<div style="text-align: center;"><span id="distMiles">0 Mi</span><br /><span style="font-size: 9px;" id="distKm">0 Km</span></div>');
                } else {
                    $(od).addClass('atiOdometer').append('<div style="text-align: center;"><span id="distKm">0 Km</span><br /><span style="font-size: 9px;" id="distMiles">0 Mi</span></div>');
                }
                od.index = 5;
                return od;
            },
            constructSave: function () {
                var od = document.createElement('DIV');
                $(od).css({ 'position': 'absolute', 'right': '10px' });
                $(od).append('<button id="bSave">save route</button>');
                od.index = 6;
                return od;
            },
            // distance cals : http://www.movable-type.co.uk/scripts/latlong.html
            calculateDistance: function (latLng1, latLng2) {
                var R = 6371; // km earth rad
                var dLat = de_ra(latLng2.lat() - latLng1.lat());
                var dLon = de_ra(latLng2.lng() - latLng1.lng());
                var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) + Math.cos(de_ra(latLng1.lat())) * Math.cos(de_ra(latLng2.lat())) * Math.sin(dLon / 2) * Math.sin(dLon / 2);
                var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
                var d = R * c;
                return d;
            },
            updateUsersLocation: function () {
                // TODO: create an interval to keep getting the users location
            },
            deletePath: function () {
                if (this.poly) {
                    this.poly.setMap(null);
                }
                if (this.startMarker) {
                    this.startMarker.setMap(null);
                }
                if (this.endMarker) {
                    this.endMarker.setMap(null);
                }
                this.poly = null;
                this.clickLatLng = [];
                var polyOptions = {
                    strokeColor: '#ff0000',
                    strokeOpacity: 1.0,
                    strokeWeight: 3
                }

                this.poly = new google.maps.Polyline(polyOptions);
                this.poly.setMap(this.map);
            },
            centerMapToPolyLine: function () {
                var bb = new google.maps.LatLngBounds();
                var path = this.poly.getPath();
                for (var i = 0; i < path.length; i++) {
                    bb.extend(path.getAt(i));
                }
                this.map.fitBounds(bb);
                return bb;
            },
            addToolbarEvents: function () {
                var that = this;
                if ($('#bFollowRoads').size() == 0) {
                    setTimeout(function () { that.addToolbarEvents(); }, 500);
                }
                $('ul.atiMapTools li a').mousedown(function () {
                    $(this).addClass('down');
                }).mouseup(function () {
                    $(this).removeClass('down');
                });
                $('#bUndo').click(function () {
                    if (that.clickLatLng.length > 0) {
                        var path = that.poly.getPath();
                        var last = that.clickLatLng.pop();
                        for (var i = (path.length - 1); i > last.PathLen; i--) {
                            path.removeAt(i);
                        }
                        if (that.endMarker) {
                            that.endMarker.setMap(null);
                        }
                        if (path.length > 0) {
                            that.endMarker = new google.maps.Marker({
                                position: path.getAt(path.length - 1),
                                map: that.map,
                                title: 'end'
                            });
                        }
                    } else {
                        that.deletePath();
                    }
                });
                $('#bFollowRoads').click(function () {
                    if (that.directionsEnabled) {
                        $(this).removeClass('mapToolSelected');
                        that.directionsEnabled = false;
                    } else {
                        $(this).addClass('mapToolSelected');
                        that.directionsEnabled = true;
                    }
                })
                $('#bCenterToPoly').click(function () {
                    if (that.poly != null) {
                        that.centerMapToPolyLine();
                    }
                });
                $('#bDeletePoly').click(function () {
                    if (confirm('Are you sure you want to delete your route?')) {
                        that.deletePath();
                    }
                });
                $('#bLoopPoly').click(function () {
                    if (that.poly != null && that.poly.getPath().length > 1) {
                        var path = that.poly.getPath();
                        that.handleClick(path.getAt(0));
                    }
                });
                $('#bThereBack').click(function () {
                    if (that.poly != null && that.poly.getPath().length > 1) {
                        var path = that.poly.getPath();
                        that.clickLatLng.push({ LatLng: path.getAt(path.length - 1), PathLen: path.length });
                        for (var i = path.length - 1; i >= 0; i--) {
                            path.push(path.getAt(i));
                        }
                        if (that.endMarker) {
                            that.endMarker.setMap(null);    // clear end marker.
                        }
                        that.endMarker = new google.maps.Marker({
                            position: path.getAt(path.length - 1),
                            map: that.map,
                            title: 'end'
                        });
                        that.calcDistance();
                    }
                });
                $('#bSave').button({ icons: { primary: 'ui-icon-disk'} }).click(function (event) {
                    var path = that.poly.getPath();
                    $('#<%=atiRouteTitle.ClientID %>').val($('#atiRouteName').val());
                    $('#<%=atiRouteDistance.ClientID %>').val(Math.round(that.dist * 100) / 100);
                    $('#<%=atiRouteWorkoutType.ClientID %>').val(Aqufit.Page.atiWorkoutTypes.getSelectedDropDownVal());
                    that.centerMapToPolyLine();
                    // var point = that.map.fromLatLngToContainerPixel(bb.
                    $('#<%=atiFitZoomLevel.ClientID %>').val(that.map.getZoom());
                    var $mapDiv = $(that.map.getDiv());
                    $('#<%=atiMapWidth.ClientID %>').val($mapDiv.width());
                    $('#<%=atiMapHeight.ClientID %>').val($mapDiv.height());
                    var str = '['
                    for (var i = 0; i < path.length; i++) {
                        str += '{ Lat : ' + path.getAt(i).lat() + ', Lng : ' + path.getAt(i).lng() + ', Order : ' + i + '}';
                        if (i != (path.length - 1)) {
                            str += ', ';
                        }
                    }
                    str = str + ']';
                    $('#<%=atiLatLngArray.ClientID %>').val(str);
                    if (that.saveButtonClick != null) {
                           that.saveButtonClick();
                    }
                    event.stopPropagation();
                    return false;
                });
            },
            calcDistance: function () {
                this.dist = 0;
                var path = this.poly.getPath();
                var that = this;
                path.forEach(function (elm, ind) {
                    if (ind > 0) {
                        that.dist += that.calculateDistance(path.getAt(ind - 1), path.getAt(ind));
                    }
                });
                var km = Math.round(this.dist * 100) / 100;
                $('#distKm').html(km + " km");
                $('#distMiles').html(Math.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KM, km, Aqufit.Units.UNIT_MILES) * 100) / 100 + " Mi");
            },
            handleClick: function (latLng) {
                var that = this;
                if (this.clickLatLng.length > 0) {
                    var lastPoint = this.clickLatLng[this.clickLatLng.length - 1].LatLng;
                } else {
                    var lastPoint = null;
                    this.startMarker = new google.maps.Marker({
                        position: latLng,
                        map: that.map,
                        title: 'start'
                    });
                }
                var lastPoint = this.clickLatLng.length > 0 ? this.clickLatLng[this.clickLatLng.length - 1].LatLng : null;
                var path = that.poly.getPath();
                that.clickLatLng.push({ LatLng: latLng, PathLen: path.length - 1 });
                if (that.directionsEnabled && lastPoint != null) {
                    var request = {
                        origin: lastPoint,
                        destination: latLng,
                        travelMode: google.maps.DirectionsTravelMode.WALKING
                    };
                    that.directionsService.route(request, function (response, status) {
                        if (status == google.maps.DirectionsStatus.OK) {
                            for (var i = 0; i < response.routes.length; i++) {
                                var leg = response.routes[i];
                                for (var j = 0; j < leg.overview_path.length; j++) {
                                    path.push(leg.overview_path[j]);
                                }
                            }
                            that.calcDistance();
                            if (that.endMarker) {
                                that.endMarker.setMap(null);    // clear end marker.
                            }
                            if (path.length > 0) {
                                that.endMarker = new google.maps.Marker({
                                    position: path.getAt(path.length - 1),
                                    map: that.map,
                                    title: 'end'
                                });
                            }
                        }
                    });

                } else {
                    // Because path is an MVCArray, we can simply append a new coordinate and it will automatically appear
                    path.push(latLng);
                    that.calcDistance();
                    if (this.endMarker) {
                        this.endMarker.setMap(null);    // clear end marker.
                    }
                    if (path.length > 0) {
                        this.endMarker = new google.maps.Marker({
                            position: path.getAt(path.length - 1),
                            map: this.map,
                            title: 'end'
                        });
                    }
                }
            },
            gotoAddress: function (address) {
                var that = this;
                if (this.geocoder) {
                    this.geocoder.geocode({ 'address': address }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            that.map.setCenter(results[0].geometry.location);
                        } else {
                            //alert("Geocode was not successful for the following reason: " + status);
                        }
                    });
                }
            },
            setup: function (json) {
                $('#atiMapLoad').hide();
                this.json = eval('(' + json + ')');
                this.placeLocation = new google.maps.LatLng(this.json["Lat"], this.json["Lng"]);
                this.zoom = parseInt(this.json["Zoom"]);
                this.init();
            },
            init: function () {
                var that = this;
                // basic map setup
                var myOptions = {
                    zoom: this.zoom,
                    center: this.placeLocation,
                    mapTypeControl: true,
                    mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                    navigationControl: true,
                    navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL, position: google.maps.ControlPosition.TOP_RIGHT },
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                this.map = new google.maps.Map(document.getElementById(this.map_canvas), myOptions);
                this.deletePath();

                /*
                if (navigator.geolocation) { 
                navigator.geolocation.getCurrentPosition(function(loc){
                //var message ="Longitude: " + loc.coords.longitude + "\n";
                //message+="Accuracy: " + loc.coords.accuracy + "\n";
                //message+="Latitude: " + loc.coords.latitude + "\n";
                    
                that.allowGeolocation = true;
                that.usersGeoLocation = new google.maps.LatLng(loc.coords.latitude, loc.coords.longitude);                   
                });
                }
                */

                // Add our custom control to the map
                this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(this.constructToolBar());
                this.map.controls[google.maps.ControlPosition.TOP_LEFT].push(this.constructOdometer());
                this.map.controls[google.maps.ControlPosition.TOP_RIGHT].push(this.constructSave());
                this.addToolbarEvents();

                /////////////////////////////////////////////////////////////////////////////////////////////
                //  Event Hooks
                /////////////////////////////////////////////////////////////////////////////////////////////
                google.maps.event.addListener(this.map, 'click', function (event) {
                    // check what mode we are in
                    that.handleClick(event.latLng);
                });
                this.initalized = true;
            }
        };



        $(function () {
            $('#dialog').dialog({ modal: true, draggable: false, resizable: false, buttons: { "Begin": function () { var $atiAddress = $('#atiAddress'); if ($atiAddress.val() != '') { Aqufit.Page.<%=this.ID %>.gotoAddress($atiAddress.val()); } $(this).dialog("close"); } } });
            $(window).resize(function() {
              $('#<%=map_canvas.ClientID %>').height($(window).height());
            });
        });


    
    </script>
</telerik:RadCodeBlock>
<div style="position: relative;" >
<div id="atiMapLoad" style="position: absolute; top: 0px; z-index: 99; background-color: #f4f4f4; width: 100%; height: 100%; padding-top: 120px; text-align: center; vertical-align: middle;">
<h2>Loading</h2>
<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif") %>" />
</div>    
<div id="map_canvas" runat="server" style="width: 100%; height: 539px;">

</div>
</div>
<asp:HiddenField ID="atiRouteDistance" runat="server" />
<asp:HiddenField ID="atiRouteTitle" runat="server" />
<asp:HiddenField ID="atiRouteWorkoutType" runat="server" />
<asp:HiddenField ID="atiLatLngArray" runat="server" />
<asp:HiddenField ID="atiFitZoomLevel" runat="server" />
<asp:HiddenField ID="atiMapWidth" runat="server" />
<asp:HiddenField ID="atiMapHeight" runat="server" />


<div id="dialog" title="Choose a route name and start location.">
	<dl>
        <dt><label>Name of route</label></dt>
        <dd><input id="atiRouteName" type="text" maxlength="64" class="ui-corner-all ui-widget-content atiTxtBox" /></dd>

        <dt><label>Route Type</label></dt>
        <dd><ati:WorkoutTypes ID="atiWorkoutTypes" runat="server" TypeDisplayMode="DROP_DOWN" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>

        <dd><label>(optional) Start address</label></dd>
        <dt><input id="atiAddress" type="text" class="ui-corner-all ui-widget-content atiTxtBox" /></dt>
    </dl>
    <a href="javascript: alert('TODO:');">Map Tutorial</a>   
    
</div>
