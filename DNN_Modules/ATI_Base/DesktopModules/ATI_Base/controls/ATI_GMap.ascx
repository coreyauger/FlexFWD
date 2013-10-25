<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_GMap.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_GMap" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style type="text/css">

a.atiGroupNameLink
{
	color: #f58220;
	font-size: 16px;
	font-weight: bold;
	text-decoration: underline;
}
a.atiGroupNameLink:hover
{
	text-decoration: none;
}
</style>
 <telerik:RadCodeBlock ID="codeBlock1" runat="server">    
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>
        
    <script type="text/javascript">

        ////////////////////////////////////////////////////////////////////////////////////////
        // Circle overlay class
        ////////////////////////////////////////////////////////////////////////////////////////
        // This file adds a new circle overlay to Google Maps v3
// Original Google Maps API v2 File : http://dawsdesign.com/drupal/google_maps_circle_overlay
// Ported to GMaps v3 by http://florent.clairambault.fr/
 
/**
 * Filled circle overlay
 */
        var MapCircleOverlay = function (center, radius, strokeWeight, strokeColor, strokeOpacity, fillColor, fillOpacity) {
            this.center = center;
            this.radius = radius;
            this.strokeWeight = strokeWeight;
            this.strokeColor = strokeColor;
            this.strokeOpacity = strokeOpacity;
            this.fillColor = fillColor;
            this.fillOpacity = fillOpacity;

            this.circlePolygon = null;

            // 50 lines look like a pretty good circle
            this.numPoints = 50;

            this.d2r = Math.PI / 180;

            this.bound = null;

            this.setCenter = function (latLng) {
                this.center = latLng;
                this.draw();
            };
            this.getCenter = function () {
                return this.center;
            }
            this.setRadius = function (radius) {
                this.radius = radius;
                this.draw();
            };
        };
 
/* base class overloads follow this comment */
MapCircleOverlay.prototype = new google.maps.OverlayView;
 
// Calculate all the points and draw them
// Base method must be implemented like this
MapCircleOverlay.prototype.draw = function() {	
	if ( ! isFinite( this.radius ) || ! isFinite( this.center.lat() ) || ! isFinite( this.center.lng() ) ) {
		if ( console != undefined ) 
			console.error('Radius has to be a number !');
		return;
	}
 
	circleLatLngs = new Array();
 
        // Remove the "* 0.621371192" to use miles instead of kilometers
	var circleLat = this.radius * 0.621371192 * 0.014483;  // Convert statute into miles and miles into degrees latitude
	var circleLng = circleLat / Math.cos( this.center.lat() * this.d2r);
 
	// 2PI = 360 degrees, +1 so that the end points meet
	for (var i = 0; i < this.numPoints+1; i++) { 
		var theta = Math.PI * (i / (this.numPoints / 2)); 
		var vertexLat =  this.center.lat() + (circleLat * Math.sin(theta)); 
		var vertexLng =  this.center.lng() + (circleLng * Math.cos(theta));
		var vertextLatLng = new google.maps.LatLng(vertexLat, vertexLng);
		circleLatLngs.push( vertextLatLng );
	}
 
	// Before drawing the new polygon, we have to remove the old one
	this.clear();
 
	this.circlePolygon = new google.maps.Polygon({
	  paths: circleLatLngs,
	  strokeColor: this.strokeColor,
	  strokeOpacity: this.strokeOpacity,
	  strokeWeight: this.strokeWeight,
	  fillColor: this.fillColor,
	  fillOpacity: this.fillOpacity
	});
 
	this.circlePolygon.setMap( this.map );
};

MapCircleOverlay.prototype.clear = function () {
    if (this.circlePolygon != null) {
        this.circlePolygon.setMap(null);
        this.circlePolygon = null;
    }
}
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////






Aqufit.Page.Controls.ATI_GMap = function (id, map_canvas) {
    this.id = id;
    this.map = null;
    this.map_canvas = map_canvas;
    this.pager = null;
    this.pinfo = null;
    this.mode = null;
    this.placeLocation = null;
    this.menuBar = null;
    this.json = null;
    this.allowGeolocation = false;
    this.usersGeoLocation = null;
    this.directionsService = new google.maps.DirectionsService();
    this.directionsDisplay = null;
    this.isDirectionMode = false;
    this.ctime = null;
    this.timeout = 1000;
    this.locMarker = null;
    this.markers = [];
    this.locationChangeCallback = null;
    this.circleRadius = 5.0;
    this.circle = null;
    this.routePath = null;
    this.onPlacesResponse = null;
    this.onMarkerClick = null;
}

