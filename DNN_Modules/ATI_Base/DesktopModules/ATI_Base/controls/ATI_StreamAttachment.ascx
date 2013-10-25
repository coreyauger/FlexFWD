<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_StreamAttachment.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_StreamAttachment" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>

<script type="text/javascript">
    Aqufit.Page.Controls.ATI_StreamAttachment = function (id, hpk, hlk, hvk, txtLinkUrl, imgId, linkEditId, wrap, linkDisplay) {
        this.id = id;
        this.hiddenPhotoKey = hpk;
        this.$hiddenLink = $('#'+hlk);
        this.hiddenVideoKey = hvk;
        this.wrap = wrap;
        this.$imgId = $('#'+imgId);
        this.$linkEdit = $('#'+linkEditId);
        this.$txtLinkUrl = $('#'+txtLinkUrl);
        this.$linkDisplay = $('#'+linkDisplay);
        this.pageMetaData = null;
        var that = this;
        this.$linkEdit.find('button').button().click(function(event){
            // TODO: need link parsing details.
            Aqufit.Page.atiLinkLoad.addLoadingOverlay(that.wrap);
            Affine.WebService.StreamService.GetPageMetaData(that.$txtLinkUrl.val(),function(json){
                that.pageMetaData = eval('('+json+')');
                if( that.pageMetaData.Status == 200 ){
                    that.$linkDisplay.find('#imgSlideInfo').html( (that.pageMetaData.SelectedImgInd+1) +' of ' + that.pageMetaData.Images.length );
                    that.$hiddenLink.val(Aqufit.Serialize(that.pageMetaData));
                    if( that.pageMetaData.Images.length > 0 ){
                        that.$linkDisplay.find('#imgSelector').css('background-image','url('+that.pageMetaData.Images[0].Src+')');
                        that.$linkDisplay.find('ul').html('<li><a href="javascript: ;">'+that.$txtLinkUrl.val()+'</a><li>'+
                                                            '<li><em>'+that.pageMetaData.Title+'</em></li>'+
                                                            '<li>'+that.pageMetaData.Description+'</li>');
                        that.$linkEdit.hide();
                        Aqufit.Page.atiLinkLoad.remove();
                        that.$linkDisplay.show();
                    }
                }else{
                    alert(that.pageMetaData.ErrorMsg);
                    Aqufit.Page.atiLinkLoad.remove();
                }
            });
            event.stopPropagation();
            return false;
        });
        this.$linkDisplay.find('button:eq(0)').button().click(function(event){            
            that.pageMetaData.SelectedImgInd--;
            if( that.pageMetaData.SelectedImgInd < 0 ){
                that.pageMetaData.SelectedImgInd = that.pageMetaData.Images.length-1;
            }
            that.$linkDisplay.find('#imgSlideInfo').html( (that.pageMetaData.SelectedImgInd+1) +' of ' + that.pageMetaData.Images.length );
            that.$linkDisplay.find('#imgSelector').css('background-image','url('+that.pageMetaData.Images[that.pageMetaData.SelectedImgInd].Src+')');
            that.$hiddenLink.val(Aqufit.Serialize(that.pageMetaData));
            event.stopPropagation();
            return false;
        });
        this.$linkDisplay.find('button:eq(1)').button().click(function(event){            
            that.pageMetaData.SelectedImgInd++;
            if( that.pageMetaData.SelectedImgInd >= that.pageMetaData.Images.length ){
                that.pageMetaData.SelectedImgInd = 0;
            }
            that.$linkDisplay.find('#imgSlideInfo').html( (that.pageMetaData.SelectedImgInd+1) +' of ' + that.pageMetaData.Images.length );
            that.$linkDisplay.find('#imgSelector').css('background-image','url('+that.pageMetaData.Images[that.pageMetaData.SelectedImgInd].Src+')');
            that.$hiddenLink.val(Aqufit.Serialize(that.pageMetaData));
            event.stopPropagation();
            return false;
        });
    };

    Aqufit.Page.Controls.ATI_StreamAttachment.prototype = {
        ImageUploadSuccess: function(uid, pid, thumbUrl){
            $('#'+this.hiddenPhotoKey).val(pid);
            Aqufit.Windows.UploadWin.win.close();
            // load the thumb preview
            this.$imgId.attr('src', thumbUrl).show();
        },
        openLinkEditor: function(){
            if( this.$linkEdit.is(':visible') ){
                this.$linkEdit.slideUp('fast');
            }else{
                this.$linkEdit.slideDown('fast');
            }
        },
        clear: function(){
            $('#'+this.hiddenPhotoKey).val('');
            this.$hiddenLink.val('');
            $('#'+this.hiddenVideoKey).val('');
            this.$txtLinkUrl.val('');
            this.$imgId.hide();
            this.$linkEdit.hide();
            this.$linkDisplay.hide();            
        }
    };

    $(function () { 
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_StreamAttachment('<%=this.ID %>','<%=hiddenAttachmentPhotoKey.ClientID %>','<%=hiddenAttachmentLinkKey.ClientID %>', '<%=hiddenAttachmentVideoKey.ClientID %>', '<%=txtLinkUrl.ClientID %>', '<%=imgAttachment.ClientID %>','<%=linkEditor.ClientID %>', '<%=controlWrap.ClientID %>','<%=linkDisplay.ClientID %>');
    });
</script>
<asp:Panel ID="controlWrap" runat="server">
<ati:LoadingPanel ID="atiLinkLoad" runat="server" />
<asp:HiddenField ID="hiddenAttachmentPhotoKey" runat="server" />
<asp:HiddenField ID="hiddenAttachmentLinkKey" runat="server" />
<asp:HiddenField ID="hiddenAttachmentVideoKey" runat="server" />
<ul class="hlist attachments" style="padding-bottom: 10px;">
    <li><a href="javascript: ;" onclick="Aqufit.Windows.UploadWin.open('?sap=0&c=<%=this.ID %>');"><img id="imgPhoto" runat="server" />Photo</a></li>
    <li><a href="javascript: ;" onclick="Aqufit.Page.<%=this.ID %>.openLinkEditor();"><img id="imgLink" runat="server" />Link</a></li>
    <li></li>
</ul>
<img id="imgAttachment" runat="server" style="display: none; padding-bottom: 10px;" />
<div id="linkEditor" runat="server" style="display: none;">
    <ul class="hlist">
        <li>http://</li>
        <li><asp:TextBox ID="txtLinkUrl" runat="server" MaxLength="512" CssClass="ui-corner-all ui-widget-content atiTxtBox" Width="300px" /></li>
        <li><button>done</button></li>
    </ul>
</div>
<div id="linkDisplay" style="position: relative;display: none;" runat="server">
    <div id="imgSelector" style="width: 100px; height: 100px; background-size: 100%; background-repeat:no-repeat;">
    
    </div>
    <span style="position: absolute; top: 0px; left: 110px;">
    <button>&lt;</button><span id="imgSlideInfo" style="padding: 0px 5px;"></span><button>&gt;</button>
    </span>
    <ul style="position: absolute; top: 0px; left: 235px;">        
    </ul>
</div>
</asp:Panel>