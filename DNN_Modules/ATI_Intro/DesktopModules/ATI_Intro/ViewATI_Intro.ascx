<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Intro.ViewATI_Intro" CodeFile="ViewATI_Intro.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="FindInvite" Src="~/DesktopModules/ATI_Base/controls/ATI_FindInvite.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="GMap" Src="~/DesktopModules/ATI_Base/controls/ATI_GMap.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileLinks" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileLinks.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="DataPager" Src="~/DesktopModules/ATI_Base/controls/ATI_DataPager.ascx" %>
<%@ Register TagPrefix="ati" TagName="CompetitionAthlete" Src="~/DesktopModules/ATI_Base/controls/ATI_CompetitionAthlete.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">

<style type="text/css">

  #workoutList
  {
  	float: right;
  }
    
.atiGadgetContainer
{
	padding-bottom: 10px;
}

.atiMainFeed div.atiStreamItemRight
{
	width: 460px;
}
div.stepIndicator
{
	position: absolute;
	top: 10px;
	right: 20px;
	display: none;
}
div.stepIndicator span
{
	width: 10px;
	height: 25px;
	border: 1px solid #999;
	display: inline-block;
	margin: 0px 5px;	
	-moz-border-radius: 5px;	
	border-radius: 5px;	
}

div.stepIndicator span.stepDone
{
    background: -webkit-gradient(linear, 0% 0%, 0% 100%, from(#ffd304), to(#d66c29));
	background: -moz-linear-gradient(-90deg, #ffd304, #d66c29);
	filter: progid:DXImageTransform.Microsoft.gradient(enabled='true',startColorstr=#ffd304, endColorstr=#d66c29);		
	_background: #d66c29;
}

div.stepByStep
{	
	background-color: #fff;
	position: relative;
}
div.stepByStep > h3
{
	font-size: 26px;
	color: #3A95CD;
	margin: 0px;
	padding: 5px 10px;
	border-top: 1px solid #CCC;
	border-bottom: 1px solid #CCC;
}

div.stepByStep h2
{
	font-size: 12px;
	font-weight: bold;
	color: #666;	
}
div.stepByStep div.introWrap
{

}
span.intoContent
{
	padding: 10px;
	display: block;	
	margin-bottom: 10px;
}

ul#atiPlacesList li
{
	display: block;
	padding: 6px 10px;
	cursor: pointer;
}
ul#atiPlacesList li:nth-child(odd)
{
	background-color: #eee;
}
ul#atiPlacesList li:hover
{
	background-color: #000;
	color: #fff;
}
div#panelGroupActions
{
	position: relative; 
	padding: 20px;
}

div.stepActions
{
	padding: 20px;
	text-align: right;
}

.bBack
{
	float: left;
}
.compAthlete a
    {
    	color: #faa01b !important;
    	font-size: 12px;
    	text-decoration: underline;
    }
    .compAthlete a:hover
    {
    	text-decoration: none;
    }
    .compAthete ul
    {
    	padding: 10px 40px;
    }
    .compAthete ul li
    {
    	list-style: none;
    }
    
    .compAthleteIsMe
    {
    	border: 3px solid #ccc;
    	width: 300px;
    	margin: 20px auto;
    	cursor: pointer;
    	position: relative;
    }    
    .compAthleteIsMe:hover
    {
    	border: 3px solid #faa01b;    	
    }
    .compAthleteIsMeButton
    {
    	position: absolute;
    	top: 50px;
    	right: 10px;
    }

<% if(Request["Step"] != null ){ %>
span.intoContent
{
	padding: 10px;
	display: block;	
	margin-bottom: 10px;
	font-size: 15px;
}
<%}%>                                 

</style>
<script type="text/javascript" >   
    
    Aqufit.Windows = {
        JoinGroupDialog: {
            win: null,
            open: function (json) {
               // var oJson = eval('(' + json + ')');
               // $('#groupStatusDiv').empty().html(oJson['html']);
                Aqufit.Windows.JoinGroupDialog.win = $find('<%=JoinGroupDialog.ClientID %>');
                Aqufit.Windows.JoinGroupDialog.win.show();
            },
            update: function(json){
                Aqufit.Page.atiLoading.remove();
                var oJson = eval('(' + json + ')');
                $('#panelJoinWrap').empty().html(oJson['html']);
            },
            close: function () {
                Aqufit.Page.atiLoading.remove();
                Aqufit.Windows.JoinGroupDialog.win.close();
            }
        },
        CreateGroup: {
            win: null,
            open: function () {
               Aqufit.Windows.CreateGroup.win = radopen('<%=ResolveUrl("~/Groups/CreateGroup") %>', 'UploadWin');
                Aqufit.Windows.CreateGroup.win.set_modal(true);
                Aqufit.Windows.CreateGroup.win.setSize(977, 600);
                Aqufit.Windows.CreateGroup.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
                Aqufit.Windows.CreateGroup.win.center();
                Aqufit.Windows.CreateGroup.win.show();
            },        
            close: function () {
                Aqufit.Windows.CreateGroup.win.close();
            }
        },
        PlayerWin: {
            win: null,
            open: function (arg) {
                Aqufit.Windows.PlayerWin.win = radopen('<%=ResolveUrl("~/Community/Player") %>?m='+arg, 'PlayerWin');
                Aqufit.Windows.PlayerWin.win.set_modal(true);
                Aqufit.Windows.PlayerWin.win.setSize(800, 772);
                Aqufit.Windows.PlayerWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Maximize);
                Aqufit.Windows.PlayerWin.win.center();
                Aqufit.Windows.PlayerWin.win.show();
                return false;
            },
            close: function () {
                if (Aqufit.Windows.PlayerWin.win) {
                    Aqufit.Windows.PlayerWin.win.close();
                    Aqufit.Windows.PlayerWin.win = null;
                }
            }
        }
    };
     

    Aqufit.Windows.SuccessDialog = {
        win: null,
        open: function (json) {
            var oJson = eval('(' + json + ')');
            $('#successStatusDiv').empty().html(oJson['html']);
            Aqufit.Windows.SuccessDialog.win = $find('<%=SuccessDialog.ClientID %>');
            Aqufit.Windows.SuccessDialog.win.show();
        },
        close: function () {
            Aqufit.Windows.SuccessDialog.win.close();
        }
    };
   

    Aqufit.Page.Actions = {
        currentPlaces: null,
        selectedPlace: null,
        gymOwner: false,
        currStep: 0,
        ConnectCompetitionAthlete: function(aid){
            $('#<%=hiddenAjaxAction.ClientID %>').val('connectCompAthlete');
            $('#<%=hiddenAjaxValue.ClientID %>').val(aid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Intro","ATI_Intro") %>', '');
        },
        SendFriendRequest: function (fid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('friendRequest');
            $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Intro","ATI_Intro") %>', '');
        },
        CenterPlace: function (p) {
            Aqufit.Page.atiLoading.addLoadingOverlay('step3');
            Aqufit.Page.Actions.selectedPlace = p;
            $('#panelGroupActions').attr('style', 'background-color: #ccffcc;');
            $('#panelGroupActions h1').html(p["Name"]);
            Aqufit.Page.atiGMap.setCenter(p["Lat"], p["Lng"]);
            Aqufit.Page.atiGMap.setZoom(11);
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Group Select', Aqufit.Page.UserName + ' => '+ Aqufit.Page.Actions.selectedPlace["UserName"], Aqufit.Page.UserSettingsId]);
            } 
            Aqufit.Windows.JoinGroupDialog.open();
            Affine.WebService.StreamService.getGroupMembersToFriend(Aqufit.Page.UserSettingsId, p["GroupKey"], 0, 25, function (json) {
                Aqufit.Page.atiMemberRequestScript.generateStreamDom(json);
                Aqufit.Page.atiLoading.remove();
            }, function (err) { alert(err.get_message()); });
        },
        OnClientSelectedIndexChangedEventHandler: function (sender, args) {
            var item = args.get_item();
            if (item.get_value() != '') {
                var p = eval('(' + item.get_value() + ')');
                if( _gaq ){
                    _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Group Select By Name', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
                } 
                Aqufit.Page.Actions.CenterPlace(p);                
            }
        },
        RecordSave: function(){
           if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:1 - Profile Pic Save', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);                
            } 
        },
        LoadStep: function( step ){            
            Aqufit.Page.Actions.currStep = step;
            $('#panelInvite').hide();
            $('td.ati_topBannerSpan div').hide();   // hide all other site functionality.
            $('.stepActions').show();
            $('div.stepByStep').hide();
            $('#step2a').show();
            $('div#step'+step).show();
            $('div.stepIndicator').show();
            if( Aqufit.Page.Actions.gymOwner ){
                $('#ownerInstructions').show();
                $( "#bJoinGroup" ).html("Claim Ownership");   
            }else{
                $('#ownerInstructions').hide();
                $( "#bJoinGroup" ).html("Join Group");   
            }                   
        }
    };

    $(function () {
        $('#tabs').tabs();
        $('.stepActions').hide();
        $('#atiStatusWidget').hide();
        $('#bCreateGroup').button().click(function (event) {
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Create Group', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Windows.CreateGroup.open();
            event.stopPropagation();
            return false;
        });
        $('.bBack').button().click(function(event){
            var bStep = Aqufit.Page.Actions.currStep-1;
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:'+Aqufit.Page.Actions.currStep+' - Back', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.LoadStep(bStep);
            event.stopPropagation();
            return false;
        });
        $('#bBegin').button().click(function (event) {
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:4 - Done', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            top.location.href = '<%= ResolveUrl("~/") %>' + Aqufit.Page.UserName + '?w=0';
            event.stopPropagation();
            return false;
        });
        $('.skip0').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:0 - Skip', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.LoadStep(1);
            event.stopPropagation();
            return false;
        });
        $('#bSkip1').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:1 - Skip', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.LoadStep(2);
            event.stopPropagation();
            return false;
        });
        $('#bSkip3').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Skip', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.LoadStep(4);
            event.stopPropagation();
            return false;
        });
        $('#bOwnerYes').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:2 - YES Owner', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.gymOwner = true;
            Aqufit.Page.Actions.LoadStep(3);
            Aqufit.Page.atiGMap.refresh();
            event.stopPropagation();
            return false;
        });
        $('#bOwnerNo').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:2 - No Owner', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.gymOwner = false;
            Aqufit.Page.Actions.LoadStep(3);
            Aqufit.Page.atiGMap.refresh();
            event.stopPropagation();
            return false;
        });
        $('#<%=rbFollowDotCom.ClientID %> input').change(function(){
            Aqufit.Page.atiLoading.addLoadingOverlay('step3');
            $('#<%=hiddenAjaxAction.ClientID %>').val('toggleDotCom');
            $('#<%=hiddenAjaxValue.ClientID %>').val($(this).val());
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Intro","ATI_Intro") %>', '');
            event.stopPropagation();
        });
        $('#bContactUs').button().click(function (event) {
            top.location.href = '<%= ResolveUrl("~/ContactUs.aspx") %>';
            event.stopPropagation();
            return null;
        });
        $('#bGroupDialogClose').button().click(function (event) {
            Aqufit.Windows.GroupDialog.close();
            event.stopPropagation();
            return null;
        });
        $('#bSuccessDialogClose').button().click(function(event){
            Aqufit.Windows.SuccessDialog.close();
            event.stopPropagation();
            return null;
        });
        $('#bJoinGroup').click(function (event) {
            Aqufit.Page.atiLoading.addLoadingOverlay('panelJoinWrap');
            $('#<%=hiddenAjaxAction.ClientID %>').val('joinGroup');
            $('#<%=hiddenAjaxValue.ClientID %>').val(Aqufit.Page.Actions.selectedPlace["GroupKey"]);
            $('#<%=hiddenAjaxValue2.ClientID %>').val(Aqufit.Page.Actions.gymOwner);
            $('#<%=hiddenAjaxValue3.ClientID %>').val( $('#cbRequestAllMembers:checked').size() );
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Join Group', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
                _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Friend All Memebers: ' + $('#cbRequestAllMembers:checked').size(), Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Intro","ATI_Intro") %>', '');
            event.stopPropagation();
            return false;
        });
        Aqufit.Page.atiGMap.onPlacesResponse = function (places) {
            var $list = $('#atiPlacesList');
            $list.empty();
            Aqufit.Page.Actions.currentPlaces = places.Data;
            for (var i = 0; i < Aqufit.Page.Actions.currentPlaces.length; i++) {
                var place = Aqufit.Page.Actions.currentPlaces[i];
                $list.append('<li onclick="Aqufit.Page.Actions.CenterPlace(Aqufit.Page.Actions.currentPlaces[' + i + '])">' + place['Name'] + '</li>');
            }
        }
        Aqufit.Page.atiGMap.onMarkerClick = function (place) {
            if( _gaq ){
                _gaq.push(['_trackEvent', 'Signup', 'STEP:3 - Group Select By Map', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            } 
            Aqufit.Page.Actions.CenterPlace(place);
        }
                           
    });

    Aqufit.addLoadEvent(function () {
        Aqufit.Page.atiGMap.setPager(Aqufit.Page.<%=atiDataPager.UniqueID %>);
        Aqufit.Page.atiMemberRequestScript.sendFriendRequest = Aqufit.Page.Actions.SendFriendRequest; 
        Aqufit.Page.atiMemberRequestScript.onDataNeeded = function(skip, take){
            Affine.WebService.StreamService.getGroupMembersToFriend(Aqufit.Page.UserSettingsId, Aqufit.Page.Actions.selectedPlace["GroupKey"], skip, take, function (json) {
                Aqufit.Page.atiMemberRequestScript.generateStreamDom(json);
                Aqufit.Page.atiLoading.remove();
            }, function (err) {  });
        }
    });

    
          
