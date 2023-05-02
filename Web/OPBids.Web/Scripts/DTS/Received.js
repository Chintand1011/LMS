var RCVD = {
    UserId: null,
    BatchId: null,
    DocData: null,
    AttData: null,
    IsFinalize: false,
    CurrentProcess: null,
    Attachments: [],
    GetParameters: function (cProc) {
        var param = DTS.Clone(DTS.param);
        param.menu_id = localStorage.getItem("sub-menu-id");
        param.page_index = $(".paging").val();
        param.filter.id = $("#u_batch_id").val();
        param.filter.department_id = Site.GetParameterByName("deptid") == null ? Site.DepartmentId : Site.GetParameterByName("deptid");
        param.filter.created_by = Site.UserId;
        if (isNaN(Site.GetParameterByName("aging")) == false) {
            switch (parseInt((Site.GetParameterByName("aging")))) {
                case 0:
                    $("#u_date_submitted_from").val('');
                    $("#u_date_submitted_to").val(Site.Date.AddMonth(-1));
                    break;
                case 1:
                    $("#u_date_submitted_from").val(Site.Date.AddMonth(-1));
                    $("#u_date_submitted_to").val(Site.Date.AddDay(-14));
                    break;
                case 2:
                    $("#u_date_submitted_from").val(Site.Date.AddDay(-14));
                    $("#u_date_submitted_to").val(Site.Date.AddDay(-5));
                    break;
                case 3:
                    $("#u_date_submitted_from").val(Site.Date.AddDay(-5));
                    $("#u_date_submitted_to").val('');
                    break;
            }
        }
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
        param.filter.department_id = Site.DepartmentId;
        param.document.dept_processed = Site.DepartmentId;
        param.document.process = cProc;
        param.document.id = RCVD.BatchId;
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
    ValidateAttachment: function (blob, val) {
        val = val.toLowerCase();
        var regex = new RegExp("(.*?)\.(jpg|png|gif|doc|pdf|docx|xls|xlsx|txt)$");
        if ((regex.test(val.toLowerCase()))) {
            var attachmentName = window.prompt("Please enter an attachment name", "");
            if (attachmentName == null || attachmentName == "") {
                $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
                return;
            }
            var param = DTS.Clone(DTS.AttachmentParam);
            param.GUID = (new Date().getTime()).toString();
            param.Blob = blob;
            attachmentName = [param.GUID, "_", attachmentName].join('');
            RCVD.Attachments.push(param);
            var imgCls = Utilities.GetFileTypeClass(val);
            $("#formAttachments").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<span actualFileName style='display:none;'>", attachmentName, "</span>",
                "<div class='attachmentName breakWord'>", attachmentName.substring(attachmentName.indexOf('_') + 1),
                "</div><div class='", imgCls, "' title='For upload'></div><input type='text' maxlength='20' />",
                "<div style='text-align:center;width:100%;'><button type='button' class='removeItem'></button></div></div>"].join(''));
            $(["#formAttachments .attachmentItem[guid='", param.GUID, "'] .removeItem"].join('')).click(function () {
                $(this).parent().parent().remove();
            });
        }
        else {
            Site.Dialogs.Alert("Invalid file extension, please upload a file with jpg, png, gif, doc, pdf, docx, xls, xlsx, or txt file exntesions.", null, null);
        }
        $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
    },
    Initialize: function () {
        RCVD.DocData = (new Date().getTime()).toString();
        RCVD.AttData = [RCVD.DocData, "-1"].join('');
        $("#pageContainer").remove();
        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                RCVD.ValidateAttachment(blobData, pFile[0].name);
            }
        });
        $("input[type='radio'][name='durationOpt']").click(function () {
            $(".filterContainer #u_date_submitted_to").val($(this).attr("dateValTo"));
            $(".filterContainer #u_date_submitted_from").val($(this).attr("dateValFrom"));
        });
        $("#btnSave").click(function () {
            RCVD.MaintainAttachments(CONST.transaction_type.save);
        });
        $("#btnRemarks").click(function () {
            var yesEvt = function () {
                $('#dataEntryModal').modal('show');
                RCVD.MaintainLogs(CONST.transaction_type.save, $("#dialogInput").val());
            }
            var noEvt = function () {
                $('#dataEntryModal').modal('show');
            }
            $('#dataEntryModal').modal('hide');
            Site.Dialogs.Input("Add Document Remarks", "Remarks:", "Save", "Cancel", yesEvt, noEvt);
        });
        $(".lstCollapser thead tr[collapse]").click(function () {
            $(this).attr("collapse", $(this).attr("collapse") == "1" ? "0" : "1");
            $(this).parent().parent().find("tbody").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
            $(this).parent().parent().find("thead tr:not([collapse])").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
        });
        $("#btnBrowse").click(function () {
            $("#fleBrowse").click();
        });
        $(".ion-search").click(function () {
            RCVD.MaintainData(CONST.transaction_type.search, null, false);
        });
        $("#fleBrowse").change(function () {
            RCVD.ValidateAttachment(this.files[0], $(this).val());
        });
        $("#btnFinalize").click(function () {
            var yesEvt = function () {
                var postEvt = function(){
                    RCVD.CurrentProcess = CONST.transaction_type.promote;
                    RCVD.MaintainData(RCVD.CurrentProcess, null, false);
                    $(".ion-search").click();
                }
                RCVD.MaintainLogs(CONST.transaction_type.save, $("#dialogInput").val(), postEvt);
            }
            var noEvt = function () {
                $('#dataEntryModal').modal('show');
            }
            $('#dataEntryModal').modal('hide');
            Site.Dialogs.Input("Add Finalizing Note", "Note:", "Save", "Cancel", yesEvt, noEvt);
            $("#dialogInput").val(["Document has been finalized by ", Site.UserName].join(''));
        });
        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
            if ($(this).attr("formid") == "formGeneral") {
                if (RCVD.IsFinalize == false) {
                    $("#footerNote").html("");
                }
                else {
                    $("#footerNote").html("");
                }
            }
            else if ($(this).attr("formid") == "formAttachments") {
                if ($("#btnBrowse").parent().css("display") == "none") {
                    $("#footerNote").html("Finalizing of document will prevent users from adding more attachments."); 
                }
                else {
                    $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
                }
            }
        });
        $("#generalTab").click();
        if (window.sessionStorage["optSearch"] == "1") {
            window.sessionStorage.removeItem("optSearch");
            window.location.href = [window.location.href, '?deptid=', Site.DepartmentId].join('');
        }
        else if (window.sessionStorage["aging"] != null) {
            var aging = window.sessionStorage["aging"];
            window.sessionStorage.removeItem("aging");
            window.location.href = [window.location.href, '?deptid=', Site.DepartmentId, '&aging=', aging].join('');
        }
        else if (window.sessionStorage["optSearch"] == "0" || Site.GetParameterByName("deptid") != null) {
        }
        else {
            $("input[type='radio'][name='durationOpt'][value='0']").click();
        }
        window.sessionStorage.removeItem("optSearch");
        $(".ion-search").click();
    },
    MaintainData: function (currentProcess, cStat, isSendToNext) {
        var param = RCVD.GetParameters(currentProcess);
        if (cStat != null && cStat != "") {
            param.document.status = cStat;
        }
        var postEvt = function () {
            if (currentProcess == CONST.transaction_type.promote) {
                var pEvt = function () {
                    $(".ion-search").click();
                    return;
                }
                if (isSendToNext == true) {
                    Site.Dialogs.Alert(["Document with Batch No: ", RCVD.BatchId, " has been successfully submitted to next recipient. Thank you!"].join(''), "Got It!", null);
                }
                else {
                    Site.Dialogs.Alert(["Document with batch number ", RCVD.BatchId, " has been successfully finalized! You can now view this document under Finalized Menu."].join(''), "Got It!", pEvt);
                }
                return;
            }
            var data = $("body").data(RCVD.DocData);
            $("#grdLst tbody").html('');
            Site.FillPagingValues(data.page_count, false);
            $(".totalDocs").html(data.total_count);
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    $("#grdLst tbody").append(["<tr document_security_level_id='", this.document_security_level_id,
                        "' delivery_type_id='", this.delivery_type_id, "' itemid='", this.id,
                        "'><td number>", this.id, "</td><td category_id='", this.category_id, "'>", this.category_code,
                        "</td><td document_type_id='", this.document_type_id, "' is_edoc=", this.is_edoc, ">", this.document_type_name, "</td><td document_code>", this.document_code,
                        "</td><td sender_id='", this.sender_id, "'>", this.sender_name, "<span class='delivery_type_name' style='display:none;'>",
                        this.delivery_type_name, "</span></td><td receipient_id='", this.receipient_id, "'>", this.receipient_name,
                        "</td><td sent_date date>", Site.FixDateString(this.created_date), "</td><td etd_to_recipient date>",
                        Site.FixDateString(this.etd_to_recipient), "</td><td style='text-align:center;' number>",
                        Site.DaysPast(this.created_date), "</td><td style='text-align:center;'>", DTS.ContextMenu,"</td></tr>"].join(''));
                });
                if (currentProcess == CONST.transaction_type.create) {
                    Site.Dialogs.Alert(["New document tracking successfully saved under batch number ", RCVD.BatchId, ". You can now view the document information under On-Hand List"].join(''), "Got It!", null);
                }
                $("#grdLst tbody tr").each(function () {
                    $(this).find(".itemName").html([$(this).find("td:first-child").html(), ' - ', $(this).find("td[category_id]").html()].join(''));
                    $(this).find(".subItemName").html($(this).find("td[document_code]").html());
                    $(this).find(".contextItemContainer").html([
                        "<li><a class='dropdown-item item-setting-inactive lnkView' href='#' itemid='", $(this).attr("itemid"), "'>View Document Information</a></li>",
                        ($(this).find("td[is_edoc]").attr("is_edoc") == 'true' ? ["<li><a class='dropdown-item item-setting-inactive lnkSubmitToNextRecipient' href='#' itemid='", $(this).attr("itemid"), "'>Submit to Next Recipient</a></li>"].join('') : ""),
                        "<li><a class='dropdown-item item-setting-inactive lnkFinalize' href='#' itemid='", $(this).attr("itemid"), "'>Finalize Document</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkPrint' href='#' itemid='", $(this).attr("itemid"), "'>Print Transmittal</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkRoutine' href='#' itemid='", $(this).attr("itemid"), "'>Print Routine slip</a></li>",
                    ].join(''));

                });
                $("#grdLst tbody tr .lnkFinalize").unbind("click");
                $("#grdLst tbody tr .lnkFinalize").click(function () {
                    RCVD.IsFinalize = true;
                    RCVD.PopUpModal($(this).attr("itemid"));
                    $("#btnFinalize").css({ "display": "inline-block" });
                    $("#btnRemarks, #btnSave, #footerNote").css({ "display": "none" });
                    $(".btnBrowse").parent().css({ "display": "none" });
                    $("#footerNote").html(""); 
                });
                $("#grdLst tbody tr .lnkSubmitToNextRecipient").unbind("click");
                $("#grdLst tbody tr .lnkSubmitToNextRecipient").click(function () {
                    RCVD.BatchId = $(this).attr("itemid");
                    var paramRoutes = DTS.Clone(DTS.param);
                    paramRoutes.documentRoute.batch_id = RCVD.BatchId;
                    paramRoutes.documentRoute.process = CONST.transaction_type.search;
                    var postEvt = function () {
                        var data = $("body").data(RCVD.DocData);
                        var cntnt = [];
                        cntnt.push("<table><tr><td style='vertical-align:top;padding-right:10px;'><label class='control-label'>Recipient:</label></td><td><select type='select-one' id='cboReceipientPopUp' style='width:100%;'>");
                        $(data).each(function () {
                            cntnt.push(['<option value="', this.receipient_id, '">', this.receipient_name, '</option>'].join(''));
                        });
                        cntnt.push("</select><br/><span style='font-size:12px;'>The names on the list is based on the set Routes for the Document.</span></td></tr></table>");
                        var yesEvt = function () {
                            RCVD.CurrentProcess = CONST.transaction_type.promote;
                            RCVD.MaintainData(RCVD.CurrentProcess, null, true);
                            var postEvt = function () {
                                window.location.reload();
                            };
                            var routeFlows = [];
                            $("#cboReceipientPopUp option").each(function () {
                                routeFlows.push($(this).html());
                            });
                            var param = DTS.Clone(DTS.param);
                            param.userNotification.process = CONST.transaction_type.create;
                            param.userNotification.message = encodeURI(["<b>", Site.UserName, "</b> sent you an e-Doc with Batch #: <b>", RCVD.BatchId, "</b>, with Doc Code <b>",
                                $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[document_code]"].join('')).html(), "</b>, created by <b>",
                                Site.UserName, "</b> going to <b>", $("#cboReceipientPopUp").val(), "</b> w/ ETD <b>",
                                $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[etd_to_recipient]"].join('')).html(), "</b><br /><b>Routes : </b>", routeFlows.join(' -> ')].join(''));
                            param.userNotification.recipient_ids = $("#cboReceipientPopUp").val();
                            param.userNotification.sender_id = Site.UserId;
                            Site.AddNotification(param, postEvt);
                        }
                        Site.Dialogs.InputGeneric("Submit to Next Recipient", cntnt.join(''), "Submit", "Cancel", yesEvt, null);
                    }
                    Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, paramRoutes, RCVD.DocData);
                });
                $("#grdLst tbody tr .lnkView").unbind("click");
                $("#grdLst tbody tr .lnkView").click(function () {
                    RCVD.IsFinalize = false;
                    RCVD.PopUpModal($(this).attr("itemid"));
                    $("#btnRemarks, #btnSave, #footerNote").css({ "display": "inline-block" });
                    $(".btnBrowse").parent().css({ "display": "inline-block" });
                    $("#btnFinalize").css({ "display": "none" });
                    $("#footerNote").html(""); 
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
        Site.PostData("/DTS/MaintainDocuments", postEvt, param, RCVD.DocData);
    },
    PopUpModal: function(itemId){
        RCVD.BatchId = itemId;
        DTS.GenerateRouteBar(RCVD.BatchId);
        var sender = $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[sender_id]"].join('')).html();
        var recipient = $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[receipient_id]"].join('')).html();
        var etd_to_recipient = $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[etd_to_recipient]"].join('')).html();
        var document_type = $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[document_type_id]"].join('')).html();
        RCVD.CurrentProcess = CONST.transaction_type.update;
        $('#dataEntryModal').modal('show');
        $(".mainCaption #curDateCaption").html([$(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[sent_date]"].join('')).html().split(' ')[0], ":"].join(''));
        $(".mainCaption #curDescription").html([sender, " sent ", document_type, " to ", recipient, " with Estimated Time of Delivery(ETD) on ", etd_to_recipient].join(''));
        $("#generalTab").click();
        $(".modal-title").html($(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td[document_code]"].join('')).html());
        $(".batchno").html(RCVD.BatchId);
        Site.DrawWaterMark("#canvasImage", $(["#grdLst tbody tr[itemid='", RCVD.BatchId, "'] td span.delivery_type_name"].join('')).html());
        RCVD.MaintainRoutes(CONST.transaction_type.search);
        RCVD.MaintainAttachments(CONST.transaction_type.search);
        RCVD.MaintainLogs(CONST.transaction_type.search);
    },
    MaintainRoutes: function (curProcess) {
        var postEvt = function () {
            var data = $("body").data(RCVD.DocData);
            if (RCVD.CurrentProcess == CONST.transaction_type.statusUpdate || RCVD.CurrentProcess == CONST.transaction_type.promote) {
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
        var param = RCVD.GetParameters(curProcess);
        param.documentRoute.process = curProcess;
        param.documentRoute.batch_id = RCVD.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            $("#lstRoutes tbody  tr:not([rowcopy]").each(function () {
                var paramRoutes = DTS.Clone(DTS.param.documentRoute);
                paramRoutes.batch_id = RCVD.BatchId;
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
            Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, param, RCVD.DocData);
        }
    },
    MaintainAttachments: function (curProcess) {
        var postEvt = function () {
            var data = $("body").data(RCVD.AttData);
            if (data != null && data != undefined && data != "") {
                if (CONST.transaction_type.search == curProcess) {
                    $("#formAttachments .attachmentItem").remove();
                    $(data).each(function (index) {
                        var imgCls = Utilities.GetFileTypeClass(this.file_name);
                        $("#formAttachments").append(["<div status ='", this.status, "' data-toggle='tooltip' ",
                            "data-html='true' data-original-title='<b>Uploaded On</b> - ",
                            this.created_date, "<br /><b>Uploaded by</b> - ", this.updated_by_name, "' attachmentid ='", this.id,
                            "' filename='", this.file_name, "' process='", CONST.transaction_type.save, "' class='attachmentItem' style='float:left;'>",
                            "<span actualFileName='", this.file_name, "' style='display:none;'>", this.file_name, "</span>",
                            "<span class='label label-danger attachmentbadge' style='float: right;'>", index + 1, "</span>",
                            "<div class='attachmentName breakWord' attachmentName='", this.attachment_name, "'>", this.attachment_name,
                            "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name), "' ><div class='", imgCls,
                            "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                            this.barcode_no, "' ", RCVD.IsFinalize == true ? "disabled='true'" : "",
                            " /><div style='text-align:center;width:100%;'>", (RCVD.IsFinalize == true ? "" : ["<button type='button' style='visibility:",
                                (Site.UserId != this.created_by ? 'hidden' : 'visible'),
                                ";' class='removeItem btn btn-danger btn-block' multiupload>REMOVE</button>"].join('')),
                            "</div></div>",
                        ].join(''));
                    });
                    $("#formAttachments .attachmentItem").tooltip();
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("status", CONST.record_status.cancel);
                    });
                }
                else {
                    $(RCVD.Attachments).each(function () {
                        var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                        if ($(curFile).length > 0) {
                            var fileName = [$(curFile).find("span[actualFileName]").html(), $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');
                            FileUploader.UploadDocument(this.Blob, fileName, null);
                        }
                    });
                    RCVD.MaintainAttachments(CONST.transaction_type.search);
                }
            }
        };
        var hasData = false;
        var param = RCVD.GetParameters(curProcess);
        param.documentAttachment.process = curProcess;
        param.documentAttachment.batch_id = RCVD.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            $("#formAttachments .attachmentItem").each(function () {
                var paramAttachments = DTS.Clone(DTS.param.documentAttachment);
                paramAttachments.batch_id = RCVD.BatchId;
                paramAttachments.process = $(this).attr("process");
                paramAttachments.id = $(this).attr("attachmentid");
                paramAttachments.barcode_no = $(this).find("input[type='text']").val();
                if (CONST.transaction_type.save == $(this).attr("process")) {
                    paramAttachments.file_name = [$(this).find("span[actualFileName]").html(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');
                }
                else {
                    paramAttachments.file_name = $(this).attr("filename");
                }
                paramAttachments.attachment_name = $(this).find("span[actualFileName]").html();
                paramAttachments.status = RCVD.IsFinalize == true ? CONST.record_status.validated : $(this).attr("status");
                param.documentAttachments.push(paramAttachments)
                hasData = true;
            });
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentAttachments", postEvt, param, RCVD.AttData);
        }
    },
    MaintainLogs: function (curProcess, curRemarks, postFinalEvt) {
        var postEvt = function () {
            if (postFinalEvt != null || postFinalEvt != undefined) {
                postFinalEvt();
            }
            var data = $("body").data(RCVD.AttData);
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
            if (curProcess == CONST.transaction_type.save && RCVD.IsFinalize == false) {
                Site.Dialogs.Alert(["Remarks successfully added to document with batch number ", RCVD.BatchId, "!"].join(''), "Got It!", null);
            }
        };
        var hasData = false;
        var param = RCVD.GetParameters(curProcess);
        param.documentLog.process = curProcess;
        param.documentLog.batch_id = RCVD.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            var paramLogs = DTS.Clone(DTS.param.documentLog);
            paramLogs.batch_id = RCVD.BatchId;
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
            Site.PostData("/DTS/MaintainDocumentLogs", postEvt, param, RCVD.AttData);
        }
    },
}
$(function () {
    RCVD.Initialize();
});