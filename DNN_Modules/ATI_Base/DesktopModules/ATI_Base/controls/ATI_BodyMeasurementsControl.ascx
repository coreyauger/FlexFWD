<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_BodyMeasurementsControl.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_BodyMeasurements" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>


<table cellSpacing="0" cellPadding="0" border="0" summary="Body Measurements Design Table" class="ati_Form_Table">	
    <tr valign="middle">
		<td noWrap style="text-align: center;" colspan="5">
		    <table cellSpacing="0" cellPadding="0" border="0">
		    <tr>
		        <td class="ati_Form_Element ati_Form_Text2" ><asp:Label id="plUnits" runat="server" controlname="txtUnits" text="Units:" /></td>
		        <td><ati:UnitControl id="atiBmUnits" runat="server" CssClass="ati_Form_TextBoxSmall" UnitType="bodymeasure" /></td>
		    </tr>
		    </table>
		</td>
	</tr>	

    <tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plNeck" runat="server" controlname="txtNeck" text="Neck:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtNeck" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revNeckt" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtNeck" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Neck must be a Number" Text="*" />		    
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plShoulders" runat="server" controlname="txtShoulders" text="Shoulders:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtShoulders" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revShoulders" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtShoulders" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Shoulders must be a Number" Text="*" />	
		</td>	
	</tr>	
	<tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plChest" runat="server" controlname="txtChest" text="Chest:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtChest" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revChest" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtChest" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Chest must be a Number" Text="*" />    
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plStomach" runat="server" controlname="txtStomach" text="Stomach:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtStomach" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />		   
		    <asp:RegularExpressionValidator ID="revStomach" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtStomach" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Stomach must be a Number" Text="*" />
		</td>	
	</tr>	
	<tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plWaist" runat="server" controlname="txtWaist" text="Waist:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtWaist" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />	
		    <asp:RegularExpressionValidator ID="revWaist" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtWaist" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Waist must be a Number" Text="*" />	    
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plHips" runat="server" controlname="txtHips" text="Hips:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtHips" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revHips" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtHips" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Hips must be a Number" Text="*" />
		</td>	
	</tr>
	<tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plBicepLeft" runat="server" controlname="txtBicepLeft" text="Left&nbsp;Bicep:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtBicepLeft" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revBicepLeft" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtBicepLeft" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Bicep Left must be a Number" Text="*" />    
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plBicepRight" runat="server" controlname="txtBicepRight" text="Right&nbsp;Bicep:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtBicepRight" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />	
		    <asp:RegularExpressionValidator ID="revBicepRight" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtBicepRight" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Bicep Right must be a Number" Text="*" />	   
		</td>	
	</tr>
	<tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plForearmLeft" runat="server" controlname="txtForearmLeft" text="Left&nbsp;Forearm:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtForearmLeft" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revForearmLeft" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtForearmLeft" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Forearm Left must be a Number" Text="*" />	    
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plForearmRight" runat="server" controlname="txtForearmRight" text="Right&nbsp;Forearm:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtForearmRight" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revForearmRight" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtForearmRight" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Forearm Right must be a Number" Text="*" />	    
		</td>	
	</tr>
	<tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plThighLeft" runat="server" controlname="txtThighLeft" text="Left&nbsp;Thigh:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtThighLeft" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revThighLeft" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtThighLeft" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Thigh Left must be a Number" Text="*" />		    
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plThighRight" runat="server" controlname="txtThighRight" text="Right&nbsp;Thigh:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtThighRight" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />	
		    <asp:RegularExpressionValidator ID="revThighRight" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtThighRight" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Thigh Right must be a Number" Text="*" />	    
		</td>	
	</tr>
	<tr valign="middle">
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plCalfLeft" runat="server" controlname="txtCalfLeft" text="Left&nbsp;Calf:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtCalfLeft" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revCalfLeft" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtCalfLeft" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Calf Left must be a Number" Text="*" />   
		</td>			
	    <td class="ati_Form_Element ati_Form_Spacer">&nbsp;</td>
	    <td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="plCalfRight" runat="server" controlname="txtCalfRight" text="Right&nbsp;Calf:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" noWrap>
		    <asp:TextBox ID="atiTxtCalfRight" runat="server" CssClass="ati_Form_TextBoxSmall" MaxLength="16" />
		    <asp:RegularExpressionValidator ID="revCalfRight" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtCalfRight" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Calf Rightt must be a Number" Text="*" />    
		</td>	
	</tr>
</table>
