var LOP = {
    UserId: null,
    ProjectId: null,
    CurrentData: null,
    AttData: null,
    IsFinalize: false,
    IsEnableEdit: null,
    CurrentProcess: null,
    Attachments: [],
    SearchParam: null,
    GetParameters: function (cProc) {
        var param = Supplier.Clone(Supplier.param);
        param.process = cProc;
        param.menu_id = localStorage.getItem("sub-menu-id");
        param.filter.created_by = Site.UserId;
        param.filter.page_index = $(".paging").val();
        param.filter.updated_by = Site.UserId;
        param.filter.submitted_from = $("#search_submit_from").val();
        param.filter.submitted_to = $("#search_submit_to").val();
        param.filter.required_from = $("#search_required_from").val();
        param.filter.required_to = $("#search_required_to").val();
        param.filter.budget_min = $("#search_budget_min").val().replace(',', '');
        param.filter.budget_max = $("#search_budget_max").val().replace(',', '');
        param.filter.grantee = $("#search_grantee").val();
        param.filter.category = $("#search_category").val();
        param.filter.project_name = $("#search_project_name").val();
        param.filter.RefNo = $("#search_draft_no").val();
        param.page_index = $(".paging").val();
        param.supplier.amount = $("#u_bid_amount").val();
        param.supplier.project_duration = $("#u_project_duration").val();
        param.supplier.bid_bond = $("#u_bid_bond").val();
        param.supplier.id = LOP.ProjectId;
        param.supplier.created_by = Site.UserId;
        param.supplier.updated_by = Site.UserId;
        param.documentAttachment.project_id = LOP.ProjectId;
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
            var param = Supplier.Clone(Supplier.AttachmentParam);
            param.GUID = (new Date().getTime()).toString();
            param.Blob = blob;
            attachmentName = [param.GUID, "_", attachmentName].join('');
            LOP.Attachments.push(param);
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
            Site.Dialogs.Alert("Invalid file extension, please upload a file with jpg, png, gif, doc, pdf, docx, xls, xlsx, or txt file exntesions.", "OK", null);
        }
        $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
    },
    Initialize: function () {
        LOP.CurrentData = (new Date().getTime()).toString();
        LOP.AttData = [LOP.CurrentData, "-1"].join('');
        $("#pageContainer").remove();
        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                LOP.ValidateAttachment(blobData, pFile[0].name);
            }
        });
        Site.GenerateControls("#formGeneral");
        $("input[type='radio'][name='durationOpt']").click(function () {
            $(".search-table #search_submit_to").val($(this).attr("dateValTo"));
            $(".search-table #search_submit_from").val($(this).attr("dateValFrom"));
        });
        $("#btnSave").click(function () {
            $('#dataEntryModal').modal('hide');
            var yesEvt = function () {
                LOP.MaintainData(CONST.transaction_type.save, CONST.project_substatus.closed_shortlist);
            }
            var noEvt = function () {
                $('#dataEntryModal').modal('show');
            }
            Site.Dialogs.Confirm("", "Are you sure you want to submit bid for the project?", "Yes", "No", yesEvt, noEvt);
        });
        $(".lstCollapser thead tr[collapse]").click(function () {
            $(this).attr("collapse", $(this).attr("collapse") == "1" ? "0" : "1");
            $(this).parent().parent().find("tbody").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
            $(this).parent().parent().find("thead tr:not([collapse])").css({ "display": ($(this).attr("collapse") == "1" ? "none" : "") });
        });
        $("#btnBrowse").click(function () {
            $("#fleBrowse").click();
        });
        $("#btnSearch").click(function () {
            LOP.SearchParam = LOP.GetParameters(CONST.transaction_type.search, null);
            LOP.MaintainData(CONST.transaction_type.search, null);
        });
        $("#fleBrowse").change(function () {
            LOP.ValidateAttachment(this.files[0], $(this).val());
        });
        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
            if ($(this).attr("formid") == "formGeneral") {
                $("#footerNote").html("Bid will not be valid until bid documents are submitted to the BAC SEC office.");
            }
            else if ($(this).attr("formid") == "formAttachments") {
                $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
            }
        });
        $("#generalTab").click();
        $("input[type='radio'][name='durationOpt'][value='0']").click();
        window.sessionStorage.removeItem("optSearch");
        $("#btnSearch").click();

        $(document).on('click touchend', function (e) {
            $(".setting-cmd").hide();
        });
    },
    MaintainData: function (currentProcess, cStat) {
        var param = LOP.SearchParam; //LOP.GetParameters(currentProcess);
        if (cStat != null && cStat != "") {
            param.supplier.status = cStat;
        }
        var postEvt = function () {
            var data = $("body").data(LOP.CurrentData);
            $("#grdLst tbody").html('');
            //Site.FillPagingValues(data.page_count, false);
            LOP.SetPagination(data.page_index, data.total_count);
            $(".totalDocs").html(data.total_count);
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    $("#grdLst tbody").append(["<tr itemid='", this.id, "'><td number>", this.index, "</td><td number>", this.ref_no,
                        "</td><td category>", this.category, "</td><td date_submitted>", this.date_submitted,
                        "</td><td project>", this.project, "</td><td amount class='text-right' style='color:#0bce09'>₱ ", this.amount, "</td><td status>", this.status,
                        "</td><td>", Supplier.ContextMenu, "</td><td approved_budget style='display:none;'>",
                        this.approved_budget, "</td><td bid_bond style='display:none;'>", this.bid_bond,
                        "</td><td project_desc style='display:none;'>", this.project_desc,
                        "</td><td deadline style='display:none;'>", Site.FixDateString(this.deadline),
                        "</td><td project_duration style='display:none;'>", this.project_duration, "</td></tr>"].join(''));
                });
                if (currentProcess == CONST.transaction_type.save) {
                    Site.Dialogs.Alert(["Bid successfully submitted. Please submit required bid documents to the BAC SEC Office on or before the deadline of submission of bids. Thank you."].join(''), "Got It!", null);
                    LOP.MaintainAttachments(CONST.transaction_type.save);
                }
                $("#grdLst tbody tr").each(function () {
                    $(this).find(".itemName").html($(this).find("td[project]").html());
                    $(this).find(".subItemName").html($(this).find("td[status]").html());
                    $(this).find(".contextItemContainer").html([
                        "<li><a class='dropdown-item item-setting-inactive lnkView' href='#' itemid='", $(this).attr("itemid"), "'>View Project Information</a></li>",
                        "<li><a class='dropdown-item item-setting-inactive lnkViewBid' href='#' itemid='", $(this).attr("itemid"), "'>View Project Bid Information</a></li>",
                    ].join(''));

                });
                $("#grdLst tbody tr .lnkViewBid").unbind("click");
                $("#grdLst tbody tr .lnkViewBid").click(function () {
                });

                $("#grdLst tbody tr .lnkView").unbind("click");
                $("#grdLst tbody tr .lnkView").click(function () {
                    LOP.EnableEdit(false);
                    LOP.PopUpModal($(this).attr("itemid"));
                    $("#btnRemarks").css({ "display": "inline-block" });
                    $("#btnFinalize").css({ "display": "none" });
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
            }
        };
        Site.PostData("/Supplier/MaintainSupplier", postEvt, param, LOP.CurrentData);
    },
    EnableEdit: function (opt) {
        LOP.IsEnableEdit = opt;
        if (opt == true) {
            $("#u_bid_amount, #u_project_duration, #u_bid_bond, .removeItem").removeAttr("disabled");
            $("#btnCancel, #btnSave").css({ "display": "" });
            $("#btnBrowse").parent().css({ "display": "" });
        }
        else {
            $("#u_bid_amount, #u_project_duration, #u_bid_bond, .removeItem").attr("disabled", true);
            $("#btnCancel, #btnSave").css({ "display": "none" });
            $("#btnBrowse").parent().css({ "display": "none" });
        }
    },
    PopUpModal: function (itemId) {
        LOP.ProjectId = itemId;
        LOP.CurrentProcess = CONST.transaction_type.update;
        $("#u_project_description").html($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[project_desc]"].join('')).html());
        $("#u_approved_budget").html($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[approved_budget]"].join('')).html());
        $("#u_status").html($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[status]"].join('')).html());
        $("#u_category").html($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[category]"].join('')).html());
        $("#u_deadline").html($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[deadline]"].join('')).html());
        $("#u_bid_amount").val($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[amount]"].join('')).html());
        $("#u_project_duration").val($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[project_duration]"].join('')).html());
        $("#u_bid_bond").val($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[bid_bond]"].join('')).html());
        $("#generalTab").click();
        $(".modal-title").html($(["#grdLst tbody tr[itemid='", LOP.ProjectId, "'] td[document_code]"].join('')).html());
        $("#dataEntryModal input").keyup();
        $('#dataEntryModal').modal('show');
        LOP.MaintainAttachments(CONST.transaction_type.search);
        LOP.GetProjectItems(itemId);
    },
    MaintainAttachments: function (curProcess) {
        var postEvt = function () {
            var data = $("body").data(LOP.AttData);
            $("#formAttachments .attachmentItem").remove();
            if (data != null && data != undefined && data != "") {
                if (CONST.transaction_type.search == curProcess) {
                    $(data).each(function () {
                        var imgCls = Utilities.GetFileTypeClass(this.file_name);
                        $("#formAttachments").append(["<div status ='", this.status, "' attachmentid ='", this.id,
                            "' filename='", this.file_name, "' process='", CONST.transaction_type.save, "' class='attachmentItem' style='float:left;'>",
                            "<span actualFileName style='display:none;'>", this.attachment_name, "</span>",
                            "<div class='attachmentName breakWord'>", this.attachment_name.substring(this.attachment_name.indexOf('_') + 1),
                            "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name),
                            "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                            this.barcode_no, "' ", (this.status == CONST.record_status.validated || LOP.IsEnableEdit == false ? "disabled='disabled'" : ""),
                            " />", (this.status == CONST.record_status.validated || LOP.IsEnableEdit == false ? "" :
                            "<div style='text-align:center;width:100%;'><button type='button' class='removeItem'></button></div>"),
                            "</div>"].join(''));
                    });
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("status", CONST.record_status.cancel);
                    });
                }
                else {
                    $(LOP.Attachments).each(function () {
                        var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                        if ($(curFile).length > 0) {
                            var fileName = [$(curFile).find("span[actualFileName]").html(), $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');
                            FileUploader.UploadDocument(this.Blob, fileName, null);
                        }
                    });
                    LOP.MaintainAttachments(CONST.transaction_type.search);
                }
            }
        };
        var hasData = false;
        var param = LOP.GetParameters(curProcess);
        param.documentAttachment.process = curProcess;
        param.documentAttachment.project_id = LOP.ProjectId;
        if (CONST.transaction_type.save == curProcess) {
            $("#formAttachments .attachmentItem").each(function () {
                var paramAttachments = Supplier.Clone(Supplier.param.documentAttachment);
                paramAttachments.project_id = LOP.ProjectId;
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
                paramAttachments.status = $(this).attr("status");
                param.documentAttachments.push(paramAttachments)
                hasData = true;
            });
        }
        if (CONST.transaction_type.search == curProcess) {
            hasData = true;
        }
        if (hasData == true) {
            Site.PostData("/Supplier/MaintainProjectAttachments", postEvt, param, LOP.AttData);
        }
    },
    GetProjectItems: function (project_id) {

        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': project_id,
        });
        var _result;

        ajaxHelper.Invoke("/ProjectRequest/ProjectItems", _param, "json", function (result) {
            if (result) {
                var $itemsTable = $("#projectRequestItems #projectItems tbody");

                $itemsTable.empty();

                var grandTotal = 0.00;
                //form-control text-box single-line
                var ctr = 1;
                $(result).each(function () {
                    var tableScope = $itemsTable;

                    var itemTotal = (parseInt(this.quantity.replace(/,/g, '')) * parseFloat(this.unit_cost.replace(/,/g, '')));
                    grandTotal += itemTotal;

                    tableScope.append(["<tr class='item'>",
                        "<td>" + ctr++ + "</td>",
                        "<td>" + this.unit + "</td>",
                        "<td>" + this.description + "</td>",
                        "<td class='text-right'>" + this.quantity + "</td>",
                        "<td class='text-right'>" + this.unit_cost + "</td>",
                        "<td class='text-right'>" + Site.ToDecimalString(itemTotal) + "</td>",
                        "</tr > "].join(''));
                });

                Site.GenerateControls($itemsTable);

                $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(grandTotal));

            }
        });
    },
    SetPagination: function (page_index, total_records) {//called from result view
        if (total_records > 0) {
            var total_pages = Math.ceil(total_records / Site.PageItemCount);
            var options = {
                bootstrapMajorVersion: 3,
                currentPage: page_index,
                totalPages: total_pages,
                numberOfPages: 10,
                alignment: 'right',
                onPageClicked: function (e, originalEvent, type, page) {
                    e.stopImmediatePropagation();
                    $("#processId").html(CONST.transaction_type.search);
                    LOP.SearchParam.page_index = page - 1;
                    LOP.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainerDocument');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                $("#processId").html(CONST.transaction_type.search);
                LOP.SearchParam.page_index = this.value - 1;
                LOP.MaintainData(CONST.transaction_type.search, null, null, null);
            });

            pageSelect.empty();
            var options = '';
            for (var i = 1; i <= total_pages; i++) {
                options += '<option value="' + i + '" ' + (i == page_index ? 'selected' : '') + '>' + i + '</option>';
            }
            pageSelect.append(options);
            pageTotal.text(total_pages);

        } else {
            var pageContainer = $('#pageContainerDocument');
            var pageSelect = pageContainer.find('.paging');

            pageContainer.hide();
            pageSelect.empty();
        }
    }
}
$(function () {
    LOP.Initialize();
});