<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Viral.ViewATI_Viral" CodeFile="ViewATI_Viral.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="FindInvite" Src="~/DesktopModules/ATI_Base/controls/ATI_FindInvite.ascx" %>
<%@ Register TagPrefix="ati" TagName="FeaturedGroup" Src="~/DesktopModules/ATI_Base/controls/ATI_FeaturedGroup.ascx" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<link href="<%=ResolveUrl("~/DesktopModules/ATI_Friends/resources/css/friends.css")%>" type="text/css" rel="stylesheet" />
<script language="javascript" type="text/javascript">         

    Aqufit.Windows.ContactsWin = {
        win: null,
        open: function (uri) {
            this.win = window.radopen(uri, null);
            this.win.set_modal(true);
            this.win.setSize(747, 600);
            this.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            this.win.center();
            this.win.show();
            return false;
        }
    };
    Aqufit.Page.Actions = {
        SendFriendRequest: function (fid) {
            $('#<%=hiddenAjaxAction.ClientID %>').val('friendRequest');
            $('#<%=hiddenAjaxValue.ClientID %>').val(fid);
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Viral","ATI_Viral") %>', '');
        }
    };

    $(function () {
        $('#tabs').tabs();
        $('#atiStatusWidget').hide();
    }); 
    
   
</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnRequestStart="OnRequestStart" OnResponseEnd="OnResponseEnd" />
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
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

<div style="position: relative;">
    <div id="atiStatusWidget" class="ui-widget">
	    <div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		    <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		    <span id="statusMsg"></span>
	    </div>
    </div>

    <!-- Start of a 3 col box layout -->    
    <div id="divCenterWrapper" runat="server" class="floatRight" style="width: 550px; margin-right: 15px; min-height: 650px; margin-right: 177px;">               
        <!-- Tabs -->
        <div id="tabs">
    		<ul>
    			<li id="tabFindFriends" runat="server"><a href="#<%=pageViewFindFriends.ClientID %>">Find/Invite Friends</a></li>                      
            </ul>    			                                               

            <!-- TAB Find Friends -->
    		<div id="pageViewFindFriends" runat="server" style="padding: 0px; background-color: White;">                    
                
                <ati:FindInvite id="atiFindInvite" runat="server" />
                 
            </div>            			
    	</div>                                                                   
    </div>

    <!-- END Tabs -->     
    <div id="divRightAdUnit" runat="server" style="width: 160px; position: absolute; right: 0px;">
        <a href="http://tastypaleo.com"><asp:Image ID="imgAd" runat="server"/></a>         
    </div>   
   
    <div id="divLeftNav" runat="server" style="width: 196px; float: left;">
        <ati:ProfileImage ID="atiProfileImage" runat="server" Small="true" />
        <div id="divMainLinks" runat="server" style="margin-top: 10px;">                            
        <ul class="linksList">
            <li><a href="/Community/HowTo.aspx?h=2">Help using this page</a></li>
            <li runat="server" id="liEdit2" style="border-bottom: 0;">&nbsp;</li>
            <li><a href="/Community/Athletes.aspx">Find people by sport</a></li>
            <li><a href="/Community/Athletes.aspx">Find people by skill level</a></li>
        </ul>
        </div>

        <div style="margin-top: 100px;">
            <ati:FeaturedGroup ID="atiFeaturedGroup" runat="server" />
        </div>
    </div>
    <div style="clear:both;"></div>     

</div>
               










    

    

         
    
               



