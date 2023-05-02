var PROJECT = {
    Status: null,
    Initialize: function () { 
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());

        $("#toolbarContent").css({ "display": "block" });
        $("#txt_search_key").attr("placeholder", "Search by Type");        
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which === 13) {
                $(".toolbar-search").click();
            }
        });
        Site.GenerateControls('#saveModal');

        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            PROJECT.Status = CONST.record_status.activate;
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
            PROJECT.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, null, null, null, CONST.record_status.delete, _item_list);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            PROJECT.MaintainData(CONST.transaction_type.search, $("#u_id").val(), null, null, null, null, null, null, CONST.project_status.draft, null);
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
            if (Site.ValidateRequiredEntries("#saveModal", null) === true) {
                PROJECT.MaintainData(CONST.transaction_type.save, $("#u_id").val(),
                    $("#title").val(), $("#description").val(), $("#grantee").val(),
                    $("#estimated_budget").val(), $("#required_date").val(), null, CONST.record_status.draft, null);
                $('#saveModal').modal('hide');
            }
        });

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            var _desc = $("#row-" + _id).data("description");
            
            $("#u_id").val(_id);
            $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
            $("#grantee").val($(["tr#row-" + _id, " td[grantee]"].join('')).html().trim());
            $("#estimated_budget").val($(["tr#row-" + _id, " td[budget]"].join('')).html().trim());
            $("#required_date").val($(["tr#row-" + _id, " td[created_date]"].join('')).html().trim());
            $("#description").val(_desc);
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        
        $(".item-setting-cancel").unbind("click");
        $(".item-setting-cancel").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            PROJECT.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), null, null, null, null, null, null, CONST.record_status.cancel, _item_list);
        });

        $(".item-setting-submit").unbind("click");
        $(".item-setting-submit").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            PROJECT.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), null, null, null, null, null, null, CONST.record_status.activate, _item_list);
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
            PROJECT.MaintainData(CONST.transaction_type.search, $("#u_id").val(), null, null, null, null, null, null, CONST.project_status.draft, null);
        });
    },

    MaintainData: function (process, id, title, description, grantee, estimated_budget, required_date, category, proj_status, item_list) {        
        var _result;

        var search = {
            'duration_scope': '',
            'submitted_from': $("#search_submit_from").data('datepicker').getFormattedDate('dd-M-yyyy'),
            'submitted_to': $("#search_submit_to").data('datepicker').getFormattedDate('dd-M-yyyy'),
            'required_from': $("#search_required_from").data('datepicker').getFormattedDate('dd-M-yyyy'),
            'required_to': $("#search_required_to").data('datepicker').getFormattedDate('dd-M-yyyy'),
            'budget_min': $("#search_budget_min").val().replace(',', ''),
            'budget_max': $("#search_budget_max").val().replace(',', ''),
            'grantee': $("#search_grantee").val(),
            'category': $("#search_category").val(),
            'project_name': $("#search_project_name").val(),
            'draft_no': $("#search_draft_no").val(),
            'project_status': proj_status
        };
        console.log(search);
        var param = {
            'id': id,
            'title': title,
            'description': description,
            'grantee': grantee,
            'estimated_budget': estimated_budget,
            'required_date': required_date,
            'category': category
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': search,
            'projectRequest': param,
            'status': status,
            'item_list': item_list
        });

        ajaxHelper.Invoke(
            "/ProjectRequest/ResultView",
            _filter,
            "html",
            renderResultCallback(_result));
        $(".modal-backdrop").remove();
    }
}
$(function () {
    PROJECT.Initialize();
});