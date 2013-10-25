<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_RecipeScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_RecipeScript" %>

<style type="text/css">
    ul.prepTimes
    {
    	padding-top: 9px;
    	padding-bottom: 9px;
    }
    
    ul.prepTimes li
    {
    	list-style: none;
    	list-style: none outside none;
	    display: inline;
	    font-size: 14px;
	    color: #666666;
	    padding-right: 18px;
    }
    
#atiRecipeView ul.ingredients
{
	position: relative;
	right: 0px;	
	list-style: none;
	list-style: none outside none;
	margin-bottom: 18px;	
	width: 425px;
	float: right;
}
#atiRecipeView ul.hList
{
	list-style: none;
	list-style: none outside none;
	display: inline;
}
#atiRecipeView ul.hList li
{
	list-style: none;
	list-style: none outside none;
	display: inline;
}
li#atiRecipeStrict,
li#atiRecipeRating
{
	font-size: 14px;
	color: #324154;
	padding-left: 9px;
	font-weight: bold;
}

#atiPreview
{
	position: absolute;
	z-index: 99999;
	padding: 9px;
	background-color: White;
}

#atiRecipeView h2
{
	color: #324154;
}
#atiRecipeView ul.ingredients li
{
	padding: 9px;
	border-bottom: 1px solid #CCCCCC;
	list-style: none;
	list-style: none outside none;
}
#atiRecipeView span.directions
{
	padding-right: 18px;
	margin-bottom: 18px;
	display: block;
}


a.time
    {
    	color:#A1A1A1;
        font-size:10px;
        white-space:nowrap;
        text-decoration: none;     
        line-height:22px;        
    }

div.commentBoxLeft ul li.deleteComment
    {
    	float: right;
    }  
   ul.atiCommentBox li 
   {
   	    list-style: none;
   	    list-style: none outside none;
   }             
   ul.atiCommentBox > li {
        border-top: 1px solid white;        
    	background-color: #f4f4f4;    
    	padding: 18px;
    	clear: both;
    	border-bottom: 1px dashed #CCCCCC;
    }
    
    div.commentBoxLeft
    {
    	float: left;	
    	width: 50px;
    	height: 100%;
    	background-color: transparent;
    }
    div.commentBoxLeft img
    {
    	border: 1px solid white;
    }
    div.commentBoxRight a.username
    {
    	color:#f48401;
    	font-weight: bold;
    }
    
    div.commentBoxRight
    {
    	float: right;	 
    	height: 100%;    
    	width: 790px;	
    	min-height: 50px;   
    }
    div.commentBoxRight span
    {
    	 	
    }        
    div.commentBoxRight img.speak
    {
    	position: absolute;
    	top: 0;
    	left: -6px;
    }    
    div.commentBoxRight textarea
    {   
    	width: 380px;
    	height: 25px;   	
    	padding: 3px 7px 3px 7px;    	
    	border: 1px solid #cccccc;
    	font-size:12px;
    }    
    div.commentBoxRight textarea.txtCommentFocus
    {   
    	height: 50px;  	
    }
    
    div#atiUserBCard
    {
    	background-color: #eeeeee;
    	padding: 9px;
    	margin-right: 9px;
    	margin-top: 9px;
    	height: 50px;		
		margin-top: 9px;
    }       

</style>

