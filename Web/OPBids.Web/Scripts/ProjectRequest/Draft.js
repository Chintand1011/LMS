var DRAFT = {
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
    ValidateAttachment: function (blob, val) {
        val = val.toLowerCase();
        var regex = new RegExp("(.*?)\.(jpg|png|gif|doc|pdf|docx|xls|xlsx|txt)$");
        if ((regex.test(val.toLowerCase()))) {
            var attachmentName = window.prompt("Please enter an attachment name", "");
            if (attachmentName == null || attachmentName == "") {
                $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
                return;
            } 
            var param = DRAFT.Clone(DRAFT.AttachmentParam);
            param.GUID = moment(new Date()).format("YYYYMMDDhhmmssSS");
            param.AttachmentName = attachmentName;
            param.FileName = val;
            param.Blob = blob;
            DRAFT.Attachments.push(param);

            var imgCls = Utilities.GetFileTypeClass(val);

            $("#formAttachments").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<div class='attachmentName breakWord'>", attachmentName,
                "</div><div class='", imgCls, "' title='For upload'></div><input type='text' maxlength='20' /></div>"].join(''));

            
           
            if (DRAFT.EnableAttachments) {
                $(["#formAttachments .attachmentItem[guid='", param.GUID, "']"].join('')).append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem btn btn-danger btn-block'>REMOVE</button></div>");
                $(["#formAttachments .attachmentItem[guid='", param.GUID, "'] .removeItem"].join('')).click(function () {
                    $(this).parent().parent().remove();
                });
            }
            
        }
        else {
            Site.Dialogs.Alert("Invalid file extension, please upload a file with jpg, png, gif, doc, pdf, docx, xls, xlsx, or txt file extensions.", "OK", null);
        }
        $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
    },

    Initialize: function () {
        window.title = $("#headerTitle").html();
        $(document).attr("title", $("#headerTitle").html());


        $("#toolbar-add-text").css({"color":"#4047cc"}).html("New Request");
        $("#toolbar-delete-text").html("Cancel Selected Drafts");                

        $("#toolbarContent").css({ "display": "block" });        
        $("#toolbarContent .toolbar-add-batch").hide();

        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();            
            ProjReqEditor.EnableFields();
            $("#formAttachments .attachmentItem").remove();
            Site.ClearAllData('#dataEntryModal');
            $("#u_id").val("0");
            $("#projectIdContainer").hide();
            $("#processId").html(CONST.process_id.create_new);
            $('#dataEntryModal #btnSave').show();
            $('#dataEntryModal #btnSubmit').hide();
            $('#dataEntryModal').modal('show');
            $("#dataEntryModal *[required]").keyup();
            $(".sectionTab").removeAttr("selected");
            $("#generalTab").parent().attr("selected", "");
            $("#generalTab").click();
            DRAFT.EnableAttachment();

            $("#projectRequestItems #projectItems tbody").empty();
            $("#required_date").val($.datepicker.formatDate("dd-M-yy", new Date()));
            DRAFT.ClearProjectItems();
        });

        $(".toolbar-delete").unbind("click");
        $(".toolbar-delete").click(function () {
            var _item_list = [];
            $("#processId").html(CONST.process_id.edit);

            $(".check-selected:checked").each(function (e) {
                _item_list.push($(this).attr("id"));
            });
            if (_item_list.length <= 0) {
                Site.Dialogs.Alert("Cannot delete. Please select which one should be deleted.");
                return;
            }
            DRAFT.MaintainData(CONST.transaction_type.statusUpdate, CONST.record_status.delete, null, _item_list);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            $("#processId").html(CONST.transaction_type.search);
            DRAFT.MaintainData(CONST.transaction_type.search, null, null, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
            alert("clicked print");
        });

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.edit);
            DRAFT.PopulateModal(this, true, false);
        });

        $(".item-setting-cancel").unbind("click");
        $(".item-setting-cancel").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.cancel);
            var _item_list = [$(this).data("itemid")];
            DRAFT.MaintainData(CONST.transaction_type.statusUpdate, CONST.record_status.cancel, null, _item_list);
        });

        $(".item-setting-newdraft").unbind("click");
        $(".item-setting-newdraft").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.create_new);
            DRAFT.PopulateModal(this, false, true);
        });

        $(".item-setting-submit").unbind("click");
        $(".item-setting-submit").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            DRAFT.PopulateModal(this, true, false);
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
            PROJECT.SearchParam.category = $("#search_category").val();
            PROJECT.SearchParam.project_name = $("#search_project_name").val();
            PROJECT.SearchParam.id = $("#search_draft_no").val();                     
            PROJECT.SearchParam.get_total = true;
            DRAFT.MaintainData(CONST.transaction_type.search, null, null, null);   
            PROJECT.SearchParam.get_total = false;
        });

        $(".sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
            if (($(this).attr("formid") == "formGeneral") || ($(this).attr("formid") == "formItems")){
                $("#footerNote").html("Projects will be saved as draft and will not yet be submitted to BAC.");
            }
            else if ($(this).attr("formid") == "formAttachments") {
                $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
            }
        });

        $("#generalTab").click();
        Site.GenerateControls("#dataEntryModal");

        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            var _txn, _user_action;
            if ($("#processId").text() === CONST.process_id.edit || $("#processId").text() === CONST.process_id.create_new) {
                _txn = CONST.transaction_type.save;
            }

            if (Site.ValidateRequiredEntries("#dataEntryModal", null) == true) {
                DRAFT.MaintainData(_txn, null, _user_action, null);
                $('#dataEntryModal').modal('hide');
            }            
        });

        $("#btnSubmit").unbind("click");
        $("#btnSubmit").click(function () {
            var _txn, _user_action;
            _txn = CONST.transaction_type.processUpdate;
            _user_action = CONST.user_action.approve;

            if (Site.ValidateRequiredEntries("#dataEntryModal", null) == true) {
                DRAFT.MaintainData(_txn, null, _user_action, null);
                $('#dataEntryModal').modal('hide');
            }
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
                    "<td><input type='text' class='form-control text-box unit-cost money' caption='Unit Cost' money required='true'/></td>",
                    "<td><input type='text' class='form-control text-box total' money='true' readonly/></td>",
                    "</tr > "].join(''));

                Site.GenerateControls($itemsTable);
                $itemsTable.find("tr:last").on("change", function () { DRAFT.ComputeItemTotalCost(this); });
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
                    if ($chkId.is(":checked"))
                    {
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
                DRAFT.ComputeItemTotalCost();
            }
        );
        

    },

    PopulateModal: function (_this, showDraftNo, showFootNote) {
        ProjReqEditor.EnableFields();
        $("#dataEntryModal #btnSave").hide();
        $("#dataEntryModal #btnSubmit").hide();

        if ($("#processId").text() === CONST.process_id.submit) {
            ProjReqEditor.DisableFields();
            DRAFT.DisableAttachment();
            $("#dataEntryModal #btnSubmit").show();
        }
        else {
            DRAFT.EnableAttachment();
            $("#dataEntryModal  #btnSave").show();
        }

        var _id = $(_this).attr("data-itemid");
        var _desc = $("#row-" + _id).data("description");
        var _cat = $("#row-" + _id).data("category");
        var _grant = $("#row-" + _id).data("grantee");
        var _grant_name = $("#row-" + _id).data("grantee-name");
        var _pr = $("#row-" + _id).data("pr-number");
        var _req_date = $("#row-" + _id).data("requiredate");

        $("#u_id").val(_id);
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        //$("#grantee").val($(["tr#row-" + _id, " td[grantee]"].join('')).html().trim());
        $("#estimated_budget").val($(["tr#row-" + _id, " span[budget]"].join('')).text().trim());
        $("#required_date").val($.datepicker.formatDate("dd-M-yy", new Date(_req_date.trim())));
        
        var $select = $('#grantee').selectize();
        var selectize = $select[0].selectize;
        selectize.addOption({ value : _grant_name, key: _grant });
        selectize.setValue(_grant);
        //selectize.refreshOptions();
        

     
        if (showDraftNo == true) {
            $("#projectId").html(_id);
            $("#projectIdContainer").show();
        }
        else {
            $("#projectIdContainer").hide();
        }

        if (showFootNote == true) {
            $("#footerNote").show();
        }
        else {
            $("#footerNote").hide();
        }

        $("#description").val(_desc);
        $("#category").val(_cat);
        $("#grantee").val(_grant);
        $("#pr_number").val(_pr);

        DRAFT.GetAttachments(_id);
        DRAFT.GetProjectItems(_id);

        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();
        
        $('#dataEntryModal .pdflink').attr("href", "/shared/getPdf/" + _id);
    },

    MaintainData: function (process, record_status, user_action, item_list) {
        var _result;
        var _id = $("#u_id").val();

        if ($("#processId").text() === CONST.process_id.create_new) {
            _id = 0;
        }

        var param;        
        var _updateAttachments;
        if ($("#processId").html() === CONST.process_id.edit || $("#processId").html() === CONST.process_id.create_new) {

            param = {
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

            $("#formAttachments .attachmentItem").each(function () {
                var paramAttachments = DRAFT.Clone(DRAFT.DocumentAttachment);
                //paramAttachments.batch_id = DRAFT.BatchId;
                paramAttachments.process = $(this).attr("process");
                paramAttachments.guid = $(this).attr("guid");
                paramAttachments.id = $(this).attr("attachmentid");
                paramAttachments.barcode_no = $(this).find("input[type='text']").val();
                if (CONST.transaction_type.save == $(this).attr("process")) {
                    paramAttachments.file_name = [$(this).find(".attachmentName").html(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');
                }
                else {
                    paramAttachments.file_name = $(this).attr("filename");
                }
                paramAttachments.attachment_name = $(this).find(".attachmentName").html();
                paramAttachments.status = $(this).attr("status");
                DRAFT.DocumentAttachments.push(paramAttachments);
                hasData = true;
            });

            DRAFT.ProjectItems = [];
            var processId = $("#processId").html();
            if (processId === CONST.process_id.edit) {

                $("#projectRequestItems #projectItems tr.item").each(function () {


                    var item = DRAFT.Clone(DRAFT.ProjectItem);

                    var $tr = $(this);

                    item.id = $tr.find("input.id").val();

                    item.process = $tr.attr("process");
                    item.unit = $tr.find("input.unit").val();
                    item.description = $tr.find("textarea.description").val();
                    item.quantity = $tr.find("input.quantity").val();
                    item.unit_cost = $tr.find("input.unit-cost").val();
                    DRAFT.ProjectItems.push(item);
                });

            }
            else if (processId === CONST.process_id.create_new) {

                $("#projectRequestItems #projectItems tr.item").each(function () {
                    var $tr = $(this);

                    var item = DRAFT.Clone(DRAFT.ProjectItem);
                    //new
                    //item.process = CONST.transaction_type.create;
                    item.process = $tr.attr("process");
                    item.id = 0;
                    item.unit = $tr.find("input.unit").val();
                    item.description = $tr.find("textarea.description").val();
                    item.quantity = $tr.find("input.quantity").val();
                    item.unit_cost = $tr.find("input.unit-cost").val();
                    DRAFT.ProjectItems.push(item);

                });
            }

            //filter out new attachments
            _updateAttachments = DRAFT.DocumentAttachments.filter(function (val) { return val.process == CONST.transaction_type.save || val.process == CONST.record_status.cancel; });
        } else {
            param = {
                'id': _id,
                'record_status': record_status,
                'user_action': user_action
            };
        }
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': param,
            'status': record_status,
            'item_list': item_list,
            'documentAttachments': _updateAttachments,
            'projectItems' : DRAFT.ProjectItems
        });

        if ($("#processId").html() === CONST.process_id.create_new) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleDraftCreate);
        } else if ($("#processId").html() === CONST.process_id.edit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleDraftEdit);//.handleDraftEdit(_result)
        } else if ($("#processId").html() === CONST.process_id.submit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleDraftSubmit(_result));
        } else if ($("#processId").html() === CONST.process_id.cancel) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", handleDraftCancel(_result));
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }        
        $(".modal-backdrop").remove();       
    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.ProjectCategory] }), "", populateViewList(_result));
        //ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.ProjectGrantee] }), "", populateGrantees);
        populateGrantees();
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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(DRAFT.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' class='barcodeInput' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", (DRAFT.EnableAttachments ? "" : "disabled"), "/></div>"].join(''));
                });
                $("#formAttachments .barcodeInput").unbind("change");
                $("#formAttachments .barcodeInput").change(function () {
                    
                    $(this).parent().attr("process", CONST.transaction_type.save);
                });
                if (DRAFT.EnableAttachments) {
                    $("#formAttachments .attachmentItem").append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem btn btn-danger btn-block' multiupload>REMOVE</button></div>");
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function () {
                        $(this).parent().parent().css({ "display": "none" }).attr("process", CONST.record_status.cancel);
                    });
                }
                
            }
        });
    },

    UploadAttachment : function () {
        try {
            var data = $("body").data(DRAFT.DocumentAttachments);
            if (data != null && data != undefined && data != "") {
                $(DRAFT.Attachments).each(function () {

                    var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                    if ($(curFile).length == 0) {
                        return true;
                    }
                    var projectId = $("#u_id").val();
                    var fileName = [this.GUID, $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');

                    var opts = {
                        dir: [DRAFT.UploadSubFolder, '\\', projectId].join('')
                    }

                    FileUploader.UploadDocument(this.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    var attachmentGuid = this.GUID.valueOf();
                    var attParam = Site.FindFirst(DRAFT.DocumentAttachments, function (e) {
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
        DRAFT.EnableAttachments = false;

        $("#btnBrowse").parent().hide();

        $("#btnBrowse").unbind("click");
        $("#fleBrowse").unbind("change");
        $("#formAttachments").unbind();
    },
    EnableAttachment: function () {
        DRAFT.EnableAttachments = true;
        $("#btnBrowse").parent().show();

        $("#btnBrowse").off("click").on("click", function () {
            $("#fleBrowse").click();
        });

        $("#fleBrowse").off("change").on("change", function () {
            DRAFT.ValidateAttachment(this.files[0], $(this).val());
        });

        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function (blobData, curId, pFile) {
                DRAFT.ValidateAttachment(blobData, pFile[0].name);
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
                    $("#processId").html(CONST.transaction_type.search);
                    PROJECT.SearchParam.page_index = page;
                    DRAFT.MaintainData(CONST.transaction_type.search, null, null, null);
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
                DRAFT.MaintainData(CONST.transaction_type.search, null, null, null);
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
                        "<td><input type='text' class='form-control text-box unit' caption='Unit' value='" + this.unit +  "' required='true'/></td>",
                        "<td><textarea class='form-control text-box description' caption='Item Description' required='true'>" + this.description +  "</textarea></td>",
                        "<td><input type='text' class='form-control text-box quantity' caption='Quantity' number value='" + this.quantity +  "' required='true'/></td>",
                        "<td><input type='text' class='form-control text-box unit-cost' caption='Unit Cost' money value='" + this.unit_cost +  "' required='true'/></td>",
                        "<td><input type='text' class='form-control text-box total' money='true' value='" + Site.ToDecimalString(itemTotal) + "' readonly/></td>",
                        "</tr > "].join(''));
                });
               
                Site.GenerateControls($itemsTable);
                $itemsTable.find("input.quantity, input.unit-cost").on("change", function () { DRAFT.ComputeItemTotalCost(this); });

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
            if ((totalValue) && isNaN(totalValue) == false) {
                grandTotal += parseFloat(totalValue);
            }
        });

        $unitsGrandTotal.html(Site.ToDecimalString(grandTotal));

    },

    ClearProjectItems: function () {
        var $itemsTable = $("#projectRequestItems #projectItems tbody");
        $itemsTable.empty();
        $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(0.00));
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
    
    
    $('#grantee').selectize({
        create: function (input, callback) {
            return {
                'key': '{"k":"-1","v":"' + input + '"}', 'value': input
            };
        },
        //persist: false,
        selectOnTab: true,
        closeAfterSelect: true,
        valueField: 'key',
        labelField: 'value',
        searchField: 'value',
        preload: true,
        loadingClass: null,
        placeholder: '-- Please Select --',
        copyClassesToDropdown: false,
        load: function (query, callback) {
            var settingList = [CONST.setting_selection.ProjectGranteeAuto];
            if (query === "") {
                settingList = [CONST.setting_selection.ProjectGranteePrevUsed];
            }

            $.ajax({
                url: "/Shared/GetSettingsList",
                data: { setting_list: settingList, search_key: query, page_index: -1 },
                dataType: "json",
                type: 'POST',
                
                error: function () {
                    callback();
                },
                success: function (res) {
                   
                    if (res.status.code === 0) {

                        var ProjectGranteeList = $.grep(res.value, function (x, y) {
                            return (x.type === CONST.setting_selection.ProjectGrantee && x.key != "");
                        });
                        console.log(ProjectGranteeList);
                        callback(ProjectGranteeList);
                    }
                    else {
                        console.log(0);
                        callback();
                    }


                }
            });
        }
    });
}

var handleDraftCreate = function (result) {
   
    if (result.status.code == 0) {
        $("#u_id").val(result.value); 
        if (DRAFT.UploadAttachment()) {
            Site.Dialogs.Alert("Project Request Successully Created! You can now <br> view this project draft under Drafts Menu <br> with draft number " + result.value + ". Thank you.", "Got It", refreshData());
        }
        else {
            Site.Dialogs.Alert("Error Uploading documents.", "Close", refreshData());
        }
    }
    else {
        Site.Dialogs.Alert("Error Saving Project Request.", "Close", refreshData());
    }
}
var handleDraftEdit = function (result) {
    if (result.status.code == 0) {
        if (DRAFT.UploadAttachment()) {
            Site.Dialogs.Alert("Draft Successfully Updated.", "Got It!", refreshData());
        }
        else {
            Site.Dialogs.Alert("Error Uploading documents.", "Close", refreshData());
        }
    }
    else {
        Site.Dialogs.Alert("Error Saving Draft.", "Close", refreshData());
    }
}
var handleDraftSubmit = function (result) {
    return function (result) {   
        var today = new Date();

        var dd = today.getDate();
        var mm = today.getMonth() + 1;
        var yyyy = today.getFullYear();

        if (dd < 10) { dd = '0' + dd; }
        if (mm < 10) { mm = '0' + mm; }

        var _date = yyyy + '-' + mm + '-' + dd

        Site.Dialogs.Alert("Project Request Successully Submitted! <br>You can now check the status under Ongoing Projects. <br>Date Submitted: " + _date + "<br> Reference No. " + result.value, "Got It", refreshData());
    }
}

var handleDraftCancel = function (result) {
    return function (result) {
        Site.Dialogs.Alert("Draft Successfully Cancelled.", "Got It!", refreshData());
    }
}

var refreshData = function () {
    return function () {
        $("#processId").html(CONST.transaction_type.search);
        DRAFT.MaintainData(CONST.transaction_type.search, null, null, null);
    }
}

$(function () {
    DRAFT.Initialize();
    DRAFT.InitializeViewList();
});