<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ProfileImage.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ProfileImage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">
 img.imgProfileLarge
 {
 	border: 1px solid #000;
 }
img.imgProfileSmall
{
	padding: 3px 10px 10px 3px;
	background: #FFFFFF url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/profileBorder.png")%>) no-repeat top left;	 
	cursor: pointer;	
}
ul.profileLinks li
{
	list-style: none;
	list-style: none outside none;
	position: absolute;
	top: 20px;
	left: 65px;
	width: 130px;
	overflow: hidden;
}
a.username
{
	color:#ca992c;
    font-size:13px;   
    font-weight: normal;   
    font-weight: bolder;        
}

</style>

<script type="text/javascript">
    
    Aqufit.Page.Controls.atiProfileImage = function(id, img){
        this.id = id;
        this.$img = $('#'+img); 
    };
    
    Aqufit.Page.Controls.atiProfileImage.prototype = {
        ImageUploadSuccess: function (us, pid) {
            var dt = new Date();
            var url = Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?f=1&us='+us+'&t=' + dt.getTime();
            this.$img.attr('src', url);
            Aqufit.Windows.UploadWin.win.close();
        }
    };

    Aqufit.Windows.UploadWin = {
        win: null,
        UserSettingsId: null,
        close: function (oWnd, args) {
        //    $('#imgProfileSmall').attr('src', Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us='+Aqufit.Windows.UploadWin.UserSettingsId);
        },
        open: function (arg) {
            Aqufit.Windows.UploadWin.win = radopen('<%=ResolveUrl("~/Profile/UploadPic.aspx") %>'+arg, 'UploadWin');
            Aqufit.Windows.UploadWin.win.set_modal(true);
            //Aqufit.Windows.UploadWin.win.setSize(747, 600);
            Aqufit.Windows.UploadWin.win.setSize(550, 320);
            Aqufit.Windows.UploadWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            Aqufit.Windows.UploadWin.win.center();
            Aqufit.Windows.UploadWin.win.show();
            return false;
        }
    };


    $(function () {
        var t;
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiProfileImage('<%=this.ID %>', '<%=imgProfileLarge.ClientID %>');
        Aqufit.Windows.UploadWin.UserSettingsId = <%= (this.Settings != null ? this.Settings.Id : 0) %>;
        var $imgedit = $('#<%=atiProfileImgEdit.ClientID %>').button({
            icons: {
                primary: 'ui-icon-wrench'
            }
        }).click(function (event) {   
            Aqufit.Windows.UploadWin.open('<%= (this.GroupUserName != null ? "?g=" + this.GroupUserName + "&c=" : "?c=") %><%=this.ID %>');            
            event.stopPropagation();
            return false;
        });

        $('img#<%=imgProfileLarge.ClientID %>').hover(function () {
            clearTimeout(t);
            $imgedit.show();
        }, function (event) {
            t = setTimeout(function () { $imgedit.fadeOut('slow'); }, 1000);
            event.stopPropagation();
            return false;
        });
    });         
</script>
</telerik:RadCodeBlock>



<div id="panelProfileImageLarge" runat="server" Visible="true" >
    <button id="atiProfileImgEdit" runat="server" style="position: absolute; display: none;">Change Image</button>    
    <img id="imgProfileLarge" class="imgProfileLarge" runat="server" width="192" />
</div>
<div id="panelProfileImageSmall" runat="server" Visible="false" style="position:relative;">
    <a id="hrefImgSmall" runat="server"><img id="imgProfileSmall" class="imgProfileSmall" runat="server" /></a>
    <ul id="profileLinks" class="profileLinks" runat="server">
        <li ><a id="hrefNameSmall" runat="server" class="username"></a></li>
    </ul>
</div>

