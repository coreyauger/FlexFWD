<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Preview.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_Preview Settings Design Table">   
   <tr>
        <td><asp:Label id="lLoginDestination" runat="server" controlname="txtLandingPage" Text="Investor&nbsp;Login&nbsp;Dest:" /></td>
        <td><asp:TextBox ID="txtLandingPage" runat="server" MaxLength="511" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lBlogUrl" runat="server" controlname="txtLandingPage" Text="Blog&nbsp;Url:" /></td>
        <td><asp:TextBox ID="txtBlogUrl" runat="server" MaxLength="511" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lSalesPhone" runat="server" controlname="txtSalesPhone" Text="Sales&nbsp;Phone:" /></td>
        <td><asp:TextBox ID="txtSalesPhone" runat="server" MaxLength="32" /></td>
   </tr>
   <tr>
        <td><asp:Label id="lWelcomeText" runat="server" controlname="txtWelcome" Text="More&nbsp;Info&nbsp;Text:" /></td>
        <td>
             <telerik:radeditor Skin="Office2007" EnableResize="false" runat="server" ID="RadEditorWelcomeText" SkinID="DefaultSetOfTools" Width="700px" Height="350px">
             <ImageManager
                ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations" >
             </ImageManager>
             <Content>             
             </Content>          
             </telerik:radeditor>
        </td>
   </tr>
   <tr>
        <td><asp:Label id="lBetaTestText" runat="server" controlname="txtBetaTest" Text="Beta&nbsp;Tester&nbsp;Text:" /></td>
        <td>
             <telerik:radeditor Skin="Office2007" EnableResize="false" runat="server" ID="RadEditorBetaTestText" SkinID="DefaultSetOfTools" Width="700px" Height="350px">
             <ImageManager
                ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations" >
             </ImageManager>
             <Content>             
             </Content>          
             </telerik:radeditor>
        </td>
   </tr>
   <tr>
        <td><asp:Label id="lInvestorText" runat="server" controlname="txtInvestorText" Text="Investor&nbsp;Text:" /></td>
        <td>
             <telerik:radeditor Skin="Office2007" EnableResize="false" runat="server" ID="RadEditorInvestorText" SkinID="DefaultSetOfTools" Width="700px" Height="350px">
             <ImageManager
                ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations" >
             </ImageManager>
             <Content>             
             </Content>          
             </telerik:radeditor>
        </td>
   </tr>
   <tr>
        <td><asp:Label id="lThankYouText" runat="server" controlname="txtThankYouText" Text="Thank&nbsp;You&nbsp;Text:" /></td>
        <td>
             <telerik:radeditor Skin="Office2007" EnableResize="false" runat="server" ID="RadEditorThankYouText" SkinID="DefaultSetOfTools" Width="700px" Height="350px">
             <ImageManager
                ViewPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                UploadPaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations"
                DeletePaths="~/Editor/Img/UserDir/Marketing,~/Editor/Img/UserDir/PublicRelations" >
             </ImageManager>
             <Content>             
             </Content>          
             </telerik:radeditor>
        </td>
   </tr>
</table>
