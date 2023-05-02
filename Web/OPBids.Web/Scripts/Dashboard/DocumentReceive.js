var DocumentRecieve = {
    Initialize: function () {
        Site.DrawConfidential(".DrawConfidential");

        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                $("#btnTrackNow").click();
            }

        });
        $(".view-scan-project").click(function () {
            var isValid = Site.ValidateRequiredEntries(".scan-barcode-section", null);
            if (isValid == true) {
                DocumentRecieve.RetrieveRecord();                
            }
        })

        $("#documentReceiveModal .sectionTab .tabCaption").click(function () {
            $("#documentReceiveModal .sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $("#documentReceiveModal form").css({ "display": "none" });
            $(["#documentReceiveModal #", $(this).attr("formid")].join('')).css({ "display": "" });
        });
        $("#documentReceiveModal #generalTab").click();
        Site.GenerateControls("#documentReceiveModal");
        Site.GenerateControls("#ProjectRequestNoteModal");

        $("#documentReceiveModal #btnReceiveDocument").click(function () {
            $("#documentReceiveModal").modal("hide");
            $("#modal-note-title").text("Documents Receiving Form");
            $("#ProjectRequestNoteModal").modal("show");
        });
        $("#ProjectRequestNoteModal #btnSaveNoteModal").click(function () {
            if (Site.ValidateRequiredEntries("#ProjectRequestNoteModal", null) == true) {
                var _result;
                var param = {
                    'id': $(".dr-id").val(),
                    'notes': $(".dr-notes").val(),
                    'project_substatus': $(".dr-project-substatus").val()
                };

                var _filter = JSON.stringify({
                    'projectRequest': param
                });

                ajaxHelper.Invoke("/Shared/ReceiveProjectRequestDocument", _filter, "json", DocumentRecieve.handleReceive(_result));
            }
        });

    },

    RetrieveRecord: function () {
        var _result;

        var criteria = {
            'id': $("#scan_barcode_ref").val()
        };

        var _filter = JSON.stringify({
            'sub_menu_id': CONST.menu_id.Ongoing,
            'txn': CONST.transaction_type.get,
            'projectSearch': criteria
        });

        ajaxHelper.Invoke("/Shared/CheckProjectRequestDocument", _filter, "json", DocumentRecieve.PopuplateModal(_result));
        ajaxHelper.Invoke("/Shared/GetProjectLogs", _filter, "json", DocumentRecieve.PopulateLogs(_result));        
    },

    PopuplateModal: function (data) {
        return function (data) {
            if (data.status.code === 0) {
                if (data.value.id > 0) {
                    var pr = data.value;
                    $("#documentReceive .dr-id").val(pr.id);
                    $("#documentReceive .dr-project-substatus").val(pr.project_substatus);
                    $("#documentReceive .dr-title").html(pr.title);
                    $("#documentReceive .dr-description").html(pr.description);
                    $("#documentReceive .dr-grantee").html(pr.grantee_name);                    
                    $("#documentReceive .dr-approved-budget").html(pr.approved_budget);
                    $("#documentReceive .dr-procurement-method").html(pr.procurement_method);
                    $("#documentReceive .dr-required-date").html(pr.required_date);
                    $("#documentReceive .dr-category").html(pr.category_desc);
                    
                    $("#documentReceiveModal .modal-status").html(pr.project_substatus_desc);
                    if (data.value.isEditable === false) {
                        $("#documentReceiveModal #btnReceiveDocument").hide();
                    } else {
                        $("#documentReceiveModal #btnReceiveDocument").show();
                    }
                    if (pr.project_substatus.indexOf("PSS-2") > -1 || pr.project_substatus.indexOf("PSS-3") > -1) {
                        $("#documentReceive .dr-budget-header").html("Estimated Budget");
                    } else {
                        $("#documentReceive .dr-budget-header").html("Approved Budget");
                    }
                        
                    DocumentRecieve.DisableAttachment();
                    DocumentRecieve.GetAttachments(pr.id);
                    //DocumentRecieve.GetProjectItems(pr.id);

                    // Get Progress
                    var _result;
                    var _filter = JSON.stringify({
                        'sub_menu_id': CONST.menu_id.Ongoing,
                        'id': pr.id
                    });
                    ajaxHelper.Invoke("/Shared/GetProjectProgress", _filter, "html", DocumentRecieve.PopulateProgress(_result));

                    $("#documentReceiveModal").modal('show');
                } else {
                    Site.Dialogs.Alert("Record does not exist.");
                }
            }
        }        
    },

    PopulateLogs: function (data) {
        return function (data) {
            if (data.status.code === 0) {
                $("#documentReceiveModal #ProjectLogs .table .row:gt(0)").remove();
                $(data.value).each(function (index, value) {                    
                    var _row = $("<div>", { class: "row" });
                    var _date = $("<div>", { class: "col-md-2" });
                    var _user = $("<div>", { class: "col-md-2" });
                    var _dept = $("<div>", { class: "col-md-3" });
                    var _logs = $("<div>", { class: "col-md-4" });
                    _date.html($(value)[0].created_date);
                    _user.html($(value)[0].created_by_name);
                    _dept.html($(value)[0].department_desc);
                    if ($(value)[0].notes === null || $(value)[0].notes === "") {
                        _logs.html("[" + $(value)[0].change_log + "]");
                    } else {
                        _logs.html("[" + $(value)[0].change_log + "] - " + $(value)[0].notes);
                    }                    
                    _row.append(_date).append(_user).append(_dept).append(_logs);
                    $("#documentReceiveModal #ProjectLogs .table").append(_row);
                });
            }
        } 
    },
    PopulateProgress: function () {
        return function (result) {
            $("#documentReceiveModal #progressContainer").html(result);
        };
    },
    handleReceive: function (data) {
        $('#ProjectRequestNoteModal').modal('hide');
        $(".modal-backdrop").remove();
        return function (data) {
            if (data.status.code === 0) {
                if (data.value > 0) {
                    switch ($(".dr-project-substatus").val()) {
                        case CONST.project_substatus.pending_doc_enduser:
                            Site.Dialogs.Alert("Project successfully received. You can now update the project information and add more attachments.", "Got It!");
                            break;
                        case CONST.project_substatus.pending_doc_budget:
                            Site.Dialogs.Alert("Project successfully received. You can now <br> add project to a batch for approval of <br> procurement method.", "Got It!");
                            break;
                        case CONST.project_substatus.for_hope_approval:
                            Site.Dialogs.Alert("Project successfully received. You can now proceed <br> to approval of Procurement Method.", "Got It!");
                            break;
                        case CONST.project_substatus.itb_preparation:
                            Site.Dialogs.Alert("Project successfully received. You can now proceed <br> to preparing of Request for Quotation / Invitation <br> to Bid.", "Got It!");
                            break;
                        case CONST.project_substatus.opening_of_bids:
                            Site.Dialogs.Alert("Project successfully received. You can now<br>submit to TWG for Ranking of Bidders.", "Got It!");
                            break; 
                        case CONST.project_substatus.twg_rank:
                            Site.Dialogs.Alert("Project successfully received. You can now<br>rank bidders with submitted documents.", "Got It!");
                            break;
                        case CONST.project_substatus.lcb_notice:
                            Site.Dialogs.Alert("Project successfully received. You can now<br>prepare Abstract of Bids and LCB Notice.", "Got It!");
                            break;
                        case CONST.project_substatus.twg_eval:
                            Site.Dialogs.Alert("Project successfully received. You can now<br>evaluate submitted bid documents of suppliers.", "Got It!");
                            break;
                        case CONST.project_substatus.post_eval:
                            Site.Dialogs.Alert("Project successfully received. You can proceed to<br>Post Qualification once the LCB has submitted the<br>additional requested documents.", "Got It!");
                            break;
                        case CONST.project_substatus.for_post_qualification:
                            Site.Dialogs.Alert("Project successfully received. You can now check<br>the qualification of the submitted<br>bid documents by the supplier.", "Got It!");
                            break;
                        case CONST.project_substatus.post_qualification:
                            Site.Dialogs.Alert("Project successfully received. You can now check<br>prepare the Notice of Post Qualification<br>and Post Qualification Report.", "Got It!");
                            break;
                        default:
                            Site.Dialogs.Alert("Project successfully received. You can now update the project information and add more attachments.", "Got It!");
                            break;
                    }
                    
                }
            }
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
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name),
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

                    tableScope.append(["<tr class='item'>",
                    "<td>" + ctr++ + "</td>",
                    "<td>" + this.unit + "</td>",
                    "<td>" + this.description + "</td>",
                    "<td class='text-right'>" + this.quantity + "</td>",
                    "<td class='text-right'>" + this.unit_cost + "</td>",
                    "<td class='text-right'>" + Site.ToDecimalString(itemTotal) + "</td>",
                        "</tr > "].join(''));
                });

                Site.GenerateControls($itemsTable);

                $("#projectRequestItems span.units-grand-total").html(Site.ToDecimalString(grandTotal));
                
            }
        });
    }
}
$(function () {
    DocumentRecieve.Initialize();
});