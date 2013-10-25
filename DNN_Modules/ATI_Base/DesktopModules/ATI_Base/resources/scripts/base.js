/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Author: Corey Auger
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Namespace Aqufit
Aqufit = {
    $: function (id) {
        return document.getElementById(id);
    },
    Data: {
},
JavaScriptStringEncode: function (sString) {
    return (sString + "").replace(/[\0-\x1F\"\\\x7F-\xA0\u0100-\uFFFF]/g, function (sChar) {
        switch (sChar) {
            case "\b": return "\\b";
            case "\t": return "\\t";
            case "\n": return "\\n";
            case "\f": return "\\f";
            case "\r": return "\\r";
            case "\\": return "\\\\";
            case "\"": return "\\\"";
        }
        var iChar = sChar.charCodeAt(0);
        if (iChar < 0x10) return "\\x0" + iChar.toString(16);
        if (iChar < 0x100) return "\\x" + iChar.toString(16);
        if (iChar < 0x1000) return "\\u0" + iChar.toString(16);
        return "\\u" + iChar.toString(16);
    });
},
Serialize: function (xValue) {
    switch (typeof (xValue)) {
        case "undefined": return "void(0)";
        case "boolean": return xValue.toString();
        case "number": return xValue.toString();
        case "string": return "\"" + Aqufit.JavaScriptStringEncode(xValue) + "\"";
        case "function": return "eval(\"" + Aqufit.JavaScriptStringEncode(xValue.toString()) + "\")";
        case "object":
            if (xValue == null) return "null";
            var bArray = true;
            var asObjectValues = [], asArrayValues = [], iCounter = 0, iLength = null;
            for (var i in xValue) {
                if (bArray) switch (i) {
                    case "length":
                        // Part of an array but not stored, keep so we can check
                        // if the length is correct
                        break;
                    case iCounter.toString():
                        // Part of an array and stored, but he index must be sequential starting at 0.
                        iCounter++;
                        asArrayValues.push(Aqufit.Serialize(xValue[i]));
                        break;
                    default:
                        // Not an array
                        bArray = false;
                }
                asObjectValues.push(Aqufit.Serialize(i) + ":" + Aqufit.Serialize(xValue[i]));
            }
            if (bArray) {
                try {
                    bArray &= (xValue.length == iCounter);
                } catch (e) {
                    bArray = false;
                }
            }
            return (bArray ?
        				"[" + asArrayValues.join(",") + "]" :
        				"{" + asObjectValues.join(",") + "}"
        			);
        default:
            throw new Error("Objects of type " + typeof (xValue) + " cannot be serialized.");
    }
},
addLoadEvent: function (func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    }
    else {
        window.onload = function () {
            oldonload();
            func();
        }
    }
},
NavigationRule: {
    map: {},
    Add: function (name, fun) {
        Aqufit.NavigationRule.map[name] = fun;
    },
    Call: function (name) {
        if (Aqufit.NavigationRule.map[name]) {
            Aqufit.NavigationRule.map[name]();
        }
    }
},
Utils: {
    parseDate: function (date) {
        var dSplit = date.split('/');
        var m = parseInt(dSplit[0]) - 1;
        var d = parseInt(dSplit[1]);
        var y = parseInt(dSplit[2]);
        date = new Date(y, m, d);
        var da = new Date();
        var gmtHours = da.getTimezoneOffset() / 60;
        date.setTime(date.getTime() - (gmtHours * 60 * 60 * 1000));
        return date;
    },
    toLocalTime: function (date) {
        if (typeof (date) == "string") {
            // parse string ex: '6/30/2010 10:17:11 PM'
            var dt = date.substr(0, date.indexOf(' '));
            var tt = date.substr(date.indexOf(' ') + 1);
            var dSplit = dt.split('/');
            var m = parseInt(dSplit[0]) - 1;
            var d = parseInt(dSplit[1]);
            var y = parseInt(dSplit[2]);
            var tSplit = tt.split(':');
            var hh = parseInt(tSplit[0]);
            var mm = parseInt(tSplit[1]);
            var ss = parseInt(tSplit[2]);
            if (tSplit[2].indexOf('PM') > 0) {
                hh = hh + 12;
            }
            date = new Date(y, m, d, hh, mm, ss);
        }
        var daa = new Date();
        var gmtHours = daa.getTimezoneOffset() / 60;
        date.setTime(date.getTime() - (gmtHours * 60 * 60 * 1000));
        return date;
    },
    toTimeAgoString: function (date) {
        var today = new Date();
        date = Aqufit.Utils.toLocalTime(date);
        //    date.add('m', -(date.getTimezoneOffset()));
        var diff = today.getTime() - date.getTime();
        var days = diff / (1000 * 60 * 60 * 24);
        if (days < 1) {
            var hours = (days - Math.floor(days)) * 24;
            if (hours < 1) {
                var min = (hours - Math.floor(hours)) * 60;
                if (min < 1) {
                    var sec = (min - Math.floor(min)) * 60;
                    return Math.floor(sec) + " seconds ago";
                }
                return Math.floor(min) + " minutes ago";
            }
            return Math.floor(hours) + " hours ago";
        }
        return Math.floor(days) + " days ago";

    },
    toPaceString: function (dur, dist, unit) {
        if (dist <= 0 || dur <= 0) {
            return "00:00:00";
        }
        // duration is in millis
        // distance is in meters
        var totalDist = Aqufit.Units.convert(Aqufit.Units.UNIT_M, dist, unit);
        var pace = (1 / totalDist) * dur;
        return Aqufit.Utils.toDurationString(pace);
    },
    getDataSrcName: function (ds) {
        if (ds == Aqufit.DataSrc.MANUAL_NO_MAP) {
            return "Manual Entry";
        } else if (ds == Aqufit.DataSrc.MANUAL_WITH_MAP) {
            return "Mapped Route";
        } else if (ds == Aqufit.DataSrc.NIKE_NO_MAP) {
            return "Nike+";
        } else if (ds == Aqufit.DataSrc.NIKE_WITH_MAP) {
            return "Nike+";
        } else if (ds == Aqufit.DataSrc.GARMIN) {
            return "Mapped with Garmin";
        } else if (ds == Aqufit.DataSrc.IPHONE) {
            return "iPhone Entry";
        } else if (ds == Aqufit.DataSrc.ANDROID) {
            return "Android Entry";
        } else if (ds == Aqufit.DataSrc.BLACKBERRY) {
            return "Blackberry Entry";
        } else if (ds == Aqufit.DataSrc.MOBILE) {
            return "Mobile Entry";
        }
        return "";
    },
    toDurationString: function (dur) {
        // duration is in millis
        var hh = dur / (60 * 60 * 1000);
        var th = Math.floor(hh);
        var mm = (hh - th) * 60;
        var tm = Math.floor(mm);
        var ss = (mm - tm) * 60;
        var ts = Math.round(ss);
        var ret = "";
        ret += (th < 10 ? "0" + th : th) + ":";
        ret += (tm < 10 ? "0" + tm : tm) + ":";
        ret += (ts < 10 ? "0" + ts : ts);
        return ret;
    },
    createElement: function (type, attrs) {
        var elm = document.createElement(type);
        if (attrs) {
            for (var aname in attrs) {
                elm.setAttribute(aname, attrs[aname]);
            }
        }
        return elm;
    },
    removeChildren: function (parent) {
        if (parent.hasChildNodes()) {
            while (parent.childNodes.length >= 1) {
                parent.removeChild(parent.firstChild);
            }
        }
    },
    getWinDims: function () {
        if (self.innerWidth) {
            _ac_winWidth = self.innerWidth;
            _ac_winHeight = self.innerHeight;
        }
        else if (document.documentElement && document.documentElement.clientWidth) {
            _ac_winWidth = document.documentElement.clientWidth;
            _ac_winHeight = document.documentElement.clientHeight;
        }
        else if (document.body) {
            _ac_winWidth = document.body.clientWidth;
            _ac_winHeight = document.body.clientHeight;
        }
        return [_ac_winWidth, _ac_winHeight];
    },
    findPos: function (obj) {
        var curleft = curtop = 0;
        if (obj.offsetParent) {
            curleft = obj.offsetLeft
            curtop = obj.offsetTop
            while (obj = obj.offsetParent) {
                curleft += obj.offsetLeft
                curtop += obj.offsetTop
            }
        }
        return [curleft, curtop];
    },
    round: function (val, places) {
        val = val * (10 * places);
        val = Math.round(val);
        return val / (10 * places);
    }
},
Const: {
    TIMEOUT: 5000
},
DataSrc: {
    MANUAL_NO_MAP: 1,
    MANUAL_WITH_MAP: 2,
    NIKE_NO_MAP: 3,
    NIKE_WITH_MAP: 4,
    GARMIN: 5,
    IPHONE: 6,
    ANDROID: 7,
    BLACKBERRY: 8,
    MOBILE: 9
},
Stream: {
    WORKOUT: 0,
    SHOUT: 1,
    NOTIFICATION: 2,
    RECIPE: 3
},
Permission: {
    OWNER: 0,
    FRIEND: 1,
    USER: 3,
    PUBLIC: 2
},
Metric: {
    NUM_RECIPES: 0,
    NUM_FOLLOWERS: 1,
    NUM_YOU_FOLLOW: 2
},
WorkoutTypes: {
    WALKING: 1,
    RUNNING: 2,
    CYCLING: 3,
    SWIMMING: 4,
    WEIGHTS: 5,
    CROSSFIT: 6,
    ROW: 7,
    OTHER: 8,
    toString: function (type) {
        switch (type) {
            case Aqufit.WorkoutTypes.WALKING:
                return "Walk";
            case Aqufit.WorkoutTypes.RUNNING:
                return "Run";
            case Aqufit.WorkoutTypes.CYCLING:
                return "Ride";
            case Aqufit.WorkoutTypes.SWIMMING:
                return "Swim";
            case Aqufit.WorkoutTypes.WEIGHTS:
                return "Weights";
            case Aqufit.WorkoutTypes.CROSSFIT:
                return "CrossFit";
            case Aqufit.WorkoutTypes.ROW:
                return "Row";
            case Aqufit.WorkoutTypes.OTHER:
                return "Other";
        }
    }
},
Relationship: {
    FRIEND: 0,
    FOLLOW: 1,
    NONE: 2,
    GROUP_OWNER: 3,
    GROUP_ADMIN: 4,
    GROUP_MEMBER: 5
},
WODTypes: {
    TIMED: 1,
    AMRAP: 2,
    SCORE: 3,
    MAXWEIGHT: 4
},
Page: {
    SiteName: 'FlexFWD',
    SiteUrl: null,
    UserId: 0,
    PortalId: 0,
    PofileId: 0,
    UserEmail: null,
    UserFirstName: null,
    UserLastName: null,
    Permission: null,
    PageBase: null,
    DistanceUnits: 1,   // UNIT_MILES
    WeightUnits: 5,     // UNIT_LBS
    UserName: null,
    Controls: {
    // page controls    
}
},
Windows: {
// stores instances of RAD windows.
},
Services: {
// store service call function ??
},
Units: {
    //UNIT_INCHES = 0, UNIT_MILES, UNIT_CM, UNIT_M, UNIT_KM, UNIT_LBS, UNIT_KG, UNIT_FT_IN
    UNIT_INCHES: 0,
    UNIT_IN: 0,
    UNIT_MILES: 1,
    UNIT_CM: 2,
    UNIT_M: 3,
    UNIT_KM: 4,
    UNIT_LBS: 5,
    UNIT_LB: 5,
    UNIT_KG: 6,
    UNIT_FT_IN: 7,
    convert: function (fromUnit, fromUnitVal, toUnit) {
        var interchange = 0.0;
        var retVal = 0.0;
        // Convert to our standard interchange unit.
        switch (fromUnit) {
            case Aqufit.Units.UNIT_INCHES:
                interchange = fromUnitVal * 0.0254;
                break;
            case Aqufit.Units.UNIT_MILES:
                interchange = fromUnitVal * 1609.344;
                break;
            case Aqufit.Units.UNIT_CM:
                interchange = fromUnitVal * 0.01;
                break;
            case Aqufit.Units.UNIT_M:
                interchange = fromUnitVal;
                break;
            case Aqufit.Units.UNIT_KM:
                interchange = fromUnitVal * 1000;
                break;
            case Aqufit.Units.UNIT_LBS:
                interchange = fromUnitVal * 0.45359237;
                break;
            case Aqufit.Units.UNIT_KG:
                interchange = fromUnitVal;
                break;
            case Aqufit.Units.UNIT_FT_IN:
                alert('UNIT_FT_IN not supported: must do a conversion in INCH only');
                break;
        }
        // Convert from our interchange unit.
        switch (toUnit) {
            case Aqufit.Units.UNIT_INCHES:
                retVal = interchange / 0.0254;
                break;
            case Aqufit.Units.UNIT_MILES:
                retVal = interchange / 1609.344;
                break;
            case Aqufit.Units.UNIT_CM:
                retVal = interchange / 0.01;
                break;
            case Aqufit.Units.UNIT_M:
                retVal = interchange;
                break;
            case Aqufit.Units.UNIT_KM:
                retVal = interchange / 1000;
                break;
            case Aqufit.Units.UNIT_LBS:
                retVal = interchange / 0.45359237;
                break;
            case Aqufit.Units.UNIT_KG:
                retVal = interchange;
                break;
            case Aqufit.Units.UNIT_FT_IN:
                alert('UNIT_FT_IN not supported: must do a conversion in INCH only');
                break;
        }
        return retVal;
    },
    getUnitName: function (unit) {
        switch (unit) {
            case this.UNIT_INCHES:
                return "IN";
                break;
            case this.UNIT_MILES:
                return "Mi";
                break;
            case this.UNIT_CM:
                return "cm";
                break;
            case this.UNIT_M:
                return "m";
                break;
            case this.UNIT_KM:
                return "km";
                break;
            case this.UNIT_LBS:
                return "Lb";
                break;
            case this.UNIT_KG:
                return "kg";
                break;
            case this.UNIT_FT_IN:
                return "ft";
                break;
        }
        return "";
    },
    SaveUnitChangeSetting: function (id, type) {
        var ddlUnitsType = Aqufit.$(id);
        var units = ddlUnitsType[ddlUnitsType.selectedIndex].value;
        Affine.WebService.UtilService.SaveUnitChangeSetting(Aqufit.Page.UserId, Aqufit.Page.PortalId, type, units, Aqufit.Units.SaveUnitChangeSettingCallback, WebServiceFailedCallback);
    },
    SaveUnitChangeSettingCallback: function (result) {
        // This means the unit prefs are saved.
        //alert(result);
    }
}
};                                                         // END Aqufit namespace 



