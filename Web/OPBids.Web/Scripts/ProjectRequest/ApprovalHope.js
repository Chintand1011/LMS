var APP_HOPE = {
    Status: null,
    EnableAttachments: false,
    UploadSubFolder: 'ProjectRequest',//subfolder in upload folder
    Initialize: function () {
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());        

        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#toolbarContent .toolbar-add").hide();
        $("#toolbarContent .toolbar-delete").hide();

        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            APP_HOPE.PopulateModal(this);
        });

        $(".item-setting-approve").unbind("click");
        $(".item-setting-approve").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            APP_HOPE.PopulateModal(this);
        });

        $(".item-setting-return").unbind("click");
        $(".item-setting-return").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.return);
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#modal-note-title").text("Batch Return Note");
            $("#ProjectRequestNoteModal").modal("show");
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


        $("#btnSearch").unbind("click");
        $("#btnSearch").click(function (e) {
            e.preventDefault();
            $("#processId").html("");

            Site.ResetTempData();

            PROJECT.SearchParam.page_index = 1;

            PROJECT.SearchParam.duration_scope = $("input:radio[name='duration']:checked").val();
            PROJECT.SearchParam.submitted_from = $("#search_submit_from").val();
            PROJECT.SearchParam.submitted_to = $("#search_submit_to").val();
            PROJECT.SearchParam.project_name = $("#search_project_name").val();
            PROJECT.SearchParam.id = $("#search_ref_no").val();

            PROJECT.SearchParam.applicable_year = $("#search_app_year").val();
            PROJECT.SearchParam.batch_id = $("#search_batch_id").val();
            PROJECT.SearchParam.barcode = $("#search_barcode").val();

            PROJECT.SearchParam.get_total = true;
            APP_HOPE.MaintainData(CONST.transaction_type.search, null, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });


        $("#dataEntryModal #btnApprove").unbind("click");
        $("#dataEntryModal #btnApprove").click(function () {
            $("#processId").html(CONST.process_id.submit);
            $("#ProjectRequestNoteModal #footerNote").text("");
            $("#modal-note-title").text("Batch Approve Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $("#dataEntryModal #btnReject").unbind("click");
        $("#dataEntryModal #btnReject").click(function () {
            $("#processId").html(CONST.process_id.reject);
            $("#modal-note-title").text("Batch Reject Note");
            $("#ProjectRequestNoteModal #footerNote").empty();
            $("#ProjectRequestNoteModal #footerNote").append("<i class='fa fa-exclamation-triangle' style='color:red'></i><span> Batch will be deemed <b>Closed</b> and <span style='color: red'><b>Rejected</b></span></span>");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $("#ProjectRequestNoteModal #btnSaveNoteModal").click(function () {
            if (Site.ValidateRequiredEntries("#ProjectRequestNoteModal", null) == true) {
                var _txn, _record_status, _user_action;
                if ($("#processId").text() === CONST.process_id.submit) {
                    _txn = CONST.transaction_type.processUpdate;
                    _user_action = CONST.user_action.approve;
                } else if ($("#processId").text() === CONST.process_id.return) {
                    _txn = CONST.transaction_type.processUpdate;
                    _user_action = CONST.user_action.return;
                } else if ($("#processId").text() === CONST.process_id.reject) {
                    _txn = CONST.transaction_type.statusUpdate;
                    _record_status = CONST.record_status.reject;
                }
                APP_HOPE.MaintainData(_txn, _record_status, _user_action, null, $(".dr-notes").val());
            }
        });

        Site.GenerateControls("#dataEntryModal");
        Site.GenerateControls("#ProjectRequestNoteModal");
    },

    PopulateModal: function (_this) {

        $("#dataEntryModal #btnApprove").hide();
        $("#dataEntryModal #btnReject").hide();
        $("#dataEntryModal #btnCancel").hide();

        switch ($("#processId").text()) {
            case CONST.process_id.submit:
                $("#dataEntryModal #btnReject").show();
                $("#dataEntryModal #btnApprove").show();
                break;
            default:
                break;
        }

        var _id = $(_this).attr("data-itemid");
        if (_id !== null || _id !== undefined) {
            $("#u_id").val(_id);
            $("#modal_batch_id").html(_id);
            $("#modal_proc_method").html($("#row-" + _id).data("procurement-method-desc"));
            $("#modal_proc_status").html($("#row-" + _id).data("project-substatus-desc"));
            $("#batch_total_projects").html($("#row-" + _id).data("total-project"));
            $("#batch_total_amount").html($("#row-" + _id).data("total-amount"));
            $("#batch_date_created").html($("#row-" + _id).data("created-date") + " (" + $("#row-" + _id).data("created-by") + ")");
        } else {
            $("#u_id").val("0");
        }

        APP_HOPE.GetProjectList($("#u_id").val());

        APP_HOPE.GetAttachments(_id);

        $('#dataEntryModal').modal('show');
    },

    GetProjectList: function (id) {
        var _result;
        var _search = {
            'batch_id': id
        };
        var _filter = JSON.stringify({
            'projectSearch': _search
        });
        ajaxHelper.Invoke("/ProjectRequest/GetBatchProjectRequestList", _filter, "json", handleGetProjectList(_result));
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;

        var search = {
            //'submitted_from': $("#search_submit_from").data('datepicker').getformatteddate('dd-m-yyyy'),
            //'submitted_to': $("#search_submit_to").data('datepicker').getformatteddate('dd-m-yyyy'),
            //'project_name': $("#search_project_name").val()
        };

        if ($("#processId").text() !== "") {
            var param = {
                'id': $("#u_id").val(),
                'record_status': record_status,
                'user_action': user_action,
                'notes': notes
            };
        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequestBatch': param
        });


        if ($("#processId").html() === CONST.process_id.submit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch has been successfully Approved!"));
        } else if ($("#processId").html() === CONST.process_id.return) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch #{value} has been returned to BAC SEC for rechecking and additional updates.", null, true));
        } else if ($("#processId").html() === CONST.process_id.reject) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch has been rejected. All projects under batch can be viewed under Rejected Menu."));
        }
        else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

    },
    GetAttachments: function (project_id) {

        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': project_id,
        });
        var _result;
        ajaxHelper.Invoke("/ProjectRequest/ProjectAttachments", _param, "json", function (result) {
            if (result) {
                $("#formAttachments .attachmentItem").remove();

                $(result).each(function () {
                    var imgCls = Utilities.GetFileTypeClass(this.file_name);
                    $("#formAttachments").append(["<div status ='", this.status, "'  attachmentid ='", this.id,
                        "' filename='", this.file_name, "' process='", CONST.transaction_type.update,
                        "' class='attachmentItem' style='float:left;'><div class='attachmentName breakWord'>",
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(APP_HOPE.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", (APP_HOPE.EnableAttachments ? "" : "disabled"), "/></div>"].join(''));
                });

                if (APP_HOPE.EnableAttachments) {
                    $("#formAttachments .attachmentItem").append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem'></button></div>");
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("process", CONST.record_status.cancel);
                    });
                }

            }
        });
    },
    SetPagination: function (page_index, total_records) {//called from result view
        if (total_records > 0) {
            var total_pages = Math.ceil(total_records / PROJECT.SearchParam.page_size);
            var options = {
                bootstrapMajorVersion: 3,
                currentPage: page_index,
                totalPages: total_pages,
                numberOfPages: 10,
                alignment: 'right',
                onPageClicked: function (e, originalEvent, type, page) {
                    e.stopImmediatePropagation();
                    PROJECT.SearchParam.page_index = page;
                    APP_HOPE.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                APP_HOPE.MaintainData(CONST.transaction_type.search, null, null, null);
            });

            pageSelect.empty();
            var options = '';
            for (var i = 1; i <= total_pages; i++) {
                options += '<option value="' + i + '" ' + (i == page_index ? 'selected' : '') + '>' + i + '</option>';
            }
            pageSelect.append(options);
            pageTotal.text(total_pages);

        } else {
            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');

            pageContainer.hide();
            pageSelect.empty();
        }
    }
}

var handleGetProjectList = function (result) {
    return function (result) {
        $(".row-selection").remove();
        $.each(result, function (key, value) {
            var _row = $("<tr>", { id: value.id, class: "row-selection" });
            var _colID = $("<td>", { html: key + 1, class: "text-right" });
            var _colRef = $("<td>", { html: value.id, class: "text-center" });
            var _colCat = $("<td>", { html: value.category_desc, class: "text-left" });
            var _colTitle = $("<td>", { html: value.title, class: "text-left" });
            var _colReqBy = $("<td>", { html: value.created_by_name });
            var _colGrantee = $("<td>", { html: value.grantee });
            var _colAmount = $("<td>", { html: value.approved_budget, class: "text-right" });
            _colAmount.css("color", "#73b011");
            _row.append(_colID).append(_colRef).append(_colCat).append(_colTitle).append(_colReqBy).append(_colGrantee).append(_colAmount);
            $("#table_batch_list").append(_row);
        });
    }
}


var refreshData = function () {
    return function () {
        $("#processId").html("");
        APP_HOPE.MaintainData(CONST.transaction_type.search, null, null, null, null);
    }
}

$(function () {
    APP_HOPE.Initialize();
});