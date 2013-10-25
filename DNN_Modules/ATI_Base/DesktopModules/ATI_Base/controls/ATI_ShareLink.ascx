<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ShareLink.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ShareLink" %>

<script type="text/javascript">
    Aqufit.Page.Controls.ATI_ShareLink = function (id, txt, tweet) {
        this.id = id;
        this.txt = txt;
        $('#'+this.txt).focus(function() {
            $(this).select();
        });
        $('#'+tweet).click( function(event){
            
        });
    };

    $(function () { 
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_ShareLink('<%=this.ID %>','<%=txtShareLink.ClientID %>','<%=aTweetShare.ClientID %>');
    });
</script>

<ul class="hlist">
    <li>
    Share Link:
<asp:TextBox ID="txtShareLink" runat="server" />
    </li>
    <li><span class="grad-FFF-EEE ui-corner-all" style="border: 1px solid #CCC; padding: 6px 2px 2px 2px;"><a target="_blank" title="Share on Twitter" id="aTweetShare" runat="server"><img id="imgTwitter" runat="server" /></a></span></li>
    <li><fb:like href="<%=this.ShareLink %>" layout="button_count" show-faces="false" width="50" action="like" colorscheme="light" /></li>
</ul>