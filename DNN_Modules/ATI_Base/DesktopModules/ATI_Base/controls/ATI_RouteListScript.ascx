<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_RouteListScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_RouteListScript" %>

<style type="text/css">    
    div.routeWrapper
    {
    	position: relative;
    }
    div.routeWrapper:hover
    {
    	background-color: #eee;
    }
    ul.routeList > li
    {
    	list-style: none;
    	list-style: none outside none;
    	border-bottom: 1px solid #EEE;
    }
    
    a.routeLink > img
    {
        padding: 4px;
        border: 1px solid #CCCCCC;
        margin: 8px;	
        -moz-box-shadow: 4px 4px 4px #CCC;
        -webkit-box-shadow: 4px 4px 4px #CCC;
        box-shadow: 4px 4px 4px #CCC;
    }
    a.routeLink > img:Hover
    {
    	border: 1px solid #F9A01B;
    	-moz-box-shadow: 4px 4px 4px #FFF;
        -webkit-box-shadow: 4px 4px 4px #FFF;
        box-shadow: 4px 4px 4px #FFF;
    }
    
    div.atiRouteDetails
    {
    	position: absolute;
    	top: 25px;
    	left: 240px;
    }
    div.atiRouteDetails ul li
    {
    	list-style: none;
    	list-style: none outside none;
    	margin: 0px;
    	padding: 0px;
    	color: #999;
    	font-size: 14px;
    }
    div.atiRouteDetails ul li a
    {
    	font-size: 14px;
    	color: #F9A01B;
    }
    
    div.atiRouteDetails ul li > a > img
    {
    	border: 1px solid #999;
    	
    }
    div.atiRouteText
    {
    	position:absolute;
    	left: 60px;
    	width: 500px;
    }    
    div.atiRouteDetails ul li h2
    {
    	margin: 0px;
    	padding: 0px;
    	
    }
    div.atiRouteDetails ul li h2 a
    {
    	font-size: 24px;
    	color: #0095CD;
    }
    div.atiRouteDetails ul li h2 a:hover
    {
    	text-decoration: none;
    }
    
    
</style>

 <!-- TODO: unpack the javascript only one time -->
