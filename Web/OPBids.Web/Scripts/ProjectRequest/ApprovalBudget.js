var APPROVAL = {
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
            if (attachmentName == null || attachmentName == "") {
                $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
                return;
            }
            var param = APPROVAL.Clone(APPROVAL.AttachmentParam);
            param.GUID = moment(new Date()).format("YYYYMMDDhhmmssSS");
            param.AttachmentName = attachmentName;
            param.FileName = val;
            param.Blob = blob;
            APPROVAL.Attachments.push(param);
            var imgCls = Utilities.GetFileTypeClass(val);
            $("#formAttachments").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<div class='attachmentName breakWord'>", attachmentName,
                "</div><div class='", imgCls, "' title='For upload'></div><input type='text' maxlength='20' /></div>"].join(''));

            if (APPROVAL.EnableAttachments) {
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

        $(".item-setting-approve").unbind("click");
        $(".item-setting-approve").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            APPROVAL.PopulateModal(this);
        });

        $(".item-setting-reject").unbind("click");
        $(".item-setting-reject").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.reject);
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);

            $("#ProjectRequestNoteModal #modal-note-title").text("Project Budget Reject Note");
            $("#ProjectRequestNoteModal #modal-note-label").text("Reason for Rejecting Project Budget");
            $("#ProjectRequestNoteModal #footerNote").text("Project will be removed from the list.");
            
            $("#ProjectRequestNoteModal").modal("show");
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
            APPROVAL.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
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

        $("#dataEntryModal #btnSave").unbind("click");
        $("#dataEntryModal #btnSave").click(function () {
            if (Site.ValidateRequiredEntries("#dataEntryModal", null) == true) {
                if ($("#processId").text() === CONST.process_id.submit) {
                    $("#modal-note-title").text("Approval Note");
                    $("#ProjectRequestNoteModal").modal("show");
                }
            };
        });

        $("#ProjectRequestNoteModal #btnSaveNoteModal").unbind("click");
        $("#ProjectRequestNoteModal #btnSaveNoteModal").click(function () {
            if (Site.ValidateRequiredEntries("#ProjectRequestNoteModal", null) == true) {
                var _txn, _record_status, _user_action;
                if ($("#processId").text() === CONST.process_id.submit) {
                    _txn = CONST.transaction_type.save;
                    _user_action = CONST.user_action.approve;
                } else if ($("#processId").text() === CONST.process_id.reject) {
                    _txn = CONST.transaction_type.statusUpdate;
                    _record_status = CONST.record_status.reject;
                }
                APPROVAL.MaintainData(_txn, _record_status, _user_action, $("#u_id").val(), $(".dr-notes").val());
            };
        });

        var $addItem = $("#projectRequestItems #addRemoveToolbar .toolbar-add");
        $addItem.unbind("click");
        $addItem.click(
            function () {
                var $itemsTable = $("#projectRequestItems #projectItems tbody");

                // $itemsTable.append('<tr><td><input type="checkbox" class="check-selected" /></td><td></td><td><input type="text"/></td><td></td><td></td><td></td><td></td></tr>');


                $itemsTable.append(["<tr class='item' process='", CONST.transaction_type.create, "'><td><input type='checkbox' value='0' class='check-selected id' /></td>",
                    "<td></td>",
                    "<td><input type='text' class='form-control text-box unit' caption='Unit' required='true'/></td>",
                    "<td><textarea class='form-control text-box description' caption='Item Description' required='true'></textarea></td>",
                    "<td><input type='text' class='form-control text-box quantity' caption='Quantity' number required='true'/></td>",
                    "<td><input type='text' class='form-control text-box unit-cost' caption='Unit Cost' money required='true'/></td>",
                    "<td><input type='text' class='form-control text-box total' money='true' readonly/></td>",
                    "</tr > "].join(''));

                Site.GenerateControls($itemsTable);
                $itemsTable.find("tr:last").on("change", function () { APPROVAL.ComputeItemTotalCost(this); });
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
                APPROVAL.ComputeItemTotalCost();
            }
        );
    },

    PopulateModal: function (_this) {

        ProjReqEditor.EnableFields();
        $("#btnSave").show();

        switch ($("#processId").text()) {
            case CONST.process_id.submit:
                ProjReqEditor.EnableFields();
                APPROVAL.EnableAttachment();
                break;
            default:
                ProjReqEditor.DisableFields();
                APPROVAL.DisableAttachment();
                $("#btnSave").hide();
                break;
        }

        var _id = $(_this).attr("data-itemid");
        var _desc = $("#row-" + _id).data("description");
        var _cat = $("#row-" + _id).data("category");
        var _grant = $("#row-" + _id).data("grantee");
        
        $("#u_id").val(_id);
        $("#proj_id").html(_id);
        $("#project_title").html($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#proj_substatus").html($("#row-" + _id).data("project-substatus"));
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#description").html(_desc);
        $("#requested_by").html(_desc);
        $("#category").html(_cat);
        $("#grantee").html(_grant);

        $("#approved_budget").val($("#row-" + _id).data("approved-budget"));
        $("#estimated_budget").html($("#row-" + _id).data("estimated-budget"));

        APPROVAL.GetAttachments(_id);
        APPROVAL.GetProjectItems(_id);

        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;

        if ($("#processId").text() !== "") {
            var param = {
                'id': $("#u_id").val(),
                'approved_budget': $("#approved_budget").val(),
                'earmark': $("#earmark").val(),
                'earmark_date': $("#earmark_date").val(),
                'source_fund': $("#source_fund").val(),
                'record_status': record_status,
                'user_action': user_action,
                'notes': notes
            };
        }

        if ($("#processId").html() === CONST.process_id.submit) {

            $("#formAttachments .attachmentItem").each(function () {
                var paramAttachments = APPROVAL.Clone(APPROVAL.DocumentAttachment);
                //paramAttachments.batch_id = DRAFT.BatchId;
                paramAttachments.process = $(this).attr("process");
                paramAttachments.guid = $(this).attr("guid");
                paramAttachments.id = $(this).attr("attachmentid");
                paramAttachments.barcode_no = $(this).find("input[type='text']").val();
                if (CONST.transaction_type.save == $(this).attr("process")) {
                    paramAttachments.file_name = [$(this).find("input[type='text']").val(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');
                }
                else {
                    paramAttachments.file_name = $(this).attr("filename");
                }
                paramAttachments.attachment_name = $(this).find(".attachmentName").html();
                paramAttachments.status = $(this).attr("status");
                APPROVAL.DocumentAttachments.push(paramAttachments)
                hasData = true;
            });
        }
        //filter out new attachments
        var _updateAttachments = APPROVAL.DocumentAttachments.filter(function (val) { return val.process != CONST.transaction_type.save; });

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': param,
            'item_list': item_list,
            'documentAttachments': _updateAttachments
        });

        if ($("#processId").html() === CONST.process_id.submit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleSubmit);
        } else if ($("#processId").html() === CONST.process_id.reject) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleReject(_result));
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.SourceFunds] }), "", populateViewList(_result));
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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(APPROVAL.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", (APPROVAL.EnableAttachments ? "" : "disabled"), "/></div>"].join(''));
                });

                if (APPROVAL.EnableAttachments) {
                    $("#formAttachments .attachmentItem").append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem'></button></div>");
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("process", CONST.record_status.cancel);
                    });
                }
            }
        });
    },
    UploadAttachment: function () {
        try {
            var data = $("body").data(APPROVAL.DocumentAttachments);
            if (data != null && data != undefined && data != "") {
                $(APPROVAL.Attachments).each(function () {

                    var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                    if ($(curFile).length == 0) {
                        return true;
                    }
                    var projectId = $("#u_id").val();
                    var fileName = [this.GUID, $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');

                    var opts = {
                        dir: [APPROVAL.UploadSubFolder, '\\', projectId].join('')
                    }

                    FileUploader.UploadDocument(this.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    var attachmentGuid = this.GUID.valueOf();
                    var attParam = Site.FindFirst(APPROVAL.DocumentAttachments, function (e) {
                        return e.guid == attachmentGuid;
                    })

                    if (FileUploader.Status && attParam) {

                        var _param = JSON.stringify(
                            {
                                'process': attParam.process,
                                'attachment_name': attParam.attachment_name,
                                'batch_id': attParam.batch_id,
                                'barcode_no': attParam.barcode_no,
                                'file_name': fileName,
                                'status': attParam.status,
                                'guid': attParam.guid,
                                'project_id': projectId
                            }
                        );
                        ajaxHelper.Invoke("/ProjectRequest/AddAttachment", _param, "json", function () { });
                    }

                });

            }
            return true;
        } catch (e) {
            return false;
        }
    },
    DisableAttachment: function () {
        APPROVAL.EnableAttachments = false;

        $("#btnBrowse").parent().hide();

        $("#btnBrowse").unbind("click");
        $("#fleBrowse").unbind("change");
        $("#formAttachments").unbind();
    },
    EnableAttachment: function () {
        APPROVAL.EnableAttachments = true;
        $("#btnBrowse").parent().show();
        $("#btnBrowse").click(function () {
            $("#fleBrowse").click();
        });

        $("#fleBrowse").change(function () {
            APPROVAL.ValidateAttachment(this.files[0], $(this).val());
        });

        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                APPROVAL.ValidateAttachment(blobData, pFile[0].name);
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
                    APPROVAL.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                APPROVAL.MaintainData(CONST.transaction_type.search, null, null, null);
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

    }

}

var populateViewList = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            var SourceFunds = $.grep(result.value, function (x, y) {
                return (x.type === CONST.setting_selection.SourceFunds);
            });
            $(SourceFunds).each(function () {
                $("#source_fund").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
            });

        }
    }
}

var handleSubmit = function (result) {
    if (result.status.code === 0) {
        if (APPROVAL.UploadAttachment()) {
            Site.Dialogs.Alert("Project Budget Approved! Please submit required documents <br> to BAC Secretariat for Consolidation of PPMP.", "Got It!", refreshData());
        }
        else {
            Site.Dialogs.Alert("Error Uploading documents.", "Close", refreshData());
        }
        
    } else {
        Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
    }
    $('.modal').modal('hide');
    $(".modal-backdrop").remove();
    
}

var handleReject = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project request successfully rejected.", "Got It!", refreshData());
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
        APPROVAL.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    APPROVAL.Initialize();
    APPROVAL.InitializeViewList();
});