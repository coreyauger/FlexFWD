<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_BodyComposition.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_BodyComposition" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="ati" TagName="UnitControl" Src="~/DesktopModules/ATI_Base/controls/ATI_UnitControl.ascx" %>

<script language="javascript">

    function Measurment(val, units) {
        this.val = val;
        this.units = units;
    }

    function RoundOneHundreth(val) {
        val *= 100;
        val = Math.round(val);
        return val / 100;
    }

    function SwitchHeightControls(sender, args) {
        var ddlUnitsType = document.getElementById(sender);
        var units = ddlUnitsType[ddlUnitsType.selectedIndex].value;
        var atiTxtHeightFeet = document.getElementById('<%=atiTxtHeightFeet.ClientID%>');
        var atiTxtHeightInch = document.getElementById('<%=atiTxtHeightInch.ClientID%>');
        var rfvHeightFeet = document.getElementById('<%=rfvHeightFeet.ClientID %>');
        var revHeightFeet = document.getElementById('<%=revHeightFeet.ClientID %>');
        if (units == Aqufit.Units.UNIT_CM) {   // metric
            rfvHeightFeet.style.display = 'none';
            revHeightFeet.style.display = 'none';
            var inches = atiTxtHeightFeet.value == '' ? 0 : parseFloat(atiTxtHeightFeet.value) * 12;
            inches += atiTxtHeightInch.value == '' ? 0 : parseFloat( atiTxtHeightInch.value );
            var cm = Aqufit.Units.convert(Aqufit.Units.UNIT_INCHES, inches, Aqufit.Units.UNIT_CM);
            atiTxtHeightFeet.style.display = 'none';
            atiTxtHeightInch.value = Math.round(cm);
        } else if (units == Aqufit.Units.UNIT_FT_IN) {    // english
            rfvHeightFeet.style.display = 'inline';
            revHeightFeet.style.display = 'inline';
            var cm = atiTxtHeightInch.value == '' ? 0 : parseFloat(atiTxtHeightInch.value);
            var inches = Aqufit.Units.convert(Aqufit.Units.UNIT_CM, cm, Aqufit.Units.UNIT_INCHES);
            var feet = Math.floor(inches/12);
            atiTxtHeightFeet.value = feet;
            inches = inches % 12;
            atiTxtHeightInch.value = Math.round(inches);  
            atiTxtHeightFeet.style.display = 'inline';
        }
    }

    function SwitchWeight(sender, args) {
        var ddlUnitsType = document.getElementById(sender);
        var units = ddlUnitsType[ddlUnitsType.selectedIndex].value;
        var atiTxtWeight = document.getElementById('<%=atiTxtWeight.ClientID%>');
        if (units == Aqufit.Units.UNIT_KG) {   // metric
            var lbs = atiTxtWeight.value == '' ? 0 : parseFloat(atiTxtWeight.value);
            var kg = Aqufit.Units.convert(Aqufit.Units.UNIT_LBS, lbs, Aqufit.Units.UNIT_KG);
            atiTxtWeight.value = Math.round(kg);
        } else if (units == Aqufit.Units.UNIT_LBS) {    // english
            var kg = atiTxtWeight.value == '' ? 0 : parseFloat(atiTxtWeight.value);
            var lbs = Aqufit.Units.convert(Aqufit.Units.UNIT_KG, kg, Aqufit.Units.UNIT_LBS);
            atiTxtWeight.value = Math.round(lbs);
        }
    }
        
