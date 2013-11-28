<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Photos.ViewATI_Photos" CodeFile="ViewATI_Photos.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="ati" TagName="PhotoTagger" Src="~/DesktopModules/ATI_Base/controls/ATI_PhotoTagger.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock id="radcodeblock1" runat="server">
<style type="text/css">           
   div#pagePhotos
   {
        min-height: 600px;
   }   
    ul.commentList{
        padding-top: 20px;
    }

    ul.commentList li{
        position: relative;
        min-height: 70px;
    }

    ul.commentList div{
        padding-top: 10px;
        margin-left: 90px;
    }
   
    li.atiPhoto img
    {
    	border: 1px solid #333;	
    	margin: 4px 8px;
    }
    
    li.atiAlbum img
    {
    	border: 1px solid #333;	
    	margin: 4px 8px;
    }
    li.atiAlbum
    {
    	width: 100px;
    } 
    li.atiAlbum div
    {
    	position: relative; 
    	display: inline;     
    } 
    
    div.panelPhotoActions
    {
    	position: relative;
    }
    
    div.panelPhotoActions ul li
    {
    	list-style: none;
    	list-style: none outside none;
    }
    
    div.panelPhotoActions ul li a
    {    	
    	display: block;
    	border-bottom: 1px solid #CCC;
    }          
    
