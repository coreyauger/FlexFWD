<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_RecipeIndex.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_RecipeIndex" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<style type="text/css">
#atiPreview
{
	position: absolute;
	z-index: 99999;
	padding: 9px;
	background-color: White;
	border: 1px solid black;
}

ul#atiIndexList li
{
	list-style: none;
	list-style: none outside none;
	padding: 9px 0px 0px 18px;
}

ul#atiIndexList li.atiIndexTitle
{
	border-left: 3px solid #CCCCCC;
	padding: 9px 0px 0px 36px;
}


</style>

<script type="text/javascript">      
    
    
    Aqufit.Page.Controls.atiRecipeIndex = function( id, control ){
        this.id = id;
        this.controlId = '#' + control;   
        this.json = null;
        this.list = null;       
    };

    Aqufit.Page.Controls.atiRecipeIndex.prototype = {        
        clear: function(){
            $('#<%=atiIndexPanel.ClientID %>').children().remove();
        },
        
        generateStreamItem: function (sd, prepend) {          
            var linkTitle = sd["Title"].replace(/ /g, '-');
            linkTitle = linkTitle.replace(/\+/g, '');
            linkTitle = escape(linkTitle);
            var recipeUrl = '/recipe/'+sd["Id"]+'/';
            var html = '<li><a class="preview" id="recipe-'+sd["Id"]+'" href="'+recipeUrl+'" >'+sd["Title"]+'</a></li>';                        
            var that = this;
            if (prepend) {
                this.list.prepend(html);                
            } else {
                this.list.append(html);
            }                        
           
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
           
                       
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        },
        generateStreamDom: function (json) {
            $('#atiIndexLoading').hide();
            
            if( this.start < 0 ){
                this.start = 0;
            }  
            this.json = eval("(" + json + ")");
            var that = this;
            var atEnd = false;
                     
            $(this.controlId).append('<ul id="atiIndexList"></ul>');
            this.list = $('#atiIndexList');            
            var lastChar = '';
            for (var i = 0; i < this.json.length; i++) {
            
                var titleChar = this.json[i]["Title"][0].toUpperCase();
                if( titleChar != lastChar ){
                    lastChar = titleChar;
                    this.list.append('<li class="atiIndexTitle"><h2>'+lastChar+'</h2></li>');
                }
                this.generateStreamItem(this.json[i], false);
            } 
            
            
            $(".preview").hover(function (e) {     
              var myid = this.id.substr( this.id.indexOf('-')+1 );
               $("body").append('<p id="atiPreview"><img src="/DesktopModules/ATI_Base/services/images/recipe.aspx?r='+myid+'" alt="Image preview" /></p>');
               $("#atiPreview")
        			.css("top", (e.pageY - 10) + "px")
        			.css("left", (e.pageX + 25) + "px")
        			.fadeIn("fast");
            },
        	function () {
        	    $("#atiPreview").remove();
        	});
            $(".preview").mousemove(function (e) {
                if( e.pageX > ($('body').width() / 2 ) ){                    
                    $("#atiPreview")
            			.css("top", (e.pageY+10) + "px")
            			.css("left", (e.pageX+25) + "px");
                }else{
                    $("#atiPreview")
            			.css("top", (e.pageY+10) + "px")
            			.css("left", (e.pageX+25) + "px");
                }
            });                                               
        },
        getStreamData: function( ){
            this.clear();
            $('#atiIndexLoading').show();
            var that = this;
            Affine.WebService.StreamService.getRecipeIndex(Aqufit.Page.PortalId, function(json){ that.generateStreamDom(json); }, WebServiceFailedCallback);            
            
        }      
        
    };    

    $(function () {                
    
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiRecipeIndex('<%=this.ID %>', '<%=atiIndexPanel.ClientID %>');
                      
    });  
    
</script>

<div id="atiIndexTarget">
<div id="atiIndexLoading" style="text-align: center;">
    <img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/loading_blue_circle.gif") %>" />
</div>  
<asp:Panel ID="atiIndexPanel" runat="server">           
</asp:Panel>

