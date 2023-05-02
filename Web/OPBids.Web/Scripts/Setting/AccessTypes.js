var AT = {
    Status: null,
    CurrentGroupId: null,
    GroupTabClick: function (ctl) {
        $(".paging").val(0);
        AT.CurrentGroupId = $(ctl).attr("itemid");
        AT.MaintainData(CONST.transaction_type.search, null, null);
    },
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(atob.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Access Type or Description");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#addRemoveToolbar *").css({ "display": "none" });
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        $(".tableCollapser tr[section]").click(function () {
            var curId = $(this).attr("rowid");
            var newClass = ($(this).find("span[collapse]").attr("collapse") == "1" ? "shown" : "hidden");
            $(this).find("span[collapse]").attr("collapse", (newClass == "shown" ? "0" : "1"));
            $([".tableCollapser tr[rowid='", curId, "']:not([section])"].join('')).removeClass("hidden").removeClass("shown").addClass(newClass);
        });

        $(".tableCollapser td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".item-setting-inactive[statusidsetter]").each(function () {
            if ($(this).attr("statusidsetter").toUpperCase() == "I") {
                $(this).html("Set as Active")
            }
        });
        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            AT.MaintainData(CONST.transaction_type.search, null, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
			Site.Print(["/Setting/Print?setting=", localStorage.getItem("sub-menu-id")].join(''));
        });

        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");
            AT.MaintainData(CONST.transaction_type.statusUpdate, (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(document).on('click touchend', function (e) {
            $(".setting-cmd").hide();
        });
        $(".item-setting-setaccessrights").unbind("click");
        $(".item-setting-setaccessrights").click(function (e) {
            localStorage.setItem("sub-menu-id", CONST.menu_id.AccessGroupPermission);
            window.location.reload();
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
    },
    MaintainData: function (process, status, item_list) {
        var _result;
        var param = {
            'parent_id': AT.CurrentGroupId,
        };
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'accessTypes': param,
            'status': status,
            'item_list': item_list,
            'page_index': $(".paging").val()
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
    AT.Initialize();
});