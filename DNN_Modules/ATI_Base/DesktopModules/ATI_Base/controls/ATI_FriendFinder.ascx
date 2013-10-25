<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FriendFinder.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FriendFinder" %>

<!-- TODO: combine all these styles and js into one file (reduce downlaod time) -->
<!-- TODO: make the page load insert them into the header -->
<script src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/jquery.fcbkcomplete.js")%>" type="text/javascript" charset="utf-8"></script>	   
<script type="text/javascript">
    // CA - Note: when i combine the jquery and the above scripts it stops working.

    Aqufit.Page.Controls.atiFriendFinder = function(id) {
        this.id = id;
        this.init();
    };

    Aqufit.Page.Controls.atiFriendFinder.prototype = {
        init: function() {
            $('#<%=atiUserNames.ClientID %>').fcbkcomplete({
                json_url: "fetched.txt",
                cache: false,
                filter_case: false,
                filter_hide: true,
                firstselected: true,
                //onremove: "testme",
                //onselect: "testme",
                filter_selected: true,
                newel: true,
                hidden: '<%=atiUserIdArray.ClientID %>'
            }); 
        }
    };
    
    $(function(){
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiFriendFinder('<%=this.ID %>');
    });
 
</script>   
<asp:HiddenField ID="atiUserIdArray" runat="server" />    
<div style="width: 450px;">
    <select id="atiUserNames" runat="server">
    </select>
</div>

