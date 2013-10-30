<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Compare.ViewATI_Compare" CodeFile="ViewATI_Compare.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="Profile" Src="~/DesktopModules/ATI_Base/controls/ATI_Profile.ascx" %>
<%@ Register TagPrefix="ati" TagName="NameValueGrid" Src="~/DesktopModules/ATI_Base/controls/ATI_NameValueGrid.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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


</style>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<script type="text/javascript" >

    Aqufit.Windows.MessageWin = {
        open: function (arg) {
            this.win = radopen('<%=ResolveUrl("~/FitnessProfile/MesageSend.aspx") %>?u=' + Aqufit.Page.UserName, 'MessageWin');
            this.win.set_modal(true);
            this.win.setSize(747, 600);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };


    Aqufit.Windows.FollowAthleteModal = {
        win: null,
        open: function () {            
            Aqufit.Windows.FollowAthleteModal.win = $find('<%=FollowAthleteModal.ClientID %>');
            Aqufit.Windows.FollowAthleteModal.win.show();
        },
        close: function () {
            Aqufit.Windows.FollowAthleteModal.win.close();
        }
    };  

    Aqufit.Page.Actions = {
        SendFriendRequest: function () {
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddFriend');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Compare") %>', '');
        }         
    };

    function OnResponseEnd(){
        Aqufit.Page.atiLoading.remove();
    }

    $(function () {
        $('#workoutList').button().click(function (event) {
            self.location.href = '<%= ResolveUrl("~/") %>' + Aqufit.Page.UserName + '/workout-history';
            event.stopPropagation();
            return false;
        });                               
        $('#bAddToFriends').click(function () {
            $('#faModalContainer').append(loading);
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddFriend');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Compare") %>', '');
        });
        $('#bAddToFollow').click(function () {
            $('#faModalContainer').append(loading);
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddFollow');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Fitness","ATI_Compare") %>', '');
        });



    });

    Aqufit.addLoadEvent(function () {        
            
    });
          
</script>    
</telerik:RadCodeBlock>

    <telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>
        <telerik:RadWindow runat="server" ID="MessageWin" />
        <telerik:RadWindow runat="server" ID="WatchList" />
        <telerik:RadWindow runat="server" ID="UploadWin" />        

        <telerik:RadWindow ID="FollowAthleteModal" runat="server" Skin="Black" Title="Add Athlete to ..." Width="700" Height="300" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div id="faModalContainer" style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div class="faChooseDiv grad-FFF-EEE" style="width: 50%; height: 100%; float: right; border-left: 1px solid #CCC;">
                        <div style="padding: 15px;">
                            <div id="bAddToFriends" class="normalButton"><img id="imgAddToFriends" runat="server" align="absmiddle" /> Send a Friend Request</div>
                            <span>
                                <p>This is an Athlete that you know.  Friends are able to perform a number of specific actions:</p>
                                <ul>
                                    <li>Compare you workouts.</li>
                                    <li>Send messages to the user.</li>
                                    <li>View thier ...</li>
                                </ul>
                            </span>
                        </div>
                    </div>
                    <div class="faChooseDiv grad-FFF-EEE" style="width: 50%; height: 100%;">
                        <div style="padding: 15px;">
                            <div id="bAddToFollow" class="normalButton"><img id="imgAddToFollow" runat="server" align="absmiddle" /> Add to Watch List</div>
                            <span>
                                <p>This i an Athlete that you want to track.  Tracking an Athlete allows you to:</p>
                                <ul style="list-style: disc;">
                                    <li>Compare you workouts.</li>
                                </ul>
                            </span>
                        </div>
                    </div>                                                
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
    </Windows>
</telerik:radwindowmanager>
  
<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">  
    <AjaxSettings>               
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel2"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>            
    </AjaxSettings>
    <ClientEvents OnResponseEnd="OnResponseEnd" />    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Office2007" />

<ati:LoadingPanel ID="atiLoading" runat="server" />

<asp:Panel ID="atiFitness" runat="server">
    
    <asp:Panel ID="panelAjax" runat="server" >
        <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
        <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
        <asp:HiddenField ID="hiddenWorkoutKey" runat="server" />
        <asp:Button ID="bAjaxPostback" runat="server" style="display: none;" OnClick="bAjaxPostback_Click" />
    </asp:Panel>

    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr valign="top">
            <td style="width: 195px;"><ati:Profile ID="atiProfile" runat="server" ShowCompareButton="false" Mode="Bio" /></td>
            <td style="padding: 0px 16px;">
            
                <div class="profileHeading grad-FFF-EEE ui-corner-all">
                    <asp:Literal ID="lUserName" runat="server" />  
                    <asp:Literal ID="lUserNameYou" runat="server" />                     
                </div>                      
                <asp:Button ID="bExportToExcel" runat="server" Text="Export Table" OnClick="bExportToExcel_Click" />
                <asp:Button ID="bExportToCSV" runat="server" Text="Export to CSV" OnClick="bExportToCsv_Click" />
                <telerik:RadGrid ShowGroupPanel="false" AutoGenerateColumns="false" ID="RadGrid1" OnNeedDataSource="RadGrid1_NeedDataSource"
                    AllowFilteringByColumn="True" AllowSorting="True" PageSize="50"
                    ShowFooter="True" runat="server" GridLines="None" AllowPaging="true" EnableLinqExpressions="false">
                <PagerStyle Mode="NextPrevAndNumeric" />               
                <MasterTableView ShowGroupFooter="true" AllowMultiColumnSorting="true">                 
                <Columns>
                    <telerik:GridNumericColumn SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false"></telerik:GridNumericColumn>  
                    <telerik:GridBoundColumn DataField="UserName" HeaderText="UserName" SortExpression="UserName" UniqueName="UserName" AllowFiltering="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Title" HeaderText="Workout" SortExpression="Title" UniqueName="ProductName"></telerik:GridBoundColumn>
                    <telerik:GridDateTimeColumn SortExpression="Date" HeaderText="Date" DataField="Date" UniqueName="Date" DataFormatString="{0:MMM dd, yyyy}" AllowFiltering="false"></telerik:GridDateTimeColumn>
                    <telerik:GridBoundColumn DataField="Score" HeaderText="Score" SortExpression="Score" UniqueName="Score"></telerik:GridBoundColumn>
                </Columns>
                <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="WODKey" />
                        </GroupByFields>
                        <SelectFields>
                            <telerik:GridGroupByField FieldName="Title" HeaderText="Workout" />
                        </SelectFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>
                </MasterTableView>
                <ClientSettings AllowDragToGroup="true" />
                <GroupingSettings ShowUnGroupButton="false" />
                </telerik:RadGrid>              
            </td>
            <td style="width: 196px;"><ati:Profile ID="atiProfileYou" runat="server" IsFriend="true" ShowCompareButton="false" Mode="Bio" /></td>
        </tr>
        </table>
                
    </div>    
    </asp:Panel>
         
    
               



