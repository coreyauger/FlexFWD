<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_Profile.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_Profile" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendsPhotos" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendsPhotos.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinksList" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinksList.ascx" %>
<%@ Register TagPrefix="ati" TagName="NameValueGrid" Src="~/DesktopModules/ATI_Base/controls/ATI_NameValueGrid.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileLinks" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileLinks.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileSuggest" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileSuggest.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<script type="text/javascript">

   
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


    Aqufit.Page.Controls.ATI_Profile = function (id) {
        this.id = id;
        this.init();
        this.friendRequestCallback = null;
    };

    Aqufit.Page.Controls.ATI_Profile.prototype = {
        init: function () {
            $('#<%=bAddTo.ClientID %>').click(function (event) {
                if (Aqufit.Page.UserSettingsId > 0) {
                    Aqufit.Windows.FollowAthleteModal.open();
                } else {
                    self.location.href = '<%=ResolveUrl("~/Login") %>?returnUrl=' + self.location.href;
                }
                event.stopPropagation();
                return false;
            });  
        },
        ShowOk: function(msg){
            $('#<%=divAddTo.ClientID %>').hide();
            if(Aqufit.Windows.FollowAthleteModal){
                Aqufit.Windows.FollowAthleteModal.close();
            }
            radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF; font-weight: bold;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png") %>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Success');        
        },
    };

    $(function () {
          Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_Profile('<%=this.ID %>');  
    });
</script>
</telerik:RadCodeBlock>

<div id="bCompareTo" runat="server" class="boldButton">
    <a href="javascript: ;" runat="server" id="aCompareTo" style="display: block; text-align: center; color: #fff; font-weight: bold;" >Compare Stats</a>
</div>
<asp:Panel ID="panelProfileImage" runat="server" CssClass="panelProfileImage">           
    <ati:ProfileImage ID="atiProfileImage" runat="server" Width="192px" />
</asp:Panel>     
<div id="divAddTo" runat="server" style="margin-top: 10px;">   
    <div id="bAddTo" runat="server" class="boldButton" style="margin-left: 18px; position: relative; margin-top: 10px;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/bFollow.png") %>" style="position: relative; margin-left: -25px; margin-top: -18px; margin-bottom:-12px;" />&nbsp;</div>                       
    <a href="javascript:Aqufit.Page.Actions.SendFriendRequest();" visible="false" runat="server" id="hlSendFriendRequest" style="display: block; text-align: center;" >Send Friend Request</a>    
</div>
<asp:Literal ID="litMainGroup" runat="server" />
<ati:ProfileLinks ID="atiProfileLinks" runat="server" />
<p><br /><br /></p> 
<ati:ProfileSuggest ID="atiProfileSuggest" runat="server" />
<div class="atiSideContainer" id="pWebLinks" runat="server">
<span>Web Links</span>                         
    <ati:WebLinksList id="atiWebLinksList" runat="server" />                         
</div>                                          
<div class="atiSideContainer" style="overflow: hidden;" id="pFriends" runat="server">
<asp:Literal ID="lAthleteTerm" runat="server" />
    <ati:FriendsPhotos id="atiFriendsPhotos" runat="server" FriendTerm="Athlete" FriendTermPlural="Athletes" />
</div>  
<div class="atiSideContainer" id="pAchievements" runat="server">
    <span><asp:Literal ID="litAchievements" runat="server" /></span>                 
    <ati:NameValueGrid ID="nvgCrossfit" runat="server" Cols="1" CssClass="nvGrid" />    
        <a id="hlUserStats" runat="server" style="position:relative; left: 125px; top: 5px; font-weight: bold;">more ...</a>
</div>   
<div class="atiSideContainer" id="pBodyComp" runat="server" visible="false">
<span>Body Composition</span>                         
    <ati:NameValueGrid ID="nvgBodyComp" runat="server" Cols="1" CssClass="nvGrid" />                        
</div>
<div class="atiSideContainer" id="pBio" runat="server" visible="false">
<span>Bio</span>                         
    <asp:Literal ID="litBio" runat="server" />                     
</div> 
<div class="atiSideContainer" id="pTrainingHistory" runat="server" visible="false">
<span>Bio</span>                         
    <asp:Literal ID="litTrainingHistory" runat="server" />                     
</div>

