<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_CompLiveScore.ViewATI_CompLiveScore" CodeFile="ViewATI_CompLiveScore.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="TimeSpan" Src="~/DesktopModules/ATI_Base/controls/ATI_TimeSpan.ascx" %>
<%@ Register TagPrefix="ati" TagName="FeaturedProfile" Src="~/DesktopModules/ATI_Base/controls/ATI_FeaturedProfile.ascx" %>
<%@ Register TagPrefix="ati" TagName="CompetitionAthlete" Src="~/DesktopModules/ATI_Base/controls/ATI_CompetitionAthlete.ascx" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">     
<script type="text/javascript" >

    function CompWOD(id, name, type, compType) {
        this.id = id;
        this.name = name;
        this.type = type;
        this.compType = compType;
    };

    Aqufit.Page.Controls.ATI_GridHelper = function () {
        this.masterTableView = $find("<%=RadGridLiveScore.ClientID %>").get_masterTableView();
        this.items = this.masterTableView.get_dataItems();
        this.numRows = this.items.length;
        this.selectedItem = 0;
        this.$atiUserCard = $('#atiUserCard');
        this.hash = [];
        this.$pre = $('#pre');
        this.loadBenchmarks();

    };

    Aqufit.Page.Controls.ATI_GridHelper.prototype = {
        loadBenchmarks: function () {
            var that = this;
            for (var i = 0; i < this.numRows; i++) {
                var item = this.items[i];
                var usid = $('#' + item.get_id() + ' .UserSettingKey').html();

            }
        },
        rebind: function(){
            this.masterTableView.rebind();    
        },
        advanceTour: function () {
            
            if (this.selectedItem > 0) {
                var last = this.items[this.selectedItem - 1];
                last.set_selected(false);
            }
            if (this.selectedItem < (this.numRows - 1)) {
                var next = this.items[this.selectedItem + 1];
                var usKey = $('#' + next.get_id() + ' .UserSettingKey').html();
                this.$pre.attr('src', 'http://flexfwd.com/DesktopModules/ATI_Base/services/images/profile.aspx?us=' + usKey + '&f=1');
            }
            this.selectInd(this.selectedItem);
            
        },
        selectInd: function(ind){
            var that = this;
            var item = this.items[ind];
            
            item.set_selected(true);
            var $row = $('#' + item.get_id());
            <% if(Request["a"] != null ){ %>
            if ($row.offset().top > _scrollFocusTop) {
                $('#<%=scrollPanel.ClientID %>').scrollTop($('#<%=scrollPanel.ClientID %>').scrollTop() + $row.height());
            }
            <%} %>
            var name = $row.find('.compAthlete a').html();
            var usid = $row.find('.UserSettingKey').html();
            var affiliate = $row.find('.compAffilate').html();
            var rank = $row.find('.compRank').html();
            var score = $row.find('.compScore').html();
            if (!this.hash[usid]) {                
                Affine.WebService.StreamService.GetUserCompetitionCardData(usid, function (data) {
                    var card = eval('(' + data + ')');
                    that.hash[usid] = card;
                    that.loadUserData(0, usid, name, affiliate, rank, score, [], []);
                });
            } else {
                this.loadUserData(0, usid, name, affiliate, rank, score, [], []);
            }
        },
        loadUserData: function (aid, usid, name, affiliate, overallRank, overallScore, rankArray, scoreArray) {
            var nameAka = name;
            if (this.hash[usid]) {
                nameAka += '<br />(a.k.a. ' + this.hash[usid].UserName + ')';
            }
            this.$atiUserCard.find('h1').html(nameAka);
            this.$atiUserCard.find('#aAffiliate').html(affiliate);
            this.$atiUserCard.find('#aRank strong').html(overallRank);
            this.$atiUserCard.find('#aScore strong').html(overallScore);
            this.$atiUserCard.find('#atiProfileImg').attr('src', 'http://flexfwd.com/DesktopModules/ATI_Base/services/images/profile.aspx?us=' + usid + '&f=1');
            if (this.hash[usid]) {
                var ucard = this.hash[usid];
                this.$atiUserCard.find('#wsFran').html(ucard.FranTime > 0?Aqufit.Utils.toDurationString(ucard.FranTime):'');
                this.$atiUserCard.find('#wsHelen').html(ucard.HelenTime > 0?Aqufit.Utils.toDurationString(ucard.HelenTime):'');
                this.$atiUserCard.find('#wsFilthy50').html(ucard.FilthyFiftyTime > 0?Aqufit.Utils.toDurationString(ucard.FilthyFiftyTime):'');
                this.$atiUserCard.find('#wsFgb').html(ucard.FGBScore > 0?ucard.FGBScore:'');
                this.$atiUserCard.find('#wsGrace').html(ucard.GraceTime > 0?Aqufit.Utils.toDurationString(ucard.GraceTime):'');
                this.$atiUserCard.find('#wsMaxBs').html(ucard.BackSquatMax > 0?Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, ucard.BackSquatMax, Aqufit.Units.UNIT_LBS), 2) + ' lbs.':'');
                this.$atiUserCard.find('#wsMaxPullups').html(ucard.PullupMax > 0?ucard.PullupMax:'');
                this.$atiUserCard.find('#wsMaxSnatch').html(ucard.SnatchMax > 0?Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, ucard.SnatchMax, Aqufit.Units.UNIT_LBS), 2) + ' lbs.':'');
                this.$atiUserCard.find('#wsMaxClean').html(ucard.CleanMax > 0?Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, ucard.CleanMax, Aqufit.Units.UNIT_LBS), 2) + ' lbs.':'');
                this.$atiUserCard.find('#wsMaxDl').html(ucard.DeadliftMax > 0?Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, ucard.DeadliftMax, Aqufit.Units.UNIT_LBS), 2) + ' lbs.':'');
            }
            var that = this;
            var speed = <%=Request["ts"] != null ? Request["ts"] : "2500" %>;
            this.$atiUserCard.fadeIn(500, function () {
                <% if(Request["a"] != null ){ %>
                setTimeout(function () {
                    that.$atiUserCard.fadeOut(500, function () {                       
                        that.nextItem();                        
                    });
                }, speed);
                <% } %>
            });
        },
        nextItem: function () {
            if (this.selectedItem < this.numRows) {
                this.selectedItem++;
                this.advanceTour();
            }else{               
                this.rebind();       // this auto switches on rebind.. (male, female, team pools)                 
            }
        }
    };

    Aqufit.Page.Controls.ATI_TwitterHashFeed = function () {
        this.hash = '';
        this.results = {};
    }

    Aqufit.Page.Controls.ATI_TwitterHashFeed.prototype = {
        getFeed: function (hash) {
            this.hash = hash;
            var that = this;
            Affine.WebService.StreamService.GetTwitterFeedForHash(this.hash, function (data) {
                var json = eval('(' + data + ')');
                that.results = json.results;
                var $ticker = $('#ticker');
                $ticker.empty();
                $ticker.append('<li><span style="font-wieght: bold;">Tweet using hash tag #cftaranis</span> <a href="#"> - Get your message on the board by tweeting with hash tag <b>#cftaranis</b>.</a>');
                for (var i = 0; i < that.results.length; i++) {
                    var tweet = that.results[i];
                    $ticker.append('<li><img style="float: left; padding-right: 10px;" src="' + tweet.profile_image_url + '" align="top" /><span><a href="http://twitter.com/' + tweet.from_user + '">' + tweet.from_user + '</a> - ' + tweet.text + '</span></li>');
                }
            });
        }
    };

    Aqufit.Page.Actions = {
        ShowFail: function (msg) {
            radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iError.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
        },
        ShowSuccess: function (msg) {
            radalert('<div style="width: 100%; height: 100%; padding: 0px;"><span style="color: #FFF;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCheck.png")%>" align="absmiddle"/> ' + msg + '</span></div>', 330, 100, 'Problem');
        },
        OnResponseEnd: function () {
            Aqufit.Page.atiLoading.remove();
        },
        TableCreated: function () {
            Aqufit.Page.GridHelper = new Aqufit.Page.Controls.ATI_GridHelper();
            <% if( Request["a"] != null ){ %>
            Aqufit.Page.GridHelper.advanceTour();     
            <% }else{ %>
            Aqufit.Page.GridHelper.selectInd(0);
            <%} %>       
        },
        RowClick: function(sender, args){
            Aqufit.Page.GridHelper.selectInd( args.get_itemIndexHierarchical() );
        }
    };
    

    function fiterTest() {
        var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
        masterTable.filter("AthleteName", "Core", Telerik.Web.UI.GridFilterFunction.Contains, true);
    }


    $(function () {
        $('.dull').focus(function () {
            $(this).val('').removeClass('dull').unbind('focus');
        });
        $('#tabsAdmin').tabs();
        $('#tabsScore').tabs();

        $('._compScoreField').blur(function () {
            var score = $(this).val();
            var athleteKey = $(this).parents('tr').find('.athleteId').html();
            $('#<%=hiddenAjaxAction.ClientID %>').val('updateAthleteScore');
            $('#<%=hiddenAjaxValue.ClientID %>').val(athleteKey);
            $('#<%=hiddenAjaxValue2.ClientID %>').val(score);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$CompLiveScore","ATI_CompLiveScore") %>', '');
        });
        Aqufit.Page.TwitterHelper = new Aqufit.Page.Controls.ATI_TwitterHashFeed();
        Aqufit.Page.TwitterHelper.getFeed('cftaranis');
        setInterval(function(){ Aqufit.Page.TwitterHelper.getFeed('cftaranis'); }, 240000);                

        $('#<%=bAdvance.ClientID %>').button();
        $('#<%=bBack.ClientID %>').button();

        var ticker = function () {
            setTimeout(function () {
                $('#ticker li:first').animate({ marginTop: '-65px' }, 800, function () {
                    $(this).detach().appendTo('ul#ticker').removeAttr('style');
                });
                ticker();
            }, 6000);
        };
        ticker();

        $('#<%=ddlCompSelector.ClientID %>').change(function(){
            var json = eval( '('+$('#<%=ddlCompSelector.ClientID %> :selected').val()+')' );
            $('#<%=hiddenAjaxCompCategoryKey.ClientID %>').val(json.cat);
            $('#<%=hiddenTeamPoolKey.ClientID %>').val(json.tp);
            Aqufit.Page.GridHelper.rebind();
        });

        <% if( Request["a"] != null ){ %>
        $(window).resize(function () {
            _wndHeight = $(window).height();
            _scrollPaneTop = $('#<%=scrollPanel.ClientID %>').offset().top;
            var scaleHeight = _wndHeight - _scrollPaneTop - 10;
            _scrollFocusTop = _scrollPaneTop + (scaleHeight / 2);
            $('#<%=scrollPanel.ClientID %>').css('height', scaleHeight + 'px');
        });
        $(window).trigger('resize');
        <% } %>
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
html body div ul.hlist > li
{
	display: inline;
}
.live-leader-item
{
	height: 40px;
}
<% if( Request["a"] != null ){%>
.compScore span,
.projector
{
	font-size: 22px !important;	
}
<%}%>
.compScore span
{
	padding: 0px 10px;
}
.bold
{
	font-weight: bold;
}
.wstats li
{
	width: 250px;
	font-size: 14px;
	padding: 1px 20px;
}
.wstats li:nth-child(even) {background: #EEE}
.wstats li:nth-child(odd) {background: #FFF}

.scrollPanel
{
	overflow: auto; 
	height: 550px;
}


.wstats li span
{
	float: right;
	font-weight: bold;
	font-size: 15px;
}
#atiUserCard table td
{
	padding: 0px 20px;
}
#aRank, #aScore
{
	display: block;
	padding: 10px;
	margin: 5px 0px;
	font-size: 20px;
	border: 1px solid #666;
}
ul#ticker {
    width: 100%;
    height: 60px;
    overflow: hidden;
}
 
ul#ticker  li {
    width: 100%;
    height: 50px;
    padding: 10px;
    border-bottom: 1px dashed #ccc;
}
 
ul#ticker li a {
    color: #666;
    font-size: 18px;
}
 
ul#ticker li span {
   
    color: #06C;
    font-size: 20px;
}

