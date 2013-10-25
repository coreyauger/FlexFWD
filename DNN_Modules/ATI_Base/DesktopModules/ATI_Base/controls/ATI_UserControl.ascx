<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_UserControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_UserControl" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<table cellSpacing="0" cellPadding="0" border="0" summary="User Design Table">
	<tr>
		<td class="SubHead ati_Form_Text2" width="275"><dnn:label id="plFirstName" runat="server" controlname="txtFirstName" text="First&nbsp;Name:"></dnn:label></td>
		<td class="NormalBold ati_Form_Control2" noWrap><asp:textbox id="txtFirstName" tabIndex="1" runat="server" cssclass="NormalTextBox"  Width="200px" maxlength="50"></asp:textbox></td>	    
	    <td class="ati_Form_Texture">&nbsp;</td>
	    <td class="ati_Form_Validate">*&nbsp;&nbsp;<asp:requiredfieldvalidator id="valFirstName" runat="server" cssclass="NormalRed" display="Static" errormessage="&nbsp;&nbsp;Required."
				controltovalidate="txtFirstName" resourcekey="valFirstName"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td class="SubHead ati_Form_Text2"><dnn:label id="plLastName" runat="server" controlname="txtLastName" text="Last&nbsp;Name:"></dnn:label></td>
		<td class="NormalBold ati_Form_Control2" noWrap><asp:textbox id="txtLastName" tabIndex="2" runat="server" cssclass="NormalTextBox"  Width="200px" maxlength="50"></asp:textbox></td>			
	    <td class="ati_Form_Texture">&nbsp;</td>
	    <td class="ati_Form_Validate">*&nbsp;&nbsp;<asp:requiredfieldvalidator id="valLastName" runat="server" cssclass="NormalRed" display="Static" errormessage="&nbsp;&nbsp;Required."
				controltovalidate="txtLastName" resourcekey="valLastName"></asp:requiredfieldvalidator></td>
	</tr>	
	<tr id="PasswordRow" runat="server" visible="true">
		<td class="SubHead ati_Form_Text2"><dnn:label id="plPassword" runat="server" controlname="txtPassword" text="Password:"></dnn:label></td>
		<td class="NormalBold ati_Form_Control2" noWrap><asp:textbox id="txtPassword" tabIndex="4" runat="server" cssclass="NormalTextBox"  Width="200px" maxlength="20" textmode="Password"></asp:textbox></td>			
	    <td class="ati_Form_Texture">&nbsp;</td>
	    <td class="ati_Form_Validate">*<asp:requiredfieldvalidator id="valPassword" runat="server" cssclass="NormalRed" display="Static" errormessage="&nbsp;&nbsp;Required."
				controltovalidate="txtPassword" resourcekey="valPassword"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="ConfirmPasswordRow" runat="server" visible="true">
		<td class="SubHead ati_Form_Text2"><dnn:label id="plConfirm" runat="server" controlname="txtConfirm" text="Confirm:"></dnn:label></td>
		<td class="NormalBold ati_Form_Control2" noWrap><asp:textbox id="txtConfirm" tabIndex="5" runat="server" cssclass="NormalTextBox"  Width="200px" maxlength="20" textmode="Password"></asp:textbox></td>			
	    <td class="ati_Form_Texture">&nbsp;</td>
	    <td class="ati_Form_Validate">*<asp:requiredfieldvalidator id="valConfirm1" runat="server" cssclass="NormalRed" display="Dynamic" errormessage="&nbsp;&nbsp;Required."
				controltovalidate="txtConfirm" resourcekey="valConfirm1"></asp:requiredfieldvalidator>
			<asp:comparevalidator id="valConfirm2" runat="server" cssclass="NormalRed" display="Dynamic" errormessage="<br>Passwords Do Not Match."
				controltovalidate="txtConfirm" resourcekey="valConfirm2" controltocompare="txtPassword"></asp:comparevalidator></td>
	</tr>
	<tr>
		<td class="SubHead ati_Form_Text2"><dnn:label id="plEmail" runat="server" controlname="txtEmail" text="Email&nbsp;Address:"></dnn:label></td>
		<td class="NormalBold ati_Form_Control2" noWrap><asp:textbox id="txtEmail" tabIndex="6" runat="server" cssclass="NormalTextBox"  Width="200px" maxlength="175"></asp:textbox></td>			
	    <td class="ati_Form_Texture">&nbsp;</td>
	    <td class="ati_Form_Validate">*<asp:requiredfieldvalidator id="valEmail1" runat="server" cssclass="NormalRed" display="Dynamic" errormessage="&nbsp;&nbsp;Required."
				controltovalidate="txtEmail" resourcekey="valEmail1"></asp:requiredfieldvalidator>
			<asp:regularexpressionvalidator id="valEmail2" runat="server" cssclass="NormalRed" display="Dynamic" errormessage="&nbsp;&nbsp;Email not Valid."
				controltovalidate="txtEmail" resourcekey="valEmail2" validationexpression="[\w\.-]+(\+[\w-]*)?@([\w-]+\.)+[\w-]+"></asp:regularexpressionvalidator></td>
	</tr>	
</table>
