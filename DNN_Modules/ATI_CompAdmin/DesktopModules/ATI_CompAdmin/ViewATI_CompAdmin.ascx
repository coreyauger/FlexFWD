<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_CompAdmin.ViewATI_CompAdmin" CodeFile="ViewATI_CompAdmin.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">     
<script type="text/javascript" >

    function CompWOD(id, name, type, compType) {
        this.id = id;
        this.name = name;
        this.type = type;
        this.compType = compType;
    };

    Aqufit.Windows.WorkoutFinder = {
        win: null,
        open: function (json) {
            Aqufit.Windows.WorkoutFinder.win = $find('<%=WorkoutFinder.ClientID %>');
            Aqufit.Windows.WorkoutFinder.win.show();
        },
        close: function () {
            Aqufit.Windows.WorkoutFinder.win.close();
        }
    };

    Aqufit.Windows.TeamPoolWin = {
        win: null,
        open: function (json) {
            Aqufit.Windows.TeamPoolWin.win = $find('<%=TeamPoolWin.ClientID %>');
            Aqufit.Windows.TeamPoolWin.win.show();
        },
        close: function () {
            Aqufit.Windows.TeamPoolWin.win.close();
        }
    };

    Aqufit.Windows.TeamPoolManagerWin = {
        win: null,
        open: function (json) {
            Aqufit.Windows.TeamPoolManagerWin.win = $find('<%=TeamPoolManagerWin.ClientID %>');
            Aqufit.Windows.TeamPoolManagerWin.win.show();
            Aqufit.Page.atiLoading.addLoadingOverlay('panelTeamPoolMan');
        },
        loadData: function (idArray) {
            var listboxDst = $find("<%= RadListBoxPoolDest.ClientID %>");
            listboxDst.trackChanges();
            listboxDst.get_items().clear();
            listboxDst.commitChanges();
            var listbox = $find("<%= RadListBoxTeamSource.ClientID %>");
            listbox.trackChanges();
            for (var i = 0; i < idArray.length; i++) {
                var item = listbox.findItemByValue(idArray[i]);
                listbox.transferToDestination(item);
            }
            listbox.commitChanges();
            Aqufit.Page.atiLoading.remove();
        },
        close: function () {
            Aqufit.Windows.TeamPoolManagerWin.win.close();
        }
    };
    

    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs2').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        ShowFail: function (msg) {
            radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
        },
        ShowSuccess: function (msg) {
            radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
        },
        findWorkout: function (compType) {
            $('#<%=hiddenAjaxValue3.ClientID %>').val($('#<%=ddlTeamPools.ClientID %> option:selected').val());
            $('#<%=hiddenAjaxValue2.ClientID %>').val(compType);
            Aqufit.Windows.WorkoutFinder.open();
        },
        createTeamPool: function () {
            Aqufit.Windows.TeamPoolWin.open();
        },
        OnResponseEnd: function () {
            Aqufit.Page.atiLoading.remove();
        },
        openPoolAdmin: function (id) {
            var listboxSrc = $find("<%= RadListBoxTeamSource.ClientID %>");
            var listboxDst = $find("<%= RadListBoxPoolDest.ClientID %>");
            var itemColl = listboxDst.get_items();
            listboxSrc.trackChanges();
            listboxDst.trackChanges();
            itemColl.forEach(function (item) {
                listboxSrc.transferFromDestination(item);
            });
            listboxSrc.commitChanges();
            listboxDst.commitChanges();
            $('#<%=hiddenAjaxAction.ClientID %>').val('LoadTeamPool');
            $('#<%=hiddenAjaxValue.ClientID %>').val(id);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompAdmin","ATI_CompAdmin") %>', '');
            Aqufit.Windows.TeamPoolManagerWin.open();
            Aqufit.Page.atiLoading.addLoadingOverlay('workoutStatusPanel');
        },
        refreshTeamPoolGrid: function () {
            var masterTable = $find("<%= RadGridTeamPool.ClientID %>").get_masterTableView();
            masterTable.rebind();
        },
        refreshGrid: function (compType) {
            var masterTable;
            if (compType == 1) {
                masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
            } else if (compType == 2) {
                masterTable = $find("<%= RadGrid2.ClientID %>").get_masterTableView();
            }
            else if (compType == 3) {
                masterTable = $find("<%= RadGrid3.ClientID %>").get_masterTableView();
            }
            masterTable.rebind();
        },
        deleteRow: function (cId) {
            if (confirm("Are you sure you want to delete this row?")) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('DeleteWOD');
                $('#<%=hiddenAjaxValue.ClientID %>').val(cId);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompAdmin","ATI_CompAdmin") %>', '');
            }
        }
    };

    function atiRadCombo_OnClientItemsRequesting(sender, eventArgs) {
        var context = eventArgs.get_context();
        context["UserSettingsId"] = Aqufit.Page.UserSettingsId;
    }

    $(function () {
        $('#atiStatusWidget').hide();
        $('.dull').focus(function () {
            $(this).val('').removeClass('dull').unbind('focus');
        });
        $('#tabs').tabs();
        $('#bCreateNewWorkout').button().click(function (event) {
            top.location.href = '<%=ResolveUrl("~/Profile/WorkoutBuilder") %>?ret=' + top.location.href;
            event.stopPropagation();
            return false;
        });
        $('#<%=ddlTeamPools.ClientID %>').change(function () {
            var masterTable = $find("<%= RadGrid2.ClientID %>").get_masterTableView();
            masterTable.rebind();
        });
        $('#bDone').button().click(function (event) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddWOD');
            var combo = $find("<%= atiRadComboBoxCrossfitWorkouts.ClientID %>");
            if (combo.get_value() != '') {
                Aqufit.Page.atiLoading.addLoadingOverlay('workoutStatusPanel');
                var wod = eval('(' + combo.get_value() + ')');
                $('#<%=hiddenAjaxValue.ClientID %>').val(wod.Id);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompAdmin","ATI_CompAdmin") %>', '');
            } else {
                // error bad wod...
            }
            event.stopPropagation();
            return false;
        });
        $('#bSaveTeamPool').button().click(function (event) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('AddTeamPool');
            var name = $('#<%= txtTeamPoolName.ClientID%>').val();
            if (name == '') {
                $('#teamPoolError').html('Pool name must have a value.');
            } else {
                $('#<%=hiddenAjaxValue.ClientID %>').val(name);
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompAdmin","ATI_CompAdmin") %>', '');
            }
            event.stopPropagation();
            return false;
        });
        $('#bSavePoolManager').button().click(function (event) {
            Aqufit.Page.atiLoading.addLoadingOverlay('workoutStatusPanel');
            $('#<%=hiddenAjaxAction.ClientID %>').val('SaveTeamPool');
            // note the teampool id is already loaded into Value1
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompAdmin","ATI_CompAdmin") %>', '');
            event.stopPropagation();
            return false;
        });
    });        

        
