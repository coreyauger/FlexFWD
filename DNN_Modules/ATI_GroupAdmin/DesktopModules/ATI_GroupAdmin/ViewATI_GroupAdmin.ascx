<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_GroupAdmin.ViewATI_GroupAdmin" CodeFile="ViewATI_GroupAdmin.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="UserName" Src="~/DesktopModules/ATI_Base/controls/ATI_UserNameRegister.ascx" %>
<%@ Register TagPrefix="ati" TagName="Slim" Src="~/DesktopModules/ATI_Base/controls/ATI_SlimControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="FriendListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_FriendListScript.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="WebLinks" Src="~/DesktopModules/ATI_Base/controls/ATI_WebLinks.ascx" %>
<%@ Register TagPrefix="ati" TagName="Address" Src="~/DesktopModules/ATI_Base/controls/ATI_AddressControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutTypes" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutTypes.ascx" %>
<%@ Register TagPrefix="ati" TagName="ThemeEditor" Src="~/DesktopModules/ATI_Base/controls/ATI_ThemeEditor.ascx" %>
<%@ Register TagPrefix="ati" TagName="FindInvite" Src="~/DesktopModules/ATI_Base/controls/ATI_FindInvite.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<style type="text/css">
html
{
    background-color: #cdcdcd; 
    background-image:url(<%=this.BackgroundImageUrl%>);    
    <%=this.ProfileCSS%>
}

html body
{
    background-color: #cdcdcd; 
    background-image:url(<%=this.BackgroundImageUrl%>);    
    <%=this.ProfileCSS%>
}
div.innerFlat
{
	background-color: #FFF !important; 
}
.atiGroupSettins
{
	border: 1px solid #ccc;
	padding: 20px 50px;
}
.atiGroupSettins ul li
{
	list-style: none;
}

.atiGroupSettins dd
{
	margin-bottom: 10px;
}

div.groupCreateHelp h2
{
	color: #666;
}

div.groupCreateHelp h4
{
	margin-top: 15px;
	margin-bottom: 0px;
	color: #666;
	display: block;
	text-decoration: underline;
}


</style>

