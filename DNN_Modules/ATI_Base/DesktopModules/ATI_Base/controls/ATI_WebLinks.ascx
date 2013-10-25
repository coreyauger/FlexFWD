<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_WebLinks.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_WebLinks" %>

<style type="text/css">
#helpText em em 
{
	background-color: #AAFFAA;
}
dl dd ul
{
	margin: 9px 0px 9px 0px;
}
dl dd ul li
{
	list-style: none;
	list-style: none outside none;
	padding: 2px 0px 2px 30px;
	font-size: 12px;
}
</style>

<script type="text/javascript">

    Aqufit.Page.Controls.Ati_WebLinks = function (id, txt, ddl, ul) {
        this.id = id;
        this.$txt = $('#'+txt);
        this.$ddl = $('#'+ddl);
        this.$ul = $('#'+ul);  
        this.linkArray = {
            'Facebook' : 'facebook.com/',
            'Facebook2' : 'facebook.com/profile.php?p=',
            'Flickr' : 'flickr.com/photos/',
            'LinkedIn' : 'linkedin.com/in/',
            'Twitter' : 'twitter.com/',
            'YouTube' : 'youtube.com/'
        };      
        this.imgArray = {
            'Facebook' : { 'url' : '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFaceBook.png")%>', 'help' : 'Enter your Facebook user ID, like: <br /><em>'+this.linkArray["Facebook"]+'<em>username</em></em> or<br /><em>facebook.com/profile.php?id=<em>8008355</em></em>', 'hidden' : '<%=hidFacebook.ClientID %>' },
            'Flickr' : { 'url' : '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iFlickr.png")%>', 'help' : 'Enter your Flickr username, like: <br /><em>'+this.linkArray["Flickr"]+'<em>username</em></em>', 'hidden' : '<%=hidFlickr.ClientID %>' },
            'LinkedIn' : { 'url' : '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iLinkedIn.png")%>', 'help' : 'Enter your LinkedIn public profile, like: <br /><em>'+this.linkArray["LinkedIn"]+'<em>username</em></em>', 'hidden' : '<%=hidLinkedIn.ClientID %>' },
            'Twitter' : { 'url' : '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iTwitter.png") %>', 'help' : 'Enter your Twitter username, like: <br /><em>'+this.linkArray["Twitter"]+'<em>username</em></em>', 'hidden' : '<%=hidTwitter.ClientID %>' },
            'YouTube' : { 'url' : '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iYouTube.png")%>', 'help' : 'Enter your YouTube username, like: <br /><em>'+this.linkArray["YouTube"]+'<em>username</em></em>', 'hidden' : '<%=hidYouTube.ClientID %>' }
        };
        this.init();
    };

    Aqufit.Page.Controls.Ati_WebLinks.prototype = {
        init: function () {
            var that = this;
            this.$ddl.change(function(){
                that.$txt.val('');
                $('#helpText').html( that.imgArray[$(this).val()]["help"] );
            });
            $('#helpText').html( that.imgArray[this.$ddl.val()]["help"] );
            $('#bAddLink').button({ icons: { primary: 'ui-icon-plusthick'} }).click(function(event){
                var $li = $('#atiWL'+that.$ddl.val());
                var link = that.$txt.val();
                link = link.replace(/http:\/\//, '');
                link = link.replace(/www./, '');
                var key = that.$ddl.val();
                if( key == 'Facebook' && link.charAt(0) >= '0' && link.charAt(0) <= '9' ){                    
                    key = 'Facebook2';      // the link is numeric.. so use the other facebook url
                }
                link = link.replace(that.linkArray[key], '' );
                link = 'http://' + that.linkArray[key] + link;
                
                that.addListItem(that.$ddl.val(), link);                             
                event.stopPropagation();
                return false;
            });
        },
        addListItem: function(type, link){        
            if( link != '' ){
                var $li = $('#atiWL'+type);
                var html = '<li id="atiWL'+type+'"><a href="'+link+'" target="_blank"><img align="absmiddle" src="'+this.imgArray[type]["url"]+'" /></a> '+link+' <a href="javascript: ;" id="atiWLDel'+type+'">[X]</a></li>';
                if( $li.size() > 0 ){
                    $li.html(html);
                }else{
                    this.$ul.append(html);
                }  
                $('#'+this.imgArray[type]["hidden"]).val(link);
                var that = this;
                $('#atiWLDel'+type).click(function(){
                    $('#atiWL'+type).remove();
                    $('#'+that.imgArray[type]["hidden"]).val('');              
                });
            }
        }
    };
    
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.Ati_WebLinks('<%=this.ID %>', '<%=txtUrl.ClientID %>', '<%=ddlOtherProfiles.ClientID %>', '<%=atiWebLinks.ClientID %>');
        Aqufit.Page.<%=this.ID %>.addListItem('Facebook','<%=this.Facebook %>');
        Aqufit.Page.<%=this.ID %>.addListItem('Twitter','<%=this.Twitter %>');
        Aqufit.Page.<%=this.ID %>.addListItem('YouTube','<%=this.YouTube %>');
        Aqufit.Page.<%=this.ID %>.addListItem('LinkedIn','<%=this.LinkedIn %>');
        Aqufit.Page.<%=this.ID %>.addListItem('Flickr','<%=this.Flickr %>');
    });

</script>   
   
   
<dl>    
    <dt id="dtPersonlUrl" runat="server"><asp:Label id="plPersonlUrl" runat="server" controlname="txtPersonlUrl" text="Personal URL:" /></dt>
    <dd id="ddPersonlUrl" runat="server">
        <asp:TextBox ID="atiPersonlUrl" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="128" Width="300px" />        
    </dd>
    <dt id="dtOtherProfiles" runat="server"><asp:Label id="plOtherProfiles" runat="server" controlname="txtOtherProfiles" text="Other Profiles:" /></dt>
    <dd id="ddOtherProfiles" runat="server">
        <select ID="ddlOtherProfiles" runat="server" class="ui-corner-all ui-widget-content atiTxtBox dull">
            <option Text="Facebook" Value="Facebook" />
            <option Text="Flickr" Value="Flickr" />
            <option Text="LinkedIn" Value="LinkedIn" />
            <option Text="Twitter" Value="Twitter" />
            <option Text="YouTube" Value="YouTube" />
        </select>
        <asp:TextBox ID="txtUrl" runat="server" MaxLength="64" CssClass="ui-corner-all ui-widget-content atiTxtBox" />
        <button id="bAddLink">Add</button>
    </dd>
    <dt id="dtWebLinksHelp" runat="server"></dt>
    <dd id="ddWebLinksHelp" runat="server"><span id="helpText" ></span></dd>
    
    <dt id="dtWebLinks" runat="server"></dt>
    <dd id="ddWebLinks" runat="server">
        <asp:HiddenField ID="hidFacebook" runat="server" />
        <asp:HiddenField ID="hidFlickr" runat="server" />
        <asp:HiddenField ID="hidLinkedIn" runat="server" />
        <asp:HiddenField ID="hidTwitter" runat="server" />
        <asp:HiddenField ID="hidYouTube" runat="server" />
        <ul id="atiWebLinks" runat="server">
        
        </ul>
    </dd>
</dl>