function checkReal(evt) {
    var target = '';
    if (!evt) {
        evt = window.event;
        target = evt.srcElement;
    } else {
        target = evt.target;
    }
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var ch = String.fromCharCode(charCode);
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;

}

function WebServiceFailedCallback(error) {
    var stackTrace = error.get_stackTrace();
    var message = error.get_message();
    var statusCode = error.get_statusCode();
    var exceptionType = error.get_exceptionType();
    var timedout = error.get_timedOut();

    // Display the error.

    var txtMessage = "{" +
        "Stack Trace: " + stackTrace + "; " +
        "Service Error: " + message + "; " +
        "Status Code: " + statusCode + "; " +
        "Exception Type: " + exceptionType + "; " +
        "Timedout: " + timedout + "}";
    alert(txtMessage);
}


function OnRequestStart(sender, eventArgs) {
    $('#atiStatusWidget').hide();
}
function OnResponseEnd(sender, eventArgs) {
    if ($('#atiStatusWidget').size()) {
        $('#atiStatusWidget').fadeIn('slow');
        setTimeout(function () {
            $('#atiStatusWidget').fadeOut('slow');
        }, Aqufit.Const.TIMEOUT);
    }
}
function UpdateStatus(status) {
    $('#statusMsg').html('');
    $('#statusMsg').append(status);
}


/////////////////////////////////////////////////////
//          *** Stream Script
/////////////////////////////////////////////////////


Aqufit.Page.Controls.atiStreamPanel = function (id, control, mode, skip, take, isSearch, editUrl, showTopPager, showBottomPager, showStreamSelect, atiStateSkipId, atiLoadingId) {
    this.id = id;
    this.controlId = '#' + control;
    this.json = null;
    this.list = null;
    this.start = parseInt(skip);
    this.take = take;
    this.mode = mode;
    this.isSearchMode = isSearch;
    this.editUrl = editUrl;
    this.MenuOptions = [];
    this.streamDeleteCallback = null;
    this.showTopPager = showTopPager;
    this.showBottomPager = showBottomPager;
    this.showStreamSelect = showStreamSelect;
    this.$atiStateSkip = $('#' + atiStateSkipId);
    this.$atiLoading = $('#' + atiLoadingId);
    this.onDeleteComment = null;
    this.onNeedData = null;
    this.noDataHtml = '<h2>No Data</h2>';    
};

