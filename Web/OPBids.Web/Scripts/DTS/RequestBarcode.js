var BR = {
    Id: null,
    Status: null,
    BarcodeData: null,
    Attachments: [],
    GetParameters: function (cProc) {
        var param = DTS.Clone(DTS.param);
        param.menu_id = localStorage.getItem("sub-menu-id");
        param.page_index = $("#pageContainerDocument .paging").val();
        param.filter.id = $("#u_request_no").val();
        param.filter.created_by = Site.UserId;
        param.filter.date_requested_from = $("#u_date_request_from").val();
        param.filter.date_requested_to = $("#u_date_request_to").val();
        param.filter.requested_by = $("#u_requested_by").val();
        param.filter.request_status = $("#u_status").val();
        param.requestBarcode.process = cProc;
        param.requestBarcode.id = BR.Id;
        param.requestBarcode.requested_quantity = $("#dataEntry #u_requested_quantity").val();
        param.requestBarcode.printed_quantity = $("#dataEntry #u_printed_quantity").val();
        param.requestBarcode.status = BR.Status;
        param.requestBarcode.remarks = $("#dataEntry #u_remarks").val();
        param.requestBarcode.created_by = Site.UserId;
        param.requestBarcode.updated_by = Site.UserId;
        return param;
    },
    Initialize: function () {
        $("#pageContainer").remove();
        BR.BarcodeData = (new Date().getTime()).toString();
        $("#btnRequestBarcode").click(function () {
            Site.Dialogs.Input("Create Request for Barcode", "Remarks:", "Save", "Cancel", null, null);
            $(".dialogBody").css({ "padding-top": "10px" }).prepend(["<div id='acceptNoteDialogs' style='font-size:16px;padding-bottom:10px;'>",
                "Quantity:&nbsp;&nbsp;&nbsp;<input number id='u_quantity' type='textbox' style='width:150px;text-align:right;' required caption='Quantity' /></div>"].join(''));
            $("#noteId").css({ "font-size": "12px" }).html("Requests will be saved as Draft until submitted");
            $("#dialogInput").attr("maxlength", 1000).css({ "height": "150px" }).attr("required", "").attr("caption", "Remarks");
            $(".dialogFooter").css({ "padding-top": "0px" });
            Site.GenerateControls(".dialogBody");
            $("#positiveDialogButton").unbind("click");
            $("#positiveDialogButton").click(function () {
                if (Site.ValidateRequiredEntries(".dialogBody", null) == true) {
                    BR.Id = null;
                    BR.Status = CONST.record_status.draft;
                    $("#u_requested_quantity").val($("#u_quantity").val());
                    $("#u_remarks").val($("#dialogInput").val());
                    BR.MaintainData(CONST.transaction_type.create);
                    $(".dialogBox").remove();
                }
            });
        });
        $("input[type='radio'][name='durationOpt']").click(function () {
            $(".filterContainer #u_date_request_to").val($(this).attr("dateValTo"));
            $(".filterContainer #u_date_request_from").val($(this).attr("dateValFrom"));
        });
        $(".ion-search").click(function () {
            BR.Id = null;
            BR.MaintainData(CONST.transaction_type.search);
        });
        $("input[type='radio'][name='durationOpt'][value='0']").click();
        $(".ion-search").click();
        Site.GenerateControls(".filterContainer");
    },
    MaintainData: function (currentProcess) {
        var param = BR.GetParameters(currentProcess);
        var postEvt = function () {
            var data = $("body").data(BR.BarcodeData);
            $("#grdLst tbody").html('');
            Site.FillPagingValues(data.page_count, false);
            if (data.value != null && data.value != undefined && data.value != "") {
                var counter = $("#pageContainerDocument .paging").val() * Site.PageItemCount;
                $(data.value).each(function () {
                    BR.Id = this.id;
                    counter++;
                    $("#grdLst tbody").append(["<tr itemid='", this.id, "'><td class='incrementalNum'>", counter,
                        "</td><td number><span department style='display:none;'>", this.department, "</span>", this.id, "</td><td date>",
                        Site.FixDateString(this.created_date), "</td><td requested_by>", this.requested_by, " - ", this.department,
                        "</td><td requested_quantity number>", this.requested_quantity, "</td><td printed_quantity number>",
                        this.printed_quantity, "</td><td status='", this.status, "'>", this.status, "</td><td remarks>",
                        this.remarks, "</td><td style='text-align:center;'>",
                        (this.status == CONST.record_status.served ? "" : DTS.ContextMenu), "</td></tr>"].join(''));
                });
                if (currentProcess == CONST.transaction_type.create) {
                    Site.Dialogs.Alert(["Request successfully created with Request Number: ", BR.Id, "."].join(''), "Got It!", null);
                }
                $("#grdLst tbody tr").each(function () {
                    var pStatus = $(this).find("td[status]").attr("status");
                    if (pStatus != CONST.record_status.served) {
                        $(this).find(".itemName").html([$(this).attr("itemid"), ' : ', $(this).find("td[printed_quantity]").html()].join(''));
                        $(this).find(".subItemName").html($(this).find("span[department]").html());
                        $(this).find(".contextItemContainer").html([
                            (pStatus == CONST.record_status.draft ? ["<li><a class='dropdown-item item-setting-inactive lnkEdit' href='#' itemid='", $(this).attr("itemid"), "'>Edit Request Information</a></li>"].join('') : ""),
                            (pStatus == CONST.record_status.draft ? ["<li><a class='dropdown-item item-setting-inactive lnkSubmit' href='#' itemid='", $(this).attr("itemid"), "'>Submit Request</a></li>"].join('') : ""),
                            (pStatus == CONST.record_status.printed || pStatus == CONST.record_status.served || pStatus == CONST.record_status.cancel ? "" : ["<li><a class='dropdown-item item-setting-inactive lnkCancel' href='#' itemid='", $(this).attr("itemid"), "'>Cancel Request</a></li>"].join('')),
                        ].join(''));
                    }
                });
                $("table.table-striped tbody td[status]").each(function () {
                    $(this).html(Site.GetStatusDescription($(this).html()));
                });
                $("#grdLst tbody tr .lnkEdit").unbind("click");
                $("#grdLst tbody tr .lnkEdit").click(function () {
                    BR.Id = $(this).attr("itemid");
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    Site.Dialogs.Input("Create Request for Barcode", "Remarks:", "Save", "Cancel", null, null);
                    $(".dialogBody").css({ "padding-top": "10px" }).prepend(["<div id='acceptNoteDialogs' style='font-size:16px;padding-bottom:10px;'>",
                        "Quantity:&nbsp;&nbsp;&nbsp;<input number id='u_quantity' type='textbox' style='width:150px;text-align:right;' required caption='Quantity' /></div>"].join(''));
                    $("#noteId").css({ "font-size": "12px" }).html("Requests will be saved as Draft until submitted");
                    $("#dialogInput").attr("maxlength", 1000).attr("required", "").attr("caption", "Remarks").css({ "height": "150px" }).val($(["tr[itemid=" + BR.Id, "] td[remarks]"].join('')).html());
                    $(".dialogFooter").css({ "padding-top": "0px" });
                    Site.GenerateControls(".dialogBody");
                    $("#u_quantity").val($(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html());
                    $("#u_printed_quantity").val($(["tr[itemid=" + BR.Id, "] td[printed_quantity]"].join('')).html());
                    $(".dialogBody *").keyup();
                    $("#positiveDialogButton").unbind("click");
                    $("#positiveDialogButton").click(function () {
                        if (Site.ValidateRequiredEntries(".dialogBody", null) == true) {
                            BR.Status = CONST.record_status.draft;
                            $("#u_requested_quantity").val($("#u_quantity").val());
                            $("#u_remarks").val($("#dialogInput").val());
                            BR.MaintainData(CONST.transaction_type.update);
                            $(".dialogBox").remove();
                        }
                    });
                });
                
                $("#grdLst tbody tr .lnkSubmit").unbind("click");
                $("#grdLst tbody tr .lnkSubmit").click(function () {
                    BR.Id = $(this).attr("itemid");
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    $("#u_requested_quantity").val($(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html());
                    $("#u_printed_quantity").val($(["tr[itemid=" + BR.Id, "] td[printed_quantity]"].join('')).html());
                    $("#u_remarks").val($(["tr[itemid=" + BR.Id, "] td[remarks]"].join('')).html());
                    BR.Status = CONST.record_status.pending;
                    BR.MaintainData(CONST.transaction_type.update);
                });
                $("#grdLst tbody tr .lnkCancel").unbind("click");
                $("#grdLst tbody tr .lnkCancel").click(function () {
                    BR.Id = $(this).attr("itemid");
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    $("#u_requested_quantity").val($(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html());
                    $("#u_printed_quantity").val($(["tr[itemid=" + BR.Id, "] td[printed_quantity]"].join('')).html());
                    $("#u_remarks").val($(["tr[itemid=" + BR.Id, "] td[remarks]"].join('')).html());
                    BR.Status = CONST.record_status.cancel;
                    BR.MaintainData(CONST.transaction_type.update);
                    $(".dialogBox").remove();
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
            }
            if (currentProcess == CONST.transaction_type.create || currentProcess == CONST.transaction_type.update) {
                $(".ion-search").click();
            }
        };
        Site.PostData("/DTS/MaintainBarcode", postEvt, param, BR.BarcodeData);
    },
}
$(function () {
    BR.Initialize();
});