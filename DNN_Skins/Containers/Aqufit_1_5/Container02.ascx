<%@ Control language="vb" CodeBehind="~/admin/Containers/container.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Containers.Container" %>
<%@ Register TagPrefix="dnn" TagName="SOLPARTACTIONS" Src="~/Admin/Containers/SolPartActions.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TITLE" Src="~/Admin/Containers/Title.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ADDCONTENT" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SYNDICATEMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRINTMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="MODULESETTINGS" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DELETEMODULE" Src="~/Admin/Containers/ActionButton.ascx" %>
<%@ Register TagPrefix="dnn" TagName="VISIBILITY" Src="~/Admin/Containers/Visibility.ascx" %>
<%@ Register TagPrefix="dnn" TagName="ICON" Src="~/Admin/Containers/Icon.ascx" %>
<!-- Skin Object that is an Icon for the Module ID -->
<div class="c2-container">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td class="c2-titleLeft">&nbsp;</td>
        <td class="c2-titleBar"><table><tr valign="middle"><td><dnn:ICON id="atiICON" runat="server" BorderWidth="0" /></td><td><dnn:TITLE runat="server" id="dnnTITLE" Cssclass="c2-titleText"/></td></tr></table></td>
        <td class="c2-titleRight">&nbsp;</td>
    </tr>
    <tr>
        <td class="c2-left">&nbsp;</td>
        <td id="ContentPane" runat="server" class="c2-main"></td>
        <td class="c2-right">&nbsp;</td>
    </tr>
    <tr>
        <td class="c2-bottomLeft">&nbsp;</td>
        <td class="c2-bottom"></td>
        <td class="c2-bottomRight">&nbsp;</td>
    </tr>
    </table>    
    <dnn:SOLPARTACTIONS runat="server" id="dnnSOLPARTACTIONS" />
    &nbsp;&nbsp;
    <dnn:DELETEMODULE runat="server" id="dnnDELETEMODULE" CommandName="DeleteModule.Action" DisplayIcon="True" DisplayLink="True" />
    &nbsp;&nbsp;
    <dnn:MODULESETTINGS runat="server" id="dnnMODULESETTINGS" CommandName="ModuleSettings.Action" DisplayIcon="True" DisplayLink="True" />
    &nbsp;&nbsp;
    <dnn:ADDCONTENT runat="server" id="dnnADDCONTENT" CommandName="AddContent.Action" DisplayIcon="True" DisplayLink="True" />
    &nbsp;&nbsp;   
    <dnn:SYNDICATEMODULE runat="server" id="dnnSYNDICATEMODULE"  CommandName="SyndicateModule.Action" DisplayIcon="True" IconFile="rss.png" DisplayLink="False" />                   
    &nbsp;&nbsp;
   <dnn:PRINTMODULE runat="server" id="dnnPRINTMODULE" CommandName="PrintModule.Action" DisplayIcon="True" IconFile="print.png" DisplayLink="False" />
</div>






