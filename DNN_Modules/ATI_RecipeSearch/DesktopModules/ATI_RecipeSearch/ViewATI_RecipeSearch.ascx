<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_RecipeSearch.ViewATI_RecipeSearch" CodeFile="ViewATI_RecipeSearch.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="RecipeScript" Src="~/DesktopModules/ATI_Base/controls/ATI_RecipeScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="FeaturedProfile" Src="~/DesktopModules/ATI_Base/controls/ATI_FeaturedProfile.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<link id="paeloStream" type="text/css" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/paleoStream.css")%>" rel="stylesheet">

<style type="text/css">
div#atiAddToFav
{
	background-color: #dddddd; padding-left: 18px; padding-right: 18px;
	border: 1px solid #000000;
	cursor: pointer;
}
div#atiAddToFav:hover
{
	background-color: #FFFFFF;
	font-size: 10px;
}

#searchCats label
{
	padding-left: 4px;
	padding-right: 45px;
	color:#003366;
	font-size: 12px;
}

dt
{
	padding-top: 9px;
	padding-right: 18px;
}

</style>
<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<script type="text/javascript">

    function OnResponseEnd(sender, args){
        $('#atiStatusWidget').show();                      
    }        
    
    function OnRequestStart(sender, args){
                        
    } 

    $(function () {
        $('#atiStatusWidget').hide();
        $('#tabs').tabs();
        $('li.deleteStream').css('top', '0px');
        
        if (navigator.appName.toLowerCase().indexOf("internet explorer") != -1) {
            $('#atiLeftFloat').css('width', '73%');
        }
        
        <% if( !atiSearchResults.Visible  ){ %>
        if (navigator.appName.toLowerCase().indexOf("internet explorer") == -1) {
            $('#atiAddToFav').dropShadow();           
        } 
        <%}else{ %>
        
        $('fieldset').wrap('<div class="formshadow ui-corner-all" style="background-color: white; margin-bottom: 18px; padding: 9px;">');
        $('#atiSelectAll').click(function(){
            $('#searchCats input:checkbox').attr('checked',true);
        });
        $('#atiSelectNone').click(function(){
            $('#searchCats input:checkbox').attr('checked',false);
        });
        
        $("#atiRecipeBasicSearch").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#bBasicSearch").trigger('click');
                event.stopPropagation();
                return false;
            }
        });
        var $bsearch = $('#bBasicSearch');
        if (navigator.appName.toLowerCase().indexOf("internet explorer") == -1) {
            $bsearch.button({
                icons: {
                    primary: 'ui-icon-search'
                }
            }); 
        }
        $bsearch.click(function (event) {
            var cats = '';
            if( $('#searchCats input:checkbox[id$=breakfast]').attr('checked') ){
                cats += " Breakfast ";
            }
            if( $('#searchCats input:checkbox[id$=lunch]').attr('checked') ){
                cats += " Lunch ";
            }
            if( $('#searchCats input:checkbox[id$=dinner]').attr('checked') ){
                cats += " Dinner ";
            }
            if( $('#searchCats input:checkbox[id$=snack]').attr('checked') ){
                cats += " Snack ";
            }
            if( $('#searchCats input:checkbox[id$=dessert]').attr('checked') ){
                cats += " Dessert ";
            }
            self.location.href = '<%=ResolveUrl("~/Search.aspx") %>?s=' + $('#atiRecipeBasicSearch').val() + cats;
            event.stopPropagation();
            return false;
        });                          
        Aqufit.addLoadEvent( function(){ 
                Aqufit.Page.atiStreamScript.getStreamData(0, '<%= Request["s"] != null ? Request["s"] : ""%>');                 
        } );   
        <%} %>
    });

</script>
</telerik:RadCodeBlock>


<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="atiAddRemFav">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>   
    </AjaxSettings>        
</telerik:RadAjaxManager>



<div>
    <asp:Panel ID="atiRecipePanel" runat="server">

    <div id="atiStatusWidget" class="ui-widget">
    	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
    		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
    		<asp:Literal ID="litStatus" runat="server" />
    	</div>
    </div>

    <div id="atiFavContainer" style="position: absolute; left: -32px; top: 50px; z-index: 9999;">
    <div id="atiAddToFav">
        <asp:ImageButton id="atiAddRemFav" runat="server" OnClick="atiAddRemFav_Click" />
    </div>
    <img id="imgCorner" runat="server" />
    </div>

      <ati:RecipeScript id="atiRecipe" runat="server" />
    </asp:Panel>
     
    <asp:Panel ID="atiSearchResults" runat="server">    
      
        
        <div id="atiLeftFloat" style="float: left;  width: 75%;">                                            
           
            <div id="tabs">
        		<ul>
        			<li id="tabInbox"><a href="#pageViewStream">Search Results</a></li>			                                       				
        		</ul>
        		<div id="pageViewStream">                                                                      
                    <div style="background-color: white;">
                        <ati:StreamScript ID="atiStreamScript" runat="server" IsSearchMode="true" />       
                    </div>       
                </div>		         			
        	</div>
        </div>
        <div style="float: right; width: 25%; ">
             <asp:Panel ID="atiSearchBar" runat="server">
                <div class="formshadow ui-corner-all ui-widget">                                                                           
                  <fieldset>
                    <dl>
                        <dt><label for="atiRecipeBasicSearch">Search Terms:</label></dt>
                        <dd><input type="text" name="atiRecipeBasicSearch" id="atiRecipeBasicSearch" maxlength="127" class="ui-corner-all ui-widget-content atiTxtBox" style="width: 220px; margin-right: 9px;" /></dd>
                        
                        <dt style="padding-bottom: 9px;">Categories (<a href="javascript: ;" id="atiSelectAll" style="color:#F7931E; font-size: 10px;">select&nbsp;all</a>,&nbsp;&nbsp;<a href="javascript: ;" id="atiSelectNone" style="color:#F7931E; font-size: 10px;">none</a>)</dt>
                        <dd id="searchCats">
                            <table border="0" cellpadding="0" cellspacing="0">
                            <tr valign="top">
                                <td>
                                <asp:CheckBox id="atiCat_breakfast" runat="server" Text="Breakfast" Checked="false" /><br />
                                <asp:CheckBox id="atiCat_lunch" runat="server" Text="Lunch" Checked="false"/><br />
                                <asp:CheckBox id="atiCat_dinner" runat="server" Text="Dinner" Checked="false"/><br />
                                </td>
                                <td>
                                <asp:CheckBox id="atiCat_snack" runat="server" Text="Snack" Checked="false"/><br />
                                <asp:CheckBox id="atiCat_dessert" runat="server" Text="Dessert" Checked="false"/><br />
                                </td>
                            </tr>
                            </table>
                        </dd>
                        
                        <dt></dt>
                        <dd><button id="bBasicSearch">Search</button></dd>
                    </dl>
                    </fieldset>         
               </div>
              </asp:Panel>
        
        
            <div id="atiFeatured" style="padding-left: 9px;">
                <h3>Featured Chef</h3>
                <ati:FeaturedProfile ID="atiFeaturedProfile" runat="server" Small="true" Width="100px" />                     
            </div>
        </div>
    </asp:Panel>
    
    </div>
</div>    

    

         
    
               