</script>    
</telerik:RadCodeBlock>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black" EnableShadow="true">
<Windows>    
    <telerik:RadWindow ID="JoinGroupDialog" runat="server" Skin="Black" Title="Join this group?" Width="800" Height="500" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div style="width: 100%; height: 100%; background-color: white; position: relative; overflow: auto;" id="panelJoinWrap">
                <div id="panelGroupActions">
                    <span style="position: absolute; right: 20px;">                                            
                        Send friend request to all group members: <input id="cbRequestAllMembers" type="checkbox" checked="true" style="border: 2px solid green;" />&nbsp;&nbsp;&nbsp;<button id="bJoinGroup" class="boldButton" style="border: 4px solid green;">Join Group</button>
                    </span>
                    <h1>&nbsp;</h1>   
                    <br /><br />
                    <span id="ownerInstructions" style="display: none;">* <strong>CLAIM YOUR GROUP </strong>You will be contacted via email to verify the ownership of this group.  Once that is done you will be granted administrative previlages in the group.</span>                                     
                </div>
                <div id="panelMembers" style="border: 1px solid #ccc;">
                    <div class="grad-FFF-EEE" style="display: block; padding: 2px 10px;"><h2>Member Listing</h2></div>
                    <ati:FriendListScript id="atiMemberRequestScript" runat="server" ControlMode="FRIEND_REQUEST" /> 
                </div>                                    
            </div>            
        </ContentTemplate>
    </telerik:RadWindow>       
