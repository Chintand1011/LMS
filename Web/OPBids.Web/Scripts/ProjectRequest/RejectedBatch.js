var REJECTED_BATCH = {
    Status: null,
    Initialize: function () {
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());

        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#toolbarContent .toolbar-add").hide();
        $("#toolbarContent .toolbar-delete").hide();
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
        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            REJECTED_BATCH.PopulateModal(this);
        });        
        $(".item-setting-reapprove").unbind("click");
        $(".item-setting-reapprove").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            $("#u_id").val($(this).attr("data-itemid"));
            REJECTED_BATCH.MaintainData(CONST.transaction_type.statusUpdate, CONST.record_status.activate, null, null, null);
        });

        $("#btnSearch").unbind("click");
        $("#btnSearch").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.transaction_type.search);

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
            REJECTED_BATCH.MaintainData(CONST.transaction_type.search, null, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });
        
        Site.GenerateControls("#dataBatchViewModal");
    },

    PopulateModal: function (_this) {        
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);
        $("#dataBatchViewModal #modal_batch_id").html(_id);
        $("#dataBatchViewModal #modal_proc_method").html($("#row-" + _id).data("procurement-method-desc"));
        $("#dataBatchViewModal #modal_proc_status").html($("#row-" + _id).data("project-substatus-desc"));
        $("#dataBatchViewModal #batch_total_projects").html($("#row-" + _id).data("total-project"));
        $("#dataBatchViewModal #batch_total_amount").html($("#row-" + _id).data("total-amount"));
        $("#dataBatchViewModal #batch_date_created").html($("#row-" + _id).data("created-date") + " (" + $("#row-" + _id).data("created-by") + ")");

        REJECTED_BATCH.GetProjectList($("#u_id").val());
        
        $('#dataBatchViewModal').modal('show');
    },

    GetProjectList: function (id) {
        var _result;
        var _search = {
            'batch_id': id
        };
        var _filter = JSON.stringify({
            'projectSearch': _search
        });
        ajaxHelper.Invoke("/ProjectRequest/GetBatchProjectRequestList", _filter, "json", REJECTED_BATCH.HandleGetProjectList(_result));
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;

        if ($("#processId").text() !== CONST.transaction_type.search) {
            var param = {
                'id': $("#u_id").val(),
                'record_status': record_status
            };
        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequestBatch': param
        });
        
        if ($("#processId").html() === CONST.transaction_type.search) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Record successully returned to For Approval!"));
        }
        $(".modal-backdrop").remove();
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
                    REJECTED_BATCH.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                REJECTED_BATCH.MaintainData(CONST.transaction_type.search, null, null, null);
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
    },

    HandleGetProjectList: function (result) {
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
}
var refreshData = function () {
    return function () {
        $("#processId").html("");
        $("#btnSearch").click();
    }
}
$(function () {
    REJECTED_BATCH.Initialize();
});