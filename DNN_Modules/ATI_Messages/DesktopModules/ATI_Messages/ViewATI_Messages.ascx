<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Messages.ViewATI_Messages" CodeFile="ViewATI_Messages.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_Profile.ascx" %>
<%@ Register TagPrefix="ati" TagName="QuickNav" Src="~/DesktopModules/ATI_Base/controls/ATI_QuickNav.ascx" %>
<%@ Register TagPrefix="ati" TagName="MessageListScript" Src="~/DesktopModules/ATI_Base/controls/ATI_MessageListScript.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">

#tabs ul li
{
	list-style: none;
	list-style: none outside none;
}
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">                    
    <script type="text/javascript" >      
        Aqufit.Windows.MessageWin = {
            win: null,
            open: function (arg) {
                this.win = window.radopen('<%=ResolveUrl("~/FitnessProfile/MesageSend.aspx") %>', null);
                this.win.set_modal(true);
                this.win.setSize(747, 600);
                this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
                this.win.center();
                this.win.show();
                return false;
            }
        };

        Aqufit.Page.Actions = {
            OnClientSelectedIndexChangedEventHandler: function (sender, args) {
                var item = args.get_item();
                if (item.get_value() != '') {    // User has selected the "add new map"
                    var selected = $('#tabs').tabs('option', 'selected');
                    self.location.href = Aqufit.Page.PageBase + 'Profile/Inbox?m=' + item.get_value() + (selected > 0 ? '#pageViewSent' : '#pageViewInbox');
                }
            },
            SendSuggestedFriendRequest: function (usid) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('AddSuggestFriend');
                $('#<%=hiddenAjaxValue.ClientID %>').val(usid);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Messages","ATI_Messages") %>', '');
            }
        };

        function OnResponseEnd(sender, args) {
            $('#atiStatusWidget').fadeIn('slow');
            setTimeout(function () { $('#atiStatusWidget').hide(); }, 3000);
        }
        function OnRequestStart(sender, args) {
            $('#atiStatusWidget').hide();
        }

        $(function () {
            $('#atiStatusWidget').hide();
            $('#tabs').tabs();
            $('div.ui-state-default').hover(function () {
                $(this).addClass('ui-state-hover');
            }, function () {
                $(this).removeClass('ui-state-hover');
            });

            $('#bOpenMessanger').button({
                icons: {
                    primary: 'ui-icon-mail-closed'
                }
            }).click(function(event){
                Aqufit.Windows.MessageWin.open();
                event.stopPropagation();
                return false;
            });
            //atiStatusWidget
        });

        Aqufit.addLoadEvent(function () {
            Aqufit.Page.MessageListScript.deleteCallback = function (id) {
                $('#<%=hiddenMessageListDeleteId.ClientID %>').val(id);
                __doPostBack('<%=bMessageListDelete.ClientID.Replace("_","$").Replace("ATI$Messages","ATI_Messages") %>', '');
            };
            Aqufit.Page.MessageSentListScript.deleteCallback = function (id) {
                $('#<%=hiddenMessageSentDeleteId.ClientID %>').val(id);
                __doPostBack('<%=bMessageListDelete.ClientID.Replace("_","$").Replace("ATI$Messages","ATI_Messages") %>', '');
            };            
        });
        
        
        
              
    </script>            
    </telerik:RadCodeBlock>


<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>     
        <telerik:AjaxSetting AjaxControlID="bMessageListDelete">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting> 
        <telerik:AjaxSetting AjaxControlID="bMessageSentDelete">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="litStatus"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>                         
    </AjaxSettings>        
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" /> 

<asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>


<div id="atiStatusWidget" class="ui-widget">
	<div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Literal ID="litStatus" runat="server" />
	</div>
</div>

    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" style="float: right; position: relative;">
        <div id="divMain" style="width: 560px; margin-left: 8px; margin-right: 168px;">            
            <!-- Tabs -->
    		<div id="tabs">
    			<ul>
    				<li id="tabInbox"><a href="#pageViewInbox">Inbox</a></li>
    				<li id="tabSent"><a href="#pageViewSent">Sent</a></li>                      
                    <button id="bOpenMessanger" style="position: absolute; right: 6px; top: 6px;">Compose Mail</button>
                </ul>
    			<div id="pageViewInbox" style="padding: 0px;">   
                    <div class="atiSearchControls grad-FFF-EEE">           
                        <telerik:RadComboBox ID="atiRadComboBoxSearchMessageInbox" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                EmptyMessage="Search My Mail" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchMessageInbox_ItemsRequested"
                                OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                        </telerik:RadComboBox>       
                    </div>                                                                                   
                    <ati:MessageListScript id="MessageListScript" runat="server" Mode="Inbox" />     
                    <asp:HiddenField ID="hiddenMessageListDeleteId" runat="server" />
                    <asp:Button ID="bMessageListDelete" runat="server" style="display: none;" OnClick="bMessageListDelete_Click" />
                </div>

    			<div id="pageViewSent" style="padding: 0px;">
                    <div class="atiSearchControls grad-FFF-EEE">
                        <telerik:RadComboBox ID="atiRadComboBoxSearchMessageSent" runat="server" Width="100%" Height="140px" CssClass="ui-corner-all ui-widget-content atiTxtBox"
                                EmptyMessage="Search My Mail" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true" OnItemsRequested="atiRadComboBoxSearchMessageSent_ItemsRequested"
                                OnClientSelectedIndexChanged="Aqufit.Page.Actions.OnClientSelectedIndexChangedEventHandler">
                        </telerik:RadComboBox>   
                    </div>
                    <ati:MessageListScript id="MessageSentListScript" runat="server" Mode="Sent" />   
                    <asp:HiddenField ID="hiddenMessageSentDeleteId" runat="server" />
                    <asp:Button ID="bMessageSentDelete" runat="server" style="display: none;" OnClick="bMessageSentDelete_Click" />
                </div>            			
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; position:absolute; right:0; top: 0;">
            <img runat="server" id="imgAd" />
        </div>
    
    </div>
    <div id="divLeftNav" style="width: 196px; float: left;">
        <ati:Profile IsSmall="true" id="atiProfile" runat="server" />                            
    </div>
    <div style="clear:both;"></div>          




    

    

         
    
               



