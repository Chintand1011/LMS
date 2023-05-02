var PROJPROP = {
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(PROJPROP.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Code or Description");
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
            PROJPROP.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });

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
                PROJPROP.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, null, null, CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            PROJPROP.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_proponent_name").val(), $("#u_proponent_designation").val(), $("#u_dept_id").val(), $("#u_proponent_emailadd").val(), $("#u_proponent_contactno").val(), PROJPROP.Status, null);
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
                PROJPROP.MaintainData(CONST.transaction_type.save, $("#u_id").val(), $("#u_proponent_name").val(), $("#u_proponent_designation").val(), $("#u_dept_id").val(), $("#u_proponent_emailadd").val(), $("#u_proponent_contactno").val(), PROJPROP.Status, null);
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
            $("#u_proponent_name").val($(["tr#row-" + _id, " td[proponent_name]"].join('')).html().trim());
            $("#u_proponent_designation").val($(["tr#row-" + _id, " td[proponent_designation]"].join('')).html().trim());
            $("#u_dept_id").val($(["tr#row-" + _id, " td[dept_id]"].join('')).html().trim());
            $("#u_proponent_emailadd").val($(["tr#row-" + _id, " td[proponent_emailadd]"].join('')).html().trim());
            $("#u_proponent_contactno").val($(["tr#row-" + _id, " td[proponent_contactno]"].join('')).html().trim());
            PROJPROP.Status = $(["tr#row-" + _id, " td[status]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");
            PROJPROP.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_proponent_name").val(), $("#u_proponent_designation").val(), $("#u_dept_id").val(), $("#u_proponent_emailadd").val(), $("#u_proponent_contactno").val(), 
                (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                var _item_list = [$(ctl).data("itemid")];
                PROJPROP.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_proponent_name").val(), $("#u_proponent_designation").val(), $("#u_dept_id").val(), $("#u_proponent_emailadd").val(), $("#u_proponent_contactno").val(), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });
    },
    MaintainData: function (process, id, name, designation, dept_id, propemail, propcno, status, item_list) {
        var _result;
        var param = {
            'id': id,
            'proponent_name': name,
            'proponent_designation': designation,
            'dept_id': dept_id,
            'proponent_emailadd': propemail,
            'proponent_contactno': propcno,
            'status': status,
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'projectproponent': param,
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
    PROJPROP.Initialize();
});