Aqufit.Page.Controls.ATI_GMap.prototype = {
    constructTopBar: function () {
        this.topBar = document.createElement('DIV');
        $(this.topBar).addClass('trans80');
        $(this.topBar).css({ 'background-color': 'white', 'width': '80%', 'height': '60px', 'positon': 'relative', 'top': '100px' });
        $(this.topBar).append('<h3 style="text-align: center; padding-top: 20px;">Loading Locations..</h3>');
        this.topBar.index = 5;
        return this.topBar;
    },
    updateUsersLocation: function () {
        // TODO: create an interval to keep getting the users location
    },
    clearMarkers: function () {
        for (var i = 0; i < this.markers.length; i++) {
            this.markers[i].setMap(null);
        }
        this.markers = new Array();
    },
    setup: function (json, mode) {
        this.mode = mode;
        this.json = eval('(' + json + ')');
        this.placeLocation = new google.maps.LatLng(this.json["Lat"], this.json["Lng"]);
        this.zoom = parseInt(this.json["Zoom"]);
        this.init();
        if (this.mode == 'group') {
            this.groupInit();
        } else if (this.mode == 'route') {
            this.routeInit();
        } else if (this.mode == 'routeview') {

            this.routeView();
        }
    },
    routeView: function () {
        if (this.json["RouteData"] != '') {
            var rd = this.json["RouteData"];
            var bb = new google.maps.LatLngBounds(new google.maps.LatLng(rd["LatMin"], rd["LngMin"]), new google.maps.LatLng(rd["LatMax"], rd["LngMax"]));
            this.map.fitBounds(bb);
            var path = [];
            for (var i = 0; i < rd["RoutePoints"].length; i++) {
                path.push(new google.maps.LatLng(rd["RoutePoints"][i].Lat, rd["RoutePoints"][i].Lng));
            }
            this.routePath = new google.maps.Polyline({
                path: path,
                strokeColor: "#FF0000",
                strokeOpacity: 1.0,
                strokeWeight: 2
            });
            this.routePath.setMap(this.map);
        }
    },
    routeInit: function () {
        var that = this;
        this.locMarker = new google.maps.Marker({
            map: this.map,
            position: this.placeLocation,
            draggable: true
        });
        if (this.circle) {
            this.circle.setMap(null);
            this.circle = null;
        }
        // We load the circle
        this.circle = new MapCircleOverlay(this.placeLocation, this.circleRadius, 2, "#0000FF", 0.5, "#0000FF", 0.1);
        // And we attach it to the map
        this.circle.setMap(this.map);

        google.maps.event.addListener(this.locMarker, "dragend", function () {
            var pos = this.getPosition();
            if (that.locationChangeCallback != null) {
                that.locationChangeCallback(pos.lat(), pos.lng());
                if (that.circle != null) {
                    that.circle.setCenter(pos);
                }
            }
        });
    },
    centerRouteMarker: function () {
        var pos = this.map.getCenter();
        this.locMarker.setPosition(pos);
        if (this.locationChangeCallback != null) {
            this.locationChangeCallback(pos.lat(), pos.lng());
            if (this.circle != null) {
                this.circle.setCenter(pos);
            }
        }
    },
    adjustSearchRadius: function (rad) {
        this.circleRadius = rad;
        var pos = this.map.getCenter();
        if (!this.circle) {
            this.circle = new MapCircleOverlay(pos, this.circleRadius, 2, "#0000FF", 0.5, "#0000FF", 0.1);
            // And we attach it to the map
            this.circle.setMap(this.map);
        }
        this.circle.setRadius(this.circleRadius);
        if (this.locationChangeCallback != null) {
            var c = this.circle.getCenter();
            this.locationChangeCallback(c.lat(), c.lng());
        }
    },
    setPager: function (atiPager) {
        this.pager = atiPager;
        var that = this;
        this.pager.onPageBack = function (skip, take, len) {
            that.getPlaces(skip, take);
        }
        this.pager.onPageForward = function (skip, take, len) {
            that.getPlaces(skip, take);
        }
        if (this.pinfo) {
            this.pager.setPagerInfo(this.pinfo);
        }
    },
    getPlaces: function (skip, take) {
        if (skip == null) {
            skip = 0;
        }
        if (take == null) {
            take = 25;
        }
        $(this.topBar).hide();
        var that = this;
        this.clearMarkers();
        var bbox = this.map.getBounds();
        Affine.WebService.StreamService.getGroupPlaces(bbox.getNorthEast().lat(), bbox.getNorthEast().lng(), bbox.getSouthWest().lat(), bbox.getSouthWest().lng(), skip, take, function (json) {
            var results = eval('(' + json + ')');
            that.pinfo = results.PagerInfo;
            if (that.pager != null) {
                that.pager.setPagerInfo(that.pinfo);
            }
            that.json = results.Data;
            for (var i = 0; i < that.json.length; i++) {
                var place = that.json[i];
                var m = new google.maps.Marker({
                    position: new google.maps.LatLng(place["Lat"], place["Lng"]),
                    map: that.map,
                    title: place["Name"]
                });
                that.json[i].marker = m;
                (function () {  // create a closure..
                    var mark = m;
                    var p2 = place;
                    google.maps.event.addListener(mark, 'click', function () {
                        if (that.onMarkerClick) {
                            that.onMarkerClick(p2);
                        } else {
                            var infowindow = new google.maps.InfoWindow({
                                content: '<div style="width: 250px; height: 100px;"><img style="float: left; width: 50px;" src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + p2["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /><div style="float: right; width: 190px;"><a class="atiGroupNameLink" href="<%=ResolveUrl("~")%>group/' + p2["UserName"] + '">' + p2["Name"] + '</a></div><br style="clear: both;" /></div>'
                            });
                            infowindow.open(that.map, mark);
                        }
                    });
                })();

                that.markers.push(m);
            }
            if (that.onPlacesResponse != null) {
                that.onPlacesResponse(results);
            }           
        }, function (err) {
            //alert('error: ' + err.Message);
        });
    },
    refresh: function(){
        this.setZoom(10);
        google.maps.event.trigger(this.map, 'resize');
        this.center(this.placeLocation);
    },
    groupInit: function () {
        var that = this;               
        //this.map = new google.maps.Map(document.getElementById(this.map_canvas), myOptions);
        this.map.controls[google.maps.ControlPosition.BOTTOM_LEFT].push(this.constructTopBar());
        google.maps.event.addListener(this.map, 'zoom_changed', function () {
            $(that.topBar).show();
            clearTimeout(that.ctime);
            that.ctime = setTimeout(function () {
                $(that.topBar).show();
                that.getPlaces();
            }, that.timeout);
        });
        google.maps.event.addListener(this.map, "dragstart", function () {
            clearTimeout(that.ctime);
        });
        google.maps.event.addListener(this.map, "dragend", function () {
            $(that.topBar).show();
            clearTimeout(that.ctime);
            that.ctime = setTimeout(function () {
                that.getPlaces();
            }, that.timeout);
        });
        that.ctime = setTimeout(function () {
            that.getPlaces();
        }, that.timeout);
    },
    init: function () {
        var that = this;
        // basic map setup
        var myOptions = {
            zoom: this.zoom,
            center: this.placeLocation,
            navigationControl: true,
            mapTypeControl: true,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        this.map = new google.maps.Map(document.getElementById(this.map_canvas), myOptions);
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

        $('#atiMapLoad').hide();

    },
    center: function (ll) {
        if (this.circle != null) {
            this.circle.setCenter(ll);
        }
        if (this.locMarker != null) {
            this.locMarker.setPosition(ll);
        }
        this.map.setCenter(ll);
        if (this.locationChangeCallback) {
            this.locationChangeCallback(ll.lat(), ll.lng());
        }
    },
    setCenter: function (lat, lng) {
        this.center(new google.maps.LatLng(lat, lng));
    },
    setZoom: function (z) {
        this.map.setZoom(z);
    },
    getMap: function () {
        return this.map;
    },
    gotoAddress: function (address, success, fail) {
        var that = this;
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                that.center(results[0].geometry.location);
                that.map.setZoom(11);
                if (success) success();
            } else {
                if (fail) fail();
            }
        });
    }
};      
    
    </script>
</telerik:RadCodeBlock>
<div style="position: relative;" id="mapWrap" runat="server" >
<div id="atiMapLoad" style="position: absolute; top: 0px; z-index: 99; background-color: #f4f4f4; width: 100%; height: 100%; padding-top: 120px; text-align: center; vertical-align: middle;">
<h2>Loading</h2>
<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading.gif") %>" />
</div>    
<div id="map_canvas" runat="server" style="width: 100%; height: 100%;" ></div>
</div>

