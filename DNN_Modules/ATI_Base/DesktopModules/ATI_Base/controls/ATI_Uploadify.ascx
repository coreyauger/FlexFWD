<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_Uploadify.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_Uploadify" %>
<script src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/jquery.uploadify.v2.1.0.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/scripts/swfobject.js")%>" type="text/javascript"></script>
<link href="<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/css/uploadify.css")%>" rel="stylesheet" type="text/css"></link>


<script type="text/javascript">
       // <![CDATA[  
       Aqufit.Page.Controls.Uploadify = function(id, control, action, callback){
            this.id = id;
            this.control = '#' + control;
            this.action = action;
            this.rid = 0;
            this.callback = callback;
            this.isInit = false;
       };               
       
    Aqufit.Page.Controls.Uploadify.prototype = {
        setRecipeId: function(rid){
            this.rid = rid;        
            this.init();   
        },
        setCallback: function(callback){
            this.callback = callback;
        },
        hide: function(){
            $(this.control).hide();
        },
        show: function(){
            $(this.control).show();
        },
        init: function () {      
            this.isInit = true;
            var that = this;      
            $('#fileInput'+this.id).uploadify({
                'uploader': '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/flex/uploadify.swf") %>',
                'script': '<%=ResolveUrl("~/DesktopModules/ATI_Base/UploadifyHandler.ashx")%>',
                'scriptData': { 'uid': Aqufit.Page.UserId, 'pid': Aqufit.Page.PortalId, 't' : this.action, 'rid' : this.rid },
                'cancelImg': '<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/cancel.png")%>',
                'auto': true,
                'multi': false,
                'fileDesc': 'Image Files',
                'fileExt': '*.jpg;*.png;*.gif;*.bmp;*.jpeg',
                'queueSizeLimit': 1,
                'sizeLimit': 4000000,
                'buttonText': 'Choose Image',
                'folder': '/uploads',
                'onComplete': function (event, queueID, fileObj, response, data) {
                    if (response == 1) {
                        that.callback.call();                        
                    }
                }
            });                
        }
    };
    
    
    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.Uploadify('<%=this.ID %>', '<%=atiUploadifyPanel.ClientID %>','<%=this.Action %>', <%=this.SucessCallbackFunction %>);       
        //Aqufit.Page.<%=this.ID %>.init();
    });  
  
   // ]]></script>
   <div id="atiUploadifyPanel" style="background-color: White; padding: 18px;" runat="server">
       <div class="ui-corner-all ui-state-highlight" id="infoHeader" runat="server" style="margin-top: 20px; padding: 0 .7em; margin-bottom: 8px;"> 
    		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
    		<asp:Label ID="infoHeaderText" runat="server" /></p>
    	</div>   
       <div style="width: 100%; text-align: center;">       
            <input id="fileInput<%=this.ID %>" name="fileInput" type="file" />
       </div>
   </div>
