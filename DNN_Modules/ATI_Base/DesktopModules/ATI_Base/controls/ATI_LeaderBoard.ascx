<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_LeaderBoard.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_LeaderBoard" %>

<style type="text/css">
div.leaderBoard dl  
{
	padding-top: 7px;
	/* border: 3px double #ccc; */
	padding: 4px 10px 4px 10px;
} 
div.leaderBoard dt  
{
	float: left; 
	clear: left; 
	
	text-align: right; 
	font-weight: bold; 
	color: #0095CD; 		
} 
div.leaderBoard dt a
{
	color: #0095CD;
}
div.leaderBoard dt:after { content: ":"; } 
div.leaderBoard dd  
{
	margin: 0 0 0 75px; 
/*	padding: 0 10; */
}
div.leaderBoard table tr td a.wodLink
{
	font-size: 14px;
	font-weight:bold;
	display: block;
	background-color: #666;
	color: White;
	padding: 2px 10px 2px 10px;
}

div.leaderBoard table tr
{
	vertical-align: top;
}
div.leaderBoard table
{
	margin: 0;
	padding: 0;
}
div.leaderBoard table tr td
{
	background-color: #eee;
	font-size: 9px;
	border-right: 1px solid #666;
}
</style>

<script type="text/javascript">
      
</script>
<div class="leaderBoard">
<asp:PlaceHolder ID="phWorkoutTotals" runat="server" />
</div>
