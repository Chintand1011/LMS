var ONGOING = {
    Status: null,
    Active_ProjectSubStatus: null,
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
    ValidateAttachment: function (blob, val) {
        val = val.toLowerCase();
        var regex = new RegExp("(.*?)\.(jpg|png|gif|doc|pdf|docx|xls|xlsx|txt)$");
        if ((regex.test(val.toLowerCase()))) {
            var attachmentName = window.prompt("Please enter an attachment name", "");
            if (attachmentName == null || attachmentName == "") {
                $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
                return;
            }
            var param = ONGOING.Clone(ONGOING.AttachmentParam);
            param.AttachmentName = attachmentName;
            param.FileName = val;
            param.Blob = blob;
            ONGOING.Attachments.push(param);
            var imgCls = Utilities.GetFileTypeClass(val);
            $("#formAttachments").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<div class='attachmentName breakWord'>", attachmentName,
                "</div><div class='", imgCls, "' title='For upload'></div><input type='text' maxlength='20' /></div>"].join(''));


            if (ONGOING.EnableAttachments) {
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

        $(".item-setting-update").unbind("click");
        $(".item-setting-update").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.edit);
            ONGOING.PopulateModal(this);
        });

        $(".item-setting-submit-budget-approval").unbind("click");
        $(".item-setting-submit-budget-approval").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            ONGOING.PopulateModal(this);
        });

        $(".item-setting-view-bidderlist").unbind("click");
        $(".item-setting-view-bidderlist").click(function (e) {
            e.preventDefault();
            ONGOING.PopulateBidderListModal(this);
        });

        $(".item-setting-proceed-bid-opening").unbind("click");
        $(".item-setting-proceed-bid-opening").click(function (e) {
            e.preventDefault();
            ONGOING.Active_ProjectSubStatus = CONST.project_substatus.closed_shortlist;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Proceed to Opening of Bids Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-reopen-project").unbind("click");
        $(".item-setting-reopen-project").click(function (e) {
            e.preventDefault();
            ONGOING.Active_ProjectSubStatus = CONST.project_substatus.closed_shortlist;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Reopen Project for Bidding Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-twg-rank").unbind("click");
        $(".item-setting-twg-rank").click(function (e) {
            e.preventDefault();
            ONGOING.Active_ProjectSubStatus = CONST.project_substatus.opening_of_bids;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit for TWG Ranking Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-return-shortlist").unbind("click");
        $(".item-setting-return-shortlist").click(function (e) {
            e.preventDefault();
            ONGOING.Active_ProjectSubStatus = CONST.project_substatus.opening_of_bids;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Return for Shortlisting Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-prepare-abstract").unbind("click");
        $(".item-setting-prepare-abstract").click(function (e) {
            e.preventDefault();
            $("#u_id").val($(this).attr("data-itemid"));
            ONGOING.PopulateBidAbstractModal(this);
        });

        $(".item-setting-prepare-lcb").unbind("click");
        $(".item-setting-prepare-lcb").click(function (e) {
            e.preventDefault();
            alert("View prepare lcb notice");
        });

        $(".item-setting-twg-eval").unbind("click");
        $(".item-setting-twg-eval").click(function (e) {
            e.preventDefault();
            ONGOING.Active_ProjectSubStatus = CONST.project_substatus.lcb_notice;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-return-twg-rank").unbind("click");
        $(".item-setting-return-twg-rank").click(function (e) {
            ONGOING.Active_ProjectSubStatus = CONST.project_substatus.lcb_notice;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Return Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-attachment").unbind("click");
        $(".item-setting-attachment").click(function (e) {
            e.preventDefault();
            alert("View attachments");
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

        //$("#btnBrowse").click(function () {
        //    $("#fleBrowse").click();
        //});

        //$("#fleBrowse").change(function () {
        //    ONGOING.ValidateAttachment(this, $(this).val());
        //});

        //$("#formAttachments").unbind("filedrop");
        //$("#formAttachments").filedrop({
        //    callback: function (blobData, curId, pFile) {
        //        ONGOING.ValidateAttachment(blobData, pFile[0].name);
        //    }
        //});

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
            ONGOING.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            //var isValid = Site.ValidateRequiredEntries("#formGeneral", null);            
            //if (isValid == true) {
            //    isValid = $("#formAttachments .attachmentItem").length > 0;
            //    if (isValid == false) {
            //        Site.Dialogs.Alert("At lease one attachment is required.", "OK", null);
            //        return;
            //    }
            //}
            //TODO: File Upload
            if ($("#processId").text() === CONST.process_id.edit) {
                $("#modal-note-title").text("Validation Note");
                $("#ProjectRequestNoteModal").modal("show");
            } else if ($("#processId").text() === CONST.process_id.submit) {
                $("#modal-note-title").text("Submit Note");
                $("#ProjectRequestNoteModal").modal("show");
            }
        });

        $("#btnSaveNoteModal").click(function () {
            var _txn, _user_action;
            if ($("#processId").text() === CONST.process_id.edit) {
                _txn = CONST.transaction_type.save;
            } else if ($("#processId").text() === CONST.process_id.submit) {
                _txn = CONST.transaction_type.processUpdate;
                _user_action = CONST.user_action.approve;
            } else if ($("#processId").text() === CONST.process_id.return) {
                _txn = CONST.transaction_type.processUpdate;
                _user_action = CONST.user_action.return;
            }
            ONGOING.MaintainData(_txn, null, _user_action, null, $(".dr-notes").val());
        });
    },

    PopulateModal: function (_this) {

        ProjReqEditor.ShowClassification();
        ProjReqEditor.EnableFields();
        $("#btnSave").show();

        ONGOING.Active_ProjectSubStatus = $("#hf_project_substatus").val();

        switch ($("#processId").text()) {
            case CONST.process_id.view:
                ProjReqEditor.DisableFields();
                ONGOING.DisableAttachment();
                $("#btnSave").hide();
                break;
            case CONST.process_id.submit:
                ProjReqEditor.DisableFields();
                ONGOING.DisableAttachment();
                $("#btnSave").html("Submit");
                break;
            default:
                ProjReqEditor.EnableFields();
                ONGOING.EnableAttachment();
                $("#btnSave").html("Save");
                break;
        }

        var _id = $(_this).attr("data-itemid");

        $("#u_id").val(_id);
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#estimated_budget").val($(["tr#row-" + _id, " td[budget]"].join('')).html().trim());

        $("#required_date").val($("#row-" + _id).data("required-date"));
        $('#required_date').datepicker("setDate", $("#row-" + _id).data("required-date"));

        $("#description").val($("#row-" + _id).data("description"));
        $("#grantee").val($("#row-" + _id).data("grantee"));
        $("#category").val($("#row-" + _id).data("category"));
        $("#classification").val($("#row-" + _id).data("classification"));
        $("#contract_type").val($("#row-" + _id).data("contract-type"));
        $("#security_level").val($("#row-" + _id).data("security-level"));
        $("#delivery_type").val($("#row-" + _id).data("delivery-type"));

        ONGOING.GetAttachments(_id);
        ONGOING.GetProjectItems(_id);

        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();
    },

    PopulateBidderListModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);

        $("#bidlist_proj_id").html(_id);
        $("#bidlist_proj_title").html($("#row-" + _id).data("title"));
        $("#bidlist_proj_desc").html($("#row-" + _id).data("description"));
        $("#bidlist_approved_budget").html($("#row-" + _id).data("approved-budget"));
        $("#bidlist_requested_by").html($("#row-" + _id).data("created-by-name"));
        $("#bidlist_category").html($("#row-" + _id).data("category-desc"));

        ONGOING.GetBidderList(_id);

        $('#dataBidderListModal').modal('show');
    },

    PopulateBidAbstractModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);

        $("#dataBidAbstractModal #bid_proj_id").html(_id);
        $("#dataBidAbstractModal #proj_title").html($("#row-" + _id).data("title"));
        //$("#dataBidAbstractModal #proj_procmethod").html($("#row-" + _id).data("title"));
        //$("#dataBidAbstractModal #opening_place").html($("#row-" + _id).data("description"));
        //$("#dataBidAbstractModal #opening_date").html($("#row-" + _id).data("approved-budget"));
        //$("#dataBidAbstractModal #approved_budget").html($("#row-" + _id).data("created-by-name"));

        ONGOING.GetBidderList(_id);

        $('#dataBidAbstractModal').modal('show');
    },

    GetBidderList: function (id) {
        var _result;
        var _search = {
            'id': id
        };
        var _filter = JSON.stringify({
            'projectSearch': _search
        });
        ajaxHelper.Invoke("/ProjectRequest/GetProjectBid", _filter, "html", handleGetBidderList(_result));
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;
        var _param;

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
            _param = {
                'id': $("#u_id").val(),
                'title': $("#title").val(),
                'description': $("#description").val(),
                'grantee': $("#grantee").val(),
                'estimated_budget': $("#estimated_budget").val(),
                'required_date': $("#required_date").data('datepicker').getFormattedDate('dd-M-yyyy'),
                'category': $("#category").val(),
                'classification': $("#classification").val(),
                'contract_type': $("#contract_type").val(),
                'security_level': $("#security_level").val(),
                'delivery_type': $("#delivery_type").val(),
                'record_status': record_status,
                'user_action': user_action,
                'notes': notes
            };
        }
        //filter out new attachments
        var _updateAttachments = ONGOING.DocumentAttachments.filter(function (val) { return val.process != CONST.transaction_type.save; });

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': _param,
            'item_list': item_list,
            'documentAttachments': _updateAttachments
        });

        if ($("#processId").html() === CONST.process_id.edit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handlOnGoingEdit);
        } else if ($("#processId").html() === CONST.process_id.submit) {
            if (ONGOING.Active_ProjectSubStatus === CONST.project_substatus.closed_shortlist) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleBidOpening(_result));
            } else if (ONGOING.Active_ProjectSubStatus === CONST.project_substatus.opening_of_bids) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleSubmitTWGRanking(_result));
            } else if (ONGOING.Active_ProjectSubStatus === CONST.project_substatus.lcb_notice) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleSubmitTWGEvaluation(_result));
            } else {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleOnGoingSubmit(_result));
            }
        } else if ($("#processId").html() === CONST.process_id.return) {
            if (ONGOING.Active_ProjectSubStatus === CONST.project_substatus.opening_of_bids) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleReturnForShortlisting(_result));
            } else if (ONGOING.Active_ProjectSubStatus === CONST.project_substatus.lcb_notice) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleReturnforReRanking(_result));
            } else {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleReOpenBid(_result));
            }
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList",
            JSON.stringify({
                "setting_list": [
                    CONST.setting_selection.ProjectCategory,
                    CONST.setting_selection.ProjectClassification,
                    CONST.setting_selection.ContractType,
                    CONST.setting_selection.DocumentSecurityLevel,
                    CONST.setting_selection.Delivery
                ]
            }),
            "", ONGOING.populateViewList(_result));
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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", (ONGOING.EnableAttachments ? "" : "disabled"), "/></div>"].join(''));
                });

                if (ONGOING.EnableAttachments) {
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
            var data = $("body").data(ONGOING.DocumentAttachments);
            if (data != null && data != undefined && data != "") {
                $(ONGOING.Attachments).each(function () {

                    var projectId = $("#u_id").val();

                    var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                    var fileName = [projectId, "_", this.AttachmentName, $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');

                    var opts = {
                        dir: [ONGOING.UploadSubFolder, '\\', projectId].join('')
                    }

                    FileUploader.UploadDocument(this.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing
                    var attachmentGuid = this.GUID.valueOf();
                    var attParam = Site.FindFirst(ONGOING.DocumentAttachments, function (e) {
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
        ONGOING.EnableAttachments = false;

        $("#btnBrowse").parent().hide();
        $("#btnBrowse").unbind("click");

        $("#fleBrowse").unbind("change");
        $("#formAttachments").unbind();
    },
    EnableAttachment: function () {
        ONGOING.EnableAttachments = true;
        $("#btnBrowse").parent().show();
        $("#btnBrowse").click(function () {
            $("#fleBrowse").click();
        });

        $("#fleBrowse").change(function () {
            ONGOING.ValidateAttachment(this.files[0], $(this).val());
        });

        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                ONGOING.ValidateAttachment(blobData, pFile[0].name);
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
                    ONGOING.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                ONGOING.MaintainData(CONST.transaction_type.search, null, null, null);
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
                $itemsTable.find("input.quantity, input.unit-cost").on("change", function () { ONGOING.ComputeItemTotalCost(this); });

                $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(grandTotal));


                switch ($("#processId").text()) {
                    case CONST.process_id.view:
                        ProjReqItems.Disable();
                        break;
                    case CONST.process_id.submit:
                        ProjReqItems.Disable();
                        break;
                    default:
                        ProjReqItems.Enable();
                        break;
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
    populateViewList : function (result) {
        return function (result) {
            if (result.status.code === 0) {
                var ProjectCategoryList = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ProjectCategory);
                });
                $(ProjectCategoryList).each(function () {
                    $("#category").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });

                var ProjectClassification = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ProjectClassification);
                });
                $(ProjectClassification).each(function () {
                    $("#classification").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });

                var ContractType = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ContractType);
                });
                $(ContractType).each(function () {
                    $("#contract_type").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });

                var DocumentSecurityLevel = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.DocumentSecurityLevel);
                });
                $(DocumentSecurityLevel).each(function () {
                    $("#security_level").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });

                var Delivery = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.Delivery);
                });
                $(Delivery).each(function () {
                    $("#delivery_type").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });
            }
        }
    }
}


