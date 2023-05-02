var INCOMING = {
    Status: null,

    Initialize: function () {
        window.title = $("#headerTitle").html();
        $(document).attr("title", $("#headerTitle").html());

        $("#incomingTable .check-selected").hide();
        
        $("#btnSearch").unbind("click");
        $("#btnSearch").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.transaction_type.search);

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
            PROJECT.SearchParam.category = $("#search_category").val();
            PROJECT.SearchParam.project_name = $("#search_project_name").val();
            PROJECT.SearchParam.id = $("#search_draft_no").val();                       
            INCOMING.MaintainData(CONST.transaction_type.search, null, null, null);   
        });
    },
    

    MaintainData: function (process, record_status, user_action, item_list) {
        var _result;
        var _id = $("#u_id").val();

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'status': record_status,
            'item_list': item_list
        });
        
        ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));     
        $(".modal-backdrop").remove();       
    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.ProjectCategory] }), "", populateViewList(_result));
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
                    $("#processId").html(CONST.transaction_type.search);
                    PROJECT.SearchParam.page_index = page;
                    INCOMING.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                $("#processId").html(CONST.transaction_type.search);
                PROJECT.SearchParam.page_index = this.value;
                INCOMING.MaintainData(CONST.transaction_type.search, null, null, null);
            });
            
            pageSelect.empty();
            var options = '';
            for (var i = 1; i <= total_pages; i++) {
                options += '<option value="' + i + '" ' + (i == page_index ? 'selected':'') + '>' + i + '</option>';
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
        $("#processId").html(CONST.transaction_type.search);
        INCOMING.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    INCOMING.Initialize();
    INCOMING.InitializeViewList();
});