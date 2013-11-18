<%@ Control language="C#" Inherits="Affine.Dnn.Modules.ATI_Register.EditATI_Register" CodeFile="EditATI_GroupAdmin.ascx.cs" AutoEventWireup="true"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<table width="650" cellspacing="0" cellpadding="0" border="0" summary="Edit Table">
	<tr valign="top">
		<td class="SubHead" width="125"><dnn:label id="lblContent" runat="server" controlname="lblContent" suffix=":"></dnn:label></td>
		<td>
			<dnn:texteditor id="txtContent" runat="server" height="200" width="500" />
			<asp:RequiredFieldValidator ID="valContent" resourcekey="valContent.ErrorMessage" ControlToValidate="txtContent"
				CssClass="NormalRed" Display="Dynamic" ErrorMessage="<br>Content is required" Runat="server" />
		</td>
	</tr>
</table>
<p>
    <asp:linkbutton cssclass="CommandButton" id="cmdUpdate" OnClick="cmdUpdate_Click" resourcekey="cmdUpdate" runat="server" borderstyle="none" text="Update"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="CommandButton" id="cmdCancel" OnClick="cmdCancel_Click" resourcekey="cmdCancel" runat="server" borderstyle="none" text="Cancel" causesvalidation="False"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="CommandButton" id="cmdDelete" OnClick="cmdDelete_Click" resourcekey="cmdDelete" runat="server" borderstyle="none" text="Delete" causesvalidation="False"></asp:linkbutton>&nbsp;
</p>
<dnn:audit id="ctlAudit" runat="server" />