</Windows>
</telerik:radwindowmanager>

<telerik:RadWindow ID="SuccessDialog" runat="server" Skin="Black" Title="Success" Width="450" Height="200" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div style="width: 100%; height: 100%; background-color: white; position: relative; overflow: hidden;">
               <div style="position: absolute; top: 25px; left: 40px; color: Black; font-size: 12px; width: 100%;">                   
                   <img id="imgCheck" runat="server" style="float: left; clear: left; padding-right: 10px;" />                       
                   <div id="successStatusDiv" style="padding: 10px; width: 100%;"></div>  
                   <button id="bSuccessDialogClose">Close</button>   
               </div>       
            </div>            
        </ContentTemplate>
</telerik:RadWindow>
  
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">     
    <ClientEvents OnRequestStart="OnRequestStart" OnResponseEnd="OnResponseEnd" />
    <AjaxSettings>          
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>    
    </AjaxSettings> 
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Office2007" />

<asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue2" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue3" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<ati:LoadingPanel ID="atiLoading" runat="server" />

<asp:Panel ID="atiFitness" runat="server">
    
    <div id="atiStatusWidget" class="ui-widget">
	    <div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		    <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		    <span id="statusMsg"></span>
	    </div>
    </div>
    
    <div>
        <div id="atiSidePanel" runat="server" style="height: 100%; float: left; width: 197px;">        
            <!-- ** Left Panel Zone -->          
            <ati:ProfileImage ID="atiProfileImage" runat="server" Small="true" />      
            <ati:ProfileLinks ID="atiProfileLinks" runat="server" ShowGettingSarted="false" />                         
            <!-- ** END Left Panel Zone -->    
        </div>
        <div id="atiMainPanel" runat="server" style="width: 728px; float: right;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr valign="top">
                <td style="padding-bottom: 7px;">                                    
                    <!-- Intro  -->

                    <div id="tabs">
    			        <ul>
    				        <li><a href="#pageInhtro">Getting Started on FlexFWD</a></li>
                        </ul>
    			        <div id="pageInhtro" style="padding: 0px; background-color: White;">                    
                            <!-- END Tabs -->                                                                                                                                      
                            <div id="step0" class="stepByStep">
                                <h3 class="grad-FFF-EEE">
                                    Is this your Crossfit Open Profile?
                                    <button class="skip0" style="float: right;">No this is not me.</button>
                                </h3>
                                <asp:PlaceHolder ID="phCompetitionAthlete" runat="server" />
                                <div class="stepActions">
                                    <button class="skip0">No this is not me.</button>
                                </div>
                            </div>
                            <div id="step1" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 1</h3>
                                <div class="stepIndicator">
                                    <span class="stepDone">&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent"><asp:Literal ID="instruction1" runat="server" /></span>
                                    <div style="margin: auto; width: 300px; margin-bottom: 20px;">
                                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" /><asp:Button ID="bUpload" runat="server" Text="Save" OnClientClick="Aqufit.Page.Actions.RecordSave" OnClick="bUpload_Click" CssClass="boldButton" />
                                    </div>
                                </div>
                                <div class="stepActions">
                                    <button id="bSkip1">Skip</button>
                                </div>                                
                            </div>

                            <div id="step2" class="stepByStep">                               
                                <h3 class="grad-FFF-EEE">Step 2</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <span class="stepDone">&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                </div>
                                <div class="introWrap" id="panelInvite">
                                    <span class="intoContent">Search your email for friends already on FlexFWD.</span>
                                    <ati:FindInvite id="atiFindInvite" runat="server" />
                                </div>
                                <div class="introWrap" id="step2a" style="display: none;">
                                    <span class="intoContent">Are you a gym owner,coach, or personal trainer ?</span>
                                    <span class="intoContent" style="text-align: center;">                                        
                                        <button id="bOwnerYes">Yes</button>
                                        <button id="bOwnerNo">No</button>
                                    </span>
                                </div>
                                <div class="stepActions" style="height: 25px;">                                    
                                    <button class="bBack">Back</button>
                                    &nbsp;
                                </div>
                            </div>

                            <div id="step3" class="stepByStep" style="position:relative; min-height: 300px;">
                                <h3 class="grad-FFF-EEE">Step 3</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(2);"><span class="stepDone">&nbsp;</span></a>
                                    <span class="stepDone">&nbsp;</span>
                                    <span>&nbsp;</span>
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">Find a group or gym that you belong to, start your own<span class="stepActions">, or <a href="javascript: Aqufit.Page.Actions.LoadStep(4);" style="text-decoration: underline; font-weight: bold;">Skip</a> this step</span>.</span>                        
                                    <span class="intoContent">Do you follow 'crossfit.com' programming?
                                        <span style="float: right;">
                                        <asp:RadioButtonList runat="server" ID="rbFollowDotCom" TextAlign="Left" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="1" />
                                            <asp:ListItem Text="No" Value="0" Selected="True" />
                                        </asp:RadioButtonList>
                                        </span>
                                    </span>
                                    <span class="intoContent">Find gym or group by name:
                                        <span style="float: right;">
                                        <telerik:RadComboBox ID="atiRadComboBoxSearchGroups" runat="server" Width="475px" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                            EmptyMessage="Enter Group Name (eg: crossfit flexfwd)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                            EnableVirtualScrolling="true"
                                            OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                                            <WebServiceSettings Method="GetGroupSearch" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
                                        </telerik:RadComboBox>
                                        </span>
                                        <br /><br />
                                        Can't find your gym or group?  Click here to create a new one: <button id="bCreateGroup" style="float: right;" >Create New Group</button>
                                        <br /><br />
                                        OR search by map location:
                                    </span>
                                    <div id="panelMap" style="padding-left: 20px;">
                                        <div style="border: 1px solid #999; position: absolute; right: 20px;"><ati:GMap id="atiGMap" runat="server" Mode="GROUP_FINDER" Height="200px" Width="400px" /></div>
                                        
                                        <div style="width: 260px; border: 1px solid #999; height: 200px;">
                                            <ati:DataPager id="atiDataPager" runat="server" />
                                            <div style="width: 260px; height: 175px; overflow: scroll;">
                                            
                                                <ul id="atiPlacesList">
                            
                                                </ul>
                                            </div>
                                        </div>
                                    </div>  
                                    <br /><br />                                  
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>
                                    <button id="bSkip3">Skip</button>
                                </div>    
                            </div>

                            <div id="step4" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 4</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(2);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(3);"><span class="stepDone">&nbsp;</span></a>
                                    <span class="stepDone">&nbsp;</span>                                    
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">Watch videos to get you started on FlexFWD.
                                    <br /><br />     
                                    <center>                               
                                    <ul class="hlist">
                                        <li style="list-style: none;">
                                        <a onclick="Aqufit.Windows.PlayerWin.open('flex_tutorial01.flv');" href="javascript: ;">
                                            <div class="tutMovie" style="display: inline-block;">
                                                <img alt="" src="/DesktopModules/ATI_Base/resources/images/iMovie.png">
                                                Find Friends
                                            </div>
                                        </a>
                                        </li>
                                        
                                        <li>
                                            <a onclick="Aqufit.Windows.PlayerWin.open('flex_tutorial02.flv');" href="javascript: ;">
                                            <div class="tutMovie" style="display: inline-block;">
                                                <img alt="" src="/DesktopModules/ATI_Base/resources/images/iMovie.png">
                                                Join Groups
                                            </div>
                                        </a>
                                        </li>
                                    </ul>
                                    </center>
                                    </span>
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>
                                    <button id="bBegin">Begin</button>
                                </div>    
                            </div>
                                                        
                        </div>
                    </div>
                </td>               
            </tr>
            </table>
       </div>
       <div style="clear:both;"></div> 
    </div>    
    </asp:Panel>
         
    
               



