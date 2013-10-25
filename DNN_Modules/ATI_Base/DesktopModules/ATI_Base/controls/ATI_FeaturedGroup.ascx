<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FeaturedGroup.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FeaturedGroup" %>
<%@ Register TagPrefix="ati" TagName="FeaturedProfile" Src="~/DesktopModules/ATI_Base/controls/ATI_FeaturedProfile.ascx" %>

<script type="text/javascript">
    Aqufit.Page.Controls.ATI_FeaturedGroup = function (id) {
        this.id = id;        
    };

    $(function () { 
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_FeaturedGroup('<%=this.ID %>');
    });
</script>
<style type="text/css">
    div.memberList ul li a
    {
    	height: 20px;
    	width: 100%;
	    display: block;
    }
div.memberList ul li
{
	list-style: none;
	
	
}
div.memberList ul li:nth-child(odd)
{
	background: #eee;
}
div.memberList ul li:nth-child(even)
{
	background: #fff;
}
div.profileHeading2
{
	border: 1px solid #ccc;
	padding: 5px;
}
</style>

<div class="profileHeading2 grad-FFF-EEE ui-corner-all">
    <h2>Featured Group</h2>
    <ati:FeaturedProfile ID="atiFeaturedProfile" runat="server" Small="true" />
    <div class="memberList">
        <h4>Members: <em><%=this.NumMembers %></em></h4>
        <asp:ListView id="listMemberList" runat="server" ItemPlaceholderID="itemContainer">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="itemContainer" runat="server" />
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li><a href="/<%#Eval("Username") %>"><%#Eval("Username") %></a></li>
        </ItemTemplate>
        </asp:ListView>
        <a id="hrefAllMembers" class="uname" runat="server">View All ...</a>
    </div>
</div>
