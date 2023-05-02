var WF = {
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(WF.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Type");
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
            WF.Status = CONST.record_status.activate;
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
            WF.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, null, null, null, CONST.record_status.delete, _item_list);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            WF.MaintainData(CONST.transaction_type.search, $("#u_id").val(), null, null, null, null, null, null, WF.Status, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
            alert("clicked print");
        });

        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            if (Site.ValidateRequiredEntries("#saveModal", null) == true) {
                WF.MaintainData(CONST.transaction_type.save, $("#u_id").val(),
                    $("#u_type").val(), $("#u_seq_title").val(), $("#u_seq_no").val(),
                    $("#u_seq_description").val(), $("#u_actor").val(), $("#u_sla").val(), WF.Status, null);
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
            $("#u_seq_title").val($(["tr#row-" + _id, " td[seq_title]"].join('')).html().trim());
            $("#u_seq_no").val($(["tr#row-" + _id, " td[seq_no]"].join('')).html().trim());
            $("#u_seq_description").val($(["tr#row-" + _id, " td[seq_description]"].join('')).html().trim());
            $("#u_actor").val($(["tr#row-" + _id, " td[actor]"].join('')).html().trim());
            $("#u_sla").val($(["tr#row-" + _id, " td[sla]"].join('')).html().trim());
            WF.Status = $(["tr#row-" + _id, " td[statusid]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });

        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            WF.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), null, null, null, null, null, null, CONST.record_status.deactivate, _item_list);
        });

        $(".item-setting-delete").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            WF.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), null, null, null, null, null, null, CONST.record_status.delete, _item_list);
        });
    },

    MaintainData: function (process, id, type, seq_title, seq_no, seq_desc, actor, sla, status, item_list) {        
        var _result;
        var param = {
            'id': id,
            'type': type,
            'seq_title': seq_title,
            'seq_no': seq_no,
            'seq_description': seq_desc,
            'actor': actor,
            'sla': sla,
            'status': status
        };
        
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'workflow': param,
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
$(function () {
    WF.Initialize();
});