<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FeaturedProfile.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FeaturedProfile" %>

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
	margin-top: 9px;
	margin-bottom: 3px;
	font-size: 11px;
	padding: 3px;	
	padding: 9px;
}

div.profileStats a
{
	font-size: 12px;
	color:#F7931E;
	font-weight: bold;
	padding-bottom: 9px;
	display: block;
}

a.uname
{
	font-size: 12px;
	color:#F7931E;
	font-weight: bold;	
}

span.stat,
ul.stat li
{
	list-style: none;
	list-style: none outside none;
	font-size: 10px;		
}

</style>

<script type="text/javascript">
    Aqufit.Page.Controls.atiFeaturedProfile = function (id) {
        this.id = id;
    }         
     
    $(function () {
        
    });        
</script>
<div style="margin-top: 10px;">
    <div id="panelProfileImageLarge" runat="server" Visible="true">
        <img id="imgProfileLarge" runat="server" />
    </div>
    <div id="panelProfileImageSmall" runat="server" Visible="false">
        <div>
            <a id="hrefProfile" runat="server">
            <img id="imgProfileSmall" runat="server" style="border: 1px solid #ccc;" />
            </a>
        </div>
    
    </div>
    <div style="overflow: hidden;">
        <asp:Literal ID="litProfileInfo" runat="server"  />
    </div>
    <div id="panelProfileStats" class="profileStats" runat="server">
        <asp:Literal ID="litStats" runat="server" />
    </div>
</div>

