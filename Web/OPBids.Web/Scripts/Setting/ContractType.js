var CON = {
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(CON.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        //$("#addCaption, #removeCaption").html($("#headerTitle").html());
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Name");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        Site.GenerateControls('#saveModal');
        $("table.table-striped tbody td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            CON.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });

        $(".toolbar-delete").unbind("click");
        $(".toolbar-delete").click(function () {
            var _item_list = [];
            $(".check-selected:checked").each(function (e) {
                _item_list.push($(this).attr("itemid"));
            });
            if (_item_list.length <= 0) {
                Site.Dialogs.Alert("Cannot delete. Please select which one should be deleted.");
                return;
            }
            CON.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, CONST.record_status.delete, _item_list);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            CON.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_type").val(), $("#u_name").val(), CON.Status, null);
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
            if (Site.ValidateRequiredEntries("#saveModal", null) == true) {
                CON.MaintainData(CONST.transaction_type.save, $("#u_id").val(), $("#u_type").val(), $("#u_name").val(), CONST.record_status.activate, null);
                $('#saveModal').modal('hide');
            }
        });

        $(document).on('click touchend', function (e) {
            $(".setting-cmd").hide();
        });
        $(".setting-icon").unbind("click");
        $(".setting-icon").click(function (e) {
            e.preventDefault();
            //$(".setting-cmd").hide();
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

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#u_type").val($(["tr#row-" + _id, " td[type]"].join('')).html().trim());
            $("#u_name").val($(["tr#row-" + _id, " td[name]"].join('')).html().trim());
            CON.Status = $(["tr#row-" + _id, " td[status]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            CON.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_type").val(), $("#u_name").val(), CONST.record_status.deactivate, _item_list);
        });
        $(".item-setting-delete").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            CON.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_type").val(), $("#u_name").val(), CONST.record_status.delete, _item_list);
        });
    },
    MaintainData: function (process, id, contract_type, contract_desc, status, item_list) {
        var _result;
        var param = {
            'id': id,
            'contract_type': contract_type,
            'contract_desc': contract_desc,
            'status': status,
        };
        console.log(param);
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'contractype': param,
            'status': status,
            'item_list': item_list
        });
        ajaxHelper.Invoke(
            "/Setting/ResultView",
            _filter,
            "html",
            renderResultCallback(_result));
        $(".modal-backdrop").remove();
    }
}
$(function() {
    CON.Initialize();
});