</style>
<script language="javascript" type="text/javascript">  

    Aqufit.Page.Tabs = {
        SwitchTab: function (ind) {
            $('#tabs').tabs('select', ind);
        }
    };

    Aqufit.Page.Actions = {
        photoMode: 'normal',
        navTo: function (p) {
            var album = '';
            var aid = parseInt($('#<%=hiddenAlbumKey.ClientID %>').val());
            if (aid > 0) {
                album = '&a=' + aid;
            }
            self.location.href = '?p=' + p + album;
            /*
            $('#atiTagList').empty();
            top.location.hash = p;
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=imgPhoto.ClientID %>');
            $('#<%=hiddenAjaxValue.ClientID %>').val(p);
            $('#<%=hiddenAjaxAction.ClientID %>').val('loadPhoto');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
            */
        },
        navPrev: function () {
            var p = $('#<%=hiddenPrevPhotoId.ClientID %>').val();
            var album = '';
            var aid = parseInt($('#<%=hiddenAlbumKey.ClientID %>').val());
            if (aid > 0) {
                album = '&a=' + aid;
            }
            self.location.href = '?p=' + p + album;

            /*$('#atiTagList').empty();
            var p = $('#<%=hiddenPrevPhotoId.ClientID %>').val();
            top.location.hash = p;
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=imgPhoto.ClientID %>');
            $('#<%=hiddenAjaxValue.ClientID %>').val(p);
            $('#<%=hiddenAjaxAction.ClientID %>').val('loadPhoto');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
            */
        },
        navNext: function () {
            var p = $('#<%=hiddenNextPhotoId.ClientID %>').val();
            var album = '';
            var aid = parseInt($('#<%=hiddenAlbumKey.ClientID %>').val());
            if (aid > 0) {
                album = '&a=' + aid;
            }
            self.location.href = '?p=' + p + album;
            /*
            $('#atiTagList').empty();
            var p = $('#<%=hiddenNextPhotoId.ClientID %>').val();
            top.location.hash = p;
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=imgPhoto.ClientID %>');
            $('#<%=hiddenAjaxValue.ClientID %>').val(p);
            $('#<%=hiddenAjaxAction.ClientID %>').val('loadPhoto');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
            */
        },
        makeProfilePic: function () {
            var p = $('#<%=hiddenPhotoId.ClientID %>').val();
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=imgPhoto.ClientID %>');
            $('#<%=hiddenAjaxValue.ClientID %>').val(p);
            $('#<%=hiddenAjaxAction.ClientID %>').val('makeProfile');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
        },
        LoadImage: function (src, ind) {
            $('#<%=imgPhoto.ClientID %>').attr('src', src).load(function () { Aqufit.Page.atiLoading.remove(); });
            $('#<%=lPhotoNum.ClientID %>').html(ind);
        },
        doneTagging: function () {
            $('#atiPhotoTagBounds').css('cursor', 'pointer');
            Aqufit.Page.Actions.photoMode = 'normal';
            $('#atiStatusWidget').hide();
            $('#photoOperation').unbind('click').click(function (event) { Aqufit.Page.Actions.navNext(); event.stopPropagation(); return false; });
            Aqufit.Page.atiPhotoTagger.hide();
        },
        showPhotoTagger: function () {
            Aqufit.Page.Actions.photoMode = 'tag';
            $('#atiPhotoTagBounds').css('cursor', 'crosshair');
            $('#atiStatusWidget').show();
            $('#photoOperation').unbind('click').click(function (event) {
                Aqufit.Page.atiPhotoTagger.moveEvent(event);
            });
            Aqufit.Page.atiPhotoTagger.gotoAndShow(100, 100);
        },
        onPhotoTag: function (id, txt, top, left, w, h) {
            $('#<%=hiddenAjaxValue.ClientID %>').val('{ FriendId: ' + id + ', Top:' + parseInt(top) + ', Left:' + parseInt(left) + ', Width:' + parseInt(w) + ', Height:' + parseInt(h) + ' }');
            $('#<%=hiddenAjaxAction.ClientID %>').val('tagPhoto');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
            Aqufit.Page.Actions.doneTagging();
        },
        onTagAdded: function (uid, id, txt) {
            var remHandle = Aqufit.Page.UserSettingsId == uid ? ' <a href="javascript: ;" onclick="Aqufit.Page.Actions.removeTag('+id+');">[Remove Tag]</a> ' : '';
            $('#atiTagList').append('<span><a id="atiShowTag_' + id + '" href="javascript:;">' + txt + '</a>'+remHandle+', </span>');
            $('#atiShowTag_' + id).hover(function () {
                Aqufit.Page.atiPhotoTagger.getTag(id).elm.addClass('showTag');
            }, function () {
                Aqufit.Page.atiPhotoTagger.getTag(id).elm.removeClass('showTag');
            });
        },
        appendComment: function (json) {
            var comment = eval('(' + json + ')');
            $('ul.commentList').append('<li>' +
                                        '<div>' + comment["Comment"] + '</div>' +
                                        '<a href="' + Aqufit.Page.PageBase + comment["UserName"] + '" style="position: absolute; left: 10; top: 10;"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?u=' + comment["UserKey"] + '&p=' + Aqufit.Page.PortalId + '" /></a>' +
                                    '</li>');
            $('#txtPhotoComment').val('');
            Aqufit.Page.atiLoading.remove();
        },
        deletePhoto: function () {
            if (confirm('Are you sure you want to delete the photo?')) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('deletePhoto');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
                Aqufit.Page.atiLoading.addLoadingOverlay('<%=imgPhoto.ClientID %>');
            }
        },
        removeTag: function(id){
            if (confirm('Are you sure you want to delete the tag?')) {
                $('#<%=hiddenAjaxValue.ClientID %>').val(id);
                $('#<%=hiddenAjaxAction.ClientID %>').val('deleteTag');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
                Aqufit.Page.atiLoading.addLoadingOverlay('<%=imgPhoto.ClientID %>');
            }
        },
        onLoadHelper: null
    };

    Aqufit.Windows.UploadWin = {
        win: null,
        close: function (oWnd, args) {
            Aqufit.Windows.UploadWin.win.close();
        },
        open: function (arg) {
            Aqufit.Windows.UploadWin.win = radopen('<%=ResolveUrl("~/Profile/UploadPic.aspx") %>' + arg, 'UploadWin');
            Aqufit.Windows.UploadWin.win.set_modal(true);
            Aqufit.Windows.UploadWin.win.setSize(650, 400);
            Aqufit.Windows.UploadWin.win.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
            Aqufit.Windows.UploadWin.win.center();
            Aqufit.Windows.UploadWin.win.show();
            return false;
        }
    };

    function OnResponseEnd(){
       // Aqufit.Page.atiLoading.remove();
    }

    $(function () {
        $('#tabs').tabs();
        $('#bPostComment').button().click(function (event) {
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=atiPhotoComment.ClientID %>');
            var comment = $('#txtPhotoComment').val();
            $('#<%=hiddenAjaxValue.ClientID %>').val(comment);
            $('#<%=hiddenAjaxAction.ClientID %>').val('addComment');
            __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
            event.stopPropagation();
            return false;
        });
        $('#<%= atiWorkoutAlbum.ClientID %>').button();
        $('#<%= atiWorkoutAlbum.ClientID %>, #bNewAlbum').click(function (event) {
            Aqufit.Windows.UploadWin.open('?a=0');
            event.stopPropagation();
            return false;
        });
        $('#<%=bDeleteAlbum.ClientID %>').button().click(function (event) {
            if (confirm("All the photos in this album with be deleted. Are you sure you want to continue?")) {
                $('#<%=hiddenAjaxAction.ClientID %>').val('deleteAlbum');
                __doPostBack('<%=bAjaxPostback.ClientID.Replace("_","$").Replace("ATI$Photos","ATI_Photos") %>', '');
            }
            event.stopPropagation();
            return false;
        });
        if ($('#photoOperation').size() > 0) {
            $('#photoOperation').click(function (event) { Aqufit.Page.Actions.navNext(); event.stopPropagation(); return false; });
        }
        if ($('#bDoneTagging').size() > 0) {
            $('#bDoneTagging').button().click(function (event) {
                Aqufit.Page.Actions.doneTagging();
                event.stopPropagation();
                return false;
            });
        }
    });

    Aqufit.addLoadEvent(function () {
        Aqufit.Page.atiPhotoTagger.parentElmId('atiPhotoTagBounds');
        Aqufit.Page.atiPhotoTagger.onCloseHandler = Aqufit.Page.Actions.onPhotoTag;
        Aqufit.Page.atiPhotoTagger.onTagAddHandler = Aqufit.Page.Actions.onTagAdded;
        <%=StartupFunction %>
        if (Aqufit.Page.Actions.onLoadHelper) {
            Aqufit.Page.Actions.onLoadHelper();
        }
        if (self.location.hash != '') {
            Aqufit.Page.Actions.navTo(self.location.hash.substr(1));
        }      
    });
    
