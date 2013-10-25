<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_LoadingPanel.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_LoadingPanel" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">   
<style  type="text/css">                
    div#atiLoading
    {
    	background-color: #CCC;
    	filter:alpha(opacity=50);
	    -moz-opacity:0.5;
	    -khtml-opacity: 0.5;
	    opacity: 0.5;
    }
    
    </style>

<script type="text/javascript">

    Aqufit.Page.Controls.ATI_LoadingPanel = function(id) {
        this.id = id;
        this.$ctr = null;
    };

    Aqufit.Page.Controls.ATI_LoadingPanel.prototype = {
        addLoadingOverlay: function( id ){
            this.$ctr = $('#'+id);
            var position = this.$ctr.offset(); // position = { left: 42, top: 567 }

            var w = this.$ctr.width();
            w += parseInt(this.$ctr.css("padding-left"), 10) + parseInt(this.$ctr.css("padding-right"), 10); //Total Padding Width
            w += parseInt(this.$ctr.css("margin-left"), 10) + parseInt(this.$ctr.css("margin-right"), 10); //Total Margin Width
            w += parseInt(this.$ctr.css("borderLeftWidth"), 10) + parseInt(this.$ctr.css("borderRightWidth"), 10); //Total Border Width

            var h = this.$ctr.height();
            h += parseInt(this.$ctr.css("padding-top"), 10) + parseInt(this.$ctr.css("padding-bottom"), 10); //Total Padding Width
            h += parseInt(this.$ctr.css("margin-top"), 10) + parseInt(this.$ctr.css("margin-bottom"), 10); //Total Margin Width
            h += parseInt(this.$ctr.css("borderTopWidth"), 10) + parseInt(this.$ctr.css("borderBottomWidth"), 10); //Total Border Width
            
            //$(anotherElement).css(position)
            $('body').append('<div id="atiLoading" style="position: absolute; z-index: 99999; width: '+w+'px; height: '+h+'px;">'+
                                '<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/proLoading.gif")%>" style="padding-left: '+((w/2)-20)+'px; padding-top: '+((h/2)-30)+'px;" />'+
                            '</div>');
            $('#atiLoading').css(position);
        },
        remove: function(){
            $('#atiLoading').remove();             
        },
        clear: function(){
            this.remove();
        }
    };
    
    $(function(){
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_LoadingPanel('<%=this.ID %>');        
    });

</script>
</telerik:RadCodeBlock>
<img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/proLoading.gif")%>" style="position: absolute; top: -999px;" />
