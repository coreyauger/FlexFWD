<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_NikePlus.ViewATI_NikePlus" CodeFile="ViewATI_NikePlus.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="Login" Src="~/DesktopModules/ATI_Base/controls/ATI_Login.ascx" %>
<%@ Register TagPrefix="ati" TagName="Stream" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script type="text/javascript" src="http://developer.garmin.com/web/communicator-api/prototype/prototype.js">&#160;</script> 
<script type="text/javascript" src="http://flexfwd.com/DesktopModules/ATI_Base/resources/scripts/garmin.js">&#160;</script> 
<script language="javascript" type="text/javascript">
    function RequestStart(sender, eventArgs) {       
    }
    function ResponseEnd(sender, eventArgs) {
    }  

    Aqufit.addLoadEvent(function () {
        var deviceUI = new GarminDeviceUI();
        var garminControl = new GarminControl(deviceUI, { "timestamp": 0, "url": "http://flexfwd.com", "key": "159065d5a29436f5d51f5d4429154758", "parse_route": true });
    });

    var GarminDeviceUI = Class.create({
        SYNCING_CLASS: 'sync_in_progress',
        garminSyncLink: null,

        initialize: function () {
            this.statusContainer = $('syncing_device_status');
            this.syncGarminLoader = $('syncing_device_garmin');

            this.nikePlusSyncLink = $('sync_nike_plus_link');
            this.garminSyncLink = $('sync_garmin_link');
            this.deviceSelector = $('garmin_device_selector');
        },

        showGarminSyncing: function () {
            this._resetUI();
            if (this.garminSyncLink) {
                this.garminSyncLink.hide().next().show();
            }
            this.updateGarminProgress(0); // reset progress bar
            this.syncGarminLoader.addClassName(this.SYNCING_CLASS);
        },

        hideGarminDeviceSelector: function () {
            this.deviceSelector.hide();
            //.select('a').each(function(c) { c.remove(); });
        },

        showNikePlusSyncing: function () {
            if (this.nikePlusSyncLink) {
                this.nikePlusSyncLink.hide().next().show();
            }
        },

        clearGarminDeviceSelectorElements: function () {
            this.deviceSelector.select('a.garmin_device').each(function (c) {
                c.remove();
            });
        },

        addGarminDeviceSelectorElement: function (elt) {
            this.deviceSelector.insert({ bottom: elt });
            this.deviceSelector.show();
        },

        hideSyncing: function () {
            if (this.garminSyncLink) {
                this.garminSyncLink.show().next().hide();
            }
            if (this.nikePlusSyncLink) {
                this.nikePlusSyncLink.show().next().hide();
            }
            this.syncGarminLoader.removeClassName(this.SYNCING_CLASS);
        },

        updateGarminProgress: function (percent) {
            if (!this.dataProgressBar) { this.dataProgressBar = $('data_progress_bar'); }
            if (!this.dataProgressNumber) { this.dataProgressNumber = $('data_progress_number'); }

            this.dataProgressBar.setStyle({ width: parseInt(percent * 6.5) + 'px' });
            this.dataProgressNumber.update(percent);
        },

        showError: function (msg) {
            this._showStatus(msg, true);
        },

        showNotice: function (msg) {
            this._showStatus(msg);
        },

        browserNotSupported: function () {
            this.showError("Your browser isn't supported by the Garmin Communicator Plugin.");
        },

        pluginNotInstalled: function () {
            this.showError('Please <a href="http://www.garmin.com/products/communicator/">install the Garmin Communicator Plugin</a>.');
        },

        _hideStatus: function () {
            this.statusContainer.hide();
        },

        _showStatus: function (msg, error) {
            var removeClass = 'error', addClass = 'notice';
            if (error) {
                removeClass = 'notice';
                addClass = 'error';
            }
            this._resetUI();
            // HACK: this.statusContainer.down('p.message') doesn't work
            $('syncing_device_status_p').update('<span class="icon">&nbsp;</span>' + msg);
            this.statusContainer.show().down().removeClassName(removeClass).addClassName(addClass);
        },

        _resetUI: function () {
            this._hideStatus();
            this.hideGarminDeviceSelector();
            this.hideSyncing();
        }
    });

  
 </script>
 <style type="text/css">
 div.accountHead
 {
 	padding: 20px;
 }
 div.accountHead img 
 {
 	float: right;
 	margin-right: 40px;
 }
 div.accountLogin
 {
 	width: 500px;
 	border: 1px solid #ccc;
 	margin: auto;
 	padding: 40px;
 	
 }
 ul#sync_device_list{
    padding: 10px 0px;
 }
 span#data_progress_bar{
    width: 100%;
    display: block;
    text-align: center;
    
    background-color: #FFF;
    border: 1px solid #CCC;
 }
 span#data_progress_bar span{
    font-size: 20px;
 }
 </style>
