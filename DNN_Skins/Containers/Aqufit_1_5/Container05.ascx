<%@ Control language="vb" CodeBehind="~/admin/Containers/container.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTACTIONS" Src="~/Admin/Containers/SolPartActions.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ADDCONTENT" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SYNDICATEMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRINTMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MODULESETTINGS" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DELETEMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="VISIBILITY" Src="~/Admin/Containers/Visibility.ascx" %>


<table width="100%" border="0" cellspacing="0" cellpadding="0" class="c5-container">
  <tr>  
    <td>
        <table width="100%"border="0" cellspacing="0" cellpadding="0" >
        <tr>
            <td class="c5-title"><dnn:TITLE runat="server" id="dnnTITLE" Cssclass="c5-titleText"/></td>
            <td class="c5-title" align="right"><dnn:VISIBILITY runat="server" id="dnnVISIBILITY" MinIcon="icomax_Minimalist.png" MaxIcon="icomin_Minimalist.png" /></td>
        </tr>
        </table>
        
        
        </td>
    </td>   
  </tr>
  <tr>
    <td id="ContentPane" runat="server" class="c5-main">
    
    </td>
  </tr>
  <tr>
    <td class="c5-main">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
              <td align="left" valign="middle" nowrap="nowrap"></td>
              <td align="right" valign="middle" nowrap="nowrap" style="padding-right: 5px;">
                    <dnn:SOLPARTACTIONS runat="server" id="dnnSOLPARTACTIONS" />
                    &nbsp;&nbsp;
                    <dnn:DELETEMODULE runat="server" id="dnnDELETEMODULE" CommandName="DeleteModule.Action" DisplayIcon="True" DisplayLink="True" />
	                &nbsp;&nbsp;
	                <dnn:MODULESETTINGS runat="server" id="dnnMODULESETTINGS" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="True" />
	                &nbsp;&nbsp;
	                <dnn:ADDCONTENT runat="server" id="dnnADDCONTENT" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" />
                    &nbsp;&nbsp;
                    <dnn:SYNDICATEMODULE runat="server" id="dnnSYNDICATEMODULE"  CommandName="SyndicateModule.Action" DisplayIcon="True" IconFile="<%=skinpath%>rss.gif" DisplayLink="False" />                   
                    &nbsp;&nbsp;
                   <dnn:PRINTMODULE runat="server" id="dnnPRINTMODULE" CommandName="PrintModule.Action" DisplayIcon="True" IconFile="<%=skinpath%>action_print.gif" DisplayLink="False" /> 
              </td>
            </tr>
        </table>
    </td>
  </tr>
  <tr>
    <td colspan="3"><img src="<%=skinpath%>spacer.gif" width="1" height="10" /></td>
  </tr>
</table>