</script>
<dl>
    <dt id="dtHeight" runat="server"><asp:Label id="plHeight" runat="server" controlname="txtHeight" text="Height:" /></dt>
    <dd id="ddHeight" runat="server">
        <asp:TextBox ID="atiTxtHeightFeet" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" MaxLength="1" style="width: 100px;" /> 
	    <asp:RequiredFieldValidator ID="rfvHeightFeet" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtHeightFeet" ErrorMessage="Feet is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
	    <asp:RegularExpressionValidator ID="revHeightFeet" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtHeightFeet" ValidationExpression="\d+" ErrorMessage="Feet must be a Number" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
	    
	    <asp:TextBox ID="atiTxtHeightInch" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox " MaxLength="4" style="width: 100px;" /> 
	    <asp:RequiredFieldValidator ID="rfvHeightInch" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtHeightInch" ErrorMessage="Inches is required!" Text="*" />  
	    <asp:RegularExpressionValidator ID="revHeightInch" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtHeightInch" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Inches must be a Real Number" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
	    
	    <ati:UnitControl id="atiHeightUnits" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" UnitType="height" ClientOnChange="SwitchHeightControls" Width="100px" />
    </dd>
	<dt id="dtWeight" runat="server"><asp:Label id="plWeight" runat="server" controlname="txtWeight" text="Weight:" /></dt>
	<dd id="ddWeight" runat="server">
        <asp:TextBox ID="atiTxtWeight" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox " MaxLength="16" style="width: 100px;" />		   
	    <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtWeight" ErrorMessage="Weight is required!" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />  
	    <asp:RegularExpressionValidator ID="revWeight" runat="server" ValidationGroup="BodyComposition" ControlToValidate="atiTxtWeight" ValidationExpression="(\d+\.\d*)|(\d*\.\d+)|(\d+)" ErrorMessage="Weight must be a Real Number" Text="<img src='/DesktopModules/ATI_Base/resources/images/iError.png' />" />
	    <ati:UnitControl id="atiWeightUnits" runat="server" UnitType="weight" ClientOnChange="SwitchWeight" CssClass="ui-corner-all ui-widget-content atiTxtBox" Width="100px" />
    </dd>  
	<dt id="dtFitnessLevel" runat="server"><asp:Label id="plFitnessLevel" runat="server" controlname="txtFitnessLevel" text="Fitness&nbsp;Level:" /></dt>   
	<dd id="ddFitnessLevel" runat="server">
        <asp:DropDownList ID="ddlFitnessLevel" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" style="width: 100px;">
	        <asp:ListItem Text="Poor" Value="0" />
	        <asp:ListItem Text="Fair" Value="1" />
	        <asp:ListItem Text="Good" Value="2" Selected="True" />
	        <asp:ListItem Text="Very Good" Value="3" />
	        <asp:ListItem Text="Excellent" Value="4" />
	    </asp:DropDownList>
    </dd>        
	<dt id="dtBirthDate" runat="server"><asp:Label id="plBirthDate" runat="server" controlname="txtAge" text="Birth&nbsp;Date:" /></dt>
	<dd id="ddBirthDate" runat="server">
        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" style="width: 100px;">
	        <asp:ListItem Selected="True" Text="MM" Value="MM" />
	    </asp:DropDownList>
	    &nbsp;
	    <asp:DropDownList ID="ddlDay" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" style="width: 100px;">
	        <asp:ListItem Selected="True" Text="DD" Value="DD" />
	    </asp:DropDownList>
	    &nbsp;
	    <asp:DropDownList ID="ddlYear" runat="server" CssClass="ui-corner-all ui-widget-content atiTxtBox" style="width: 100px;">
	        <asp:ListItem Selected="True" Text="YYYY" Value="YYYY" />
	    </asp:DropDownList>
	</dd>				
	<dt id="dtGender" runat="server"><asp:Label id="plGender" runat="server" controlname="txtGender" text="Gender:" /></dt>	
	<dd id="ddGender" runat="server">
        <asp:DropDownList id="ddlGender" tabIndex="4" runat="server" cssclass="ui-corner-all ui-widget-content atiTxtBox" >
	        <asp:ListItem Text="Male" Value="M" />
	        <asp:ListItem Text="Female" Value="F" />
	    </asp:DropDownList>
    </dd>
</dl>