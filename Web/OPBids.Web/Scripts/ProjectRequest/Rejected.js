var REJECTED = {
    Status: null,

    Attachments: [],
    AttachmentParam: {
        GUID: null,
        Blob: null,
    },
    DocumentAttachments: [],
    DocumentAttachment: {
        process: null,
        id: null,
        attachment_name: null,
        batch_id: null,
        barcode_no: null,
        file_name: null,
        status: null,
        guid: null,
        project_id: null
    },
    ProjectItems: [],
    ProjectItem: {
        process: null,
        id: null,
        unit: null,
        description: null,
        quantity: null,
        unit_cost: null,
        project_id: null
    },
    UploadSubFolder: 'ProjectRequest',//subfolder in upload folder
    EnableAttachments: true,

    Clone: function (obj) {
        return $.parseJSON(JSON.stringify(obj));
    },
    Initialize: function () {
        Site.Initialize();
        window.title = $("#headerTitle").html();
        $(document).attr("title", $("#headerTitle").html());        
        
        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            $("#processId").html(CONST.transaction_type.search);
            REJECTED.MaintainData(CONST.transaction_type.search, null, null, null);
        });

        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            PROJECT.PopulateProjectInformation(this);
        });

        $(".item-setting-reapprove").unbind("click");
        $(".item-setting-reapprove").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            var _item_list = [];
            _item_list.push($(this).attr("data-itemid"));                                    
            REJECTED.MaintainData(CONST.transaction_type.statusUpdate, CONST.record_status.activate, null, _item_list);
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

            Site.ResetTempData();

            $("#processId").html(CONST.transaction_type.search);

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
            REJECTED.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
        });

        $("#generalTab").click();
        Site.GenerateControls("#dataEntryModal");        
    },
    PopulateModal: function (_this) {
        ProjReqEditor.DisableFields();
        REJECTED.DisableAttachment();
        
        var _id = $(_this).attr("data-itemid");
        var _desc = $("#row-" + _id).data("description");
        var _cat = $("#row-" + _id).data("category");
        var _grant = $("#row-" + _id).data("grantee");
        var _reqDate = $("#row-" + _id).data("required-date");
        var _pr = $("#row-" + _id).data("pr-number");

        $("#u_id").val(_id);
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#estimated_budget").val($(["tr#row-" + _id, " td[budget]"].join('')).text().trim());
        $("#required_date").val(_reqDate);
        $("#description").val(_desc);
        $("#category").val(_cat);
        $("#grantee").val(_grant);
        $("#pr_number").val(_pr);

        REJECTED.GetAttachments(_id);
        REJECTED.GetProjectItems(_id);

        $('#dataEntryModal').modal('show');
    },

    MaintainData: function (process, record_status, user_action, item_list) {
        var _result;
        var _id = $("#u_id").val();

        var param = {
            'id': _id,
            'record_status': record_status,
            'user_action': user_action
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': param,
            'status': record_status,
            'item_list': item_list
        });

        if ($("#processId").html() === CONST.transaction_type.search) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));           
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", REJECTED.handleReApproval(_result));
        }
        $(".modal-backdrop").remove();
    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.ProjectCategory, CONST.setting_selection.ProjectGrantee] }), "", REJECTED.populateViewList(_result));
    },

    GetAttachments: function (project_id) {

        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': project_id,
        });
        var _result;
        ajaxHelper.Invoke("/ProjectRequest/ProjectAttachments", _param, "json", function (result) {
            if (result) {
                $("#formAttachments .attachmentItem").remove();

                $(result).each(function () {
                    var imgCls = Utilities.GetFileTypeClass(this.file_name);


                    $("#formAttachments").append(["<div status ='", this.status, "'  attachmentid ='", this.id,
                        "' filename='", this.file_name, "' process='", CONST.transaction_type.update,
                        "' class='attachmentItem' style='float:left;'><div class='attachmentName breakWord'>",
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(REJECTED.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", (REJECTED.EnableAttachments ? "" : "disabled"), "/></div>"].join(''));
                });

            }
        });
    },
    
    DisableAttachment: function () {        
        $("#btnBrowse").parent().hide();

        $("#btnBrowse").unbind("click");
        $("#fleBrowse").unbind("change");
        $("#formAttachments").unbind();
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
                    REJECTED.MaintainData(CONST.transaction_type.search, null, null, null);
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
                REJECTED.MaintainData(CONST.transaction_type.search, null, null, null);
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
    },

    GetProjectItems: function (project_id) {

        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': project_id,
        });
        var _result;

        ajaxHelper.Invoke("/ProjectRequest/ProjectItems", _param, "json", function (result) {
            if (result) {
                var $itemsTable = $("#projectRequestItems #projectItems tbody");

                $itemsTable.empty();

                var grandTotal = 0.00;
                //form-control text-box single-line
                var ctr = 1;
                $(result).each(function () {
                    var tableScope = $itemsTable;

                    var itemTotal = (parseInt(this.quantity.replace(/,/g, '')) * parseFloat(this.unit_cost.replace(/,/g, '')));
                    grandTotal += itemTotal;

                    tableScope.append(["<tr class='item' process='", CONST.transaction_type.update, "'><td><input type='checkbox' class='check-selected id' value='" + this.id + "'/></td>",
                        "<td>" + ctr++ + "</td>",
                        "<td><input type='text' class='form-control text-box unit' caption='Unit' value='" + this.unit + "' required='true'/></td>",
                        "<td><textarea class='form-control text-box description' caption='Item Description' required='true'>" + this.description + "</textarea></td>",
                        "<td><input type='text' class='form-control text-box quantity' caption='Quantity' number value='" + this.quantity + "' required='true'/></td>",
                        "<td><input type='text' class='form-control text-box unit-cost' caption='Unit Cost' money value='" + this.unit_cost + "' required='true'/></td>",
                        "<td><input type='text' class='form-control text-box total' money='true' value='" + Site.ToDecimalString(itemTotal) + "' readonly/></td>",
                        "</tr > "].join(''));
                });

                Site.GenerateControls($itemsTable);
                $itemsTable.find("input.quantity, input.unit-cost").on("change", function () { REJECTED.ComputeItemTotalCost(this); });

                $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(grandTotal));

                if ($("#processId").text() === CONST.process_id.submit) {
                    ProjReqItems.Disable();
                } else {
                    ProjReqItems.Enable();
                }

            }
        });
    },

    ComputeItemTotalCost: function (sender) {

        var $unitsGrandTotal = $("#projectRequestItems span.units-grand-total");
        var grandTotal = 0.00;

        if (sender != null) {
            var $tr = $(sender).closest('tr');

            var qty = parseInt($tr.find("input.quantity").val().replace(/,/g, ''));
            if (isNaN(qty)) { qty = 0; }

            var cost = parseFloat($tr.find("input.unit-cost").val().replace(/,/g, ''));
            if (isNaN(cost)) { cost = 0; }

            $tr.find("input.total").val(Site.ToDecimalString((qty * cost)));
        }

        var $totalInputs = $("#projectRequestItems #projectItems tbody tr input.total:visible");

        $totalInputs.each(function () {

            var totalValue = $(this).val().replace(/,/g, '');
            console.log(totalValue);
            console.log(isNaN(totalValue));
            if ((totalValue) && isNaN(totalValue) == false) {
                grandTotal += parseFloat(totalValue);
            }
        });

        $unitsGrandTotal.html(Site.ToDecimalString(grandTotal));

    },
    populateViewList: function (result) {
        return function (result) {
            if (result.status.code === 0) {
                var ProjectCategoryList = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ProjectCategory);
                });
                $(ProjectCategoryList).each(function () {
                    $("#category").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });
                var ProjectGranteeList = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ProjectGrantee);
                });
                $(ProjectGranteeList).each(function () {
                    $("#grantee").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });
            }
        }
    },
    handleReApproval: function (result) {
        return function (result) {
            Site.Dialogs.Alert("Record successully returned to For Approval!", "Got It!", REJECTED.refreshData());
        }
    },
    refreshData: function () {
        return function () {
            $("#processId").html(CONST.transaction_type.search);
            REJECTED.MaintainData(CONST.transaction_type.search, null, null, null);
        }
    },
}


$(function () {
    REJECTED.Initialize();
    REJECTED.InitializeViewList();
});