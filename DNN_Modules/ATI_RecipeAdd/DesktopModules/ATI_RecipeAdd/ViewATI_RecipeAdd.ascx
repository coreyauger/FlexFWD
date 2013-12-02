<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_RecipeAdd.ViewATI_RecipeAdd" CodeFile="ViewATI_RecipeAdd.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="Uploadify" Src="~/DesktopModules/ATI_Base/controls/ATI_Uploadify.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendsPhotos" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendsPhotos.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinksList" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinksList.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<script type="text/javascript">
                                
    Paleo = {
        rating: 2.5,
        strict: 2,        
        numIng: 1,        
        blurIng: function( elm ){   // THis is a bad hack to get ingedients to .. but it works.
            var ingNum = elm.id.replace(/atiRecipeIngredient-/g,'');
            var ingArray =  $('#<%=atiRecipeIngredients.ClientID %>').val().split('\n');
            if( ingArray == 0 ){
                $('#<%=atiRecipeIngredients.ClientID %>').val( $(elm).val() + '\n' );
            }else{
                $('#<%=atiRecipeIngredients.ClientID %>').val( '' );    // reset value
                for( var i=0; i < ingArray.length; i++ ){
                    if( i == (ingNum-1) ){
                        $('#<%=atiRecipeIngredients.ClientID %>').val( $('#<%=atiRecipeIngredients.ClientID %>').val() + $(elm).val() + '\n' );
                    }else{
                        $('#<%=atiRecipeIngredients.ClientID %>').val( $('#<%=atiRecipeIngredients.ClientID %>').val() + ingArray[i] + '\n' );
                    }
                }   
            }                    
        },
        AddMedia: function( id ){
            $('#atiRecipeInfo').hide();
            $('#atiRecipteMedia').show();
            Aqufit.Page.atiUploadifyImg1.setRecipeId(id);

            Aqufit.Page.atiUploadifyImg1.setCallback(function () {
                $('#atiMedia dl:eq(0)').fadeOut('slow');
            });
            Aqufit.Page.atiUploadifyImg2.setRecipeId(id);
            Aqufit.Page.atiUploadifyImg2.setCallback(function () {
                $('#atiMedia dl:eq(1)').fadeOut('slow');
            });
            Aqufit.Page.atiUploadifyImg3.setRecipeId(id);
            Aqufit.Page.atiUploadifyImg3.setCallback(function () {
                $('#atiMedia dl:eq(2)').fadeOut('slow');
            });
            Aqufit.Page.atiUploadifyImg4.setRecipeId(id);
            Aqufit.Page.atiUploadifyImg4.setCallback(function () {
                $('#atiMedia dl:eq(3)').fadeOut('slow');
            }); 
        },
        OnResponseEnd: function (sender, args){
            $('#atiStatusWidget').show();      
                
        },        
        OnRequestStart: function(sender, args){
            $('.dull').val('');
            $('#<%=bSaveRecipe.ClientID %>').hide();
                    
        },
        OnFail: function(){
            $('#<%=bSaveRecipe.ClientID %>').show();
        },
        focusIngredient: function (cntr) {
            $(cntr).unbind('focus');
            ++Paleo.numIng;
            $('#atiIngredientsFieldSet').append('<dl>' +
                    	'<dt><label for="atiRecipeIngredient-' + Paleo.numIng + '">Ingredient ' + Paleo.numIng + ':</label></dt>' +
                            '<dd><input type="text" name="atiRecipeIngredient-' + Paleo.numIng + '" id="atiRecipeIngredient-' + Paleo.numIng + '" maxlength="256" class="atiRecipeIngredient ui-corner-all ui-widget-content atiTxtBox" /></dd>' +
                        '</dl>');
            $('#atiRecipeIngredient-' + Paleo.numIng).focus(function () {
                Paleo.focusIngredient(this);
            });
            $('#atiRecipeIngredient-'+ Paleo.numIng).blur(function () {                    
                Paleo.blurIng(this);                           
            });           
        },
        initForm: function( r ){
            var $strict = $('#<%=atiHiddenRecipeStrict.ClientID %>');
            $strict.val(Paleo.strict);           
            var $rating = $('#<%=atiHiddenRecipeRate.ClientID %>');
            $rating.val(Paleo.rating);
            // textbox hints
            $('#<%=atiRecipeName.ClientID %>').val(r.Title);
            $('#<%=atiRecipeDescription.ClientID %>').val(r.Description);
            $('#<%=atiRecipePrep.ClientID %>').val(r.TimePrep);
            $('#<%=atiRecipeCook.ClientID %>').val(r.TimeCook);
            $('#<%=atiRecipeServings.ClientID %>').val(r.NumServings);
            var tags = r.Tags;
                    
            if( tags.indexOf("Breakfast") != -1 ){
                tags = tags.replace(/,Breakfast/g,'');
                $('input:checkbox[id$=breakfast]').attr('checked', true);
            }
            if( tags.indexOf("Lunch") != -1 ){
                tags = tags.replace(/,Lunch/g,'');
                $('input:checkbox[id$=lunch]').attr('checked', true);      
            }
            if( tags.indexOf("Dinner") != -1 ){
                tags = tags.replace(/,Dinner/g,'');
                $('input:checkbox[id$=dinner]').attr('checked', true);
            }
            if( tags.indexOf("Snack") != -1 ){
                tags = tags.replace(/,Snack/g,'');
                $('input:checkbox[id$=snack]').attr('checked', true);
            }
            if( tags.indexOf("Dessert") != -1 ){
                tags = tags.replace(/,Dessert/g,'');
                $('input:checkbox[id$=dessert]').attr('checked', true);
            }
            $('#<%=atiRecipeTags.ClientID %>').val(tags);
            $('#atiRecipeIngredient-1').focus(function () {                   
                    Paleo.focusIngredient(this);
            }); 
            $('#atiRecipeIngredient-1').blur(function () {                   
                    Paleo.blurIng(this);
                    //$('#<%=atiRecipeIngredients.ClientID %>').val(  $('#<%=atiRecipeIngredients.ClientID %>').val() + $(this).val() + '\n' );                    
            });            
            if( r.Id > 0 ){              
                $('#<%=atiRecipeDirections.ClientID %>').val(r.RecipeExtended.Directions);
                $('.dull').removeClass('dull');
                for( var i=0; i<r.RecipeExtended.Ingredients.length; i++ ){
                    $('#atiRecipeIngredient-' + Paleo.numIng).val(r.RecipeExtended.Ingredients[i].Name).trigger('focus');                   
                    $('#<%=atiRecipeIngredients.ClientID %>').val(  $('#<%=atiRecipeIngredients.ClientID %>').val() + r.RecipeExtended.Ingredients[i].Name + '\n' );
                }
                if( r.RecipeExtended.Image1Id > 0 ){
                    $('img#<%=atiRecipeImg1.ClientID %>').attr('src', Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/image.aspx?i=' + r.RecipeExtended.Image1Id);
                    $('div#atiRecipeImg1Div').show();
                    Aqufit.Page.atiUploadifyImg1.hide();
                }
                if( r.RecipeExtended.Image2Id > 0 ){
                    $('img#atiRecipeImg2').attr('src', Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/image.aspx?i=' + r.RecipeExtended.Image2Id);
                    $('div#atiRecipeImg2Div').show();
                    Aqufit.Page.atiUploadifyImg2.hide();
                }
                if( r.RecipeExtended.Image3Id > 0 ){
                    $('img#atiRecipeImg3').attr('src', Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/image.aspx?i=' + r.RecipeExtended.Image3Id);
                    $('div#atiRecipeImg3Div').show();
                    Aqufit.Page.atiUploadifyImg3.hide();
                }
                if( r.RecipeExtended.Image4Id > 0 ){
                    $('img#atiRecipeImg4').attr('src', Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/image.aspx?i=' + r.RecipeExtended.Image4Id);
                    $('div#atiRecipeImg4Div').show();
                    Aqufit.Page.atiUploadifyImg4.hide();
                }
            }else{
                $('#atiRecipeIngredient-1').val('eg: 8 oz. Grain Fed Tenderlion');
                $('.dull').focus(function () {
                    if ($(this).hasClass('dull')) {
                        $(this).removeClass('dull').val('');
                    }
                });               
            }
            
            Paleo.rating = r.AvrRating;
            $('input.rate').each(function () {
                    if ($(this).val() == Paleo.rating) {
                        $(this).attr('checked', 'true');
                    }
                }).rating(
            {
                split: 2,
                callback: function (value, link) {
                    Paleo.rating = value;
                    $rating.val(Paleo.rating);
                },
                focus: function (value, link) {
                    $('#atiRateDescription').html(value);
                },
                blur: function (value, link) {
                    $('#atiRateDescription').html(rating);
                }

            }
            );
            $('#atiRecipeRatingContainer div.rating-cancel').click(function () {
                // This is a hack to get around the fact that i dont see an event for when the person clears the rating
                rating = 0;
                $rating.val(Paleo.rating);
                $('#atiRateDescription').html(rating);
            });

            var strictDesc = ["Not Paleo", "Baby Steps", "Pretty Close", "Super Tight"];
            Paleo.strict = r.AvrStrictness;
            $('input.strict').each(function () {
                    if ($(this).val() == Paleo.strict) {
                        $(this).attr('checked', 'true');
                    }
                }).rating(
            {
                callback: function (value, link) {
                    Paleo.strict = value;
                    $strict.val(Paleo.strict);
                },
                focus: function (value, link) {
                    $('#atiStrictDescription').html(strictDesc[value]);
                },
                blur: function (value, link) {
                    $('#atiStrictDescription').html(strictDesc[Paleo.strict]);
                }

            }
            );
        }
    };

    $(function () {
        $('#atiStatusWidget').hide();
        $('#tabs').tabs();
        $('#<%=bSaveRecipe.ClientID %>').click(function(){
            $('.dull').val('');
        });
        if (navigator.appName.toLowerCase().indexOf("internet explorer") != -1) {
            $('#atiRightFloat').css('width', '70%');
          //  $('#profileBorder').css('border', '1px solid #666666');
        }      
        
        $('fieldset').wrap('<div class="formshadow ui-corner-all" style="background-color: white; margin-bottom: 18px; padding: 9px;">');
        //$('legend').addClass('ui-corner-all');
        $('#<%=bSaveRecipe.ClientID %>').button({
            icons: {
                primary: 'ui-icon-disk'
            }
        });                                         
        
        $('input').keydown(function (event) {       
            if (event.keyCode == 13) {                
                event.stopPropagation();
                return false;
            }
        });        
        
        <%if( this.RecipeEditId == 0 ){ %>
            var recipe = { 
                'Id':0,
                'Title': 'eg: Popeyes big steak breakfast',
                'Description':'eg: Steak and eggs power breakfast with a lot of protein and great taste.',
                'AvrRating' : Paleo.rating,
                'AvrStrictness' : Paleo.strict,
                'Tags' : 'eg: BBQ, Fast, High protein',
                'NumServings': '4',
                'TimePrep': '7',
                'TimeCook': '7',
                'RecipeExtended' : {
                    'Directions': '',
                    'Ingredients' : [
                        { 'Name':'eg: 8 oz. Grain Fed Tenderlion' }
                    ]
                }
            };
            Paleo.initForm(recipe);
        <%}else{ %>
            Affine.WebService.StreamService.getRecipe(<%=this.RecipeEditId %>,function(ret){
                var json = eval('(' + ret + ')');
                Paleo.initForm(json);
            }, function(){ alert('failed to get recipe'); });
        <%} %>         
        
        $("#<%=atiRecipeServings.ClientID %>, #<%=atiRecipeCook.ClientID %>, #<%=atiRecipePrep.ClientID %>").keydown(function (event) {
            // Allow only backspace and delete
            if (event.keyCode == 46 || event.keyCode == 8) {
                // let it happen, don't do anything
            }
            else {
                // Ensure that it is a number and stop the keypress                
                if (((event.keyCode >= 96 && event.keyCode <= 105) || (event.keyCode >= 48 && event.keyCode <= 57))) {
                    //alert(event.keyCode);
                } else {
                    event.preventDefault();
                }
            }
        });


        $('#bDoneSave').button().click(function (event) {
            self.location.href = '<%=ResolveUrl("~/") %>' + Aqufit.Page.UserName;
            event.stopPropagation();
            return false;
        });        
        $('#atiRecipeStrictContainer div.rating-cancel').click(function () {
            // This is a hack to get around the fact that i dont see an event for when the person clears the rating
            strict = 0;       
            $strict.val(strict);     
            $('#atiStrictDescription').html(strictDesc[strict]);
        }); 

    });

</script>
</telerik:RadCodeBlock>


<style type="text/css">

.removeButton
{
	position: relative; top: -175px; left: -100px;
}

#atiCatPanel label
{
	padding-left: 4px;
	padding-right: 45px;
}

#pageViewAdd ul,
#pageViewAdd dt
{
	padding-top: 9px;
	padding-right: 18px;
}

#pageViewAdd legend
{	
	font-weight: bold;
	padding-bottom: 3px;	
	border-bottom: 4px solid #2f3e51;
	width: 100%;
	color: #2f3e51;
	font-size: 14px;
}

#pageViewAdd ul li{
    line-style:none;        
    display: inline;       
    flow: horizontal;  
    color: #003366;
    font-family:Tahoma,Arial,Helvetica;
    font-size:11px;
    font-weight:bold;
    clear: both;
    padding-right: 19px;
} 

#pageViewAdd table tr td label
{
    display: block;
    color: #003366;
    font-family:Tahoma,Arial,Helvetica;
    font-size:11px;
    font-weight:bold;
}

div
{
	font-size:11px;
}

span.units
{
	color: #666666;
	font-size: 14px;
	font-weight:bold;
}
.ui-widget input.atiTxtBox, .ui-widget textarea.atiTxtArea, .ui-widget select.atiTxtBox
{
	width: 100%;
}
</style>


  <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnResponseEnd="Paleo.OnResponseEnd" OnRequestStart="Paleo.OnRequestStart"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="bSaveRecipe">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>   
            <telerik:AjaxSetting AjaxControlID="bRemoveImg1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>      
            <telerik:AjaxSetting AjaxControlID="bRemoveImg2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>  
            <telerik:AjaxSetting AjaxControlID="bRemoveImg3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>   
            <telerik:AjaxSetting AjaxControlID="bRemoveImg4">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>            
        </AjaxSettings>        
    </telerik:RadAjaxManager>

<div>
    <div style="float: left; width: 25%; ">
        <div style="padding-right: 19px;">
           <div id="profileBorder" class="dropshadow" style="padding: 9px; background-color: White;">
                <ati:ProfileImage ID="atiProfileImage" runat="server" Width="192px" />                                                                                                                       
           </div>                 
                 
           <div class="atiSideContainer" style="margin-top: 18px;">
            <span><asp:Literal ID="litFriendsTitle" runat="server" /></span>
                <ati:FriendsPhotos id="atiFriendsPhotos" runat="server" />
            </div>              
        </div>
    </div>
    <div id="atiRightFloat" style="float: right;  width: 75%;">
        <div id="atiStatusWidget" class="ui-widget">
        	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
        		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        		<asp:Literal ID="litStatus" runat="server" />
        	</div>
        </div>
        
        <div id="tabs">
    		<ul>
    			<li id="tabInbox"><a href="#pageViewAdd">Add Your Recipe</a></li>                                           				
    		</ul>
    		<div id="pageViewAdd">                                                        
                <div id="atiRecipeInfo">
                    <fieldset>
                	<legend>Recipe Info</legend>
                    <asp:HiddenField ID="atHiddenRecipeId" runat="server" Value="0" />
                    <dl>
                    	<dt><label for="atiRecipeName">Recipe Name:</label></dt>
                        <dd><asp:TextBox id="atiRecipeName" runat="server" maxlength="127" class="ui-corner-all ui-widget-content atiTxtBox dull" /></dd>
                   
                    	<dt><label for="atiRecipeDescription">Short Description:</label></dt>
                        <dd><asp:TextBox id="atiRecipeDescription" runat="server" maxlength="255" class="ui-corner-all ui-widget-content atiTxtBox dull" /></dd>
                        
                        <dt>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 500px;">
                        <tr>
                            <td>
                                <label for="atiRecipePrep">Prep Time:</label>
                                <asp:TextBox id="atiRecipePrep" maxlength="128" runat="server" class="ui-corner-all ui-widget-content atiTxtBoxSmall dull" />
                                <span class="units">Min</span>                   
                            </td>
                        
                            <td>
                                <label for="atiRecipeCook">Cook Time:</label>
                                <asp:TextBox id="atiRecipeCook" maxlength="128" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBoxSmall dull" />
                                <span class="units">Min</span> 
                            </td>
                        
                            <td>
                                <label for="atiRecipeServings">Number of Servings:</label>
                                <asp:TextBox id="atiRecipeServings" maxlength="128" runat="server" class="ui-corner-all ui-widget-content atiTxtBoxSmall dull" />
                                <span class="units">Servings</span> 
                           </td>
                        </tr>
                        </table>
                        </dt>
                        
                        <dt>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-top: 9px;">
                        <tr>
                            <td width="50%" id="atiRecipeRatingContainer">
                                <input type="hidden" ID="atiHiddenRecipeRate" runat="server" value="" />
                        	    <label for="atiRecipeRating">Your Rating:</label>                      
                                <input name="atiRecipeRating" type="radio" class="rate" value="0.5"/>
                                <input name="atiRecipeRating" type="radio" class="rate" value="1"/>
                                <input name="atiRecipeRating" type="radio" class="rate" value="1.5"/>
                                <input name="atiRecipeRating" type="radio" class="rate" value="2"/>   
                                <input name="atiRecipeRating" type="radio" class="rate" value="2.5" checked="checked"/>                         
                                <input name="atiRecipeRating" type="radio" class="rate" value="3"/>
                                <input name="atiRecipeRating" type="radio" class="rate" value="3.5"/>                                
                                <input name="atiRecipeRating" type="radio" class="rate" value="4" />
                                <input name="atiRecipeRating" type="radio" class="rate" value="4.5"/>   
                                <input name="atiRecipeRating" type="radio" class="rate" value="5"/> 
                                <span id="atiRateDescription" class="units" style="margin-left: 9px;"></span>                    
                            </td>
                            <td width="50%" id="atiRecipeStrictContainer">
                                <input type="hidden" ID="atiHiddenRecipeStrict" runat="server" value="" />
                                <label for="atiRecipeStrict">Paleo Strictness:</label></dt>                        
                                <input name="atiRecipeStrict" type="radio" class="strict" value="1"/>
                                <input name="atiRecipeStrict" type="radio" class="strict" value="2" checked="checked"/>
                                <input name="atiRecipeStrict" type="radio" class="strict" value="3" />                              
                                <span id="atiStrictDescription" class="units" style="margin-left: 9px;"></span>                           
                            </td>
                        </tr>
                        </table>
                        </dt>
                        
                    	<dt><label for="atiRecipeTags">Tags (seperate with a comma ',' )</label></dt>
                        <dd><asp:TextBox id="atiRecipeTags" runat="server" maxlength="127" class="ui-corner-all ui-widget-content atiTxtBox dull" /></dd>
                    
                        <dt><label for="atiRecipeCategories">Categories (select ALL that apply)</label></dt>
                        <dd>
                            <div id="atiCatPanel" style="padding-top: 9px; color: Black;">
                             <asp:Panel ID="atiRecipeCategoriesPanel" runat="server" />
                             </div>
                        </dd>
                    </dl>
                    
                    </fieldset>
                    
                    
                    <fieldset id="atiIngredientsFieldSet">
                	<legend>Ingredients</legend>
                    <dl>
                    	<dt><label for="atiRecipeIngredient-1">Ingredient 1:</label></dt>
                        <dd><input type="text" name="atiRecipeIngredient-1" id="atiRecipeIngredient-1" maxlength="256" class="atiRecipeIngredient ui-corner-all ui-widget-content atiTxtBox dull" /></dd>
                    </dl>                
                    </fieldset>
                    
                    <fieldset>
                	<legend>Directions</legend>
                    <dl>
                    	<dt><label for="atiRecipeDirections">Cooking Instructions:</label></dt>
                        <dd><asp:TextBox id="atiRecipeDirections" runat="server" TextMode="MultiLine" class="ui-corner-all ui-widget-content atiTxtArea dull">BBQ the Tenderloin to your desired liking
In a small bowl crack 3 eggs
ect..</asp:TextBox></dd>
                    </dl>                
                    </fieldset>
                    
                    <fieldset>
                    <dl>
                    	<dt>                       
                        <asp:Button ID="bSaveRecipe" runat="server" Text="Save" OnClick="bSaveRecipe_Click" />
                        </dt>
                    </dl>
                    </fieldset>
                </div>  
                
                <asp:TextBox ID="atiRecipeIngredients" runat="server" TextMode="MultiLine" style="display: none;;"></asp:TextBox>
                <div id="atiRecipteMedia" style="display: none;">
                    <fieldset id="atiMedia">
                	<legend>Image Upload</legend>
                    <div class="ui-corner-all ui-state-highlight" style="margin-top: 20px; padding: 18px; margin-bottom: 8px;"> 
    		            <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
    		            Select an image to upload by clicking the 'Choose Image' button.</p>
    	            </div>   
                    
                    <dl>
                    	<dt><label for="atiUploadifyImg1">Image 1:</label></dt>
                        <dd>
                            <ati:Uploadify id="atiUploadifyImg1" runat="server" ShowInfoHeader="false" />
                            <div id="atiRecipeImg1Div" style="display: none;">                            
                            <img id="atiRecipeImg1" runat="server" /><asp:Button ID="bRemoveImg1" runat="server" Text="Remove" OnClick="bRemoveImg1_Click" CssClass="removeButton" /></div>
                        </dd>
                    </dl>  
                    <dl>
                    	<dt><label for="atiUploadifyImg2">Image 2:</label></dt>
                        <dd>
                            <ati:Uploadify id="atiUploadifyImg2" runat="server" ShowInfoHeader="false" />
                            <div id="atiRecipeImg2Div" style="display: none;">                            
                            <img id="atiRecipeImg2" /><asp:Button ID="bRemoveImg2" runat="server" Text="Remove" OnClick="bRemoveImg_Click" CssClass="removeButton" /></div>
                        </dd>
                    </dl>  
                    <dl>
                    	<dt><label for="atiUploadifyImg3">Image 3:</label></dt>
                        <dd>
                            <ati:Uploadify id="atiUploadifyImg3" runat="server" ShowInfoHeader="false" />
                            <div id="atiRecipeImg3Div" style="display: none;">                            
                            <img id="atiRecipeImg3" /><asp:Button ID="bRemoveImg3" runat="server" Text="Remove" OnClick="bRemoveImg_Click" CssClass="removeButton" /></div>
                        </dd>
                    </dl>    
                    <dl>
                    	<dt><label for="atiUploadifyImg4">Image 4:</label></dt>
                        <dd>
                            <ati:Uploadify id="atiUploadifyImg4" runat="server" ShowInfoHeader="false" />
                            <div id="atiRecipeImg4Div" style="display: none;">                            
                            <img id="atiRecipeImg4" />
                            <asp:Button ID="bRemoveImg4" runat="server" Text="Remove" OnClick="bRemoveImg_Click" CssClass="removeButton" /></div>
                        </dd>
                    </dl>  
                              
                    </fieldset>
                     <fieldset>
                    <dl>
                    	<dt><button id="bDoneSave">Done</button></dt>
                    </dl>
                    </fieldset>
                </div>
            </div>		         			
    	</div>
    
    
    </div>
</div>    

    

         
    
               



