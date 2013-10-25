<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamCommentScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamCommentScript" %>

<script type="text/javascript">

    $(function () {
        $('#<%=panelStreamComment.ClientID %> div.commentBoxLeft').append('<img src="' + Aqufit.Page.PageBase + "DesktopModules/ATI_Base/services/images/profile.aspx?u=" + Aqufit.Page.UserId + '&p='+Aqufit.Page.PortalId+'" />');
        $('#bPostShout').click(function (event) {
            var txt = $('#atiTxtComment').val();
            Affine.WebService.StreamService.SaveStreamShout(Aqufit.Page.UserId, Aqufit.Page.PortalId, txt, function (json) {
                Aqufit.Page.Controls.atiStreamPanel.prependJson(json);
            },
            function () {
                $('#sendCommentFail').show();
            });
            event.stopPropagation();
        });
    });
    
</script>

<div id="sendCommentFail" class="ui-widget" style="display: none;">
	<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;"> 
		<p><span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span> 
		<strong>Alert:</strong> Failed to send your comment. Try a page refresh?</p>
	</div>
</div>
<asp:Panel id="panelStreamComment" CssClass="workoutForm" runat="server">
<ul>
    <li>
    <div class="commentBoxLeft">        
    </div>
    <div class="commentBoxRight" style="width: 610px;">
    <img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/speak.png")%>" class="speak" />
        <textarea ID="atiTxtComment" maxlength="512" style="width: 100%; height: 50px;" ></textarea>
    </div>
    </li>
</ul>
(twitter)(facebook)<input type="button" value="Post" id="bPostShout" />
</asp:Panel>