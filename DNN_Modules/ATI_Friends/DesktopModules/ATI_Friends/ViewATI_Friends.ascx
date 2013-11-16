<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Friends.ViewATI_Friends" CodeFile="ViewATI_Friends.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ContactInviteScript" Src="~/DesktopModules/ATI_Base/controls/ATI_ContactInviteScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinksList" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinksList.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<link href="<%=ResolveUrl("~/DesktopModules/ATI_Friends/resources/css/friends.css")%>" type="text/css" rel="stylesheet" />
<script language="javascript" type="text/javascript">     

    Aqufit.Windows.ContactsWin = {
        win: null,
        open: function (uri) {
            alert(uri);
            this.win = window.radopen(uri, null);
            this.win.set_modal(true);
            this.win.setSize(747, 600);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };

    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        OnClientSelectedIndexChangedEventHandler: function (sender, args) {
            var item = args.get_item();
            if (item.get_value() != '') {    // User has selected the "add new map"                
                top.location.href = Aqufit.Page.PageBase + item.get_value();
            }
        }
    };

    $(function () {
        $('#atiStatusWidget').hide();
        $('#tabs').tabs();      
                
        $('.dull').focus(function(){
            $(this).valueOf('').removeClass('dull');
        });       
    });

    Aqufit.addLoadEvent(function () {
        if (Aqufit.Page.atiFriendRequestScript) {
            Aqufit.Page.atiFriendRequestScript.sendFriendRequest = function (fid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('friendRequest');
                $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Friends","ATI_Friends") %>', '');
            }
        }
        if (Aqufit.Page.atiFriendListScript) {
            // the following is the case when viewing other peoples friends that you want to make a request to...
            Aqufit.Page.atiFriendListScript.sendFriendRequest = function (fid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('friendRequest');
                $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Friends","ATI_Friends") %>', '');
            }
            Aqufit.Page.atiFriendListScript.onDataNeeded = function (skip, take) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('friendListDataPage');
                $('#<%=hiddenAjaxValue.ClientID %>').val(skip);
                $('#<%=hiddenAjaxValue2.ClientID %>').val(take);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Friends","ATI_Friends") %>', '');
            }
        }
        if (Aqufit.Page.atiFollowingList) {
            Aqufit.Page.atiFollowingList.onDataNeeded = function (skip, take) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('followingListDataPage');
                $('#<%=hiddenAjaxValue.ClientID %>').val(skip);
                $('#<%=hiddenAjaxValue2.ClientID %>').val(take);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Friends","ATI_Friends") %>', '');
            }
        }
        if (Aqufit.Page.atiFollowerList) {
            Aqufit.Page.atiFollowerList.onDataNeeded = function (skip, take) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('followerListDataPage');
                $('#<%=hiddenAjaxValue.ClientID %>').val(skip);
                $('#<%=hiddenAjaxValue2.ClientID %>').val(take);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Friends","ATI_Friends") %>', '');
            }
        }
    });
    
</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>          
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>    
        </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   

 <asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue2" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<div id="atiStatusWidget" class="ui-widget">
	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		<span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<span id="statusMsg"></span>
	</div>
</div>


    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" runat="server" class="floatRight" style="width: 723px; min-height: 501px;">               
        <!-- Tabs -->
        <div style="float:left; width: 550px;" id="divControl" runat="server">
        <div id="tabs">
    		<ul>
    			<li id="tabFriends" runat="server"><a href="#<%=pageViewFriends.ClientID %>"><asp:Literal ID="litFriendTerm" runat="server" Text="Friends" /></a></li>
                <li id="tabFollowing" runat="server"><a href="#<%=pageViewFollowing.ClientID %>">Following</a></li>
    			<li id="tabFollowers" runat="server"><a href="#<%=pageViewFollowers.ClientID %>">Followers</a></li>        		                
            </ul>    			                                
                
            <!-- TAB Friends -->
            <div id="pageViewFriends" runat="server" style="padding: 0px; background-color: White;">                                                                                                        
                <asp:Panel ID="atiFriendPanel" runat="server">
                    <div class="atiSearchControls grad-FFF-EEE">
                        <telerik:RadComboBox ID="atiRadComboBoxSearchFriends" runat="server" Width="100%" Height="140px"
                            EmptyMessage="Search friends" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                            EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchFriends_ItemsRequested"
                            OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                        </telerik:RadComboBox>
                    </div>

                    <asp:Panel ID="atiNoFriends" runat="server" Visible="false">
                        <p>You have not connected with any friends Yet.  Click on '<a href="javascript: void(0);" onclick="Aqufit.Page.Tabs.SwitchTab(1);">Find Friends</a>', to search or locate people through your email contacts.</p>
                    </asp:Panel>                      
                    <ati:FriendListScript id="atiFriendListScript" runat="server" ControlMode="FRIEND_LIST" />    
                </asp:Panel> 
            </div>

            <!-- TAB Following -->
            <div id="pageViewFollowing" runat="server" style="padding: 0px; background-color: White;">          
                <div class="atiSearchControls grad-FFF-EEE">                                                                                             
                    <telerik:RadComboBox ID="atiRadComboBoxSearchFollowing" runat="server" Width="100%" Height="140px"
                            EmptyMessage="Search people you follow" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                            EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchFollowing_ItemsRequested"
                            OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                    </telerik:RadComboBox>
                </div>                                      
                <ati:FriendListScript id="atiFollowingList" runat="server" ControlMode="FOLLOWING_LIST" />                                                                                                         
            </div>

            <div id="pageViewFollowers" runat="server" style="padding: 0px; background-color: White;">                    
                <div class="atiSearchControls grad-FFF-EEE">
                    <telerik:RadComboBox ID="atiRadComboBoxSearchFollower" runat="server" Width="100%" Height="140px"
                            EmptyMessage="Search people that are following" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                            EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchFollower_ItemsRequested"
                            OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                    </telerik:RadComboBox>
                </div>
                    
                <ati:FriendListScript id="atiFollowerList" runat="server" ControlMode="FOLLOWING_LIST" /> 
            </div> 
                     			
    	</div>        
            <!-- END Tabs -->                                                       
        </div>

        <div style="float:right; width: 200px; padding-left: 10px; width: 160px;" id="divAd" runat="server">
            <a href="http://tastypaleo.com"><img id="imgAdRight" runat="server" /></a>
        </div>
        
    </div>
    <div id="divLeftNav" runat="server" style="width: 196px; float: left;">
        <ati:ProfileImage ID="atiProfileImage" runat="server" Small="false" />
        <asp:Panel ID="panelGroupInfo" runat="server" Visible="false">
            <asp:Literal ID="litGroupName" runat="server" />
            <ati:WebLinksList id="atiWebLinksList" runat="server" />   
        </asp:Panel>
    </div>
    <div style="clear:both;"></div>     
               










    

    

         
    
               