</telerik:RadCodeBlock>



<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="atiAjaxPanel">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="atiAjaxPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>    
    <ClientEvents OnRequestStart="RequestStart" />
    <ClientEvents OnResponseEnd="ResponseEnd" />
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />

<div id="atiTopMarker" style="width: 100%; height: 1px;"></div>
     <div style="width: 728; height: 90px; background-color: #CCCCCC; border: 1px solid #666666; text-align: center;">
        AD UNIT
    </div>
    
    <div class="accountHead">
        <img id="imgLogo" runat="server" />
        <asp:Literal ID="litAccountName" runat="server" />       
    </div>   

    
    <asp:Panel ID="atiAjaxPanel" runat="server">     
         <asp:Panel id="panelGarmin" runat="server" Visible="false">
        <div class="grad-FFF-EEE rounded accountLogin">
        <div id="garminDisplay"/> 

        <div>
      <div id="syncing_device_status" class="flash" style="display: none;">
        <div>
          <p id="syncing_device_status_p" class="message">
            <span class="icon">&nbsp;</span>
          </p>
        </div>
        <div class="clearl empty_div"></div>
      </div>
      <div id="syncing_device_garmin" class="syncing_device_container">
        <div id="device_data_loading">
          <span id="data_progress_bar">
            <span id="data_progress_number">0</span>
          </span>
        </div>
      </div>
      <div id="garmin_device_selector" style="display: none">
        <strong>Please select the Garmin device you would like to import from:</strong>
      </div>
      <ul id="sync_device_list">
        <li id="garmin_import_listitem" class="faux_input full" style="font-size: 100%; list-style: none;">
          <div class="yui-ge">
            <div class="yui-u first">
              <div class="icon"></div>
              <div class="left">
                <strong class="block">Garmin</strong>
                <span class="grey smaller">Account: <asp:Literal ID="litGarminAccountName" runat="server" /></span>
              </div>
            </div>
            <div class="yui-u">
              <a id="sync_garmin_link" class="ati_Form_Button" style="text-decoration: none; font-weight: bold; padding-top: 7px; cursor: pointer;">Sync Garmin</a><span style="display: none; color: #888; line-height: 40px;"><img alt="Spinner" id="imgSpinner" runat="server" /> Syncing...</span>
            </div>
          </div>
        </li>        
      </ul>      
    </div>
    </div>
    <asp:Button ID="Button1" runat="server" Text="Change Account" OnClick="bChangeAccount_Click" CssClass="ati_Form_ButtonGrey" />
    </asp:Panel>

    <asp:HiddenField ID="hiddenAccountType" runat="server" />
    <div class="grad-FFF-EEE rounded accountLogin" id="panelLogin" runat="server">
        <asp:Label ID="lStatus" runat="server" />
        <asp:Literal ID="litInstructions" runat="server" />
        <ati:Login ID="atiLogin" runat="server" ShowForgotPassword="false" />
        <asp:Button ID="bLogin" runat="server" Text="Login" OnClick="bLogin_Click" CssClass="ati_Form_Button" />        
    </div>
    <div style="padding: 10px 20px;" id="panelStream" runat="server">
        <!-- HACK: clean this up and do a progress indicator -->
        <div class="grad-FFF-EEE rounded accountLogin" style="margin-bottom: 20px;">
            <asp:Literal ID="litStatus" runat="server" />
        </div>
        <asp:Button ID="bChangeAccount" runat="server" Text="Change Account" OnClick="bChangeAccount_Click" CssClass="ati_Form_ButtonGrey" />
        <asp:Button ID="bSaveAndClose" runat="server" Text="Save & Close" OnClick="bSaveAndClose_Click" CssClass="ati_Form_Button" />        
        <div style="display: none;"> 
            <ati:Stream ID="atiStream" runat="server" AllowPaging="false" />
        </div>
    </div>
</asp:Panel>
<div id="atiBottomMarker" style="width: 100%; height: 1px;"></div>

    

    

         
    
               



