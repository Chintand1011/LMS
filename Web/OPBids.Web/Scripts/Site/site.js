
var Site = {
    RequestCounter: [],
    UserId: null,
    DepartmentId: null,
    DepartmentCode: null,
    FullName: null,
    EmailAddress: null,
    DepartmentName: null,
    UserName: null,
    PageItemCount: null,
    DateFormat: "dd-MMM-yyyy",
    Initialize: function () {
        $('.fa.fa-star').unbind("click");
        $('.fa.fa-star').click(function (e) {
            e.preventDefault();
            $(this).toggleClass('checked');

            var _id = $(this).data("project-id");
            var _action = CONST.record_status.activate;
            if (!$(this).hasClass("checked")) { _action = CONST.record_status.delete; };
            var _monitor = {
                'project_request_id': _id,
                'action': _action
            };
            var _filter = JSON.stringify({
                'sub_menu_id': localStorage.getItem("sub-menu-id"),
                'txn': CONST.transaction_type.monitorProject,
                'monitoredProject': _monitor
            });
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", function (result) {
                if (result.status.code === 0) { }
            });
        });

    },
    DrawWaterMark:function(cntr, val){
        var canvas = document.createElement("canvas");
        var ctx = canvas.getContext("2d");
        ctx.font = "700 20px Arial";
        ctx.translate(canvas.width/2, canvas.height/2);
        ctx.rotate(-(Math.PI / 15));
        ctx.textAlign = "center";
        ctx.strokeStyle = "#d8d8d8";
        ctx.fillStyle = "#d8d8d8";
        ctx.fillText(val.toString().toUpperCase(), 0, -40);
        var img = canvas.toDataURL("image/png");
        $(cntr).css({"background-image": ["url(", img, ")"].join('')});
    },
    DrawConfidential: function (cntr, val) {
        if (val == null || val == undefined) { val = "CONFIDENTIAL"; };
        var canvas = document.createElement("canvas");
        var ctx = canvas.getContext("2d");
        ctx.font = "750 20px Arial";
        ctx.translate(canvas.width / 2, canvas.height / 2);
        ctx.rotate(-(20* Math.PI / 180));
        ctx.textAlign = "center";
        ctx.strokeStyle = "#d8d8d8";
        ctx.fillStyle = "#d8d8d8";
        ctx.fillText(val.toString().toUpperCase(), 0, -40);
        var img = canvas.toDataURL("image/png");
        $(cntr).css({ "background-image": ["url(", img, ")"].join('') });
    },
    DrawConfidential2: function (cntr, val) {
        if (val == null || val == undefined) { val = "CONFIDENTIAL"; };
        var canvas = document.createElement("canvas");
        var ctx = canvas.getContext("2d");
        ctx.font = "750 20px Arial";
        ctx.translate(canvas.width / 2, canvas.height / 2);
        ctx.rotate(-(20 * Math.PI / 180));
        ctx.textAlign = "center";
        ctx.strokeStyle = "#d8d8d8";
        ctx.fillStyle = "#d8d8d8";
        ctx.fillText(val.toString().toUpperCase(), 0, -40);
        var img = canvas.toDataURL("image/png");
        $(cntr).css({ "background-image": ["url(", img, ")"].join('') });
    },
    Print: function (pUrl)
    {
                var objFra = document.createElement('iframe');   // Create an IFrame.
                objFra.style.visibility = "hidden";    // Hide the frame.
                objFra.src = pUrl;                      // Set source.
                document.body.appendChild(objFra);  // Add the frame to the web page.
                objFra.contentWindow.focus();       // Set focus.
                objFra.contentWindow.print();      // P                }
    },
    ScanBarcodeFromFile: function(srcCtl){
        var img = document.createElement("img");
        img.src = srcCtl
        $(img).attr("width", [$(window).width(), "px"].join(''));
        $(img).attr("height", [$(window).height(), "px"].join(''));
        $("ean-13_,ean-8_,code-39_,code-93_,code-2of5_standard,code-2of5_interleaved,codabar_,code-128_".split(',')).each(function () {
            var itm = this.split('_');
            javascriptBarcodeReader(img, { barcode: itm[0], type: itm[1] }).then(code => {
                alert(code);
            });
        });
    },
    DropDownRefresh: function (ctl) {
        $(ctl).selectpicker('val', $(ctl).val());
        $(ctl).selectpicker('refresh');
        $(ctl).find("option").each(function () {
            var curVal = $(this).html();
            var curTitle = $(this).attr("desc");
            $(ctl).next().next().find("li").each(function () {
                if ($(this).find(".text").html() == curVal) {
                    $(this).find(".text").parent().parent().attr("title", curTitle);
                }
            });
        });
        Site.GenerateToolTips($(ctl).next().next().find("li"));
    },
    DropDownCommonSettings: function (ctl) {
        $(ctl).selectpicker('refresh');
        $(ctl).selectpicker('toggle');
        $(ctl).find("option").each(function () {
            var curVal = $(this).html();
            var curTitle = $(this).attr("desc");
            $(ctl).next().next().find("li").each(function () {
                if ($(this).find(".text").html() == curVal) {
                    $(this).find(".text").parent().parent().attr("title", curTitle);
                }
            });
        });
        Site.GenerateToolTips($(ctl).next().next().find("li"));
        $(ctl).selectpicker('toggle');
    },
    GenerateToolTips: function (ctlGrps) {
        $(ctlGrps).tooltipster(
            {
                position: 'left',
                animation: 'fade',
                delay: 200,
                theme: 'tooltipster-default',
                touchDevices: true,
                onlyOne: true,
                timer: 5000,
                autoClose: true,
                trigger: 'hover'
            });
    },
    Date: {
        AddDay: function (val) {
            var curDate = new Date();
            return $.datepicker.formatDate('M dd yy', (new Date(curDate.getFullYear(), curDate.getMonth(), curDate.getDate() + val)));
        },
        AddMonth: function (val) {
            var curDate = new Date();
            return $.datepicker.formatDate('M dd yy', (new Date(curDate.getFullYear(), curDate.getMonth() + val, curDate.getDate())));
        },
        AddYear: function (val) {
            var curDate = new Date();
            return $.datepicker.formatDate('M dd yy', (new Date(curDate.getFullYear() + val, curDate.getMonth(), curDate.getDate())));
        },
    },
    AddNotification: function (pData, postEvt) {
        Site.PostData("/Shared/MaintainUserNotification", postEvt, pData, null);
    },
    GetParameterByName: function (name) {
        var url = window.location.href.toLowerCase();
        name = name.replace(/[\[\]]/g, "\\$&").toLowerCase();
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    ShortVal: function (val, maxVal) {
        var val = $.trim(val);
        if (val != null && val.length >= maxVal) {
            return [val.substring(0, maxVal - 3), '...'].join('');
        }
        return val;
    },
    ParseInt: function (val) {
        if (isNaN(val) == true){
            return 0;
        }
        return parseInt(val);
    },
    FillPagingValues: function (counter, isPageReset) {
        if (isNaN(counter) == false) {
            var curVal = (isPageReset == 1 ? 0 : $(".paging").val());
            if (curVal == null) {
                curVal = 0;
            }
            $(".paging option").remove();
            for (var i = 0; i < counter; i++) {
                $(".paging").append(["<option value='", i, "'>", i + 1, "</option>"].join(''));
            }
            $(".pageTotal").html(counter);
            if (curVal == null) {
                $(".paging").val($(".paging option:first-child").attr("value"));
            }
            else {
                $(".paging").val(curVal);
            }
            $(".paging").unbind("change");
            $(".paging").change(function () {
                $(".toolbar-search, .ion-search, #btnSearch").click();
            });

}
    },
    FixDateString: function (val) {
        if (val == null || val == undefined) {
            val = "";
        }
        val = val.replace(/  /g, ' ');
        val = [$.datepicker.formatDate("dd-M-yy", new Date(val.substring(0, val.lastIndexOf(" ")))), val.substring(val.lastIndexOf(" "))].join('');
        return val;
    },
    DaysPast: function (val) {
        if (val == null || val == undefined) {
            val = "";
        }
        val = val.replace(/  /g, ' ');
        val = [val.substring(0, val.length - 2), " ", val.substring(val.length - 2)].join('') 
        var startDay = new Date(val);
        var endDay = new Date();
        startDay = new Date(startDay.getFullYear(), startDay.getMonth() + 1, startDay.getDate());
        endDay = new Date(endDay.getFullYear(), endDay.getMonth() + 1, endDay.getDate());
        var millisBetween = endDay.getTime() - startDay.getTime();
        var days = millisBetween / (1000 * 60 * 60 * 24);
        return (Math.floor(days)) + 1;
    },
    PopulateSequence: function() {
        var counter = $("select.paging").val() * Site.PageItemCount;
        $("#grdLst tbody tr td .incrementalNum").each(function () {
            counter++;
            $(this).html(counter);
            $(this).parent().css({ "white-space": "nowrap" });
        });
    },
    GetTime: function(val){
        val = val.toString().split(' ');
        return [val[0], " ", val[1]].join('')
    },
    FormatTime: function (val) {
        val = val.toString().split(':');
        return [val[0], " : ", val[1].substring(0, val[1].length - 2), " ", val[1].slice(2)].join('')
    },
    GenerateSearch: function (rbtnWeek, rbtnMonth, rbtn60, submittedFrom, submittedTo) {        
        $(rbtnWeek).click(function () {            
            var curDay = new Date().getDay();
            var endDay = 6 - curDay;
            $(submittedFrom).val(Site.Date.AddDay(-curDay));            
            $(submittedTo).val(Site.Date.AddDay(endDay));
        });
        $(rbtnMonth).click(function () {
            var _now = new Date();
            var curDate = _now.getDate();
            var nextMonth = new Date(_now.getFullYear(), _now.getMonth() + 1, 1);
            var _datediff = Math.abs(nextMonth - _now);
            var _diff = parseInt(_datediff / (24 * 60 * 60 * 1000), 10);
            $(submittedFrom).val(Site.Date.AddDay(-curDate + 1));
            $(submittedTo).val(Site.Date.AddDay(_diff));
        });
        $(rbtn60).click(function () {
            $(submittedFrom).val(Site.Date.AddDay(-60));
            $(submittedTo).val(Site.Date.AddDay(0));
        });
    },
    GenerateControls: function (cntr) {
        $(cntr).find("*[maxlength]").each(function () {
            $(this).parent().append(["<label class='lengthCtl'><span class='remainingLength'>0</span>/", $(this).attr("maxlength"), "</label>"].join(''));
            $(this).keyup(function () {
                $(this).parent().find(".remainingLength").html($(this).val().length);
            });
        });
        $(cntr).find("input[datetime]").datepicker({ format: 'dd-M-yyyy', clearBtn: true, enableOnReadonly: true, autoclose: true }).prop("readonly", true);
        $(cntr).find("input[timepicker]").prop("readonly", true).wickedpicker({
            minuteStep: 1,
            twentyFour: false, 
            showSeconds: false,
            defaultTime: 'modal'
        });
        $(cntr).find('*[alphanumeric]').keypress(function (e) {
            var regex = new RegExp("^[a-zA-Z0-9 ]*$");
            var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            if (regex.test(str)) {
                return true;
            }
            e.preventDefault();
            return false;
        });
        $(cntr).find('*[personName]').keypress(function (e) {
            var regex = new RegExp("^[\u00F1a-zA-Z\ \.\-]+$");
            var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            if (regex.test(str)) {
                return true;
            }
            e.preventDefault();
            return false;
        });
        $(cntr).find('*[number]').keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                return;
            }
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        $(cntr).find('*[money]').blur(function (e) {
            $(this).val(function () {
                var parts = $(this).val().toString().split(".");
                parts[0] = parts[0].replace(",", "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                return parts.join(".");
            });         
        });

        $(cntr).find('*[money]').each(
            function () {
                var el = $(this);
                var p = el.parent();
                if (p.hasClass('input-icon') == false) {
                    el.wrap("<div class='input-icon'></div>").after('<i>₱</i>');
                }
               
            }
        );
        
        $(cntr).find('select.selectpicker').selectpicker();

        
    },
    LoadContent: function (cntr, _Path, dynamicCode, isAlwaysLoad, counter) {
        if (isAlwaysLoad == true || isAlwaysLoad == false && counter == 0) {
            $(cntr).load(_Path, function (responseTxt, statusTxt, xhr) {
                if (statusTxt == "success") {
                    if (dynamicCode != "") {
                        dynamicCode();
                    }
                }
                if (statusTxt == "error") {
                    //alert(["Error: ", xhr.status, ": ", xhr.statusText].join(''));
                }
            });
        }
    },
    Dialogs: {
        Alert: function (msg, okLbl, callBackParam) {
            $(".dialogBoxAlert").remove();
            var dialogVar = ['<div class="dialogBoxAlert"><div class="dialogContainer"><div class="dialogHeader">',
                '<button type="button" class="close glyphicon glyphicon-remove-circle" data-dismiss="modal" aria-hidden="true" onclick="javascript:',
                '$(\'.dialogFooter #positiveDialogButton\').click();"></button></div><div class="dialogBody" style="text-align:center !important;">', msg,
                            '</div><p class="dialogFooter"><button type="button" id="positiveDialogButton" ',
                            'class="ui-button ui-corner-all ui-widget positiveBlue"><span style="position:relative;left:-5px;">&nbsp;',
                            (okLbl == null || okLbl == undefined ? "Got It!" : okLbl), '</button></p></div></div>'].join('');
            $("body").append(dialogVar);
            $(".dialogBoxAlert #positiveDialogButton").click(function () {
                $(".dialogBoxAlert").remove();
                if (callBackParam != null && callBackParam != undefined) {
                    callBackParam();
                }
            });
        },
        Input: function (hdr, msg, positiveButton, negativeButton, positiveBtnCallbackParam, negativeBtnCallbackParam, preEvent) {
            $(".dialogBox").remove();
            if (hdr == "" || hdr == null) {
                hdr = document.title;
            }
            var dialogVar = ['<div class="dialogBox"><div class="dialogContainer"><div class="dialogHeader">', hdr, 
                            '<button type="button" class="close glyphicon glyphicon-remove-circle" data-dismiss="modal" aria-hidden="true" onclick="javascript:',
                            '$(\'.dialogFooter #negativeDialogButton\').click();"></button></div>',
                            '<div class="dialogBody" style="text-align:left;">', msg,
                            '<br /><textarea class="form-control" id="dialogInput" maxlength="200" style="width:100%;" rows="3">',
                            '</textarea></div><p class="dialogFooter"><span id="noteId" style="float:left;line-height:inherit;padding-top:10px;"></span><button type="button" id="negativeDialogButton" class="ui-button ui-corner-all ui-widget">',
                            (negativeButton == null || negativeButton == undefined ? "Cancel" : negativeButton), '</button><button type="button" id="positiveDialogButton" class="ui-button ui-corner-all ui-widget">',
                            (positiveButton == null || positiveButton == undefined ? "Ok" : positiveButton), '</button></p></div></div>'].join('');
            $("body").append(dialogVar);
            $("#positiveDialogButton").click(function () {
                if (positiveBtnCallbackParam != null && positiveBtnCallbackParam != undefined) {
                    if ($("#dialogInput").val() != "") {
                        positiveBtnCallbackParam();
                        $(".dialogBox").remove();
                    }
                }
                else {
                    $(".dialogBox").remove();
                }
            });
            $("#negativeDialogButton").click(function () {
                if (negativeBtnCallbackParam != null && negativeBtnCallbackParam != undefined) {
                    negativeBtnCallbackParam();
                    $(".dialogBox").remove();
                }
                else {
                    $(".dialogBox").remove();
                }
            });
            if (preEvent != null && preEvent != undefined && preEvent != "") {
                preEvent();
            }
            if ($(":focus").length > 0) {
                $(":focus").blur();
            }
            $("#dialogInput").focus();
        },
        InputGeneric: function (hdr, cntnt, positiveButton, negativeButton, positiveBtnCallbackParam, negativeBtnCallbackParam, preEvent) {
            $(".dialogBox").remove();
            if (hdr == "" || hdr == null) {
                hdr = document.title;
            }
            var dialogVar = ['<div class="dialogBox"><div class="dialogContainer"><div class="dialogHeader">', hdr,
                            '<button type="button" class="close glyphicon glyphicon-remove-circle" data-dismiss="modal" aria-hidden="true" onclick="javascript:',
                            '$(\'.dialogFooter #negativeDialogButton\').click();"></button></div>',
                            '<div class="dialogBody" style="text-align:left;">', cntnt,
                            '</div><p class="dialogFooter"><span id="noteId" style="float:left;line-height:inherit;padding-top:10px;"></span><button type="button" id="negativeDialogButton" class="ui-button ui-corner-all ui-widget">',
                            (negativeButton == null || negativeButton == undefined ? "Cancel" : negativeButton), '</button><button type="button" id="positiveDialogButton" class="ui-button ui-corner-all ui-widget">',
                            (positiveButton == null || positiveButton == undefined ? "Ok" : positiveButton), '</button></p></div></div>'].join('');
            $("body").append(dialogVar);
            $("#positiveDialogButton").click(function () {
                if (positiveBtnCallbackParam != null && positiveBtnCallbackParam != undefined) {
                    if ($("#dialogInput").val() != "") {
                        positiveBtnCallbackParam();
                        $(".dialogBox").remove();
                    }
                }
                else {
                    $(".dialogBox").remove();
                }
            });
            $("#negativeDialogButton").click(function () {
                if (negativeBtnCallbackParam != null && negativeBtnCallbackParam != undefined) {
                    negativeBtnCallbackParam();
                    $(".dialogBox").remove();
                }
                else {
                    $(".dialogBox").remove();
                }
            });
            if (preEvent != null && preEvent != undefined && preEvent != "") {
                preEvent();
            }
            if ($(":focus").length > 0) {
                $(":focus").blur();
            }
            $("#dialogInput").focus();
        },
        Confirm: function (hdr, msg, positiveButton, negativeButton, positiveBtnCallbackParam, negativeBtnCallbackParam) {
            $(".dialogBox").remove();
            if (hdr == "" || hdr == null) {
                hdr = document.title;
            }
            var dialogVar = ['<div class="dialogBox"><div class="dialogContainer"><div class="dialogHeader">', hdr,
                '<button type="button" class="close glyphicon glyphicon-remove-circle" data-dismiss="modal" aria-hidden="true" onclick="javascript:',
                '$(\'.dialogFooter #negativeDialogButton\').click();"></button></div>',
                '<div class="dialogBody">', msg,
                '</div><p class="dialogFooter"><button type="button" id="negativeDialogButton" class="ui-button ui-corner-all ui-widget"><span style="position:relative;left:-5px;">&#x2716;</span>&nbsp;',
                (negativeButton == null || negativeButton == undefined ? "Cancel" : negativeButton),
                '</button><button type="button" id="positiveDialogButton" class="ui-button ui-corner-all ui-widget"><span style="position:relative;left:-5px;">&#x2714;</span>&nbsp;',
                (positiveButton == null || positiveButton == undefined ? "Ok" : positiveButton), '</button></p></div></div>'].join('');
            $("body").append(dialogVar);
            $("#positiveDialogButton").click(function () {
                $(".dialogBox").remove();
                if (positiveBtnCallbackParam != null && positiveBtnCallbackParam != undefined) {
                    positiveBtnCallbackParam();
                }
            });
            $("#negativeDialogButton").click(function () {
                $(".dialogBox").remove();
                if (negativeBtnCallbackParam != null && negativeBtnCallbackParam != undefined) {
                    negativeBtnCallbackParam();
                }
            });
        },
    },
    GetStatusDescription: function (statCode) {
        switch (statCode) {
            case CONST.record_status.delete:
                return "Deleted";
            case CONST.record_status.activate:
                return "Active";
            case CONST.record_status.deactivate:
                return "Inactive";
            case CONST.record_status.cancel:
                return "Cancelled";
            case CONST.record_status.onhand:
                return "On-Hand";
            case CONST.record_status.received:
                return "Received";
            case CONST.record_status.finalized:
                return "Finalized";
            case CONST.record_status.served:
                return "Served";
            case CONST.record_status.draft:
                return "Draft";
            case CONST.record_status.pending:
                return "Pending";
            case CONST.record_status.printed:
                return "Printed";
        }
    },
    ClearAllData: function (container) {
        $([container, " input[type='text']:enabled, ", container, " input[type='password']:enabled, ", container, " textarea:enabled, ", container, " input[type='hidden']:enabled"].join('')).val("");
        $([container, " select:enabled"].join('')).val($([container, " select:enabled option:first"].join('')).val());
        $([container, " input[type='checkbox']:enabled"].join('')).prop('checked', false);
    },
    ValidateEmail: function (val) {
        return val.match(/^[_a-zA-ZáéíñóúüÁÉÍÑÓÚÜ0-9-]+(\.[_a-zA-ZáéíñóúüÁÉÍÑÓÚÜ0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,4})$/);
    },
    ValidatePassword: function (val) {
        return (val.length >= 6 && val.match(/[a-z]/) && val.match(/[A-Z]/) && val.match(/\d+/));
    },
    ValidateRequiredEntries: function (container, customMsg) {
        var controlCols = [];
        $(container).find("*[required]").each(function () {
            if ($(this).is(':visible')) {
                switch ((this.type == undefined || this.type == null ? this.nodeName.toLowerCase() : this.type)) {
                    case "text":
                    case "textarea":
                    case "password":
                        if ($(this).val().trim() == "") {
                            controlCols.push([" - ", $(this).attr("caption"), "<br />"].join(''));
                        }
                        else if ($(this).attr("email") != undefined && $(this).attr("email") != null) {
                            if (!Site.ValidateEmail($(this).val())) {
                                controlCols.push([" - Please enter a valid ", $(this).attr("caption"), "<br />"].join(''));
                            }
                        }
                        else if ($(this).attr("minlength") != undefined && $(this).attr("minlength") != null) {
                            if (parseInt($(this).attr("minlength")) > $(this).val().length) {
                                controlCols.push([" - Please enter at least ", $(this).attr("minlength"),
                                    " characters in length for ", $(this).attr("caption"), "<br />"].join(''));
                            }
                        }
                        break;
                    case "file":
                        if ($(this)[0].files.length <= 0) {
                            controlCols.push([" - ", $(this).attr("caption"), "<br />"].join(''));
                        }
                        break;
                    case "select":
                    case "select-one":
                        if ($(this).find("option").length <= 0 || $(this).find("option:selected").length <= 0 || $(this).val() <= "0") {
                            controlCols.push([" - ", $(this).attr("caption"), "<br />"].join(''));
                        }
                        break;
                }
            }
        });
        $(container).find(".selection-box").each(function (e) {
            if ($("#batch_selection_to > tbody > tr").length === 0) {
                controlCols.push([" - ", $(this).attr("caption"), "<br />"].join(''));
            }
        });

        var msg = controlCols.join('\r');
        if (msg != "") {
            if (customMsg != "" && customMsg != null && customMsg != undefined) {
                Site.Dialogs.Alert([customMsg, "\r<div id='errorContainer' style='text-align:left;padding:20px;'>", msg, "</div>"].join(''), null, null);
            }
            else {
                Site.Dialogs.Alert(["Cannot process. Please fill-in the required entries below:\r<div id='errorContainer' style='text-align:left;padding:20px;'>", msg, "</div>"].join(''), null, null);
            }
            return false;
        }
        return true;
    },
    PostData: function (action, postEvents, pData, curData) {
        $(".waitWindow").css({ "display": "block" });
        Site.RequestCounter.push(1);
        $.ajax({
            async: true,
            type: "POST",
            url: action,
            data: pData,
            accepts: "*",
            success: function (data) {
                $("body").data(curData, data);
                if (postEvents != "" && postEvents != null) {
                    postEvents();
                }
                Site.RequestCounter.pop();
                if (Site.RequestCounter.length <= 0) {
                    $(".waitWindow").css({ "display": "" });
                }
            },
            error: function (a) {
                console.log("fail");
                Site.RequestCounter.pop();
                if (Site.RequestCounter.length <= 0) {
                    $(".waitWindow").css({ "display": "" });
                }
            }
        });
    },
    FindFirst: function (arr, callback) { //array.find not supported in IE
        for (var i = 0; i < arr.length; i++) {
            var match = callback(arr[i]);
            if (match) {
                return arr[i];
                break;
            }
        }
    },
    Resizer: function () {
        $(".sidebar, .body-content, .content-wrapper").css({ "height": "auto", "min-height": "auto" });
        $(".body-content").css({ "overflow": "auto", "height": [($(window).height() - $(".navbar-static-top").height()), "px"].join('') });
        $("#leftMenu").css({ "height": [($(window).height() - $("#logoContainer").height()), "px"].join(''), "overflow-y":"auto" });
    },

    SetActiveMenu: function (menu_id, sub_menu_id) {
        localStorage.setItem("active-menu-id", menu_id);
        if (sub_menu_id == null) {
            sub_menu_id = menu_id;
        }
        localStorage.setItem("sub-menu-id", sub_menu_id);
    },
    TriggerMenu: function (menu_id, sub_menu_id) {
        $(".sidebar-menu a[menu-id='" + menu_id + "']").get(0).click();
    },
    ToDecimalString: function(val) {
        if (isNaN(val) == true) {
            return 0;
        }
        return val.toLocaleString("en-PH", { minimumFractionDigits: 2 })
    },
    ResetTempData: function () {
        window.sessionStorage.removeItem("optDashboard");
        if (window.PROJECT!=null) {
            PROJECT.ResetSearchParam();
        }
    }
    
}
$.fn.extend({
    filedrop: function (options) {
        var defaults = {
            callback: null,
            callerId: null
        }
        options = $.extend(defaults, options)
        return this.each(function () {
            var files = []
            var $this = $(this)
            $this.bind('dragover dragleave', function (event) {
                event.stopPropagation();
                event.preventDefault();
                event.originalEvent.dataTransfer.effectAllowed = 'copy';
                event.originalEvent.dataTransfer.dropEffect = 'copy';
            })
            $this.bind('drop', function (event) {
                event.stopPropagation()
                event.preventDefault()
                options.callerId = ["#", event.currentTarget.id].join('');
                files = event.originalEvent.target.files || event.originalEvent.dataTransfer.files
                var isValid = true;
                if ($(event.currentTarget).attr("ext") != undefined && $(event.currentTarget).attr("ext") != null) {
                    var exts = $(event.currentTarget).attr("ext").toLowerCase().split(',');
                    isValid = false;
                    for (var i = 0 ; i < exts.length; i++) {
                        if (isValid == false) {
                            isValid = exts[i] == files[0].name.substring(files[0].name.lastIndexOf('.') + 1).toLowerCase();
                        }
                    }
                }
                if (isValid == true) {
                    if (options.callback) {
                        options.callback(files[0], options.callerId, files);
                    }
                }
                else {
                    Site.Dialogs.Alert('<b>Error</b><br/><br/><br/>The file you tried to upload is not in the correct format<br/><br/>', null, null);
                }
                return false;
            })
        })
    },
});

