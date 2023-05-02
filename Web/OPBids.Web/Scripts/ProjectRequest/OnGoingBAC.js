var ONGOINGBAC = {
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
    Clone: function(obj) {
        return $.parseJSON(JSON.stringify(obj));
    },
    ValidateAttachment: function(blob, val) {
        val = val.toLowerCase();
        var regex = new RegExp("(.*?)\.(jpg|png|gif|doc|pdf|docx|xls|xlsx|txt)$");
        if ((regex.test(val.toLowerCase()))) {
            var attachmentName = window.prompt("Please enter an attachment name", "");
            if (attachmentName === null || attachmentName === "") {
                $("#fleBrowse").replaceWith($("#fleBrowse").val('').clone(true));
                return;
            }
            var param = ONGOINGBAC.Clone(ONGOINGBAC.AttachmentParam);
            param.GUID = moment(new Date()).format("YYYYMMDDhhmmssSS");
            param.AttachmentName = attachmentName;
            param.FileName = val;
            param.Blob = blob;
            ONGOINGBAC.Attachments.push(param);
            var imgCls = Utilities.GetFileTypeClass(val);
            $("#formAttachments").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<div class='attachmentName breakWord'>", attachmentName,
                "</div><div class='", imgCls, "' title='For upload'></div><input type='text' maxlength='20' /></div>"].join(''));


            if (ONGOINGBAC.EnableAttachments) {
                $(["#formAttachments .attachmentItem[guid='", param.GUID, "']"].join('')).append("<div style='text-align:center;width:100%;'><button type='button' class='removeItem btn btn-danger btn-block delete_file'  data-fileguid='", param.GUID ,"'>REMOVE</button></div>");
                $(["#formAttachments .attachmentItem[guid='", param.GUID, "'] .removeItem"].join('')).click(function() {
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

        /*=================================
        Process :   [2.2] [7.1] [8.1] [9.1] [10.1] [11.1] [12.1] [14.1]
        ==================================*/
        $(".item-setting-view").unbind("click");
        $(".item-setting-view").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            PROJECT.PopulateProjectInformation(this);
        });

        $(".item-setting-prepare-rfq").unbind("click");
        $(".item-setting-prepare-rfq").click(function (e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.rfq_preparation;

            var _id = $(this).attr("data-itemid");
            var _row = $("#row-" + _id);
            $("#u_id").val(_id);
            
            $("#bidlist_proj_id").html(_id);
            $("#bidlist_proj_title").html(_row.data("title"));

            var data = {
                "proj_id": _id,
                "proj_title": _row.data("title"),
                "proj_description": _row.data("description"),
                "approved_budget": _row.data("approved-budget"),
                "proc_method": _row.data("proc-method"),
                "prn_number": _row.data("pr-number"),
                "classification": _row.data("classification-desc"),
                "created_by": _row.data("created-by"),
                "created_by_dept": _row.data("grantee"),
                "rfq_deadline": _row.data("rfq-deadline"),
                "rfq_place": _row.data("rfq-place"),
                "rfq_requestor": _row.data("rfq-requestor"),
                "rfq_requestor_dept": _row.data("rfq-requestor-dept"),
                "rfq_request_date": _row.data("rfq-request-date")
            };

            RFQDETAILS.ShowModal(Utilities.CreateJSON(data));
            RFQ.EnableFields();
            RFQDETAILS.SetupControls(true);
        });

        $(".item-setting-rfq").unbind("click");
        $(".item-setting-rfq").click(function (e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.rfq_preparation;

            var _id = $(this).attr("data-itemid");
            var _row = $("#row-" + _id);
            $("#u_id").val(_id);
            
            $("#bidlist_proj_id").html(_id);
            $("#bidlist_proj_title").html(_row.data("title"));

            var data = {
                "proj_id": _id,
                "proj_title": _row.data("title"),
                "proj_description": _row.data("description"),
                "approved_budget": _row.data("approved-budget"),
                "proc_method": _row.data("proc-method"),
                "prn_number": _row.data("pr-number"),
                "classification": _row.data("classification-desc"),
                "created_by": _row.data("created-by"),
                "created_by_dept": _row.data("grantee"),
                "rfq_deadline": _row.data("rfq-deadline"),
                "rfq_place": _row.data("rfq-place"),
                "rfq_requestor": _row.data("rfq-requestor"),
                "rfq_requestor_dept": _row.data("rfq-requestor-dept"),
                "rfq_request_date": _row.data("rfq-request-date")
            };

            RFQDETAILS.ShowModal(Utilities.CreateJSON(data));
            RFQ.DisableFields();
            RFQDETAILS.SetupControls(false);
        });

        $(".item-setting-submit-advertising").unbind("click");
        $(".item-setting-submit-advertising").click(function (e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.rfq_preparation;

            var _id = $(this).attr("data-itemid");            
            $("#u_id").val(_id);
            var _hasRFQ = $("#row-" + _id).data("rfq-deadline");
            if (_hasRFQ != null && _hasRFQ != "") {
                console.log(_hasRFQ);
                $("#processId").html(CONST.process_id.submit);

                $("#modal-note-title").text("Submit to Advertising Note");
                $("#ProjectRequestNoteModal").modal("show");
            } else {
                Site.Dialogs.Alert("Please Prepare Request for Quotation first!", "Ok Got it!");
            }
        });

        $(".item-setting-advertise").unbind("click");
        $(".item-setting-advertise").click(function (e) {
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.advertise;
            e.preventDefault();
            $("#processId").html(CONST.process_id.edit);            
            ADVERTISEMENT.ShowModal(this);
        });

        /*=================================
        Process :   [12.1]
        ==================================*/
        $(".item-setting-view-lowest-bid").unbind("click");
        $(".item-setting-view-lowest-bid").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            ONGOINGBAC.PopulateLowestBidderModal(this);
        });

        /*=================================
        Process :   [2.2] 
        ==================================*/
        $(".item-setting-update").unbind("click");
        $(".item-setting-update").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.edit);
            ONGOINGBAC.PopulateModal(this);
        });

        /*=================================
        Process :   [2.2]
        ==================================*/
        $(".item-setting-submit-budget-approval").unbind("click");
        $(".item-setting-submit-budget-approval").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*================================
        Process :   [7.1] [8.1] [8.3] [9.1]
        ==================================*/
        $(".item-setting-view-bidderlist").unbind("click");
        $(".item-setting-view-bidderlist").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.PopulateBidderListModal(this);
        });

        /*=================================
        Process :   [7.1]
        ===================================*/
        $(".item-setting-close-shortlist").unbind("click");
        $(".item-setting-close-shortlist").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.submit);
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.advertise;

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);

            $("#modal-note-title").text("Close for Shortlisting Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [8.1]
        ===================================*/
        $(".item-setting-proceed-bid-opening").unbind("click");
        $(".item-setting-proceed-bid-opening").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.closed_shortlist;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Proceed to Opening of Bids Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [8.1]
        ===================================*/
        $(".item-setting-reopen-project").unbind("click");
        $(".item-setting-reopen-project").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.closed_shortlist;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Reopen Project for Bidding Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [8.3]
        ===================================*/
        $(".item-setting-twg-rank").unbind("click");
        $(".item-setting-twg-rank").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.opening_of_bids;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit for TWG Ranking Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [8.3]
        ===================================*/
        $(".item-setting-return-shortlist").unbind("click");
        $(".item-setting-return-shortlist").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.opening_of_bids;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Return for Shortlisting Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [10.1], [11.1]
        ===================================*/
        $(".item-setting-prepare-abstract").unbind("click");
        $(".item-setting-prepare-abstract").click(function(e) {
            e.preventDefault();
            $("#u_id").val($(this).attr("data-itemid"));
            ONGOINGBAC.PopulateBidAbstractModal(this);
        });

        /*==================================
        Process :   [10.1]
        ===================================*/
        $(".item-setting-prepare-lcb").unbind("click");
        $(".item-setting-prepare-lcb").click(function(e) {
            e.preventDefault();
            $("#u_id").val($(this).attr("data-itemid"));
            ONGOINGBAC.PopulateLCBNoticeModal(this);
        });

        /*==================================
        Process :   [10.1]
        ===================================*/
        $(".item-setting-twg-eval").unbind("click");
        $(".item-setting-twg-eval").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.lcb_notice;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [10.1]
        ===================================*/
        $(".item-setting-return-twg-rank").unbind("click");
        $(".item-setting-return-twg-rank").click(function(e) {
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.lcb_notice;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Return Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [12.1]
        ===================================*/
        $(".item-setting-submit-post-qual").unbind("click");
        $(".item-setting-submit-post-qual").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.post_eval;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [12.1]
        ===================================*/
        $(".item-setting-return-twg-eval").unbind("click");
        $(".item-setting-return-twg-eval").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.post_eval;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.return);
            $("#modal-note-title").text("Return Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [14.1]
        ===================================*/
        $(".item-setting-prepare-pq-report").unbind("click");
        $(".item-setting-prepare-pq-report").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            ONGOINGBAC.PopulatePostQualReportModal(this);
        });

        /*==================================
        Process :   [14.1]
        ===================================*/
        $(".item-setting-prepare-pq-notice").unbind("click");
        $(".item-setting-prepare-pq-notice").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            ONGOINGBAC.PopulatePostQualNoticeModal(this);
        });

        /*==================================
        Process :   [14.1]
        ===================================*/
        $(".item-setting-proceed-recom").unbind("click");
        $(".item-setting-proceed-recom").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.post_qualification;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Proceed Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [15.1]
        ===================================*/
        $(".item-setting-prepare-bac-reso").unbind("click");
        $(".item-setting-prepare-bac-reso").click(function(e) {
            e.preventDefault();
            $("#u_id").val($(this).attr("data-itemid"));
            alert("Prepare BAC Resolution");
        });

        /*==================================
        Process :   [15.1]
        ===================================*/
        $(".item-setting-prepare-app-bac-reso").unbind("click");
        $(".item-setting-prepare-app-bac-reso").click(function(e) {
            e.preventDefault();
            $("#u_id").val($(this).attr("data-itemid"));
            alert("Prepare Approval of BAC Resolution");
        });

        /*==================================
        Process :   [15.1]
        ===================================*/
        $(".item-setting-submit-awarding").unbind("click");
        $(".item-setting-submit-awarding").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.for_recommendation;
            $("#u_id").val($(this).attr("data-itemid"));
            $("#processId").html(CONST.process_id.submit);
            $("#modal-note-title").text("Submit Note");
            $("#ProjectRequestNoteModal").modal("show");
        });

        /*==================================
        Process :   [15.2]
        ===================================*/
        $(".item-setting-notice-award").unbind("click");
        $(".item-setting-notice-award").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            ONGOINGBAC.PopulateNoticeAwardModal(this);
        });

        /*==================================
        Process :   [15.2]
        ===================================*/
        $(".item-setting-notice-proceed").unbind("click");
        $(".item-setting-notice-proceed").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            ONGOINGBAC.PopulateNoticeProceedModal(this);
        });

        /*==================================
        Process :   [15.2]
        ===================================*/
        $(".item-setting-awarding").unbind("click");
        $(".item-setting-awarding").click(function(e) {
            e.preventDefault();
            $("#processId").html(CONST.process_id.view);
            ONGOINGBAC.PopulateAwardingModal(this);
        });

        /*==================================
        Process :   [16.1]
        ===================================*/
        $(".item-setting-update-comp-status").unbind("click");
        $(".item-setting-update-comp-status").click(function(e) {
            e.preventDefault();
            ONGOINGBAC.PopulateImplementationModal(this);
        });



        /*==================================
        Process :   [7.1] [8.1] [8.3] [9.1], [11.1]
        ===================================*/
        $(".item-setting-attachment").unbind("click");
        $(".item-setting-attachment").click(function(e) {
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

        $(".sectionTab .tabCaption").click(function() {
            $(".sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#dataEntryModal form").css({ "display": "none" });
            $(["#dataEntryModal #", $(this).attr("formid")].join('')).css({ "display": "" });

            if ($(this).attr("formid") === "formAttachments") {
                $("#footerNote").html("Click the plus (+) sign or drag a file to attach.");
            }
        });

        $("#generalTab").click();

        $("#btnSearch").unbind("click");
        $("#btnSearch").click(function(e) {
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
            PROJECT.SearchParam.budget_max = $("#search_budget_max").val().replace(',','');
            PROJECT.SearchParam.grantee = $("#search_grantee").val();
            PROJECT.SearchParam.category = $("#search_category").val()
            PROJECT.SearchParam.project_name = $("#search_project_name").val();
            PROJECT.SearchParam.id = $("#search_draft_no").val();
            PROJECT.SearchParam.get_total = true;
            ONGOINGBAC.MaintainData(CONST.transaction_type.search, null, null, null);
            PROJECT.SearchParam.get_total = false;
        });

        $("#dataEntryModal #btnSave").unbind("click");
        $("#dataEntryModal #btnSave").click(function () {
            if (Site.ValidateRequiredEntries("#dataEntryModal", null) == true) {
                $("#modal-note-title").text("Validation Note");
                $("#ProjectRequestNoteModal").modal("show");
            }
        });

        $("#dataEntryModal #btnSubmit").unbind("click");
        $("#dataEntryModal #btnSubmit").click(function () {
            if (Site.ValidateRequiredEntries("#dataEntryModal", null) == true) {
                $("#modal-note-title").text("Submit Note");
                $("#ProjectRequestNoteModal").modal("show");
            }
        });

        $("#dataRFQModal #btnSave").unbind("click");
        $("#dataRFQModal #btnSave").click(function (e) {
            e.preventDefault();
            if (Site.ValidateRequiredEntries("#dataRFQModal", null) == true) {
                $("#processId").html(CONST.process_id.edit);
                $("#modal-note-title").text("Request Note");
                $("#ProjectRequestNoteModal").modal("show");
            }
        });

        $("#dataAdvertisementModal #btnAdvertiseSave").unbind("click");
        $("#dataAdvertisementModal #btnAdvertiseSave").click(function () {
            if (Site.ValidateRequiredEntries("#dataAdvertisementModal", null) == true) {
                $("#processId").html(CONST.process_id.edit);
                ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.advertise;
                ONGOINGBAC.MaintainData(CONST.transaction_type.save, null, null, null, null);
            }
        });   

        $("#dataAwardingModal #btnSave").unbind("click");
        $("#dataAwardingModal #btnSave").click(function() {
            $("#processId").html(CONST.process_id.submit);
            ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.for_awarding;
            ONGOINGBAC.MaintainData(CONST.transaction_type.processUpdate, null, CONST.user_action.approve, null, null);
        });

        $("#dataImplementationModal #btnSave").unbind("click");
        $("#dataImplementationModal #btnSave").click(function () {
            if ($("#dataImplementationModal #imp_status").val() === "17.1") {
                //TODO
                $("#processId").html(CONST.process_id.submit);
                ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.under_implementation;
                ONGOINGBAC.MaintainData(CONST.transaction_type.processUpdate, null, CONST.user_action.update_imp_status, null, null);
            } else {
                $("#processId").html(CONST.process_id.submit);
                ONGOINGBAC.Active_ProjectSubStatus = CONST.project_substatus.under_implementation;
                ONGOINGBAC.MaintainData(CONST.transaction_type.processUpdate, null, CONST.user_action.update_imp_status, null, null);
            }            
        });

        $("#ProjectRequestNoteModal #btnSaveNoteModal").click(function () {
            if (Site.ValidateRequiredEntries("#ProjectRequestNoteModal", null) == true) {
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
                ONGOINGBAC.MaintainData(_txn, null, _user_action, null, $(".dr-notes").val());
            }
        });

        Site.GenerateControls("#dataEntryModal");
        Site.GenerateControls("#dataAwardingModal");
        Site.GenerateControls("#dataImplementationModal");
        Site.GenerateControls("#ProjectRequestNoteModal");
        Site.GenerateControls("#dataRFQModal");


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
                $itemsTable.find("tr:last").on("change", function () { ONGOINGBAC.ComputeItemTotalCost(this); });
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
                ONGOINGBAC.ComputeItemTotalCost();
            }
        );
    },

    PopulateModal: function(_this) {

        ProjReqEditor.ShowClassification();
        //ProjReqEditor.EnableFields();
        $("#btnSave").hide();
        $("#btnSubmit").hide();

        ONGOINGBAC.Active_ProjectSubStatus = $("#hf_project_substatus").val();

        switch ($("#processId").text()) {
            case CONST.process_id.view:
                ProjReqEditor.DisableFields();
                ONGOINGBAC.DisableAttachment();
                break;
            case CONST.process_id.submit:
                ProjReqEditor.DisableFields();
                ONGOINGBAC.DisableAttachment();
                $("#btnSubmit").show();
                break;
            default:
                ProjReqEditor.EnableFields();
                ONGOINGBAC.EnableAttachment();
                $("#btnSave").show();
                break;
        }

        var _id = $(_this).attr("data-itemid");

        $("#u_id").val(_id);
        $("#title").val($(["tr#row-" + _id, " td[proj_title]"].join('')).html().trim());
        $("#estimated_budget").val($(["tr#row-" + _id, " td[budget]"].join('')).html().trim());
        
        $("#required_date").val($.datepicker.formatDate("M dd yy", new Date($("#row-" + _id).data("required-date"))));

        $("#description").val($("#row-" + _id).data("description"));
        //$("#grantee").val($("#row-" + _id).data("grantee"));
        $("#category").val($("#row-" + _id).data("category"));
        $("#classification").val($("#row-" + _id).data("classification"));
        $("#contract_type").val($("#row-" + _id).data("contract-type"));
        $("#security_level").val($("#row-" + _id).data("security-level"));
        $("#delivery_type").val($("#row-" + _id).data("delivery-type"));
        $("#pr_number").val($("#row-" + _id).data("pr-number"));
        $("#proj_substatus").html($("#row-" + _id).data("sub-status"));
        ONGOINGBAC.GetProjectProgress(_id);
        ONGOINGBAC.GetAttachments(_id);
        ONGOINGBAC.GetProjectItems(_id);        
        $('#dataEntryModal').modal('show');
        $("#dataEntryModal *[required]").keyup();

        var _grant = $("#row-" + _id).data("grantee");
        var _grant_name = $("#row-" + _id).data("grantee-name");
        var $select = $('#grantee').selectize();
        var selectize = $select[0].selectize;
        selectize.addOption({ value: _grant_name, key: _grant });
        selectize.setValue(_grant);
    },

    PopulateProjectInformation: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': _id
        });
        ajaxHelper.Invoke("/ProjectRequest/GetProjectInformation", _filter, "html", function (result) {
            $("#dataMainModal").html(result);
            PROJINFO.InitializeProjectInformation(_id);
            $("#MainModalContainer").modal("show");
        });
    },

    PopulateBidderListModal: function(_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);

        $("#bidlist_proj_id").html(_id);
        $("#bidlist_proj_title").html($("#row-" + _id).data("title"));
        $("#bidlist_proj_desc").html($("#row-" + _id).data("description"));
        $("#bidlist_approved_budget").html($("#row-" + _id).data("approved-budget"));
        $("#bidlist_requested_by").html($("#row-" + _id).data("created-by-name"));
        $("#bidlist_category").html($("#row-" + _id).data("category-desc"));

        ONGOINGBAC.GetBidderList(_id);

        $('#dataBidderListModal').modal('show');
    },

    PopulateBidAbstractModal: function(_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);
        BIDABSTRACT.SetData($("#row-" + _id), _id)
        ONGOINGBAC.GetBidderList(_id);
        $('#dataBidAbstractModal').modal('show');
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
            if (result.status.code === 0) {
                LOWESTBIDDER.SetData($("#row-" + _id), result.value );
                $("#dataLowestBidderModal").modal('show');
            }
        });
    },

    PopulatePostQualReportModal: function (_this) {
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
            if (result.status.code === 0) {
                POSTQUALREPORT.SetData($("#row-" + _id), result.value);
                $("#dataPostQualReportModal").modal('show');
            }
        });
    },

    PopulatePostQualNoticeModal: function (_this) {
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
            if (result.status.code === 0) {
                POSTQUALNOTICE.SetData($("#row-" + _id), result.value);
                $("#dataPostQualNoticeModal").modal('show');
            }
        });
    },

    PopulateNoticeAwardModal: function (_this) {
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
            if (result.status.code === 0) {
                NOTICEAWARD.SetData($("#row-" + _id), result.value);
                $("#dataNoticeAwardModal").modal('show');
            }
        });
    },

    PopulateNoticeProceedModal: function (_this) {
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
            if (result.status.code === 0) {
                NOTICEPROCEED.SetData($("#row-" + _id), result.value);
                $("#dataNoticeProceedModal").modal('show');
            }
        });
    },

    PopulateAwardingModal: function (_this) {        
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
            if (result.status.code === 0) {
                AWARDCONTRACT.SetData($("#row-" + _id), result.value);
                $('#dataAwardContractModal').modal('show');
            }
        });
    },

    PopulateLCBNoticeModal: function (_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);
        LCBNOTICE.SetData($("#row-" + _id), _id)
        ONGOINGBAC.GetBidderListNotice(_id);
        $('#dataLCBNoticeModal').modal('show');
    },

    PopulateImplementationModal: function(_this) {
        var _id = $(_this).attr("data-itemid");
        $("#u_id").val(_id);

        $('#dataImplementationModal').modal('show');
    },

    GetBidderList: function(id) {
        var _result;
        var _search = {
            'id': id
        };
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': CONST.transaction_type.getBid,
            'projectSearch': _search
        });
        ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", ONGOINGBAC.handleGetBidderList(_result));
    },
    GetBidderListNotice: function (id) {
        var _result;
        var _search = {
            'id': id
        };
        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': CONST.transaction_type.getBid,
            'projectSearch': _search
        });
        ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", ONGOINGBAC.handleGetBidderListNotice(_result));
    },

    MaintainData: function(process, record_status, user_action, item_list, notes) {
        var _result;
        var _param;

        if ($("#processId").text() !== "") {
            _param = {
                'id': $("#u_id").val(),
                'title': $("#title").val(),
                'description': $("#description").val(),
                'grantee': $("#grantee").val(),
                'estimated_budget': $("#estimated_budget").val(),
                'budget_min': $("#search_budget_min").val().replace(',', ''),
                'budget_max': $("#search_budget_max").val().replace(',', ''),
                'required_date': $("#required_date").data('datepicker').getFormattedDate(CONST.dateformat),
                'category': $("#category").val(),
                'classification': $("#classification").val(),
                'contract_type': $("#contract_type").val(),
                'security_level': $("#security_level").val(),
                'delivery_type': $("#delivery_type").val(),
                'imp_perc_status': $("#dataImplementationModal #imp_status").val(),
                'pr_number': $("#pr_number").val(),
                'record_status': record_status,
                'user_action': user_action,
                'notes': notes
            };
        }

        if ($("#processId").html() === CONST.process_id.edit) {
            if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.rfq_preparation) {
                var _data = RFQDETAILS.GetData();
                _param = {
                    'id': $("#u_id").val(),
                    'rfq_deadline': _data.rfq_deadline,
                    'rfq_place': _data.rfq_place,
                    'rfq_requestor': _data.rfq_requestor,
                    'rfq_requestor_dept': _data.rfq_requestor_dept,
                    'rfq_request_date': _data.rfq_request_date,
                    'notes': notes
                };
            }

            $("#formAttachments .attachmentItem").each(function () {

               
                var paramAttachments = ONGOINGBAC.Clone(ONGOINGBAC.DocumentAttachment);
                paramAttachments.process = $(this).attr("process");
                paramAttachments.guid = $(this).attr("guid");
                paramAttachments.id = $(this).attr("attachmentid");
                paramAttachments.barcode_no = $(this).find("input[type='text']").val();
                if (CONST.transaction_type.save == $(this).attr("process")) {
                    //paramAttachments.file_name = [$(this).find("input[type='text']").val(), $(this).attr("filename").substring($(this).attr("filename").lastIndexOf("."))].join('');


                    paramAttachments.file_name = $(this).attr("filename");
                }
                else {
                    paramAttachments.file_name = $(this).attr("filename");
                }
                paramAttachments.attachment_name = $(this).find(".attachmentName").html();
                paramAttachments.status = $(this).attr("status");
                ONGOINGBAC.DocumentAttachments.push(paramAttachments)
                hasData = true;
            });
        }
        ONGOINGBAC.ProjectItems = [];
        var processId = $("#processId").html();
        if (processId === CONST.process_id.edit) {
            $("#projectRequestItems #projectItems tr.item").each(function () {
                var item = ONGOINGBAC.Clone(ONGOINGBAC.ProjectItem);

                var $tr = $(this);

                item.id = $tr.find("input.id").val();

                item.process = $tr.attr("process");

                item.unit = $tr.find("input.unit").val();
                item.description = $tr.find("textarea.description").val();
                item.quantity = $tr.find("input.quantity").val();
                item.unit_cost = $tr.find("input.unit-cost").val();
                ONGOINGBAC.ProjectItems.push(item);
            });
        }
        else if (processId === CONST.process_id.create_new) {
            $("#projectRequestItems #projectItems tr.item").each(function () {
                var $tr = $(this);
                var item = ONGOINGBAC.Clone(ONGOINGBAC.ProjectItem);
                item.process = $tr.attr("process");
                item.id = 0;
                item.unit = $tr.find("input.unit").val();
                item.description = $tr.find("textarea.description").val();
                item.quantity = $tr.find("input.quantity").val();
                item.unit_cost = $tr.find("input.unit-cost").val();
                ONGOINGBAC.ProjectItems.push(item);                
            });
        }

        //filter out new attachments
        var _updateAttachments = ONGOINGBAC.DocumentAttachments;//.filter(function(val) { return val.process != CONST.transaction_type.save; });

        var _advertise = null;
        if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.advertise) {
            _advertise = ADVERTISEMENT.GetData();
        }

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'projectSearch': PROJECT.SearchParam,
            'projectRequest': _param,
            'item_list': item_list,
            'documentAttachments': _updateAttachments,
            'projectItems': ONGOINGBAC.ProjectItems,
            'projectAdvertisement': _advertise
        });
        
        if ($("#processId").html() === CONST.process_id.edit) {
            if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.rfq_preparation) {
                var msg = "RFQ No " + $("#u_id").val() + " - successfully created.<br> You can now proceed to advertising. Thank you.";
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, msg));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.advertise) {
                ajaxHelper.Invoke("/ProjectRequest/AdvertiseProject", _filter, "json", Utilities.HandlerResultMessage(_result, "Project Advertisement successfully updated."));
            } else {
                var msg = "Project request information successfully updated.You can <br> now submit project request for Budget Approval.";
                if (!ONGOINGBAC.UploadAttachment()) {
                    msg = "Error Uploading documents.";
                }
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, msg));
            }
        } else if ($("#processId").html() === CONST.process_id.submit) {
            if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.closed_shortlist) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project has been endorsed to Opening of Bids.<br> Submitted Bid Documents will now be ranked<br> and evaluated to determine the bid winner."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.opening_of_bids) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project successfully submitted for TWG Ranking."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.lcb_notice) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project successfully submitted for TWG Evaluation."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.post_eval) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project has been submitted for Post <br> Qualification of documents"));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.post_qualification) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Proceeding to project recommendation successful. You can now prepare the BAC Resolution for the project."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.for_recommendation) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Projet successfully recommended for awarding. <br> Please have the BAC Resolution approved by HoPE <br> to proceed to Awarding of project to LCRB. Thank you."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.for_awarding) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project successfully awarded to Supplier. You can now view project on Under Implementation status."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.rfq_preparation) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project is now open for submission of quotations.<br>Please update the advertisement posting for the project.<br>Thank you."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.under_implementation) {
                var msg = "Project status successfully updated. <br> Thank you.";
                if ($("#dataImplementationModal #imp_status").val() === "17.1") {
                    msg = "Project status successfully updated. <br> You can now view project under <br> Completed Projects Menu.";
                }
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, msg));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.advertise) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project is now closed for shortlisting. Suppliers can <br> no longer bid for the project but can <br> still submit requirements until the deadline."));
            }
            else {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project succussfully submitted for Budget Approval. <br> Thank you."));
            }
        } else if ($("#processId").html() === CONST.process_id.return) {
            if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.opening_of_bids) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project successfully returned to Shortlisting."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.lcb_notice) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project successfully returned for TWG Ranking."));
            } else if (ONGOINGBAC.Active_ProjectSubStatus === CONST.project_substatus.post_eval) {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project successfully returned to TWG"));
            } else {
                ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "json", Utilities.HandlerResultMessage(_result, "Project has been Reopened for Bidding."));
            }
        } else {
            ajaxHelper.Invoke("/ProjectRequest/ResultView", _filter, "html", renderResultCallback(_result));
        }

    },

    InitializeViewList: function() {
        var _result;
        ajaxHelper.Invoke("/Shared/GetSettingsList",
            JSON.stringify({
                "setting_list": [
                    CONST.setting_selection.ProjectCategory,
                    CONST.setting_selection.ProjectClassification,
                    CONST.setting_selection.ContractType,
                    CONST.setting_selection.DocumentSecurityLevel,
                    CONST.setting_selection.Delivery,
                    //CONST.setting_selection.ProjectGrantee
                ]
            }),
            "", ONGOINGBAC.populateViewList(_result));

        populateGrantees();
    },

    GetAttachments: function(project_id) {
        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': project_id,
        });
        var _result;
        ajaxHelper.Invoke("/ProjectRequest/ProjectAttachments", _param, "json", function(result) {
            if (result) {
                $("#formAttachments .attachmentItem").remove();

                $(result).each(function () {

                    
                    var imgCls = Utilities.GetFileTypeClass(this.file_name);
                    $("#formAttachments").append(["<div status ='", this.status, "'  attachmentid ='", this.id,
                        "' filename='", this.file_name, "' process='", CONST.transaction_type.update,
                        "' class='attachmentItem' style='float:left;'><div class='attachmentName breakWord'>",
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name) , "'",
                        " title='Click item to preview' download  data-container='#dataMainModal'  data-toggle='popover' data-placement='auto' data-html='true' data-trigger='hover' data-content='<p><strong>Uploaded On</strong> - ", this.updated_date, " </p><p><strong>Uploaded By</strong> - ", this.updated_by_name,"'><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", (ONGOINGBAC.EnableAttachments ? "" : "disabled"), "/></div>"].join(''));
                });

                

                if (ONGOINGBAC.EnableAttachments) {
                    $("#formAttachments .attachmentItem").append("<div style='text-align:center;width:100%;'><button type='button' multiupload class='removeItem btn btn-danger btn-block delete_file'  data-fileguid='noid'>REMOVE</button></div>");
                    $("#formAttachments .attachmentItem .removeItem").unbind("click");
                    $("#formAttachments .attachmentItem .removeItem").click(function() {
                        $(this).parent().parent().css({ "display": "none" }).attr("process", CONST.record_status.cancel);
                    });
                }

               

                $('[data-toggle="popover"]').popover();

                
                
            }
        });
    },
    UploadAttachment: function() {
        try {
            var data = $("body").data(ONGOINGBAC.DocumentAttachments);
            if (data != null && data != undefined && data != "") {
                $(ONGOINGBAC.Attachments).each(function() {

                    var curFile = $(["#formAttachments div[guid='", this.GUID, "']"].join(''));
                    if ($(curFile).length == 0) {
                        return true; 
                    }
                    var projectId = $("#u_id").val();
                    var fileName = [this.GUID, $(curFile).attr("filename").substring($(curFile).attr("filename").lastIndexOf("."))].join('');

                    var opts = {
                        dir: [ONGOINGBAC.UploadSubFolder, '\\', projectId].join('')
                    }

                    FileUploader.UploadDocument(this.Blob, fileName, opts, false, null);//upload synchronously to complete the upload before continuing

                    var attachmentGuid = this.GUID.valueOf();
                    var attParam = Site.FindFirst(ONGOINGBAC.DocumentAttachments, function(e) {
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
                        ajaxHelper.Invoke("/ProjectRequest/AddAttachment", _param, "json", function() { });
                    }

                });

            }
            return true;
        } catch (e) {
            return false;
        }
    },
    DisableAttachment: function() {
        ONGOINGBAC.EnableAttachments = false;

        $("#btnBrowse").parent().hide();
        $("#btnBrowse").unbind("click");

        $("#fleBrowse").unbind("change");
        $("#formAttachments").unbind();
    },
    EnableAttachment: function() {
        ONGOINGBAC.EnableAttachments = true;
        $("#btnBrowse").parent().show();
        $("#btnBrowse").click(function() {
            $("#fleBrowse").click();
        });

        $("#fleBrowse").change(function() {
            ONGOINGBAC.ValidateAttachment(this.files[0], $(this).val());
        });

        $("#formAttachments").unbind("filedrop");
        $("#formAttachments").filedrop({
            callback: function(blobData, curId, pFile) {
                ONGOINGBAC.ValidateAttachment(blobData, pFile[0].name);
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
                    ONGOINGBAC.MaintainData(CONST.transaction_type.search, null, null, null);
                }
            }
            $('#pager').bootstrapPaginator(options);

            var pageContainer = $('#pageContainer');
            var pageSelect = pageContainer.find('.paging');
            var pageTotal = pageContainer.find('.pageTotal');

            pageContainer.css({ "display": "inline" });
            pageSelect.on('change', function () {
                PROJECT.SearchParam.page_index = this.value;
                ONGOINGBAC.MaintainData(CONST.transaction_type.search, null, null, null);
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

    handleGetBidderList: function (result) {
        return function (result) {
            $("#dataBidAbstractModal #div_bidder_list").html(result);
        }
    },
    handleGetBidderListNotice: function (result) {
        return function (result) {
            $("#dataLCBNoticeModal #div_bidder_list").html(result);
        }
    },
    GetProjectProgress: function (_id) {
        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': _id,
        });
        ajaxHelper.Invoke("/Shared/GetProjectProgress", _param, "html", function (result) {
            if (result) {
                $("#dataEntryModal #progressContainer").html(result);
            }
        });
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
                $itemsTable.find("input.quantity, input.unit-cost").on("change", function () { ONGOINGBAC.ComputeItemTotalCost(this); });

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

                var ProjectGranteeList = $.grep(result.value, function (x, y) {
                    return (x.type === CONST.setting_selection.ProjectGrantee);
                });
                $(ProjectGranteeList).each(function () {
                    $("#grantee").append(["<option value='", this.key, "'>", this.value, "</option>"].join(''));
                });
            }
        }
    },
}

var refreshData = function() {
    return function() {
        $("#processId").html("");
        ONGOINGBAC.MaintainData(CONST.transaction_type.search, null, null, null);
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

$(function() {
    ONGOINGBAC.Initialize();
    ONGOINGBAC.InitializeViewList();
    
});