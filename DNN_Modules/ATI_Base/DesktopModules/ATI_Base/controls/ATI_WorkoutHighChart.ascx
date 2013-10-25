<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WorkoutHighChart.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_HighChart" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
 
</style>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 

<script type="text/javascript" src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/Highcharts-2.1.3/js/highcharts.src.js") %>"></script>
<script type="text/javascript">
    Aqufit.Page.Controls.ATI_WorkoutHighChart = function (id) {
        this.id = id;
        this.min = Number.MAX_VALUE;
        this.length = 0;
        this.chart = null;
        this.series = new Array(2);
        this.colors = [];
        this.categories = [];
        this.dataArray = [];
        this.title = '';
    }

    Aqufit.Page.Controls.ATI_WorkoutHighChart.prototype = {
        fromYourStreamData: function (jsonStr) {
            var serie = { name: 'You', data: [], type: 'spline' };
            var json = eval('(' + jsonStr + ')');
            var i = 0;
            var scoreNum = 0;
            var scoreTxt = '';
            var numToShow = json.length > 15 ? 15 : json.length;
            for (i = 0; i < numToShow; i++) {
                var sd = json[i];
                if (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT) {
                    if (sd["Score"] > 0) {
                        scoreNum = Aqufit.Utils.round(sd["Score"], 2);
                        scoreTxt = scoreNum;
                    } else if (sd["Max"] > 0) {
                        scoreNum = Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, sd["Max"], Aqufit.Page.WeightUnits), 2);
                        scoreTxt = 'Max ' + scoreNum + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.WeightUnits);
                    } else {
                        scoreNum = sd["Duration"];
                        scoreTxt = Aqufit.Utils.toDurationString(sd["Duration"]);
                    }
                    if (scoreNum < this.min) {
                        this.min = scoreNum;
                    }
                    scoreTxt = sd["UserName"] + ' ' + scoreTxt;
                    serie.data[i] = scoreNum;
                    this.dataArray[serie.name + i] = { toolTip: scoreTxt, streamData: json[i] };
                }
            }
            for (var j = i; j < this.length; j++) {
                serie.data[j] = scoreNum;
                this.dataArray[serie.name + j] = { toolTip: scoreTxt, streamData: json[i - 1] };
            }
            this.pushSeries(1, serie);
        },
        genChart: function (json) {
            //this.min = Number.MAX_VALUE;
            var serie = { name: 'Scores', data: [], type: 'column' };
            this.length = json.length;
            for (var i = 0; i < json.length; i++) {
                var sd = json[i];
                if (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT) {
                    var scoreNum = 0;
                    var scoreTxt = '';
                    if (sd["Score"] > 0) {

                        scoreNum = Aqufit.Utils.round(sd["Score"], 2);
                        scoreTxt = scoreNum;
                    } else if (sd["Max"] > 0) {
                        scoreNum = Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, sd["Max"], Aqufit.Page.WeightUnits), 2);
                        scoreTxt = 'Max ' + scoreNum + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.WeightUnits);
                    } else {
                        scoreNum = sd["Duration"];
                        scoreTxt = Aqufit.Utils.toDurationString(sd["Duration"]);
                    }
                    if (scoreNum < this.min) {
                        this.min = scoreNum;
                    }
                    scoreTxt = sd["UserName"] + ' ' + scoreTxt;
                    serie.data[i] = scoreNum;
                    this.dataArray[serie.name + i] = { toolTip: scoreTxt, streamData: json[i] };
                }
            }
            this.pushSeries(0, serie);
        },
        fromStreamData: function (jsonStr) {
            var json = eval('(' + jsonStr + ')');
            this.genChart(json);
        },
        pushSeries: function (ind, s) {
            s.dataLabels = {
                enabled: true,
                /*rotation: -90,*/
                color: '#FFFFFF',
                align: 'center',
                /* x: -15, */
                y: 20,
                formatter: function () {
                    //return this.y + " km";
                    return '';
                },
                style: {
                    font: 'bolder 14px Verdana, sans-serif'
                }
            };
            this.series[ind] = s;
        },
        getToolTip: function (name, x, y) {
            return this.dataArray[name + x].toolTip;
            //return name + " " + y + " km";
        },
        handlePointClick: function (name, x, y) {
            top.location.href = Aqufit.Page.SiteUrl + this.dataArray[name + x].streamData['UserName'] + '/achievements';
        },
        getData: function (name, x) {
            return this.dataArray[name + x].streamData;
        },
        drawChart: function () {
            var that = this;
            this.chart = new Highcharts.Chart({
                chart: {
                    renderTo: '<%=atiWorkoutHighChartPanel.ClientID %>',
                    defaultSeriesType: 'column',
                    margin: [10, 30, 10, 10]
                },
                colors: that.colors,
                title: {
                    text: that.title
                },
                plotOptions: {
                    column: {
                        borderRadius: 2,
                        borderWidth: 1,
                        pointPadding: 0.05,
                        groupPadding: 0.05,
                        shadow: true,
                        dataLabels: {
                            enabled: false
                        },
                        point: {
                            events: {
                                click: function () {
                                    return that.handlePointClick(this.series.name, this.x, this.y);
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
                        enabled: false,
                        /*rotation: -45,*/
                        align: 'center',
                        style: {
                            font: 'bold 13px Verdana, sans-serif'
                        }
                    }
                },
                yAxis: {
                    min: this.min,
                    opposite: true,
                    labels: {
                        enabled: false
                    },
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
                        return that.getToolTip(this.series.name, this.x, this.y);
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

<asp:Panel ID="atiWorkoutHighChartPanel" runat="server">

</asp:Panel>
