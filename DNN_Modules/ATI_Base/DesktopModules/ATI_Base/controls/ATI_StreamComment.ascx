<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamComment.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamComment" %>

<script type="text/javascript">

    Aqufit.Page.Controls.ATI_StreamComment = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;
    };

    Aqufit.Page.Controls.ATI_StreamComment.prototype = {
        getTxt: function () {
            return $('#<%=atiTxtComment.ClientID %>').val();
        },
        clear: function(){
            $('#<%=atiTxtComment.ClientID %>').val('');
        }
    };

    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_StreamComment('<%=this.ID %>','<%=panelStreamComment.ClientID %>');
    });

</script>
<asp:Panel id="panelStreamComment" CssClass="commentForm" runat="server">
<fieldset class="atiFieldSet">
    <dl>
       <dt><asp:Label id="plComment" runat="server" controlname="txtComment" text="Comment (1024 character max):" style="font-size: 10px;" /></dt>
       <dd><asp:TextBox ID="atiTxtComment" runat="server" TextMode="MultiLine" MaxLength="1024" Width="100%" Height="75px" CssClass="ui-corner-all ui-widget-content atiTxtBox" /></dd>       
    </dl>
</fieldset>
</asp:Panel>