</script>    
<style type="text/css">    
div.atiStreamItemRight
{
	width: 380px !important;
}
div.compOptions
{
	padding: 20px;
}
</style>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="Aqufit.Page.Actions.OnResponseEnd" />
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>      
        <telerik:AjaxSetting AjaxControlID="RadGrid1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>  
        <telerik:AjaxSetting AjaxControlID="RadGrid2">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>         
        <telerik:AjaxSetting AjaxControlID="RadGrid3">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>     
        <telerik:AjaxSetting AjaxControlID="RadGridTeamPool">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridTeamPool" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                
            </UpdatedControls>
        </telerik:AjaxSetting>                   
    </AjaxSettings>        
</telerik:RadAjaxManager>

<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" /> 
<ati:LoadingPanel ID="atiLoading" runat="server" />

<asp:Panel ID="panelAjax" runat="server">
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue2" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue3" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" OnClick="bAjaxPostback_Click" style="display: none;" />
</asp:Panel>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>            
        <telerik:RadWindow ID="WorkoutFinder" runat="server" Skin="Black" Title="Workout Finder" Width="600" Height="300" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3>Workout Finder</h3>                       
                    </div>
                    <div style="padding: 30px 10px;">
                        <center>
                        <telerik:RadComboBox ID="atiRadComboBoxCrossfitWorkouts" runat="server" Width="50%" Height="140px"
                        EmptyMessage="Select a WOD" EnableLoadOnDemand="True" ShowMoreResultsBox="true"
                        EnableVirtualScrolling="true" OnClientItemsRequesting="atiRadCombo_OnClientItemsRequesting">
                        <WebServiceSettings Method="GetWorkoutsOnDemand" Path="~/DesktopModules/ATI_Base/resources/services/StreamService.asmx" />
                        </telerik:RadComboBox>
                         - OR - 
                         <button id="bCreateNewWorkout">Create new workout</button> 
                         </center>
                         <br /><br />
                         <span>Enter the name of a workout.  When it appears in the drop down manu, select it and click the done button.</span>
                    </div>                    
                       
                    <button id="bDone" style="position: absolute; bottom: 10px; right: 10px;">Done</button>                 
                   
                </div>                    
            </ContentTemplate>
        </telerik:RadWindow> 
        
        <telerik:RadWindow ID="TeamPoolWin" runat="server" Skin="Black" Title="Create Team Pool" Width="600" Height="250" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3>Create Team Pool</h3>                       
                    </div>
                    <div style="padding: 30px 10px;">
                        <center>
                        Name of Pool: <asp:TextBox ID="txtTeamPoolName" runat="server" MaxLength="127" />
                        <span id="teamPoolError" style="color: Red; display: block;"></span>                   
                        </center>
                        <br /><br />
                    </div>                                           
                    <button id="bSaveTeamPool" style="position: absolute; bottom: 10px; right: 10px;">Done</button>                 
                   
                </div>                    
            </ContentTemplate>
        </telerik:RadWindow>   
        
        <telerik:RadWindow ID="TeamPoolManagerWin" runat="server" Skin="Black" Title="Team Pool Manager" Width="600" Height="400" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div id="panelTeamPoolMan" style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3>Team Pool Manager</h3>                       
                    </div>
                    <div style="padding: 30px 10px;">
                        <span>All Teams</span>
                        <span style="float: right; padding-right: 75px;">Teams Assigned to Pool</span><br />
                        <telerik:RadListBox ID="RadListBoxTeamSource" runat="server" Width="250px" Height="200px" AllowTransferDuplicates="false" TransferMode="Move"
                            SelectionMode="Multiple" AllowTransferOnDoubleClick="true" AllowTransfer="true" TransferToID="RadListBoxPoolDest" AutoPostBackOnTransfer="false"
                            AllowReorder="false" EnableDragAndDrop="true">  
                            <ButtonSettings ShowTransferAll="false" />                          
                        </telerik:RadListBox>                        
                        <telerik:RadListBox ID="RadListBoxPoolDest" runat="server" Width="250px" Height="200px" AllowDelete="false" TransferMode="Move"  AllowTransferDuplicates="false"
                                            SelectionMode="Multiple" AllowReorder="false" AutoPostBackOnReorder="false" EnableDragAndDrop="true">           
                        </telerik:RadListBox>
                        <br /><br />
                    </div>                                           
                    <button id="bSavePoolManager" style="position: absolute; bottom: 10px; right: 10px;">Done</button>                   
                </div>                    
            </ContentTemplate>
        </telerik:RadWindow>       
    </Windows>