Aqufit.Page.Controls.atiStreamPanel.prototype = {
    commentSaveSuccess: function (comm) {
        var comment = eval("(" + comm + ")");
        if (comment["StreamKey"] > 0) {
            var $comList = $('#atiCommentAdd' + comment["StreamKey"]);
            $comList.before('<li id="atiComment' + comment["Id"] + '">' +
                                            '<div class="commentBoxLeft"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + comment["UserSettingId"] + '" /></div>' +
                                            '<div class="commentBoxRight">' +
                                                '<a class="username" href="' + Aqufit.Page.PageBase + comment["UserName"] + '">' + comment["UserName"] + '</a>' +
                                                '<span>&nbsp;-&nbsp;' + comment["Text"] + '<br /></span>' +
                                                '<ol><li><a href="javascript: ;" class="time">' + Aqufit.Utils.toTimeAgoString(new Date(comment["DateTicks"])) + '</a></li></ol>' +
                                            '</div>' +
                                        '</li>');
            $("#atiComment" + comment["Id"]).hide().slideDown("slow");
            $('#commentTxt' + sd["Id"]).val('');
        } else {
            this.commentSaveFail(comm);
        }
    },
    commentSaveFail: function (comm) {
        // TODO: failed to save comment
        alert('fail');
        var comment = eval("(" + comm + ")");
        if (comment["StreamKey"] > 0) {
            var bComm = Aqufit.$("atiButtonAddComm" + comment["StreamKey"]);
            bComm.enabled = true;
            var txt = Aqufit.$("commentTxt" + sid);
            txt.enabled = true;
        }
    },
    clear: function () {
        $(this.controlId).children().remove();
    },
    deleteStreamFail: function (error) {
        var stackTrace = error.get_stackTrace();
        var message = error.get_message();
        var statusCode = error.get_statusCode();
        var exceptionType = error.get_exceptionType();
        var timedout = error.get_timedOut();
        var txtMessage = "{" +
            "Stack Trace: " + stackTrace + "; " +
            "Service Error: " + message + "; " +
            "Status Code: " + statusCode + "; " +
            "Exception Type: " + exceptionType + "; " +
            "Timedout: " + timedout + "}";
        alert(txtMessage);
    },
    prependJson: function (json) {
        json = eval('(' + json + ')');
        this.generateStreamItem(json, true);
    },
    generateStreamItem: function (sd, prepend) {    
        var numRecipe = 0;
        var followers = 0;
        var following = 0;
        for (var i = 0; i < sd["Metrics"].length; i++) {
            if (sd["Metrics"][i]["MetricType"] == Aqufit.Metric.NUM_RECIPES) {
                numRecipe = sd["Metrics"][i]["MetricValue"];
            } else if (sd["Metrics"][i]["MetricType"] == Aqufit.Metric.NUM_FOLLOWERS) {
                followers = sd["Metrics"][i]["MetricValue"];
            } else if (sd["Metrics"][i]["MetricType"] == Aqufit.Metric.NUM_YOU_FOLLOW) {
                following = sd["Metrics"][i]["MetricValue"];
            }
        }
        var linkTitle = sd["Title"].replace(/ /g, '_');
        var shareTitle = Aqufit.WorkoutTypes.toString(sd["WorkoutType"]) + ' - ' + (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT ?
                                (sd["Score"] > 0 ? Aqufit.Utils.round(sd["Score"], 2) : sd["Max"] > 0 ? 'Max ' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, sd["Max"], Aqufit.Page.WeightUnits), 2) + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.WeightUnits) : Aqufit.Utils.toDurationString(sd["Duration"]))
                                :
                                '' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_M, sd["Distance"], Aqufit.Page.DistanceUnits), 2) + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.DistanceUnits) + ' time: ' + Aqufit.Utils.toDurationString(sd["Duration"]) + ' / ' +
                                'pace: ' + Aqufit.Utils.toPaceString(sd["Duration"], sd["Distance"], Aqufit.Page.DistanceUnits))
        linkTitle = linkTitle.replace(/\+/g, '');
        linkTitle = escape(linkTitle);
        var recipeUrl = Aqufit.Page.PageBase + 'recipe/' + sd["Id"] + '/';
        var shareUrl = Aqufit.Page.SiteUrl + Aqufit.Page.ProfileUserSettingsId + 'workout/' + sd["Id"] + '/';
        var statsLink = Aqufit.Page.PageBase + sd["UserName"] + "/workout/" + sd["Id"];
        var workoutLink = statsLink;
        if (sd["DataSrc"] == Aqufit.DataSrc.MANUAL_WITH_MAP) {
            workoutLink = Aqufit.Page.PageBase + "workout/" + sd["Id"] + "/route/";
        } else if (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT) {
            workoutLink = Aqufit.Page.SiteUrl + 'workout/' + sd["WODKey"];
        }
        if (sd["StreamType"] == Aqufit.Stream.NOTIFICATION) {
            statsLink = Aqufit.Page.PageBase + 'fitnessprofile/requests?n=' + sd["Id"];
            workoutLink = Aqufit.Page.PageBase + 'fitnessprofile/requests?n=' + sd["Id"];
        }
        var html = '<li id="atiStreamItem' + sd["Id"] + '" class="atiStreamItem" style="border-top: 1px solid #EEE;">' +
                        '<div class="atiStreamItemLeft">' +
                            ((sd["StreamType"] == Aqufit.Stream.RECIPE) ?
                            '<a class="title" href="' + recipeUrl + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/recipe.aspx?r=' + sd["Id"] + '" class="dropShadow" /></a>'
                            :
                            (sd["StreamType"] == Aqufit.Stream.NOTIFICATION) ?
                            '<a class="title" href="' + recipeUrl + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/notificationType' + sd["NotificationType"] + '.png" class="dropShadow" /></a>'
                            :
                            '<a href="' + Aqufit.Page.PageBase + sd["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + sd["UserSettingId"] + '" class="dropShadow" /></a>'
                            ) +
                            (sd["IsPr"] ?
                            '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/pr.png" style="position: absolute; z-index: 99; bottom: 5px; left: 50px;" />'
                            :
                            '') +
                        '</div>' +
                        '<div class="atiStreamItemRight">' +
                            ((sd["StreamType"] == Aqufit.Stream.RECIPE) ?
                            '<div style="float:left;"><input type="radio" class="rate" value="0.5"/><input type="radio" class="rate" value="1"/><input type="radio" class="rate" value="1.5"/><input type="radio" class="rate" value="2"/><input type="radio" class="rate" value="2.5"/><input type="radio" class="rate" value="3"/><input type="radio" class="rate" value="3.5"/><input type="radio" class="rate" value="4" /><input type="radio" class="rate" value="4.5"/><input type="radio" class="rate" value="5"/><span class="stars">' + sd["AvrRating"] + '&nbsp;Stars</span></div>' +
                            '<div style="float:right; margin-right: 10px;"><input type="radio" class="strict" value="1"/><input type="radio" class="strict" value="2"/><input type="radio" class="strict" value="3"/><span class="stars">Paleo&nbsp;Strict</span></div>' +
                            '<div class="atiUserBCard">' +
                            '<div style="float:left; width: 50px;"><a href="' + Aqufit.Page.PageBase + sd["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + sd["UserSettingId"] + '" /></a></div>' +
                                '<div style="float:right; margin-right: 10px; width: 340px;">' +
                                    '<a style="font-weight: bold; font-size: 14px; float: right; color: #F48401;" href="' + Aqufit.Page.PageBase + sd["UserName"] + '">' + sd["UserName"] + '</a>' +
                                    '<span class="atiUserStats">Num Recipies: <em>' + numRecipe + '</em><br />Followers: <em>' + followers + '</em><br />Following: <em>' + following + '</em></span>' +
                                '</div>' +
                            '</div>' +
                            '<div style="clear: both;" />'
                            :
                            (sd["StreamType"] == Aqufit.Stream.NOTIFICATION) ?
                            '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/src0.png" />'
                            :                            
                            (sd["ToUserSettingKey"] > 0 ?
                            '<a class="username" href="' + Aqufit.Page.PageBase + sd["UserName"] + '">' + sd["UserName"] + '</a><em>&gt;</em> <a class="username" href="' + Aqufit.Page.PageBase + sd["ToUserName"] + '">' + sd["ToUserName"] + '</a>'
                            :
                            '<a class="username" href="' + Aqufit.Page.PageBase + sd["UserName"] + '">' + sd["UserName"] + '</a><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/src0.png" />')
                            ) +
                            ((sd["StreamType"] == Aqufit.Stream.WORKOUT) ?   // is this a workout
                                '<a class="atiWorkoutLink" href="' + workoutLink + '" title="' + Aqufit.Utils.getDataSrcName(sd["DataSrc"]) + '">' + Aqufit.WorkoutTypes.toString(sd["WorkoutType"]) + '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/src' + sd["DataSrc"] + '.png" /></a>&nbsp;' +
                                '<span class="dist grad-FFF-EEE ui-corner-all">&nbsp;&nbsp;<em style="font-weight: bold;">' +
                                (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT ?
                                (sd["IsRx"] ?
                                '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/src99.png" style="float: left;" />'
                                :
                                ''
                                ) +
                                (sd["Score"] > 0 ? sd["Score"] : sd["Max"] > 0 ? 'Max ' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, sd["Max"], Aqufit.Page.WeightUnits), 2) + '&nbsp;' + Aqufit.Units.getUnitName(Aqufit.Page.WeightUnits) : sd["Duration"] > 0 ? Aqufit.Utils.toDurationString(sd["Duration"]) : '')
                                :
                                '<em style="font-weight: bold;">' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_M, sd["Distance"], Aqufit.Page.DistanceUnits), 2) + '&nbsp;' + Aqufit.Units.getUnitName(Aqufit.Page.DistanceUnits) + '</em>&nbsp;/&nbsp;<em style="font-weight: bold;">' + Aqufit.Utils.toDurationString(sd["Duration"]) + '</em></span>' +
                                '<span class="pace" id="atiPace' + sd["id"] + '">' + Aqufit.Utils.toPaceString(sd["Duration"], sd["Distance"], Aqufit.Page.DistanceUnits) + '&nbsp;pace</span>')
                            : '') + '</em></span>' + // else was not a workout
                             ((Aqufit.Page.UserId == sd["UserKey"]) ? // if this is the owner
                                '<a class="deleteStream" id="aDelStream' + sd["Id"] + '" style="display:none; cursor: pointer;" title=\"delete\">[X]</a>'
                                :
                                '') +
                            ((sd["StreamType"] == Aqufit.Stream.RECIPE) ?
                            '<a class="title" href="' + recipeUrl + '">' + sd["Title"] + '</a>'
                            :
                            (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT ?
                            '<a href="' + workoutLink + '" class="title">' + sd["Title"] + '</a>'
                            :
                            '<a href="' + workoutLink + '" class="title">' + sd["Title"] + '</a>')
                            ) +
                            '<p>' + ((sd["Description"] != null ? sd["Description"] : '')) + '&nbsp;</p>' +
                            '<div id="attachments_' + this.id + '_' + sd["Id"] + '" style="position: relative; margin-top: 10px;"></div>' +
                            '<ul class="hlist streamTools">' +
                                '<li>' +
                                    '<a href="javascript: ;" class="time" title="' + Aqufit.Utils.toLocalTime(sd["DateTicks"]).toString() + '">' + Aqufit.Utils.toTimeAgoString(sd["DateTicks"]) + '</a>' +
                                '</li>' +
                                '<li class="shareStream">' +
                                    ((sd["StreamType"] == Aqufit.Stream.NOTIFICATION) ?
                                    (sd["WODKey"] > 0 ?
                                    '<button id="bWODInfo' + sd["Id"] + '">View Workout Info</button>'
                                    :
                                    (sd["MessageKey"] > 0) ?
                                    '<button id="bMessageInfo' + sd["Id"] + '">View Message</button>'
                                    :
                                    '') +
                                    '<button id="bRemNotification' + sd["Id"] + '">Remove Notification</button>'
                                    :
                                    '<a href=\"' + statsLink + '\" title="Compare Stats"><img src=\"' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iStats_s.png\" /></a>' +
                                    '<span class="toolDiv">|</span>' +
                                    '<a target=\"_blank\" href=\"http://twitter.com/share?url=' + shareUrl + '&related=flexfwd&text=' + shareTitle + '\" title="Share on Twitter"><img src=\"' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iTwitter.png\" /></a>' +
                                    '<span class="toolDiv">|</span>' +
                                    ((sd["StreamType"] != Aqufit.Stream.NOTIFICATION && sd["StreamType"] != Aqufit.Stream.RECIPE && Aqufit.Page.UserSettingsId > 0)
                                    ?
                                    '<a href="javascript: ;" id="atiLinkAddComment' + sd["Id"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iComment.png" align="absmiddle" />&nbsp;comment</a>'
                                    :
                                    '<span style="padding-right: 9px; padding-left: 9px;">' + sd["Comments"].length + ' comments</span>'
                                    )) +
                                '</li>' +
                            '</ul>' +
                            '<ul class="atiCommentBox" id="atiCommentList' + this.id + '_' + sd["Id"] + '">' +
                            '</ul>' +
                            '<br style="clear: both;">' +
                        '</div></li>';
        var that = this;
        if (prepend) {
            this.list.prepend(html);
            $("#atiStreamItem" + sd["Id"]).hide();
            $("#atiStreamItem" + sd["Id"]).show("slow");
        } else {
            this.list.append(html);
        }
        if (sd["Attachments"].length > 0) {
            var $attachDiv = $('#attachments_' + this.id + '_' + sd["Id"]);
            for (var a = 0; a < sd["Attachments"].length; a++) {
                var attachment = sd["Attachments"][a];
                // should check the type here..
                if (attachment.Type == 1) { // photo
                    $attachDiv.append('<a href="javascript: ;" onclick="Aqufit.Windows.PhotoWin.open(' + attachment.Id + ',\'' + sd["UserName"] + '\');"><img src="' + attachment.ThumbUrl + '" /></a>');
                } else if (attachment.Type == 2) { // link
                    $attachDiv.append('<a href="' + attachment.LinkUrl + '"><div id="imgSelector" style="width: 80px; height: 60px; background-size: 100%; background-repeat:no-repeat; background-image:url(' + attachment.ThumbUrl + ');"></div></a>' +
                                       '<ul style="position: absolute; top: 0px;left: 100px; width: 300px; overflow:hidden;">' +
                                       '<li><a class="username" href="' + attachment.LinkUrl + '">' + attachment.LinkUrl + '</a></li>' +
                                       '<li>' + attachment.Title + '</li>' +
                                       '<li>' + attachment.Description + '</li>' +
                                       '</ul>');
                }
            }
        }
        if ((sd["StreamType"] == Aqufit.Stream.RECIPE)) {
            $('#atiStreamItem' + sd["Id"] + ' input.rate').each(function () {
                if ($(this).val() == sd["AvrRating"]) {
                    $(this).attr('checked', 'true');
                }
            }).rating({
                split: 2,
                readOnly: true
            });
            $('#atiStreamItem' + sd["Id"] + ' input.strict').each(function () {
                if ($(this).val() == sd["AvrStrictness"]) {
                    $(this).attr('checked', 'true');
                }
            }).rating({
                readOnly: true
            });
        }
        if (sd["Comments"] && (sd["StreamType"] != Aqufit.Stream.RECIPE)) {                   // append all comments to the list
            var $commentList = $('#atiCommentList' + that.id + '_' + sd["Id"]);
            for (var j = 0; j < sd["Comments"].length; j++) {
                (function () {  // create a closure 
                    var comment = sd["Comments"][j];
                    $commentList.append('<li id="atiComment' + comment["Id"] + '">' +
                                            '<div class="commentBoxLeft"><a href="' + Aqufit.Page.PageBase + comment["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + comment["UserSettingId"] + '" /></a></div>' +
                                            '<div class="commentBoxRight">' +
                                                '<a class="username" href="' + Aqufit.Page.PageBase + comment["UserName"] + '">' + comment["UserName"] + '</a>' +
                                                '<span style="padding-right: 16px;">&nbsp;-&nbsp;' + comment["Text"] + '<br /></span>' +
                                                '<ul>' +
                                                    '<li><a href="javascript: ;" class="time" title="' + Aqufit.Utils.toLocalTime(comment["DateTicks"]).toString() + '">' + Aqufit.Utils.toTimeAgoString(comment["DateTicks"]) + '</a></li>' +
                                                    ((Aqufit.Page.UserId == comment["UserKey"]) ? // if this is the owner 
                                                    '<li class="deleteComment"><a href="javascript: ;" id="bDelComment' + comment["Id"] + '" class="hidden">[X]</a></li>'
                                                    :
                                                    '') +
                                                    '<li>' +
                                                '</ul>' +
                                            '</div>' +
                                        '</li>');
                    $('#atiComment' + comment["Id"]).hover(function () {
                        $('#bDelComment' + comment["Id"]).removeClass('hidden');
                    }, function () {
                        $('#bDelComment' + comment["Id"]).addClass('hidden');
                    });
                    $('#bDelComment' + comment["Id"]).click(function (event) {
                        if (confirm("Are you sure you want to delete?")) {
                            if (that.onDeleteComment) {
                                $("#atiComment" + comment["Id"]).hide("slow");
                                that.onDeleteComment(comment["Id"]);
                                //Affine.WebService.StreamService.deleteComment(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, comment["Id"], function (json) { that.deleteCommentSuccess(json); }, function (json) { that.deleteStreamFail(json); });
                            }
                        }
                        event.stopPropagation();
                        return false;
                    });
                })();   // end closure ... and call it.
            }
            $commentList.append('<li id="atiCommentAdd' + sd["Id"] + '" style="display:none;">' +
                                    '<div class="commentBoxLeft">' +
                                        '<img src="' + Aqufit.Page.PageBase + "DesktopModules/ATI_Base/services/images/profile.aspx?us=" + Aqufit.Page.UserSettingsId + '" />' +
                                    '</div>' +
                                    '<div class="commentBoxRight">' +
                                        '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/speak.png" class="speak" />' +
                                        '<textarea id="commentTxt' + sd["Id"] + '" />' +
                                        '<button id="atiButtonAddComm' + sd["Id"] + '" type="button">done</button> <span style="font-size: 8px;">max (1024 characters)</span>' +
                                    '</div>' +
                                    '</li>');
            $('#commentTxt' + sd["Id"]).focus(function (event) {
                $(this).addClass('txtCommentFocus');
                event.stopPropagation();
                return false;
            });
            $('#atiButtonAddComm' + sd["Id"]).button().click(function () {
                var $txt = $('#commentTxt' + sd["Id"]);
                if ($txt.val() != "") {
                    $('#atiLinkAddComment' + sd["Id"]).trigger('click');
                    Affine.WebService.StreamService.addComment(Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, sd["Id"], $txt.val(), function (json) { that.commentSaveSuccess(json); }, function (json) { that.commentSaveFail(json); });
                } else {
                    $('#atiLinkAddComment' + sd["Id"]).trigger('click');
                }
            });
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // EVENTS Now attach the events to needed elements.
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //$('#atiLinkAddComment' + sd["Id"]).button({ icons: { primary: 'ui-icon-comment'} })
        $('#atiLinkAddComment' + sd["Id"]).click(function (event) {
            var $txt = $('#atiLinkAddComment' + sd["Id"]);
            var $comment = $("#atiCommentAdd" + sd["Id"]);
            if (!$comment.is(':visible')) {
                $comment.fadeIn("slow");
            } else {
                $comment.fadeOut("slow");
            }
            event.stopPropagation();
            return false;
        });
        if (sd["WODKey"] > 0) {
            $('#bWODInfo' + sd["Id"]).button().click(function (event) {
                top.location.href = Aqufit.Page.PageBase + 'workout/' + sd["WODKey"] + "?rn=" + sd["Id"];
                event.stopPropagation();
                return false;
            });
        }
        /*
        $('#aEditStream' + sd["Id"]).click(function (event) {
        self.location.href= that.editUrl + '?s='+sd["Id"];
        event.stopPropagation();
        return false;
        });
        */
        $('#bMessageInfo' + sd["Id"]).button().click(function (event) {
            top.location.href = Aqufit.Page.PageBase + "Profile/Inbox?m=" + sd["MessageKey"] + "&rn=" + sd["Id"];
            event.stopPropagation();
            return false;
        });
        $('#bRemNotification' + sd["Id"]).button();
        $('#aDelStream' + sd["Id"] + ', #bRemNotification' + sd["Id"]).click(function (event) {
            if (confirm("Are you sure you want to delete?")) {
                if (that.streamDeleteCallback) {
                    that.streamDeleteCallback(sd["Id"]);
                    $('#atiStreamItem' + sd["Id"]).hide("slow");
                    $('#atiStreamItem' + sd["Id"]).children().remove();
                } else {
                    alert("no streamDeleteCallback has been set");
                }
            }
            event.stopPropagation();
            return false;
        });

        $('#atiStreamItem' + sd["Id"]).hover(function () {
            $('#aDelStream' + sd["Id"]).show();
        }, function () {
            $('#aDelStream' + sd["Id"]).hide();
        });

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
    },
    clearAndShowLoad: function () {
        this.clear();
        this.$atiLoading.show();
    },
    generateStreamDom: function (json) {
        $('#bStreamNext, #bStreamNext2').button('enable');
        if (this.start < 0) {
            this.start = 0;
        }
        this.json = eval("(" + json + ")");
        var that = this;
        var atEnd = false;
        if (this.json.length < this.take) {
            atEnd = true;
        }
        $(this.controlId).css('width', '100%');
        if (this.json.length == 0 && this.start == 0) {
            $(this.controlId).append('<div style="text-align: center;padding-top: 30px; padding-bottom: 30px;">' + this.noDataHtml + '<ul class="atiStreamList" id="atiStreamList' + this.id + '"></ul></div>');
            this.list = $('#atiStreamList' + this.id);
            this.$atiLoading.hide();
        } else {
            if (this.$atiLoading.is(':visible')) {    // this is a hack.. (we use this to tell if this is the first time rendered or not) 
                $(this.controlId).append((this.showTopPager ?
                                            '<div class="messageListHead">' +
                                               '<div><div style="float: right;"><button id="bStreamBack">Newer</button> ' + this.start + ' to ' + (this.start + this.json.length) + ' <button id="bStreamNext">Older</button></div></div>' +
                                            '</div>' :
                                            '') +
                                            (this.showStreamSelect ?
                                            '<div id="atiStreamSelect"><input type="radio" id="ssFriends" name="radio" ' + (this.mode == 0 ? ' checked="checked"' : '') + '/><label for="ssFriends">' + Aqufit.Page.ProfileUserName + ' + Friends</label><input type="radio" id="ssMe" name="radio"' + (this.mode == 3 ? ' checked="checked"' : '') + '/><label for="ssMe">Just ' + Aqufit.Page.ProfileUserName + '</label></div>'
                                            :
                                            ''
                                            ) +
                                            '<ul class="atiStreamList" id="atiStreamList' + this.id + '"></ul>' +
                                            (this.showBottomPager ?
                                            '<div class="messageListHead">' +
                                                '<div><div style="float: right;"><button id="bBackToFullStream" style="display: none;">Back to full stream</button><button id="bStreamNext2">More ..</button></div></div>' +
                                            '</div>' :
                                            '')
                                            );
                this.list = $('#atiStreamList' + this.id);
                this.$atiLoading.hide();
                if (this.showStreamSelect) {
                    $("#atiStreamSelect").buttonset();
                    $('#ssMe').click(function (event) {
                        that.clear();
                        that.$atiLoading.show();
                        that.getStreamData(3, '');
                        // return false;
                    });
                    $('#ssFriends').click(function (event) {
                        that.clear();
                        that.$atiLoading.show();
                        that.getStreamData(0, '');
                        // return false;
                    });
                }
                $('#bStreamBack, #bStreamBack2').button({
                    icons: {
                        primary: 'ui-icon-seek-prev'
                    }
                }).click(function (event) {
                    that.start = that.start - that.take;
                    that.$atiStateSkip.val(that.start);
                    that.getStreamData(that.mode, that.query);
                    event.stopPropagation();
                    return false;
                });
                $('#bStreamNext, #bStreamNext2').button({
                    icons: {
                        primary: 'ui-icon-seek-next'
                    }
                }).click(function (event) {
                    $('#bStreamNext, #bStreamNext2').button('disable');
                    that.start = that.start + that.take;
                    that.$atiStateSkip.val(that.start);
                    that.getStreamData(that.mode, that.query);
                    event.stopPropagation();
                    return false;
                });

            }
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false);
            }

            if (this.start <= 0) {
                $('#bStreamBack, #bStreamBack2').button('disable');
            }
            if (atEnd) {
                $('#bStreamNext, #bStreamNext2').button('disable');
            }
        }
    },
    getStreamItem: function (id, mode, query) {
        var that = this;
        Affine.WebService.StreamService.getStreamItem(id, function (json) {
            that.generateStreamDom(json);
            $('#bBackToFullStream').button().show().click(function (event) {
                $(this).hide();
                that.clearAndShowLoad();
                that.getStreamData(mode, query);
                event.stopPropagation();
                return false;
            });
        });
    },
    getStreamData: function (mode, query) {
        //this.clear();
        this.mode = mode;
        var that = this;
        this.query = query;
        if (this.onNeedData) {
            this.onNeedData(this.start, this.take);
        } else {
            if (this.isSearchMode) {
                Affine.WebService.StreamService.getRecipeStreamData(Aqufit.Page.PortalId, query, this.start, this.take,
                    function (json) { that.generateStreamDom(json); },
                //WebServiceFailedCallback
                    function () { }
                    );
            } else {
                Affine.WebService.StreamService.getStreamData(Aqufit.Page.GroupSettingsId, Aqufit.Page.UserId, Aqufit.Page.PortalId, Aqufit.Page.ProfileId, Aqufit.Page.Permission, this.mode, this.start, this.take,
                    function (json) { that.generateStreamDom(json); },
                //   WebServiceFailedCallback
                    function () { }
                );
            }
        }
    }

};


