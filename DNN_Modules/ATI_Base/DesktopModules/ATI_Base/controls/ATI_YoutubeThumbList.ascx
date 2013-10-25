<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_YoutubeThumbList.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_YoutubeThumbList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">        
    span.mediabox
    {
    	border: 1px solid #ccc;
    	width: 137px;
    	text-align: center;
    	display: inline-block;
    	padding: 10px;
    	height: 175px;  
    	background-color: #EEE;	
    	cursor: pointer;
    	
    }
    span.mediabox:hover
    {
    	border: 1px solid #fbcb09;
    	background-color: #fdf9e1;
    }
    span.mediabox img
    {
        margin-bottom: 10px;
        border: 1px solid #333;
    }
    span.mediabox span
    {
        display: block;
    }
    
</style>

<script type="text/javascript">


    Aqufit.Windows.MediaWin = {
        win: null,
        close: function (oWnd, args) {
            oWnd.SetUrl('about:blank');
        },
        open: function (vid) {
            Aqufit.Windows.MediaWin.win = radopen('<%=ResolveUrl("~/Media") %>?vid=' + vid, 'MediaWin');
            Aqufit.Windows.MediaWin.win.set_modal(true);
            Aqufit.Windows.MediaWin.win.setSize(747, 650);
            Aqufit.Windows.MediaWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            Aqufit.Windows.MediaWin.win.add_close(Aqufit.Windows.MediaWin.close);
            Aqufit.Windows.MediaWin.win.center();
            Aqufit.Windows.MediaWin.win.show();
            return false;
        },
        openWOD: function (wod) {
            Aqufit.Windows.MediaWin.win = radopen('<%=ResolveUrl("~/Media") %>?wod=' + wod, 'MediaWin');
            Aqufit.Windows.MediaWin.win.set_modal(true);
            Aqufit.Windows.MediaWin.win.setSize(747, 650);
            Aqufit.Windows.MediaWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            Aqufit.Windows.MediaWin.win.add_close(Aqufit.Windows.MediaWin.close);
            Aqufit.Windows.MediaWin.win.center();
            Aqufit.Windows.MediaWin.win.show();
            return false;
        }
    };  

//    $(function () {
      
  //  });        
</script>
</telerik:RadCodeBlock>


<asp:ListView id="mediaList" runat="server" ItemPlaceholderID="itemPlaceholder">    
    <LayoutTemplate>
        <ul id="mediaListWrap" class="hlist">
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </ul>
    </LayoutTemplate>
    
    <ItemTemplate>
        <li>
            <span class="ui-corner-all mediabox" onclick="Aqufit.Windows.MediaWin.open('<%#Eval("VideoId") %>')">
                <img src="<%#Eval("ImageUrl") %>" />
                <span><%#Eval("Title")%><br />(<%#Eval("Duration")%>)</span>
            </span>            
        </li>
    </ItemTemplate>

    <EmptyItemTemplate>
        <center><h3>No Relevant Media</h3></center>
    </EmptyItemTemplate>

</asp:ListView>