</script>
</telerik:RadCodeBlock>

<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <ClientEvents OnResponseEnd="OnResponseEnd"></ClientEvents>
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="panelAjax">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="panelAjax"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>  
        <telerik:AjaxSetting AjaxControlID="RadListViewTags">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadListViewTags" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>  
        <telerik:AjaxSetting AjaxControlID="RadListViewAlbumPhotos">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadListViewAlbumPhotos" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>  
        <telerik:AjaxSetting AjaxControlID="RadListViewAlbum">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadListViewAlbum" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>    
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />   


<ati:LoadingPanel ID="atiLoading" runat="server" />
<ati:PhotoTagger ID="atiPhotoTagger" runat="server" />

 <asp:Panel ID="panelAjax" runat="server" >   
    <asp:HiddenField ID="hiddenAjaxAction" runat="server" />
    <asp:HiddenField ID="hiddenAjaxValue" runat="server" />
    <asp:HiddenField ID="hiddenNextPhotoId" runat="server" />
    <asp:HiddenField ID="hiddenPhotoId" runat="server" />
    <asp:HiddenField ID="hiddenPrevPhotoId" runat="server" />
    <asp:HiddenField ID="hiddenAlbumKey" runat="server" />
    <asp:Button ID="bAjaxPostback" runat="server" Style="display: none;" OnClick="bAjaxPostback_Click" />
