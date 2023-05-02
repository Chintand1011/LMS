var FLZD = {
    UserId: null,
    BatchId: null,
    DocData: null,
    AttData: null,
    CurrentProcess: null,
    Attachments: [],
    GetParameters: function (cProc) {
        var param = DTS.Clone(DTS.param);
        param.menu_id = localStorage.getItem("sub-menu-id");
        param.page_index = $(".paging").val();
        param.filter.id = $("#u_batch_id").val();
        param.filter.department_id = Site.GetParameterByName("deptid") == null ? Site.DepartmentId : Site.GetParameterByName("deptid");
        param.filter.created_by = Site.UserId;
        param.filter.date_submitted_from = $("#u_date_submitted_from").val();
        param.filter.date_submitted_to = $("#u_date_submitted_to").val();
        param.filter.etd_from = $("#u_etd_from").val();
        param.filter.etd_to = $("#u_etd_to").val();
        param.filter.sender_name = $("#u_sender_id").next().attr("title");
        param.filter.receipient_name = $("#u_receipient_id").next().attr("title");
        param.filter.category_name = $("#u_category_id").next().attr("title");
        param.filter.document_type_name = $("#u_document_type_id").next().attr("title");
        param.filter.barcode_no = $("#u_barcode_no").val();
        param.filter.document_code = $("#u_document_code").val();
        param.filter.record_section = $("ul.sidebar-menu li a[menu-id='D-ARCHIVED']").attr("record-section") == "1";
        param.document.process = cProc;
        param.document.id = FLZD.BatchId;
        param.document.dept_processed = Site.DepartmentId;
        param.document.category_id = $("#formGeneral #u_category_id").val();
        param.document.document_type_id = $("#formGeneral #u_document_type_id").val();
        param.document.document_code = $("#formGeneral #u_document_code").val();
        param.document.sender_id = $("#formGeneral #u_sender_id").val();
        param.document.receipient_id = $("#formGeneral #u_receipient_id").val();
        param.document.etd_to_recipient = [$("#formGeneral #u_etd_to_recipient").val(), " ", $("#u_etd_to_recipient_time").val()].join('');
        param.document.delivery_type_id = $("#formGeneral #u_delivery_type_id").val();
        param.document.document_security_level_id = $("#formGeneral #u_document_security_level_id").val();
        param.document.created_by = Site.UserId;
        param.document.updated_by = Site.UserId;
        return param;
    },
    Initialize: function () {
        FLZD.DocData = (new Date().getTime()).toString();
        FLZD.AttData = [FLZD.DocData, "-1"].join('');
        $("#pageContainer").remove();
        $("input[type='radio'][name='durationOpt']").click(function () {
            $(".filterContainer #u_date_submitted_to").val($(this).attr("dateValTo"));
            $(".filterContainer #u_date_submitted_from").val($(this).attr("dateValFrom"));
        });
        $(".lstCollapser thead tr[collapse]").click(function () {
            $(this).attr("collapse", $(this).attr("collapse") == "1" ? "0" : "1");
            $(this).parent().parent().find("tbody").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
            $(this).parent().parent().find("thead tr:not([collapse])").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
        });
        $(".ion-search").click(function () {
            FLZD.MaintainData(CONST.transaction_type.search, null);
        });
        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
        });
        $("#generalTab").click();
        if (window.sessionStorage["optSearch"] == "0" || Site.GetParameterByName("deptid") != null) {
        }
        else if (window.sessionStorage["optSearch"] == "1") {
            window.sessionStorage.removeItem("optSearch");
            window.location.href = [window.location.href, '?deptid=', Site.DepartmentId].join('');
        }
        else {
            $("input[type='radio'][name='durationOpt'][value='0']").click();
        }
        window.sessionStorage.removeItem("optSearch");
        $(".ion-search").click();
    },
    MaintainData: function (currentProcess, cStat) {
        var param = FLZD.GetParameters(currentProcess);
        if (cStat != null && cStat != "") {
            param.document.status = cStat;
        }
        var postEvt = function () {
            if (currentProcess == CONST.transaction_type.statusUpdate) {
                var pEvt = function () {
                    $(".ion-search").click();
                    return;
                }
                Site.Dialogs.Alert(["Document with batch number ", FLZD.BatchId, " is successfully submitted for Archiving. Document can be viewed under @Records Section Menu once Archiving Request has been accepted"].join(''), "Got It!", null);
                return;
            }
            var data = $("body").data(FLZD.DocData);
            $("#grdLst tbody").html('');
            Site.FillPagingValues(data.page_count, false);
            $(".totalDocs").html(data.total_count);
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    FLZD.BatchId = this.id;
                    $("#grdLst tbody").append(["<tr status='", this.status ,"' document_security_level_id='", this.document_security_level_id,
                        "' delivery_type_id='", this.delivery_type_id, "' itemid='", this.id,
                        "'><td number>", this.id, "</td><td category_id='", this.category_id, "'>", this.category_code,
                        "</td><td document_type_id='", this.document_type_id, "'>", this.document_type_name, "</td><td document_code>", this.document_code,
                        "</td><td sender_id='", this.sender_id, "'>", this.sender_name, "<span class='delivery_type_name' style='display:none;'>",
                        this.delivery_type_name, "</span></td><td receipient_id='", this.receipient_id, "'>",
                        this.receipient_name, "</td><td sent_date date>", Site.FixDateString(this.created_date),
                        "</td><td etd_to_recipient date>", Site.FixDateString(this.etd_to_recipient),
                        "</td><td style='text-align:center;' number>0</td><td style='text-align:center;'>", DTS.ContextMenu, "</td></tr>"].join(''));
                });
                $("#grdLst tbody tr").each(function () {
                    $(this).find(".itemName").html([$(this).find("td:first-child").html(), ' - ', $(this).find("td[category_id]").html()].join(''));
                    $(this).find(".subItemName").html($(this).find("td[document_code]").html());
                    $(this).find(".contextItemContainer").html([
                        "<li><a class='dropdown-item item-setting-inactive lnkView' href='#' itemid='", $(this).attr("itemid"), "'>View Document Information</a></li>",
                        ($(this).attr("status") == CONST.record_status.archiving ? "" :
                        ["<li><a class='dropdown-item item-setting-inactive lnkArchive' href='#' itemid='", $(this).attr("itemid"),
                        "'>Archive at Records Division</a></li>"].join('')),
                        "<li><a class='dropdown-item item-setting-inactive lnkPrint' href='#' itemid='", $(this).attr("itemid"), "'>Print Transmittal</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkRoutine' href='#' itemid='", $(this).attr("itemid"), "'>Print Routine slip</a></li>",
                    ].join(''));

                });
                $("#grdLst tbody tr .lnkArchive").unbind("click");
                $("#grdLst tbody tr .lnkArchive").click(function () {
                    FLZD.BatchId = BatchId = $(this).attr("itemid");
                    var lnkLst = $(this);
                    var yesEvt = function () {
                        var postEvt = function () {
                            FLZD.MaintainData(CONST.transaction_type.statusUpdate, CONST.record_status.archiving);
                            $(lnkLst).remove();
                        }
                        FLZD.MaintainLogs(CONST.transaction_type.save, $("#dialogInput").val(), postEvt);
                    }
                    Site.Dialogs.Input("Add Archiving Note", "Note:", "Save", "Cancel", yesEvt, null);
                    $("#dialogInput").val(["Document submitted for archiving by ", Site.UserName].join(''));
                });
                
                $("#grdLst tbody tr .lnkView").unbind("click");
                $("#grdLst tbody tr .lnkView").click(function () {
                    FLZD.PopUpModal($(this).attr("itemid"));
                });

                $(document).on('click touchend', function (e) {
                    $(".setting-cmd").hide();
                });
                $(".setting-icon").unbind("click");
                $(".setting-icon").click(function (e) {
                    e.preventDefault();
                    var popup = $(this).siblings(".setting-cmd");

                    if (popup.is(':visible')) {
                        $(".setting-cmd").hide();
                        return;
                    }
                    else {
                        $(".setting-cmd").hide();
                    }
                    popup.show();
                    new Popper(this, popup,
                        {
                            placement: 'bottom',
                            offset: 10,
                            keepTogether: false
                        });
                });

                $("#grdLst tbody tr .lnkRoutine").unbind("click");
                $("#grdLst tbody tr .lnkRoutine").click(function () {
                    var batchId = $(this).attr("itemid");
                    window.open("/report/RoutingSlip/" + batchId);
                });
            }
        };
        Site.PostData("/DTS/MaintainDocuments", postEvt, param, FLZD.DocData);
    },
    PopUpModal: function(itemId){
        FLZD.BatchId = itemId;
        DTS.GenerateRouteBar(FLZD.BatchId);
        var sender = $(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td[sender_id]"].join('')).html();
        var recipient = $(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td[receipient_id]"].join('')).html();
        var etd_to_recipient = $(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td[etd_to_recipient]"].join('')).html();
        var document_type = $(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td[document_type_id]"].join('')).html();
        FLZD.CurrentProcess = CONST.transaction_type.update;
        $('#dataEntryModal').modal('show');
        $(".mainCaption #curDateCaption").html([$(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td[sent_date]"].join('')).html().split(' ')[0], ":"].join(''));
        $(".mainCaption #curDescription").html([sender, " sent ", document_type, " to ", recipient, " with Estimated Time of Delivery(ETD) on ", etd_to_recipient].join(''));
        $("#generalTab").click();
        $(".modal-title").html($(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td[document_code]"].join('')).html());
        $(".batchno").html(FLZD.BatchId);
        Site.DrawWaterMark("#canvasImage", $(["#grdLst tbody tr[itemid='", FLZD.BatchId, "'] td span.delivery_type_name"].join('')).html());
        FLZD.MaintainRoutes(CONST.transaction_type.search);
        FLZD.MaintainAttachments(CONST.transaction_type.search);
        FLZD.MaintainLogs(CONST.transaction_type.search);
    },
    MaintainRoutes: function (curProcess) {
        var postEvt = function () {
            var data = $("body").data(FLZD.DocData);
            if (FLZD.CurrentProcess == CONST.transaction_type.statusUpdate || FLZD.CurrentProcess == CONST.transaction_type.promote) {
                $(".ion-search").click();
            }
            else {
                $("#lstRoutes tbody").html('');
                if (data != null && data != undefined && data != "") {
                    var counter = 0;
                    $(data).each(function () {
                        counter++;
                        $("#lstRoutes tbody").append(["<tr><td number>", counter, "</td><td>", this.department_name, "</td><td>",
                            this.receipient_name, "</td></tr>"].join(''));
                    });
                }
            }
        };
        var hasData = false;
        var param = FLZD.GetParameters(curProcess);
        param.documentRoute.process = curProcess;
        param.documentRoute.batch_id = FLZD.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            $("#lstRoutes tbody  tr:not([rowcopy]").each(function () {
                var paramRoutes = DTS.Clone(DTS.param.documentRoute);
                paramRoutes.batch_id = FLZD.BatchId;
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
            Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, param, FLZD.DocData);
        }
    },
    MaintainAttachments: function (curProcess) {
        var postEvt = function () {
            var data = $("body").data(FLZD.AttData);
            var tooltip;
            if (data != null && data != undefined && data != "") {
                if (CONST.transaction_type.search == curProcess) {
                    $("#formAttachments .attachmentItem").remove();
                    $(data).each(function (index) {
                        var imgCls = Utilities.GetFileTypeClass(this.file_name);
                        $("#formAttachments").append(["<div status ='", this.status, "' data-toggle='tooltip' ",
                            "data-html='true' data-original-title='<b>Uploaded On</b> - ",
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
                }
                else {
                    $(FLZD.Attachments).each(function () {
                        var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                        var fileName = [$(curFile).find("span[actualFileName]").html(), $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');
                        FileUploader.UploadDocument(this.Blob, fileName, null);
                    });
                    FLZD.MaintainAttachments(CONST.transaction_type.search);
                }
            }
        };
        var hasData = false;
        var param = FLZD.GetParameters(curProcess);
        param.documentAttachment.process = curProcess;
        param.documentAttachment.batch_id = FLZD.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            $("#formAttachments .attachmentItem").each(function () {
                var paramAttachments = DTS.Clone(DTS.param.documentAttachment);
                paramAttachments.batch_id = FLZD.BatchId;
                paramAttachments.process = $(this).attr("process");
                paramAttachments.id = $(this).attr("attachmentid");
                paramAttachments.barcode_no = $(this).find("input[type='text']").val();
                if (CONST.transaction_type.save == $(this).attr("process")) {
                    paramAttachments.file_name = [$(this).find("input[type='text']").val(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');
                }
                else {
                    paramAttachments.file_name = [$(this).find("span[actualFileName]").html(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');
                }
                paramAttachments.attachment_name = $(this).find("span[actualFileName]").html();
                paramAttachments.status = FLZD.IsFinalize == true ? CONST.record_status.validated : $(this).attr("status");
                param.documentAttachments.push(paramAttachments)
                hasData = true;
            });
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentAttachments", postEvt, param, FLZD.AttData);
        }
    },
    MaintainLogs: function (curProcess, curRemarks, postFinalEvt) {
        var postEvt = function () {
            if (postFinalEvt != null || postFinalEvt != undefined) {
                postFinalEvt();
            }
            var data = $("body").data(FLZD.AttData);
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
            if (curProcess == CONST.transaction_type.save && FLZD.IsFinalize == false) {
                Site.Dialogs.Alert(["Remarks successfully added to document with batch number ", FLZD.BatchId, "!"].join(''), "Got It!", null);
            }
        };
        var hasData = false;
        var param = FLZD.GetParameters(curProcess);
        param.documentLog.process = curProcess;
        param.documentLog.batch_id = FLZD.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            var paramLogs = DTS.Clone(DTS.param.documentLog);
            paramLogs.batch_id = FLZD.BatchId;
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
            Site.PostData("/DTS/MaintainDocumentLogs", postEvt, param, FLZD.AttData);
        }
    },
}
$(function () {
    FLZD.Initialize();
});