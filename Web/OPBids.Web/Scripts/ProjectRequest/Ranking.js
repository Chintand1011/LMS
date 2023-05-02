var RANKING = {
    Status: null,
    Active_ProjectSubStatus: null,
    UploadSubFolder: 'ProjectRequest',//subfolder in upload folder
	Initialize: function () {

		Site.Initialize();


        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#processId").html("");

        /*=================================
        Process :   [9.1] [11.1]
        ==================================*/
        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            PROJECT.PopulateProjectInformation(this);
        });
       
        /*=================================
        Process :   [9.1] 
        ==================================*/
        $(".item-setting-rank-bidder").unbind("click");
        $(".item-setting-rank-bidder").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            $("#u_stage").val(CONST.twg_stage.ranking);
            RANKING.PopulateBidderListModal(this);
        });

        /*=================================
        Process :   [11.1] 
        ==================================*/
        $(".item-setting-eval-bidder").unbind("click");
        $(".item-setting-eval-bidder").click(function (e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            $("#u_stage").val(CONST.twg_stage.evalualtion);
            RANKING.PopulateBidderListModal(this);
        });


        /*=================================
        Process :   [9.1] 
        ==================================*/
        $(".item-setting-lcb-notice").unbind("click");
        $(".item-setting-lcb-notice").click(function (e) {
            e.preventDefault();
            RANKING.Active_ProjectSubStatus = CONST.project_substatus.twg_rank;
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*=================================
        Process :   [11.1] 
        ==================================*/
        $(".item-setting-post-eval").unbind("click");
        $(".item-setting-post-eval").click(function (e) {
            e.preventDefault();
            RANKING.Active_ProjectSubStatus = CONST.project_substatus.twg_eval;
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*=================================
        Process :   [9.1] [11.1]
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

        /*=================================
        Process :   [9.1] [11.1]
        ==================================*/
        $(".item-setting-attachment").unbind("click");
        $(".item-setting-attachment").click(function (e) {
            e.preventDefault();
            alert("View attachments");
        });

        /*=================================
        Process :   [9.1] 
        ==================================*/
        $(".item-bid-setting-view").unbind("click");
        $(".item-bid-setting-view").click(function (e) {
            e.preventDefault();
            var _id = $(this).attr("data-itemid");
            $("#u_bid_id").val(_id);            
            RANKING.PopulateBidChecklistModal(this);
            BIDCHECKLIST.DisableFields();
        });

        $(".item-bid-setting-check-doc").unbind("click");
        $(".item-bid-setting-check-doc").click(function (e) {
            e.preventDefault();
            var _id = $(this).attr("data-itemid");
            $("#u_bid_id").val(_id);
            RANKING.PopulateBidChecklistModal(this);
            BIDCHECKLIST.EnableFields();
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
            RANKING.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent #addRemoveToolbar a").hide();

        $("#dataBidCheckListModal #btnBidCheckSave").unbind("click");
        $("#dataBidCheckListModal #btnBidCheckSave").click(function () {
            var _result;
            var _bid = BIDCHECKLIST.GetSummaryData();
            var _list = BIDCHECKLIST.GetChecklistData();
            
            var _search = {
            };
            var _projRequest = {
                'id': $("#u_id").val()
            };
            var _filter = JSON.stringify({
                'sub_menu_id': localStorage.getItem("sub-menu-id"),
                'projectSearch': PROJECT.SearchParam,
                'projectRequest': _projRequest,
                'projectBids': _bid,
                'projectBidChecklists': _list
            });
            ajaxHelper.Invoke("/ProjectRequest/CreateProjectBidChecklists", _filter, "json", RANKING.handleBidChecklist(_result));
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
            RANKING.MaintainData(_txn, null, _user_action, null, $(".dr-notes").val());
        });

        $("#dataEntryModal .sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });
        });

        $("#dataBidCheckListModal .sectionTab .tabCaption").click(function () {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataBidCheckListModal form").css({ "display": "none" });
            $(["#dataBidCheckListModal #", $(this).attr("formid")].join('')).css({ "display": "" });
        });

        $("#dataEntryModal #generalTab").click();
        Site.GenerateControls("#dataEntryModal");

        $("#dataBidCheckListModal #generalTab").click();
        Site.GenerateControls("#dataBidCheckListModal");
    },

    PopulateModal: function (_this) {
        ProjReqEditor.ShowClassification();
        ProjReqEditor.DisableFields();
        RANKING.DisableAttachment();
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

        RANKING.GetAttachments(_id);

        $('#dataEntryModal').modal('show');
    },

    PopulateBidderListModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);
        RANKING.Active_ProjectSubStatus = $("#row-" + _id).data("project-substatus");
        
        RANKBIDDER.SetData(_id, $("#row-" + _id));
        RANKING.GetBidderList(_id);
        $("#dataRankBidderModal").modal('show');        
    },

    PopulateBidChecklistModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_bid_id").val(_id);
        RANKING.GetBidCheckList();
        $('#dataBidCheckListModal').modal('show');
    },

    GetBidderList: function (id) {
        var _result;
        var _search = {
            'id': id
        };
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': CONST.transaction_type.getBid,
            'projectSearch': _search
        });
        ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", RANKING.handleGetBidderList(_result));
    },

    GetBidCheckList: function () {
        var _result;
        var _search = {
            'id': $("#u_id").val(),
            'bid_id': $("#u_bid_id").val(),
            'stage': $("#u_stage").val()
        }
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'projectSearch': _search
        });
        
        ajaxHelper.Invoke("/ProjectRequest/GetProjectBidChecklists", _filter, "json",
            function (_result) {
                BIDCHECKLIST.Initialize($("#u_stage").val());
                BIDCHECKLIST.SetData(
                    $("#row-" + $("#u_bid_id").val()),
                    _result,
                    $("#u_id").val(),
                    $("#dataRankBidderModal #bidlist_proj_title").html(),
                    $("#dataRankBidderModal #bidlist_grantee").html(),
                    $("#u_bid_id").val(),
                    $("#dataRankBidderModal #bidlist_approved_budget").html()
                );
            }
        );
    },

    MaintainData: function (process, record_status, user_action, item_list, notes) {
        var _result;
        var _param;

       
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
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", RANKING.handleResult(_result));
        } else if ($("#processId").html() === CONST.process_id.return) {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", RANKING.handleResult(_result));
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
            "", RANKING.populateViewList(_result));
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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetPFMSDocument(RANKING.UploadSubFolder + '/' + project_id + '/' + this.file_name),
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

    populateViewList: function (result) {
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
    },

    handleGetBidderList: function (result) {        
        
        return function (result) {
            $("#dataRankBidderModal #div_bidder_list").html(result);
            BIDDERLIST.Initialize($("#u_stage").val());
        }
    },

    handleResult: function (result) {
        return function (result) {
            if (result.status.code === 0) {
                if (RANKING.Active_ProjectSubStatus === CONST.project_substatus.twg_rank) {
                    Site.Dialogs.Alert("Project submitted to BAC for the generation <br> of LCB Notice to Supplier", "Got It!", RANKING.refreshData());
                } else if (RANKING.Active_ProjectSubStatus === CONST.project_substatus.twg_eval) {
                    Site.Dialogs.Alert("Project has been submitted to BAC for <br> Post Evaluation of Supplier", "Got It!", RANKING.refreshData());
                } else if ($("#processId").html() === CONST.process_id.return) {
                    Site.Dialogs.Alert("Project successfully returned to BAC", "Got It!", RANKING.refreshData());
                }

            } else {
                Site.Dialogs.Alert("Something went wrong, please try again", "Ok", RANKING.refreshData());
            }
            $('.modal').modal('hide');
            $(".modal-backdrop").remove();
        }
    },

    handleBidChecklist: function (result) {
        return function (result) {
            if (result.status.code === 0) {
                Site.Dialogs.Alert("Bid Checklist successfully updated!", "Got It!", function () { $("#dataBidCheckListModal #generalTab").click(); });
            } else {
                Site.Dialogs.Alert("Something went wrong, please try again", "Ok", function () { $("#dataBidCheckListModal #generalTab").click(); });
            }
        }
    },

    refreshData: function () {
        return function () {
            $("#btnSearch").click();
        }
    },
    SetPagination: function (page_index, total_records) {//called from result view
        console.log("paging");
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
                    RANKING.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            };
            $('#pager').bootstrapPaginator(opts);


            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                $("#processId").html(CONST.transaction_type.search);
                PROJECT.SearchParam.page_index = this.value;
                RANKING.MaintainData(CONST.transaction_type.search, null, null, null);
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
    }
}

$(function () {
    RANKING.Initialize();
    RANKING.InitializeViewList();
});