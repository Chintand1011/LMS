﻿var DELAYED = {
    Status: null,
    Active_ProjectSubStatus: null,
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
            DELAYED.PopulateModal(this);
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
            DELAYED.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $("#dataEntryModal #btnSave").unbind("click");
        $("#dataEntryModal #btnSave").click(function () {
            var _txn, _user_action;
            if ($("#processId").text() === CONST.process_id.create_new) {
                _txn = CONST.transaction_type.save;

                DELAYED.MaintainData(_txn, null, null, null);
                $('#dataEntryModal').modal('hide');
            }
        });

        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });

            if ($(this).attr("formid") == "formAttachments") {
                $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
            }
        });

        $("#generalTab").click();
        Site.GenerateControls("#dataEntryModal");
    },

    PopulateModal: function (_this) {
        ProjReqEditor.DisableFields();
        DELAYED.DisableAttachment();
        $("#btnSave").hide();

        var _id = $(_this).attr("data-itemid");

        $("#u_id").val(_id);
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#estimated_budget").val($(["tr#row-" + _id, " td[budget]"].join('')).html().trim());

        //$("#required_date").val($("#row-" + _id).data("required-date"));
        //$('#required_date').datepicker("setDate", $("#row-" + _id).data("required-date"));
        $("#required_date").val($.datepicker.formatDate("M dd yy", new Date($("#row-" + _id).data("required-date"))));

        $("#description").val($("#row-" + _id).data("description"));
        $("#grantee").val($("#row-" + _id).data("grantee"));
        $("#category").val($("#row-" + _id).data("category"));
        $("#classification").val($("#row-" + _id).data("classification"));
        $("#contract_type").val($("#row-" + _id).data("contract-type"));
        $("#security_level").val($("#row-" + _id).data("security-level"));
        $("#delivery_type").val($("#row-" + _id).data("delivery-type"));

        DELAYED.GetAttachments(_id);
        DELAYED.GetProjectItems(_id);

        if ($("#processId").text() === CONST.process_id.create_new) {
            ProjReqEditor.EnableFields();
            $("#btnSave").show();
        }

        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();
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

        DELAYED.ProjectItems = [];
        var processId = $("#processId").html();
        if (processId === CONST.process_id.edit) {

            $("#projectRequestItems #projectItems tr.item").each(function () {


                var item = DELAYED.Clone(DELAYED.ProjectItem);

                var $tr = $(this);

                item.id = $tr.find("input.id").val();
                item.process = $tr.attr("process");
                item.unit = $tr.find("input.unit").val();
                item.description = $tr.find("textarea.description").val();
                item.quantity = $tr.find("input.quantity").val();
                item.unit_cost = $tr.find("input.unit-cost").val();
                DELAYED.ProjectItems.push(item);
            });

        }
        else if (processId === CONST.process_id.create_new) {

            $("#projectRequestItems #projectItems tr.item").each(function () {
                var $tr = $(this);

                var item = DELAYED.Clone(DELAYED.ProjectItem);
                //new
                item.process = $tr.attr("process");

                item.id = 0;
                item.unit = $tr.find("input.unit").val();
                item.description = $tr.find("textarea.description").val();
                item.quantity = $tr.find("input.quantity").val();
                item.unit_cost = $tr.find("input.unit-cost").val();
                DELAYED.ProjectItems.push(item);
                
            });

        }


        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': _param,
            'projectItems': DELAYED.ProjectItems
        });

        if ($("#processId").html() === CONST.process_id.submit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleOnGoingSubmit(_result));
        } else if ($("#processId").html() === CONST.process_id.create_new) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleDraftCreate(_result));
        }
        else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetDocument(DELAYED.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' disabled /></div>"].join(''));
                });

                if (DELAYED.EnableAttachments) {
                    $("#formAttachments .attachmentItem").append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem'></button></div>");
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("process", CONST.record_status.cancel);
                    });
                }
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
                    PROJECT.SearchParam.page_index = page;
                    DELAYED.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                DELAYED.MaintainData(CONST.transaction_type.search, null, null, null);
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

                    tableScope.append(["<tr class='item' process='", CONST.transaction_type.update,  "'><td><input type='checkbox' class='check-selected id' value='" + this.id + "'/></td>",
                    "<td>" + ctr++ + "</td>",
                    "<td><input type='text' class='form-control text-box unit' caption='Unit' value='" + this.unit + "' required='true'/></td>",
                    "<td><textarea class='form-control text-box description' caption='Item Description' required='true'>" + this.description + "</textarea></td>",
                    "<td><input type='text' class='form-control text-box quantity' caption='Quantity' number value='" + this.quantity + "' required='true'/></td>",
                    "<td><input type='text' class='form-control text-box unit-cost' caption='Unit Cost' money value='" + this.unit_cost + "' required='true'/></td>",
                    "<td><input type='text' class='form-control text-box total' money='true' value='" + Site.ToDecimalString(itemTotal) + "' readonly/></td>",
                        "</tr > "].join(''));
                });

                Site.GenerateControls($itemsTable);

                $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(grandTotal));

                ProjReqItems.Disable();
                
            }
        });
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
var handleDraftCreate = function (result) {
    return function (result) {
        if (result.status.code == 0) {
            Site.Dialogs.Alert("Project Request Successully Created! You can now <br> view this project draft under Drafts Menu <br> with draft number " + result.value + ". Thank you.", "Got It", refreshData());
        }
        else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Close", refreshData());
        }
    }
}

var handleOnGoingSubmit = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project succussfully submitted. Thank you.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var refreshData = function () {
    return function () {
        $("#processId").html("");
        DELAYED.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    DELAYED.Initialize();
    DELAYED.InitializeViewList();
});