/////////////////////////////////////////////////////
//          END Stream Script
/////////////////////////////////////////////////////



/////////////////////////////////////////////////////
//          START TimeSpan Script
/////////////////////////////////////////////////////

Aqufit.Page.Controls.atiTimeSpan = function (id, hh, mm, ss) {
    this.id = id;
    this.$hh = $('#' + hh);
    this.$mm = $('#' + mm);
    this.$ss = $('#' + ss);
}

Aqufit.Page.Controls.atiTimeSpan.prototype = {
    clear: function () {
        this.$hh.val('');
        this.$mm.val('');
        this.$ss.val('');
    },
    getMilliDuration: function () {
        var hh = parseInt(this.$hh.val());
        if (isNaN(hh)) hh = 0;
        var mm = parseInt(this.$mm.val());
        if (isNaN(mm)) mm = 0;
        var ss = parseInt(this.$ss.val());
        if (isNaN(ss)) ss = 0;
        var milli = ((hh * 60 * 60) + (mm * 60) + (ss)) * 1000;
        return milli;
    },
    setTime: function (hh, mm, ss) {
        this.setHour(hh);
        this.setMin(mm);
        this.setSec(ss);
    },
    setHour: function (hh) {
        this.$hh.val(parseInt(ss));
    },
    setMin: function (mm) {
        this.$mm.val(parseInt(ss));
    },
    setSec: function (ss) {
        this.$ss.val(parseInt(ss));
    }
};
/////////////////////////////////////////////////////
//          END TimeSpan Script
/////////////////////////////////////////////////////



