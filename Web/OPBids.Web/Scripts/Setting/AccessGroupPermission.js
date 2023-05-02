var AGP = {
    CurDate: null,
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(AGP.Initialize, 1000);
            return;
        }
        $("#lstAccessGroupType input[type='checkbox']").click(function () {
            $(this).parent().parent().attr("updated", "");
        });
        $("#pageContainer").css({ "display": "inline-block" });
        AGP.CurData = (new Date().getTime()).toString();
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key, #basic-addon2").css({ "display": "none" });
        
        $("#addRemoveToolbar").html(['<a class="toolbar-add" href="#"><img src="../Content/images/icon-save.svg" />',
            '<span style="color: forestgreen">Save ', $("#headerTitle").html(), '</span></a>'].join(''));

        $("#cboAccessGroupPermission").unbind("change");
        $("#cboAccessGroupPermission").change(function (e) {
            var _filter = JSON.stringify({ 'sub_menu_id': localStorage.getItem("sub-menu-id"), 'id': $(this).val() });
            PopuplatePartialView(_filter);
        });

        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            var paramList = [];
            $("#lstAccessGroupType tbody tr[details][updated]").each(function () {
                var id = $(this).find("td[itemid]").attr("itemid");
                var access_type_id = $(this).find("td[access_type_id]").attr("access_type_id");
                var param = {
                    access_group_type_id: id,
                    access_type_id: access_type_id,
                    access_group_id: $("#cboAccessGroupPermission").val(),
                    view_transact_data: $(this).find("td input[view_transact_data][type='checkbox']").is(":checked"),
                    add_edit_data: $(this).find("td input[add_edit_data][type='checkbox']").is(":checked"),
                    delete_data: $(this).find("td input[delete_data][type='checkbox']").is(":checked"),
                    record_section: $(this).find("td input[record_section][type='checkbox']").is(":checked"),
                    disp_menu_to_mobile: null
                }
                paramList.push(param);
            });
            if (paramList.length > 0) {
                AGP.MaintainData(paramList);
            }
        });
        AGP.GetAccessGroups();
        $(".tableCollapser tr[section]").click(function () {
            var curId = $(this).attr("rowid");
            var newClass = ($(this).find("span[collapse]").attr("collapse") == "1" ? "shown" : "hidden");
            $(this).find("span[collapse]").attr("collapse", (newClass == "shown" ? "0" : "1"));
            $([".tableCollapser tr[rowid='", curId, "']:not([section])"].join('')).removeClass("hidden").removeClass("shown").addClass(newClass);
        });
        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
			Site.Print(["/Setting/Print?setting=", localStorage.getItem("sub-menu-id")].join(''));
        });

    },
    MaintainData: function (access_group_type_list) {
        var _result;

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': CONST.transaction_type.save,
            'search_key': $("#txt_search_key").val(),
            'accessGroupType': access_group_type_list,
            'status': null,
            'item_list': null
        });
        ajaxHelper.Invoke(
            "/Setting/ResultView",
            _filter,
            "html",
            renderResultCallback(_result));
        $(".modal-backdrop").remove();
    },
    GetAccessGroups: function () {
        var _result;
        var param = {
            'id': null,
            'dept_code': null,
            'dept_name': null,
            'is_main_dept': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.menu_id.AccessGroups,
            'txn': null,
            'search_key': null,
            'accessGroups': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#cboAccessGroupPermission option").remove();
            var data = $("body").data(AGP.CurData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#cboAccessGroupPermission").append(["<option value='", this.id, "'>", this.group_code, "</option>"].join(''));
                })
            }
            $("#cboAccessGroupPermission").val($("#lstAccessGroupType tr td[access_group_id]").attr("access_group_id"));
            $("#cboAccessGroupPermission").selectpicker('refresh');
        }
        Site.PostData("/Setting/GetAccessGroups", postEvt, _filter, AGP.CurData);
    },
}
$(function() {
    AGP.Initialize();
});