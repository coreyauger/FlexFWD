<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_IconSelector.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_IconSelector" %>
<style type="text/css">
div.iconHolder
{
	padding-left: 5px;
	background: url(<%=ResolveUrl("~/DesktopModules/ATI_Fitness/resources/images/iHolder.png")%>) no-repeat top left;
	height: 43px;	
	width: 159px; 
}
div.iconHolder > img
{
	padding-right: 3px; 
}
</style>
<script type="text/javascript">

    Aqufit.Page.Controls.atiIconHelper = function (id, h, s, b, c) {
        this.id = id;
        this.hiddenId = h;
        this.statusId = s;
        this.basePath = b;
        this.clientId = c;
        selectedId = -1;
    }

    Aqufit.Page.Controls.atiIconHelper.prototype = {
        setHiddenId: function (id) {
            this.hiddenId = id;
        },
        setStatusId: function (id) {
            this.statusId = id;
        },
        setBaseUrl: function (bUrl) {
            this.basePath = bUrl;
        },
        setClientId: function (id) {
            this.clientId = id;
        },
        switchIcon: function (id, num, val) {
            if (id != this.selectedId) document.getElementById(this.clientId + '_' + id).src = this.basePath + id + '_' + num + '.png';
        },
        clear: function () {
            if (this.selectedId != '') {
                var $selected = $('#' + this.clientId + '_' + this.selectedId);
                $selected.attr('src', this.basePath + this.selectedId + '_0.png');
                this.selectedId = '';
                $('#' + this.hiddenId).val('');
                $('#' + this.statusId).html('');
            }
        },
        selectIcon: function (id, status, val) {
            var $selected = $('#' + this.clientId + '_' + id);
            if ($selected) {
                var $lastSelected = $('#' + this.clientId + '_' + this.selectedId);
                if ($lastSelected.length) {
                    $lastSelected.attr('src', this.basePath + id + '_0.png');
                }
                $selected.attr('src', this.basePath + id + '_1.png');
                this.selectedId = id;
                $('#' + this.hiddenId).value = val;
                $('#' + this.statusId).html(status);
            }
        }
    };

    $(function(){
        Aqufit.Page.<%= this.ID%> = new Aqufit.Page.Controls.atiIconHelper('<%=this.ID %>','<%=hiddenValue.ClientID%>','<%=lStatus.ClientID%>', '<%=ResolveUrl(this.BaseImgUrl)%>', '<%=this.ClientID%>');
    });
       
        
</script>

<div runat="server" id="imgPreload" style="position: absolute; top: -1000px; left: -2000px; overflow: hidden;">    
</div>

<label><asp:Label ID="lTitle" runat="server" CssClass="ati_Form_TextLeft" /></label>
<div id="iconHolder" runat="server" class="iconHolder">
    <asp:HiddenField ID="hiddenValue" runat="server" Value="0" />
</div>
<label><asp:Label ID="lStatus" runat="server" Text=" " /></label>