var Utilities = {
    CalculateVariance: function (bid_amount, approved_amount) {
        return (((bid_amount / approved_amount) - 1) * 100).toFixed(2);
    },

    GetFileTypeClass: function (filename) {
        if (filename != null) {
            switch (filename.toLowerCase().split('.').pop()) {
                case 'pdf':
                    return 'imgFile-pdf';
                case 'doc':
                case 'docx':
                    return 'imgFile-doc';
                case 'xls':
                case 'xlsx':
                    return 'imgFile-xls';
                case 'jpg':
                case 'jpeg':
                case 'png':
                case 'gif':
                    return 'imgFile-img';
                case 'txt':
                    return 'imgFile-txt';
                default:
                    return 'imgFile';

            }
        }
        else {
            return 'imgFile';
        }
    },

    CreateJSON: function (list) {
        var json_list;
        if (list !== null) {
            var i = 0;
            json_list = '{';
            $.each(list, function (key, value) {
                if (i === 0) {
                    json_list += '"' + key + '":"' + value + '"';
                } else {
                    json_list += "," + '"' + key + '":"' + value + '"';
                }                
                i += 1;          
            });
            json_list += '}';
        }
        return JSON.parse(json_list);
    },

    HandlerResultMessage: function (result, msg, btnmsg, valueDisplay) {
        return function (result) {
            if (result.status.code === 0) {
                _btmmsg = "Got It!";
                if (btnmsg != null || btnmsg != undefined) {
                    _btmmsg = btnmsg;
                }
                if (valueDisplay != null || valueDisplay != undefined) {
                    msg = msg.replace("{value}", result.value);
                    Site.Dialogs.Alert(msg, _btmmsg, refreshData());
                } else {
                    Site.Dialogs.Alert(msg, _btmmsg, refreshData());
                }
                
            } else {
                Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
            }
            $('.modal').modal('hide');
            $(".modal-backdrop").remove();
        }
    }
}