var handleGetBidderList = function (result) {
    return function (result) {
        $("#div_bidder_list").html(result);
        $("#table_bidder_list .bid-tools").hide();
    }
}

var handlOnGoingEdit = function (result) {
    if (result.status.code === 0) {
        if (ONGOING.UploadAttachment()) {
            Site.Dialogs.Alert("Project request information successfully updated. You can <br> now submit project request for Budget Approval.", "Got It!", refreshData());
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

var handleOnGoingSubmit = function (result) {
    return function (result) {        
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project succussfully submitted for Budget Approval. <br> Thank you.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var handleBidOpening = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project has been endorsed to Opening of Bids.<br> Submitted Bid Documents will now be ranked<br> and evaluated to determine the bid winner.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var handleReOpenBid = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project has been Reopened for Bidding.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var handleReturnForShortlisting = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project successfully returned to Shortlisting.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var handleSubmitTWGRanking = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project successfully submitted for TWG Ranking.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var handleSubmitTWGEvaluation = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project successfully submitted for TWG Evaluation.", "Got It!", refreshData());
        } else {
            Site.Dialogs.Alert("Something went wrong, please try again", "Ok", refreshData());
        }
        $('.modal').modal('hide');
        $(".modal-backdrop").remove();
    }
}

var handleReturnforReRanking = function (result) {
    return function (result) {
        if (result.status.code === 0) {
            Site.Dialogs.Alert("Project successfully returned for TWG Ranking.", "Got It!", refreshData());
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
        ONGOING.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    ONGOING.Initialize();
    ONGOING.InitializeViewList();
});