<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>

    <script type="text/javascript" >
        Aqufit.Page.Tabs = {
            SwitchTab: function (ind) {
                $('#tabs').tabs('select', ind);
            }       
        };        

        Aqufit.Windows.ErrorDialog = {
            win: null,
            open: function (json) {
                var oJson = eval('(' + json + ')');
                $('#errorStatusDiv').empty().html(oJson['html']);
                Aqufit.Windows.ErrorDialog.win = $find('<%=ErrorDialog.ClientID %>');
                Aqufit.Windows.ErrorDialog.win.show();
            },
            close: function () {
                Aqufit.Windows.ErrorDialog.win.close();
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
        }

        Aqufit.Page.Actions = {
            adminAddSuccess: function () {
            },
            SendGroupInvite: function (fid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('sendGroupInvite');
                $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');
            }
        };

        $(function () {
            $('#tabs').tabs({
                select: function (event, ui) {
                    if (ui.index == 4) {    // invite tab was clicked..
                        $('#<%=hiddenAjaxAction.ClientID %>').val('friendData');
                        $('#<%=hiddenAjaxValue.ClientID %>').val(0);    // skip
                        $('#<%=hiddenAjaxValue2.ClientID %>').val(25);  // take                   
                        __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');
                    }
                }
            });
            $('.dull').focus(function () {
                $(this).val('').removeClass('dull').unbind('focus');
            });
            // $('#tabs').tabs("option", "disabled", [1, 2, 3, 4]);
        });


        Aqufit.addLoadEvent(function () {
            //Aqufit.Page.atiMemberRequestScript.sendFriendRequest = Aqufit.Page.Actions.SendFriendRequest; 
            Aqufit.Page.atiMemberList.onDataNeeded = function (skip, take) {
                Affine.WebService.StreamService.getMemberListDataOfRelationship($('#<%=hiddenGroupKey.ClientID %>').val(), Aqufit.Relationship.GROUP_MEMBER, skip, take, function (json) {
                    Aqufit.Page.atiMemberList.generateStreamDom(json);
                    //Aqufit.Page.atiLoading.remove();
                }, function (err) { });
            };
            Aqufit.Page.atiMemberList.onMakeMemberAdmin = function (mid) {
                if (confirm("Are you sure you want to make this member an Admin?")) {
                    $('#<%=hiddenAjaxAction.ClientID %>').val('makeAdmin');
                    $('#<%=hiddenAjaxValue.ClientID %>').val(mid);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');
                }
            };
            Aqufit.Page.atiMemberList.onRemoveMember = function (mid) {
                if (confirm("Are you sure you want to remove this member from the group?")) {
                    $('#<%=hiddenAjaxAction.ClientID %>').val('removeMember');
                    $('#<%=hiddenAjaxValue.ClientID %>').val(mid);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');
                }
            };
            Aqufit.Page.atiMemberListAdmin.onDataNeeded = function (skip, take) {
                Affine.WebService.StreamService.getMemberListDataOfRelationship($('#<%=hiddenGroupKey.ClientID %>').val(), Aqufit.Relationship.GROUP_ADMIN, skip, take, function (json) {
                    Aqufit.Page.atiMemberListAdmin.generateStreamDom(json);
                    //Aqufit.Page.atiLoading.remove();
                }, function (err) { });
            };
            Aqufit.Page.atiMemberListAdmin.onRemoveMember = function (mid) {
                if (confirm("Are you sure you want to remove user from Admins?\n NOTE: THEY WILL STILL BE A MEMBER.")) {
                    $('#<%=hiddenAjaxAction.ClientID %>').val('removeAdmin');
                    $('#<%=hiddenAjaxValue.ClientID %>').val(mid);
                    __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');
                }
            };
            Aqufit.Page.atiFriendList.onDataNeeded = function (skip, take) {
                Aqufit.Page.atiLoading.addLoadingOverlay('panelInviteFriend');
                $('#<%=hiddenAjaxAction.ClientID %>').val('friendData');
                $('#<%=hiddenAjaxValue.ClientID %>').val(skip);    // skip
                $('#<%=hiddenAjaxValue2.ClientID %>').val(take);  // take                   
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');    
            }
            Aqufit.Page.atiFriendList.acceptFriendAction = function (fid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('sendGroupInvite');
                $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$GroupAdmin","ATI_GroupAdmin") %>', '');
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

<ati:LoadingPanel ID="atiLoading" runat="server" />

<telerik:RadWindow ID="ErrorDialog" runat="server" Skin="Black" Title="Error" Width="450" Height="250" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
        <ContentTemplate>
            <div style="width: 100%; height: 100%; background-color: white; position: relative; overflow: hidden;">
               <div style="position: absolute; top: 50px; left: 40px; color: Black; font-size: 12px;">                   
                   <h3 style="color: Red; font-size: 16px;">                   
                       <img id="imgError" runat="server" style="float: left; clear: left; padding-right: 10px;" />
                       We have encountered a problem.
                   </h3>  
                   <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="aqufit" />                     
                   <div id="errorStatusDiv" style="padding: 10px;">Unknown</div>     
               </div>       
            </div>            
        </ContentTemplate>
</telerik:RadWindow>
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


<asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue2" runat="server" />
    <asp:HiddenField ID="hiddenGroupKey" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<asp:Panel ID="atiGroupAdmin" runat="server">
    <div id="tabs">
        <ul>
            <li id="tabInfo" runat="server"><a href="#pageViewInfo">Basic Info</a></li>
            <li id="tabAdvanced" runat="server"><a href="#pageViewAdvanced">Advanced</a></li>
            <li id="tabAppearance" runat="server"><a href="#pageViewAppearance">Appearance</a></li>
            <li id="tabMembers" runat="server"><a href="#pageViewMembers">Members</a></li>
            <li id="tabInvite" runat="server"><a href="#pageViewInvite">Invite</a></li>
        </ul>
        <div id="pageViewInfo" class="innerFlat">
             <div runat="server" id="panelProfilePic" style="float: left; width: 220px;" >
                <ati:ProfileImage ID="atiProfileImage" runat="server" Width="192px" />   
                <div class="groupCreateHelp">
                    <h2>Group Registration Help</h2>
                    
                    <h4>Group Url</h4>
                    <span>Group Url name must be a unique name.  This will create a group page on FelxFWD with the following address format:<br /><br />
                    <span style="font-size: 10px;">http://flexfwd.com/group/<strong>[Group Url]</strong></span></span>

                    <h4>Location Information</h4>
                    <span>We geolocate your group on the map.  This allows people in your area to be made aware of your group and the services that you offer.</span>

                </div>
            </div>
            <div style="float: right; width: 650px;" id="atiRightPanel" runat="server">
                <asp:Panel ID="atiGroupBasicInfo" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                <fieldset>
                <dl>        
                    <dt><asp:Label ID="lGroupName" runat="server" controlname="txtGroupName" Text="Name:" /></dt>
                    <dd>
                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" Width="300px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ValidationGroup="groups" ControlToValidate="txtGroupName" ErrorMessage="Name is required!" Text="*" />  
                    </dd>
                    
                    <ati:UserName Id="atiGroupName" runat="server" Mode="GROUP_NAME" ValidationGroupName="groups" Width="300px" />                    

                    <dt><asp:Label id="Label1" runat="server" controlname="txtEmail" text="Email&nbsp;Address:" /></dt>	
                    <dd>
                        <asp:TextBox ID="atiTxtGroupEmail" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" Width="300px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="groups" ControlToValidate="atiTxtGroupEmail" ErrorMessage="Email is required!" Text="*" />  
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="groups" ControlToValidate="atiTxtGroupEmail" ValidationExpression="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" ErrorMessage="Email Address has wrong format" Text="*" />
                    </dd>

                    <dt><asp:Label ID="lGroupDescription" runat="server" controlname="txtGroupDescription" Text="Description:" /></dt>
                    <dd>
                        <asp:TextBox ID="atTxtGroupDescription" runat="server" TextMode="MultiLine" MaxLength="512" Width="300px" Height="100px" CssClass="ui-corner-all ui-widget-content atiTxtBox" />                        
                    </dd>
    
                    <dt><asp:Label ID="lGroupType" runat="server" controlname="ddlGroupType" Text="Group Type:" /></dt>
                    <dd><asp:DropDownList ID="ddlGroupType" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" Width="300px" /></dd>

                    <dt><asp:Label ID="lAccess" runat="server" controlname="rblAccess" Text="Access:" /></dt>
                    <dd>
                        <ul style="width: 300px;">
                            <li><input id="rbAccessPublic" type="radio" runat="server" checked="true" value="public" /> <label for="rbAccessPublic"><strong>This group is open.</strong></label><br /> <span>Anyone can join and invite others to join.  Group info and content can be viewed by anyone and may be indexed by search engines.</span></li>
                            <li><input id="rbAccessPrivate" type="radio" runat="server" value="private" /> <label for="rbAccessPrivate"><strong>This group is closed.</strong></label><br /> <span>Admins must approve requests for new members to join.  Anyone can see the group description, but only members can interact with the group.</span></li>
                        </ul>
                    </dd>                     
        
                    <ati:Address ID="atiAddress" runat="server" Width="300px" />
                    
                </dl>
                </fieldset>                                
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>

        <div id="pageViewAdvanced" class="innerFlat">
            <div style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Advanced Help</h2>
                    
                    <h4>Follow Group</h4>
                    <span>This option allows for you to follow the same workout schedule as another group.  For example my group "CrossFit FlexFWD" follows the programming from "Crossfit.com"<br />
                    <br />
                    Note: You will still be allowed to schedule other workouts on top of the group being followed.</span>                    

                </div>
            </div>
            <div style="float: right; width: 650px;">
                <asp:Panel ID="atiPanelAdvanced" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                    <fieldset>
                    <dl>        
                        <dt><asp:Label ID="lFollowGroup" runat="server" controlname="txtFollowGroup" Text="Follow another groups programming:" /></dt>
                        <dd>
                            <telerik:RadComboBox ID="atiRadComboBoxSearchGroups" runat="server" Width="275px" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                EmptyMessage="Enter Group Name (eg: crossfit flexfwd)" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchGroups_ItemsRequested">
                            </telerik:RadComboBox>
                        </dd>
                    </dl>
                    </fieldset>
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>

        <div id="pageViewAppearance" class="innerFlat">
            <div style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Appearance Help</h2>
                    
                    <h4>Your URL</h4>
                    <span>This is your business or blog url that will be displayed on the profile page.</span>                    

                    <h4>Other Profiles</h4>
                    <span>These are your other accounts: Facebook, Twitter, Flikr, LinkedIn, and YouTube.</span>

                    <h4>Background Image</h4>
                    <span>You can customize your profile background with a custom image of your choice.</span>

                    <h4>Background Color</h4>
                    <span>Customize your profile background color.</span>
                </div>
            </div>
            <div style="float: right; width: 650px;">
                <asp:Panel ID="atiPanelAppearance" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                    <fieldset>
                    <dl> 
                        <ati:WebLinks id="atiWebLinks" runat="server" />                    
                        <ati:ThemeEditor id="atiThemeEditor" runat="server" />
                    </dl>
                    </fieldset>
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>

        <div id="pageViewMembers" class="innerFlat">
            <div style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Member Managment</h2>
                    
                    <h4>Admin Users</h4>
                    <span>Making a member an admin user gives them access to change any of the group settings.<br />
                    <br />
                    
                </div>
            </div>
            <div style="float: right; width: 650px;">                
                <asp:Panel ID="atiPanelMembers" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">                    
                    <h3>Creator</h3>
                    <ati:ProfileImage ID="atiOwnerProfile" runat="server" Width="192px" Small="true" />  
                    <br /><br /> 
                    <h3>Admin Members</h3>
                    <div class="innerFlat" style="border: 1px solid #ccc;"><ati:FriendListScript ID="atiMemberListAdmin" runat="server" ControlMode="MEMBERADMIN_ADMIN" /></div>
                    <br /><br />
                    <h3>Member List</h3>
                    <div class="innerFlat" style="border: 1px solid #ccc;"><ati:FriendListScript ID="atiMemberList" runat="server" ControlMode="MEMBER_ADMIN" /></div>
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>

        <div id="pageViewInvite" class="innerFlat">
            <div style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Invite</h2>
                    
                    <h4>Select contacts from your email</h4>
                    <span>Import you contact list from: gmail, yahoo mail, or microsoft hotmail.  You can then select which contacts you would like to send an invite to.  Additionally adding a personal message to the invite.</span>                    

                    <h4>Manually add addresses of people to invite</h4>
                    <span>If you have a contact sheet or other comma seperated list of email addresses, simply copy and past them into address field.  Then customize the invite message and click send.</span>
                </div>
            </div>
            <div style="float: right; width: 650px;">                
                <asp:Panel ID="atiPanelInvite" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">                                        
                    <h3>Invite via Email</h3>
                    <div class="innerFlat" style="border: 1px solid #ccc;">
                        <ati:FindInvite id="atiFindInvite" runat="server" IsInviteOnly="true" />
                    </div>
                    <br /><br />
                    <h3>Invite Friends</h3>
                    <div id="panelInviteFriend" class="innerFlat" style="border: 1px solid #ccc;"><ati:FriendListScript ID="atiFriendList" runat="server" ControlMode="GROUP_INVITE" /></div>                    
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>
        <asp:ValidationSummary ID="ValidationSummary5" CssClass="validateSumm" runat="server" ValidationGroup="groups" />
        <div style="text-align: right; padding: 5px 20px; position: relative;">
            <a id="hrefBackToProfile" runat="server" style="position:absolute; left: 10px; top: 10px;" visible="false">Back</a>
            <asp:Button ID="bCreateGroup" runat="server" Text="Create Group" OnClick="bCreateGroup_Click" ValidationGroup="groups" CssClass="boldButton" />
        </div>
</div>

</asp:Panel>


    

         
    
               



