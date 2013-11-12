<%@ Control Language="C#" AutoEventWireup="false" CodeFile="Settings.ascx.cs" Inherits="Affine.Dnn.Modules.ATI_Feedback.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<table cellspacing="5" cellpadding="5" border="0" summary="ATI_Feedback Settings Design Table">   
   <tr>
        <td><asp:Label id="lShowFeedback" runat="server" controlname="txtShowFeedback" Text="Show&nbsp;Feedback:" /></td>
        <td><asp:CheckBox ID="cbShowFeedback" runat="server" /></td>
   </tr> 
</table>
