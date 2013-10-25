<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ATI_LeaderBoard2.ascx.cs" Inherits="DesktopModules_ATI_Base_controls_ATI_LeaderBoard2" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
<style type="text/css">

div.lbWrap
{
	position: relative;
	width: 340px;
	display: table-cell;
	padding: 15px;
}
div.lbWrap:hover
{
	background-color: #eee;
}

div.lbWorkout
{
	border-left: 1px solid #ccc;
	border-right: 1px solid #ccc;
	border-bottom: 1px solid #ccc;
}
div.lbWorkout a
{
	color: #0095d8;
}
div.lbWorkout .atiListHeading a
{
	font-weight: bold;
	text-transform: capitalize;
	font-size: larger;
}

img.leaderMale
{
	margin-left: 60px;
}

img.leaderFemale
{
	float: right;
	margin-right: 60px;
}

ul.lbMale span, ul.lbFemale span
{
	float: right;
}

ul.lbMale li:nth-child(odd), ul.lbFemale li:nth-child(even)
{
	background-color: #eee;
}

ul.lbMale li, ul.lbFemale li
{
	list-style: none;
	padding: 2px 5px;
	background-color: #FFF;
}

ul.lbMale
{	
	width: 50%;	
	border-right: 1px solid #ccc;
}
ul.lbFemale
{
	position: absolute;
	right: 0px;
	top: 0px;
	width: 50%;
}   
</style>

<script type="text/javascript">
   

    Aqufit.Page.Controls.ATI_LeaderBoard2 = function (id, lb, cols, edit, nopics) {
        this.id = id;
        this.$lb = $('#'+lb);
        this.onCloseCallback = null;
        this.editMode = edit;
        this.noPics = nopics;
        this.init();
        this.numCols = cols;
        this.colCount = cols;
        this.wodArray = [];
        this.deleteCallback = null;
    }

    Aqufit.Page.Controls.ATI_LeaderBoard2.prototype = {
        init: function () {            
        
        },
        appendLeaderBoard: function(wod){
            this.colCount++;
            if( this.colCount >= this.numCols ){
                this.$lb.append('<div style="display: table-row">');
                this.colCount = 0;
            }
            this.$lb.append(
                    '<div class="lbWrap" id="lbWod'+wod.Id+'">'+
                        (this.editMode ?
                        '<a id="lbDelWod'+wod.Id+'" href="javascript: ;">remove [X]</a>'
                        :
                        '')+
                        (wod.Male.length > 0 && !this.noPics ?
                        '<a href="/'+wod.Male[0].UserName+'"><img src="http://flexfwd.com/DesktopModules/ATI_Base/services/images/profile.aspx?us='+wod.Male[0].UsId+'" class="leaderMale"></a>'
                        :
                        '')+
                        (wod.Female.length > 0 && !this.noPics ?
                        '<a href="/'+wod.Female[0].UserName+'"><img src="http://flexfwd.com/DesktopModules/ATI_Base/services/images/profile.aspx?us='+wod.Female[0].UsId+'" class="leaderFemale"></a>'
                        :
                        '')+
                        '<div class="lbWorkout">'+
                            '<div class="atiListHeading grad-FFF-EEE" style="margin-top: 5px; text-align: center;">'+                                
                                '<img src="/DesktopModules/ATI_Base/resources/images/iMale.png" style="float: left;" />'+
                                '<a href="/workout/'+wod.Id+'">'+wod.Name+'</a>'+
                                '<img src="/DesktopModules/ATI_Base/resources/images/iFemale.png" style="float: right;" />'+
                            '</div>'+
                            '<div style="position: relative;">'+
                            '<ul class="lbMale">'+                                                               
                            '</ul>'+
                            '<ul class="lbFemale">'+                                
                            '</ul>'+
                            '</div>'+
                        '</div>'+
                    '</div>'
                    );
            if( this.colCount >= this.numCols ){
                this.$lb.append('</div>');
            }
            var that = this;
            $('#lbDelWod'+wod.Id).click(function(event){
                if( that.deleteCallback != null ){
                    if( confirm("Are you sure you want to delete this workout from your leader board?") ){
                        that.deleteCallback(wod.Id);
                        $('#lbWod'+wod.Id).remove();
                    }
                }else{
                    alert('no deleteCallback defined');
                }
                event.stopPropagation();
                return false;
            });
            var $mlist = $('#lbWod'+wod.Id+' ul.lbMale');
            var m=0;
            for( m=0; m<wod.Male.length; m++){
                $mlist.append('<li><a href="/'+wod.Male[m].UserName+'">'+wod.Male[m].UserName+'</a> <span>'+wod.Male[m].Score +'</span></li>');
            }           
            var $flist = $('#lbWod'+wod.Id+' ul.lbFemale');
            var f=0;
            for( f=0; f<wod.Female.length; f++){
                $flist.append('<li><a href="/'+wod.Female[f].UserName+'">'+wod.Female[f].UserName+'</a> <span>'+wod.Female[f].Score+'</span></li>');
            }
            for( f; f<m; f++){
                $flist.append('<li>&nbsp;</li>');
            }
            for( m; m<f; m++){
                $mlist.append('<li>&nbsp;</li>');
            }            
        },
        appendLeaderBoardJson: function(json){
            var data = eval('('+json+')');
            // data is a flat array of Male, and Female times... we need to organize them
            var newNames = [];
            for( var i=0; i<data.length; i++ ){
                var d = data[i];
                if( !this.wodArray[d.Name]){
                    newNames[d.Name] = d.Name;
                    this.wodArray[d.Name] = { Male:[], Female:[], Name:d.Name, Id:d.Id };
                }
                if( d.Sex == 'M' ){
                    this.wodArray[d.Name].Male = d.Data;
                }else{
                    this.wodArray[d.Name].Female = d.Data;
                }
            }
            for( var i in newNames ){                
                this.appendLeaderBoard(this.wodArray[i]);
            }
        },
        loadLeaderBoardFromJson: function( json ){
            var data = eval('('+json+')');
            // data is a flat array of Male, and Femal times... we need to organize them
            for( var i=0; i<data.length; i++ ){
                var d = data[i];
                if( !this.wodArray[d.Name]){
                    this.wodArray[d.Name] = { Male:[], Female:[], Name:d.Name, Id:d.Id };
                }
                if( d.Sex == 'M' ){
                    this.wodArray[d.Name].Male = d.Data;
                }else{
                    this.wodArray[d.Name].Female = d.Data;
                }
            }
            for( var i in this.wodArray ){                
                this.appendLeaderBoard(this.wodArray[i]);
            }
        }        
    };

    

    $(function () {
        Aqufit.Page.<%=this.ID %> = new Aqufit.Page.Controls.ATI_LeaderBoard2('<%=this.ID %>', '<%=lb2.ClientID %>', <%=this.Cols %>, <%=this.EditMode ? "true" : "false" %>, <%=this.NoPics ? "true": "false" %> );
    });
        
