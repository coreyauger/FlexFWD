<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Groups.ViewATI_Groups" CodeFile="ViewATI_Groups.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_Profile.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendsPhotos" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendsPhotos.ascx" %>
<%@ Register TagPrefix="ati" TagName="Comment" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamComment.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutScheduler" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutScheduler.ascx" %>
<%@ Register TagPrefix="ati" TagName="StreamScript" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinksList" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinksList.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="LeaderBoard2" Src="~/DesktopModules/ATI_Base/controls/ATI_LeaderBoard2.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="GMap" Src="~/DesktopModules/ATI_Base/controls/ATI_GMap.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutHighChart" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutHighChart.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
html
{
    background-color: #cdcdcd; 
    background-image:url(<%=this.BackgroundImageUrl%>);
    background-repeat:no-repeat;
    background-attachment:fixed;
    <%=this.ProfileCSS%>
}

html body
{
    background-color: #cdcdcd; 
    background-image:url(<%=this.BackgroundImageUrl%>);
    background-repeat:no-repeat;
    background-attachment:fixed;
    <%=this.ProfileCSS%>
}
.atiMainFeed div.atiStreamItemRight
{
	width: 460px;
}

.groupFeature h2 a
{
	font-size: 16px;	
	color: #faac45;
}

.groupFeature a
{	
	color: #4bb3dc;
}

</style>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnGroupResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting> 
        <telerik:AjaxSetting AjaxControlID="panelAjax2">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax2"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>           
    </AjaxSettings>        
</telerik:RadAjaxManager>

<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" /> 
<ati:LoadingPanel ID="atiLoading" runat="server" />

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" />

