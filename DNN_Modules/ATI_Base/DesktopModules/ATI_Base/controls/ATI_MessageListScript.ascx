<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_MessageListScript.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_MessageListScript" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">  
<style type="text/css">         
      
    div.messageListHead > h2
    {
    	padding-left: 3px;
    }
    div.messageListHead > div
    {
    	padding: 3px;    
    	font-size: 12px;
    	color: #E78F08;
    	font-weight: bold;  
    	border-top: 1px solid #CCC;  
    	border-bottom: 1px solid #CCC;	
    }
    div.messageListHead > span
    {
    	padding: 4px;
    	background-color: White;
    	display: block;
    }
    div.messageListHead > span a
    {
        color: #E78F08;
    }
    
    div.messageListHead > div > input
    {
    	font-size: 12px;
    }
          
    ul.atiStreamList 
    {
    	background-color: White;
    }  
    ul.atiStreamList  > li
    {
    	border-top: 1px solid #EEEEEE;
    	list-style: none;
    	list-style: none outside none;
    	clear: both;     	   	    	
    }  
    
    
    ul.atiMessageView > li
    {
    	border-top: 1px solid #EEEEEE;
    	list-style: none;
    	list-style: none outside none;
    	clear: both;     	   	    	
    } 
    div.atiMessage
    {
    	background-color: White;
    }
    div.atiMessage > h2, div.atiMessage > a
    {
    	padding-left: 8px;
    }
    div.atiMessage > a 
    {
    	color: #E78F08;
    	font-weight: bold;      		
    }
    div.atiMessage textarea
    {
    	margin: 8px 16px 8px 16px;   
    	width: 523px;
    	height: 100px;
    }   
    
    div.messageWrapper
    {
    	cursor: pointer;  
    	height: 90px;
    	border: 1px solid white;
    }
    div.messageWrapper:Hover
    {
    	border: 1px solid #fbcb09;
    }
    
    div.unread
    {
    	background-color: #eee;
    }
    
    li.checkboxCell
    {
    	padding-right: 8px;
    	position: relative;
    	bottom: 28px;    	
    }
     div.atiStreamItemLeft li.checkboxCell img
    {
    	background: none;
    	padding: 0;    	
    }        
        
    div.atiStreamItemLeft
    {
    	position: relative;
    	float: left;
    	width: 110px;    
    	padding: 12px 16px 12px 16px;    
    }
    div.atiStreamItemLeft img
    {
    	padding: 3px 10px 10px 3px;
    	background: #FFFFFF url(<%=ResolveUrl("~/DesktopModules/ATI_Base/resources/images/profileBorder.png")%>) no-repeat top left;	 	
    }
    div.atiMessageItemLeft
    {
    	width: 60px;
    }
  
    .hidden
    {
    	visibility: hidden;
    }  
    

    div.atiStreamItemRight
    {
    	position: relative;
    	float: right;
    	width: 400px;    
    	padding-top: 8px;
    	padding-bottom: 8px;      	    	
    }  
    div.atiMessageItemRight
    {
    	width: 450px;    
    }       
    
   div.atiStreamItemRight a.username
    {
    	color:#ca992c;
        font-size:11px;        
        font-weight: normal;   
        font-weight: bolder;        
    }
    
    ul.hList li{
        border: 0;       
        line-style:none;        
        display: inline;       
        flow: horizontal;
        background-color: transparent;
    }       
       
    
    div.atiStreamItemRight a.time
    {
    	color:#A1A1A1;
        font-size:10px;
        white-space:nowrap;
        text-decoration: none;          
    }
    div.atiStreamItemRight a.time:hover
    {
    	text-decoration: underline;
    }
    
    div.atiStreamItemRight span.title,
    div.atiStreamItemRight span.dist
    {
        color:#333333;
        font-size:12px;      
        font-weight: normal;             
    }   
    div.atiStreamItemRight span.title
    {
     	color:#666666;
    }   
     
    div.atiStreamItemRight span.pace 
    {
        color:#A1A1A1;
        font-size:9px;
        margin-left:10px;
        white-space:nowrap;
    }
    div.atiStreamItemRight p
    {
    	color:#666666;
    	font-size:11px;
    }
    li.deleteStream
    {
    	position: absolute;
    	top: 28px;
    	right: 8px;
    }
   
    li.messageSummary
    {
    	position: absolute;    
    	left: 64px;
    	top: 16px;
    	width: 290px;
    }   
    li.messageSender
    {
    	position: absolute;    
    	left: -16px;
    	top: 19px;
    }   
   ul.atiCommentBox li 
   {
   	    list-style: none;
   	    list-style: none outside none;
   }             
   ul.atiCommentBox > li {
        border-top: 1px solid white;        
    	background-color: #f4f4f4;    
    	padding: 7px;
    	clear: both;
    }
    
    div.commentBoxLeft
    {
    	position: absolute;    	
    	width: 50px;
    	height: 100%;
    	background-color: transparent;
    }
    div.commentBoxLeft img
    {
    	border: 1px solid white;
    }
    div.commentBoxRight
    {
    	position: relative;
    	left: 62px;    	 
    	height: 100%;    
    	width: 380px;	
    	min-height: 50px;
    }
    div.commentBoxRight span
    {    	
    }        
    div.commentBoxRight img.speak
    {
    	position: absolute;
    	top: 0px;
    	left: -6px;
    }    
    div.commentBoxRight textarea
    {   
    	width: 380px;
    	height: 25px;   	
    	padding: 3px 7px 3px 7px;    	
    	border: 1px solid #cccccc;
    	font-size:12px;
    }    
    div.commentBoxRight textarea.txtCommentFocus
    {   
    	height: 50px;  	
    }
    
</style>

<script type="text/javascript">        

    $(function () {
        <% if( Request["m"] != null ){ %>
            Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiMessage('<%=this.ID %>','<%=atiMessage.ClientID %>');
            Affine.WebService.StreamService.getMessage(Aqufit.Page.UserSettingsId, <%= Request["m"] %>, function(json){Aqufit.Page.<%=this.ID %>.generateMessageDom(json);}, WebServiceFailedCallback);
        <%}else{ %>
            Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.atiMessageList('<%=this.ID %>','<%=atiMessageList.ClientID %>', <%=(int)this.Mode %>);
            Aqufit.Page.<%=this.ID%>.getMessageList();            
        <%} %>              
    });  
    
</script>
</telerik:RadCodeBlock>
<asp:Panel ID="atiMessageList" runat="server">             
</asp:Panel>
<asp:Panel ID="atiMessage" runat="server" CssClass="atiMessage">             
</asp:Panel>
<div style="clear: both;"></div>
