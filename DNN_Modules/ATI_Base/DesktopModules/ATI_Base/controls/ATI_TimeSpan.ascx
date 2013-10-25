<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_TimeSpan.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_TimeSpan" %>

<script type="text/javascript">  

    // Note: TimeSpan JS is in base.js 

    $(function () {        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiTimeSpan('<%=this.ID %>', '<%=atiTxtTimeHour.ClientID %>', '<%=atiTxtTimeMin.ClientID %>', '<%=atiTxtTimeSec.ClientID %>');   
        $('#<%=ddTimeSpan.ClientID %> input[type="text"]').click(function(){
            $(this).focus().select();
        }).keydown(function(e){
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
            return (
                key == 8 || 
                key == 9 ||
                key == 46 ||
                (key >= 37 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        });                    
    });       
    
</script>

<asp:label id="plTime" runat="server" controlname="txtWorkoutTime" CssClass="ati_Form_TextLeft" text="Time:" /><asp:Label ID="lPace" runat="server" CssClass="ati_Form_TextLeft" Text=" (Pace : ) &lt;br /&gt;" />
<div id="ddTimeSpan" runat="server">       
    <asp:TextBox id="atiTxtTimeHour" runat="server" Text="HH" MaxLength="2" Width="50px" /><asp:Literal ID="litHourSep" runat="server" Text=":" />
    <asp:TextBox id="atiTxtTimeMin" runat="server" Text="MM" MaxLength="2" Width="50px" />:
    <asp:TextBox id="atiTxtTimeSec" runat="server" Text="SS" MaxLength="2" Width="50px" />
</div>