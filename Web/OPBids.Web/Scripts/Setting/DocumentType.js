﻿var DT = {
    CurData: null,
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(DT.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        DT.CurData = (new Date().getTime()).toString();
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());        
		$("#txt_search_key").attr("placeholder", "Search by Code, Type or Definition");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();        
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        $("[data-toggle=tooltip]").mouseenter(function () {
            var $this = $(this);
            $this.attr('title', $this.val());
        });
        Site.GenerateControls('#saveModal');
        $("table.table-striped tbody td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".item-setting-inactive[statusidsetter]").each(function () {
            if ($(this).attr("statusidsetter").toUpperCase() == "I") {
                $(this).html("Set as Active")
            }
        });
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            DT.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');
            $("#u_document_category_id").val('').selectpicker('refresh');
            Site.DropDownRefresh("#u_document_category_id");
            $("#saveModal *[required]").keyup();
        });
        DT.GetDocumentCategory();
        $(".toolbar-delete").unbind("click");
        $(".toolbar-delete").click(function () {
            var confirmEvt = function () {
                var _item_list = [];
                $(".check-selected:checked").each(function () {
                    _item_list.push($(this).attr("itemid"));
                });
                if (_item_list.length <= 0) {
                    Site.Dialogs.Alert("Cannot delete. Please select which one should be deleted.");
                    return;
                }
                DT.MaintainData(CONST.transaction_type.statusUpdate, 0, 0, null, null, CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            DT.MaintainData(CONST.transaction_type.search, $("#u_document_category_id").val(), $("#u_id").val(), $("#u_document_type_code").val(), $("#u_document_type_description").val(), DT.Status, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
            Site.Print(["/Setting/Print?setting=", localStorage.getItem("sub-menu-id")].join(''));
        });

        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
          //  debugger;
            if (Site.ValidateRequiredEntries("#saveModal", null) == true) {
                DT.MaintainData(CONST.transaction_type.save, $("#u_document_category_id").val(), $("#u_id").val(), $("#u_document_type_code").val(), $("#u_document_type_description").val(), DT.Status, null);
                $('#saveModal').modal('hide');
            }
        });

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#u_document_category_id").val($(["tr#row-" + _id, " td[document_category_id]"].join('')).attr("document_category_id")).selectpicker('refresh');
            Site.DropDownRefresh("#u_document_category_id");
            $("#u_document_type_code").val($(["tr#row-" + _id, " td[document_type_code]"].join('')).html().trim());
            $("#u_document_type_description").val($(["tr#row-" + _id, " td[document_type_description]"].join('')).html().trim());
            DT.Status = $(["tr#row-" + _id, " td[status]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");
            DT.MaintainData(CONST.transaction_type.statusUpdate, $("#u_document_category_id").val(), $("#u_id").val(),
                $("#u_document_type_code").val(), $("#u_document_type_description").val(),
                (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                e.preventDefault();
                var _item_list = [$(ctl).data("itemid")];
                DT.MaintainData(CONST.transaction_type.statusUpdate, $("#u_document_category_id").val(), $("#u_id").val(), $("#u_document_type_code").val(), $("#u_document_type_description").val(), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
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
    },
    MaintainData: function (process, document_category_id, id, document_type_code, document_type_description, status, item_list) {
        var _result;
        var param = {
            'id': id,
            'document_category_id': document_category_id,
            'document_type_code': document_type_code,
            'document_type_description': document_type_description,
            'status': status,
            'page_index': $(".paging").val(),
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'documentType': param,
            'status': status,
            'page_index': $(".paging").val(),
            'item_list': item_list
        });
        ajaxHelper.Invoke(
            "/Setting/ResultView",
            _filter,
            "html",
            renderResultCallback(_result));
        $(".modal-backdrop").remove();
    },
    GetDocumentCategory: function () {
        var _result;
        var param = {
            'id': null,
            'document_category_code': null,
            'document_category_name': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.menu_id.DocumentCategory,
            'txn': null,
            'search_key': null,
            'documentCategory': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#u_document_category_id option").remove();
            var data = $("body").data(DT.CurData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {                    
                    $("#u_document_category_id").append(["<option desc='", this.document_category_name, "' value='", this.id, "'>", this.document_category_code, "</option>"].join(''));
                })                
            }
            Site.DropDownCommonSettings("#u_document_category_id");
        }
        Site.PostData("/Setting/GetDocumentCategory", postEvt, _filter, DT.CurData);
    },
}
$(function() {
    DT.Initialize();    
});