<script type="text/javascript">   

    Aqufit.Page.Controls.atiRecipeScript = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#' + control;
        this.json = null;
        this.re = null;
    };        

    Aqufit.Page.Controls.atiRecipeScript.prototype = {        
        displayRecipe: function( json ){
            this.json = eval('('+json+')');
            this.re = this.json["RecipeExtended"];
            var width = '450';
            if (navigator.appName.toLowerCase().indexOf("internet explorer") != -1) {
                width = '425';
            }
            $('#atiRecipeImg').prepend('<img width="'+width+'" src="'+ Aqufit.Page.SiteUrl + 'DesktopModules/ATI_Base/services/images/recipe.aspx?r='+this.json["Id"]+'&f=1" align="top" />'+
                                        ((Aqufit.Page.UserId == this.json["UserKey"] )?
                                        '<button id="atiDelete" style="float: right; margin-top: 4px;">delete</button>'+
                                        '<button id="atiEdit" style="float: right; margin-top: 4px;">edit</button>'
                                        :
                                        '')
                                        );
            var that = this;                                            
             if (navigator.appName.toLowerCase().indexOf("internet explorer") != -1) {     
                // TODO: this is a tmp fix.. IE does not show the buttons if i use the jquery-ui                                   
                   $('#atiEdit').click(function(event){
                                self.location.href = Aqufit.Page.PageBase + "AddRecipe.aspx?s="+that.json["Id"];               
                                event.stopPropagation();
                                return false;
                            });
                            $('#atiDelete').click(function(event){
                                if( confirm("Are you sure you want to delete this recipe?") ){
                                    Affine.WebService.StreamService.deleteStream( Aqufit.Page.UserId, Aqufit.Page.PortalId, 348732201, that.json["Id"], function(){
                                        self.location.href = Aqufit.Page.PageBase + Aqufit.Page.UserName;     
                                    }, function(){
                                        alert('There was an error.  Please "report bug".');
                                    });
                                }            
                                event.stopPropagation();
                                return false;
                            });                     
                }else{
                   $('#atiEdit').button({ icons:{ primary: 'ui-icon-pencil' }}).click(function(event){
                                self.location.href = Aqufit.Page.PageBase + "AddRecipe.aspx?s="+that.json["Id"];               
                                event.stopPropagation();
                                return false;
                            });
                            $('#atiDelete').button({ icons:{ primary: 'ui-icon-trash' }}).click(function(event){
                                if( confirm("Are you sure you want to delete this recipe?") ){
                                    Affine.WebService.StreamService.deleteStream( Aqufit.Page.UserId, Aqufit.Page.PortalId, 348732201, that.json["Id"], function(){
                                        self.location.href = Aqufit.Page.PageBase + Aqufit.Page.UserName;     
                                    }, function(){
                                        alert('There was an error.  Please "report bug".');
                                    });
                                }            
                                event.stopPropagation();
                                return false;
                            });
                }
            
            var ingrList = "<li><em>Ingredients:</em></li>";
            
            for( var i=0; i < this.re["Ingredients"].length; i++ ){
                ingrList += '<li>' + this.re["Ingredients"][i]["Name"] + '</li>';
            }   
              
            var directions = "<em>Directions:</em><br />"+this.re["Directions"].replace(/\n/g, '<br />');
            $('#atiRecipeView h2').text(this.json["Title"]).next('span').text(this.json["Description"]).next('ul').append('<li>Prep Time: <em>'+this.json["TimePrep"]+' min</em></li><li>Cook Time: <em>'+this.json["TimeCook"]+' min</em></li><li>Servings: <em>'+this.json["NumServings"]+'</em></li>').next('ul').append(ingrList);
            var $supportImg = $('div.supportImages');
            if( this.re["Image2Id"] > 0 ){
               $supportImg.append('<img class="preview" width="120" height="120" src="'+ Aqufit.Page.SiteUrl + 'DesktopModules/ATI_Base/services/images/image.aspx?i='+this.re["Image2Id"] +'" />');
            }
            if( this.re["Image3Id"] > 0 ){
                $supportImg.append('<img class="preview" width="120" height="120" src="'+ Aqufit.Page.SiteUrl + 'DesktopModules/ATI_Base/services/images/image.aspx?i='+this.re["Image3Id"] +'" />');
            }
            if( this.re["Image4Id"] > 0 ){
                $supportImg.append('<img class="preview" width="120" height="120" src="'+ Aqufit.Page.SiteUrl + 'DesktopModules/ATI_Base/services/images/image.aspx?i='+this.re["Image4Id"] +'" />');
            }
            $supportImg.children('img').css('border','1px solid #f58605').css('margin', '9px');
            $('#atiRecipeView span.directions').html(directions);
            
            $('#atiRecipeRating').text(this.json["AvrRating"]);
            
            
            xOffset = 20;
            yOffset = 30;

            // these 2 variable determine popup's distance from the cursor
            // you might want to adjust to get the right result

            /* END CONFIG */
            $("img.preview").hover(function (e) {
               
               $("body").append("<p id='atiPreview'><img src='" + this.src + "&f=1' alt='Image preview' /></p>");
               $("#atiPreview")
        			.css("top", (e.pageY - xOffset) + "px")
        			.css("left", (e.pageX - 490) + "px")
        			.fadeIn("fast");
            },
        	function () {
        	    $("#atiPreview").remove();
        	});
            $("img.preview").mousemove(function (e) {
                if( e.pageX > ($('body').width() / 2 ) ){                    
                    $("#atiPreview")
            			.css("top", (e.pageY + xOffset) + "px")
            			.css("left", (e.pageX - 490) + "px");
                }else{
                    $("#atiPreview")
            			.css("top", (e.pageY + xOffset) + "px")
            			.css("left", (e.pageX + xOffset) + "px");
                }
            });
            
                        
            var that = this;           
            $('#atiRecipeView input.rate').each(function () {
                    if ($(this).val() == that.json["AvrRating"]) {
                        $(this).attr('checked', 'true'); 
                    }
                }).rating({
                split: 2,
                readOnly: true
            });
            $('#atiRecipeView input.strict').each(function () {
                    if ($(this).val() == that.json["AvrStrictness"]) {
                        $(this).attr('checked', 'true');
                    }
                }).rating({             
                    readOnly: true
                });
            if (navigator.appName.toLowerCase().indexOf("internet explorer") == -1) {
                setTimeout(function(){
                    $('#atiRecipeImg').dropShadow();    
                    }, 1500);
            }
            var numRecipe = 0;
            var followers = 0;
            var following = 0;
            for( var i=0; i<this.json["Metrics"].length; i++){
                if( this.json["Metrics"][i]["MetricType"] == Aqufit.Metric.NUM_RECIPES ){
                    numRecipe = this.json["Metrics"][i]["MetricValue"];
                }else if( this.json["Metrics"][i]["MetricType"] == Aqufit.Metric.NUM_FOLLOWERS ){
                    followers = this.json["Metrics"][i]["MetricValue"];
                }else if( this.json["Metrics"][i]["MetricType"] == Aqufit.Metric.NUM_YOU_FOLLOW ){
                    following = this.json["Metrics"][i]["MetricValue"];
                }
            }    
            $('#atiUserBCard').append('<div style="float:left; width: 50px;"><a href="' + Aqufit.Page.PageBase + this.json["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + this.json["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /></a></div>'+
                            '<div style="float:right; margin-right: 10px; width: 340px;">'+
                                '<a style="font-weight: bold; font-size: 14px; float: right;" href="' + Aqufit.Page.PageBase + this.json["UserName"] + '">' + this.json["UserName"] + '</a>'+
                                //'<ul class="atiUserStats"><li></li><li>Num&nbsp;Recipies:&nbsp;<em>'+ numRecipe +'</em></li><li>Followers:&nbsp;<em>'+followers+'</em></li><li>Following:&nbsp;<em>'+ following +'</em></li></ul>'+
                                '<span class="atiUserStats">Num&nbsp;Recipes:&nbsp;<em>'+ numRecipe +'</em><br />Followers:&nbsp;<em>'+followers+'</em><br />Following:&nbsp;<em>'+ following +'</em></span>'+
                            '</div>');
            addthis.button('#addThis', {}, {url: self.location.href, title: this.json["Title"]});
            if (this.json["Comments"]) {                   // append all comments to the list
                var $commentList = $('ul.atiCommentBox');
                for (var j = 0; j < this.json["Comments"].length; j++) {
                    (function () {  // create a closure 
                        var comment = this.json["Comments"][j];
                        $commentList.append('<li id="atiComment' + comment["Id"] + '">' +
                                            '<div class="commentBoxLeft"><a href="' + Aqufit.Page.PageBase + comment["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + comment["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /></a></div>' +
                                            '<div class="commentBoxRight">' +
                                                '<a class="username" href="' + Aqufit.Page.PageBase + comment["UserName"] + '">' + comment["UserName"] + '</a>' +
                                                '<span style="padding-right: 16px;">&nbsp;-&nbsp;' + comment["Text"] + '<br /></span>' +
                                                '<ul>' +
                                                    '<li><a href="javascript: ;" class="time">' + Aqufit.Utils.toTimeAgoString(new Date(comment["DateTicks"])) + '</a></li>' +
                                                    ((Aqufit.Page.UserId == comment["UserKey"]) ? // if this is the owner 
                                                    '<li class="deleteComment"><a href="javascript: ;" id="bDelComment' + comment["Id"] + '" class="hidden">[X]</a></li>'
                                                    :
                                                    '') +
                                                    '<li>' +
                                                '</ul>' +
                                            '</div><br style="clear: both;" />' +
                                        '</li>');
                        $('#atiComment' + comment["Id"]).hover(function () {
                            $('#bDelComment' + comment["Id"]).removeClass('hidden');
                        }, function () {
                            $('#bDelComment' + comment["Id"]).addClass('hidden');
                        });
                        $('#bDelComment' + comment["Id"]).click(function (event) {
                            if (confirm("Are you sure you want to delete?")) {
                                //$("#atiComment" + comment["Id"]).hide("slow");
                                Affine.WebService.StreamService.deleteComment(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, comment["Id"], 
                                    function (id) {
                                        if (id > 0) {   // success
                                            $("#atiComment" + id).hide("slow");
                                            $("#atiComment" + id).children().remove();
                                        }
                                    }, function(){});
                            }
                            event.stopPropagation();
                            return false;
                        });
                    })();   // end closure ... and call it.
                }
                if( Aqufit.Page.UserId > 0 ){   // only reg users can comment
                    $commentList.append('<li id="atiCommentAdd">' +
                                        '<div class="commentBoxLeft">' +
                                            '<img src="' + Aqufit.Page.PageBase + "DesktopModules/ATI_Base/services/images/profile.aspx?u=" + Aqufit.Page.UserId + '&p=' + Aqufit.Page.PortalId + '" />' +
                                        '</div>' +
                                        '<div class="commentBoxRight">' +
                                            '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/speak.png" class="speak" />' +
                                            '<textarea id="commentTxt" /><br />' +
                                            '<button id="atiButtonAddComm" type="button">done</button>' +
                                        '</div><br style="clear: both;" />' +
                                        '</li>');
                    $('#commentTxt').focus(function () {
                        $(this).addClass('txtCommentFocus');
                    });
                    $('#atiButtonAddComm').button().click(function () {
                        $(this).hide();
                        var $txt = $('#commentTxt');
                        if ($txt.val() != "") {
                            Affine.WebService.StreamService.addComment(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, that.json["Id"], $txt.val(), 
                                        function(comm){
                                            var comment = eval("(" + comm + ")");
                                            if (comment["StreamKey"] > 0) {
                                                var $comList = $('#atiCommentAdd');
                                                $comList.before('<li id="atiComment' + comment["Id"] + '">' +
                                                                            '<div class="commentBoxLeft"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + comment["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /></div>' +
                                                                            '<div class="commentBoxRight">' +
                                                                                '<a class="username" href="' + Aqufit.Page.PageBase + comment["UserName"] + '">' + comment["UserName"] + '</a>' +
                                                                                '<span>&nbsp;-&nbsp;' + comment["Text"] + '<br /></span>' +
                                                                                '<ol><li><a href="javascript: ;" class="time">' + Aqufit.Utils.toTimeAgoString(new Date(comment["DateTicks"])) + '</a></li></ol>' +
                                                                            '</div><br style="clear: both;" />' +
                                                                        '</li>');
                                                $("#atiComment" + comment["Id"]).hide().slideDown("slow");
                                                $('#commentTxt' + sd["Id"]).val('');
                                            } else {
                                                alert('failed to save comment... contact admin');
                                            }
                                            $('#atiButtonAddComm').show();
                                        }, function(){ alert('failed to save comment... contact admin'); } );
                        }
                    });
                }
            } 
      
        }        
    };

    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiRecipeScript('<%=this.ID %>', 'atiRecipeView');  
        Affine.WebService.StreamService.getRecipe(<%=this.RecipeId %>, Aqufit.Page.<%=this.ID %>.displayRecipe, function(){ alert('failed to get recipe'); });
        
    });

