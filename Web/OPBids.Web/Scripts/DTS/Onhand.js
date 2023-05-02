var OH = {
    RoutesComplete: null,
    AttachmentComplete: null,
    BatchId: null,
    DocData: null,
    IsReadOnly: null,
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
        param.document.process = cProc;
        param.document.id = OH.BatchId;
        param.document.dept_processed = Site.DepartmentId;
        param.document.category_id = $("#formGeneral #u_category_ids").val();
        param.document.tags = $("#formGeneral #u_tags").val();
        param.document.document_type_id = $("#formGeneral #u_document_type_ids").val();
        param.document.document_code = $("#formGeneral #u_document_code").val();
        param.document.sender_id = $("#formGeneral #u_sender_ids").val();
        param.document.receipient_id = $("#formGeneral #u_recipient_ids").val();
        param.document.etd_to_recipient = [$("#formGeneral #u_etd_to_recipient").val(), " ", $("#u_etd_to_recipient_time").val()].join('');
        param.document.delivery_type_id = $("#formGeneral #u_delivery_type_ids").val();
        param.document.document_security_level_id = $("#formGeneral #u_document_security_level_ids").val();
        param.document.created_by = Site.UserId;
        param.document.is_edoc = $("#formGeneral #rdoIsEdoc").prop("checked");
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
            OH.Attachments.push(param);
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
    ClearData: function () {
        Site.ClearAllData('#dataEntryModal');
        $("#dataEntryModal select option[value='0']").html("Please select...").attr("title", "");

        OH.BatchId = 0;
        OH.RoutesComplete = false;
        OH.AttachmentComplete = false;
        OH.Attachments = [];
        OH.CurrentProcess = CONST.transaction_type.create;
        $("#lstRoutes tbody tr:not([rowcopy]), .attachmentItem").remove();
        $("#generalTab").click();
        $("#formGeneral select:not([disabled])").each(function () {
            $(this).val($(this).find("option:first-child").attr("value"));
        });
        $("#u_category_ids").val('default').selectpicker('refresh');
        $("#u_document_type_ids").val('default').selectpicker('refresh');
        $("#u_recipient_ids").val('default').selectpicker('refresh');
        $("#u_delivery_type_ids").val('default').selectpicker('refresh');
        $("#u_document_security_level_ids").val('default').selectpicker('refresh');
        $("#formGeneral input, #formGeneral textarea").val("");
        Site.ClearAllData("#formRoutes");
    },
    Initialize: function () {
        OH.DocData = (new Date().getTime()).toString();
        OH.AttData = [OH.DocData, "-1"].join('');
        $("#pageContainer").remove();
        $("#btnTrackNewDocument").click(function () {
            OH.BatchId = 0;
            if ($(["#formGeneral #u_sender_ids option[userid='", Site.UserId, "']"].join('')).length <= 0) {
                Site.Dialogs.Alert("You cannot Track New Document. Please add yourself to the Sender/Recipient menu as Sender.", null, null);
                return;
            }
            OH.ClearData();
            $(".lblCancel").html("Cancel");
            $("#rdoIsManual").prop("checked", true);
            $("#rdoIsEdoc").prop("checked", false);
            var curOpt;
            $(["#formGeneral #u_sender_ids option[userid='", Site.UserId, "']"].join('')).each(function () {
                if ($(this).attr("email").toLowerCase() == Site.EmailAddress.toLowerCase()) {
                    curOpt = $(this).val();
                }
            });
            $("#formGeneral #u_sender_ids").val(curOpt);
            $('#dataEntryModal').modal('show');
            $("#dataEntryModal *[required]").keyup();
            $(".modal-title").html("Enter New Document Information");

            $("#formGeneral input:not(#u_sender_ids), #formGeneral textarea, #formGeneral select:not(#u_sender_ids), #lstRoutes *, #formAttachments *").removeAttr("disabled");
            $('.selectpicker').selectpicker('refresh');
            $("#btnSave, #btnAddRoute, #lstRoutes .removeItem, #footerNote").css({ "display": "" });
            $("#footerNote").css({ "display": "block" });
            $("#btnBrowse, #formAttachments .removeItem").parent().css({ "display": "" });
            $(["#formGeneral #rdoIsManual"].join('')).prop("checked", true);
            Site.DropDownCommonSettings("#u_category_ids");
            Site.DropDownCommonSettings("#u_document_type_ids");
            Site.DropDownCommonSettings("#u_recipient_ids");
            Site.DropDownCommonSettings("#u_delivery_type_ids");
            Site.DropDownCommonSettings("#u_document_security_level_ids");
            Site.GenerateControls(".dataEntryModal");
            $("#btnAddRoute").click();

        });
        $("#u_recipient_ids").change(function () {
            var deptId = $(this).find("option:selected").attr("department_id");
            var userId = $(this).find("option:selected").attr("userid");
            var ctlRoutes = $("#lstRoutes tbody tr:not([rowcopy])")[0];
            $(ctlRoutes).find("select.department_id").val(deptId).selectpicker('val', deptId).selectpicker('refresh').val(deptId);
            var selVal = $(ctlRoutes).find(["select.receipient_id option[department_id='", deptId, "'][userid='", userId, "']"].join('')).attr("value");
            $(ctlRoutes).find("select.receipient_id").val(selVal).selectpicker('val', selVal).selectpicker('refresh').val(selVal);
        });
        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                OH.ValidateAttachment(blobData, pFile[0].name);
            }
        });
        $(".document_security_level_id").change(function () {
        });
        $("input[type='radio'][name='durationOpt']").click(function () {
            $(".filterContainer #u_date_submitted_to").val($(this).attr("dateValTo"));
            $(".filterContainer #u_date_submitted_from").val($(this).attr("dateValFrom"));
        });
        $("#u_category_ids").on('changed.bs.select', function (e, clickedIndex, newValue, oldValue) {
            var ctl = $(this).next().next().find("ul li");
            var curVal = $($("#u_category_ids option")[clickedIndex]).val();
            if ($(this).val() == 0) {
                $("#u_document_type_ids option").css({ "display": "" }).removeAttr("disabled");
            }
            else {
                $("#u_document_type_ids option").css({ "display": "none" }).removeAttr("disabled").attr("disabled", "disabled");
                $(["#u_document_type_ids option[categoryid='", curVal, "']"].join('')).css({ "display": "" }).removeAttr("disabled");
            }
            $("#u_document_type_ids").val('');
            $("#u_document_type_ids").selectpicker('val', '');
            Site.DropDownCommonSettings("#u_document_type_ids");
        });
        $("#btnSave").click(function () {
            var isValid = Site.ValidateRequiredEntries("#formGeneral", null);
            if (isValid == true) {
                isValid = $("#formRoutes #lstRoutes tbody tr:not([rowcopy])").length > 0;
                if (isValid == false) {
                    Site.Dialogs.Alert("At lease one route is required.", null, null);
                    return;
                }
                if ($("#formRoutes #lstRoutes tbody tr:not([rowcopy]) .department_id:selected[value='0']").length > 0 ||
                    $("#formRoutes #lstRoutes tbody tr:not([rowcopy]) .receipient_id:selected[value='0']").length > 0) {
                    Site.Dialogs.Alert("Please make sure the department(s) and recipient(s) are filled in the 'Routes' section.", null, null);
                    return;
                }
            }
            if (isValid == true) {
                isValid = $("#formAttachments .attachmentItem").length > 0;
                if (isValid == false) {
                    Site.Dialogs.Alert("At lease one attachment is required.", null, null);
                    return;
                }
                $("#formAttachments .attachmentItem input[type='text']").each(function () {
                    if ($.trim($(this).val()) == "" && isValid == true) {
                        isValid = false;
                        Site.Dialogs.Alert("Please input a barcode on all attachments.", null, null);
                        return;
                    }
                });
            }
            if (isValid == true) {
                var param = OH.GetParameters(CONST.transaction_type.isexists);
                var postEvt = function () {
                    var data = $("body").data(OH.DocData);
                    if (data.value != null && data.value != undefined && data.value != "") {
                        Site.Dialogs.Alert("Cannot Save. Document Code already exists.", null, null);
                    }
                    else {
                        OH.MaintainData(OH.CurrentProcess, null);
                    };
                }
                Site.PostData("/DTS/MaintainDocuments", postEvt, param, OH.DocData);
            }
        });
        $("#btnAddRoute").click(function () {
            var rowcopy = $("#lstRoutes tr[rowcopy]").clone();
            $(rowcopy).attr("process", CONST.transaction_type.create).attr("routeid", "0");
            $(rowcopy).attr("status", CONST.record_status.activate);
            $("#lstRoutes tbody").append(rowcopy);
            $(rowcopy).css({ "display": "table-row" }).removeAttr("rowcopy");
            $(rowcopy).find("td:nth-child(2)").html($("#lstRoutes tbody tr[status='A']:not([rowcopy])").length);
            $(rowcopy).find(".removeItem").click(function () {
                $(this).parent().parent().remove();
                var counter = 0;
                $("#lstRoutes tbody tr[status='A']:not([rowcopy])").each(function () {
                    counter++;
                    $(this).find("td:nth-child(2)").html(counter);
                });
            });
            $(rowcopy).find("td .department_id").on('changed.bs.select', function (e, clickedIndex, newValue, oldValue) {
                var curVal = $($(this).find("option")[clickedIndex]).val();
                var optCtl = $(this).parent().parent().next().find("select.receipient_id option");
                if ($(this).val() == 0) {
                    $(this).parent().parent().next().find("select.receipient_id option").css({ "display": "" }).removeAttr("disabled");
                }
                else {
                    $(optCtl).css({ "display": "none" }).removeAttr("disabled").attr("disabled", "disabled");
                    $(optCtl).parent().find(["option[department_id='", curVal, "']"].join('')).css({ "display": "" }).removeAttr("disabled");
                }
                $(optCtl).parent().val('');
                $(optCtl).parent().selectpicker('val', '');
                Site.DropDownCommonSettings($(optCtl).parent());
            });
            $(rowcopy).find(".receipient_id, .department_id").addClass("selectpicker").selectpicker('refresh');
            $($("#lstRoutes tbody tr:not([rowcopy])")[0]).find(".removeItem").remove();
            $($("#lstRoutes tbody tr:not([rowcopy])")[0]).find("select").prop("disabled", true).selectpicker('refresh');
        });
        $("#btnBrowse").click(function () {
            $("#fleBrowse").click();
        });
        $(".ion-search").click(function () {
            OH.MaintainData(CONST.transaction_type.search, null);
        });
        $("#fleBrowse").change(function () {
            OH.ValidateAttachment(this.files[0], $(this).val());
        });
        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
            if ($(this).attr("formid") == "formGeneral") {
                $("#footerNote").html("Document information will be saved and have an auto-generated batch number.");
            }
            else if ($(this).attr("formid") == "formRoutes") {
                $("#footerNote").html("Routes will be saved and printed on the transmittal.");
            }
            else if ($(this).attr("formid") == "formAttachments") {
                $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
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
    MaintainData: function (currentProcess, cStat) {
        var param = OH.GetParameters(currentProcess);
        if (cStat != null && cStat != "") {
            param.document.status = cStat;
        }
        var postEvt = function () {
            var data = $("body").data(OH.DocData);
            $("#grdLst tbody").html('');
            Site.FillPagingValues(data.page_count, false);
            $(".totalDocs").html(data.total_count);
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    if (currentProcess == CONST.transaction_type.create) {
                        OH.BatchId = this.id;
                    }
                    $("#grdLst tbody").append(["<tr document_security_level_id='", this.document_security_level_id,
                        "' delivery_type_id='", this.delivery_type_id, "' itemid='", this.id,
                        "'><td number>", this.id, "</td><td category_id='", this.category_id, "'>", this.category_code,
                        "</td><td document_type_id='", this.document_type_id, "' is_edoc=", this.is_edoc, ">", this.document_type_name, "</td><td document_code>", this.document_code,
                        "</td><td sender_id='", this.sender_id, "'>", this.sender_name, "</td><td recipient_id='", this.receipient_id, "'>",
                        this.receipient_name, "</td><td sent_date date>", Site.FixDateString(this.created_date),
                        "</td><td etd_to_recipient date>", Site.FixDateString(this.etd_to_recipient),
                        "</td><td style='text-align:center;' number>", Site.DaysPast(this.created_date),
                        "<span tags style='display:none;'>", this.tags, "</span></td><td style='text-align:center;'>", DTS.ContextMenu,
                        "</td></tr>"].join(''));
                });
                if (currentProcess == CONST.transaction_type.promote) {
                    Site.Dialogs.Alert(["Document with Batch No: ", OH.BatchId, " has been successfully submitted to next recipient. Thank you!"].join(''), "Got It!", null);
                }
                if (currentProcess == CONST.transaction_type.create) {
                    DTS.MaintainLogs(CONST.transaction_type.save, OH.BatchId, [Site.UserName, ' added document for tracking'].join(''));
                    Site.Dialogs.Alert(["New document tracking successfully saved under batch number ", OH.BatchId, ". You can now view the document information under On-Hand List"].join(''), "Got It!", null);
                }
                $("#grdLst tbody tr").each(function () {
                    $(this).find(".itemName").html([$(this).find("td:first-child").html(), ' - ', $(this).find("td[category_id]").html()].join(''));
                    $(this).find(".subItemName").html($(this).find("td[document_code]").html());
                    $(this).find(".contextItemContainer").html([
                        "<li><a class='dropdown-item item-setting-inactive lnkView' href='#' itemid='", $(this).attr("itemid"), "'>View Document Information</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkEdit' href='#' itemid='", $(this).attr("itemid"), "'>Edit Document Information</a></li>",
                        ($(this).find("td[is_edoc]").attr("is_edoc") == 'true' ? ["<li><a class='dropdown-item item-setting-inactive lnkSubmitToNextRecipient' href='#' itemid='", $(this).attr("itemid"), "'>Submit to Next Recipient</a></li>"].join('') : ""),
                        "<li><a class='dropdown-item item-setting-inactive lnkCancel' href='#' itemid='", $(this).attr("itemid"), "'>Cancel Document</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkPrint' href='#' itemid='", $(this).attr("itemid"), "'>Print Transmittal</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkRoutine' href='#' itemid='", $(this).attr("itemid"), "'>Print Routine slip</a></li>",
                    ].join(''));
                });
                $("#grdLst tbody tr .lnkEdit").unbind("click");
                $("#grdLst tbody tr .lnkEdit").click(function () {
                    OH.BatchId = $(this).attr("itemid");
                    OH.EditViewPopUp(this, false);
                    Site.GenerateControls(".dataEntryModal");
                    Site.DropDownCommonSettings("#u_category_ids");
                    Site.DropDownCommonSettings("#u_document_type_ids");
                    Site.DropDownCommonSettings("#u_recipient_ids");
                    Site.DropDownCommonSettings("#u_delivery_type_ids");
                    Site.DropDownCommonSettings("#u_document_security_level_ids");
                });
                $("#grdLst tbody tr .lnkSubmitToNextRecipient").unbind("click");
                $("#grdLst tbody tr .lnkSubmitToNextRecipient").click(function () {
                    OH.BatchId = $(this).attr("itemid");
                    var paramRoutes = DTS.Clone(DTS.param);
                    paramRoutes.documentRoute.batch_id = OH.BatchId;
                    paramRoutes.documentRoute.process = CONST.transaction_type.search;
                    var postEvt = function () {
                        var data = $("body").data(OH.DocData);
                        var cntnt = [];
                        cntnt.push("<table><tr><td style='vertical-align:top;padding-right:10px;'><label class='control-label'>Recipient:</label></td><td><select type='select-one' id='cboReceipientPopUp' style='width:100%;'>");
                        $(data).each(function () {
                            cntnt.push(['<option value="', this.recipient_id, '">', this.receipient_name, '</option>'].join(''));
                        });
                        cntnt.push("</select><br/><span style='font-size:12px;'>The names on the list is based on the set Routes for the Document.</span></td></tr></table>");
                        var yesEvt = function () {
                            OH.CurrentProcess = CONST.transaction_type.promote;
                            OH.MaintainData(OH.CurrentProcess, null);
                            var postEvt = function () {
                                window.location.reload();
                            };
                            var routeFlows = [];
                            $("#cboReceipientPopUp option").each(function () {
                                routeFlows.push($(this).html());
                            });
                            var param = DTS.Clone(DTS.param);
                            param.userNotification.process = CONST.transaction_type.create;
                            param.userNotification.message = encodeURI(["<b>", Site.UserName, "</b> sent you an e-Doc with Batch #: <b>", OH.BatchId, "</b>, with Doc Code <b>",
                                $(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[document_code]"].join('')).html(), "</b>, created by <b>",
                                Site.UserName, "</b> going to <b>", $("#cboReceipientPopUp").val(), "</b> w/ ETD <b>",
                                $(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[etd_to_recipient]"].join('')).html(), "</b><br /><b>Routes : </b>", routeFlows.join(' -> ')].join(''));
                            param.userNotification.recipient_ids = $("#cboReceipientPopUp").val();
                            param.userNotification.sender_id = Site.UserId;
                            Site.AddNotification(param, postEvt);
                        }
                        Site.Dialogs.InputGeneric("Submit to Next Recipient", cntnt.join(''), "Submit", "Cancel", yesEvt, null);
                    }
                    Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, paramRoutes, OH.DocData);
                });
                $("#grdLst tbody tr .lnkCancel").unbind("click");
                $("#grdLst tbody tr .lnkCancel").click(function () {
                    var curId = $(this).attr("itemid");
                    var postEvt = function () {
                        OH.BatchId = curId;
                        OH.CurrentProcess = CONST.transaction_type.statusUpdate;
                        OH.MaintainData(OH.CurrentProcess, CONST.record_status.cancel);
                        $(".ion-search").click();
                    }
                    Site.Dialogs.Confirm("Warning. You are about to Cancel a Document", "Are you sure you want to cancel this document?", "Yes", "No", postEvt, null);

                });
                $("#grdLst tbody tr .lnkView").unbind("click");
                $("#grdLst tbody tr .lnkView").click(function () {
                    OH.EditViewPopUp(this, true);
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
            if (currentProcess == CONST.transaction_type.create || currentProcess == CONST.transaction_type.update) {
                OH.MaintainRoutes(CONST.transaction_type.save);
                OH.MaintainAttachments(CONST.transaction_type.save);
            }
        };
        Site.PostData("/DTS/MaintainDocuments", postEvt, param, OH.DocData);
    },
    EditViewPopUp: function (ctl, isreadOnly) {
        OH.ClearData();
        OH.BatchId = $(ctl).attr("itemid");
        OH.CurrentProcess = CONST.transaction_type.update;
        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();
        $("#generalTab").click();
        var dte = $(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[etd_to_recipient]"].join('')).html().replace(/  /g, ' ');
        $("#formGeneral #u_category_ids").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[category_id]"].join('')).attr("category_id"));
        $("#formGeneral [data-id='u_category_ids'] .filter-option-inner-inner").html($("#formGeneral #u_category_ids option:selected").html());
        $("#formGeneral #u_document_type_ids").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[document_type_id]"].join('')).attr("document_type_id"));
        $("#formGeneral [data-id='u_document_type_ids'] .filter-option-inner-inner").html($("#formGeneral #u_document_type_ids option:selected").html());
        $("#formGeneral #u_document_code").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[document_code]"].join('')).html());
        $("#formGeneral #u_sender_ids").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[sender_id]"].join('')).attr("sender_id"));
        $("#formGeneral [data-id='u_sender_ids'] .filter-option-inner-inner").html($("#formGeneral #u_sender_ids option:selected").html());
        $("#formGeneral #u_recipient_ids").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[recipient_id]"].join('')).attr("recipient_id"));
        $("#formGeneral [data-id='u_recipient_ids'] .filter-option-inner-inner").html($("#formGeneral #u_recipient_ids option:selected").html());
        $("#formGeneral #u_etd_to_recipient").val($.datepicker.formatDate("M dd yy", new Date(dte.split(' ')[0])));
        $("#formGeneral #u_tags").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "'] span[tags]"].join('')).text());
        $("#formGeneral #u_etd_to_recipient_time").val(Site.FormatTime(dte.split(' ')[1]));
        $("#formGeneral #u_delivery_type_ids").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "']"].join('')).attr("delivery_type_id"));
        $("#formGeneral [data-id='u_delivery_type_ids'] .filter-option-inner-inner").html($("#formGeneral #u_delivery_type_ids option:selected").html());
        $("#formGeneral #u_document_security_level_ids").val($(["#grdLst tbody tr[itemid='", OH.BatchId, "']"].join('')).attr("document_security_level_id"));
        $("#formGeneral [data-id='u_document_security_level_ids'] .filter-option-inner-inner").html($("#formGeneral #u_document_security_level_ids option:selected").html());
        $("#formGeneral #u_document_security_level_ids").change();
        OH.IsReadOnly = isreadOnly;
        if (isreadOnly == true) {
            $("h4.modal-title").html("View Existing Document Information");
            $("#addfile").parent().css({ "display": "none" });
            $(".lblCancel").html("Close");
        }
        else {
            $("h4.modal-title").html("Edit Existing Document Information");
            $("#addfile").parent().css({ "display": "" });
            $(".lblCancel").html("Cancel");
        }
        OH.MaintainRoutes(CONST.transaction_type.search);
        OH.MaintainAttachments(CONST.transaction_type.search);
        $("#formGeneral textarea, #formGeneral input:not(#u_sender_ids), #formGeneral select:not(#u_sender_ids), #lstRoutes *, #formAttachments *").removeAttr("disabled").attr("disabled", isreadOnly).keyup();
        $('.selectpicker').selectpicker('refresh');
        $("#btnSave, #btnAddRoute, #lstRoutes .removeItem, #footerNote").css({ "display": isreadOnly == true ? "none" : "" });
        $("#footerNote").css({ "display": "none" });
        $("#btnBrowse, #formAttachments .removeItem").parent().css({ "display": isreadOnly == true ? "none" : "" });
        $(["#formGeneral #", $(["#grdLst tbody tr[itemid='", OH.BatchId, "'] td[is_edoc]"].join('')).attr("is_edoc") == "true" ? "rdoIsEdoc" : "rdoIsManual"].join('')).prop("checked", true);
    },
    ProcessComplete: function () {
        if (OH.RoutesComplete == true && OH.AttachmentComplete == true) {
            $('#dataEntryModal').modal('hide');
            $(".ion-search").click();
        }
    },
    MaintainRoutes: function (curProcess) {
        var postEvt = function () {
            OH.RoutesComplete = true;
            var data = $("body").data(OH.DocData);
            if (OH.CurrentProcess == CONST.transaction_type.statusUpdate || OH.CurrentProcess == CONST.transaction_type.promote) {
                $(".ion-search").click();
            }
            else {
                $("#lstRoutes tbody tr:not([rowcopy])").remove();
                if (data != null && data != undefined && data != "") {
                    var counter = 0;
                    $(data).each(function () {
                        counter++;
                        var rowcopy = $("#lstRoutes tr[rowcopy]").clone();
                        $(rowcopy).attr("status", this.status);
                        $(rowcopy).attr("process", CONST.transaction_type.update).attr("routeid", this.id);
                        $(rowcopy).css({ "display": "table-row" }).removeAttr("rowcopy");
                        $(rowcopy).find("td:nth-child(2)").html(counter);

                        $(rowcopy).find("td .department_id").on('changed.bs.select', function (e, clickedIndex, newValue, oldValue) {
                            var curVal = $($(this).find("option")[clickedIndex]).val();
                            var optCtl = $(this).parent().parent().next().find("select.receipient_id option");
                            if ($(this).val() == 0) {
                                $(this).parent().parent().next().find("select.receipient_id option").css({ "display": "" }).removeAttr("disabled");
                            }
                            else {
                                $(optCtl).css({ "display": "none" }).removeAttr("disabled").attr("disabled", "disabled");
                                $(optCtl).parent().find(["option[department_id='", curVal, "']"].join('')).css({ "display": "" }).removeAttr("disabled");
                            }
                            $(optCtl).parent().val('');
                            $(optCtl).parent().selectpicker('val', '');
                            Site.DropDownCommonSettings($(optCtl).parent());
                        });

                        $(rowcopy).find(".department_id").val(this.department_id);
                        $(rowcopy).find(".department_id").change();
                        $(rowcopy).find(".receipient_id option").css({ "display": "none" }).removeAttr("disabled").attr("disabled", "disabled");
                        $(rowcopy).find(".receipient_id").find(["option[department_id='", this.department_id, "']"].join('')).css({ "display": "" }).removeAttr("disabled");
                        $(rowcopy).find(".receipient_id").val($(rowcopy).find([".receipient_id option[userid='", this.receipient_id, "']"].join('')).attr("value"));
                        $(rowcopy).find(".receipient_id, .department_id").addClass("selectpicker").selectpicker('refresh');
                        $("#lstRoutes tbody").append(rowcopy);
                    });
                    $($("#lstRoutes tbody tr:not([rowcopy])")[0]).find(".removeItem").remove();
                    $($("#lstRoutes tbody tr:not([rowcopy])")[0]).find("select").prop("disabled", true).selectpicker('refresh');
                }
                $("#lstRoutes tbody tr:not([rowcopy]) .removeItem").unbind("click");
                $("#lstRoutes tbody tr:not([rowcopy]) .removeItem").click(function () {
                    $(this).parent().parent().attr("status", CONST.record_status.cancel).css({ "display": "none" });
                    var counter = 0;
                    $("#lstRoutes tbody tr[status='A']:not([rowcopy])").each(function () {
                        counter++;
                        $(this).find("td:nth-child(2)").html(counter);
                    });
                });
            }
            if (curProcess != CONST.transaction_type.search) {
                OH.ProcessComplete();
            }
        };
        var hasData = false;
        var param = OH.GetParameters(curProcess);
        param.documentRoute.process = curProcess;
        param.documentRoute.batch_id = OH.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            $("#lstRoutes tbody  tr:not([rowcopy]").each(function () {
                var paramRoutes = DTS.Clone(DTS.param.documentRoute);
                paramRoutes.batch_id = OH.BatchId;
                paramRoutes.process = $(this).attr("process");
                paramRoutes.id = $(this).attr("routeid");
                paramRoutes.department_id = $(this).find("select.department_id").val();
                paramRoutes.department_name = null;
                paramRoutes.receipient_id = $(this).find("select.receipient_id option:selected").attr("userid");
                paramRoutes.receipient_name = null;
                paramRoutes.status = $(this).attr("status");
                param.documentRoutes.push(paramRoutes)
                hasData = true;
            });
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, param, OH.DocData);
        }
    },
    MaintainAttachments: function (curProcess) {
        var postEvt = function () {
            var data = $("body").data(OH.AttData);
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
                            "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name), "' ><div class='",
                            imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                            this.barcode_no, "' ", OH.IsReadOnly == true ? "disabled='true'" : "",
                            " /><div style='text-align:center;width:100%;'>", (OH.IsReadOnly == true ? "" : ["<button type='button' style='visibility:",
                                (Site.UserId != this.created_by ? 'hidden' : 'visible'),
                                ";' class='removeItem btn btn-danger btn-block' multiupload>REMOVE</button>"].join('')),
                            "</div></div>",
                        ].join(''));
                    });
                    $("#formAttachments .attachmentItem").tooltip();
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("status", CONST.record_status.cancel);
                        var counter = 0;
                        $(".attachmentbadge").each(function (index) {
                            if ($(this).is(":visible")) {
                                counter++
                            }
                            $(this).html(counter);
                        });
                    });
                }
                else {
                    $(OH.Attachments).each(function () {
                        var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                        var fileName = [$(curFile).find("span[actualFileName]").html(), $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');
                        FileUploader.UploadDocument(this.Blob, fileName, null);
                    });
                    OH.AttachmentComplete = true;
                    OH.ProcessComplete();
                }
            }
        };
        var hasData = false;
        var param = OH.GetParameters(curProcess);
        param.documentAttachment.process = curProcess;
        param.documentAttachment.batch_id = OH.BatchId;
        if (CONST.transaction_type.save == curProcess) {
            $("#formAttachments .attachmentItem").each(function () {
                var paramAttachments = DTS.Clone(DTS.param.documentAttachment);
                paramAttachments.batch_id = OH.BatchId;
                paramAttachments.process = $(this).attr("process");
                paramAttachments.id = $(this).attr("attachmentid");
                paramAttachments.barcode_no = $(this).find("input[type='text']").val();
                paramAttachments.created_by = Site.UserId;

                console.log("CONST.transaction_type : " + CONST.transaction_type)

                if (CONST.transaction_type.save == $(this).attr("process")) {
                    //paramAttachments.file_name = [$(this).find("span[actualFileName]").html(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');
                    paramAttachments.file_name = [$(this).find("span[actualFileName]").html()];
                }
                else {
                    paramAttachments.file_name = $(this).attr("filename");
                }
                //paramAttachments.attachment_name = $(this).find("span[actualFileName]").html();
                //attachmentName
                paramAttachments.attachment_name = $(this).find("div[attachmentName]").html();

                paramAttachments.status = $(this).attr("status");
                param.documentAttachments.push(paramAttachments)
                hasData = true;
            });
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/DTS/MaintainDocumentAttachments", postEvt, param, OH.AttData);
        }
    },
}
$(function () {
    OH.Initialize();
});