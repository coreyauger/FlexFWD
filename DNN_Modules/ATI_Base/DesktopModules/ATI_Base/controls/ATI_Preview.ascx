<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_Preview.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_Preview" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="ati" TagName="TextBox" Src="~/DesktopModules/ATI_Base/controls/ATI_TextBoxControl.ascx" %>

<table cellSpacing="0" cellPadding="0" border="0" summary="User Design Table" class="ati_Form_Table">	
    <tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:label id="plFirstName" runat="server" controlname="txtFirstName" text="First&nbsp;Name:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <ati:TextBox ID="atiTxtFirstName" runat="server" CssClass="ati_Form_TextBox" MaxLength="63" IsRequired="true" IsDouble="false" />
		</td>			
	    <td class="ati_Form_Element ati_Form_Texture">&nbsp;</td>	
	</tr>
	<tr valign="middle">
		<td class="ati_Form_Element_Alt ati_Form_Text2" noWrap><asp:label id="plLastName" runat="server" controlname="txtLastName" text="Last&nbsp;Name:" /></td>
		<td class="ati_Form_Element_Alt ati_Form_Control2" noWrap>
		    <ati:TextBox ID="atiTxtLastName" runat="server" CssClass="ati_Form_TextBox" MaxLength="63" IsRequired="true" IsDouble="false" />
		<td class="ati_Form_Element_Alt ati_Form_Texture">&nbsp;</td>
	</tr>	
    <tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:label id="plEmail" runat="server" controlname="txtEmail" text="Email&nbsp;Address:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <ati:TextBox ID="atiTxtEmail" runat="server" CssClass="ati_Form_TextBox" MaxLength="128" IsRequired="true" IsDouble="false" RegEx="^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$" RegExErrorMsg="Not an Email Address" />
		</td>			
	    <td class="ati_Form_Element ati_Form_Texture">&nbsp;</td>
	</tr>
	<tr valign="top">
		<td class="ati_Form_Element_Alt ati_Form_Text2" noWrap style="padding-top: 8px;"><asp:label id="plComments" runat="server" controlname="txtComments" text="Comments:" /></td>
		<td class="ati_Form_Element_Alt ati_Form_Control2" noWrap>
		    <asp:TextBox ID="txtComments" runat="server" CssClass="ati_Form_TextArea" TextMode="MultiLine" />
		<td class="ati_Form_Element_Alt ati_Form_Texture">&nbsp;</td>
	</tr>	
			
</table>

