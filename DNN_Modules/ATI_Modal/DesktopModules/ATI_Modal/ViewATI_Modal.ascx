<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Modal.ViewATI_Modal" CodeFile="ViewATI_Modal.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="ati" TagName="Stream" Src="~/DesktopModules/ATI_Base/controls/ATI_StreamControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="LoadingPanel" Src="~/DesktopModules/ATI_Base/controls/ATI_LoadingPanel.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
<script language="javascript" type="text/javascript">

    Aqufit.Page.Actions = {
        Save: function () {
            Aqufit.Page.atiLoading.addLoadingOverlay('<%=atiUploadAlbumPhotos.ClientID %>');
        }
    };

    $(function () {
        $('#bNext').button().click(function (event) {
            if ($('#<%=txtAlbumName.ClientID %>').val() != '') {
                $('#step1').hide();
                $('#step2').show();
            }
            event.stopPropagation();
            return false;
        });
    });

</script>
</telerik:RadCodeBlock>


<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">  
    <AjaxSettings>       
        <telerik:AjaxSetting AjaxControlID="RadProgressArea1">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RadProgressArea1"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>   
     </AjaxSettings>      
</telerik:RadAjaxManager>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Office2007" />

<ati:LoadingPanel ID="atiLoading" runat="server" />
<!--
<div style="width: 728; height: 90px; background-color: #CCCCCC; border: 1px solid #666666; text-align: center;">
    AD UNIT
</div>
-->
<!-- Ajaxing this does not allow us to have the file post back -->
<asp:Panel ID="atiUploadProfileImgPanel" runat="server">
    <div class="atiListHeading grad-FFF-EEE" style="border-right: 1px solid #ccc; border-left: 1px solid #ccc;">
        <h3 id="h3UploadTitle" runat="server"></h3>
    </div>
    <div style="margin: 50px auto; width: 300px;">
        <asp:FileUpload ID="fileUpload" runat="server" />
        <asp:Button ID="bUpload" runat="server" Text="Save" OnClick="bUpload_Click" CssClass="boldButton" />
        <asp:Panel ID="panelAlbumChoose" runat="server">
            <br />
            <asp:Label ID="lStatus" runat="server" />
            <br />
            <h3>or choose one from your Albums</h3>
            <a id="hlAlbum" runat="server">click here</a>
        </asp:Panel>
    </div>
</asp:Panel>    

<asp:Panel ID="atiUploadAlbumPhotos" runat="server" Visible="false">
    <div class="atiListHeading grad-FFF-EEE" style="border-right: 1px solid #ccc; border-left: 1px solid #ccc;">
        <h3>Create a new photo album.</h3>
    </div>    
    <asp:Panel ID="atiAlbum" runat="server">
    <div style="margin: 50px auto; width: 300px; position: relative;">
        <div id="step1">
            <asp:Label id="plAlbumName" runat="server" controlname="txtAlbumName" text="Album&nbsp;Name:" />
            <asp:TextBox ID="txtAlbumName" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="255" />
            <asp:RequiredFieldValidator ID="rfvAlbumName" runat="server" ValidationGroup="Album" ControlToValidate="txtAlbumName" ErrorMessage="<a href='javascript: focusOnErrorControl(this);'>Album Name</a> required!" Text="*" />
            <button id="bNext">Next</button>
        </div>
        <div id="step2" style="display: none;">
            <asp:Button ID="bAsyncUpload" runat="server" Text="Save" OnClick="bAsyncUpload_Click" ValidationGroup="Album" OnClientClick="Aqufit.Page.Actions.Save();" CssClass="boldButton" style="position: absolute; top: 25px; right: -50px;" />
            <span>Select multiple files to upload by holding "ctrl" button down while selecting.</span>
            <br /><br />
            <telerik:RadProgressManager runat="server" ID="RadProgressManager1" />                                                                                      
            <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1" MultipleFileSelection="Automatic" AllowedFileExtensions="jpeg,jpg,gif,png">
            </telerik:RadAsyncUpload>

            <telerik:RadProgressArea runat="server" ID="RadProgressArea1">
            </telerik:RadProgressArea>           
        </div>
    </div>
    </asp:Panel>


    

    


</asp:Panel>

    
   


    

    

         
    
               



