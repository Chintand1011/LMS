var POSTQUALIFICATION = {
    Status: null,
    UploadSubFolder: 'ProjectRequest',//subfolder in upload folder
    Initialize: function () {
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#processId").html("");

        /*=================================
        Process :   [13.1]
        ==================================*/
        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            PROJECT.PopulateProjectInformation(this);
        });

        /*=================================
        Process :   [13.1]
        ==================================*/
        $(".item-setting-view-lowest-bid").unbind("click");
        $(".item-setting-view-lowest-bid").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            POSTQUALIFICATION.PopulateLowestBidderModal(this);
        });
        
        /*=================================
        Process :   [13.1] 
        ==================================*/
        $(".item-setting-recommend-award").unbind("click");
        $(".item-setting-recommend-award").click(function (e) {
            e.preventDefault();
            POSTQUALIFICATION.Active_ProjectSubStatus = CONST.project_substatus.for_post_qualification;
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Recommendation Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*=================================
        Process :   [13.1]
        ==================================*/
        $(".item-setting-return-bac").unbind("click");
        $(".item-setting-return-bac").click(function (e) {
            e.preventDefault();
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Return Note");
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
            POSTQUALIFICATION.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent #addRemoveToolbar a").hide();
        

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
            POSTQUALIFICATION.MaintainData(_txn, null, _user_action, null, $(".dr-notes").val());
        });

        $("#dataEntryModal .sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
        });
        
        $("#dataEntryModal #generalTab").click();
        Site.GenerateControls("#dataEntryModal");
        
    },

    PopulateModal: function (_this) {
        ProjReqEditor.ShowClassification();
        ProjReqEditor.DisableFields();
        POSTQUALIFICATION.DisableAttachment();
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

        POSTQUALIFICATION.GetAttachments(_id);

        $('#dataEntryModal').modal('show');
    },    

    PopulateLowestBidderModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);
        
        var _search = {
            'id': _id
        };
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': CONST.transaction_type.getLCB
        });
        ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", function (result) {
            console.log(result);
            if (result.status.code === 0) {
                LOWESTBIDDER.SetData($("#row-" + _id), result.value);
                $("#dataLowestBidderModal").modal('show');
            }
        });
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;
        var _param;

        var _search = {
        };

        if ($("#processId").text() !== "") {
            _param = {
                'id': $("#u_id").val(),

                'record_status': record_status,
                'user_action': user_action,
                'notes': notes
            };
        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': _param,
            'item_list': item_list
        });

        if ($("#processId").html() === CONST.process_id.submit) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", POSTQUALIFICATION.handleResult(_result));
        } else if ($("#processId").html() === CONST.process_id.return) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", POSTQUALIFICATION.handleResult(_result));
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(POSTQUALIFICATION.UploadSubFolder + '/' + project_id + '/' + this.file_name),
                        "' title='Click to download' download><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' disabled /></div>"].join(''));
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
        
    handleResult: function (result) {
        return function (result) {
            if (result.status.code === 0) {
                if (POSTQUALIFICATION.Active_ProjectSubStatus === CONST.project_substatus.for_post_qualification) {
                    Site.Dialogs.Alert("Project has been recommended for awarding. <br> Thank you.", "Got It!", POSTQUALIFICATION.refreshData());
                } else if ($("#processId").html() === CONST.process_id.return) {
                    Site.Dialogs.Alert("Project successfully returned to BAC", "Got It!", POSTQUALIFICATION.refreshData());
                }
            } else {
                Site.Dialogs.Alert("Something went wrong, please try again", "Ok", POSTQUALIFICATION.refreshData());
            }
            $('.modal').modal('hide');
            $(".modal-backdrop").remove();
        }
    },
    
    refreshData: function () {
        return function () {
            $("#btnSearch").click();
        }
    },

    SetPagination: function (page_index, total_records) {//called from result view
        var pageContainer = $('#pageContainer');
        var pageSelect = pageContainer.find('.paging');

        if (total_records > 0) {
            var total_pages = Math.ceil(total_records / PROJECT.SearchParam.page_size);
            var opts = {
                bootstrapMajorVersion: 3,
                currentPage: page_index,
                totalPages: total_pages,
                numberOfPages: 10,
                alignment: 'right',
                onPageClicked: function (e, originalEvent, type, page) {
                    e.stopImmediatePropagation();
                    $("#processId").html(CONST.transaction_type.search);
                    PROJECT.SearchParam.page_index = page;
                    POSTQUALIFICATION.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            };
            $('#pager').bootstrapPaginator(opts);

           
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                $("#processId").html(CONST.transaction_type.search);
                PROJECT.SearchParam.page_index = this.value;
                POSTQUALIFICATION.MaintainData(CONST.transaction_type.search, null, null, null);
            });

            pageSelect.empty();
            var options = '';
            for (var i = 1; i <= total_pages; i++) {
                options += '<option value="' + i + '" ' + (i == page_index ? 'selected' : '') + '>' + i + '</option>';
            }
            pageSelect.append(options);
            pageTotal.text(total_pages);

        } else {
            //var pageContainer = $('#pageContainer');
            //var pageSelect = pageContainer.find('.paging');

            pageContainer.hide();
            pageSelect.empty();
        }
    },

}

$(function () {
    POSTQUALIFICATION.Initialize();
});