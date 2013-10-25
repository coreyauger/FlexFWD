<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ExerciseListScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ExerciseListScript" %>

<style type="text/css">        
    /* Exercises */
    
    div.exerciseWrapper
    {
    	padding: 16px;
    	border: 1px solid #CCCCCC;
    	height: 330px;
    	clear: both;
    	background-color: #EFEFEF;
    }      
    
    #atiExerciseSearch
    {
    	width: 100%;
    }   
    
    
    
    
    div.exerciseWrapper div.exerciseListScroll
    {
    	overflow: auto;
    	height: 280px;    	
    	margin-top: 8px;
    	border: 1px solid #ccc;    
    	background-color: #FFF;	
    }
    
    div.exerciseListScroll li
    {
    	font-size: 11px;
        color: #666666;
        display: block;
        padding: 2px 6px
    }     
        
    div.exerciseListScroll li:hover
    {
        background-color: #EFEFEF;    	
        color: #666666;
    } 
    
</style>


<style type="text/css">
	.draggable { width: 90px; height: 80px; padding: 5px; float: left; margin: 0 10px 10px 0; font-size: .9em; }
	.ui-widget-header p, .ui-widget-content p { margin: 0; }
	#snaptarget { height: 140px; }
	</style>
	<script type="text/javascript">
	    $(function () {
	        $("#draggable").draggable({ snap: true });
	        $("#draggable2").draggable({ snap: '.ui-widget-header' });
	        $("#draggable3").draggable({ snap: '.ui-widget-header', snapMode: 'outer' });
	        $("#draggable4").draggable({ grid: [20, 20] });
	        $("#draggable5").draggable({ grid: [80, 80] });
	        //hover states on the static widgets	        
	    });
	</script>



 <!-- TODO: unpack the javascript only one time -->
<script type="text/javascript" >
   

    Aqufit.Page.Controls.atiContactInvite = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;  
        this.iter = 0;
        this.iter2 = 0;
        this.json = null;
        this.list = null;
        this.exerciseList = [];
    };
    
    Aqufit.Page.Controls.atiContactInvite.prototype = {
        getExercise: function( id ){
            return this.exerciseList[id];
        },                                       
        generateStreamItem: function (sd, prepend) {    
            var html = '<li id="'+this.id+'-e' + sd["Id"] + '" class="ui-state-default ui-corner-all exerciseOption">' +                            
                            ''+sd["Name"] +                         
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
            $('#'+this.id+'-e' + sd["Id"]).hover(
				function () { $(this).addClass('ui-state-hover'); },
				function () { $(this).removeClass('ui-state-hover'); }
			);          
                    
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            this.iter++;
        },        
        generateStreamDom: function (json) {            
            var that = this;
            this.json = eval("(" + json + ")");
            $(this.controlId).children().remove();
            $(this.controlId).append('<div class="exerciseWrapper">'+
                                            '<div class="contactLeft ui-widget">'+
                                                '<h4 style="color: #1C94C4;">Drag and Drop Exercises</h4>'+
                                                '<input type="textbox" id="atiExerciseSearch" class="dull" style="height: 20px; border: 1px solid #666;" value="filter exercises" />'+
                                                '<div class="exerciseListScroll">'+
                                                '<ul id="'+this.id+'atiExerciseList"></ul>'+
                                                '</div>'+
                                            '</div>'+                                                 
                                        '</div>'
                                        );
            $('.dull').focus(function () {
                $(this).val('').removeClass('dull').unbind('focus');
            });      
            this.list = $('#'+this.id+'atiExerciseList');            
            for (var i = 0; i < this.json.length; i++) {
                this.exerciseList[this.json[i]["Id"]] = this.json[i];
                this.generateStreamItem(this.json[i], false);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////                  
            var cid = this.id;            
            $('#atiExerciseSearch').keyup(function(event) {  
                //if esc is pressed or nothing is entered  
                if (event.keyCode == 27 || $(this).val() == '') {  
                    //if esc is pressed we want to clear the value of search box  
                    $(this).val('');  
               
                    //we want each row to be visible because if nothing  
                    //is entered then all rows are matched. 
                    $('#'+cid+'atiExerciseList li').show();  
                    //$('tbody tr').removeClass('visible').show().addClass('visible');  
                }else{
                    //alert( $(this).val() );
                    var query =   $.trim($(this).val()); //trim white space  
                    query = query.replace(/ /gi, '|'); //add OR for regex query
                    $('#'+cid+'atiExerciseList li').each(function() {  
                        //alert( $(this).text() );
                        ($(this).text().search(new RegExp(query, "i")) < 0) ? $(this).hide() : $(this).show();  
                    });  
                }   
            });
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        }          
    };
    
   
    
    $(function () {
        //Affine.WebService.StreamService.getFriendListData(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.Controls.atiFriendList.generateStreamDom, WebServiceFailedCallback);
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiContactInvite('<%=this.ID %>', '<%=atiExerciseList.ClientID %>');
                      
    });  
        
    
</script>
<asp:Panel ID="atiExerciseList" runat="server" CssClass="atiContactInvite">             
</asp:Panel>
<div style="clear: both;"></div>
