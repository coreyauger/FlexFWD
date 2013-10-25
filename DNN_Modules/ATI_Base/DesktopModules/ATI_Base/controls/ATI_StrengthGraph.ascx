<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StrengthGraph.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StrengthGraph" %>

<style type="text/css">
ul.strengthGraph li
{
	padding-right: 22px;
	min-width: 75px;
}
</style>

<script type="text/javascript">
    Aqufit.Page.Controls.ATI_StrengthGraph = function (id, control) {
        this.id = id;        
        this.control = control;
        this.workoutArray = [];
    };

    Aqufit.Page.Controls.ATI_StrengthGraph.prototype = {
        setup: function(json){
            this.workoutArray = eval('('+json+')');
            var $ctr = $('#'+this.control + ' > ul');
            var html = '';
            for( var i=0; i<this.workoutArray.length; i++){
                var w = this.workoutArray[i];
                if( w.Id > 0 ){                    
                    var s = w.S;
                    if( w.T == Aqufit.WODTypes.TIMED ){
                        s = Aqufit.Utils.toDurationString(s);
                    }
                    html += '<li><a href="/'+Aqufit.Page.UserName+'/workout/'+w.Id+'" title="'+w.UId+' ('+s+')"><img src="/DesktopModules/ATI_Base/resources/images/graphStrength.png" /></a></li>';
                }else{
                    html += '<li><img src="/DesktopModules/ATI_Base/resources/images/graphStrength.png" style="visibility: hidden;" /></li>';
                }
            }
            $ctr.append(html);
        }   
    };

    $(function () { 
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_StrengthGraph('<%=this.ID %>','<%=atiStrengthGraph.ClientID %>');            
    });
</script>
<div id="atiStrengthGraph" runat="server" style="position: absolute; z-index: 9999999; margin-top: 80px; margin-left: 20px;">
    <ul class="hlist strengthGraph">
    </ul>    
</div>