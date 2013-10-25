<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_TotalDistColors.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_TotalDistColors" %>

<script type="text/javascript">      

    Aqufit.Page.Controls.ATI_TotalDistColors = function (id) {
        this.id = id;   
        this.bandWidth = 373.0;
        this.colors = [
                        {'spx':0, 'epx':24, 'rgb':'#ffc500', 'name':'Yellow', 'skm': 0, 'ekm': 49},
                        {'spx':24, 'epx':66, 'rgb':'#ff8a00', 'name':'Orange', 'skm': 50, 'ekm': 249},
                        {'spx':66, 'epx':146, 'rgb':'#77c300', 'name':'Green', 'skm': 250, 'ekm': 999},
                        {'spx':147, 'epx':236, 'rgb':'#2d6eba', 'name':'Blue', 'skm': 1000, 'ekm': 2499},
                        {'spx':237, 'epx':344, 'rgb':'#5953ad', 'name':'Purple', 'skm': 2500, 'ekm': 4999},
                        {'spx':345, 'epx':373, 'rgb':'#000000', 'name':'Black', 'skm': 5000, 'ekm': 99999999}
                      ];
    }

    Aqufit.Page.Controls.ATI_TotalDistColors.prototype = {
        getLevel: function (dist) {
            for (var i = 0; i < this.colors.length; i++) {
                var level = this.colors[i];
                if (level.ekm > dist) {
                    return level;
                }
            }
            return this.colors[this.colors.length - 1];
        },
        setupDistance: function (dist) {
            var level = this.getLevel(dist);
            var left = level.ekm - dist;
            // get the percent of the total dist
            var p = dist / level.ekm;
            var px = level.spx + p * (level.epx - level.spx);
            $('#distColorName').css('color', level.rgb).html(level.name);
            var distStr = Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KM, dist, Aqufit.Page.DistanceUnits), 2) + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.DistanceUnits);
            $('#colorTotalDist').css('color', level.rgb).html(distStr);

            $('#colorSelect').css('left', px - 5 + 'px');
        },
        getColorForDist: function (dist) {
        }
    };      
</script>
<div style="position: absolute; right: 55px; width: 375px; z-index: 9;">
<div style="float: left; display: block;"><span>My Level: <span id="distColorName" style="font-weight: bold; line-height: 24px;"></span></span></div><div style="float: right;"><span>Total: <span id="colorTotalDist" style="font-weight: bold;"></span></span></div>
<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/levelSelect.png")%>" id="colorSelect" style="position: absolute; top: 18px; left: 0px;" />
<br style="clear: both;" />
<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/distColors.png")%>" />
</div>