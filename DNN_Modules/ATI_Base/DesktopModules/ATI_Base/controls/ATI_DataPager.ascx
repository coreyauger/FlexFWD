<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_DataPager.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_DataPager" %>

<style type="text/css">
 div.pagerWrap
 {
 	height: 25px;
 	border-bottom: 1px solid #ccc;
 	border-top: 1px solid #ccc;
 	text-align: center;
 	vertical-align: middle;
 	position: relative; 	
 }
 span.pageStart, span.pageEnd, span.totalLen
 {
 	font-size: 12px;
 	font-weight: bold; 	
 	color: #999 !important;
 }
 span.pagerInfo
 {
    color: #999 !important;
 	text-align: center;
 	line-height: 25px;
 }
 div.pagerWrap button.pageBack
 {
 	position: absolute;
 	left: 0px;
 	top: 0px;
 	z-index: 9;
 }
  div.pagerWrap button.pageNext
 {
 	position: absolute;
 	right: 0px;
 	top: 0px;
 	z-index: 9;
 }
</style>

<script type="text/javascript">
    

    $(function () { 
        Aqufit.Page.<%=this.UniqueID %> = new Aqufit.Page.Controls.atiDataPager('<%=this.UniqueID %>', '<%=pagerHolder.ClientID %>','<%=bBack.ClientID %>','<%=bNext.ClientID %>');    
    });        
</script>
<div id="pagerHolder" runat="server" class="pagerWrap grad-FFF-EEE">
    <button id="bBack" class="pageBack" runat="server">Prev</button><span class="pagerInfo"><span class="pageStart"></span> - <span class="pageEnd"></span> of <span class="totalLen"></span></span><button id="bNext" class="pageNext" runat="server">Next</button>
</div>