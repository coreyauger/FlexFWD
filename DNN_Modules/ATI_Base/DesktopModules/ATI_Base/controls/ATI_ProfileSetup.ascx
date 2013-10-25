<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_ProfileSetup.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_ProfileSetup" %>
<%@ Register TagPrefix="ati" TagName="ProfileImage" Src="~/DesktopModules/ATI_Base/controls/ATI_ProfileImage.ascx" %>
<%@ Register TagPrefix="ati" TagName="WorkoutTypes" Src="~/DesktopModules/ATI_Base/controls/ATI_WorkoutTypes.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    
<style type="text/css">
.atiThreeList
{
	padding-right: 10px;
	padding-bottom: 4px;
}
</style>    
    
<script language="javascript">

    function OpenWindow() {
        var wnd = window.radopen("http://www.google.com", null);
        wnd.setSize(640, 480);
        return false;
    }           
    
</script>


<telerik:radwindowmanager runat="server" RestrictionZoneID="offsetElement" ID="RadWindowManager1" Skin="Black" />

<table cellSpacing="0" cellPadding="0" border="0" summary="Address Design Table">
	<tr>
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="lProfilePic" runat="server" controlname="txtProfilePic" text="Profile&nbsp;Pic:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" nowrap>
		    <ati:ProfileImage ID="atiProfileImage" runat="server" Width="200px" Height="126px" />            
		</td>		
	</tr>	
	<tr>
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="lActivities" runat="server" controlname="txtActivities" text="Activities:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" nowrap>
		    <ati:WorkoutTypes ID="atiWorkoutTypes" runat="server" />
		</td>
	</tr>
	<tr>
		<td class="ati_Form_Element_Alt SubHead ati_Form_Text2" noWrap><asp:Label id="lPrivacySettings" runat="server" controlname="txtPrivacySettings" text="Privacy&nbsp;Settings:" /></td>
		<td class="ati_Form_Element_Alt ati_Form_Control2" nowrap>
		    PRIVACY drop down with(settings like, locked, trusted, friends, standard, open)<br />		    
		</td>		
	</tr>
	<tr>
	    <td class="ati_Form_Element_Alt SubHead ati_Form_Text2" noWrap>&nbsp;</td>
	    <td>description of each below.. as the person makes a selction</td>
	</tr>
	<tr>
		<td class="ati_Form_Element ati_Form_Text2" noWrap><asp:Label id="lFriends" runat="server" controlname="txtFriends" text="Find&nbsp;Friends:" /></td>
		<td class="ati_Form_Element ati_Form_Control2" nowrap>
		    <input type="button" id="bOpenFriends" onclick="return OpenWindow()" value="Open" />
		</td>		
	</tr>		
</table>
