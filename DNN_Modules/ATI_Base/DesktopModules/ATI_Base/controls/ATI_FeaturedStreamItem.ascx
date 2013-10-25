<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_FeaturedStreamItem.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_FeaturedStreamItem" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<script type="text/javascript">    
    

    $(function () {        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_FeaturedStreamItem('<%=this.ID %>', '<%=atiStreamPanel.ClientID %>', '<%=this.Title %>');       
    });  
    
</script>

<asp:Panel ID="atiStreamPanel" runat="server">           
</asp:Panel>
