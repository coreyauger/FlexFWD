<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ProfileLinks.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ProfileLinks" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<script type="text/javascript">

    Aqufit.Page.Action = {        
        SwitchPageTabs: function (tab) {
            if (Aqufit.Page.Tabs && Aqufit.Page.Tabs.SwitchToLogWorkoutTab) {
                Aqufit.Page.Tabs.SwitchToLogWorkoutTab();
            } else {
                top.location.href = '<%=ResolveUrl("~/") %>' + Aqufit.Page.UserName + '?w=0';
            }
        }
    };

    Aqufit.Windows.WatchList = {
        win: null,
        open: function (arg) {
            var uname = Aqufit.Page.GroupUserName != '' ?  Aqufit.Page.GroupUserName : Aqufit.Page.ProfileUserName;
            Aqufit.Windows.WatchList.win = window.radopen('<%=ResolveUrl("~/Profile/ComparePallet") %>?u=' + uname, null);
            Aqufit.Windows.WatchList.win.set_modal(true);
            Aqufit.Windows.WatchList.win.setSize(585, 550);
            Aqufit.Windows.WatchList.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            Aqufit.Windows.WatchList.win.center();
            Aqufit.Windows.WatchList.win.show();
            return false;
        },
        close: function () {
            Aqufit.Windows.WatchList.win.close();
        }
    };


    $(function () {
        $('#bFindFriends').button().click(function (event) {
            top.location.href = '<%=ResolveUrl("~/Profile/FindInvite") %>';
            event.stopPropagation();
            return false;
        });
        $('#<%=divExtraLinks.ClientID %> div#atiMoreActions').click(function (event) {
            var $ul = $('ul#atiMoreActionList');
            if ($ul.is(':visible')) {
                $ul.slideUp('fast');
                $(this).find('span').html('+');
            } else {
                $ul.slideDown('fast');
                $(this).find('span').html('-');
            }
        });
    });
</script>
</telerik:RadCodeBlock>

<div id="divMainLinks" runat="server" style="margin-top: 10px;">                            
    <ul class="linksList">
        <li runat="server" id="liEdit"><a runat="server" id="bEditProfile"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iEdit_s.png")%>" /> <asp:Literal ID="litEditProfile" runat="server" Text="Edit Profile" /></a></li>
        <li runat="server" id="liEdit2" style="border-bottom: 0;">&nbsp;</li>
        <li runat="server" id="liGettingStarted"><a href="" id="aGettingStarted" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iGettingStarted.png")%>" /> Getting Started</a></li>
        <li runat="server" id="liFriends"><button id="bFindFriends" style="width: 100%;">Connect <img align="absmiddle" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMailServices.png")%>" /></button></li> 
        <li runat="server" id="liStats"><a href="" id="hlStats" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iStats_s.png")%>" /> <asp:Literal ID="litStats" runat="server" Text="Athlete Stats" /></a></li> 
        <li runat="server" id="liCreateWorkout" visible="false"><a href="javascript: ;" onclick="Aqufit.Page.Tabs.SwitchTab(1);"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iWorkout_s.png")%>" /> Create Group Workout</a></li>     
        <li runat="server" id="liWorkout"><a href="javascript: ;" onclick="Aqufit.Page.Action.SwitchPageTabs(1);"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iWorkout_s.png")%>" /> Log Workout</a></li> 
        <li runat="server" id="liWorkoutHistory"><a id="hlWorkouts" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iHistory_s.png")%>" /> Workout History</a></li>                  
        <li runat="server" id="liMessages"><a id="hlMessages" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMail_s.png")%>" /> Send Message</a></li>   
        <li runat="server" id="liPhotos"><a runat="server" id="hlViewPhotos"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCamera_s.png")%>" /> View Photos</a></li>
        <li runat="server" id="liMembers"><a id="hlMembers" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFriend_s.png")%>" /> Members</a></li> 
        <li runat="server" id="liSpace" visible="false" style="border-bottom: 0;">&nbsp;</li>
        <li runat="server" id="liAddBio" visible="false"><a runat="server" id="hlAddBio"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iAdd_s.png")%>" /> Add Info</a></li>
    </ul>                            
</div>
<div style="margin-top: 20px;" id="divExtraLinks" runat="server">  
    <div id="atiMoreActions">More Actions <span style="float: right;">+</span></div>                          
    <ul class="linksList" id="atiMoreActionList" style="display:none;">
        <li runat="server" id="liMyFriend"><a id="hlMyFriends" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFriend_s.png")%>" /> <asp:Literal ID="lMyFriends" runat="server" Text="Friends" /></a></li> 
        <li runat="server" id="liGroups"><a id="hlGroups" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iGroup_s.png")%>" /> Groups</a></li>
        <li runat="server" id="liInbox"><a id="hlInbox" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iMail_s.png")%>" /> Inbox</a></li>        
        <li runat="server" id="liRoutes"><a ID="hlRoutes" runat="server" ><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iRoutes.png")%>" /> My Routes</a></li>    
        <li runat="server" id="liWorkouts"><a ID="hlMyWorkouts" runat="server"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iWorkout_s.png")%>" /> My Workouts</a></li>                           
    </ul>
</div>