</asp:Panel>

    <!-- Start of a 3 col box layout -->    
    <asp:Panel ID="panelAll" runat="server">
    <div id="divCenterWrapper" style="position: relative;">        
        <div id="divMain" style="width: 767px; position: relative;">            
            <!-- Tabs -->
            <ul class="hlist atiTopButtons">                
                <li><button id="atiWorkoutAlbum" runat="server">+ Create a photo album</button></li>
            </ul>
    		<div id="tabs">
    			<ul>
    				<li id="tabPhotos"><a href="#pagePhotos"><asp:Literal ID="photoTabTitle" runat="server" /></a></li>    				              
                </ul>
    			<div id="pagePhotos" style="padding: 0px; background-color: White;">                                                                                                        
                    <asp:Panel ID="panelPhotoList" runat="server">
                    <div class="atiListHeading grad-FFF-EEE">
                        <button id="bDeleteAlbum" runat="server" style="float: right;" visible="false">Delete Album</button>
                        <h3 id="atiPhotosTitle" runat="server"></h3>                        
                        <asp:Literal ID="litPhotoNav" runat="server" />                        
                    </div>
                    <div id="divPhotoArea">

                        <telerik:RadListView ID="RadListViewTags" runat="server" ItemPlaceholderID="PlaceHolder1" AllowPaging="true" DataSourceID="EntityDataSource1" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <asp:Panel ID="PhotoPanel" runat="server">                                        
                                        <ul class="hlist">
                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                        </ul>
                                        &nbsp;
                                    </asp:Panel>     
                                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PageSize="15" Skin="Default" >
                                        <Fields>
                                            <telerik:RadDataPagerSliderField />
                                            <telerik:RadDataPagerButtonField FieldType="Numeric" />                                        
                                            <telerik:RadDataPagerPageSizeField PageSizeText="Page size: " />
                                        </Fields>
                                    </telerik:RadDataPager>                             
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li class="category atiPhoto">                                     
                                        <a href="?p=<%#Eval("Photo.Id")%>"><img ID="photoNorm" runat="server"  src='<%#Eval("Photo.ThumbUri")%>' /></a>                                        
                                    </li>
                                </ItemTemplate>
                                <SelectedItemTemplate>
                                    <li class="selected atiPhoto">                                       
                                        <a href="?p=<%#Eval("Photo.Id")%>"><img ID="photoSelected" runat="server" src='<%#Eval("Photo.ThumbUri") %>' /></a>                                    
                                    </li>
                                </SelectedItemTemplate>
                                <EmptyDataTemplate>
                                    <div style="padding: 50px 100px;">
                                        <h2>You have not yet been tagged in any photos.</h2>
                                    </div>
                                </EmptyDataTemplate>
                            </telerik:RadListView>
                            
                            <asp:EntityDataSource ID="EntityDataSource1" runat="server" 
                                    ContextTypeName="Affine.Data.aqufitEntities"
                                    EntitySetName="User2Photo" Include="Photo"
                                    EnableUpdate="False" EnableDelete="True" EnableInsert="False"
                                    EntityTypeFilter="" Select=""
                                    OrderBy="it.[Id] DESC" 
                                    AutoGenerateWhereClause="true">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="hiddenUserSettingsKey" DbType="Int64" Name="UserSettingsKey" PropertyName="Value" />                                 
                            </WhereParameters>
                            </asp:EntityDataSource>
                        

                         <telerik:RadListView ID="RadListViewAlbumPhotos" runat="server" ItemPlaceholderID="PlaceHolder1" AllowPaging="true" DataSourceID="eds" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <asp:Panel ID="PhotoPanel" runat="server">                                        
                                        <ul class="hlist">
                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                        </ul>
                                        &nbsp;
                                    </asp:Panel>     
                                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PageSize="15" Skin="Default" >
                                        <Fields>
                                            <telerik:RadDataPagerSliderField />
                                            <telerik:RadDataPagerButtonField FieldType="Numeric" />                                        
                                            <telerik:RadDataPagerPageSizeField PageSizeText="Page size: " />
                                        </Fields>
                                    </telerik:RadDataPager>                             
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li class="category atiPhoto">                                     
                                        <a href="?p=<%#Eval("Id")+(Request["a"] != null ? "&a="+Request["a"] : "")%>"><img ID="photoNorm" runat="server"  src='<%#Eval("ThumbUri")%>' /></a>                                        
                                    </li>
                                </ItemTemplate>
                                <SelectedItemTemplate>
                                    <li class="selected atiPhoto">                                       
                                        <a href="?p=<%#Eval("Id")%>"><img ID="photoSelected" runat="server" src='<%#Eval("ThumbUri") %>' /></a>                                    
                                    </li>
                                </SelectedItemTemplate>
                                <EmptyDataTemplate>
                                    <div style="padding: 50px 100px;">
                                        <h2>You have not yet been tagged in any photos.</h2>
                                    </div>
                                </EmptyDataTemplate>
                            </telerik:RadListView>
                            <asp:HiddenField ID="hiddenUserSettingsKey" runat="server" />
                            
                            <asp:EntityDataSource ID="eds" runat="server" 
                                    ContextTypeName="Affine.Data.aqufitEntities"
                                    EntitySetName="UserAttachments"
                                    EntityTypeFilter="Photo" 
                                    EnableUpdate="False" EnableDelete="True" EnableInsert="False"
                                    Select=""
                                    OrderBy="it.[Id] DESC" 
                                    AutoGenerateWhereClause="true">
                            <WhereParameters>
                                <asp:ControlParameter ControlID="hiddenUserSettingsKey" DbType="Int64" Name="UserSetting.Id" PropertyName="Value" /> 
                                <asp:ControlParameter ControlID="hiddenAlbumKey" DbType="Int64" Name="Album.Id" PropertyName="Value" /> 
                            </WhereParameters>
                            </asp:EntityDataSource>
                        </div>

                        <asp:Panel id="atiAlbumList" runat="server">
                            <div class="atiListHeading grad-FFF-EEE">
                                <h3>Albums</h3>
                            </div>
                            <div id="albumArea">
                                <telerik:RadListView ID="RadListViewAlbum" runat="server" ItemPlaceholderID="PlaceHolder1" AllowPaging="true" DataSourceID="eds2" DataKeyNames="Id">
                                    <LayoutTemplate>
                                        <asp:Panel ID="PhotoPanel" runat="server">                                        
                                            <ul class="hlist">
                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                            </ul>
                                            &nbsp;
                                        </asp:Panel>     
                                        <telerik:RadDataPager ID="RadDataPager1" runat="server" PageSize="5" Skin="Default" >
                                            <Fields>
                                                <telerik:RadDataPagerSliderField />
                                                <telerik:RadDataPagerButtonField FieldType="Numeric" />                                                                                    
                                            </Fields>
                                        </telerik:RadDataPager>                             
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <li class="category atiAlbum"> 
                                            <div>                                    
                                                <a href="?a=<%#Eval("Id")%>" title="<%#Eval("Name")%>"><img ID="photoNorm" runat="server"  src='<%#Eval("Photo.ThumbUri")%>' style="margin-bottom: 30px;" /></a>
                                                <a href="?a=<%#Eval("Id")%>" title="<%#Eval("Name")%>" style="position: absolute; bottom: 0px; left: 5px; font-weight: bold;"><%#Eval("Name")%></a>     
                                            </div>                              
                                        </li>
                                    </ItemTemplate>
                                    <SelectedItemTemplate>
                                        <li class="selected atiPhoto">                                       
                                            <a href="?a=<%#Eval("Id")%>" title="<%#Eval("Name")%>"><img ID="photoSelected" runat="server" src='<%#Eval("Photo.ThumbUri") %>' /></a>                                    
                                        </li>
                                    </SelectedItemTemplate>
                                    <EmptyDataTemplate>
                                        <% if (base.Permissions == AqufitPermission.OWNER)
                                           { %>
                                        <li class="selected atiPhoto">                                       
                                            <a id="bNewAlbum" href="javascript: ;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/iCreateAlbum.png")%>" /></a>                                    
                                        </li>
                                        <%} %>
                                    </EmptyDataTemplate>
                                </telerik:RadListView>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:EntityDataSource ID="eds2" runat="server" 
                                        ContextTypeName="Affine.Data.aqufitEntities"
                                        EntitySetName="Albums" Include="Photo"
                                        EnableUpdate="False" EnableDelete="True" EnableInsert="False"
                                        AutoGenerateWhereClause="true">
                                <WhereParameters>
                                    <asp:ControlParameter ControlID="hiddenUserSettingsKey" DbType="Int64" Name="UserSetting.Id" PropertyName="Value" /> 
                                </WhereParameters>
                                </asp:EntityDataSource>
                            </div>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="panelPhoto" runat="server" Visible="false" style="padding: 20px;">                        
                        <div>
                            <h2 style="margin: 0;">Photos of <asp:Literal ID="litProfileName" runat="server" /></h2>
                            <span>Photo <asp:Label ID="lPhotoNum" runat="server" /> of <asp:Literal ID="litPhotoCount" runat="server" /> (<a id="hlBackAlbums" runat="server">back to photos</a>)</span>
                            <span style="float: right;"><a href="javascript: Aqufit.Page.Actions.navPrev();">prev</a>  |  <a href="javascript: Aqufit.Page.Actions.navNext();">next</a></span>
                            <center><a id="photoOperation" href="javascript: ;"><div id="atiPhotoTagBounds" style="margin: 20px 0px; position: relative;"><img id="imgPhoto" runat="server" style="border: 1px solid #333;" /></div></a></center>
                        </div>

                        <div id="atiStatusWidget" class="ui-widget" style="display: none;">
	                        <div class="ui-state-highlight ui-corner-all" style="margin-bottom: 20px; padding: 18px;"> 
		                        <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		                        <span id="statusMsg">Click on peoples faces in the photo to tag them.</span>
                                <button id="bDoneTagging" style="float: right;">Done tagging</button>
	                        </div>
                        </div>

                        <div class="panelPhotoActions">
                            <span>In this photo: </span><span id="atiTagList"></span>
                            <div style="width: 500px; min-height: 75px;">
                                
                                <telerik:RadListView ID="RadListView1" runat="server" ItemPlaceholderID="PlaceHolder1" AllowPaging="true" DataSourceID="edsComments" DataKeyNames="Id">
                                <LayoutTemplate>
                                    <asp:Panel ID="PhotoPanel" runat="server">                                        
                                        <ul class="commentList">
                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                        </ul>
                                    </asp:Panel>                                                         
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li>    
                                        <div><%#Eval("Comment")%></div>                                 
                                        <a href="<%=ResolveUrl("~") %><%#Eval("UserSetting.UserName")%>" style="position: absolute; left: 10; top: 10;"><img src="<%=ResolveUrl("~/DesktopModules/ATI_Base/services/images/profile.aspx") %>?u=<%#Eval("UserSetting.UserKey")%>&p=<%#Eval("UserSetting.PortalKey")%>" /></a>                                        
                                    </li>
                                </ItemTemplate>  
                                </telerik:RadListView>
                            
                                <asp:EntityDataSource ID="edsComments" runat="server" 
                                        ContextTypeName="Affine.Data.aqufitEntities" 
                                        EntitySetName="AttachmentComments" Include="UserSetting" 
                                        EnableUpdate="False" EnableDelete="True" EnableInsert="False"
                                        AutoGenerateWhereClause="true">
                                <WhereParameters>
                                    <asp:ControlParameter ControlID="hiddenPhotoId" DbType="Int64" Name="UserAttachment.Id" PropertyName="Value" /> 
                                </WhereParameters>
                                </asp:EntityDataSource>                                
                                                                                                
                                <div id="atiPhotoComment" runat="server" style="width: 100%; background: #EEE; position: relative; height: 100px;">
                                    <img id="imgThisUser" runat="server" style="margin: 10px;" />
                                    <textarea id="txtPhotoComment" style="position: absolute; top: 10px; right: 10px; width: 400px;"></textarea>
                                    <button id="bPostComment" style="position: absolute; bottom: 10px; right: 10px;">Post Comment</button>
                                </div>
                            </div>
                            <ul style="position: absolute; right: 0px; top: 0px; width: 200px;">
                                <li>From the Album: </li>
                                <li><asp:Literal ID="litAlbumName" runat="server" /></li>
                                <li>&nbsp;</li>
                                <li><a href="javascript: Aqufit.Page.Actions.showPhotoTagger();">Tag photo</a></li>
                                <li><a href="javascript: Aqufit.Page.Actions.makeProfilePic();">Make profile picture</a></li>
                                <li>&nbsp;</li>
                                <li id="liDeletePhoto" runat="server" visible="false"><a href="javascript: Aqufit.Page.Actions.deletePhoto();">Delete Photo</a></li>
                                <!-- TODO: 
                                <li><a href="javascript: ;">Report photo</a></li> -->
                            </ul>                           
                        </div>
                    </asp:Panel>
                    
                </div>
          			
    		</div>
            <!-- END Tabs -->                       
        </div>
        <div id="divRightAdUnit" style="width: 160px; position: absolute; right: 0; top: 0;">
            <img runat="server" id="imgAd" />
        </div>
    
    </div>   
    <div style="clear:both;"></div>        
    </asp:Panel>    