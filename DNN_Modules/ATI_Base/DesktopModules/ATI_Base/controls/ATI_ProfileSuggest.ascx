<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ProfileSuggest.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ProfileSuggest" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">
ul.listSugested li
{
	list-style: none;
}
div.uSuggestWrap
{
	position: relative;
	padding: 4px 10px;
}
div.uSuggestWrap:hover
{
	background-color: #eee;
}
div.uInfo
{
	position: absolute;
	top: 10px;
    left: 70px;
    font-size: 11px;
}

a.sugestRem
{
	position: absolute;
	font-size: 8px;
	top: 0px;
	right: 10px;
}

a.sugestAdd
{
	position: absolute;
	font-size: 10px;
	color: #F9A01B;
	bottom: 5px;
	left: 70px;
}

</style>
<script type="text/javascript">
    Aqufit.Page.Controls.ATI_ProfileSuggest = function (id, list, contain) {
        this.id = id;
        this.$list = $('#'+list);
        this.$contain = $('#'+contain);
        this.avoid = [];
        this.numItems = 0;
    }

    Aqufit.Page.Controls.ATI_ProfileSuggest.prototype = {
        genListItem: function( u ){
            this.numItems++;
            var that = this;
            this.$list.append('<li><div class="uSuggestWrap"><a href="'+Aqufit.Page.PageBase+u["UserName"]+'"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + u["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /><div class="uInfo"><strong>'+u["UserName"]+'</strong><br />('+u["FirstName"]+ ' ' + u["LastName"] + ')</div></a><a id="sugestAdd'+u["Id"]+'" class="sugestAdd" href="javascript: ;">[+] add friend</a><a id="sugestRem'+u["Id"]+'" class="sugestRem" href="javascript: ;">[x]</a></div></li>');
            this.avoid.push( u["Id"] );
            $('#sugestRem'+u["Id"]).click(function(event){
                $(this).closest('li').fadeOut('slow', function(){ $(this).empty(); that.numItems--; });
                Affine.WebService.StreamService.getFriendSuggestions(Aqufit.Page.UserSettingsId, Aqufit.Page.MainGroupKey, that.avoid, 1, function(json){                    
                    that.geneateListDom(json);
                }, function(){ alert('fail'); });
                event.stopPropagation();
                return false;
            });
            $('#sugestAdd'+u["Id"]).click(function(event){
                if( Aqufit.Page.Actions.SendSuggestedFriendRequest ){
                    Aqufit.Page.Actions.SendSuggestedFriendRequest(u["Id"]);
                    $(this).closest('li').fadeOut('slow', function(){ $(this).empty(); that.numItems--; });
                    Affine.WebService.StreamService.getFriendSuggestions(Aqufit.Page.UserSettingsId, Aqufit.Page.MainGroupKey, that.avoid, 1, function(json){
                       that.geneateListDom(json);
                    }, function(){ alert('fail'); });
                }else{
                    alert('no Aqufit.Page.Actions.SendSuggestedFriendRequest defined');
                }
                event.stopPropagation();
                return false;
            });
        },
        geneateListDom: function(results){
            var json = eval('('+results+')');
            //this.$list.empty();
            if( json.length <= 0 && this.numItems <= 0){
                this.$contain.hide();
            }
            for( var i=0; i<json.length; i++ ){
                var u = json[i];
                this.genListItem(u);
            }
        }
    };

    $(function () {       
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_ProfileSuggest('<%=this.ID %>', '<%=listSugested.ClientID %>', '<%=atiSideContainer.ClientID %>');        
    });
</script>
</telerik:RadCodeBlock>
<div class="atiSideContainer" id="atiSideContainer" runat="server">
<span>Friend Suggestions</span>                         
    <ul id="listSugested" runat="server" class="listSugested">
    
    </ul>                    
</div> 


