<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WorkoutListScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_RouteListScript" %>

<style type="text/css">    
    div.routeWrapper
    {
    	position: relative;
    }
    ul.routeList > li
    {
    	list-style: none;
    	list-style: none outside none;
    	border-bottom: 1px solid #EEE;
    }
    div.profileView
    {
    	position: absolute;
    }
    
    img.profileShadow
    {
        padding: 4px;
        border: 1px solid #CCCCCC;
        margin: 8px;	
        -moz-box-shadow: 4px 4px 4px #CCC;
        -webkit-box-shadow: 4px 4px 4px #CCC;
        box-shadow: 4px 4px 4px #CCC;
    }
    img.profileShadow:Hover
    {
    	border: 1px solid #F9A01B;
    	-moz-box-shadow: 4px 4px 4px #FFF;
        -webkit-box-shadow: 4px 4px 4px #FFF;
        box-shadow: 4px 4px 4px #FFF;
    }
    
    div.atiRouteDetails
    {
    	min-height: 90px;
    	margin-left: 85px;
    	padding-top: 10px;
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
    div.atiRouteDetails ul li h2
    {
    	margin: 0px;
    	padding: 0px;
    	
    }
    div.atiRouteDetails ul li h2 a
    {
    	font-size: 18px;
    	color: #0095CD;
    }
    div.atiRouteDetails ul li h2 a:hover
    {
    	text-decoration: none;
    }
    
    
</style>

 <!-- TODO: unpack the javascript only one time -->
<script type="text/javascript" >

   

    Aqufit.Page.Controls.atiWorkoutList = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;
        this.start = 0;
        this.take = 30;
        this.dataBinder = null;
        this.isMyWorkouts = false;
        this.addWorkoutToFavCallback = null;
        this.remWorkoutToFavCallback = null;
        this.logWorkoutUrl = Aqufit.Page.UserSettingsId > 0 ? Aqufit.Page.SiteUrl + Aqufit.Page.UserName : Aqufit.Page.SiteUrl + "Login";
    };           

    
    Aqufit.Page.Controls.atiWorkoutList.prototype = {                           
        generateStreamItem: function (sd, prepend) {    
            var cLat = (sd['LatMax'] + sd['LatMin']) / 2.0;
            var cLng = (sd['LngMax'] + sd['LngMin']) / 2.0;
            var zoom = sd['Zoom'] != '' ? sd['Zoom'] : 16;            
            var url = Aqufit.Page.SiteUrl + 'workout/' + sd["Id"];
            var html = '<li id="'+this.id+'atiStreamItem' + sd["Id"] + '">' +
                        '<div class="routeWrapper">' +
                        '<div class="profileView">'+
                        (sd["Standard"] > 0 ?
                        '<img class="profileShadow" src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/xfit.png" /></div>'
                        :
                        '<a href="' + Aqufit.Page.PageBase + (sd["IsGroup"]?'group/':'') + sd["UserName"] + '"><img class="profileShadow" src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + sd["UserSettingsKey"] + '" /></a></div>'
                        )+
                        '<div class="atiRouteDetails">' +
                            '<ul>' +
                                (sd["UserName"] != 'host' ?
                                '<li><h2><a href="'+url+'">' + sd["Name"] + '</a></h2>'+sd["WODTypeName"]+' workout created by <a href="' + Aqufit.Page.PageBase + (sd["IsGroup"]?'group/':'') + sd["UserName"] + '">'+sd["UserName"]+'</a></li>'
                                :
                                '<li><h2><a href="'+url+'">' + sd["Name"] + '</a></h2>'+sd["WODTypeName"]+' <b>STANDARD</b></li>')+
                                '<li><em>Workouts Recorded:</em>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'+sd["NumDone"]+'</li>'+                              
                                (sd["Description"] != null ?
                                '<li>'+sd["Description"]+'</li>'
                                :
                                ''
                                )+
                                '<li><a href="'+url+'">View Workout</a>&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;<a href="'+this.logWorkoutUrl+'?w='+sd["Id"]+'">Log Workout</a>&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;'+
                                (
                                this.isMyRoutes ?
                                (sd["Standard"] == 0 ?'<a id="workoutRem'+sd["Id"]+'" href="javascript: ;">Remove Workout</a></li>':'')
                                :
                                (sd["UserName"] != 'host' 
                                ?
                                '<a id="workoutAdd'+sd["Id"]+'" href="javascript: ;">Add to My Workouts</a></li>' 
                                :
                                '')
                                )
                                 +
                                '<li></li>' + 
                            '</ul>' +
                        '</div>' +                                               
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
            $('#workoutAdd'+sd["Id"]).click(function(event){
                //alert('addWorkoutToFavCallback != null is: ' + (that.addWorkoutToFavCallback != null));
                if( that.addWorkoutToFavCallback != null ){
                    that.addWorkoutToFavCallback(sd["Id"]);
                }
            });   
            $('#workoutRem'+sd["Id"]).click(function(event){
                if( that.remWorkoutToFavCallback != null ){
                    that.remWorkoutToFavCallback(sd["Id"], '#'+that.id+'atiStreamItem' + sd["Id"]);                    
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
            $(this.controlId).empty();
        },     
        generateStreamDom: function (json) {  
            //alert(json);       
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
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiWorkoutList('<%=this.ID %>', '<%=atiWorkoutList.ClientID %>');               
    });  
        
    
</script>
<asp:HiddenField ID="atiStateSkip" runat="server" EnableViewState="true" Value="0" />
<div id="atiStreamTarget" runat="server">
    <div id="atiStreamLoading" style="text-align: center;">
        <img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading_blue_circle.gif") %>" />
    </div> 
    <asp:Panel ID="atiWorkoutList" runat="server"> 
             
    </asp:Panel>
</div>