/////////////////////////////////////////////////////
//          START FriendList Script
/////////////////////////////////////////////////////


Aqufit.Page.Controls.atiFriendList = function (id, control, mode, isOwner, loadImgSrc, checkImgSrc, errImgSrc, title) { // New object constructor
    this.id = id;
    this.controlId = '#' + control;
    this.mode = mode;
    this.acceptFriendAction = null;
    this.denyFriendAction = null;
    this.sendFriendRequest = null;
    this.isOwner = isOwner;
    this.loadingImgSrc = loadImgSrc;
    this.errImgSrc = errImgSrc;
    this.checkImgSrc = checkImgSrc;
    this.json = null;
    this.list = null;
    this.title = title;
    this.bottomPager = null;
    this.topPager = null;
    this.pinfo = null;
    this.onDataNeeded = null;
    this.onRemoveMember = null;
};


Aqufit.Page.Controls.atiFriendList.prototype = {
    generateStreamItem: function (sd, prepend) {
        var html = '<li id="' + this.id + 'atiStreamItem' + sd["Id"] + '">' +
                        '<div class="friendWrapper' + (sd["RequestStatus"] == 1 ? ' unread' : '') + '">' +
                            '<ul class="hList friendAvatar">' +
                                '<li class="checkboxCell" style="margin-left: 16px;">' +
                                '<input type="checkbox" id="' + this.id + 'atiMessageCheck' + sd["Id"] + '" />' +
                                '</li>' +
                                '<li>' +
                                    (this.mode == "GROUP_LIST" ?
                                    '<a href="' + Aqufit.Page.PageBase + 'group/' + sd["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + sd["Id"] + '" /></a>'
                                    :
                                    '<a href="javascript: ;" onclick="top.location.href=\'' + Aqufit.Page.PageBase + sd["UserName"] + '\'"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + sd["Id"] + '" /></a>'
                                    ) +
                                '</li>' +
                            '</ul>' +
                            '<ul class="hList friendInfo">' +
                                '<li>' +
                                    (this.mode == "GROUP_LIST" ?
                                    '<a class="username" href="' + Aqufit.Page.PageBase + 'group/' + sd["UserName"] + '">' + sd["UserName"] + '</a><br />'
                                    :
                                    '<a class="username" href="javascript: ;" onclick="top.location.href=\'' + Aqufit.Page.PageBase + sd["UserName"] + '\'">' + sd["UserName"] + '</a><br />'
                                    ) +
                                    (sd["FirstName"] ?
                                    '<span>' + sd["FirstName"] + ' ' + sd["LastName"] + '</span>'
                                    :
                                    '<span>' + sd["Name"] + '<br />' + sd["Address"] + '</span>') +
                                '</li>' +
                                '<li class="friendActions">' +
                                    (this.mode == 'FRIEND_RESPONSE' ?
                                    '<button id="' + this.id + 'bFriendAccept' + sd["Id"] + '">Accept Request</button>'
                                    :
                                    ''
                                    ) +
                                    (this.mode == 'FRIEND_REQUEST' ?
                                        (sd["RequestStatus"] == 1 ?
                                        '<span class="pending">Request Pending</span>'
                                        :   // 0 = you are already friends
                                        (sd["RequestStatus"] == 0 ?
                                        'You are already friends'
                                        :
                                        '<a href="javascript: ;" id="' + this.id + 'bFriendRequest' + sd["Id"] + '">Send Friend Request</a>'
                                        ))
                                    :
                                    (this.isOwner ?
                                    '<button style="margin-left: 10px;" id="' + this.id + 'bDelStream' + sd["Id"] + '">Delete</button>' // Delete "remove friend"
                                    :
                                    '')
                                    ) +
                                    (this.mode == 'MEMBER_ADMIN' ?
                                    '<button style="margin-left: 10px;" id="' + this.id + 'bAdminMember' + sd["Id"] + '">MakeAdmin</button>'
                                    :
                                    '') +
                                    (this.mode == 'MEMBER_ADMIN' || this.mode == 'MEMBERADMIN_ADMIN' ?
                                    '<button style="margin-left: 10px;" id="' + this.id + 'bDelStream' + sd["Id"] + '">Remove</button>'
                                    :
                                    ''
                                    ) +
                                    (this.mode == 'GROUP_INVITE' ?
                                        (sd["RequestStatus"] == 0 ?
                                        'Already a Member'
                                        :
                                        '<button style="margin-left: 10px;" id="' + this.id + 'bInviteMember' + sd["Id"] + '">Invite</button>')
                                    :
                                    '') +
                                    (this.mode == 'GROUP_JOIN' ?
                                    '<button style="margin-left: 10px;" id="' + this.id + 'bJoin' + sd["Id"] + '">Join</button><button style="margin-left: 10px;" id="' + this.id + 'bDelStream' + sd["Id"] + '">Remove</button>'
                                    :
                                    '') +
                                '</li>' +
                            '</ul>' +
                        '</div>' +
                        '</li>';
        if (prepend) {
            this.list.prepend(html);
            $('#' + this.id + 'atiStreamItem' + sd["Id"]).hide();
            $('#' + this.id + 'atiStreamItem' + sd["Id"]).show("slow");
        } else {
            this.list.append(html);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // EVENTS Now attach the events to needed elements.
        /////////////////////////////////////////////////////////////////////////////////////////////////////////            
        var cid = this.id;  // (this.id) has new context iside the closures so save it under a new name
        var that = this;
        if (this.mode == "FRIEND_REQUEST") {
            $('#' + this.id + 'bFriendRequest' + sd["Id"]).button({ icons: { primary: "ui-icon-plus"} }).click(function (event) {
                event.stopPropagation();
                $(this).html('sending ... <img src="' + this.loadingImgSrc + '" />');
                $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'EFEFEF');
                if (that.sendFriendRequest != null) {
                    that.sendFriendRequest(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'f1f6f0');
                    $('#' + cid + 'bFriendRequest' + sd["Id"]).html('<img src="' + that.checkImgSrc + '" />');
                    $('#' + cid + 'atiStreamItem' + sd["Id"]).fadeOut(3500);
                } else {
                    alert('No function defined for: sendFriendRequest');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $('#' + cid + 'bFriendRequest' + sd["Id"]).html('<img src="' + that.errImgSrc + '" />');
                }
            });
        } else if (this.mode == "FRIEND_RESPONSE" && this.isOwner) {
            $('#' + this.id + 'bFriendAccept' + sd["Id"]).button({ icons: { primary: "ui-icon-check"} }).click(function (event) {
                event.stopPropagation();
                $(this).html('sending ... <img src="' + this.loadingImgSrc + '" />');
                $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'EFEFEF');
                // TODO: add a spinner graphic by the txt or something.                    
                if (that.acceptFriendAction != null) {
                    that.acceptFriendAction(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'f1f6f0');
                    $('#' + cid + 'bFriendAccept' + sd["Id"]).html('<img src="' + that.checkImgSrc + '" />');
                    $('#' + cid + 'atiStreamItem' + sd["Id"]).fadeOut(3500);
                } else {
                    alert('No function defined for: acceptFriendAction');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $('#' + cid + 'bFriendAccept' + sd["Id"]).html('<img src="' + that.errImgSrc + '" />');
                }
                return false;
            });
            $('#' + cid + 'bDelStream' + sd["Id"]).click(function (event) {
                event.stopPropagation();
                if (confirm("Are you sure you want to reject this friend request?")) {
                    if (that.denyFriendAction != null) {
                        that.denyFriendAction(sd["Id"]);
                        $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                        //    $('#' + cid + 'bFriendAccept' + sd["Id"]).html('<img src="' + that.checkImgSrc + '" />');
                        $('#' + cid + 'atiStreamItem' + sd["Id"]).fadeOut(1500);
                    } else {
                        alert('No function defined for: denyFriendAction');
                        $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'cc9999');
                        $('#' + cid + 'bFriendAccept' + sd["Id"]).html('<img src="' + that.errImgSrc + '" />');
                    }
                }
                event.stopPropagation();
                return false;
            }).button({
                icons: {
                    primary: 'ui-icon-closethick'
                },
                text: false
            });
        } else if (this.mode == 'FOLLOWING_LIST' && this.isOwner) {
            $('#' + cid + 'bDelStream' + sd["Id"]).click(function (event) {
                alert('delete click');
                event.stopPropagation();
                return false;
            }).button({
                icons: {
                    primary: 'ui-icon-closethick'
                },
                text: false
            });
        } else if (this.mode == 'MEMBER_ADMIN' || this.mode == 'MEMBERADMIN_ADMIN') {
            $('#' + cid + 'bDelStream' + sd["Id"]).click(function (event) {
                if (that.onRemoveMember != null) {
                    that.onRemoveMember(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $('#' + cid + 'atiStreamItem' + sd["Id"]).fadeOut(1500);
                } else {
                    alert('No function defined for: onRemoveMember');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'cc9999');
                    $('#' + cid + 'bFriendAccept' + sd["Id"]).html('<img src="' + that.errImgSrc + '" />');
                }
                event.stopPropagation();
                return false;
            }).button({
                icons: {
                    primary: 'ui-icon-closethick'
                },
                text: false
            });
            $('#' + cid + 'bAdminMember' + sd["Id"]).button().click(function (event) {
                if (that.onMakeMemberAdmin != null) {
                    that.onMakeMemberAdmin(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $('#' + cid + 'atiStreamItem' + sd["Id"]).fadeOut(1500);
                } else {
                    alert('No function defined for: onMakeMemberAdmin');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'cc9999');
                    $('#' + cid + 'bFriendAccept' + sd["Id"]).html('<img src="' + that.errImgSrc + '" />');
                }
                event.stopPropagation();
                return false;
            })
        } else if (this.isOwner) {
            $('#' + cid + 'bDelStream' + sd["Id"]).click(function (event) {
                alert('TODO:');
                event.stopPropagation();
                return false;
            }).button({
                icons: {
                    primary: 'ui-icon-closethick'
                },
                text: false
            });
        } else if (this.mode == 'GROUP_INVITE') {
            $('#' + this.id + 'bInviteMember' + sd["Id"]).button().click(function (event) {
                event.stopPropagation();
                $(this).html('sending ... <img src="' + this.loadingImgSrc + '" />');
                $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'EFEFEF');
                if (that.acceptFriendAction != null) {
                    that.acceptFriendAction(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'f1f6f0').fadeOut(3500);
                    $(this).html('<img src="' + that.checkImgSrc + '" />');
                } else {
                    alert('No function defined for: acceptFriendAction');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $(this).html('<img src="' + that.errImgSrc + '" />');
                }
                return false;
            });

        } else if (this.mode == 'GROUP_JOIN') {
            $('#' + this.id + 'bJoin' + sd["Id"]).button().click(function (event) {
                event.stopPropagation();
                $(this).html('sending ... <img src="' + this.loadingImgSrc + '" />');
                $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'EFEFEF');
                if (that.acceptFriendAction != null) {
                    that.acceptFriendAction(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'f1f6f0').fadeOut(3500);
                    $(this).html('<img src="' + that.checkImgSrc + '" />');
                } else {
                    alert('No function defined for: acceptFriendAction');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $(this).html('<img src="' + that.errImgSrc + '" />');
                }
                return false;
            });
            $('#' + cid + 'bDelStream' + sd["Id"]).click(function (event) {
                if (that.denyFriendAction != null) {
                    that.denyFriendAction(sd["Id"]);
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'f1f6f0').fadeOut(3500);
                    $(this).html('<img src="' + that.checkImgSrc + '" />');
                } else {
                    alert('No function defined for: acceptFriendAction');
                    $('#' + cid + 'atiStreamItem' + sd["Id"] + ' .friendWrapper').css('background-color', 'ffebe8');
                    $(this).html('<img src="' + that.errImgSrc + '" />');
                }
                event.stopPropagation();
                return false;
            }).button({
                icons: {
                    primary: 'ui-icon-closethick'
                },
                text: false
            });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
    },
    setTopPager: function (pager) {
        this.topPager = pager;
        this.topPager.hide();
        var that = this;
        this.topPager.onPageBack = function (skip, take, len) {
            if (that.onDataNeeded) {
                that.onDataNeeded(skip, take);
            }
        }
        this.topPager.onPageForward = function (skip, take, len) {
            if (that.onDataNeeded) {
                that.onDataNeeded(skip, take);
            }
        }
        if (this.pinfo) {
            this.topPager.setPagerInfo(this.pinfo);
            if (this.pinfo.Length > this.pinfo.Take) {
                this.topPager.show();
            }
        }
    },
    setBottomPager: function (pager) {
        this.bottomPager = pager;
        this.bottomPager.hide();
        var that = this;
        this.bottomPager.onPageBack = function (skip, take, len) {
            if (that.onDataNeeded) {
                that.onDataNeeded(skip, take);
            }
        }
        this.bottomPager.onPageForward = function (skip, take, len) {
            if (that.onDataNeeded) {
                that.onDataNeeded(skip, take);
            }
        }
        if (this.pinfo) {
            this.bottomPager.setPagerInfo(this.pinfo);
            if (this.pinfo.Length > this.pinfo.Take) {
                this.bottomPager.show();
            }
        }
    },
    generateStreamDom: function (results) {
        var res = results;
        if (typeof (results) == "string") {
            res = eval("(" + results + ")");
        }
        this.pinfo = res.PagerInfo;
        if (this.topPager != null) {
            this.topPager.setPagerInfo(this.pinfo);
            if (this.pinfo.Length > this.pinfo.Take) {
                this.topPager.show();
            }
        } if (this.bottomPager != null) {
            this.bottomPager.setPagerInfo(this.pinfo);
            if (this.pinfo.Length > this.pinfo.Take) {
                this.bottomPager.show();
            }
        }
        this.json = res.Data;
        $(this.controlId).children().remove();
        if (this.mode == "FRIEND_REQUEST") {
            $(this.controlId).append('<div class="friendListHead grad-FFF-EEE">' +
                                            '<div><button id="bSendFriendRequest">Send Friend Request</button>' +
                                            '<span class="title">' + this.title + '</span><span>' + this.pinfo.Length + ' found</span>' +
                                            '</div>' +
                                            '<span>Select: <a href="javascript: ;" id="' + this.id + 'selectAll">All</a>, <a href="javascript: ;" id="' + this.id + 'selectNone">None</a>' +
                                        '</div>' +
                                        '<ul class="atiUserList" id="' + this.id + 'atiStreamList"></ul>');
        } else if (this.mode == "FRIEND_LIST") {
            $(this.controlId).append('<div class="friendListHead grad-FFF-EEE">' +
                                                '<div style="min-height: 20px;">' +
                                                (this.isOwner ?
                                                '<button id="bDefriend">Defriend</button>'
                                                : '') +
                                                '<span>' + this.pinfo.Length + ' friends</span>' +
                                                '</div>' +
                                                '<span>Select: <a href="javascript: ;" id="' + this.id + 'selectAll">All</a>, <a href="javascript: ;" id="' + this.id + 'selectNone">None</a>' +
                                            '</div>' +
                                            '<ul class="atiUserList" id="' + this.id + 'atiStreamList"></ul>');
        } else if (this.mode == "GROUP_INVITE") {
            $(this.controlId).append('<div class="friendListHead grad-FFF-EEE">' +
            //  '<div><button id="bSendFriendRequest">Send Group Invite</button>' +
                                            '<span class="title">' + this.title + '</span><span>' + this.pinfo.Length + ' found</span>' +
                                            '</div>' +
                                            '<span>Select: <a href="javascript: ;" id="' + this.id + 'selectAll">All</a>, <a href="javascript: ;" id="' + this.id + 'selectNone">None</a>' +
                                        '</div>' +
                                        '<ul class="atiUserList" id="' + this.id + 'atiStreamList"></ul>');
        } else if (this.mode == "GROUP_LIST" || this.mode == "MEMBER_ADMIN" || this.mode == "MEMBERADMIN_ADMIN" || this.mode == "GROUP_JOIN") {
            $(this.controlId).append('<div class="friendListHead grad-FFF-EEE">' +
                                                '<div style="min-height: 20px;">' +
                                                '<span class="title">' + this.title + '</span><span>' + this.pinfo.Length + ' found</span>' +
                                                '</div>' +
                                            '</div>' +
                                            '<ul class="atiUserList" id="' + this.id + 'atiStreamList"></ul>');
        } else if (this.mode == "FRIEND_RESPONSE") {
            $(this.controlId).append('<div class="friendListHead grad-FFF-EEE"><h2>Friend Requests</h2>' +
                                                '<div><button id="bAccept">Accept</button><button id="bReject">Reject</button>' +
                                                '<span class="title">' + this.title + '</span><span>' + this.pinfo.Length + ' requests</span>' +
                                                '</div>' +
                                                '<span>Select: <a href="javascript: ;" id="' + this.id + 'selectAll">All</a>, <a href="javascript: ;" id="' + this.id + 'selectNone">None</a>' +
                                            '</div>' +
                                            '<ul class="atiUserList" id="' + this.id + 'atiStreamList"></ul>');
        } else if (this.mode == "FOLLOWING_LIST") {
            $(this.controlId).append('<div class="friendListHead grad-FFF-EEE">' +
                                                '<div style="padding: 6px;">&nbsp;' +
                                                '<span class="title">' + this.title + '</span><span>Following: ' + this.pinfo.Length + '</span>' +
                                                '</div>' +
                                                '<span>Select: <a href="javascript: ;" id="' + this.id + 'selectAll">All</a>, <a href="javascript: ;" id="' + this.id + 'selectNone">None</a>' +
                                            '</div>' +
                                            '<ul class="atiUserList" id="' + this.id + 'atiStreamList"></ul>');
        }
        this.list = $('#' + this.id + 'atiStreamList');
        if (this.json.length <= 0) {
            $('#' + this.id + 'atiStreamList').append('<li><div class="noResults">No results were found.</div></li>');
        }
        for (var i = 0; i < this.json.length; i++) {
            this.generateStreamItem(this.json[i], false);
        }
        $('#bAccept, #bReject').button().click(function (event) {
            alert('TODO:');
            event.stopPropagation();
            return false;
        });
        $('#bGrouLeave').button().click(function (event) {
            alert('TODO:');
            event.stopPropagation();
            return false;
        });
        $('#bDefriend').button().click(function (event) {
            alert('TODO:');
            event.stopPropagation();
            return false;
        });
        $('#bSendFriendRequest').button().click(function (event) {
            alert('TODO:');
            event.stopPropagation();
            return false;
        });
        var cid = this.id;
        $('#' + this.id + 'selectAll').click(function () {
            $('#' + cid + 'atiStreamList input:checkbox').attr("checked", true);
        });
        $('#' + this.id + 'selectNone').click(function () {
            $('#' + cid + 'atiStreamList input:checkbox').attr("checked", false);
        });

    }
};
/////////////////////////////////////////////////////
//          END TimeSpan Script
/////////////////////////////////////////////////////




