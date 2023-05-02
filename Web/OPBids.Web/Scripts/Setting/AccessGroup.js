var AG = {
    CurData: null,
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(AG.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Group Code or Description");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        AG.CurData = (new Date().getTime()).toString();
        Site.GenerateControls('#saveModal');
        $("table.table-striped tbody td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".item-setting-inactive[statusidsetter]").each(function () {
            if ($(this).attr("statusidsetter").toUpperCase() == "I") {
                $(this).html("Set as Active")
            }
        });
        $(".item-setting-setaccessrights").unbind("click");
        $(".item-setting-setaccessrights").click(function (e) {
            localStorage.setItem("sub-menu-id", CONST.menu_id.AccessGroupPermission);
            window.location.reload();
        });
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            AG.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
            $("#u_dashboard_id").val('').selectpicker('refresh');
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
                AG.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, $("#u_dashboard_id").val(),
                    CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            AG.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_group_code").val(),
                $("#u_group_description").val(), $("#u_dashboard_id").val(), AG.Status, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
			Site.Print(["/Setting/Print?setting=", localStorage.getItem("sub-menu-id")].join(''));
        });

        $("#btnCancel").unbind("click");
        $("#btnCancel").click(function () {
            $('#saveModal').modal('hide');
        });
        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            var postEvt = function () {
                if (Site.ValidateRequiredEntries("#saveModal", null) == true) {
                    AG.MaintainData(CONST.transaction_type.save, $("#u_id").val(), $("#u_group_code").val(), $("#u_group_description").val(),
                        $("#u_dashboard_id").val(), AG.Status, null);
                    $('#saveModal').modal('hide');
                }
            }
            AG.IsExists(postEvt);
        });
        AG.GetDashBoardConfig(); 

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
            $("#u_group_code").val($(["tr#row-" + _id, " td[group_code]"].join('')).text().trim());
            $("#u_group_description").val($(["tr#row-" + _id, " td[group_description]"].join('')).text());
            $("#u_dashboard_id").val($(["tr#row-" + _id, " td[dashboard_id]"].join('')).attr("dashboard_id"));
            $("#u_dashboard_id").selectpicker('refresh');
            $("#u_dept_id").val($(["tr#row-" + _id, " td[dept_id]"].join('')).attr("dept_id"));
            $("#u_group_id").val($(["tr#row-" + _id, " td[group_id]"].join('')).attr("group_id"));

            AG.Status = $(["tr#row-" + _id, " td[status]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");
            AG.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_group_code").val(), $("#u_group_description").val(),
                $("#u_dashboard_id").val(), (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                var _item_list = [$(ctl).data("itemid")];
                AG.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_group_code").val(), $("#u_group_description").val(),
                    $("#u_dashboard_id").val(), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

    },
    IsExists: function (postSaveEvt) {
        var _result;
        var param = {
            'id': null,
            'dept_code': null,
            'dept_name': null,
            'is_main_dept': null,
            'status': null,
        };

        var _filter = {
            'id': $("#u_id").val(),
            'sub_menu_id': CONST.menu_id.AccessGroups,
            'txn': null,
            'search_key': $("#u_group_code").val().toLowerCase(),
            'accessGroups': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#u_group_id option").remove();
            var data = $("body").data(AG.CurData);
            var isExists = false;
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if ($("#u_group_code").val().toLowerCase() == this.group_code.toLowerCase()) {
                        isExists = true;
                    }
                });
            }
            if (isExists == true) {
                Site.Dialogs.Alert("Cannot save. Access Code already exists.");
            }
            else {
                postSaveEvt();
            }
        }
        Site.PostData("/Setting/GetAccessGroups", postEvt, _filter, AG.CurData);
    },
    MaintainData: function (process, id, group_code, group_description, dashboard_id, status, item_list) {
        var _result;
        var param = {
            'id': id,
            'group_code': group_code,
            'group_description': group_description,
            'dashboard_id':dashboard_id,
            'status': status,
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'accessGroup': param,
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

      GetDashBoardConfig: function () {
        var _result;
        var param = {
            'dashboard_id': null,
            'dashboard_desc': null,
            'id': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.setting_selection.DashboardConfig,
            'txn': null,
            'search_key': null,
            'dashboardConfig': param,
            'status': null,
            'item_list': null,
        };
        var postEvt = function () {
            $("#u_dashboard_id option").remove();
            var data = $("body").data(AG.CurData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_dashboard_id").append(["<option value='", this.dashboard_id, "'>", this.dashboard_desc, "</option>"].join(''));
                 })
            }
            Site.DropDownCommonSettings("#u_dashboard_id");
        }
     
        Site.PostData("/Setting/GetDashBoardConfigs", postEvt, _filter, AG.CurData);

    }

  
}
$(function() {
    AG.Initialize();
});