<asp:Panel ID="atiGroupProfile" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">  
    <link id="aqufitStream" href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/aqufitStream.css")%>" type="text/css" rel="stylesheet">
   
    <script type="text/javascript" >

        Aqufit.Page.Tabs = {
            SwitchTab: function (ind) {
                $('#tabs').tabs('select', ind);
            }
        };

        Aqufit.Page.Actions = {           
            ShowFail: function (msg) {
                radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
            },
            ShowSuccess: function (msg) {
                radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Success');
            }
        };

        function OnGroupResponseEnd(sender, eventArgs) {
            Aqufit.Page.atiLoading.remove();           
        }

        $(function () {
            $('#atiStatusWidget').hide();
            $('#tabs').tabs();
            $('#<%=bJoinGroup.ClientID %>').button();
            $('#bSubmitWorkoutAjax').click(function (event) {
                Aqufit.Page.atiLoading.addLoadingOverlay('<%=pageScheduleWOD.ClientID %>');
                $('#<%=hiddenAjaxAction.ClientID %>').val('scheduleWOD');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Groups","ATI_Groups") %>', '');
                event.stopPropagation();
                return false;
            });
            $('#bSchedule').button().click(function (event) {
                top.location.href = Aqufit.Page.PageBase + Aqufit.Page.GroupUserName + '/workout-history';
                event.stopPropagation();
                return false;
            });
        });

        Aqufit.addLoadEvent(function () {
            Aqufit.Page.atiStreamScript.getStreamData(3, '');
            Aqufit.Page.atiStreamScript.streamDeleteCallback = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delStream');
                $('#<%=hiddenAjaxValue.ClientID %>').val('' + id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Groups","ATI_Groups") %>', '');
            };
            Aqufit.Page.atiStreamScript.onDeleteComment = function (id) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('delComment');
                $('#<%=hiddenAjaxValue.ClientID %>').val(id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Groups","ATI_Groups") %>', '');
            };
        });
      
    </script>    
    </telerik:RadCodeBlock>
      
    <asp:Panel ID="panelAjax" runat="server" >
        <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
        <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
        <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
    </asp:Panel>
    <div style="position: relative;">
        <div id="ati_sidePanel" style="height: 100%; float: left;">        
            <!-- ** Left Panel Zone -->     
           <div id="divAddTo" style="position: relative;">           
               <asp:Button ID="bJoinGroup" runat="server" OnClick="bJoinGroup_Click" CssClass="fullwidth" Text="Join Group" style="margin-bottom: 9px;" />                     
           </div>
           <ati:Profile ID="atiProfile" runat="server" />                                                                      
            <!-- ** END Left Panel Zone -->    
        </div>
        <div style="width: 728px; float: right;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr valign="top">
                <td colspan="2" style="padding-bottom: 7px;">           
                    <div class="profileHeading grad-FFF-EEE ui-corner-all" style="vertical-align: middle;">
                        <asp:Literal ID="litGroupName" runat="server" /> 
                        <div style="position: absolute; top: 4px; right: 150px; width: 200px;"><ati:WebLinksList id="atiWebLinksList" runat="server" IsOwner="false" HidePersonalLink="true" /></div>
                        <ul class="hlist">                         
                            <li><a id="aLeaders" runat="server" title="View Leader Board"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iStats_s.png")%>" /></span></a></li>
                            <li><a id="aHistory" runat="server" title="Workout History"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iHistory_s.png")%>" /></span></a></li>
                            <li><a id="aMembers" runat="server" title="Member List"><span class="grad-FFF-EEE ui-corner-all"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFriend_s.png")%>" /></span></a></li>
                        </ul>
                    </div>                                         
                    <!-- Tabs -->
            		<div id="tabs">
            			<ul>
            				<li id="tabHistory" runat="server"><a href="#<%=pageViewHistory.ClientID %>">View History</a></li>
                            <li id="tabWorkout" runat="server" visible="false"><a href="#<%=pageScheduleWOD.ClientID %>">Schedule Workout</a></li>
            				<li id="tabComment" runat="server"><a href="#<%=pageViewComment.ClientID %>">Shout</a></li>
            			</ul>
            			<div id="pageViewHistory" runat="server" style="padding: 3px;">
                                <div style="width: 722px; height: 500px; text-align: center; overflow: auto; background-color: #ccc;">                           
                                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">                            
                                    <asp:Literal ID="litTodaysWOD" runat="server" />
                                    <button id="bSchedule" style="position: absolute; top: 5px; right: 0px;">View Schedule</button>
                                </div>
                                
                                <ati:WorkoutHighChart ID="atiWorkoutHighChart" runat="server" Width="100%" Height="200px" />
                                <ati:LeaderBoard2 ID="atiLeaderBoard" runat="server" Visible="false" NoPics="true" />  
                           </div>
                        </div>
            			
                        <div id="pageScheduleWOD" runat="server" visible="false" class="grad-FFF-EEE">
                            <ati:WorkoutScheduler id="atiWorkoutScheduler" runat="server" OnWodItemsRequested="atiWorkout_WodItemsRequested" />
                            <button id="bSubmitWorkoutAjax" class="ati_Form_Button">Post</button>
                        </div>

            			<div id="pageViewComment" runat="server">
                            <asp:Panel ID="atiCommentPanel" runat="server">   
                                <ati:Comment ID="atiComment" runat="server" />   
                                <asp:Button ID="bSubmitComment" runat="server" Text="Post" OnClick="bSubmitComment_Click" CssClass="ati_Form_Button" />                             
                            </asp:Panel>
                        </div>
            		</div>
                    <!-- END Tabs -->                                                                                                                                      
                </td>   
            </tr>            
            <tr valign="top">
                <td style="width: 550px; padding-right: 16px;">                   
                    <!-- ** MAIN FEED Area -->
                    <asp:Panel ID="atiStreamPanelAjax" runat="server" CssClass="atiMainFeed">
                        <ati:StreamScript ID="atiStreamScript" runat="server" ShowTopPager="false" />                       
                    </asp:Panel>
                    <!-- ** END MAIN FEED Area -->
                </td>
                <td align="right">                    
                    <a href="http://tastypaleo.com"><img id="imgAdRight" runat="server" /></a>
                </td>
            </tr>
            </table>
       </div>
       <div style="clear:both;"></div> 
    </div>    