<script type="text/javascript" >

   

    Aqufit.Page.Controls.atiRouteList = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;
        this.start = 0;
        this.take = 10;
        this.dataBinder = null;
        this.isMyRoutes = false;
        this.addRouteToFavCallback = null;
        this.remRouteToFavCallback = null;
        this.logWorkoutUrl = Aqufit.Page.UserSettingsId > 0 ? Aqufit.Page.SiteUrl + Aqufit.Page.UserName : Aqufit.Page.SiteUrl + "Login";
    };           

    
    Aqufit.Page.Controls.atiRouteList.prototype = {                           
        generateStreamItem: function (sd, prepend) {    
            var cLat = (sd['LatMax'] + sd['LatMin']) / 2.0;
            var cLng = (sd['LngMax'] + sd['LngMin']) / 2.0;
            var zoom = sd['Zoom'] != '' ? sd['Zoom'] : 16;
            var imgUrl = "http://maps.google.com/maps/api/staticmap?sensor=false&format=jpeg&size=200x160&center=" + cLat + "," + cLng + "&zoom=" + zoom + "&path=weight:3|color:red|enc:" + sd['PolyLine'];
            var url = Aqufit.Page.SiteUrl + 'route/' + sd["Id"];
            var html = '<li id="'+this.id+'atiStreamItem' + sd["Id"] + '">' +
                        '<div class="routeWrapper">' +
                        '<div class="atiRouteDetails">' +
                            '<ul>' +
                                '<li><div class="atiRouteText"><h2><a href="'+url+'">' + sd["Name"] + '</a></h2>'+Aqufit.WorkoutTypes.toString( sd["WorkoutTypeKey"] )+' route posted by <a href="' + Aqufit.Page.PageBase + sd["UserName"] + '">'+sd["UserName"]+'</a> </div><a href="' + Aqufit.Page.PageBase + sd["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + sd["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /></a></li>'+
                                '<li><em>Distance:</em>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_M, sd["Distance"], Aqufit.Page.DistanceUnits), 2)  + '&nbsp;' + Aqufit.Units.getUnitName(Aqufit.Page.DistanceUnits)  + '</li>'+                              
                                '<li><a href="'+url+'">View Route</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;<a href="'+this.logWorkoutUrl+'?r='+sd["Id"]+'">Log Workout</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;'+
                                (
                                this.isMyRoutes ?
                                '<a id="routeRem'+sd["Id"]+'" href="javascript: ;">Remove Route</a></li>'
                                :
                                '<a id="routeAdd'+sd["Id"]+'" href="javascript: ;">Add to My Routes</a></li>' 
                                )
                                 +
                                '<li></li>' + 
                            '</ul>' +
                        '</div>' +
                        '<a class="routeLink" href="'+url+'"><img src="'+imgUrl+'" /></a>'+                        
                        '</div>'+
                        '</li>';
            if (prepend) {
                this.list.prepend(html);
                $('#'+this.id+'atiStreamItem' + sd["Id"]).hide();
                $('#'+this.id+'atiStreamItem' + sd["Id"]).show("slow");
            } else {
                this.list.append(html);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////            
            var cid = this.id;  // (this.id) has new context iside the closures so save it under a new name
            var that = this;
            $('#routeAdd'+sd["Id"]).click(function(event){
                if( that.addRouteToFavCallback != null ){
                    that.addRouteToFavCallback(sd["Id"]);
                }
            });   
            $('#routeRem'+sd["Id"]).click(function(event){
                if( that.remRouteToFavCallback != null ){
                    that.remRouteToFavCallback(sd["Id"], '#'+that.id+'atiStreamItem' + sd["Id"]);                    
                }
            });                       
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        },   
        showLoading: function(){
            this.clear();
            $('#<%=atiStreamTarget.ClientID %> div#atiStreamLoading').show();
        },
        hideLoading: function(){
            $('#<%=atiStreamTarget.ClientID %> div#atiStreamLoading').hide();
        },
        clear: function(){
            $(this.controlId).children().remove();
        },     
        generateStreamDom: function (json) {         
            var that = this;
            this.json = eval("(" + json + ")");
            this.clear();     
            this.hideLoading();
            if( this.start < 0 ){
                this.start = 0;
            }
            if( this.start == 0 ){
                this.take = this.json.length;
            }  
            var atEnd = false;
            if( this.json.length < this.take ){
                atEnd = true;
            }               
            $(this.controlId).append('<div class="routeListHead">'+                                                                                       
                                        '</div>'+
                                        '<ul id="'+this.id+'atiStreamList" class="routeList"></ul>'+
                                      '<div class="messageListHead  grad-FFF-EEE" style="position: relative; height: 40px;">'+                                                
                                            '<div style="position: absolute; right: 10;"><button id="bStreamBack2">Back</button> '+this.start+' to '+(this.start + this.json.length)+' <button id="bStreamNext2">Next</button></div>'+                                                                                                    
                                        '</div>' );
            $('#bStreamBack, #bStreamBack2').button({
                    icons: {
                        primary: 'ui-icon-seek-prev'
                    }
                }).click(function(event){
                    that.start = that.start-that.take;     
                    $('#<%=atiStateSkip.ClientID %>').val( that.start );                    
                    if( that.dataBinder != null ){
                        that.dataBinder(that.start, that.take);    
                    } 
                    event.stopPropagation();
                    return false;
                });
            if( this.start <= 0 ){
                $('#bStreamBack, #bStreamBack2').button('disable');
            }
                
            $('#bStreamNext, #bStreamNext2').button({
                icons: {
                    primary: 'ui-icon-seek-next'
                }
            }).click(function(event){
                that.start = that.start+that.take;  
                $('#<%=atiStateSkip.ClientID %>').val( that.start );             
                if( that.dataBinder != null ){
                    that.dataBinder(that.start, that.take);    
                }          
                event.stopPropagation();
                return false;
            });
            if(atEnd){
                $('#bStreamNext, #bStreamNext2').button('disable');
            }                                                      
            this.list = $('#'+this.id+'atiStreamList');            
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false);
            }               
        }          
    };    
   
    
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiRouteList('<%=this.ID %>', '<%=atiRouteList.ClientID %>');               
    });  
        
    
</script>
<asp:HiddenField ID="atiStateSkip" runat="server" EnableViewState="true" Value="0" />
<div id="atiStreamTarget" runat="server">
    <div id="atiStreamLoading" style="text-align: center;">
        <img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading_blue_circle.gif") %>" />
    </div> 
    <asp:Panel ID="atiRouteList" runat="server"> 
             
    </asp:Panel>
</div>
