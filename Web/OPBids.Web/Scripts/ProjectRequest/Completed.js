var COMPLETED = {
    Status: null,
    Active_ProjectSubStatus: null,
    UploadSubFolder: 'ProjectRequest',//subfolder in upload folder
    Initialize: function () {
        Site.Initialize();

        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#processId").html("");

        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#toolbarContent .toolbar-add").hide();
        $("#toolbarContent .toolbar-delete").hide();

        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            PROJECT.PopulateProjectInformation(this);
        });

        $(".item-setting-newdraft").unbind("click");
        $(".item-setting-newdraft").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.create_new);
            COMPLETED.PopulateModal(this);
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
            PROJECT.SearchParam.required_from = $("#search_required_from").val();
            PROJECT.SearchParam.required_to = $("#search_required_to").val();
            PROJECT.SearchParam.budget_min = $("#search_budget_min").val().replace(',', '');
            PROJECT.SearchParam.budget_max = $("#search_budget_max").val().replace(',', '');
            PROJECT.SearchParam.grantee = $("#search_grantee").val();
            PROJECT.SearchParam.category = $("#search_category").val()
            PROJECT.SearchParam.project_name = $("#search_project_name").val();
            PROJECT.SearchParam.id = $("#search_draft_no").val();

            PROJECT.SearchParam.get_total = true;
            COMPLETED.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });
       


        
    },
    
    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;
        var _param;
        var _id = $("#u_id").val();

        //var _search = {
        //    'duration_scope': '',
        //    'submitted_from': $("#search_submit_from").data('datepicker').getFormattedDate('dd-M-yyyy'),
        //    'submitted_to': $("#search_submit_to").data('datepicker').getFormattedDate('dd-M-yyyy'),
        //    'required_from': $("#search_required_from").data('datepicker').getFormattedDate('dd-M-yyyy'),
        //    'required_to': $("#search_required_to").data('datepicker').getFormattedDate('dd-M-yyyy'),
        //    'budget_min': $("#search_budget_min").val(),
        //    'budget_max': $("#search_budget_max").val(),
        //    'grantee': $("#search_grantee").val(),
        //    'category': $("#search_category").val(),
        //    'project_name': $("#search_project_name").val()
        //};

        if ($("#processId").text() !== "") {
            if ($("#processId").text() === CONST.process_id.create_new) {
                _id = 0;
            }
            _param = {
                'id': _id,
                'title': $("#title").val(),
                'description': $("#description").val(),
                'grantee': $("#grantee").val(),
                'estimated_budget': $("#estimated_budget").val(),
                'required_date': $("#required_date").val(),
                'category': $("#category").val(),
                'record_status': record_status,
                'user_action': user_action
            };
        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': _param,
            'item_list': item_list
        });

        ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));

    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList",
            JSON.stringify({
                "setting_list": [
                    CONST.setting_selection.ProjectCategory
                ]
            }),
            "", populateViewList(_result));

        ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.ProjectGrantee] }), "", populateGrantees);
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
                    COMPLETED.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                COMPLETED.MaintainData(CONST.transaction_type.search, null, null, null);
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

var populateViewList = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            var ProjectCategoryList = $.grep(result.value, function (x, y) {
                return (x.type === CONST.setting_selection.ProjectCategory);
            });
            $(ProjectCategoryList).each(function () {
                $("#category").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
            });
        }
    }
}
var populateGrantees = function (result) {

    if (result.status.code === 0) {
        var ProjectGranteeList = $.grep(result.value, function (x, y) {
            return (x.type === CONST.setting_selection.ProjectGrantee);
        });
        $(ProjectGranteeList).each(function () {
            $("#grantee").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
        });
    }
}

var refreshData = function () {
    return function () {
        $("#processId").html("");
        COMPLETED.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    COMPLETED.Initialize();
    COMPLETED.InitializeViewList();
});