</asp:Panel>

<asp:Panel ID="atiGroupListPanel" runat="server" Visible="false">

    <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server"> 
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>

    <script type="text/javascript" >

        Aqufit.Page.Tabs = {
            SwitchTab: function (ind) {
                $('#tabs2').tabs('select', ind);
            }
        };

        Aqufit.Page.Actions = {
            OnClientSelectedIndexChangedEventHandler: function (sender, args) {
                var item = args.get_item();
                if (item.get_value() != '') {    // User has selected the "add new map"
                    var val = eval('(' + item.get_value() + ')');
                    self.location.href = Aqufit.Page.PageBase + 'group/' + val.UserName;
                }
            },
            SearchRoutes: function () {
                var address = $('#<%=atiRouteSearch.ClientID %>').val();
                Aqufit.Page.atiGMap.gotoAddress(address, null, function () {
                    // if we could not find a location ... do a postback for route names
                });
            },
            ShowFail: function (msg) {
                radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
            },
            ShowSuccess: function (msg) {
                radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
            }
        };
        function OnGroupResponseEnd(sender, eventArgs) { } // dummy function

        $(function () {
            $('#atiStatusWidget').hide();
            $('.dull').focus(function () {
                $(this).val('').removeClass('dull').unbind('focus');
            });
            $('#tabs2').tabs({
                show: function (event, ui) {
                    if (ui.panel.id == "pageFindGroups") {
                        Aqufit.Page.atiGMap.refresh();
                    }
                }
            }
            );
            $('#bCreateGroup').button().click(function (event) {
                self.location.href = '<%=ResolveUrl("~/Groups/GroupSettings")%>';
                event.stopPropagation();
                return false;
            });
            Aqufit.Page.atiGMap.onPlacesResponse = function (places) {
                Aqufit.Page.atiGroupSearch.generateStreamDom(places);                
            }
     //       Aqufit.Page.atiGMap.onMarkerClick = function (place) {
                //    Aqufit.Page.Actions.CenterPlace(place);
       //     }
        });
        Aqufit.addLoadEvent(function () {
            Aqufit.Page.atiGroupList.onDataNeeded = function (skip, take) {
                Aqufit.Page.atiLoading.addLoadingOverlay('panelActiveGroups');
                Affine.WebService.StreamService.getActiveGroups(skip, take, function (json) {
                    Aqufit.Page.atiGroupList.generateStreamDom(json);
                    Aqufit.Page.atiLoading.remove();
                }, function (err) { });
            }
        });

        
    </script>    
    <style type="text/css">    
    div.atiStreamItemRight
    {
	    width: 380px !important;
    }
    </style>
    </telerik:RadCodeBlock>

  
    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" style="width: 729px; float: right; ">
        <div id="divMain" style="width: 550px; float: left; margin-left: 8px; margin-right: 8px;">            
           <!-- TODO: a search box to search messages -->
            <!-- Tabs -->
    		<div id="tabs2">
    			<ul>
                    <li id="tabGroups"><a href="#pageGroups">Groups</a></li>
    				<li id="tabMyGroups" runat="server"><a href="#<%=pageMyGroups.ClientID %>">My Groups</a></li>                    
    				<li id="tabFindGroup"><a href="#pageFindGroups">Find by location</a></li>                                
                </ul>
                <button id="bCreateGroup" style="position: absolute; right: 6px; top: 6px;">New Group</button>                      			
                
                <div id="pageGroups" style="padding: 0px; background-color: White;"> 
                    <div class="atiSearchControls grad-FFF-EEE">
                        <telerik:RadComboBox ID="atiRadComboBoxSearchGroups2" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                EmptyMessage="Search Groups (eg: crossfit.com)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true"
                                OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                                <WebServiceSettings Method="GetGroupSearch" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
                        </telerik:RadComboBox>                        
                    </div>
                    <div class="groupFeature">
                        <div class="atiSearchControls grad-FFF-EEE" style=" position: relative;">                                                
                            <div style="width: 200px; min-height: 420px;">
                                <a id="hrefGroupLink2" runat="server"><ati:ProfileImage ID="atiFGProfileImg" runat="server" IsOwner="false" /></a>                            
                                <div style="margin-top: 5px; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc; background: #fff;">
                                    <div class="atiListHeading grad-FFF-EEE">
                                    Members
                                    </div>
                                    <ati:FriendsPhotos id="atiFriendsPhotos" runat="server" FriendTerm="Member" FriendTermPlural="Members" />
                                </div>
                            </div>
                            <div style="width: 300px; position: absolute; right: 10px; top: 0px;">
                                <h2><a id="hrefGroupName" runat="server"></a></h2>
                                <asp:Literal ID="litGroupDescription" runat="server" />
                                <div style="margin-top: 5px; border-left: 1px solid #ccc; border-bottom: 1px solid #ccc; border-right: 1px solid #ccc; background: #fff;">
                                    <div class="atiListHeading grad-FFF-EEE">
                                    Leader Board
                                    </div>
                                    <div style="overflow: scroll; height: 350px; background-color: #FFF; border: 1px solid #ccc;">
                                        <ati:LeaderBoard2 ID="atiLeaderBoard2" runat="server" Cols="1" />
                                    </div>
                                </div>
                            </div>
                            
                        </div>                        
                    </div>
                    <div id="panelActiveGroups">
                        <ati:FriendListScript id="atiGroupList" runat="server" ControlMode="GROUP_LIST" Title="Active Groups" />    
                    </div>
                </div>
                
                <div id="pageMyGroups" runat="server" style="padding: 0px; background-color: White;">                                                                                                        
                    <div class="atiSearchControls grad-FFF-EEE">
                        <telerik:RadComboBox ID="atiRadComboBoxSearchGroups" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                EmptyMessage="Search Groups (eg: crossfit.com)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true"
                                OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                                <WebServiceSettings Method="GetGroupSearch" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
                        </telerik:RadComboBox>                        
                    </div>
                    <asp:Panel ID="atiNoFriends" runat="server" Visible="false">
                        <p>You have not joined any groups yet.  Select the "Groups" tab or "Find by locations" tab to locate groups that intrest you.</p>
                    </asp:Panel>                      
                    <ati:FriendListScript id="atiMyGroupList" runat="server" ControlMode="GROUP_LIST"  />                                                                                                        
                </div>                


    			<div id="pageFindGroups" style="padding: 0px; background-color: White;">            
                    <div class="atiSearchControls grad-FFF-EEE">
                        <ul class="hlist">
                            <li><asp:TextBox ID="atiRouteSearch" runat="server" Width="450px"  CssClass="ui-corner-all ui-widget-content atiTxtBox dull" Text="search by address." /></li>
                            <li><a href="javascript: Aqufit.Page.Actions.SearchRoutes();" title="Search"><span class="grad-FFF-EEE rounded seachButton"><img id="imgSearch" runat="server" /></span></a></li>
                        </ul>
                    </div>
                    <ati:GMap id="atiGMap" runat="server" Mode="GROUP_FINDER" Height="300px" />
                    <ati:FriendListScript id="atiGroupSearch" runat="server" ControlMode="GROUP_LIST" Title="Groups located on map" />                  
                </div>            			
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; float: right;">
            <a href="http://tastypaleo.com"><img runat="server" id="imgAd" /></a>
        </div>
    
    </div>
    <div id="divLeftNav" style="width: 160px; float: left;">
       
       
                            
    </div>
    <div style="clear:both;"></div>  

</asp:Panel>        








    

    

         
    
               