/////////////////////////////////////////////////////
//         atiDataPager
/////////////////////////////////////////////////////
Aqufit.Page.Controls.atiPagerInfo = function () {
    this.Skip = 0;
    this.Take = 0;
    this.Length = 0;
};

Aqufit.Page.Controls.atiDataPager = function (id, control, bPev, bNext) {
    this.id = id;
    this.$pControl = $('#' + control);
    this.info = new Aqufit.Page.Controls.atiPagerInfo();
    this.onPageBack = null;
    this.onPageForward = null;
    var that = this;
    this.$bPev = $('#' + bPev).button({ disabled: true, icons: { primary: 'ui-icon-seek-prev'} }).click(function (event) {
        if (that.onPageBack) {
            var skip = that.info.Skip - that.info.Take;
            if (skip < 0) {
                skip = 0;
            }
            that.onPageBack(skip, that.info.Take, that.info.Length);
        }
        event.stopPropagation();
        return false;
    });
    this.$bNext = $('#' + bNext).button({ disabled: true, icons: { primary: 'ui-icon-seek-next'} }).click(function (event) {
        if (that.onPageForward) {
            var skip = that.info.Skip + that.info.Take;
            that.onPageForward(skip, that.info.Take, that.info.Length);
        }
        event.stopPropagation();
        return false;
    });
};