$(function () {
    $([".side-menu-link[sub-menu-id='", window.localStorage["sub-menu-id"], "']"].join('')).parent().addClass("active");

    if (window.localStorage["active-menu-id"] == "D-SETTING" || window.localStorage["active-menu-id"] == "P-SETTING" || window.localStorage["active-menu-id"] == "D-RPT") {

        var menuItem = $(["#", window.localStorage["active-menu-id"]].join(''));
        var subMenu = $(["#", window.localStorage["active-menu-id"], " .treeview-menu"].join(''));
        
        menuItem.addClass('menu-open');
        subMenu.css({ "display": "block" });
    }
    $(window).resize(function () {
        Site.Resizer();
    });
    $(window).resize();
    $(".sidebar-menu > li:first-child").css({ "border-top": "0px none transparent" });
});


var ajaxHelper = new function () {
    this.Invoke = function (url, payload, dataType, callback) {
        $(".waitWindow").css({ "display": "block" });
        Site.RequestCounter.push(1);
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: url,
            data: payload,
            dataType: dataType
        }).done(function (result) {
            callback(result);
            Site.RequestCounter.pop();
            console.log("DONE:" + Site.RequestCounter);
            if (Site.RequestCounter.length <= 0) {
                $(".waitWindow").css({ "display": "" });
            }
        }).fail(function (a, b, c) {
            console.log("fail");
            //console.log(a);
            //console.log(b);
            //console.log(c);
            Site.RequestCounter.pop();
            console.log("FAIL:" + Site.RequestCounter);
            if (Site.RequestCounter.length <= 0) {
                $(".waitWindow").css({ "display": "" });
            }
        });
    },
    this.NoWaitInvoke = function (url, payload, dataType, callback) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: url,
            data: payload,
            dataType: dataType
        }).done(function (result) {
            callback(result);
        });
    }
};