<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_CompRegister.ViewATI_CompRegister" CodeFile="ViewATI_CompRegister.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="GMap" Src="~/DesktopModules/ATI_Base/controls/ATI_GMap.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="BodyComp" Src="~/DesktopModules/ATI_Base/controls/ATI_BodyComposition.ascx" %>
<%@ Register TagPrefix="ati" TagName="DataPager" Src="~/DesktopModules/ATI_Base/controls/ATI_DataPager.ascx" %>
<%@ Register TagPrefix="ati" TagName="Slim" Src="~/DesktopModules/ATI_Base/controls/ATI_SlimControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="TimeSpan" Src="~/DesktopModules/ATI_Base/controls/ATI_TimeSpan.ascx" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">

<style type="text/css">
.compreg
{
	border: 1px solid #CCC;
	width: 600px;
	margin: auto;
	padding: 30px;
}                            

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
        RegType: 1,
        ConnectCompetitionAthlete: function(aid){
            $('#<%=hiddenAjaxAction.ClientID %>').val('connectCompAthlete');
            $('#<%=hiddenAjaxValue.ClientID %>').val(aid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompRegister","ATI_CompRegister") %>', '');
        },
        SendFriendRequest: function (fid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('friendRequest');
            $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompRegister","ATI_CompRegister") %>', '');
        },
        CenterPlace: function (p) {
            Aqufit.Page.atiLoading.addLoadingOverlay('step3');
            Aqufit.Page.Actions.selectedPlace = p;
            $('#affiliateName').html(p.Name);
            $('#<%=bNotMyAffiliate.ClientID %>').show();
            $('#panelFindAffiliate').hide();
            for( var x in Aqufit.Page.Groups ){
                if(Aqufit.Page.Groups[x] == p.GroupKey ){                                            
                    Aqufit.Page.atiLoading.remove();
                    return; // dont display a the Join Group for a group we are already in..
                }
            }
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
                // check to see if the user is already a part of the group                
                //alert(Aqufit.Page.Groups.length);
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
            $('.stepActions').show();
            $('div.stepByStep').hide();
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
        $('.dull').removeClass('dull');
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
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:'+Aqufit.Page.Actions.currStep+' - Back', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.Actions.LoadStep(bStep);
            event.stopPropagation();
            return false;
        });        
        $('#bNext1').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:1 - Done', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            // make changes to the form based on the type of registraction
            Aqufit.Page.Actions.RegType = $("#<%=ddlCompetitionType.ClientID %> option:selected").val();
            if( Aqufit.Page.Actions.RegType == 1 ){
                $('.individual').show();
                $('.team').hide();
            }else{
                $('.individual').hide();
                $('.team').show();
            }
            Aqufit.Page.Actions.LoadStep(2);
            Aqufit.Page.atiGMap.refresh();
            event.stopPropagation();
            return false;
        });        
        $('#bgoNext2').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:2 - Done', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            $('#<%=hiddenAjaxValue.ClientID %>').val(0);
            if( Aqufit.Page.Actions.RegType == 2 && Aqufit.Page.Actions.selectedPlace["GroupKey"]){ // team affilate (check that we are not at limit) 2 teams per affiliate
                Aqufit.Page.atiLoading.addLoadingOverlay('step2');
                $('#<%=hiddenAjaxAction.ClientID %>').val('checkTeamLimit');
                $('#<%=hiddenAjaxValue.ClientID %>').val(Aqufit.Page.Actions.selectedPlace["GroupKey"]);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompRegister","ATI_CompRegister") %>', '');
            }else{
                Aqufit.Page.Actions.LoadStep(3);    
            }        
            event.stopPropagation();
            return false;
        });
        $('#bNext3').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:3 - Done', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            var validationGroup = (Aqufit.Page.Actions.RegType == 1 ? 'compreg' : 'compregteam');
            if( Page_ClientValidate(validationGroup) ){
                Aqufit.Page.Actions.LoadStep(4);     
            }       
            event.stopPropagation();
            return false;
        });  
        $('#bNext4').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:4 - Done', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            var validationGroup = (Aqufit.Page.Actions.RegType == 1 ? 'compregEmerg' : 'compregMembers');
            if( Page_ClientValidate(validationGroup) ){
                Aqufit.Page.Actions.LoadStep(5);            
            }
            event.stopPropagation();
            return false;
        }); 
        $('#bSave').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:5 - Save', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            Aqufit.Page.atiLoading.addLoadingOverlay('step5');
            $('#<%=hiddenAjaxAction.ClientID %>').val('saveRegistration');
            $('#<%=hiddenAjaxValue.ClientID %>').val(0);
            if( Aqufit.Page.Actions.selectedPlace["GroupKey"] ){                
                $('#<%=hiddenAjaxValue.ClientID %>').val(Aqufit.Page.Actions.selectedPlace["GroupKey"]);
            }
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompRegister","ATI_CompRegister") %>', '');
            event.stopPropagation();
            return false;
        });

        $('#<%=bNotMyAffiliate.ClientID %>').button().click(function(event){
            if( _gaq ){
                _gaq.push(['_trackEvent', 'CompRegister', 'STEP:2 - Change Affiliate', Aqufit.Page.UserName, Aqufit.Page.UserSettingsId]);
            }
            $('#panelFindAffiliate').show();
            Aqufit.Page.atiGMap.refresh();
            $(this).hide();
            event.stopPropagation();
            return false;
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
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompRegister","ATI_CompRegister") %>', '');
            event.stopPropagation();
            return false;
        });
        Aqufit.Page.atiGMap.onPlacesResponse = function (places) {
            Aqufit.Page.atiLoading.remove();
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

    function OnResponseEnd(){
        Aqufit.Page.atiLoading.remove();
    }
          
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
    <ClientEvents OnResponseEnd="OnResponseEnd" />
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
    <asp:HiddenField ID="hiddenAjaxRegistrationId" runat="server" />
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
            <!-- ** END Left Panel Zone -->    
        </div>
        <div id="atiMainPanel" runat="server" style="width: 728px; float: right;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr valign="top">
                <td style="padding-bottom: 7px;">                                    
                    <!-- Intro  -->

                    <div id="tabs">
    			        <ul>
    				        <li><a href="#pageInhtro">Competition Registration</a></li>
                        </ul>
    			        <div id="pageInhtro" style="padding: 0px; background-color: White;">                    
                            <!-- END Tabs -->                                                                                                                                                                 
                            <div id="step1" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 1</h3>
                                <div class="stepIndicator">
                                    <span class="stepDone">&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">Select which competition type you would like to register for.</span>
                                    <div style="margin: auto; width: 300px; margin-bottom: 20px;">
                                    I am registering 
                                    <asp:DropDownList ID="ddlCompetitionType" runat="server">
                                        <asp:ListItem Text="as an individual" Value="1" Selected="True" />
                                        <asp:ListItem Text="a team (I am the team captin)" Value="2" />
                                    </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="stepActions">
                                    <button id="bNext1">Next</button>
                                </div>                                
                            </div>
                            

                            <div id="step2" class="stepByStep" style="position:relative; min-height: 300px;">
                                <h3 class="grad-FFF-EEE">Step 2</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(0);"><span class="stepDone">&nbsp;</span></a>
                                    <span class="stepDone">&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">Select the Affiliate that you belong to, start your own<span class="stepActions">, or <a href="javascript: Aqufit.Page.Actions.LoadStep(3);" style="text-decoration: underline; font-weight: bold;">Skip</a> this step</span>.</span>                        
                                    <div style="text-align:center;">
                                        <h3>My Affiliate is: </h3><h3 style="color: #3A95CD; font-size: 26px;" id="affiliateName"><asp:Literal ID="litAffiliateName" runat="server" /></h3>
                                        <button id="bNotMyAffiliate" runat="server" visible="false">Change my Affiliate</button>
                                    </div>
                                    
                                    <div id="panelFindAffiliate">
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
                                    </div>
                                    <br />                                  
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>
                                    <button id="bgoNext2">Next</button>
                                </div>    
                            </div>

                            <div id="step3" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 3</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(2);"><span class="stepDone">&nbsp;</span></a>                                    
                                    <span class="stepDone">&nbsp;</span>      
                                    <span>&nbsp;</span>       
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>                       
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">Complete all fileds in the form </span>
                                    <br />
                                    <div class="compreg grad-FFF-EEE rounded"> 
                                        <div class="individual">
                                            <ati:Slim ID="atiSlimControl" runat="server" ValidationGroupName="compreg" /> 
                                            <ati:BodyComp id="atiBodyComp" runat="server" ValidationGroupName="compreg" FitnessLevelVisible="false" WeightVisible="false" HeightVisible="false" />
                                            <fieldset>
                                            <dl>        
                                                <dt><asp:Label ID="lAddress" runat="server" controlname="txtAddress" Text="Mailing Address:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ValidationGroup="compreg" ControlToValidate="txtAddress" ErrorMessage="Address is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>

                                                <dt><asp:Label ID="lPhone" runat="server" controlname="txtPhone" Text="Phone:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="compreg" ControlToValidate="txtPhone" ErrorMessage="Phone number is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>
                                       
                                                <dt><asp:Label ID="lShirt" runat="server" controlname="ddlShirtSize" Text="T-Shirt Size:" /></dt>
                                                <dd>
                                                    <asp:DropDownList ID="ddlShirtSize" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="L" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <dt>Please list any medical conditions that might affect you during the competition or that medical personnel should know about: </dt>
                                                <dd>
                                                    <asp:TextBox ID="txtMedical" runat="server" TextMode="MultiLine" Rows="4" MaxLength="2048" CssClass="ui-corner-all ui-widget-content" width="300px" />
                                                </dd>

                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="compreg" CssClass="validateSumm" />
                                            </dl>
                                            </fieldset>
                                        </div>
                                        <div class="team">
                                            <fieldset>
                                            <dl>
                                                <dt><asp:Label ID="lTeamName" runat="server" controlname="txtTeamName" Text="Team Name:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="compregteam" ControlToValidate="txtTeamName" ErrorMessage="Address is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>
                                                    
                                                <dt><asp:Label ID="lTeamAddress" runat="server" controlname="txtTeamAddress" Text="Mailing Address:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamAddress" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="compregteam" ControlToValidate="txtTeamAddress" ErrorMessage="Address is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>

                                                <dt><asp:Label ID="lTeamPhone" runat="server" controlname="txtTeamPhone" Text="Phone:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamPhone" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="compregteam" ControlToValidate="txtTeamPhone" ErrorMessage="Phone number is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>

                                                <dt><asp:Label ID="lTeamEmail" runat="server" controlname="txtTeamEmail" Text="Email:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamEmail" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="compregteam" ControlToValidate="txtTeamEmail" ErrorMessage="Email number is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationGroup="compregteam" ControlToValidate="txtTeamEmail" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
        
                                                </dd>
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="compregteam" CssClass="validateSumm" />
                                            </dl>
                                            </fieldset>
                                        </div>
                                    </div>  
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>
                                    <button id="bNext3">Next</button>
                                </div>    
                            </div>


                            <div id="step4" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 4</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(2);"><span class="stepDone">&nbsp;</span></a>  
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(3);"><span class="stepDone">&nbsp;</span></a>                                      
                                    <span class="stepDone">&nbsp;</span>      
                                    <span>&nbsp;</span>
                                    <span>&nbsp;</span>                       
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">
                                        <span class="individual">
                                        Emergency Contact Info.  Complete all fileds in the form 
                                        </span>
                                    </span>
                                    <br />
                                    <div class="compreg grad-FFF-EEE rounded"> 
                                        <div class="individual">
                                            <span><h3>Emergency Contact Info</h3></span> 
                                            <fieldset>
                                            <dl>        
                                                <dt><asp:Label ID="lEName" runat="server" controlname="txtEmergName" Text="Name:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtEmergName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="compregEmerg" ControlToValidate="txtEmergName" ErrorMessage="Name is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>

                                                <dt><asp:Label ID="lEPhone" runat="server" controlname="txtEmergPhone" Text="Phone:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtEmergPhone" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="compregEmerg" ControlToValidate="txtEmergPhone" ErrorMessage="Phone number is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>
                                       
                                                <dt><asp:Label ID="lERelation" runat="server" controlname="txtEmergRelation" Text="Relationship to Athlete:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtEmergRelation" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="256" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="compregEmerg" ControlToValidate="txtEmergRelation" ErrorMessage="Relationship number is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
                                                </dd>

                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="compregEmerg" CssClass="validateSumm" />
                                            </dl>
                                            </fieldset>
                                        </div>
                                        <div class="team">
                                            <h3>Each team MUST have 2 Male and 2 Female Athletes.</h3>
                                            <fieldset>
                                            <dl>
                                                <dt><asp:Label ID="lTeamCaptin" runat="server" controlname="txtTeamCaptinName" Text="Team Captain:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamCaptinName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" Enabled="false" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamCaptinName" ErrorMessage="Name is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:TextBox ID="txtTeamCaptinEmail" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" Enabled="false" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamCaptinEmail" ErrorMessage="Email is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamCaptinEmail" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />        
                                                    <asp:DropDownList ID="ddlTShirtCaptain" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="T-Shirt (L)" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <dt><asp:Label ID="lTeamMember2" runat="server" controlname="txtTeamMemeber2Name" Text="Athlete 2:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamMemeber2Name" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber2Name" ErrorMessage="Name is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:TextBox ID="txtTeamMemeber2Email" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber2Email" ErrorMessage="Email is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber2Email" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />        
                                                    <asp:DropDownList ID="gglTShirtTeamMember2" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="T-Shirt (L)" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <dt><asp:Label ID="lTeamMember3" runat="server" controlname="txtTeamMemeber3Name" Text="Athlete 3:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamMemeber3Name" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber3Name" ErrorMessage="Name is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:TextBox ID="txtTeamMemeber3Email" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber3Email" ErrorMessage="Email is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber3Email" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />        
                                                    <asp:DropDownList ID="ddlTShirtTeamMember3" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="T-Shirt (L)" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <dt><asp:Label ID="lTeamMember4" runat="server" controlname="txtTeamMemeber4Name" Text="Athlete 4:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamMemeber4Name" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber4Name" ErrorMessage="Name is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:TextBox ID="txtTeamMemeber4Email" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber4Email" ErrorMessage="Email is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" /> 
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationGroup="compregMembers" ControlToValidate="txtTeamMemeber4Email" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />        
                                                    <asp:DropDownList ID="ddlTShirtTeamMember4" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="T-Shirt (L)" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <dt><asp:Label ID="lAlternate1" runat="server" controlname="txtTeamAlternate1Name" Text="Alternate Male Athlete:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamAlternate1Name" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:TextBox ID="txtTeamAlternate1Email" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:DropDownList ID="ddlTShirtTeamAlternate1" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="T-Shirt (L)" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <dt><asp:Label ID="lAlternate2" runat="server" controlname="txtTeamAlternate2Name" Text="Alternate Female Athlete:" /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamAlternate2Name" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:TextBox ID="txtTeamAlternate2Email" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" />
                                                    <asp:DropDownList ID="ddlTShirtTeamAlternate2" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox">
                                                        <asp:ListItem Text="S" Value="S" />
                                                        <asp:ListItem Text="M" Value="M" />
                                                        <asp:ListItem Text="T-Shirt (L)" Value="L" Selected="True" />
                                                        <asp:ListItem Text="XL" Value="XL" />
                                                    </asp:DropDownList>
                                                </dd>

                                                <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="compregMembers" CssClass="validateSumm" />
                                            </dl>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>
                                    <button id="bNext4">Next</button>
                                </div>    
                            </div>


                            <div id="step5" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 5</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(2);"><span class="stepDone">&nbsp;</span></a>  
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(3);"><span class="stepDone">&nbsp;</span></a> 
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(4);"><span class="stepDone">&nbsp;</span></a>                                         
                                    <span class="stepDone">&nbsp;</span>      
                                    <span>&nbsp;</span>                                                 
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">
                                        <span class="individual">
                                        Athlete Profile information.  Add as much detail as possible.
                                        </span>
                                    </span>
                                    <br />
                                    <div class="compreg grad-FFF-EEE rounded">   
                                        <div class="individual">
                                            <fieldset>
                                            <dl>        
                                                <dt><asp:Label ID="lFran" runat="server" controlname="atiFranTime" Text="Fran:" /></dt>
                                                <dd>
                                                    <ati:TimeSpan id="atiTimeSpanFran" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ShowPace="false" ShowHour="false" />
                                                    <asp:HiddenField ID="hiddenFranId" runat="server" />
                                                </dd>

                                                <dt><asp:Label ID="lHelen" runat="server" controlname="atiHelenTime" Text="Helen:" /></dt>
                                                <dd>
                                                    <ati:TimeSpan id="atiTimeSpanHelen" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ShowPace="false" ShowHour="false" />
                                                    <asp:HiddenField ID="hiddenHelenKey" runat="server" />
                                                </dd>

                                                <dt><asp:Label ID="lFF" runat="server" controlname="atiTimeSpanFilthyFifty" Text="Filthy Fifty:" /></dt>
                                                <dd>
                                                    <ati:TimeSpan id="atiTimeSpanFilthyFifty" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ShowPace="false" ShowHour="false" />
                                                    <asp:HiddenField ID="hiddenFilthyFiftyKey" runat="server" />
                                                </dd>

                                                <dt><asp:Label ID="lGrace" runat="server" controlname="atiTimeSpanGrace" Text="Grace:" /></dt>
                                                <dd>
                                                    <ati:TimeSpan id="atiTimeSpanGrace" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" ShowPace="false" ShowHour="false" />
                                                    <asp:HiddenField ID="hiddenGraceKey" runat="server" />
                                                </dd>

                                                <dt><asp:Label ID="lFGB" runat="server" controlname="txtFGB" Text="Fight Gone Bad:" /></dt>
                                                <dd>
                                                    <asp:TextBox id="txtFGB" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" />
                                                    <asp:HiddenField ID="hiddenFGBKey" runat="server" />
                                                </dd>

                                                <dt><asp:Label ID="lMaxPullups" runat="server" controlname="txtMaxPullups" Text="Max Pull-ups:" /></dt>
                                                <dd>
                                                    <asp:TextBox id="txtMaxPullups" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" />
                                                    <asp:HiddenField ID="hiddenMaxPullupKey" runat="server" />
                                                </dd>

                                                <dt><asp:Label ID="lMaxBS" runat="server" controlname="txtMaxBS" Text="Max Back Squat:" /></dt>
                                                <dd>
                                                    <asp:TextBox id="txtMaxBS" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" /> 
                                                    <ati:UnitControl id="atiMaxBSUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" /> 
                                                    <asp:HiddenField ID="hiddenMaxBackSquatKey" runat="server" />
                                                </dd>
                                        
                                                <dt><asp:Label ID="lMaxClean" runat="server" controlname="txtMaxClean" Text="Max Clean:" /></dt>
                                                <dd>
                                                    <asp:TextBox id="txtMaxClean" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" /> 
                                                    <ati:UnitControl id="atiMaxCleanUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" /> 
                                                    <asp:HiddenField ID="hiddenMaxCleanKey" runat="server" />
                                                </dd> 
                                        
                                                <dt><asp:Label ID="lMaxDeadlift" runat="server" controlname="txtMaxDeadlift" Text="Max Deadlift:" /></dt>
                                                <dd>
                                                    <asp:TextBox id="txtMaxDeadlift" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" /> 
                                                    <ati:UnitControl id="atiMaxDeadliftUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" /> 
                                                    <asp:HiddenField ID="hiddenMaxDeadliftKey" runat="server" />
                                                </dd>
                                        
                                                <dt><asp:Label ID="lMaxSnatch" runat="server" controlname="txtMaxSnatch" Text="Max Snatch:" /></dt>
                                                <dd>
                                                    <asp:TextBox id="txtMaxSnatch" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" /> 
                                                    <ati:UnitControl id="atiMaxSnatchUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" /> 
                                                    <asp:HiddenField ID="hiddenMaxSnatchKey" runat="server" />
                                                </dd>                                                                                            
                                            </dl>
                                            </fieldset>
                                        </div>
                                        <div class="team">
                                            <fieldset>
                                            <dl>
                                                <dt><asp:Label ID="lTeamDescription" runat="server" controlname="txtTeamDescription" Text="Team Profile Info: Please tell us about your team." /></dt>
                                                <dd>
                                                    <asp:TextBox ID="txtTeamDescription" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" TextMode="MultiLine" Rows="12" Width="400px" />
                                                </dd>
                                            </dl>
                                            </fieldset>                                        
                                        </div>
                                    </div>                                   
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>
                                    <button id="bSave">Save</button>
                                </div>    
                            </div>


                            <div id="step6" class="stepByStep">
                                <h3 class="grad-FFF-EEE">Step 6</h3>
                                <div class="stepIndicator">
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(1);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(2);"><span class="stepDone">&nbsp;</span></a>  
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(3);"><span class="stepDone">&nbsp;</span></a> 
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(4);"><span class="stepDone">&nbsp;</span></a>
                                    <a href="javascript: Aqufit.Page.Actions.LoadStep(5);"><span class="stepDone">&nbsp;</span></a>                                         
                                    <span class="stepDone">&nbsp;</span>                                                                                   
                                </div>
                                <div class="introWrap">
                                    <span class="intoContent">Make payment.</span>
                                    <br />                                    
                                    <div class="compreg grad-FFF-EEE rounded"> 
                                        <h3>Your registration has been saved.  The last step is to make the payment.</h3>
                                        <center>
                                        <a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=9T6GYB6GPW9NC"><img src="https://www.paypalobjects.com/en_US/i/btn/btn_buynowCC_LG.gif" border="0" alt="PayPal - The safer, easier way to pay online!"></a>
                                        <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1">
                                        </center>
                                    </div>
                                </div>
                                <div class="stepActions">
                                    <button class="bBack">Back</button>                                   
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
         
    
               



