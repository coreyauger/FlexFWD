<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_PeopleListScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_PeopleListScript" %>

<style type="text/css">   
    div.hScrollWrapper
    {
    	position: relative; 
   	    z-index: 9999;  	
    }
     
   div.hScrollWrapper div.hWrapper
   {
   	    position: relative; 
   	    z-index: 9999;  	
   	    overflow: hidden;
   	    width: 100%;
   	    height: 125px;   	        
   }
   
   div.hScrollWrapper div.hScrollInner
   {
   	    position: absolute;
   	    width: 99999px;
   	    display: inline;
   }
   
   div.personCard
   {
        float: left;
   	    width: 140px;
   	    height: 120px;
   	    text-align: center;
   	    border: 1px solid #CCC;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        margin-top: 2px;               
   }
   
   div.personCard img
   {
   	    border: 6px solid #fff;
        -moz-border-radius: 4px;
        -webkit-border-radius: 4px;
        margin-top: 10px;
        display: block;   
   }   
    div.personCard img:hover
    {
    	border: 6px solid #ccc;
    }
   
   div.personCard a.username	
    {
	    color:#f9a01b;
	    display: block;
    }
    
    div.personCard span
    {
    	font-size: 10px;
    	color: #666;
    }

   div.hScrollWrapper div.disabled
   {
        background-color: #ccc;
   }
   
   div.slideActionLeft
   {
   	margin-right: 1px;
   	position: absolute;
   	width: 25px;
   	height: 100%;
   	left: 0px;
   	z-index: 99999; 
   	background-color: #FFF;
   }
   div.slideActionRight
   {
   	position: absolute;
   	width: 25px;
   	height: 100%;
   	right: 0px;
   	z-index: 99999; 
   	background-color: #FFF;
   }
   
</style>

 <!-- TODO: unpack the javascript only one time -->
<script type="text/javascript" >

   


    Aqufit.Page.Controls.ATI_PeopleListScript = function (id, control, callback) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;
        this.json = null;
        $(this.controlId).append('<div class="hScrollWrapper"><div class="slideActionLeft disabled" id="'+this.id+'slideActionLeft"><a href="javascript: ;">&lt;</a></div><div class="slideActionRight" id="'+this.id+'slideActionRight"><a href="javascript: ;">&gt;</a></div><div id="'+this.id+'hWrapper" class="hWrapper"><div class="hScrollInner"><ul id="'+this.id+'atiPeopleStreamList" class="hlist"></ul></div></div></div>');
        this.list = $('#'+this.id+'atiPeopleStreamList');
        this.$hWrapper = $('#'+this.id+'hWrapper');
        this.viewWidth = 0;
        this.displayItems = 0;
        this.currentPage = 0;
        this.init();
        this.isSetup = false;
        this.itemLoadCallback = callback;
        this.atEnd = false;
        this.pagesLoaded = 0;
        this.pagerWidth = 0;
    };           

    
    Aqufit.Page.Controls.ATI_PeopleListScript.prototype = {                                
        init: function(){
            var that = this;
            this.pagerWidth = $('div.slideActionRight').width();
            
            this.viewWidth = this.$hWrapper.width() - (this.pagerWidth*2);
            this.$hWrapper.css('margin-left',this.pagerWidth+'px');
            this.$hWrapper.width((this.$hWrapper.width() - (this.pagerWidth*2)));
            $('div#'+this.id+'slideActionLeft a').click(function(event){
                that.atEnd = false;
                that.currentPage--;
                that.slide(-1);
                if( that.currentPage <= 0 ){   
                    that.currentPage = 0;
                    $('div#'+that.id+'slideActionLeft').addClass('disabled');
                    // TODO: disable the scroll left.
                }                                           
            });
            $('div#'+this.id+'slideActionRight a').click(function(event){
                that.currentPage++;
                $('div#'+that.id+'slideActionLeft').removeClass('disabled');                
                that.slide(1);   
                if( that.atEnd ){
                    $('div#'+that.id+'slideActionRight').addClass('disabled');
                }else{
                    if( that.itemLoadCallback && that.pagesLoaded < that.currentPage ){
                        // when we scroll right ... we ajax load another set of items..                       
                        that.itemLoadCallback(that, that.currentPage+1, that.displayItems);
                        that.pagesLoaded = that.currentPage;
                    }   
                }
            });
        },
        slide: function(dir){         
            //var left = (this.$hWrapper.width() * dir) - 16; // - (this.displayItems*3); // (this.displayItems*2) accounts for borders..
           
       //     var left = (dir * (this.itemWidth) * (this.displayItems+2));//($('div.slideActionRight').width()*2)+2;
       var left = dir * (this.itemWidth+2) * (this.displayItems);
       //var left = dir*142;
            var that = this;
            this.$hWrapper.animate({
                scrollLeft : '+=' + left
            }, 750, function () {
                // after animation
            });
        },
        generateStreamItem: function (sd, prepend) {    
            var html = '<li>' +
                        '<div class="personCard touchNew grad-FFF-EEE">'+
                            '<center><a href="javascript: ;" onclick="top.location.href=\'' + Aqufit.Page.PageBase + sd["UserName"] + '\'"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + sd["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /></a></center>'+
                            '<a href="javascript: ;" onclick="top.location.href=\'' + Aqufit.Page.PageBase + sd["UserName"] + '\'" class="username">'+sd["UserName"]+'</a>'+
                            '<span>'+sd["FName"]+' ' +sd["LName"]+'</span>'+
                            (sd["Duration"] > 0 ?
                            '<br /><span>'+Aqufit.Utils.toDurationString(sd["Duration"])+'</span>'
                            :
                            '')+
                        '</div>'+
                        '</li>';
            if (prepend) {
                this.list.prepend(html);
                $('#'+this.id+'atiStreamItem' + sd["Id"]).hide();
                $('#'+this.id+'atiStreamItem' + sd["Id"]).show("slow");
            } else {
                this.list.append(html);
            }            
        },        
        generateStreamDom: function (json) {            
            this.json = eval("(" + json + ")");            
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false);
            }
            if( this.json.length < this.displayItems ){
                this.atEnd = true;                
            }
            if( !this.isSetup ){
                this.displayItems = this.json.length/2;
                this.isSetup = true;
            }
            var sizeNeeded = (this.viewWidth / this.displayItems);
            this.itemWidth = this.$hWrapper.find('li div.touchNew:first-child').width();
            var padding = (sizeNeeded - this.$hWrapper.find('li div.touchNew:first-child').width())/2;
            padding = padding - 2;
            this.$hWrapper.find('div.touchNew').css('padding','0px '+padding+'px');
            $('div.touchNew').removeClass('touchNew');             
        }          
    };    
   
    
    $(function () {
        //Affine.WebService.StreamService.getFriendListData(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.Controls.atiFriendList.generateStreamDom, WebServiceFailedCallback);
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_PeopleListScript('<%=this.ID %>', '<%=atiPeopleList.ClientID %>', <%=this.Client_OnItemsLoad %>);               
    });  
        
    
</script>
<asp:Panel ID="atiPeopleList" runat="server">             
</asp:Panel>
