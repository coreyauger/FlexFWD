<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_CompetitionAthlete.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_CompetitionAthlete"  %>
<script type="text/javascript">
    Aqufit.Page.Controls.Ati_CompetitionAthlete = function(id){
        this.id = id;
    };

    Aqufit.Page.Controls.Ati_CompetitionAthlete.prototype = {
        init: function(){
        },
        loadAthlete: function(a){
            var h = parseFloat( a.Height );
            if( !isNaN(h) ){
                var inches = Aqufit.Units.convert(Aqufit.Units.UNIT_M, h, Aqufit.Units.UNIT_INCHES);
                var feet = Math.floor(inches / 12);
                inches = Math.ceil(inches % 12);
                var height = '' + feet + "' " + inches + '"';
            }
            var w = parseFloat( a.Weight );
            if( !isNaN(w) ){
                var lbs = Aqufit.Units.convert(Aqufit.Units.UNIT_KG, w, Aqufit.Units.UNIT_LBS);
                lbs = Math.round(lbs * 100) / 100;
                var weight = lbs + ' lbs';
            }
            $('#caTitle').html('<h3>'+a.Name+' '+a.Rank+' ('+a.Score+')</h3>');
            $('#<%=imgAthlete.ClientID %>').attr('src',a.Img);
            $('#caDetails').html('<ul>'+
                                            '<li>Height: <em>' +height+'</em></li>'+                
                                            '<li>Weight: <em>' + weight + '</em></li>'+
                                            '<li>&nbsp;</li>'+
                                            '<li>Affiliate: <em>' + a.Affiliate + '</em></li>'+
                                            '<li>Region: <em>' + a.Region + '</em></li>'+
                                            '<li>Hometown: <em>' + a.Hometown + '</em></li>'+
                                            '<li>Country: <em>' + a.Country + '</em></li>'+              
                                            '</ul>');           
        },
        selectedAthlete: function(id){
            var that = this;
            Affine.WebService.StreamService.GetCompetitionAthlete(id, function(json){
                that.loadAthlete( eval('('+json+')') );
            });         
        }
    };
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.Ati_CompetitionAthlete('<%=this.ID %>');
    });    
</script>
<div class="compAthete" style="background-color: White;">
    <div class="atiSearchControls grad-FFF-EEE">
        <h3 id="caTitle"><asp:Literal ID="litName" runat="server" /></h3>
    </div>
    <center>
    <span style="border: 1px solid #CCC; display: inline-block;">
    <asp:Image ID="imgAthlete" runat="server" />
    </span>
    </center>
    <div id="caDetails"></div>
    <asp:Literal ID="litDetails" runat="server" />
</div>