.ddlOptions
{
	font-size: 24px;
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
        <telerik:AjaxSetting AjaxControlID="RadGridLiveScore">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadGridLiveScore" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                
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
    <asp:HiddenField ID="hiddenCompWodKey" runat="server" />
    <asp:HiddenField ID="hiddenTeamPoolKey" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" OnClick="bAjaxPostback_Click" style="display: none;" />
</asp:Panel>

<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>            
        <telerik:RadWindow ID="WorkoutFinder" runat="server" Skin="Black" Title="Workout Finder" Width="600" Height="300" VisibleOnPageLoad="false" Behaviors="Move, Close" EnableShadow="true" Modal="true">
            <ContentTemplate>
                <div id="workoutStatusPanel" style="width: 100%; height: 100%; background-color: white; position: relative;">
                    <div class="atiListHeading grad-FFF-EEE" style="position: relative;">
                        <h3 id="wsTitle">Workout Finder</h3>                       
                    </div>
                    <div style="padding: 30px 10px;">
                        <center>                       
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
    </Windows>
</telerik:radwindowmanager>
  
  <img alt="" id="pre" style="position: absolute; top: -1000px; left: -1000px;" />
<div id="divCenterWrapper" style="width: 100%">
<asp:Panel ID="panelAdmin" runat="server" Visible="false">
      
       <!-- Tabs -->
        <div id="tabsAdmin">
    		<ul>
                <li id="tabViewScoreAdmin"><a href="#pageViewScoreAdmin">Competitions Score Keeping</a></li>   
            </ul>  
            
            <div id="pageViewScoreAdmin" class="innerFlat" style="padding:0px;">
                <div class="atiSearchControls grad-FFF-EEE">
                    <asp:Button ID="bBack" runat="server" Text="Back Competition" OnClick="bBack_Click" style="float: left;" />
                    <asp:Button ID="bAdvance" runat="server" Text="Advance Competition" OnClick="bAdvance_Click" style="float: right;" />
                    
                    <center><h1 style="display: inline-block;"><asp:Literal ID="litWodName" runat="server" /></h1></center>
                    
                </div>
                <div class="atiSearchControls grad-FFF-EEE">
                    <table style="width: 100%;">
                    <tr>
                        <td>
                        <asp:DropDownList ID="ddlCompetitionType" runat="server" OnSelectedIndexChanged="ddlCompetitionType_SelectedIndexChanged" AutoPostBack="true" style="margin: 0px auto; font-size: 20px;">
                        </asp:DropDownList>

                        <asp:DropDownList ID="ddlTeamPool" runat="server" style="margin: 0px auto; font-size: 20px;" OnSelectedIndexChanged="ddlTeamPool_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                        </td>
                        <td>
                         <!-- TODO: another way to choose the following:
                         TODO: fast filter by name, <br />
                show male/female/both<br />
                show only people with no score for workout X<br />
                Team / Individual picker <br />
                         -->
                          
                        </td>
                    </tr>
                    </table>
                </div>
                <telerik:RadGrid ID="RadGrid1" Width="100%" AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowGroupPanel="false" 
                        AllowPaging="True" PageSize="250" runat="server" AllowSorting="true" AllowFilteringByColumn="true" AllowAutomaticInserts="false" AllowAutomaticDeletes="true" 
                        OnNeedDataSource="RadGrid1_NeedDataSource" GridLines="None" OnItemDataBound="RadGrid1_ItemDataBound">
                    <MasterTableView Width="100%" DataKeyNames="Id" CommandItemDisplay="None">
                    <CommandItemSettings ShowAddNewRecordButton="false" />
                    <Columns>                                 
                        <telerik:GridBoundColumn ItemStyle-CssClass="athleteId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                        <telerik:GridTemplateColumn ItemStyle-CssClass="athleteName" HeaderStyle-Width="250px" HeaderText="Athlete Name" UniqueName="AthleteName" SortExpression="AthleteName" HeaderButtonType="TextButton" DataField="AthleteName" AutoPostBackOnFilter="true" FilterControlWidth="200px"  CurrentFilterFunction="Contains" ShowFilterIcon="false" >
                            <ItemTemplate>
                                <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" Text='<%# Eval("AthleteName") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn ItemStyle-CssClass="athleteSex"  SortExpression="Sex" HeaderText="Sex" HeaderButtonType="TextButton" DataField="Sex" Display="true" AutoPostBackOnFilter="true" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                        <telerik:GridBoundColumn ItemStyle-CssClass="athleteRank"  SortExpression="Rank" HeaderText="Rank" HeaderButtonType="TextButton" DataField="Rank" Display="true" AllowFiltering="false" />
                        <telerik:GridBoundColumn ItemStyle-CssClass="athleteScore"  SortExpression="Score" HeaderText="Score" HeaderButtonType="TextButton" DataField="Score" Display="true" AllowFiltering="false" />
                        <telerik:GridBoundColumn ItemStyle-CssClass="athleteWodScore"  SortExpression="WodScore" HeaderText="WodScore" HeaderButtonType="TextButton" DataField="WodScore" Display="true" AllowFiltering="false" />
                                        
                        <telerik:GridTemplateColumn HeaderText="Actions" SortExpression="Actions" HeaderButtonType="TextButton" AllowFiltering="false">
                        <ItemTemplate>                           
                            <asp:Literal id="litDebug" runat="server" Visible="false" />                                       
                            
                            <asp:TextBox id="atiTxtScore" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox _compScoreField" Visible="false" />                                       
                            <ati:UnitControl id="atiMaxWeightUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="weight" Visible="false" />  
                            <asp:RegularExpressionValidator ID="revTime" runat="server" ControlToValidate="atiTxtScore" ValidationExpression="^\d+(:\d+)?(:\d+)?$|hh:mm:ss" Display="Static" SetFocusOnError="true" ErrorMessage="Must be a time mm:ss" Text="Must be a time mm:ss" /> 
                            <asp:RegularExpressionValidator ID="revReal" runat="server" ControlToValidate="atiTxtScore" ValidationExpression="^[+-]?\d+(\.\d+)?$" Display="Static" SetFocusOnError="true" ErrorMessage="Must be Real number (eg: 3.14159)" Text="* Must be Real number (eg: 3.14159)" />                                                   
                        </ItemTemplate>                
                        </telerik:GridTemplateColumn>
                    </Columns>                                
                    </MasterTableView>
                    <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="false" AllowColumnsReorder="False">
                    <Selecting AllowRowSelect="True"></Selecting>
                    </ClientSettings>
                                                                     
                </telerik:RadGrid>
                
            </div>                        	          			
    	</div>
        <!-- END Tabs -->                       
</asp:Panel>
 
 
<asp:Panel ID="panelLiveScore" runat="server">
<asp:HiddenField ID="hiddenAjaxCompCategoryKey" runat="server" />
<asp:HiddenField ID="hiddenAjaxTeamPoolKey" runat="server" />
        <div id="tabsScore">
    		<ul>
                <li><a href="#pageViewInfo">Competitions Details</a></li>   
            </ul>            
            <div id="pageViewInfo" style="padding: 0px; background-color: White; _clear:both;">
                <asp:Panel ID="atiWorkoutPanel" runat="server">                        
                    <div id="atiWorkoutSearch" runat="server">
                        <div class="atiSearchControls grad-FFF-EEE">                                                          
                            <div style="border: 2px solid #ccc;">
                                <div style="height: 200px; overflow: hidden;">
                                    <div id="atiUserCard" style="margin: auto;">
                                        <center>
                                        <table>
                                        <tr valign="top">
                                            <td><img id="atiProfileImg" src="" style="border: 1px solid #000;" /></td>
                                            <td style="width: 300px;">
                                                <ul class="vlist">
                                                    <li><h1>Athlete Name</h1></li>
                                                    <li><span id="aAffiliate"></span></li>
                                                    <li><span id="aRank">Rank: <strong></strong></span></li>
                                                    <li><span id="aScore">Score: <strong></strong></span></li>
                                                </ul>
                                            </td>
                                            <td>
                                                <ul class="vlist wstats">
                                                    <li>Fran: <span id="wsFran"></span></li>
                                                    <li>Helen: <span id="wsHelen"></span></li>
                                                    <li>Filthy 50: <span id="wsFilthy50"></span></li>
                                                    <li>FGB: <span id="wsFgb"></span></li>
                                                    <li>Grace: <span id="wsGrace"></span></li>
                                                    <li>Back Squat: <span id="wsMaxBs"></span></li>
                                                    <li>Pull-ups: <span id="wsMaxPullups"></span></li>
                                                    <li>Snatch: <span id="wsMaxSnatch"></span></li>
                                                    <li>Clean: <span id="wsMaxClean"></span></li>
                                                    <li>Deadlift: <span id="wsMaxDl"></span></li>
                                                </ul>
                                            </td>
                                        </tr>
                                        </table>
                                        </center>
                                    </div>
                                </div>                                                    
                            </div>                            
                        </div>                            
                    </div>
                    <div id="orderByPanel" class="grad-FFF-EEE" style="position: relative; display: block; height: 90px;">                                                          
                         
                    <ul id="ticker">
                        <li>
                            <span>Tweet using hash tag #cftaranis</span><a href="#"> - Get your message on the board by tweeting with hash tag #cftaranis.</a>
                        </li>  
                    </ul>
                         
                    <h1 id="compCategoryName" style="padding: 2px 10px 0px 10px; border-top: 1px solid #666; margin-bottom: 0px;">-- Mens Individuals ---</h1>                      
                    </div>   
                    <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="LeaveTargetAndToolTip"
                        Width="350" Height="250" runat="server" EnableShadow="true" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element"
                        Position="MiddleRight">
                    </telerik:RadToolTipManager>
                    <asp:DropDownList ID="ddlCompSelector" runat="server" CssClass="ddlOptions" />
                    <div id="scrollPanel" runat="server">
                        <telerik:RadGrid ID="RadGridLiveScore" Width="100%"  AutoGenerateColumns="false" AllowMultiRowSelection="false" ShowGroupPanel="False"  OnItemCommand="RadGridLiveScore_ItemCommand"
                                AllowPaging="True" PageSize="100" runat="server" AllowSorting="true" AllowFilteringByColumn="false" OnItemDataBound="RadGridLiveScore_ItemDataBound"
                                OnNeedDataSource="RadGridLiveScore_NeedDataSource" GridLines="None"
                                ItemStyle-CssClass="live-leader-item" AlternatingItemStyle-CssClass="live-leader-item"
                                ClientSettings-ClientEvents-OnRowClick="Aqufit.Page.Actions.RowClick"
                                ClientSettings-ClientEvents-OnMasterTableViewCreated="Aqufit.Page.Actions.TableCreated">
                                <ExportSettings HideStructureColumns="false" />
                            <MasterTableView Width="100%" DataKeyNames="Id" Height="200px" CommandItemDisplay="Top">
                            <CommandItemSettings ShowExportToWordButton="true" ShowExportToExcelButton="true" ShowExportToCsvButton="true" ShowAddNewRecordButton="false" />
                            <Columns> 
                                <telerik:GridTemplateColumn ItemStyle-CssClass="compFlex projector" HeaderText="" SortExpression="FlexId" HeaderButtonType="TextButton" DataField="FlexId" AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:Label ID="numberLabel" runat="server" />                                    
                                </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn ItemStyle-CssClass="compId" DataType="System.Int64" SortExpression="Id" HeaderText="Id" HeaderButtonType="TextButton" DataField="Id" Display="false" />    
                                <telerik:GridBoundColumn ItemStyle-CssClass="UserSettingKey" DataType="System.Int64" SortExpression="UserSettingKey" HeaderText="UserSettingKey" HeaderButtonType="TextButton" DataField="UserSettingKey" Display="false" />                                
                                <telerik:GridBoundColumn SortExpression="RegionKey" HeaderText="RegionKey" HeaderButtonType="TextButton" DataField="RegionKey" Display="false" />
                                <telerik:GridBoundColumn SortExpression="Sex" HeaderText="Sex" HeaderButtonType="TextButton" DataField="Sex" Display="false" />                                
                                <telerik:GridBoundColumn ItemStyle-CssClass="compHeight"  SortExpression="Height" HeaderText="Height" HeaderButtonType="TextButton" DataField="Height" Display="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compWeight"  SortExpression="Weight" HeaderText="Weight" HeaderButtonType="TextButton" DataField="Weight" Display="false" />
                                <telerik:GridBoundColumn SortExpression="Country" HeaderText="Country" HeaderButtonType="TextButton" DataField="Country" Display="false" />
                                <telerik:GridTemplateColumn ItemStyle-CssClass="compAthlete projector" HeaderText="Athlete" SortExpression="AthleteName" HeaderButtonType="TextButton" DataField="AthleteName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="javascript: ;" CssClass="projector" Text='<%# Eval("AthleteName") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn ItemStyle-CssClass="compAge" SortExpression="Age" HeaderText="Age" HeaderButtonType="TextButton" DataField="Age" Display="true" HeaderStyle-Width="25px" AllowFiltering="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compAffilate projector" SortExpression="AffiliateName" HeaderText="Affiliate" HeaderButtonType="TextButton" DataField="AffiliateName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"  />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compRank projector bold" SortExpression="OverallRank" HeaderText="Rank" HeaderButtonType="TextButton" DataField="OverallRank" AllowFiltering="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compScore projector" SortExpression="OverallScore" HeaderText="Score" HeaderButtonType="TextButton" DataField="OverallScore" AllowFiltering="false" />
                                <telerik:GridBoundColumn ItemStyle-CssClass="compScore projector" SortExpression="WodRankScore" HeaderText="Rank / WOD" HeaderButtonType="TextButton" DataField="WodRankScore" AllowFiltering="false" />                                                                                                
                            </Columns>                                
                            </MasterTableView>
                            <ClientSettings ReorderColumnsOnClient="False" AllowDragToGroup="True" AllowColumnsReorder="False">
                            <Selecting AllowRowSelect="True"></Selecting>
                            </ClientSettings>
                            <PagerStyle Mode="NextPrevAndNumeric" Position="TopAndBottom" />    
                            <GroupingSettings CaseSensitive="false" />                                             
                        </telerik:RadGrid>
                    </div>                    
                </asp:Panel> 
                
            </div>                     	          			
    	</div>
</asp:Panel>    

</div>  
      








    

    

         
    
               



