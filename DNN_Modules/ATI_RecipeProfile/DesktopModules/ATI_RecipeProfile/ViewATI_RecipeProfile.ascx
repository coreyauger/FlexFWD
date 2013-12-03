<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_RecipeProfile.ViewATI_RecipeProfile" CodeFile="ViewATI_RecipeProfile.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendsPhotos" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendsPhotos.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinksList" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinksList.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<link id="paeloStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/paleoStream.css")%>" type="text/css" rel="stylesheet">

<script type="text/javascript">

        Aqufit.Page.Tabs = {
            SwitchTab: function (ind) {
                $('#tabs').tabs('select', ind);            
            }
        };
    
     
        function OnResponseEnd(sender, args){
            $('#atiStatusWidget').show();                      
        }        
        
        function OnRequestStart(sender, args){
                            
        }          
    
    $(function () {
        var currInd = 0;
        $('#atiStatusWidget').hide();
        $('#tabs').tabs();
        $('#<%=atiFollow.ClientID %>').css('width','100%').button();        
        $('fieldset').wrap('<div class="formshadow ui-corner-all" style="background-color: white; margin-bottom: 18px; padding: 9px;">');
        $('#bSearchRecipes2').button({
            icons: {
                primary: 'ui-icon-search'
            }
        }).click(function (event) {
            var s = $('#atiTxtSearch2').val();
            Aqufit.Page.atiStreamScript.getStreamData(currInd, s);       
            event.stopPropagation();
            return false;
        });
        /*
         $("#atiTxtSearch2").keyup(function (event) {
            alert(event.keyCode);
            if (event.keyCode == 13) {
                $("#atiTxtSearch2").trigger('click');
                event.stopPropagation();
                return false;
            }
        });*/
        if (navigator.appName.toLowerCase().indexOf("internet explorer") != -1) {
            $('#atiRightFloat').css('width', '73%');
            $('#profileBorder').css('border', '1px solid #666666');
        }
        var streamScript = null;
        $('#tabs').bind('tabsselect', function(event, ui) {
           $('#<%=atiTabState.ClientID %>').val(ui.index);
            // Objects available in the function context:
           // ui.tab;     // anchor element of the selected (clicked) tab
           // ui.panel;   // element, that contains the selected/clicked tab contents
           // ui.index;   // zero-based index of the selected (clicked) tab
            Aqufit.Page.atiStreamScript.clear();
            var streamScript = $('#atiStreamTarget').detach();            
            streamScript.appendTo( '#'+ui.panel.id+' div' ); 
            currInd = ui.index+1;            
            Aqufit.Page.atiStreamScript.getStreamData(ui.index+1, '<%= Request["s"] != null ? Request["s"] : ""%>');                  
        });       
        <% if(Request["t"] != null && Request["t"] == "fav"){ %>
            Aqufit.Page.Tabs.SwitchTab(1);            
        <%} else{ %>
            var ind = parseInt( $('#<%=atiTabState.ClientID %>').val() );           
            Aqufit.addLoadEvent( function(){ 
                if( ind == 0 ){
                    currInd = 1;
                    Aqufit.Page.atiStreamScript.getStreamData(1, '<%= Request["s"] != null ? Request["s"] : ""%>');  
                    var streamScript = $('#atiStreamTarget').detach();            
                    streamScript.appendTo( '#pageViewStream div' ); 
                 }else{                    
                    Aqufit.Page.Tabs.SwitchTab( ind );
                 }
            } );            
        <%} %>
    });

</script>
</telerik:RadCodeBlock>

<div style="position: absolute; left: -9999px;">
<ati:StreamScript ID="atiStreamScript" runat="server" />       
</div>    

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="atiFollow" >
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
                <telerik:AjaxUpdatedControl ControlID="atiFollow" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>               
            </UpdatedControls>
        </telerik:AjaxSetting>   
    </AjaxSettings>        
</telerik:RadAjaxManager>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   
    
<asp:HiddenField ID="atiTabState" runat="server" Value="0" EnableViewState="true" />
<div>
    <div style="float: left; width: 25%;">
        <div style="padding-right: 19px;">
            <div id="profileBorder" class="dropshadow" style="padding: 9px; background-color: White;">
                <ati:ProfileImage ID="atiProfileImage" runat="server" Width="192px" />                                                                                                                                   
           </div>  
           
           <div style="margin-top: 9px;">
                <asp:Button ID="atiFollow" runat="server" Text="Follow Chef" OnClick="atiFollow_Click" />
           </div> 
           
           <ul class="linksList">
            <li runat="server" id="liCreations"><a href="javascript: ;" onclick="Aqufit.Page.Tabs.SwitchTab(2);"><asp:Literal ID="lNumCreations" runat="server" /></a></li> 
            <li runat="server" id="liFav"><a href="javascript: ;" onclick="Aqufit.Page.Tabs.SwitchTab(1);">Favorites (<asp:Literal ID="lNumFavorites" runat="server" />)</a></li>                                      
            <li runat="server" id="liEdit"><a id="bEditProfile" runat="server">Edit Profile</a></li>
           </ul>           
           
            <div class="atiSideContainer" style="margin-top: 18px;">
            <span>Web Links</span>                            
               <ati:WebLinksList id="atiWebLinksList" runat="server" />                          
            </div>                                          
            <div class="atiSideContainer">
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
        <!--
        <div style="position: absolute; z-index: 999; top: 75px; left: 290px;"><input type="text" id="atiTxtSearch2" class="ui-corner-all ui-widget-content" style="height: 20px; background: none; background-color: #EEEEEE; color: #666666; width: 150px;" /><button id="bSearchRecipes2">Search</button></div>
        -->
        <div id="tabs">
    		<ul>
    			<li><a href="#pageViewStream">Chefs <asp:Literal ID="lUserName" runat="server" /> follows</a></li>	
                <li><a href="#pageViewFavorites">Favorites (<asp:Literal ID="lNumFavorites2" runat="server" />)</a></li>	
                <li><a href="#pageViewMine"><asp:Literal ID="lNumCreations2" runat="server" /></a></li>		                                       				
    		</ul>
    		<div id="pageViewStream">                                                                      
                <div style="background-color: white;">
                    
                </div>       
            </div>	
            <div id="pageViewFavorites">                                                                      
                <div style="background-color: white;">                 
                </div>       
            </div>	
            <div id="pageViewMine">                                                                      
                <div style="background-color: white;">  
                </div>       
            </div>	         			
    	</div>
    
    
    </div>
</div>    



         
    
               



