<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_HowTo.ViewATI_HowTo" CodeFile="ViewATI_HowTo.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">
 div.atiMainFeed
 {
 	width: 660px;
 	padding: 10px 20px;
 }
    
.atiMainFeed div.atiStreamItemRight
{
	width: 570px;
}

div.atiMainFeed h1
{
	text-decoration: underline;
	width: 100%;
	display: block;
}

div.atiMainFeed a
{
	text-decoration: underline;
	color: #0081bd; 
}

div.atiMainFeed h2
{
	color: #666;
	font-size: 16px;
	font-weight: bold;
	font-family: Arial;
}
div.atiMainFeed span
{
	padding-top: 5px;
	padding-bottom: 5px;
	display: block;
	margin-bottom: 10px;
	font-size: 12px;
}
div.atiMainFeed ul
{
	padding: 0px 10px 10px 40px;
}
div.atiMainFeed ul li
{
	list-style: disc;
}

div.tutImgZoom img
{
	border-bottom: 2px solid #FFF;
}
div.tutImgZoom
{
	width: 290px;
	cursor: pointer;
	margin: 0 auto;
	border: 2px solid #666;
	background-color: #666;
	padding-bottom: 30px;
	background: #666 url(/DesktopModules/ATI_Base/resources/images/iImgZoom.png) no-repeat bottom right;
}
div.tutImgZoom:hover
{
	border: 2px solid #fa9a1c;
	background: #fa9a1c url(/DesktopModules/ATI_Base/resources/images/iImgZoom.png) no-repeat bottom right;
}

div.tutorialIndex
{
	margin-top: -20px;
}

</style>
<script type="text/javascript">
    Aqufit.Windows = {
        ImageWin: {
            win: null,
            open: function (arg) {
                Aqufit.Windows.ImageWin.win = radopen(arg, 'ImageWin');
                Aqufit.Windows.ImageWin.win.set_modal(true);
                Aqufit.Windows.ImageWin.win.setSize(750, 600);
                Aqufit.Windows.ImageWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
                Aqufit.Windows.ImageWin.win.center();
                Aqufit.Windows.ImageWin.win.show();
                return false;
            },
            close: function () {
                if (Aqufit.Windows.ImageWin.win) {
                    Aqufit.Windows.ImageWin.win.close();
                    Aqufit.Windows.ImageWin.win = null;
                }
            }
        },
        PlayerWin: {
            win: null,
            open: function (arg) {
                Aqufit.Windows.PlayerWin.win = radopen('<%=ResolveUrl("~/Community/Player") %>?m='+arg, 'PlayerWin');
                Aqufit.Windows.PlayerWin.win.set_modal(true);
                Aqufit.Windows.PlayerWin.win.setSize(800, 772);
                Aqufit.Windows.PlayerWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Maximize);
                Aqufit.Windows.PlayerWin.win.center();
                Aqufit.Windows.PlayerWin.win.show();
                return false;
            },
            close: function () {
                if (Aqufit.Windows.PlayerWin.win) {
                    Aqufit.Windows.PlayerWin.win.close();
                    Aqufit.Windows.PlayerWin.win = null;
                }
            }
        }
    };


    Aqufit.Page.Actions = {
        onNodeClicking: function (sender, args) {
            //alert("OnClientNodeClicking: " + args.get_node().get_value());
            top.location.href = '?h=' + args.get_node().get_value();
        }
    };

    $(function () {
        $('#streamerTabs').tabs();
        $('#menuTabs').tabs();

        $('.tutImgZoom').click(function (event) {
            var src = $(this).find('img').attr('src');
            Aqufit.Windows.ImageWin.open(src.replace('_s', '_l'));
        });
        var anCount = 0;
        var anName = [];
        $('.tutorialHead').each(function () {
            var $h = $(this);
            anName[anCount] = $h.html();
            $h.append('<a name="an' + (anCount++) + '" />');
        });


        var $ind = $('div.tutorialIndex');
        var $list = $ind.append('<ul></ul>').find('ul');
        for (var i = 0; i < anCount; i++) {
            $list.append('<li><a href="#an'+i+'">'+anName[i]+'</a></li>');
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


<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager" Skin="Black"  EnableShadow="true">
    <Windows>
        <telerik:RadWindow runat="server" ID="ImageWin" />        
    </Windows>
</telerik:radwindowmanager>


 <asp:Panel ID="panelAjax" runat="server" >
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>


<div style="position: relative;">
<div style="width: 729px; float: right;">
    <div id="streamerTabs">
        <ul>
            <li id="tabActivity"><a href="#pageViewActivity">Site Help / How to</a></li>            				
        </ul>
            			
        <div id="pageViewActivity" style="padding: 0px; background-color: White;">
            <div class="atiMainFeed">
                <asp:Literal ID="litContent" runat="server" />
            </div>
        </div>
    </div>
</div>
<div style="width: 197px; float: left;">
    <div id="menuTabs">
        <ul>
            <li><a href="#pageViewMenu">Help Menu</a></li>            				
        </ul>
        <div id="pageViewMenu" style="padding: 0px; background-color: White;">
            <telerik:RadTreeView runat="server" ID="RadTreeView1" Style="margin: 15px;" Skin="Office2007" Width="197px"
                DataSourceID="EntityDataSource1" DataTextField="Title" DataValueField="Id" OnDataBound="RadTreeView1_DataBound"
                DataFieldID="Id" DataFieldParentID="ParentKey" OnClientNodeClicking="Aqufit.Page.Actions.onNodeClicking">       
            </telerik:RadTreeView>
            <asp:EntityDataSource runat="server" 
                                    ID="EntityDataSource1" 
                                    ContextTypeName="Affine.Data.aqufitEntities"
                                    EntitySetName="HelpPages">
            </asp:EntityDataSource>
        </div>
    </div>
</div>
    
</div>
<br style="clear: both;" />
    

         
    
               