Aqufit.Page.Controls.atiDataPager.prototype = {
    removePagerControl: function () {
        this.$pControl.empty();
        return this.$pControl;
    },
    hide: function () {
        this.$pControl.hide();
    },
    show: function () {
        this.$pControl.show();
    },
    setPagerInfo: function (pinfo) {
        this.info = pinfo;
        var skip = this.info.Skip;
        var take = skip + this.info.Take;

        this.$pControl.find('span.pageStart').html(skip);
        this.$pControl.find('span.pageEnd').html(this.info.Length < take ? this.info.Length : take);
        if (this.info.Length > take) {
            this.$bNext.button("option", "disabled", false);
        } else {
            this.$bNext.button("option", "disabled", true);
        }        
        if (skip > 0) {
            this.$bPev.button("option", "disabled", false);
        } else {
            this.$bPev.button("option", "disabled", true);
        }        
        this.$pControl.find('span.totalLen').html(this.info.Length);
    }
};
/////////////////////////////////////////////////////
//         END Data Pager
/////////////////////////////////////////////////////




/////////////////////////////////////////////////////
//         atiSlimForm
/////////////////////////////////////////////////////
Aqufit.Page.Controls.atiSlimForm = function (id, postal, hAddressId, hLatId, hLngId) {
    this.id = id;
    this.$postal = $('#' + postal);
    this.$lat = $('#' + hLatId);
    this.$lng = $('#' + hLngId);
    this.$address = $('#' + hAddressId);
    this.geocoder = null;
    this.locationLoaded = false;
}