</script>
</telerik:RadCodeBlock>
<div id="lb2" runat="server" style="display: table;">
</div>
<% /* %>
<div style="display: table-row;">
    <div class="lbWrap">
        <a href="/coreya"><img src="/DesktopModules/ATI_Base/services/images/profile.aspx?us=6" class="leaderMale"></a>
        <a href="/coreya"><img src="/DesktopModules/ATI_Base/services/images/profile.aspx?us=6" class="leaderFemale"></a>
        <div class="lbWorkout">
            <div class="atiListHeading grad-FFF-EEE" style="margin-top: 5px; text-align: center;">
                <img src="/DesktopModules/ATI_Base/resources/images/iMale.png" style="float: left;" />
                <a href="javascript: ;">Fran</a>   
                <img src="/DesktopModules/ATI_Base/resources/images/iFemale.png" style="float: right;" />                            
            </div>
            <div style="position: relative;">
            <ul class="lbMale">
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
            </ul>
            <ul class="lbFemale">
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
            </ul>
            </div>
        </div>
    </div>
    <div class="lbWrap">
        <a href="/coreya"><img src="/DesktopModules/ATI_Base/services/images/profile.aspx?us=6" class="leaderMale"></a>
        <a href="/coreya"><img src="/DesktopModules/ATI_Base/services/images/profile.aspx?us=6" class="leaderFemale"></a>
        <div class="lbWorkout">
            <div class="atiListHeading grad-FFF-EEE" style="margin-top: 5px; text-align: center;">
                <img src="/DesktopModules/ATI_Base/resources/images/iMale.png" style="float: left;" />
                <a href="javascript: ;">Fran</a>   
                <img src="/DesktopModules/ATI_Base/resources/images/iFemale.png" style="float: right;" />                            
            </div>
            <div style="position: relative;">
            <ul class="lbMale">
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
            </ul>
            <ul class="lbFemale">
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
                <li><a href="javascript: ;">Name name</a> <span>00:12:02</span></li>
            </ul>
            </div>
        </div>
    </div>
</div>
<% */ %>