<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WorkoutSummaryHead.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_WorkoutSummaryHead" %>
<script type="text/javascript">

    Aqufit.Page.Controls.ATI_WorkoutSummaryHead = function (id) {
        this.id = id;
        this.$ul = $('#<%=panelWorkoutSummary.ClientID %> ul');        
    }

    Aqufit.Page.Controls.ATI_WorkoutSummaryHead.prototype = {
        set: function (dist, units, cal, nc, ns, s, e) {
            this.setDist(dist, units);
            this.setCal(cal);
            this.setNumCardio(nc);
            this.setNumStrength(ns);
            this.setTimeSpan(s, e);
        },
        setDist: function (dist, units) {
            this.$ul.find('li:nth-child(1)').html(dist + ' <span>' + Aqufit.Units.getUnitName(units) + '</span>');
        },
        setCal: function (cal) {
            this.$ul.find('li:nth-child(2)').html(cal + ' <span>cal</span>');
        },
        setNumCardio: function (num) {
            this.$ul.find('li:nth-child(3)').html(num + ' <span>Cardio</span>');
        },
        setNumStrength: function (num) {
            this.$ul.find('li:nth-child(4)').html(num + ' <span>Strength</span>');
        },
        setTimeSpan: function (start, end) {
            var s = new Date();
            var e = new Date();
            s = (typeof (start) == "string") ? Aqufit.Utils.parseDate(start) : start;
            e = (typeof (end) == "string") ? Aqufit.Utils.parseDate(end) : end;
            var em = (e.getMonth() + 1) < 10 ? "0" + (e.getMonth() + 1) : "" + (e.getMonth() + 1);
            var sm = (s.getMonth() + 1) < 10 ? "0" + (s.getMonth() + 1) : "" + (s.getMonth() + 1);
            var ed = e.getDate() < 10 ? "0" + e.getDate() : e.getDate();
            var sd = s.getDate() < 10 ? "0" + s.getDate() : s.getDate();
            this.$ul.find('li:nth-child(5)').html(sm + '.' + sd + ' - ' + em + '.' + ed);
        }
    };       
    
</script>

<asp:Panel id="panelWorkoutSummary" runat="server" CssClass="atiWorkoutSummaryHead">       
<ul class="hlist">
    <li>20 <span>km</span></li>
    <li>1000 <span>cal</span></li>
    <li>2 <span>Cardio</span></li>
    <li>4 <span>Strength</span></li>
    <li style="border: 0;">06.28 - 07.04</li>
</ul>
</asp:Panel>