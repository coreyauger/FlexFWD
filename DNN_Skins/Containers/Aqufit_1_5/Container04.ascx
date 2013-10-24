<%@ Control language="vb" CodeBehind="~/admin/Containers/container.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTACTIONS" Src="~/Admin/Containers/SolPartActions.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ADDCONTENT" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SYNDICATEMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRINTMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MODULESETTINGS" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DELETEMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="VISIBILITY" Src="~/Admin/Containers/Visibility.ascx" %>




<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="8"><img src="cnt-4-top-left.gif" width="9" height="30" /></td>
    <td width="100%" background="cnt-4-top-center.gif"><table width="100%" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td valign="middle" nowrap="nowrap"></td>
        <td valign="middle" width="100%" nowrap="nowrap"><dnn:TITLE runat="server" id="dnnTITLE" Cssclass="cnt-4-title"/></span>
</td>
        <td valign="middle" nowrap="nowrap"><dnn:VISIBILITY runat="server" id="dnnVISIBILITY" MinIcon="hide.gif" MaxIcon="hide.gif" /></td>

      </tr>
    </table></td>
    <td width="8"><img src="cnt-4-top-right.gif" width="9" height="30" /></td>
  </tr>
  <tr>
    <td background="cnt-4-main-left.gif">&nbsp;</td>
    <td valign="top" background="cnt-4-main-center.gif"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td id="ContentPane" valign="top" class="cnt-4-body" align="left"><!-- Start_Module_4244 -->
        </td>

      </tr>
      <tr>
        <td><table width="100%" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td align="left" valign="middle" nowrap="nowrap"></td>

            <td align="right" valign="middle" nowrap="nowrap" style="padding-right: 5px;">
                    <dnn:DELETEMODULE runat="server" id="dnnDELETEMODULE" CommandName="DeleteModule.Action" DisplayIcon="True" DisplayLink="True" />
	                &nbsp;&nbsp;
	                <dnn:MODULESETTINGS runat="server" id="dnnMODULESETTINGS" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="True" />
	                &nbsp;&nbsp;
	                <dnn:ADDCONTENT runat="server" id="dnnADDCONTENT" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" />
                    &nbsp;&nbsp;
                    <dnn:SYNDICATEMODULE runat="server" id="dnnSYNDICATEMODULE"  CommandName="SyndicateModule.Action" DisplayIcon="True" IconFile="rss.gif" DisplayLink="False" />                   
                    &nbsp;&nbsp;
                    <dnn:PRINTMODULE runat="server" id="dnnPRINTMODULE" CommandName="PrintModule.Action" DisplayIcon="True" IconFile="print.gif" DisplayLink="False" /> 
              
            </td>
          </tr>
        </table></td>
      </tr>
    </table></td>
    <td background="cnt-4-main-right.gif">&nbsp;</td>
  </tr>

  <tr>
    <td><img src="cnt-4-bottom-left.gif" width="9" height="8" /></td>
    <td background="cnt-4-bottom-center.gif"><img src="spacer.gif" width="1" height="1" /></td>
    <td><img src="cnt-4-bottom-right.gif" width="9" height="8" /></td>
  </tr>
  <tr>
    <td colspan="3"><img src="spacer.gif" width="1" height="10" /></td>
  </tr>
</table>















<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD valign="top">

	<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD valign="top" class="EON_MI_IBS04TL"><img src="<%=skinpath%>dummy.gif" width="10" height="1"  border="0"></TD>
		<TD valign="top" class="EON_MI_IBS04T">
				<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD class="EON_MI_IBS04TitleTD"><dnn:TITLE runat="server" id="dnnTITLE" Cssclass="EON_MI_IBS04Title"/></td>
					<TD class="EON_MI_IBS04ActionsTD"><dnn:SOLPARTACTIONS runat="server" id="dnnSOLPARTACTIONS" /></td>
				</tr></table>
		</TD>
		<TD valign="top" class="EON_MI_IBS04TR"><dnn:VISIBILITY runat="server" id="dnnVISIBILITY" MinIcon="icomax_Minimalist.png" MaxIcon="icomin_Minimalist.png" /></TD>
	</TR></TABLE>

</td></tr>
<tr><TD valign="top">

	<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD valign="top" class="EON_MI_IBS04ML"><img src="<%=skinpath%>dummy.gif" width="10" height="1"  border="0"></TD>
		<TD valign="top" class="EON_MI_IBS04M">
				<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD class="EON_MI_IBS04Content" id="ContentPane" runat="server" valign="top"></TD></tr>
				</TABLE>
		</TD>
		<TD valign="top" class="EON_MI_IBS04MR"><img src="<%=skinpath%>dummy.gif" width="10" height="1"  border="0"></TD>
	</TR></TABLE>

</td></tr>
<tr><TD valign="top">

	<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD valign="top" class="EON_MI_IBS04BL"><img src="<%=skinpath%>dummy.gif" width="10" height="1"  border="0"></TD>
		<TD valign="top" class="EON_MI_IBS04B"><img src="<%=skinpath%>dummy.gif" width="1" height="1"  border="0"><BR>
			<TABLE width="100%"  border="0" cellspacing="0" cellpadding="0"><tr><TD class="EON_MI_IBS04Icons"><dnn:DELETEMODULE runat="server" id="dnnDELETEMODULE" CommandName="DeleteModule.Action" DisplayIcon="True" DisplayLink="True" /><dnn:MODULESETTINGS runat="server" id="dnnMODULESETTINGS" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="True" /><dnn:ADDCONTENT runat="server" id="dnnADDCONTENT" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" /><dnn:SYNDICATEMODULE runat="server" id="dnnSYNDICATEMODULE" CommandName="SyndicateModule.Action" DisplayIcon="True" DisplayLink="False" /><dnn:PRINTMODULE runat="server" id="dnnPRINTMODULE" CommandName="PrintModule.Action" DisplayIcon="True" DisplayLink="False" /></TD></tr></TABLE>
		</TD>
		<TD valign="top" class="EON_MI_IBS04BR"><img src="<%=skinpath%>dummy.gif" width="10" height="1"  border="0"></TD>
	</TR></TABLE>

</td></tr>
</table><BR>







