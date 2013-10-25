<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_HighChart.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_HighChart" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
 
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 

<script type="text/javascript" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/Highcharts-2.1.3/js/highcharts.src.js") %>"></script>
<script type="text/javascript">
    Aqufit.Page.Controls.ATI_HighChart = function (id) {
        this.id = id;
        this.chart = null;
        this.series = [];
        this.colors = [];
        this.categories = [];
    }

    Aqufit.Page.Controls.ATI_HighChart.prototype = {        
        pushSeries: function( s ){
            s.dataLabels = {
                        enabled: true,
                        /*rotation: -90,*/
                        color: '#FFFFFF',
                        align: 'center',
                        /* x: -15, */
                        y: 20, 
                        formatter: function () {
                            return this.y + " km";
                        },
                        style: {
                            font: 'bolder 14px Verdana, sans-serif'                                                   
                        }                         
                    };
            this.series.push(s);
        },
        getToolTip: function( name, x, y ){
            return name + " " + y + " km";
        },
        drawChart: function () {
            var that = this;
            this.chart = new Highcharts.Chart({
                chart: {
                    renderTo: '<%=atiHighChartPanel.ClientID %>',
                    defaultSeriesType: 'column',
                    margin: [55, 35, 20, 15]                         
                },                
                colors: that.colors,
                title: {
                    text: ''
                },
                plotOptions: {
                    column: {                    
                        borderRadius: 10,
                        borderWidth: 3,                   
                        pointPadding: 0.05,
                        groupPadding: 0.05,
                        shadow: true,
                        point: {
                            events: {
                                click: function(event){
                                    alert(this.id);
                                }
                            }
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    categories: this.categories,
                    labels: {
                        /*rotation: -45,*/
                        align: 'center',
                        style: {
                            font: 'bold 13px Verdana, sans-serif'                            
                        }
                    }
                },
                yAxis: {
                    min: 0,
                    opposite: true,
                    title: {
                        text: ''
                    }
                },
                legend: {
                    enabled: true,
                    style: {
	                    position: 'absolute',
	                    zIndex: 10,
	                    top: '10px',
	                    left: '15px',
	                    padding: '5px'
                    }

                },
                tooltip: {
                    formatter: function () {
                        return that.getToolTip( this.series.name, this.x, this.y );
                    }
                },
                series: this.series
            });
        }
    };

    //$(function () {
        // chart js gets created and loaded on server end.        
    //});        
</script>
</telerik:RadCodeBlock>

<asp:Panel ID="atiHighChartPanel" runat="server">

</asp:Panel>
