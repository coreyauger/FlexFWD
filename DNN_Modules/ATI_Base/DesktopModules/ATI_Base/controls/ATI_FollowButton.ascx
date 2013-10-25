<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FollowButton.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FollowButton" %>


<style type="text/css">
 
img#imgProfileSmall
{
	padding: 3px 10px 10px 3px;
	background: #FFFFFF url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/profileBorder.png")%>) no-repeat top left;	 
	cursor: pointer;	
}
ul#profileLinks li
{
	list-style: none;
	list-style: none outside none;
	position: absolute;
	top: 16px;
	left: 65px;
}
a.username
{
	color:#ca992c;
    font-size:13px;   
    font-weight: normal;   
    font-weight: bolder;        
}
div.profileStats
{
	margin-top: 3px;
	margin-bottom: 3px;
	border: 1px solid #CCCCCC;
	font-size: 11px;
	padding: 3px;
	background-color: #EFEFEF;
}

</style>

<script type="text/javascript">
    Aqufit.Page.Controls.atiFollowButton = function (id, control) {
        this.id = id;
        this.control = control;
    };

    $(function () { 
        $('button.atiFollow').button({
            icons: {
                primary: 'ui-icon-person'
            }
        }).click(function (event) {
            <%if( this.UnFollow ){ %>
                Affine.WebService.StreamService.UnFollowUser(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, function(){
                    alert('success');
                }, function(){
                    alert('fail');
                });
            <%}else{ %>
                Affine.WebService.StreamService.FollowUser(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, function(){
                    alert('success');
                }, function(){
                    alert('fail');
                });
            <%} %>
            event.stopPropagation();
            return false;
        });       
});        
</script>

<button class="atiFollow" style="width: 100%;"><%=this.Text %></button>
