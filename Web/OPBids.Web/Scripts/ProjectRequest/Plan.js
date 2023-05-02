var PLAN = {
    Status: null,
    UploadSubFolder: 'ProjectRequestBatch',
    Active_ProjectSubStatus: null,
    Initialize: function () {
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());

        $(".toolbar-content").css({ "display": "block" });
        $("#toolbarContent .toolbar-add").hide();
        $("#toolbarContent .toolbar-delete").hide();

        $(".toolbar-add-batch").unbind("click");
        $(".toolbar-add-batch").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#dataEntryModal');
            $("#u_id").val("0");
            $("#processId").html(CONST.process_id.create_new);
            PLAN.PopulateModal(this);
        });

        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            PLAN.PopulateBatchInfoModal(this);
        });

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.edit);
            PLAN.PopulateModal(this);
        });

        $(".item-setting-submit-hope-approval").unbind("click");
        $(".item-setting-submit-hope-approval").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            PLAN.Active_ProjectSubStatus = CONST.project_substatus.batch_consolidation;     
            
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);

            $("#modal-note-title").text("Submit to HoPE for Approval");
            $("#ProjectRequestNoteModal").modal("show");

        });

        $(".item-setting-cancel").unbind("click");
        $(".item-setting-cancel").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.cancel);
            PLAN.PopulateModal(this);
        });

        $(".item-setting-invite").unbind("click");
        $(".item-setting-invite").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.invite);
            PLAN.PopulateModalBidding(this);
        });
        
        $(".item-setting-view-invite").unbind("click");
        $(".item-setting-view-invite").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            PLAN.PopulateModalBidding(this);
        });

        $(".item-setting-submit-advertising").unbind("click");
        $(".item-setting-submit-advertising").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            PLAN.Active_ProjectSubStatus = CONST.project_substatus.bid_invitation;

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);

            $("#modal-note-title").text("Submit to Advertising Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        $(".item-setting-advertise").unbind("click");
        $(".item-setting-advertise").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.edit);
            PLAN.Active_ProjectSubStatus = CONST.project_substatus.advertise;
            ADVERTISEMENT.ShowModal(this);
        });

        $(".item-setting-close-shortlist").unbind("click");
        $(".item-setting-close-shortlist").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            PLAN.Active_ProjectSubStatus = CONST.project_substatus.advertise;

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);

            $("#modal-note-title").text("Close for Shortlisting Note");
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


        $("#btnMoveRight").unbind("click");
        $("#btnMoveRight").click(function (e) {
            e.preventDefault();
            PLAN.MoveSelection("#batch_selection_from", "#batch_selection_to");
        });

        $("#btnMoveLeft").unbind("click");
        $("#btnMoveLeft").click(function (e) {
            e.preventDefault();
            PLAN.MoveSelection("#batch_selection_to", "#batch_selection_from");
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
            PROJECT.SearchParam.project_name = $("#search_project_name").val();
            PROJECT.SearchParam.id = $("#search_ref_no").val();

            PROJECT.SearchParam.applicable_year = $("#search_app_year").val();
            PROJECT.SearchParam.batch_id = $("#search_batch_id").val();
            PROJECT.SearchParam.barcode = $("#search_barcode").val();

            PROJECT.SearchParam.get_total = true;
            PLAN.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });


        $("#dataEntryModal #btnSave").unbind("click");
        $("#dataEntryModal #btnSave").click(function () {
            if (Site.ValidateRequiredEntries("#dataEntryModal", null) == true) {
                var _txn, _record_status, _user_action, _item_list;
                if ($("#processId").text() === CONST.process_id.create_new || $("#processId").text() === CONST.process_id.edit) {
                    _txn = CONST.transaction_type.save;
                    _item_list = PLAN.GetSelection();
                } else if ($("#processId").text() === CONST.process_id.submit) {
                    _user_action = CONST.user_action.approve;
                    _txn = CONST.transaction_type.processUpdate;
                } else if ($("#processId").text() === CONST.process_id.cancel) {
                    _record_status = CONST.record_status.cancel;
                    _txn = CONST.transaction_type.statusUpdate;
                } else {
                    _txn = CONST.transaction_type.statusUpdate;
                }

                PLAN.MaintainData(_txn, _record_status, _user_action, _item_list);
            }
        });

        $("#dataEntryBiddingModal #btnBidSave").unbind("click");
        $("#dataEntryBiddingModal #btnBidSave").click(function () {
            if (Site.ValidateRequiredEntries("#dataEntryBiddingModal", null) == true) {
                if ($("#processId").text() === CONST.process_id.invite) {
                    $("#modal-note-title").text("Invitation Note");
                    $("#ProjectRequestNoteModal").modal("show");
                };
            }
        });

        $("#dataAdvertisementModal #btnAdvertiseSave").unbind("click");
        $("#dataAdvertisementModal #btnAdvertiseSave").click(function () {
            if (Site.ValidateRequiredEntries("#dataAdvertisementModal", null) == true) {
                $("#processId").html(CONST.process_id.edit);
                PLAN.Active_ProjectSubStatus = CONST.project_substatus.advertise;
                PLAN.MaintainData(CONST.transaction_type.save, null, null, null, null);
            }
        });        

        $("#ProjectRequestNoteModal #btnSaveNoteModal").unbind("click");
        $("#ProjectRequestNoteModal #btnSaveNoteModal").click(function () {
            if (Site.ValidateRequiredEntries("#ProjectRequestNoteModal", null) == true) {
                var _txn, _record_status, _user_action;
                if ($("#processId").text() === CONST.process_id.invite) {
                    _txn = CONST.transaction_type.save;
                } else if ($("#processId").text() === CONST.process_id.submit) {
                    _txn = CONST.transaction_type.processUpdate;
                    _user_action = CONST.user_action.approve;
                }
                PLAN.MaintainData(_txn, _record_status, _user_action, null, $(".dr-notes").val());
            }
        });

        Site.GenerateControls("#dataEntryModal");
        Site.GenerateControls("#dataEntryBiddingModal");
        Site.GenerateControls("#ProjectRequestNoteModal");
    },

    PopulateModal: function (_this) {
        
        BATCHSELECTION.EnableFields();
        $("#dataEntryModal #btnCancel").show();
        $("#dataEntryModal #btnSave").show();        

        var icon = $("<i>", { class: "fa fa-check fa-dialog" });
        switch ($("#processId").text()) {
            case CONST.process_id.create_new:
                $("#btnSave").html(icon).append(" Save");
                break;
            case CONST.process_id.edit:
                $("#btnSave").html(icon).append(" Save");
                break;
            case CONST.process_id.view:
                BATCHSELECTION.DisableFields();
                $("#btnSave").hide();
                break;
            case CONST.process_id.submit:
                BATCHSELECTION.DisableFields();
                $("#btnSave").html(icon).append(" Submit");
                break;
            case CONST.process_id.cancel:
                BATCHSELECTION.DisableFields();
                $("#btnCancel").hide();
                $("#btnSave").html(icon).append(" Cancel Batch");
                break;
            default:
                BATCHSELECTION.DisableFields();
                $("#btnSave").hide();
                break;
        }

        var _id = $(_this).attr("data-itemid");
        if (_id !== null || _id !== undefined) {
            $("#u_id").val(_id);            
            $("#batch_applicable_year").val($("#row-" + _id).data("applicable-year"));
            $("#batch_procurement_method").val($("#row-" + _id).data("procurement-method"));                        
        } else {
            $("#u_id").val("0");
        }
        PLAN.Active_ProjectSubStatus = CONST.project_substatus.batch_consolidation;

        PLAN.GetProjectList($("#u_id").val());

        $('#dataEntryModal').modal('show');
    },

    PopulateBatchInfoModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);
        $("#dataBatchInfoModal #modal_batch_id").html(_id);
        $("#dataBatchInfoModal #modal_proc_method").html($("#row-" + _id).data("procurement-method-desc"));
        $("#dataBatchInfoModal #modal_proc_status").html($("#row-" + _id).data("project-substatus-desc"));
        $("#dataBatchInfoModal #batch_total_projects").html($("#row-" + _id).data("total-projects"));
        $("#dataBatchInfoModal #batch_total_amount").html($("#row-" + _id).data("total-amount"));
        $("#dataBatchInfoModal #batch_date_created").html($("#row-" + _id).data("created-date") + " (" + $("#row-" + _id).data("created-by") + ")");

        PLAN.GetProjectList($("#u_id").val());
        
        $('#dataBatchInfoModal').modal('show');
    },

    PopulateModalBidding: function (_this) {

        switch ($("#processId").text()) {
            case CONST.process_id.view:
                BIDINVITATION.DisableFields();
                $("#dataEntryBiddingModal #btnBidSave").hide();
                break;
            case CONST.process_id.invite:
                BIDINVITATION.EnableFields();
                $("#dataEntryBiddingModal #btnBidSave").show();
                break;
            default:
                break;
        }

        var _id = $(_this).attr("data-itemid");

        if (_id !== null || _id !== undefined) {
            $("#u_id").val(_id);
            $("#batch_total_projects").html($("#row-" + _id).data("total-projects"));
            $("#batch_created_date").html($("#row-" + _id).data("created-date"));
            $("#modal_bid_proc_method").html($("#row-" + _id).data("procurement-method-desc"));
            $("#modal_bid_batch_id").html(_id);
            
            var itb_data = '{' +
                '"pre_bid_date": "' + $("#row-" + _id).data("pre-bid-date") + '", ' +
                '"pre_bid_time": "' + $("#row-" + _id).data("pre-bid-time") + '", ' +
                '"pre_bid_place": "' + $("#row-" + _id).data("pre-bid-place") + '", ' +

                '"deadline_date": "' + $("#row-" + _id).data("deadline-date") + '", ' +
                '"deadline_time": "' + $("#row-" + _id).data("deadline-time") + '", ' +
                '"deadline_place": "' + $("#row-" + _id).data("deadline-place") + '", ' +

                '"opening_date": "' + $("#row-" + _id).data("opening-date") + '", ' +
                '"opening_time": "' + $("#row-" + _id).data("opening-time") + '", ' +
                '"opening_place": "' + $("#row-" + _id).data("opening-place") +
                '"}';
            INVITATIONTOBID.SetData(JSON.parse(itb_data));
        } else {
            $("#u_id").val("0");
        }
        PLAN.Active_ProjectSubStatus = CONST.project_substatus.bid_invitation;

        $('#dataEntryBiddingModal').modal('show');
    },

    MoveSelection: function (source, target) {
        $(source + ' > tbody  > tr').each(function (key, value) {
            var _selected = $(value).find("#cbx_select");
            if ($(_selected).is(':checked')){
                $(target + ' > tbody').append(value);
            }            
        });
        PLAN.RefreshSummary();
    },

    GetSelection: function () {
        var _list = [];
        $('#batch_selection_to > tbody  > tr').each(function (key, value) {
            _list.push($(value).attr("id"));
        }); 
        
        return _list;
    },

    RefreshSummary: function () {
        var _count = 0;
        var _amount = 0;
        $("#batch_selection_to").find(".project-amount").each(function (e) {
            _count = _count + 1;
            _amount = _amount + parseFloat($(this).html().replace(/,/g, ''));
        });
        $("#dataEntryModal #batch_total_count").html(_count);
        $("#dataEntryModal #batch_total_amount").html("₱ " + Site.ToDecimalString(_amount));
    },

    GetProjectList: function (id) {
        var _result;

        if ($("#processId").text() === CONST.process_id.view) {
            var _search = {
                'batch_id': id,
            };
            var _filter = JSON.stringify({
                'projectSearch': _search
            });
            ajaxHelper.Invoke("/ProjectRequest/GetProjectRequestForBatch", _filter, "json", PLAN.HandleGetProjectListView(_result));
        } else {
            var _search = {
                'batch_id': id,
                'no_batch_id': true
            };
            var _filter = JSON.stringify({
                'projectSearch': _search
            });
            ajaxHelper.Invoke("/ProjectRequest/GetProjectRequestForBatch", _filter, "json", PLAN.HandleGetProjectList(_result));
        }
        
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;

        if ($("#processId").text() !== "") {
            var param;

            if (process === CONST.transaction_type.statusUpdate || process === CONST.transaction_type.processUpdate) {
                param = {
                    'id': $("#u_id").val(),
                    'notes': notes,
                    'record_status': record_status,
                    'user_action': user_action
                };
            }

            if (process === CONST.transaction_type.save && PLAN.Active_ProjectSubStatus === CONST.project_substatus.batch_consolidation) {
                param = {
                    'id': $("#u_id").val(),
                    'applicable_year': $("#batch_applicable_year").val(),
                    'procurement_method': $("#batch_procurement_method").val(),

                    'notes': notes,
                    'record_status': record_status,
                    'user_action': user_action
                };
            }

            if (process === CONST.transaction_type.save && PLAN.Active_ProjectSubStatus === CONST.project_substatus.bid_invitation) {
                param = {
                    'id': $("#u_id").val(),                    
                    'notes': notes,
                    'record_status': record_status,
                    'user_action': user_action
                };
                $.extend(param, INVITATIONTOBID.GetData());
            };

            if (process === CONST.transaction_type.save && PLAN.Active_ProjectSubStatus === CONST.project_substatus.advertise) {
                var ad_data = ADVERTISEMENT.GetData();

                param = {
                    'id': $("#u_id").val(),
                    'notes': notes,
                    'record_status': record_status,
                    'user_action': user_action,

                    'philgeps_publish_date': ad_data.philgeps_publish_date,
                    'philgeps_publish_by': ad_data.philgeps_publish_by,
                    'mmda_publish_date': ad_data.mmda_publish_date,
                    'mmda_publish_by': ad_data.mmda_publish_by,
                    'conspost_date_lobby': ad_data.conspost_date_lobby,
                    'conspost_date_reception': ad_data.conspost_date_reception,
                    'conspost_date_command': ad_data.conspost_date_command,
                    'conspost_by': ad_data.conspost_by,
                    'newspaper_sent_date': ad_data.newspaper_sent_date,
                    'newspaper_publisher': ad_data.newspaper_publisher,
                    'newspaper_received_by': ad_data.newspaper_received_by,
                    'newspaper_post_date': ad_data.newspaper_post_date,
                    'newspaper_post_by': ad_data.newspaper_post_by
                };

                var opts = {
                    dir: [PLAN.UploadSubFolder, '\\', param.id].join('')
                };

                

                var philgeps_att = ATTACHMENT.GetAttachmentObj("fleBrowse_philgeps");
                var mmda_att = ATTACHMENT.GetAttachmentObj("fleBrowse_mmda");
                var cons_mmda_att = ATTACHMENT.GetAttachmentObj("fleBrowse_cons_mmda");
                var cons_redep_att = ATTACHMENT.GetAttachmentObj("fleBrowse_cons_redep");
                var cons_command_att = ATTACHMENT.GetAttachmentObj("fleBrowse_cons_command");
                var news_att = ATTACHMENT.GetAttachmentObj("fleBrowse_news");
                
                if (philgeps_att != null && philgeps_att.fileArray.length > 0) {
                    //console.log(philgeps_att.fileArray);
                    var att = philgeps_att.fileArray[0];
                    var fileName = [att.GUID, att.Blob.name.substring(att.Blob.name.lastIndexOf("."))].join('');
                    FileUploader.UploadDocument(att.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    if (!FileUploader.Status) {
                        Site.Dialogs.Alert("Error Uploading Image");
                        return;
                    }
                    param.philgeps_att = fileName;
                }
                else {
                    param.philgeps_att = $("#" + philgeps_att.formBrowse + " .attachmentItem").attr("filename");
                }

                if (mmda_att != null && mmda_att.fileArray.length > 0) {
                    var att = mmda_att.fileArray[0];
                    var fileName = [att.GUID, att.Blob.name.substring(att.Blob.name.lastIndexOf("."))].join('');
                    FileUploader.UploadDocument(att.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    if (!FileUploader.Status) {
                        Site.Dialogs.Alert("Error Uploading Image");
                        return;
                    }
                    param.mmda_portal_att = fileName;
                }
                else {
                    param.mmda_portal_att = $("#" + mmda_att.formBrowse + " .attachmentItem").attr("filename");
                }

                if (cons_mmda_att != null && cons_mmda_att.fileArray.length > 0) {
                    var att = cons_mmda_att.fileArray[0];
                    var fileName = [att.GUID, att.Blob.name.substring(att.Blob.name.lastIndexOf("."))].join('');
                    FileUploader.UploadDocument(att.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    if (!FileUploader.Status) {
                        Site.Dialogs.Alert("Error Uploading Image");
                        return;
                    }
                    param.conspost_lobby_att = fileName;
                }
                else {
                    param.conspost_lobby_att = $("#" + cons_mmda_att.formBrowse + " .attachmentItem").attr("filename");
                }

                if (cons_redep_att != null && cons_redep_att.fileArray.length > 0) {
                    var att = cons_redep_att.fileArray[0];
                    var fileName = [att.GUID, att.Blob.name.substring(att.Blob.name.lastIndexOf("."))].join('');
                    FileUploader.UploadDocument(att.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    if (!FileUploader.Status) {
                        Site.Dialogs.Alert("Error Uploading Image");
                        return;
                    }
                    param.conspost_reception_att = fileName;
                }
                else {
                    param.conspost_reception_att = $("#" + cons_redep_att.formBrowse + " .attachmentItem").attr("filename");
                }

                if (cons_command_att != null && cons_command_att.fileArray.length > 0) {
                    var att = cons_command_att.fileArray[0];
                    var fileName = [att.GUID, att.Blob.name.substring(att.Blob.name.lastIndexOf("."))].join('');
                    FileUploader.UploadDocument(att.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    if (!FileUploader.Status) {
                        Site.Dialogs.Alert("Error Uploading Image");
                        return;
                    }
                    param.conspost_command_att = fileName;
                }
                else {
                    param.conspost_command_att = $("#" + cons_command_att.formBrowse + " .attachmentItem").attr("filename");
                }

                if (news_att != null && news_att.fileArray.length > 0) {
                    var att = news_att.fileArray[0];
                    var fileName = [att.GUID, att.Blob.name.substring(att.Blob.name.lastIndexOf("."))].join('');
                    FileUploader.UploadDocument(att.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    if (!FileUploader.Status) {
                        Site.Dialogs.Alert("Error Uploading Image");
                        return;
                    }
                    param.newspaper_att = fileName;
                }
                else {
                    param.newspaper_att = $("#" + news_att.formBrowse + " .attachmentItem").attr("filename");
                }
            };
        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequestBatch': param,
            'item_list': item_list
        });
        
        if ($("#processId").html() === CONST.process_id.create_new) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch # {value} successfully created! You can now send <br> batch for HoPE Approval.", null, true));
        } else if ($("#processId").html() === CONST.process_id.edit) {
            if (PLAN.Active_ProjectSubStatus === CONST.project_substatus.advertise) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Successfully updated advertisement posting. Thank you."));
            } else {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch # {value} successfully updated! You can now send <br> batch for HoPE Approval.", null, true));
            }            
        } else if ($("#processId").html() === CONST.process_id.submit) {
            if (PLAN.Active_ProjectSubStatus === CONST.project_substatus.batch_consolidation) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch successully sent for HoPE Approval"));
            } else if (PLAN.Active_ProjectSubStatus === CONST.project_substatus.bid_invitation) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch is now open for bidding. Please <br> update the advertisement posting for <br> the batch or projects. Thank you."));
            } else if (PLAN.Active_ProjectSubStatus === CONST.project_substatus.advertise) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch is now closed for shortlisting. Suppliers can<br>no longer bid for the projects in the list but can<br>still submit requirements until the deadline."));
            }                                    
        } else if ($("#processId").html() === CONST.process_id.cancel) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Batch successully cancelled."));
        } else if ($("#processId").html() === CONST.process_id.invite) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Invitation to Bid for Batch # {value} successfully created.<br> You can now proceed to advertising. Thank you.", null, true));
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

    },

    InitializeViewList: function () {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList", JSON.stringify({ "setting_list": [CONST.setting_selection.ProcurementMethod] }), "", PLAN.PopulateViewList(_result));
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
                    PLAN.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                PLAN.MaintainData(CONST.transaction_type.search, null, null, null);
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

    HandleGetProjectList: function (result) {
        return function (result) {
            $(".row-selection").remove();
            $.each(result, function (key, value) {
                var _row = $("<tr>", { id: value.id, class: "row-selection" });

                var _colID = $("<td>", { class: "text-center" });
                var _cbx = $("<input>", { type: "checkbox", id: "cbx_select" })
                var _id = $("<span>", { html: value.id });
                _colID.append(_cbx).append(_id);

                var _colTitle = $("<td>", { html: value.title, class: "text-left" });
                var _colReqBy = $("<td>", { html: value.created_by_name });
                var _colAmount = $("<td>", { html: value.approved_budget, class: "text-left project-amount money" });
                _row.append(_colID).append(_colTitle).append(_colReqBy).append(_colAmount);

                if (value.batch_id === 0) {
                    $("#batch_selection_from").append(_row);
                } else {
                    if (String(value.batch_id) === $("#u_id").val()) {
                        $("#batch_selection_to").append(_row);
                    }
                }
            });
            PLAN.RefreshSummary();
        }
    },

    HandleGetProjectListView: function (result) {
        return function (result) {
            $("#dataBatchInfoModal .row-selection").remove();
            $.each(result, function (key, value) {
                var _row = $("<tr>", { id: value.id, class: "row-selection" });
                var _colID = $("<td>", { html: key + 1, class: "text-right" });
                var _colRef = $("<td>", { html: value.id, class: "text-center" });
                var _colCat = $("<td>", { html: value.category_desc, class: "text-left" });
                var _colTitle = $("<td>", { html: value.title, class: "text-left" });
                var _colReqBy = $("<td>", { html: value.created_by_name });
                var _colGrantee = $("<td>", { html: value.grantee });
                var _colAmount = $("<td>", { html: value.approved_budget, class: "text-right" });
                _colAmount.css("color", "#73b011");
                _row.append(_colID).append(_colRef).append(_colCat).append(_colTitle).append(_colReqBy).append(_colGrantee).append(_colAmount);
                $("#dataBatchInfoModal #table_batch_list").append(_row);
            });
        }
    },

    PopulateViewList: function (result) {
        return function (result) {
            if (result.status.code === 0) {
                var ProcMethod = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ProcurementMethod);
                });
                $(ProcMethod).each(function () {
                    $("#batch_procurement_method").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });

            }
        }
    }
}





var refreshData = function () {
    return function () {
        $("#btnSearch").click();
    }
}

$(function () {
    PLAN.Initialize();
    PLAN.InitializeViewList();
});