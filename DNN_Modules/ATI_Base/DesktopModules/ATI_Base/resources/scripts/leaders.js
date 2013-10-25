var __ati_css = 'http://flexfwd.com/DesktopModules/ATI_Base/resources/css/lb/default.css';
if (document.createStyleSheet) {
    document.createStyleSheet(__ati_css);
}
else {
    var styles = "@import url('"+__ati_css+"');";
    var newSS = document.createElement('link');
    newSS.rel = 'stylesheet';
    newSS.href = 'data:text/css,' + escape(styles);
    document.getElementsByTagName("head")[0].appendChild(newSS);
}


if (typeof (Aqufit) == 'undefined') {
    Aqufit = {
        Page: {
            Controls: {
            }
        }
    };
}


Aqufit.Page.Controls.ATI_LeaderBoard2 = function (id, lb, cols, edit, nopics) {
    this.id = id;
    this.$lb = $('#' + lb);
    this.onCloseCallback = null;
    this.editMode = (edit == 'undefined' ? false : edit);
    this.noPics = (nopics == 'undefined' ? false : nopics);
    this.init();
    this.numCols = cols;
    this.colCount = cols;
    this.wodArray = [];
    this.deleteCallback = null;
}

Aqufit.Page.Controls.ATI_LeaderBoard2.prototype = {
    init: function () {

    },
    appendLeaderBoard: function (wod) {
        this.colCount++;
        if (this.colCount >= this.numCols) {
            this.$lb.append('<div style="display: table-row"></div>');
            this.colCount = 0;
        }
        this.$lb.append(
                    '<div class="lbWrap" id="lbWod' + wod.Id + '">' +
                        (this.editMode ?
                        '<a id="lbDelWod' + wod.Id + '" href="javascript: ;">remove [X]</a>'
                        :
                        '') +
                        (wod.Male.length > 0 && !this.noPics ?
                        '<a href="http://flexfwd.com/' + wod.Male[0].UserName + '"><img src="http://flexfwd.com/DesktopModules/ATI_Base/services/images/profile.aspx?us=' + wod.Male[0].UsId + '" class="leaderMale"></a>'
                        :
                        '') +
                        (wod.Female.length > 0 && !this.noPics ?
                        '<a href="http://flexfwd.com/' + wod.Female[0].UserName + '"><img src="http://flexfwd.com/DesktopModules/ATI_Base/services/images/profile.aspx?us=' + wod.Female[0].UsId + '" class="leaderFemale"></a>'
                        :
                        '') +
                        '<div class="lbWorkout">' +
                            '<div class="atiListHeading grad-FFF-EEE" style="margin-top: 5px; text-align: center;">' +
                                '<img src="http://flexfwd.com/DesktopModules/ATI_Base/resources/images/iMale.png" style="float: left;" />' +
                                '<a href="http://flexfwd.com/workout/' + wod.Id + '">' + wod.Name + '</a>' +
                                '<img src="http://flexfwd.com/DesktopModules/ATI_Base/resources/images/iFemale.png" style="float: right;" />' +
                            '</div>' +
                            '<div style="position: relative;">' +
                            '<ul class="lbMale">' +
                            '</ul>' +
                            '<ul class="lbFemale">' +
                            '</ul>' +
                            '</div>' +
                        '</div>' +
                    '</div>'
                    );
        var that = this;
        $('#lbDelWod' + wod.Id).click(function (event) {
            if (that.deleteCallback != null) {
                if (confirm("Are you sure you want to delete this workout from your leader board?")) {
                    that.deleteCallback(wod.Id);
                    $('#lbWod' + wod.Id).remove();
                }
            } else {
                alert('no deleteCallback defined');
            }
            event.stopPropagation();
            return false;
        });
        var $mlist = $('#lbWod' + wod.Id + ' ul.lbMale');
        var m = 0;
        for (m = 0; m < wod.Male.length; m++) {
            $mlist.append('<li><a href="http://flexfwd.com/' + wod.Male[m].UserName + '">' + wod.Male[m].UserName + '</a> <span>' + wod.Male[m].Score + '</span></li>');
        }
        var $flist = $('#lbWod' + wod.Id + ' ul.lbFemale');
        var f = 0;
        for (f = 0; f < wod.Female.length; f++) {
            $flist.append('<li><a href="http://flexfwd.com/' + wod.Female[f].UserName + '">' + wod.Female[f].UserName + '</a> <span>' + wod.Female[f].Score + '</span></li>');
        }
        for (f; f < m; f++) {
            $flist.append('<li>&nbsp;</li>');
        }
        for (m; m < f; m++) {
            $mlist.append('<li>&nbsp;</li>');
        }
    },
    appendLeaderBoardJson: function (json) {        
        var data = eval('(' + json + ')');
        // data is a flat array of Male, and Female times... we need to organize them
        var newNames = [];
        for (var i = 0; i < data.length; i++) {
            var d = data[i];
            if (!this.wodArray[d.Name]) {
                newNames[d.Name] = d.Name;
                this.wodArray[d.Name] = { Male: [], Female: [], Name: d.Name, Id: d.Id };
            }
            if (d.Sex == 'M') {
                this.wodArray[d.Name].Male = d.Data;
            } else {
                this.wodArray[d.Name].Female = d.Data;
            }
        }
        for (var i in newNames) {
            this.appendLeaderBoard(this.wodArray[i]);
        }        
    },
    load: function () {
        this.$lb.append('<a style="position: absolute; top: -25px; background-color: transparent;" href="http://flexfwd.com"><img src="http://flexfwd.com/DesktopModules/ATI_Base/resources/images/powered.png" /></a><br />');
        if (__ati_json) {
            this.loadLeaderBoardFromJson(__ati_json);
        }
        this.$lb.wrap('<div class="atiLbBorder"><a class="lbTitle" href="http://flexfwd.com/group/' + __ati_group.UserName + '">' + __ati_group.Name + '</a>');
    },
    loadLeaderBoardFromJson: function (json) {
        var data = eval('(' + json + ')');
        // data is a flat array of Male, and Femal times... we need to organize them
        for (var i = 0; i < data.length; i++) {
            var d = data[i];
            if (!this.wodArray[d.Name]) {
                this.wodArray[d.Name] = { Male: [], Female: [], Name: d.Name, Id: d.Id };
            }
            if (d.Sex == 'M') {
                this.wodArray[d.Name].Male = d.Data;
            } else {
                this.wodArray[d.Name].Female = d.Data;
            }
        }
        for (var i in this.wodArray) {
            this.appendLeaderBoard(this.wodArray[i]);
        }
        for (var c = this.colCount; c < (this.numCols-1); c++) {
            this.$lb.append('<div class="lbWrap">&nbsp;</div>');
        }
    }
};