</script>
<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js"></script>

<div id="atiRecipeView" style="position: relative;">
    <div>
        <div id="atiRecipeImg" style="background-color: white; padding: 9px; width: 450px; float: left; margin-right: 18px; margin-bottom: 18px;"></div>    
        
        <ul class="hList">            
            <li>
                <input type="radio" class="rate" value="0.5"/>
                <input type="radio" class="rate" value="1"/>
                <input type="radio" class="rate" value="1.5"/>
                <input type="radio" class="rate" value="2"/>   
                <input type="radio" class="rate" value="2.5"/>                         
                <input type="radio" class="rate" value="3"/>
                <input type="radio" class="rate" value="3.5"/>                                
                <input type="radio" class="rate" value="4" />
                <input type="radio" class="rate" value="4.5"/>   
                <input type="radio" class="rate" value="5"/> 
            </li>
            <li id="atiRecipeRating"></li>
            <li id="atiRecipeStrict" style="float: right; width: 175px;">
                 <input type="radio" class="strict" value="1"/>
                 <input type="radio" class="strict" value="2" checked="checked"/>
                 <input type="radio" class="strict" value="3" />&nbsp;&nbsp;Paleo&nbsp;Strict
            </li>
        </ul>        
        <div id="atiUserBCard">
        </div>
        <a id="addThis"></a>
        <h2></h2>
        <span class="description"></span>
        <ul class="prepTimes">
        </ul>
        <ul class="ingredients">
        </ul>
        <div class="supportImages"></div>
    </div>
    <br style="clear: left;" />
    <span class="directions"></span>             

    <em>Comments:</em>  
    <asp:LinkButton ID="linkLogin" runat="server" Text="Login to leave a comment" OnClick="linkLogin_Click" />
    <ul class="atiCommentBox"></ul>

</div>




