var ONGOINGUSER = {
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
    UploadSubFolder: 'ProjectRequest',//subfolder in upload folder
    EnableAttachments: true,
    Clone: function (obj) {
        return $.parseJSON(JSON.stringify(obj));
    },
    ValidateAttachment: function (blob, val) {
        val = val.toLowerCase();
        var regex = new RegExp("(.*?)\.(jpg|png|gif|doc|pdf|docx|xls|xlsx|txt)$");
        if ((regex.test(val.toLowerCase()))) {
            var attachmentName = window.prompt("Please enter an attachment name", "");
            if (attachmentName === null || attachmentName === "") {
                $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
                return;
            }
            var param = ONGOINGUSER.Clone(ONGOINGUSER.AttachmentParam);
            param.GUID = moment(new Date()).format("YYYYMMDDhhmmssSS");
            param.AttachmentName = attachmentName;
            param.FileName = val;
            param.Blob = blob;
            ONGOINGUSER.Attachments.push(param);
            var imgCls = Utilities.GetFileTypeClass(val);
            $("#formAttachments").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<div class='attachmentName breakWord'>", attachmentName,
                "</div><div class='", imgCls, "' title='For upload'></div><input type='text' maxlength='20' /></div>"].join(''));


            if (ONGOINGUSER.EnableAttachments) {
                $(["#formAttachments .attachmentItem[guid='", param.GUID, "']"].join('')).append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem'></button></div>");
                $(["#formAttachments .attachmentItem[guid='", param.GUID, "'] .removeItem"].join('')).click(function () {
                    $(this).parent().parent().remove();
                });
            }
        }
        else {
            Site.Dialogs.Alert("Invalid file extension, please upload a file with jpg, png, gif, doc, pdf, docx, xls, xlsx, or txt file exntesions.", "OK", null);
        }
        $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
    },
    Initialize: function () {
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
            ONGOINGUSER.PopulateModal(this);
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
            ONGOINGUSER.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $("#dataEntryModal #btnSave").unbind("click");
        $("#dataEntryModal #btnSave").click(function () {
            var _txn, _user_action;
            if ($("#processId").text() === CONST.process_id.create_new) {
                _txn = CONST.transaction_type.save;

                ONGOINGUSER.MaintainData(_txn, null, null, null);
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

        var $addItem = $("#projectRequestItems #addRemoveToolbar .toolbar-add");
        $addItem.unbind("click");
        $addItem.click(
            function () {
                var $itemsTable = $("#projectRequestItems #projectItems tbody");
                
                $itemsTable.append(["<tr class='item' process='", CONST.transaction_type.create, "'><td><input type='checkbox' value='0' class='check-selected id' /></td>",
                    "<td></td>",
                    "<td><input type='text' class='form-control text-box unit' caption='Unit' required='true'/></td>",
                    "<td><textarea class='form-control text-box description' caption='Item Description' required='true'></textarea></td>",
                    "<td><input type='text' class='form-control text-box quantity' caption='Quantity' number required='true'/></td>",
                    "<td><input type='text' class='form-control text-box unit-cost' caption='Unit Cost' money required='true'/></td>",
                    "<td><input type='text' class='form-control text-box total' money='true' readonly/></td>",
                    "</tr > "].join(''));

                Site.GenerateControls($itemsTable);
                $itemsTable.find("tr:last").on("change", function () { ONGOINGUSER.ComputeItemTotalCost(this); });
            }
        );

        var $removeItem = $("#projectRequestItems #addRemoveToolbar .toolbar-delete");
        $removeItem.unbind("click");
        $removeItem.click(
            function () {
                $("#projectRequestItems #projectItems tr.item").each(function () {

                    var $tr = $(this);
                    //   debugger;
                    var $chkId = $tr.find("input.id");
                    if ($chkId.is(":checked")) {
                        if ($chkId.val() != "0") {
                            //hide
                            $chkId.prop('checked', false);
                            $chkId.closest("tr").hide();
                            $chkId.closest("tr").attr("process", CONST.transaction_type.delete);
                            //$tr.hide();
                        } else {
                            $chkId.closest("tr").remove();
                        }

                    }
                });
                ONGOINGUSER.ComputeItemTotalCost();
            }
        );
    },

    PopulateModal: function (_this) {
        ProjReqEditor.DisableFields();
        ONGOINGUSER.DisableAttachment();
        $("#btnSave").hide();        

        var _id = $(_this).attr("data-itemid");

        $("#u_id").val(_id);
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#estimated_budget").val($(["tr#row-" + _id, " td[budget]"].join('')).html().trim());        
        $("#required_date").val($.datepicker.formatDate("M dd yy", new Date($("#row-" + _id).data("required-date"))));

        $("#description").val($("#row-" + _id).data("description"));
        $("#grantee").val($("#row-" + _id).data("grantee"));
        $("#category").val($("#row-" + _id).data("category"));
        $("#pr_number").val($("#row-" + _id).data("pr-number"));
        $("#classification").val($("#row-" + _id).data("classification"));
        $("#contract_type").val($("#row-" + _id).data("contract-type"));
        $("#security_level").val($("#row-" + _id).data("security-level"));
        $("#delivery_type").val($("#row-" + _id).data("delivery-type"));
        $("#pr_number").val($("#row-" + _id).data("pr-number"));

        ONGOINGUSER.GetAttachments(_id);
        ONGOINGUSER.GetProjectItems(_id);

        if ($("#processId").text() === CONST.process_id.create_new) {
            ProjReqEditor.EnableFields();
            ONGOINGUSER.EnableAttachment();
            $("#btnSave").show();
        }

        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();
    },
    
    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;
        var _param;
        var _id = $("#u_id").val();        

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
                'pr_number': $("#pr_number").val(),
                'record_status': record_status,
                'user_action': user_action
            };
        }

        ONGOINGUSER.ProjectItems = [];

        var processId = $("#processId").html();
        if (processId === CONST.process_id.edit) {

            $("#projectRequestItems #projectItems tr.item").each(function () {


                var item = ONGOINGUSER.Clone(ONGOINGUSER.ProjectItem);

                var $tr = $(this);

                item.id = $tr.find("input.id").val();

                item.process = $tr.attr("process");

                item.unit = $tr.find("input.unit").val();
                item.description = $tr.find("textarea.description").val();
                item.quantity = $tr.find("input.quantity").val();
                item.unit_cost = $tr.find("input.unit-cost").val();
                ONGOINGUSER.ProjectItems.push(item);
            });

        }
        else if (processId === CONST.process_id.create_new) {

            $("#projectRequestItems #projectItems tr.item").each(function () {
                var $tr = $(this);

                var item = ONGOINGUSER.Clone(ONGOINGUSER.ProjectItem);
                //new
                  
                item.process = $tr.attr("process");
                item.id = 0;
                item.unit = $tr.find("input.unit").val();
                item.description = $tr.find("textarea.description").val();
                item.quantity = $tr.find("input.quantity").val();
                item.unit_cost = $tr.find("input.unit-cost").val();
                ONGOINGUSER.ProjectItems.push(item);
                
            });

        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': _param,
            'projectItems': ONGOINGUSER.ProjectItems
        });

        if ($("#processId").html() === CONST.process_id.submit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project succussfully submitted. Thank you."));
        } else if ($("#processId").html() === CONST.process_id.create_new) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project Request Successully Created! You can now <br> view this project draft under Drafts Menu <br> with draft number {value}.", null, true));
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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(ONGOINGUSER.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='",imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' disabled /></div>"].join(''));
                });

                if (ONGOINGUSER.EnableAttachments) {
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
        ONGOINGUSER.EnableAttachments = false;

        $("#btnBrowse").parent().hide();
        $("#btnBrowse").unbind("click");

        $("#fleBrowse").unbind("change");
        $("#formAttachments").unbind();
    },
    EnableAttachment: function () {
        ONGOINGUSER.EnableAttachments = true;
        $("#btnBrowse").parent().show();
        $("#btnBrowse").click(function () {
            $("#fleBrowse").click();
        });

        $("#fleBrowse").change(function () {
            ONGOINGUSER.ValidateAttachment(this.files[0], $(this).val());
        });

        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                ONGOINGUSER.ValidateAttachment(blobData, pFile[0].name);
            }
        });
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
                    ONGOINGUSER.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                ONGOINGUSER.MaintainData(CONST.transaction_type.search, null, null, null);
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

                $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(grandTotal));

                
                if ($("#processId").text() === CONST.process_id.create_new) {
                    ProjReqItems.Enable();
                } else {
                    ProjReqItems.Disable();
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
            if ((totalValue) && isNaN(totalValue) == false) {
                grandTotal += parseFloat(totalValue);
            }
        });

        $unitsGrandTotal.html(Site.ToDecimalString(grandTotal));

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
        ONGOINGUSER.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    ONGOINGUSER.Initialize();
    ONGOINGUSER.InitializeViewList();
});