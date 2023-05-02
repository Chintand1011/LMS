var DTSDB = {
    RoutesComplete: null,
    AttachmentComplete: null,
    UserId: null,
    BatchId: null,
    RecordClassData: null,
    RecordCategoryData: null,
    DocData: null,
    AttData: null,
    AgingData: null,
    CurrentProcess: null,
    IsArchiving: false,
    IsDisposable: false,
    TrackStatus: null,
    Counter: null,
    RecordCategory: null,
    DocumentClassification: null,
    DocumentSecurityLevel: null,
    DocumentStage: null,
    SenderDepartmentId: null,
    YearsRetention: 0,
    CreatedBy: null,
    AttachmentCountValidated: 0,
    Attachments: [],
    CategoryOption:[],
    ClassificationOption: [],
    GetRecordCategory: function () {
        var postEvt = function () {
            var data = $("body").data(DTSDB.RecordCategoryData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    DTSDB.CategoryOption.push(["<option retention_period='", this.retention_period, "' classid='",
                        this.classification_id, "' value='", this.id, "' desc='", this.category_desc, "'>",
                        this.category_code, "</option>"].join(''));
                });
            }
        }
        Site.PostData("/Setting/GetRecordCategory", postEvt, null, DTSDB.RecordCategoryData);
    },
    GetRecordClassification: function () {
        var postEvt = function () {
            var data = $("body").data(DTSDB.RecordClassData);
           if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    DTSDB.ClassificationOption.push(["<option value='", this.id, "' desc='", this.classification_desc, "'>", this.classification_code, "</option>"].join(''));
                });
            }
        }
        Site.PostData("/Setting/GetRecordClassification", postEvt, null, DTSDB.RecordClassData);
    },
    GetParameters: function (txtSearchVal, cProc) {
        $(".modal-dialog").css({ "min-width": "1000px" });
        var param = DTS.Clone(DTS.param);
        param.menu_id = localStorage.getItem("sub-menu-id");
        param.filter.id = txtSearchVal;
        param.filter.department_id = Site.DepartmentId;
        param.filter.created_by = Site.UserId;
        param.filter.barcode_no = txtSearchVal;
        param.filter.document_code = txtSearchVal;
        param.filter.record_section = $("ul.sidebar-menu li a[menu-id='D-ARCHIVED']").attr("record-section") == "1";
        param.document.process = cProc;
        param.document.created_by = Site.UserId;
        param.document.dept_processed = Site.DepartmentId;
        param.document.document_classification = DTSDB.DocumentClassification;
        param.document.record_category = DTSDB.RecordCategory;
        param.document.id = txtSearchVal;
        param.barcode_no = txtSearchVal;
        return param;
    },
    Initialize: function () {
        while ($("#grdLst").length <= 0) {
            setTimeout(DTSDB.Initialize, 300);
            return;
        }
        $("#dataEntryModal").on('hide.bs.modal', function () {
            $("#formAttachments .attachmentItem").remove();
            $("#lstRoutes tbody, #formGeneral #lstLogs tbody, .routesBar").html('');
            $("#txtBarcodeValidate").val('');
        });
        window.onresize = function () {
            $("#dashboardCategoryLst, #top5Aging").css({ "height": "auto" }).css({ "height": [Math.max($("#dashboardCategoryLst").height(), $("#top5Aging").height()), "px"].join('') });
        }
        $("#DepartmentName").html(Site.DepartmentName);
        $("<th style='white-space:nowrap;'>Sender's Group</th>").insertAfter("#grdLst thead tr th:nth-child(5)");
        $("#cboDocList").change(function () {
            $("#lstSelected").html($(this).find("option:selected").html());
            var curProcesss = "";
            switch ($(this).val()) {
                case "0":
                    curProcesss = CONST.menu_id.OnHand;
                    break;
                case "1":
                    curProcesss = CONST.menu_id.Received;
                    break;
                case "2":
                    curProcesss = CONST.menu_id.Finalized;
                    break;
                case "3":
                    curProcesss = CONST.menu_id.Archived;
                    break;
            }
            DTSDB.MaintainData("", curProcesss, null, false);
        });
        $("#btnViewAll").click(function () {
            DTSDB.NavigateDocuments($("#cboDocList").val(), null);
        });
        $("#lstSelected").html();
        $("#grdLst tr th:last-child, #grdLst tr th:first-child").remove();
        //$("#grdLst").addClass("header-transparent");
        DTSDB.DocData = (new Date().getTime()).toString();
        DTSDB.AttData = [DTSDB.DocData, "-1"].join('');
        DTSDB.AgingData = [DTSDB.DocData, "-2"].join('');
        DTSDB.RecordCategoryData = [DTSDB.DocData, "-3"].join('');
        DTSDB.RecordClassData = [DTSDB.DocData, "-4"].join('');
        DTSDB.GetRecordCategory();
        DTSDB.GetRecordClassification();
        $("#btnReceive").click(function () {
            if (DTSDB.AttachmentCountValidated <= 0) {
                Site.Dialogs.Alert("Cannot receive. There should be at least one attachment validated.", "Got It!", null);
                return;
            }
            var yesEvt = function () {
                var postEvt = function () {
                    var ctl = $(["#formAttachments .attachmentItem[barcodeno]"].join(''));
                    DTSDB.Counter = 0;
                    if ($(ctl).length > 0) {
                        $(ctl).each(function () {
                            var postScanValidator = function () {
                                DTSDB.Counter++;
                                if (DTSDB.Counter >= $(ctl).length) {
                                    DTSDB.MaintainData($("#scan_barcode_ref").val(), CONST.transaction_type.promote, null, false);
                                }
                            }
                            DTSDB.MaintainAttachments(CONST.transaction_type.save, this, postScanValidator);
                        });
                    }
                    else {
                        DTSDB.MaintainData($("#scan_barcode_ref").val(), CONST.transaction_type.promote, null, false);
                    }
                }
                DTSDB.MaintainLogs(CONST.transaction_type.save, $("#dialogInput").val(), postEvt);

            }
            var noEvt = function () {
                $('#dataEntryModal').modal('show');
            }
            $('#dataEntryModal').modal('hide');
            Site.Dialogs.Input("Add Receive Note", "Note:", "Save", "Cancel", yesEvt, noEvt);
            $("#dialogInput").val(["Document received by ", Site.UserName].join(''));
        });
        DTSDB.MaintainData($("#scan_barcode_ref").val(), CONST.transaction_type.get, null, false);
        $("#btnAccept").click(function () {
            if (DTSDB.AttachmentCountValidated <= 0) {
                Site.Dialogs.Alert("Cannot accept. There should be at least one attachment validated.", "Got It!", null);
                return;
            }
            var yesEvt = function () {
                var postEvt = function () {
                    DTSDB.MaintainData($("#scan_barcode_ref").val(), CONST.transaction_type.promote, null, true);
                }
                DTSDB.MaintainLogs(CONST.transaction_type.save, $("#dialogInput").val(), postEvt);
                DTSDB.IsDisposable = $("#is_disposable").is(":checked");
                DTSDB.YearsRetention = $("#u_years_retention").val();
                DTSDB.RecordCategory = $("#u_archive_category").val();
                DTSDB.DocumentClassification = $("#u_record_classification").val();
            }
            var noEvt = function () {
                $('#dataEntryModal').modal('show');
            }
            $('#dataEntryModal').modal('hide');
            Site.Dialogs.Input("Add Accept Note", "Note:", "Save", "Cancel", yesEvt, noEvt);
            $(".dialogBody").css({ "padding-top": "10px" }).prepend(["<div id='acceptNoteDialogs' style='font-size:16px;padding-bottom:10px;'>",
                "Document Classification:&nbsp<select id='u_record_classification' class='form-control selectpicker' data-actions-box='true' ",
                " data-live-search='true' required caption = 'Document Classification' data-none-selected-text > ",
                DTSDB.ClassificationOption.join(''), "</select > <br />Archive Category:&nbsp<select id='u_archive_category' class='form-control selectpicker' ",
                " data-actions-box='true' data-live-search='true' required caption = 'Archive Category' data-none-selected-text > ",
                DTSDB.CategoryOption.join(''), "</select > <br /> <br />",
                "<input id='is_disposable' type='checkbox' />",
                "&nbsp;is Disposable&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;",
                "<input number id='u_years_retention' type='textbox' style='width:50px;' />&nbsp;Years - Retention</div>"].join(''));
            Site.GenerateControls("#acceptNoteDialogs");
            $('.dialogBody').css({ "padding-bottom": "0px" });
            Site.DropDownCommonSettings("#u_record_classification");
            Site.DropDownCommonSettings("#u_archive_category");
            $("#u_record_classification").on('changed.bs.select', function (e, clickedIndex, newValue, oldValue) {
                var ctl = $(this).next().next().find("ul li");
                var curVal = $($("#u_record_classification option")[clickedIndex]).val();
                if ($(this).val() == 0) {
                    $("#u_archive_category option").css({ "display": "" }).removeAttr("disabled");
                }
                else {
                    $("#u_archive_category option").css({ "display": "none" }).removeAttr("disabled").attr("disabled", "disabled");
                    $(["#u_archive_category option[classid='", curVal, "']"].join('')).css({ "display": "" }).removeAttr("disabled");
                }
                $("#u_archive_category").val('');
                $("#u_archive_category").selectpicker('val', '');
                Site.DropDownCommonSettings("#u_archive_category");
                $("#u_years_retention").val('');
            });
            $("#u_archive_category").on('changed.bs.select', function (e, clickedIndex, newValue, oldValue) {
                var ctl = $(this).next().next().find("ul li");
                var retention_period = $($("#u_archive_category option")[clickedIndex]).attr("retention_period");
                $("#u_years_retention").val(retention_period);
            });
            $("#dialogInput").val(["Document is archived by ", Site.UserName].join(''));
        });
        $(".lstCollapser thead tr[collapse]").click(function () {
            $(this).attr("collapse", $(this).attr("collapse") == "1" ? "0" : "1");
            $(this).parent().parent().find("tbody").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
            $(this).parent().parent().find("thead tr:not([collapse])").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
        });
        $("#btnTrackNow").click(function () {
            var isValid = Site.ValidateRequiredEntries(".scan-barcode-section", null);
            if (isValid == true) {
                DTSDB.MaintainData($("#scan_barcode_ref").val(), CONST.transaction_type.track, null, false);
            }
        });
        $('#txtBarcodeValidate').keyup(function (event) {
            if (event.which == 13) {
                var isValid = Site.ValidateRequiredEntries("#barcodeAttachmentScan", null);
                if (isValid == true) {
                    var ctl = $(["#formAttachments .attachmentItem[barcodeno='", $(this).val(), "']"].join(''));
                    if ($(ctl).length <= 0) {
                        Site.Dialogs.Alert(["Barcode No: ", $(this).val(), " does not exists."].join(''), "Got It!", null);
                        $(this).val("");
                        return;
                    }
                    if ($(ctl).attr("status") == CONST.record_status.validated) {
                        Site.Dialogs.Alert(["Barcode No: ", $(this).val(), " already validated."].join(''), "Got It!", null);
                        $(this).val("");
                        return;
                    }
                    $(ctl).attr("status", CONST.record_status.validated);
                    $(ctl).find("input[type='text']").css({ "background-color": "#119137", "font-weight": "bold" });
                    var isValid = Site.ValidateRequiredEntries(".scan-barcode-section", null);
                    DTSDB.AttachmentCountValidated++;
                    $("#validatedEntries").html(["Validated: ", DTSDB.AttachmentCountValidated, " / ", $("#formAttachments .attachmentItem").length].join(''));
                    $(this).val("");
                }
                event.preventDefault();
                return false;
            }
        });
        $('#scan_barcode_ref').keyup(function (event) {
            if (event.which == 13) {
                $("#btnTrackNow").click();
                event.preventDefault();
                return false;
            }
        });
        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
            if ($(this).attr("formid") == "formGeneral") {
                $("#barcodeNote").html("Please scan barcode to accept physical documents.");
                $("#footerNote").html("Document information will be saved and have an auto-generated batch number.");
            }
            else if ($(this).attr("formid") == "formAttachments") {
                $("#barcodeNote").html("Barcodes highlighted in <b style='color:#008000'>Green</b> are already validated.");
                $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
            }
        });
        $("#generalTab").click();
        $("#chkThisWeek, #chkLastWeek").click(function () {
            $("#cboDocList").change();
        });
        DTSDB.GetAging();
    },
    NavigateDocuments: function (cboVal, opt) {
        if (isNaN(cboVal) == true) {
            cboVal = "0";
        }
        window.sessionStorage.removeItem("optSearch");
        if (opt != null) {
            window.sessionStorage.setItem("optSearch", opt);
        }
        switch (cboVal.toString()) {
            case "0":
                $(["ul.sidebar-menu li a[sub-menu-id='", CONST.menu_id.OnHand, "']"].join('')).click();
                break;
            case "1":
                $(["ul.sidebar-menu li a[sub-menu-id='", CONST.menu_id.Received, "']"].join('')).click();
                break;
            case "2":
                $(["ul.sidebar-menu li a[sub-menu-id='", CONST.menu_id.Finalized, "']"].join('')).click();
                break;
            case "3":
                $(["ul.sidebar-menu li a[sub-menu-id='", CONST.menu_id.Archived, "']"].join('')).click();
                break;
        }
        window.location.reload();
    },
    NavigateAging: function (cboVal, opt) {
        if (isNaN(cboVal) == true) {
            cboVal = "0";
        }
        window.sessionStorage.removeItem("optSearch");
        window.sessionStorage.removeItem("aging");
        if (cboVal != null) {
            window.sessionStorage.setItem("aging", cboVal);
        }
        switch (opt.toString()) {
            case "0":
                $(["ul.sidebar-menu li a[sub-menu-id='", CONST.menu_id.OnHand, "']"].join('')).click();
                break;
            case "1":
                $(["ul.sidebar-menu li a[sub-menu-id='", CONST.menu_id.Received, "']"].join('')).click();
                break;
        }
        window.location.reload();
    },
    ViewDocument: function (id) {
        DTSDB.MaintainData(id, CONST.transaction_type.track, null, false);
    },
    MaintainData: function (txtSearchVal, currentProcess, cStat, isArchiving) {
        var param = DTSDB.GetParameters(txtSearchVal, currentProcess);
        var noAging = false;
        if (currentProcess == CONST.menu_id.OnHand || currentProcess == CONST.menu_id.Received ||
            currentProcess == CONST.menu_id.Finalized || currentProcess == CONST.menu_id.Archived) {
            var firstday = null;
            var lastday = null;
            param.document.process = CONST.transaction_type.dashBoard;
            if ($("#chkThisWeek").is(":checked") == true && $("#chkLastWeek").is(":checked") == false) {
                firstday = new Date();
                lastday = new Date();
                firstday = new Date(firstday.setDate(firstday.getDate() - firstday.getDay()));
                lastday = new Date(lastday.setDate(lastday.getDate() - lastday.getDay() + 6));
            }
            else if ($("#chkThisWeek").is(":checked") == false && $("#chkLastWeek").is(":checked") == true) {
                firstday = new Date();
                lastday = new Date();
                firstday = new Date(firstday.setDate(firstday.getDate() - firstday.getDay() - 7));
                lastday = new Date(lastday.setDate(lastday.getDate() - lastday.getDay() - 1));
            }
            else if ($("#chkLastWeek").is(":checked") == true && $("#chkLastWeek").is(":checked") == true) {
                firstday = new Date();
                lastday = new Date();
                firstday = new Date(firstday.setDate(firstday.getDate() - firstday.getDay() - 7));
                lastday = new Date(lastday.setDate(lastday.getDate() - lastday.getDay() + 6));
            }
            param.filter.date_submitted_from = $.datepicker.formatDate("dd-M-yy", firstday);
            param.filter.date_submitted_to = $.datepicker.formatDate("dd-M-yy", lastday);
            param.menu_id = currentProcess;
            param.filter.id = null;
            param.filter.barcode_no = null;
            param.document.id = DTSDB.BatchId;
            param.barcode_no = null;
            param.process = CONST.transaction_type.search;
            if (currentProcess != CONST.menu_id.Received && currentProcess != CONST.menu_id.OnHand) {
                noAging = true;
            }
            currentProcess = param.process;
        }
        if (isArchiving == true) {
            param.menu_id = CONST.menu_id.Finalized;
            param.document.is_disposable = DTSDB.IsDisposable;
            param.document.years_retention = DTSDB.YearsRetention;
        }
        if (cStat != null && cStat != "") {
            param.document.status = cStat;
        }
        param.document.id = DTSDB.BatchId;
        var postEvt = function () {
            var data = $("body").data(DTSDB.DocData);
            if (currentProcess == CONST.transaction_type.track) {
                if (data.value != null && data.value != undefined && data.value != "") {
                    $(data.value).each(function () {
                        DTSDB.BatchId = this.id;
                        DTSDB.DocumentStage = this.doc_stage;
                        DTSDB.SenderDepartmentId = this.sender_department_id;
                        DTSDB.DocumentSecurityLevel = this.document_security_level_id;
                        DTSDB.CreatedBy = this.created_by;
                        DTS.GenerateRouteBar(DTSDB.BatchId);
                        DTSDB.IsArchiving = (this.status == CONST.record_status.archiving);
                        DTSDB.MaintainRoutes(CONST.transaction_type.search, false);
                        DTSDB.TrackStatus = this.track_status;
                        DTSDB.MaintainAttachments(CONST.transaction_type.search, null, null);
                        DTSDB.MaintainLogs(CONST.transaction_type.search);
                        var sender = this.sender_name;
                        var recipient = $(["#grdLst tbody tr[itemid='", DTSDB.BatchId, "'] td[receipient_id]"].join('')).html();
                        var etd_to_recipient = $(["#grdLst tbody tr[itemid='", DTSDB.BatchId, "'] td[etd_to_recipient]"].join('')).html();
                        var document_type = $(["#grdLst tbody tr[itemid='", DTSDB.BatchId, "'] td[document_type_id]"].join('')).html();
                        DTSDB.CurrentProcess = CONST.transaction_type.update;
                        $('#dataEntryModal').modal('show');
                        Site.DrawWaterMark("#canvasImage", this.delivery_type_name);
                        $(".mainCaption #curDateCaption").html([Site.FixDateString(this.created_date).split(' ')[0], ":"].join(''));
                        $(".mainCaption #curDescription").html([this.sender_name, " sent ", this.document_type_name, " to ", this.receipient_name,
                            " with Estimated Time of Delivery(ETD) on ", Site.FixDateString(this.etd_to_recipient)].join(''));
                        $("#generalTab").click();
                        $(".modal-title").html(this.document_code);
                        $(".batchno").html(DTSDB.BatchId);
                    });
                }
                else {
                    Site.Dialogs.Alert(["Document with Barcode or Batch No: ", $("#scan_barcode_ref").val(), " cannot be found. Please make sure that the input is correct and try again later. Thank you!"].join(''), null, pEvt);
                }
            }
            else if (currentProcess == CONST.transaction_type.promote) {
                var pEvt = function () {
                    window.location.reload();
                    return;
                }
                if (isArchiving == true) {
                    Site.Dialogs.Alert(["Document with batch number ", DTSDB.BatchId, " has been successfully archived! You can now view this document under Archived Menu."].join(''), "Got It!", pEvt);
                }
                else {
                    Site.Dialogs.Alert(["Document with batch number ", DTSDB.BatchId, " has been successfully received! You can now view this document under Received Menu."].join(''), "Got It!", pEvt);
                }
            }
            else if (currentProcess == CONST.transaction_type.get) {
                $("#grdLst tbody").html('');
                $("#summary, #underDepartment, #documentAging").html('');
                if (data.value != null && data.value != undefined && data.value != "") {
                    var counter = 0;
                    var curArrColor = 0;
                    var arrColor = ['#5A9BD5', '#ED7D31', '#A5A5A5', '#FFC000', '#4473C5', '#70AD46', '#FB01D2', '#0839FE', '#FE1808', '#B408FE'];
                    var chartData1 = new Array();
                    var labelsCaption = new Array();
                    $(data.value).each(function () {
                        switch (this.category_id) {
                            case 1:
                                $("#summary").append(["<tr><td>", this.category_name, ":</td><td><div class='amount' style='cursor:default;'>", this.statistics, "</div></td></tr>"].join(''));
                                break;
                            case 2:
                                $("#underDepartment").append(["<tr><td>", this.category_name, ":</td><td><div class='amount' onclick='DTSDB.NavigateDocuments(", (this.id - 1), ", 1);'>", this.statistics, "</div></td></tr>"].join(''));
                                break;
                            case 3:
                                $("#documentAging").append(["<tr><td>", this.category_name, ":</td><td style='text-align:center;'>",
                                    (this.id == 1 ? "<div style='font-size:9px;margin-top:-10px;color:#000000;white-space:nowrap;'>On-Hand</div>" : ""),
                                    "<span class='digits' onclick='DTSDB.NavigateAging(", (this.id - 1), ", 0);'>", this.statistics,
                                    "</span></td><td>+</td><td style='text-align:center;'>", (this.id == 1 ?
                                    "<div style='font-size:9px;margin-top:-10px;color:#000000;white-space:nowrap;'>Received</div>" : ""),
                                    "<span class='digits' onclick='DTSDB.NavigateAging(", (this.id - 1), ", 1);'>", this.statistics1, "</span>",
                                    "</td><td>=</td><td><div class='amount' style='cursor:default;'>", (Site.ParseInt(this.statistics) + Site.ParseInt(this.statistics1)),
                                    "</div></td></tr>"].join(''));
                                break;
                            case 4:
                                counter++;
                                $("#categoryLst tbody").append(["<tr><td style='color:", arrColor[curArrColor], ";text-align:left !important;'>", counter, ". ", this.category_code,
                                    "</td><td style='color:", arrColor[curArrColor], ";'>", this.statistics, "</td></tr>"].join(''));
                                curArrColor++;
                                if (curArrColor > arrColor.length) {
                                    curArrColor = 0;
                                }
                                chartData1.push(parseInt(this.statistics));
                                labelsCaption.push(this.category_name);
                                break;
                        }
                    });
                    var ctx1 = document.getElementById("chart1").getContext('2d');
                    var chart1 = new Chart(ctx1, {
                        type: 'pie',
                        data: {
                            datasets: [
                                {
                                    data: chartData1,
                                    backgroundColor: arrColor
                                }
                            ],
                            labels: labelsCaption,
                        },
                        options: {
                            legend: {
                                display: false
                            },
                            tooltips: {
                                enabled: false
                            },
                            plugins: {
                                labels: {
                                    render: 'percentage',
                                    fontColor: '#000',
                                    position: 'border',
                                    outsidePadding: 4
                                },
                                datalabels: {
                                    display: false
                                }
                            },
                            responsive: true,
                            maintainAspectRatio: false,
                        }
                    });

                    $('.chartRow').each(function (index) {
                        $(this).css('color', arrColor[index]);
                    });

                }
                $("#cboDocList").change();
                $(".iconItemContainers").css({ "display": "inline-block" });
                window.onresize();
            }
            else if (currentProcess == CONST.transaction_type.search) {
                $("#grdLst tbody").html('');
                $(data.value).each(function () {
                    $("#grdLst tbody").append(["<tr><td style='width:10%;'>", this.category_code, "</td><td style='width:15%;'>", this.document_type_name,
                        "</td><td style='width:25%;'><button style='font-size:12px;' type='button' class='clearButton' onclick='DTSDB.ViewDocument(",
                        this.id, ");'>", this.document_code, "</button></td><td style='width:20%;'>", this.sender_name,
                        "</td><td style='width:10%;white-space:nowrap;'>", this.sender_department_name, "</td><td style='width:20%;white-space:nowrap;'>",
                        this.receipient_name, "</td><td date style='width:5%;white-space:nowrap;'>", Site.FixDateString(this.created_date),
                    "</td><td date style='width:5%;white-space:nowrap;'>",
                    Site.FixDateString(this.etd_to_recipient), "</td><td style='text-align:center;width:5%;white-space:nowrap;' number>",
                    (noAging == true ? 0 : Site.DaysPast(this.created_date)), "</td></tr>"].join(''));
                });
            }
        };
        if (currentProcess == CONST.transaction_type.search && param.document.process != CONST.transaction_type.dashBoard) {
            param.filter.department_id = null;
        }
        Site.PostData("/DTS/MaintainDocuments", postEvt, param, DTSDB.DocData);
    },
    MaintainRoutes: function (curProcess, isReadOnly) {
        var postEvt = function () {
            var data = $("body").data(DTSDB.DocData);
            if (DTSDB.CurrentProcess == CONST.transaction_type.statusUpdate || DTSDB.CurrentProcess == CONST.transaction_type.promote) {
                $(".ion-search").click();
            }
            else {
                $("#lstRoutes tbody").html('');
                var isUserNotInRoute = true;
                var alreadyReceived = false;
                var track_status;
                var routeFilled = false;
                if (DTSDB.TrackStatus == "V") {
                    isUserNotInRoute = false;
                    routeFilled = true;
                }
                else if (DTSDB.TrackStatus == "R") {
                    isUserNotInRoute = false;
                    routeFilled = true;
                }
                else if ((DTSDB.DocumentSecurityLevel == 1 || DTSDB.DocumentSecurityLevel == 2 || DTSDB.DocumentSecurityLevel == 4) && DTSDB.SenderDepartmentId == Site.DepartmentId && DTSDB.DocumentStage == "O") {
                    isUserNotInRoute = false;
                    routeFilled = true;
                }
                else if (DTSDB.CreatedBy != Site.UserId) {
                }
                else {
                    isUserNotInRoute = false;
                    routeFilled = true;
                }
                if (data != null && data != undefined && data != "") {
                    var counter = 0;
                    $(data).each(function () {
                        counter++;
                        $("#lstRoutes tbody").append(["<tr><td number>", counter, "</td><td>", this.department_name, "</td><td>",
                            this.receipient_name, "</td></tr>"].join(''));
                        if (routeFilled == false && Site.DepartmentId == this.department_id && (DTSDB.DocumentSecurityLevel == 4 || DTSDB.DocumentSecurityLevel == 2) &&
                            (DTSDB.SenderDepartmentId != null && DTSDB.SenderDepartmentId != undefined)) {
                            isUserNotInRoute = true;
                        }
                        else if (routeFilled == false && Site.UserId == this.receipient_id) {
                            isUserNotInRoute = false;
                        }
                        track_status = this.track_status;
                    });
                }
                $("#barcodeNote").css({ "visibility": isReadOnly == true || (isUserNotInRoute == true && DTS.IsRecordSection == false) || DTSDB.TrackStatus == "V" ? "hidden" : "visible" });
                $("#btnReceive").css({
                    "display": isReadOnly == true || DTSDB.IsArchiving == true || DTSDB.TrackStatus != "R" ? "none" : "inline-block"
                });
                $("#btnAccept").css({ "display": isReadOnly == false && DTSDB.IsArchiving == true && DTSDB.TrackStatus == "D" ? "inline-block" : "none" });
                $("#barcodeAttachmentScan, #validatedEntries").css({ "display": isReadOnly == true || (isUserNotInRoute == true && DTS.IsRecordSection == false) || DTSDB.TrackStatus == "V" ? "none" : "" });
            }
        };
        var hasData = false;
        var param = DTSDB.GetParameters($("#scan_barcode_ref").val(), curProcess);
        param.documentRoute.process = curProcess;
        param.documentRoute.batch_id = DTSDB.BatchId;
        param.documentRoute.department_id = Site.DepartmentId;
        if (CONST.transaction_type.save == curProcess) {
            $("#lstRoutes tbody  tr:not([rowcopy]").each(function () {
                var paramRoutes = DTS.Clone(DTS.param.documentRoute);
                paramRoutes.batch_id = DTSDB.BatchId;
                paramRoutes.process = $(this).attr("process");
                paramRoutes.id = $(this).attr("routeid");
                paramRoutes.department_id = $(this).find(".department_id").val();
                paramRoutes.department_name = null;
                paramRoutes.receipient_id = $(this).find(".receipient_id").val();
                paramRoutes.receipient_name = null;
                paramRoutes.status = CONST.record_status.activate;
                param.documentRoutes.push(paramRoutes)
                hasData = true;
            });
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, param, DTSDB.DocData);
        }
    },
    MaintainAttachments: function (curProcess, ctl, postScanEvt) {
        var postEvt = function () {
            var data = $("body").data(DTSDB.AttData);
            DTSDB.AttachmentCountValidated = 0;
            if (postScanEvt != null || postScanEvt != undefined) {
                postScanEvt();
            }
            if (data != null && data != undefined && data != "") {
                if (CONST.transaction_type.search == curProcess) {
                    $("#formAttachments .attachmentItem").remove();
                    var maxCount = 0;
                    $(data).each(function (index) {
                        maxCount++;
                        if (this.status == CONST.record_status.validated) {
                            DTSDB.AttachmentCountValidated++;
                        }
                        var imgCls = Utilities.GetFileTypeClass(this.file_name);
                        $("#formAttachments").append(["<div barcodeno ='", this.barcode_no, "' status ='", this.status,
                            "' data-toggle='tooltip' data-html='true' data-original-title='<b>Uploaded On</b> - ",
                            this.created_date, "<br /><b>Uploaded by</b> - ", this.updated_by_name,
                            "' attachmentid ='", this.id, "' filename='", this.file_name, "' process='", CONST.transaction_type.save, "' class='attachmentItem' style='float:left;'>",
                            "<span actualFileName='", this.file_name, "' style='display:none;'>", this.file_name, "</span>",
                            "<span class='label label-danger attachmentbadge' style='float: right;'>", index + 1, "</span>",
                            "<div class='attachmentName breakWord' attachmentName='", this.attachment_name, "'>", this.attachment_name,
                            "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name), "' ><div class='", imgCls,
                            "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                            this.barcode_no, "' disabled='true'",
                            " /></div>",
                        ].join(''));
                    });
                    $("#formAttachments .attachmentItem").tooltip();
                    $("#validatedEntries").html(["Validated: ", DTSDB.AttachmentCountValidated, " / ", maxCount].join(''));
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("status", CONST.record_status.cancel);
                    });
                }
                else {
                    $(DTSDB.Attachments).each(function () {
                        var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                        var fileName = [$(curFile).find("input[type='text']").val(), $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');
                        FileUploader.UploadDocument(this.Blob, fileName, null);
                    });
                    DTSDB.MaintainAttachments(CONST.transaction_type.search, null);
                }
            }
        };
        var hasData = false;
        var param = DTSDB.GetParameters($("#scan_barcode_ref").val(), curProcess);
        param.documentAttachment.process = curProcess;
        param.documentAttachment.batch_id = DTSDB.BatchId;
        if (CONST.transaction_type.save == curProcess && ctl != null && ctl != undefined) {
            var paramAttachments = DTS.Clone(DTS.param.documentAttachment);
            paramAttachments.batch_id = DTSDB.BatchId;
            paramAttachments.process = $(ctl).attr("process");
            paramAttachments.id = $(ctl).attr("attachmentid");
            paramAttachments.barcode_no = $(ctl).find("input[type='text']").val();
            paramAttachments.file_name = $(ctl).attr("filename");
            paramAttachments.attachment_name = $(ctl).find(".attachmentName").html();
            paramAttachments.status = CONST.record_status.validated;
            param.documentAttachments.push(paramAttachments)
            hasData = true;
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentAttachments", postEvt, param, DTSDB.AttData);
        }
    },
    MaintainLogs: function (curProcess, curRemarks, postFinalEvt) {
        var postEvt = function () {
            if (postFinalEvt != null || postFinalEvt != undefined) {
                postFinalEvt();
            }
            var data = $("body").data(DTSDB.AttData);
            $("#currentPerson").html("");
            $("#formGeneral #lstLogs tbody").html('');
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#lstLogs").append(["<tr><td date>", Site.FixDateString(this.log_date).split(' ')[0],
                        "</td><td time>", Site.FixDateString(this.log_date).split(' ')[1], "</td><td>", Site.GetStatusDescription(this.status), "</td><td recipient>",
                        this.receipient_name, "</td><td>", this.remarks, "</td></tr>"].join(''));
                });
            }
            if ($("#lstLogs tbody tr").length > 0) {
                $("#currentPerson").html($("#lstLogs tbody tr:first-child td[recipient]").html());
            }
        };
        var hasData = false;
        var param = DTSDB.GetParameters($("#scan_barcode_ref").val(), curProcess);
        param.documentLog.process = curProcess;
        param.documentLog.batch_id = DTSDB.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            var paramLogs = DTS.Clone(DTS.param.documentLog);
            paramLogs.batch_id = DTSDB.BatchId;
            paramLogs.process = curProcess;
            paramLogs.id = null;
            paramLogs.status = CONST.record_status.received;
            paramLogs.receipient_id = Site.UserId;
            paramLogs.remarks = curRemarks;
            param.documentLogs.push(paramLogs)
            hasData = true;
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentLogs", postEvt, param, DTSDB.AttData);
        }
    },
    GetAging: function () {
        var param = DTSDB.GetParameters($("#scan_barcode_ref").val(), CONST.transaction_type.top5agingdocuments);
        var postEvt = function () {
            var data = $("body").data(DTSDB.AgingData);
            $("#documentAgingLst tbody").html('');
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    $("#documentAgingLst tbody").append(["<tr><td style='text-align:left;'>", this.category_code,
                        "</td><td style='color:#808080;text-align:left;'>", this.document_type_name,
                        "</td><td style='color:#808080;text-align:left;'>", this.document_code, "</td><td date style='width:5%;white-space:nowrap;'>",
                        Site.FixDateString(this.created_date), "</td><td date style='width:5%;white-space:nowrap;'>", Site.FixDateString(this.etd_to_recipient),
                        "</td><td style='text-align:center;width:5%;white-space:nowrap;' number>", Site.DaysPast(this.created_date), "</td></tr>"].join(''));
                });
            }
            window.onresize();
        };
        Site.PostData("/DTS/MaintainDocuments", postEvt, param, DTSDB.AgingData);
    },
}
$(function () {
    DTSDB.Initialize();
});