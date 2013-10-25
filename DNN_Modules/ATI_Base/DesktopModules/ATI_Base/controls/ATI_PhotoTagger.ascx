<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_PhotoTagger.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_PhotoTagger" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">

div#atiTagWrap
{
	text-align: left;
}

div#atiTagWrap ul li
{
	list-style: none;
	list-style: none outside none;
}
input #txtFriendFilter 
{
	text-decoration: none;
}
 div.showTag
{
    border: 1px solid red;
}   
</style>

<script type="text/javascript">
    

    Aqufit.Page.Controls.ATI_PhotoTagger = function (id, ctrl) {
        this.id = id;
        this.$ctrl = $('#'+ctrl);
        this.$parent = null;    
        this.allowMove = true;
        this.onCloseHandler = null;
        this.onTagAddHandler = null; 
        this.tagArray = [];      
    };
    Aqufit.Page.Controls.ATI_PhotoTagger.prototype = {
        parentElmId: function( id ){
            this.$parent = $('#'+id);
            this.$parent.append(this.$ctrl.remove());  
            $('#txtFriendFilter').keyup(function(event) {
                Aqufit.Page.<%=this.ID %>.filterFriendList( $(this).val() );   
            });
            $('#atiTagWrap').click(function(event){
                event.stopPropagation();
            });
            $('#txtFriendFilter').focus(function(){
                $(this).removeClass('dull').val('').unbind('focus');
            });            
        },        
        gotoAndShow: function( left, top ){
            this.$ctrl.css('top', top+'px').css('left',left+'px').show();
        },
        hide: function(){
            this.$ctrl.hide();
        },
        moveEvent: function(event){
            if( this.allowMove ){
                var offset = this.$parent.offset();
                this.gotoAndShow(event.pageX-offset.left-(this.$ctrl.width()/2), event.pageY-offset.top-(this.$ctrl.height()/2));
            }
        },
        filterFriendList: function(filter){
            filter = filter.toLowerCase();
            var listbox = $find("<%= RadListBox1.ClientID %>");
            listbox.get_items().forEach(function(item){
                if( ! item.get_text().toLowerCase().indexOf(filter) || filter == '' ){
                    item.ensureVisible();
                }
            });   
        },
        handleClose: function(id, txt){
            this.onCloseHandler(id, txt, this.$ctrl.css('top'), this.$ctrl.css('left'), this.$ctrl.css('width'), this.$ctrl.css('height') );
        },
        clearTags: function(){
            for (var key in this.tagArray){
                this.tagArray[key].elm.remove();
            }            
            this.tagArray = [];
        },
        addTagElement: function(uid, id, txt, top, left, w, h ){            
            this.$parent.append('<div id="tag_'+id+'" style="position: absolute; top:'+top+'px; left:'+left+'px; width: '+w+'px; height:'+h+'px;"></div>');
            $('#tag_'+id).hover(function(){
                $(this).addClass('showTag').append('<span style="display:block; background-color: #fff;">'+txt+'</span');
            },function(){
                $(this).removeClass('showTag').empty();
            });
            this.tagArray['tag_'+id] = { 'id':id, 'elm': $('#tag_'+id), 'txt':txt, 'uid':uid };
            if( this.onTagAddHandler ){
                this.onTagAddHandler(uid, id, txt);
            }
        },
        getTag: function(id){
            return this.tagArray['tag_'+id];
        }
    };

    function RadListBox1_ItemChecked(sender, eventArgs){
        var item = eventArgs.get_item();
        item.set_selected(item.get_checked());
        if( Aqufit.Page.<%=this.ID %>.onCloseHandler ){
            Aqufit.Page.<%=this.ID %>.handleClose( item.get_value(), item.get_text() );
            $('#atiTagWrap input:checked').attr('checked', false);
        }
    };

    $(function () {        
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_PhotoTagger('<%=this.ID %>', '<%=atiPhotoTagWrapper.ClientID %>');                          
    });       
    
</script>

<asp:Panel ID="atiPhotoTagWrapper" runat="server" style="position: absolute; display:none; z-index: 9999;">
    <div style="width: 125px; height: 125px; border:5px solid black;">
        <div style="width: 115px; height: 115px; border: 5px solid white;">
        &nbsp;
        </div>
    </div>
    <div id="atiTagWrap" style="width: 200px; height: 225px; border: 1px solid black; position: absolute; top: 0px; left: 135px; background-color:#FFF;">
        <input id="txtFriendFilter" type="text" maxlength="64" value="filter friends" class="dull" style="display: block; width: 100%; height: 25px;" />
        <telerik:RadListBox ID="RadListBox1" runat="server" CheckBoxes="true" Width="200px" Height="100%" Skin="Black" OnClientItemChecked="RadListBox1_ItemChecked">       
            <Items>            
            </Items>
        </telerik:RadListBox>    
    </div>
</asp:Panel>