Aqufit.Page.Controls.atiSlimForm.prototype = {
    storeLatLngHome: function () {
        var that = this;
        this.geocoder.geocode({ 'address': that.$postal.val() }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                that.$address.val(results[0].formatted_address);
                that.$lat.val(results[0].geometry.location.lat());
                that.$lng.val(results[0].geometry.location.lng());
            }
        });
    },
    gmapLoad: function () {
        this.geocoder = new google.maps.Geocoder();
    }
};
///////////////////////////////////////////////////////
//          END atiSlimForm
///////////////////////////////////////////////////////



    Aqufit.Page.Controls.ATI_FeaturedStreamItem = function (id, control, title) {
        this.id = id;
        this.controlId = '#' + control;
        this.json = null;
        this.list = null;
        this.title = title;

        $(this.controlId).css('width', '100%').append('<ul class="atiStreamList" id="atiStreamList' + this.id + '"></ul>');
        this.list = $('#atiStreamList' + this.id);
    };

    Aqufit.Page.Controls.ATI_FeaturedStreamItem.prototype = {
        addItem: function (json, heading) {
            // alert(heading);
            this.json = eval("(" + json + ")");
            var sd = this.json[0];
            this.title = heading;
            this.generateStreamItem(sd, false);
        },
        generateStreamItem: function (sd, prepend) {
            var numRecipe = 0;
            var followers = 0;
            var following = 0;
            var linkTitle = sd["Title"].replace(/ /g, '_');
            var shareTitle = Aqufit.WorkoutTypes.toString(sd["WorkoutType"]) + ' - ' + (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT ?
                                (sd["Score"] > 0 ? Aqufit.Utils.round(sd["Score"], 2) : sd["Max"] > 0 ? 'Max ' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, sd["Max"], Aqufit.Page.WeightUnits), 2) + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.WeightUnits) : Aqufit.Utils.toDurationString(sd["Duration"]))
                                :
                                '' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_M, sd["Distance"], Aqufit.Page.DistanceUnits), 2) + ' ' + Aqufit.Units.getUnitName(Aqufit.Page.DistanceUnits) + ' time: ' + Aqufit.Utils.toDurationString(sd["Duration"]) + ' / ' +
                                'pace: ' + Aqufit.Utils.toPaceString(sd["Duration"], sd["Distance"], Aqufit.Page.DistanceUnits))
            linkTitle = linkTitle.replace(/\+/g, '');
            linkTitle = escape(linkTitle);
            var recipeUrl = Aqufit.Page.PageBase + 'recipe/' + sd["Id"] + '/';
            var shareUrl = Aqufit.Page.SiteUrl + Aqufit.Page.ProfileUserSettingsId + 'workout/' + sd["Id"] + '/';
            var statsLink = Aqufit.Page.PageBase + sd["UserName"] + "/workout/" + sd["Id"];
            var workoutLink = statsLink;
            var html = '<li id="atiStreamItem' + sd["Id"] + '" class="atiStreamItem grad-fade">' +
                        '<div class="featuredTitle"><span>' + this.title + '</span></div>' +
                        '<div class="atiStreamItemLeft">' +
                            ((sd["StreamType"] == Aqufit.Stream.RECIPE) ?
                            '<a class="title" href="' + recipeUrl + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/recipe.aspx?r=' + sd["Id"] + '" /></a>'
                            :
                            '<a href="' + Aqufit.Page.PageBase + sd["UserName"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/services/images/profile.aspx?us=' + sd["UserSettingId"] + '" /></a>'
                            ) +
                        '</div>' +
                        '<div class="atiStreamItemRight">' +
                            '<a class="username" href="' + Aqufit.Page.PageBase + sd["UserName"] + '">' + sd["UserName"] + '</a>' +
                            ((sd["StreamType"] == Aqufit.Stream.WORKOUT) ?   // is this a workout
                                '<a class="atiWorkoutLink" href="' + workoutLink + '" title="' + Aqufit.Utils.getDataSrcName(sd["DataSrc"]) + '">' + Aqufit.WorkoutTypes.toString(sd["WorkoutType"]) + '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/src' + sd["DataSrc"] + '.png" /></a>&nbsp;' +
                                '<span class="dist grad-FFF-EEE ui-corner-all">&nbsp;<em style="font-weight: bold;">' +
                                (sd["WorkoutType"] == Aqufit.WorkoutTypes.CROSSFIT ?
                                (sd["Score"] > 0 ? Aqufit.Utils.round(sd["Score"], 2) : sd["Max"] > 0 ? 'Max ' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_KG, sd["Max"], Aqufit.Page.WeightUnits), 2) + '&nbsp;' + Aqufit.Units.getUnitName(Aqufit.Page.WeightUnits) : Aqufit.Utils.toDurationString(sd["Duration"]))
                                :
                                '<em style="font-weight: bold;">' + Aqufit.Utils.round(Aqufit.Units.convert(Aqufit.Units.UNIT_M, sd["Distance"], Aqufit.Page.DistanceUnits), 2) + '&nbsp;' + Aqufit.Units.getUnitName(Aqufit.Page.DistanceUnits) + '</em>&nbsp;/&nbsp;<em style="font-weight: bold;">' + Aqufit.Utils.toDurationString(sd["Duration"]) + '</em></span>' +
                                '<span class="pace" id="atiPace' + sd["id"] + '">' + Aqufit.Utils.toPaceString(sd["Duration"], sd["Distance"], Aqufit.Page.DistanceUnits) + '&nbsp;pace</span>')
                            : '') + '</em>' + // else was not a workout
                            ((sd["StreamType"] == Aqufit.Stream.RECIPE) ?
                            '<a class="title" href="' + recipeUrl + '">' + sd["Title"] + '</a>'
                            :
                            '<a href="' + workoutLink + '" class="title">' + sd["Title"] + '</a>'
                            ) +
                            '<p>' + ((sd["Description"] != null ? sd["Description"] : '')) + '&nbsp;</p>' +
                            '<ol class="hList streamTools">' +
                                '<li>' +
                                    '<a href="javascript: ;" class="time" title="' + Aqufit.Utils.toLocalTime(sd["DateTicks"]).toString() + '">' + Aqufit.Utils.toTimeAgoString(sd["DateTicks"]) + '</a>' +
                                '</li>' +
                                '<li><span style="line-height: 30px;">&nbsp;</span></li>' +
                                '<li class="shareStream">' +
                                    '<a href=\"' + statsLink + '\" title="Compare Stats"><img src=\"' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iStats_s.png\" /></a>' +
                                    '<span class="toolDiv">|</span>'+                                    
                                    '<a target=\"_blank\" href=\"http://twitter.com/share?url=' + shareUrl + '&related=flexfwd&text=' + shareTitle + '\" title="Share on Twitter"><img src=\"' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iTwitter.png\" /></a>' +
                                    '<span class="toolDiv">|</span>' +                                    
                                    ((sd["StreamType"] != Aqufit.Stream.NOTIFICATION && sd["StreamType"] != Aqufit.Stream.RECIPE &&
                                    Aqufit.Page.Permission <= Aqufit.Permission.FRIEND) ?    // if right perms and NOT notification
                                    '<a href="javascript: ;" id="atiLinkAddComment' + sd["Id"] + '"><img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iComment.png" align="absmiddle" />&nbsp;comment</a>'
                                    :
                                    '<span style="padding-right: 9px; padding-left: 9px;">' + sd["Comments"].length + ' comments</span>'
                                    ) +
                                '</li>' +
                                ((Aqufit.Page.UserId == sd["UserKey"]) ? // if this is the owner
                                '<li class="deleteStream"><a id="aDelStream' + sd["Id"] + '" style="display:none; cursor: pointer;" title=\"delete\">[X]</a></li>'
                                :
                                '') +
                            '</ol>' +
                            '<ul class="atiCommentBox" id="atiCommentList' + this.id + '_' + sd["Id"] + '">' +
                            '</ul>' +
                        '</div></li>';
            var that = this;
            if (prepend) {
                this.list.prepend(html);
                $("#atiStreamItem" + sd["Id"]).hide();
                $("#atiStreamItem" + sd["Id"]).show("slow");
            } else {
                this.list.append(html);
            }
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        },
        generateStreamDom: function (json) {
            this.json = eval("(" + json + ")");
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false);
            }
        }
    };

    
    Aqufit.Page.Controls.atiMessage = function (id, control) { // New object constructor
        this.id = id;
        this.controlId = '#'+control;
        this.json = null;
    };
    
    Aqufit.Page.Controls.atiMessage.prototype = {              
        generateMessageDom: function(json) {
            var that = this;
            this.json = eval("(" + json + ")");
            var message = this.json[0];
            // put in the title and some controls
            var $control = $(this.controlId);
            $control.append(
                                        '<h2>'+message.Subject+'</h2>'+
                                        '<a href="'+Aqufit.Page.PageBase + 'Profile/Inbox">&lt; Back to Inbox</a>'
                                        ).append('<ul id="atiMessageView'+this.id+'" class="atiMessageView"></ul>');
            this.list = $('#atiMessageView'+this.id);            
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false, false)
            }
            
            $control.append('<div style="padding-bottom: 8px;"><textarea id="replyTxt'+this.id+'"></textarea><br /><button style="position: relative; left: 470px;" id="messageReply'+this.id+'">Reply</button></div>');
            var cid = this.id;
            $('#messageReply'+cid).button({
                icons: {
                    primary: 'ui-icon-arrowreturnthick-1-w'
                }
            }).click(function(event){
                var $button = $(this);
                $button.button('disable');
                event.stopPropagation();
                Affine.WebService.StreamService.saveReply(Aqufit.Page.UserSettingsId, message["Id"], $('#replyTxt' + cid).val(), function (json) {
                    var message = eval("(" + json + ")");
                    $('#replyTxt'+that.id).val("");
                    that.generateStreamItem( message, false, true );     
                    $button.button('enable');
                }, WebServiceFailedCallback);       
                return false;         
            });
        },
        generateStreamItem: function (sd, prepend, slow) {           
            var html = '<li id="atiStreamItem' + sd["Id"] + '">' +                        
                        '<div class="atiStreamItemLeft atiMessageItemLeft">' +
                            '<ul class="hList">' +
                                '<li><a href="' + Aqufit.Page.PageBase + sd["UserName"] + '"><img src="' + Aqufit.Page.PageBase + "DesktopModules/ATI_Base/services/images/profile.aspx?us=" + sd["UserSettingId"] + '" /></a></li>' +
                            '</ul>' +
                        '</div>' +
                        '<div class="atiStreamItemRight atiMessageItemRight">' +                            
                            '<a class="username" href="' + Aqufit.Page.PageBase + sd["UserName"] + '">' + sd["UserName"] + '</a>&nbsp;-&nbsp;<a href="javascript: ;" class="time">' + sd["Date"] + '</a>' +
                            '<p>' + ((sd["Text"] != null ? sd["Text"] : '')) + '&nbsp;</p>' +
                        '</div>' +                
                        '</li>';
            var $control = $(this.controlId);
            if (prepend) {
                this.list.prepend(html);
                $("#atiStreamItem" + sd["Id"]).hide().show("slow");
            } else {
                $('#atiMessageView'+this.id).append(html);
                if( slow ){
                    $("#atiStreamItem" + sd["Id"]).hide().show("slow");
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////           
           
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        } 
    };
    
    
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    
    Aqufit.Page.Controls.atiMessageList = function( id, control, mode ){
        this.id = id;
        this.controlId = '#'+control;
        this.json = null;
        this.start = 0;
        this.take = 20;  
        this.mode = mode;
        this.deleteCallback = null;
    }
    
    
    
    Aqufit.Page.Controls.atiMessageList.prototype = {            
        generateStreamItem: function (sd, prepend) {      
            var html = '<li id="atiStreamItem' + sd["Id"] + '">' +
                        '<div class="messageWrapper' + (sd["Unread"] && this.mode != 1 ? ' unread' : '') + '">' +
                        '<div class="atiStreamItemLeft">' +
                            '<ul class="hList">' +                                
                                ((sd["LastUserKey"] == Aqufit.Page.UserId && this.mode != 1) ?
                                '<li class="checkboxCell">'+                                
                                '<img src="' + Aqufit.Page.PageBase + 'DesktopModules/ATI_Base/resources/images/iReply.png" /><input type="checkbox" id="atiMessageCheck' + sd["Id"] + '" />'
                                :
                                '<li class="checkboxCell" style="margin-left: 16px;">' +
                                '<input type="checkbox" id="atiMessageCheck' + sd["Id"] + '" />'
                                ) +
                                '</li>' +                                
                                '<li>'+
                                    ((sd["LastUserKey"] != Aqufit.Page.UserId) ?
                                    '<a href="' + Aqufit.Page.PageBase + sd["LastUserName"] + '"><img src="' + Aqufit.Page.PageBase + "DesktopModules/ATI_Base/services/images/profile.aspx?us=" + sd["LastUserKey"] + '&p='+Aqufit.Page.PortalId+'" /></a>'
                                    :
                                    '<a href="' + Aqufit.Page.PageBase + sd["SecondUserName"] + '"><img src="' + Aqufit.Page.PageBase + "DesktopModules/ATI_Base/services/images/profile.aspx?us=" + sd["SecondUserKey"] + '&p='+Aqufit.Page.PortalId+'" /></a>'
                                    )+
                                '</li>' +
                            '</ul>' +
                        '</div>' +
                        '<div class="atiStreamItemRight">' +
                            '<ul class="hList">' +
                                ((sd["LastUserKey"] != Aqufit.Page.UserId) ?
                                '<li class="messageSender">' +
                                    '<a class="username" href="' + Aqufit.Page.PageBase + sd["LastUserName"] + '">' + sd["LastUserName"] + '</a><br />' +
                                    '<a href="javascript: ;" class="time">' + sd["LastDateTime"] + '</a>' +
                                '</li>' +
                                '<li class="messageSummary">' +
                                    '<span class="title"><em style="font-weight: bold;">' + sd["Subject"] + '</em>' +
                                    '<p>' + ((sd["LastText"] != null ? sd["LastText"] : '')) + '&nbsp;</p>' +
                                '</li>' 
                                :
                                '<li class="messageSender">' +
                                    '<a class="username" href="' + Aqufit.Page.PageBase + sd["SecondUserName"] + '">' + sd["SecondUserName"] + '</a><br />' +
                                    '<a href="javascript: ;" class="time">' + sd["SecondDateTime"] + '</a>' +
                                '</li>' +
                                '<li class="messageSummary">' +
                                    '<span class="title"><em style="font-weight: bold;">' + sd["Subject"] + '</em>' +
                                    '<p>' + ((sd["SecondText"] != null ? sd["SecondText"] : '')) + '&nbsp;</p>' +
                                '</li>' 
                                ) +
                                '<li class="deleteStream">' +
                                    '<button id="bDelStream' + sd["Id"] + '">Delete</a>' +       // TODO: delete click (toggle)
                                '</li>'+
                            '</ul>' +
                        '</div>' +
                        '</div>'+
                        '</li>';
            if (prepend) {
                this.list.prepend(html);
                $("#atiStreamItem" + sd["Id"]).hide();
                $("#atiStreamItem" + sd["Id"]).show("slow");
            } else {
                this.list.append(html);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            // EVENTS Now attach the events to needed elements.
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            $('#atiStreamItem' + sd["Id"]).click(function () {
                //$(this).fadeOut("slow");
                self.location.href = self.location.href + "?m="+sd["Id"];
            });
            var that = this;
            $('#bDelStream' + sd["Id"]).click(function (event) {
                event.stopPropagation();
                // TODO: nice looking confirm
                if( that.deleteCallback && confirm("Are you sure you want to delete this message?") ){
                    that.deleteCallback(sd["Id"]);
                    $('#atiStreamItem'+sd["Id"]).hide("slow").children().remove();
                }else{
                    alert('deleteCallback not set.');  
                }              
                return false;
            }).button({
                icons: {
                    primary: 'ui-icon-trash'
                },
                text: false
            });                    
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        },             
        getMessageList: function(){
            var that = this;
            $(this.controlId).children().remove();
            Affine.WebService.StreamService.getMessageListData(Aqufit.Page.UserSettingsId, this.start, this.take, this.mode, function(json){that.generateStreamDom(json);}, WebServiceFailedCallback);
        },   
        generateStreamDom: function (json) {   
            if( this.start < 0 ){
                this.start = 0;
            }  
            this.json = eval("(" + json + ")");
            var that = this;
            var atEnd = false;
            if( this.json.length < this.take ){
                atEnd = true;
            }     
            $(this.controlId).append('<div class="messageListHead grad-FFF-EEE">'+
                                                    '<div><button id="bAllMarkUnread'+this.id+'">Mark as Unread</button><button id="bAllDelete'+this.id+'">Delete</button><div style="float: right;"><button id="bMessageBack'+this.id+'">Newer</button> '+this.start+' to '+(this.start + this.json.length)+' <button id="bMessageNext'+this.id+'">Older</button></div></div>'+
                                                    '<span>Select: <a href="javascript: ;" id="selectAll'+this.id+'">All</a>, <a href="javascript: ;" id="selectUnread'+this.id+'">Unread</a>, <a href="javascript: ;" id="selectNone'+this.id+'">None</a>'+                                                    
                                                '</div>'+
                                                '<ul class="atiStreamList" id="atiStreamList'+this.id+'"></ul>'+
                                                '<div class="messageListHead grad-FFF-EEE">'+
                                                    '<div><button id="bAllMarkUnread2'+this.id+'">Mark as Unread</button><button id="bAllDelete2'+this.id+'">Delete</button><div style="float: right;"><button id="bMessageBack2'+this.id+'">Newer</button> '+this.start+' to '+(this.start + this.json.length)+' <button id="bMessageNext2'+this.id+'">Older</button></div></div>'+                                                                                                    
                                                '</div>');
                                              
            this.list = $('#atiStreamList'+this.id);            
            for (var i = 0; i < this.json.length; i++) {
                this.generateStreamItem(this.json[i], false);
            }
            $('#selectAll'+this.id).click(function(){
                $('#atiStreamList input:checkbox').attr("checked",true);
            });
            $('#selectNone'+this.id).click(function(){
                $('#atiStreamList input:checkbox').attr("checked",false);
            });
            $('#selectUnread'+this.id).click(function(){
                $('.unread input:checkbox').attr("checked",true); 
            });
            $('#bAllMarkUnread'+this.id+', #bAllMarkUnread2'+this.id).button({
                icons: {
                    primary: 'ui-icon-mail-open'
                }
            }).click(function(event){
                alert('TODO: ');
                event.stopPropagation();
                return false;
            }).next().button({
                icons: {
                    primary: 'ui-icon-trash'
                }
            }).click(function(event){
                alert('TODO:');
                event.stopPropagation();
                return false;
            });
            $('#bMessageBack'+this.id+', #bMessageBack2'+this.id).button({
                icons: {
                    primary: 'ui-icon-seek-prev'
                }
            }).click(function(event){
                that.start = that.start-that.take;             
                that.getMessageList();               
                event.stopPropagation();
                return false;
            });
            if( this.start <= 0 ){
                $('#bMessageBack'+this.id+', #bMessageBack2'+this.id).button('disable');
            }
            
            $('#bMessageNext'+this.id+', #bMessageNext2'+this.id).button({
                icons: {
                    primary: 'ui-icon-seek-next'
                }
            }).click(function(event){
                that.start = that.start+that.take;               
                that.getMessageList();               
                event.stopPropagation();
                return false;
            });
            if(atEnd){
                $('#bMessageNext'+this.id+', #bMessageNext2'+this.id).button('disable');
            }
        }
    };