</telerik:radwindowmanager>
  
<!-- Start of a 2 col box layout -->    
<div id="divCenterWrapper" style="width: 100%">
    
      
       <!-- Tabs -->
    	<div id="tabs">
    		<ul>
                <li id="tabViewInfo"><a href="#pageViewInfo">Competitions Details</a></li>   
                <li id="tabTeams"><a href="#pageCompTeams">Team Pool Manager</a></li> 
                <li id="tabWorkouts"><a href="#pageCompWorkouts">Competitions Workouts</a></li> 
                <li id="tabRegistration"><a href="#pageRegistration">Registration</a></li>    				                              
            </ul>  
            
            <div id="pageViewInfo" class="innerFlat">
             <div runat="server" id="panelProfilePic" style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Competition Creation Help</h2>
                    
                    <h4>Competition Owner</h4>
                    <span>Name of the group that created the competion.</span>

                    <h4>Competition Name</h4>
                    <span>Name of your competition.  eg. "CrossFit X - Fight Gone Bad 4"</span>

                    <h4>Competition Description</h4>
                    <span>Brief description of what your competition is about.</span>

                    <h4>Competition Dates</h4>
                    <span>The starting and ending dates of your competition.</span>
                </div>
            </div>
            <div style="float: right; width: 650px;">
                <asp:Panel ID="atiGroupBasicInfo" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                <fieldset>
                <dl> 
                    <dt><asp:Label ID="lCompOwner" runat="server" controlname="txtCompName" Text="Competition Owner:" /></dt>
                    <dd>
                        <strong><asp:Literal ID="litCompOwner" runat="server" Text="" /></strong>
                    </dd>
                       
                    <dt><asp:Label ID="lCompName" runat="server" controlname="txtCompName" Text="Competition Name:" /></dt>
                    <dd>
                        <asp:TextBox ID="txtCompName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" Width="300px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ValidationGroup="groups" ControlToValidate="txtCompName" ErrorMessage="Name is required!" Text="*" />  
                    </dd>

                    <dt><asp:Label ID="lCompDescription" runat="server" controlname="txtCompDescription" Text="Description:" /></dt>
                    <dd>
                        <asp:TextBox ID="atTxtCompeDescription" runat="server" TextMode="MultiLine" MaxLength="512" Width="300px" Height="100px" CssClass="ui-corner-all ui-widget-content atiTxtBox" />                        
                    </dd>
                    
                    <dt><asp:Label id="lCompStartDate" runat="server" controlname="txtEmail" text="Competition Start&nbsp;Date:" /></dt>	
                    <dd>
                        <telerik:RadDateInput ID="RadCompStartDate" runat="server" />
                    </dd>

                    <dt><asp:Label id="lCompEndDate" runat="server" controlname="txtEmail" text="Competition End&nbsp;Date:" /></dt>	
                    <dd>
                        <telerik:RadDateInput ID="RadCompEndDate" runat="server" />
                    </dd>        
                    
                </dl>
                </fieldset>                                
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>


        <div id="pageCompTeams" class="innerFlat">
             <div runat="server" id="Div3" style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Team Manager</h2>
                    
                    <h4>Create a Team Pool</h4>
                    <span>Create a competition pool.  Once you have a competition pool you can assign teams to it.  Each pool will be scored seperate.</span>

                    <h4>Add / Drop Teams</h4>
                    <span>Once you have created a pool.  Add or drop teams to that pool.  You can then assign workouts to a pool in the 'Competitions Workouts' tab.</span>
                </div>
            </div>
            <div style="float: right; width: 650px;">
                <asp:Panel ID="Panel3" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                
                <div class="normalButton" onclick="Aqufit.Page.Actions.createTeamPool();">
                Create a Team Pool
                <asp:Image id="Image6" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" />
                </div>                  
                 
                 <telerik:RadGrid ID="RadGridTeamPool" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowGroupPanel="false" 
                        AllowPaging="True" PageSize="25" runat="server" AllowSorting="false" AllowFilteringByColumn="false" AllowAutomaticInserts="false" AllowAutomaticDeletes="true"
                        OnNeedDataSource="RadGridTeamPool_NeedDataSource" GridLines="None">
                    <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <Columns>                                 
                        <telerik:GridBoundColumn ItemStyle-CssClass="Id" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                        <telerik:GridBoundColumn ItemStyle-CssClass="compRegId" DataType="System.Int64" SortExpression="CompRegKey" HeaderText="CompRegKey" HeaderButtonType="TextButton" DataField="CompRegKey" Display="false" />    
                        <telerik:GridTemplateColumn ItemStyle-CssClass="compPoolName" HeaderText="Pool" SortExpression="Pool" HeaderButtonType="TextButton" DataField="Pool" AllowFiltering="false" >
                            <ItemTemplate>
                                <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='<%# Eval("Pool") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn ItemStyle-CssClass="compTeamNames" DataType="System.String" SortExpression="Teams" HeaderText="Teams" HeaderStyle-Width="300px" HeaderButtonType="TextButton" DataField="Teams" Display="true" />    
                        
                        <telerik:GridTemplateColumn HeaderText="Actions" HeaderButtonType="TextButton">
                        <ItemTemplate>   
                            <a href="javascript: ;" title="Add Drop Teams" onclick="Aqufit.Page.Actions.openPoolAdmin( <%#Eval("Id") %>)">
                            [ add/remove teams ]
                            </a>
                            &nbsp;&nbsp;&nbsp;
                            <a href="javascript: ;" title="Delete" onclick="Aqufit.Page.Actions.deleteRow( <%#Eval("Id") %>)">
                            [ X ]                   
                            </a>                                   
                        </ItemTemplate>                
                        </telerik:GridTemplateColumn>
                    </Columns>                                
                    </MasterTableView>
                    <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="false" AllowColumnsReorder="False">
                        <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                                                                     
                </telerik:RadGrid>  
                  
                                             
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>

                          
        <div id="pageCompWorkouts" class="innerFlat">         
            <div style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Competition Workouts</h2>
                    
                    <h4>Add Individual MENS Workout </h4>
                    <span>Click the <a href="javascript: ;" onclick="Aqufit.Page.Actions.findWorkout(1);">Add Individual Workout <asp:Image id="Image2" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" /></a> button 
                    to bring up the Workout Finder.  Once you select a workout and click "done" it will added to the list.</span>

                    <h4>Add Individual WOMENS Workout</h4>
                    <span>Click the <a href="javascript: ;" onclick="Aqufit.Page.Actions.findWorkout(3);">Add Individual Workout <asp:Image id="Image5" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" /></a> button 
                    to bring up the Workout Finder.  Once you select a workout and click "done" it will added to the list.</span>

                    <h4>Add Team Workout</h4>
                    <span>Click the <a href="javascript: ;" onclick="Aqufit.Page.Actions.findWorkout(2);">Add Team Workout <asp:Image id="Image3" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" /></a> button 
                    to bring up the Workout Finder.  Once you select a workout and click "done" it will added to the list.</span>

                </div>
            </div>
            <div style="float: right; width: 650px;">
                <asp:Panel ID="Panel1" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                                                 
                <div class="normalButton" onclick="Aqufit.Page.Actions.findWorkout(1);">
                Add Individual MENS Workout
                <asp:Image id="imgAdd" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" />
                </div>
                     
                <telerik:RadGrid ID="RadGrid1" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowGroupPanel="false" 
                        AllowPaging="True" PageSize="25" runat="server" AllowSorting="false" AllowFilteringByColumn="false" AllowAutomaticInserts="false" AllowAutomaticDeletes="true"
                        OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="None">
                    <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <Columns> 
                                
                        <telerik:GridBoundColumn ItemStyle-CssClass="compWodId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                        <telerik:GridBoundColumn ItemStyle-CssClass="compWodId" DataType="System.Int64" SortExpression="WodKey" HeaderText="WodKey" HeaderButtonType="TextButton" DataField="WodKey" Display="false" />    
                        <telerik:GridTemplateColumn ItemStyle-CssClass="workoutName" HeaderText="Workout Name" SortExpression="WorkoutName" HeaderButtonType="TextButton" DataField="WorkoutName" AllowFiltering="false" >
                            <ItemTemplate>
                                <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='<%# Eval("WorkoutName") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn ItemStyle-CssClass="workoutType"  SortExpression="WorkoutType" HeaderText="Type" HeaderButtonType="TextButton" DataField="WorkoutType" Display="true" AllowFiltering="false" />
                        <telerik:GridBoundColumn ItemStyle-CssClass="workoutOrder"  SortExpression="WorkoutOrder" HeaderText="Order" HeaderButtonType="TextButton" DataField="WorkoutOrder" Display="false" AllowFiltering="false" />
                        <telerik:GridTemplateColumn HeaderText="Actions" HeaderButtonType="TextButton">
                        <ItemTemplate>                            
                            <a href="javascript: ;" title="Delete" onclick="Aqufit.Page.Actions.deleteRow( <%#Eval("Id") %>)">
                            [ X ]                   
                            </a>                                   
                        </ItemTemplate>                
                        </telerik:GridTemplateColumn>
                    </Columns>                                
                    </MasterTableView>
                    <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="false" AllowColumnsReorder="False">
                    <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                                                                     
                </telerik:RadGrid>  
                                            
                <br /><br /><br />

                <div class="normalButton" onclick="Aqufit.Page.Actions.findWorkout(3);">
                Add Individual WOMENS Workout
                <asp:Image id="Image4" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" />
                </div>
                     
                <telerik:RadGrid ID="RadGrid3" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowGroupPanel="false" 
                        AllowPaging="True" PageSize="25" runat="server" AllowSorting="false" AllowFilteringByColumn="false" AllowAutomaticInserts="false" AllowAutomaticDeletes="true"
                        OnNeedDataSource="RadGrid3_NeedDataSource" GridLines="None">
                    <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <Columns> 
                                
                        <telerik:GridBoundColumn ItemStyle-CssClass="compWodId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                        <telerik:GridBoundColumn ItemStyle-CssClass="compWodId" DataType="System.Int64" SortExpression="WodKey" HeaderText="WodKey" HeaderButtonType="TextButton" DataField="WodKey" Display="false" />    
                        <telerik:GridTemplateColumn ItemStyle-CssClass="workoutName" HeaderText="Workout Name" SortExpression="WorkoutName" HeaderButtonType="TextButton" DataField="WorkoutName" AllowFiltering="false" >
                            <ItemTemplate>
                                <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='<%# Eval("WorkoutName") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn ItemStyle-CssClass="workoutType"  SortExpression="WorkoutType" HeaderText="Type" HeaderButtonType="TextButton" DataField="WorkoutType" Display="true" AllowFiltering="false" />
                        <telerik:GridBoundColumn ItemStyle-CssClass="workoutOrder"  SortExpression="WorkoutOrder" HeaderText="Order" HeaderButtonType="TextButton" DataField="WorkoutOrder" Display="false" AllowFiltering="false" />
                        <telerik:GridTemplateColumn HeaderText="Actions" HeaderButtonType="TextButton">
                        <ItemTemplate>                           
                            <a href="javascript: ;" title="Delete" onclick="Aqufit.Page.Actions.deleteRow( <%#Eval("Id") %>)">
                            [ X ]                   
                            </a>                                   
                        </ItemTemplate>                
                        </telerik:GridTemplateColumn>
                    </Columns>                                
                    </MasterTableView>
                    <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="false" AllowColumnsReorder="False">
                    <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                                                                     
                </telerik:RadGrid>  
                                            
                <br /><br /><br />
                Selected Pool: 
                <asp:DropDownList id="ddlTeamPools" runat="server" style="font-size: 22px; padding: 3px 10px;">               
                </asp:DropDownList>
                <div id="buttonAddPoolWorkout" runat="server" class="normalButton" onclick="Aqufit.Page.Actions.findWorkout(2);">
                Add Team Pool Workout
                <asp:Image id="Image1" runat="server" ImageUrl="~/DesktopModules/ATI_Base/resources/images/iAdd_s.png" />
                </div>
                <telerik:RadGrid ID="RadGrid2" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowGroupPanel="false"
                            AllowPaging="True" PageSize="25" runat="server" AllowSorting="false" AllowFilteringByColumn="false" AllowAutomaticInserts="false" AllowAutomaticDeletes="true"
                            OnNeedDataSource="RadGrid2_NeedDataSource" GridLines="None">
                        <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="Top">
                        <CommandItemSettings ShowAddNewRecordButton="false" />
                        <Columns> 
                                
                            <telerik:GridBoundColumn ItemStyle-CssClass="compWodId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                            <telerik:GridBoundColumn ItemStyle-CssClass="compWodId" DataType="System.Int64" SortExpression="WodKey" HeaderText="WodKey" HeaderButtonType="TextButton" DataField="WodKey" Display="false" />    
                            <telerik:GridTemplateColumn ItemStyle-CssClass="workoutName" HeaderText="Workout Name" SortExpression="WorkoutName" HeaderButtonType="TextButton" DataField="WorkoutName" AllowFiltering="false" >
                                <ItemTemplate>
                                    <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='<%# Eval("WorkoutName") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn ItemStyle-CssClass="workoutType"  SortExpression="WorkoutType" HeaderText="Type" HeaderButtonType="TextButton" DataField="WorkoutType" Display="true" AllowFiltering="false" />
                            <telerik:GridBoundColumn ItemStyle-CssClass="workoutOrder"  SortExpression="WorkoutOrder" HeaderText="Order" HeaderButtonType="TextButton" DataField="WorkoutOrder" Display="false" AllowFiltering="false" />
                            <telerik:GridTemplateColumn HeaderText="Actions" HeaderButtonType="TextButton">
                            <ItemTemplate>                                
                                <a href="javascript: ;" title="Delete" onclick="Aqufit.Page.Actions.deleteRow( <%#Eval("Id") %>)">
                                [ X ]                   
                                </a>                                   
                            </ItemTemplate>                
                            </telerik:GridTemplateColumn>
                        </Columns>                                
                        </MasterTableView>
                        <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="false" AllowColumnsReorder="False">
                        <Selecting AllowRowSelect="True"></Selecting>
                        </ClientSettings>
                                                                     
                    </telerik:RadGrid>         
                                            
                    </asp:Panel>  
            </div> 
            <br style="clear: both;" />	              
            </div>  
            
            
            
            <div id="pageRegistration" class="innerFlat">
             <div runat="server" id="Div2" style="float: left; width: 220px;" >
                <div class="groupCreateHelp">
                    <h2>Competition Registration Help</h2>                    
                </div>
            </div>
            <div style="float: right; width: 650px;">
                <asp:Panel ID="Panel2" runat="server" CssClass="atiGroupSettins grad-FFF-EEE rounded">
                <fieldset>
                <dl> 
                   
                    <dt><asp:Label ID="Label2" runat="server" controlname="txtCompName" Text="Comming soon.." /></dt>
                    <dd>
                    </dd>
                    
                </dl>
                </fieldset>                                
                </asp:Panel>
            </div>
            <br style="clear: both;" />
        </div>  
            	          			
    	</div>
        <!-- END Tabs -->                       
         
</div>   
<div style="clear:both;"></div>  
      








    

    

         
    
               



