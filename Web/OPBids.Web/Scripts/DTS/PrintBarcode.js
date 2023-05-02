var BR = {
    Id: null,
    Status: null,
    BarcodeData: null,
    Attachments: [],
    GetParameters: function (cProc) {
        var param = DTS.Clone(DTS.param);
        param.menu_id = localStorage.getItem("sub-menu-id");
        param.page_index = $("#pageContainerDocument .paging").val();
        param.filter.page_index = $("#pageContainerDocument .paging").val();
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
        BR.BarcodeData = (new Date().getTime()).toString();
        $("#u_status option:nth-child(1), #u_status option:nth-child(2)").remove();
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
            var paging = $("#pageContainerDocument .paging").val();
            Site.FillPagingValues(data.page_count, false);
            $("#pageContainerDocument .paging").val(paging == null ? 0 : paging);
            if (data.value != null && data.value != undefined && data.value != "") {
                var counter = $("#pageContainerDocument .paging").val() * 10;
                $(data.value).each(function () {
                    BR.Id = this.id;
                    counter++;
                    $("#grdLst tbody").append(["<tr itemid='", this.id, "'><td class='incrementalNum'>", counter,
                        "</td><td number><span department_id style='display:none;'>", this.department_id,
                        "</span><span department style='display:none;'>", this.department, "</span>", this.id, "</td><td date>",
                        Site.FixDateString(this.created_date), "</td><td requested_by>", this.requested_by, " - ",
                        this.department, "</td><td requested_quantity number>", this.requested_quantity,
                        "</td><td printed_quantity number>", this.printed_quantity, "</td><td status='", this.status, "'>",
                        this.status, "</td><td remarks>", this.remarks, "</td><td style='text-align:center;'>",
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
                            (pStatus == CONST.record_status.printed || pStatus == CONST.record_status.cancel ? "" : ["<li><a class='dropdown-item item-setting-inactive lnkPrint' href='#' itemid='", $(this).attr("itemid"), "'>Print Barcodes</a></li>"].join('')),
                            (pStatus == CONST.record_status.printed || pStatus == CONST.record_status.cancel ? "" : ["<li><a class='dropdown-item item-setting-inactive lnkSetPrint' href='#' itemid='", $(this).attr("itemid"), "'>Set as Printed</a></li>"].join('')),
                            (pStatus == CONST.record_status.printed || pStatus == CONST.record_status.cancel ? "" : ["<li><a class='dropdown-item item-setting-inactive lnkCancel' href='#' itemid='", $(this).attr("itemid"), "'>Cancel Request</a></li>"].join('')),
                            (pStatus == CONST.record_status.printed ? ["<li><a class='dropdown-item item-setting-inactive lnkServed' href='#' itemid='", $(this).attr("itemid"), "'>Set as Served</a></li>"].join('') : ""),
                        ].join(''));
                    }
                });
                $("table.table-striped tbody td[status]").each(function () {
                    $(this).html(Site.GetStatusDescription($(this).html()));
                });
                $("#grdLst tbody tr .lnkPrint").unbind("click");
                $("#grdLst tbody tr .lnkPrint").click(function () {
                    BR.Id = $(this).attr("itemid");
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    window.sessionStorage['curBarcode'] = $(["tr[itemid=" + BR.Id, "] span[department_id]"].join('')).html();
                    Site.Dialogs.Input("Print Barcodes", "Remarks:", "Full Print", "Close", null, null);
                    $(".dialogBody").css({ "padding-top": "10px" }).prepend(["<div id='acceptNoteDialogs' style='font-size:16px;padding-bottom:10px;'>",
                        "Quantity to Print:&nbsp;&nbsp;&nbsp;<input number id='u_quantity' type='textbox' style='width:150px;text-align:right;' required caption='Quantity' />",
                        "<span style='float:right;'>Required Quantity: ", $(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html(), " pcs<br />Printed Quantity: ",
                    $(["tr[itemid=" + BR.Id, "] td[printed_quantity]"].join('')).html(), " pcs</span></div>"].join(''));
                    $("#noteId").css({ "font-size": "12px" }).html(["Clicking <span style='color:#32CD32;'>Full Print</span> ",
                        "will automatically set Status to <span style='color:#32CD32;'>Printed</span>."].join(''));
                    $("#dialogInput").attr("maxlength", 1000).attr("required", "").attr("caption", "Remarks").css({ "height": "150px" }).val($(["tr[itemid=" + BR.Id, "] td[remarks]"].join('')).html());
                    $(".dialogFooter").css({ "padding-top": "0px" });
                    $('<button type="button" style="background-color:#1935EA;" id="partialPrintButton" class="ui-button ui-corner-all ui-widget">Partial Print</button>').insertBefore("#positiveDialogButton");
                    Site.GenerateControls(".dialogBody");
                    $("#partialPrintButton").unbind("click");
                    $("#partialPrintButton").click(function () {
                        if (Site.ValidateRequiredEntries(".dialogBody", null) == true) {
                            var qty = (isNaN(parseInt($("#u_quantity").val())) == true ? 0 : parseInt($("#u_quantity").val()));
                            var prntedQty = (isNaN(parseInt($("#u_printed_quantity").val())) == true ? 0 : parseInt($("#u_printed_quantity").val()));
                            $("#u_printed_quantity").val(qty);
                            $("#u_remarks").val($("#dialogInput").val());
                            BR.MaintainData(CONST.transaction_type.update);
                            $(".dialogBox").remove();
                        }
                    });
                    $("#positiveDialogButton").unbind("click");
                    $("#positiveDialogButton").click(function () {
                        $("#u_quantity").val($("#u_requested_quantity").val());
                        if (Site.ValidateRequiredEntries(".dialogBody", null) == true) {
                            BR.Status = CONST.record_status.printed;
                            $("#u_printed_quantity").val($("#u_requested_quantity").val());
                            $("#u_remarks").val($("#dialogInput").val());
                            BR.MaintainData(CONST.transaction_type.update);
                            $(".dialogBox").remove();
                        }
                    });
                    $(".dialogBox .dialogContainer").css({ "max-width": "1000px" });
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    $("#u_requested_quantity").val($(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html());
                    $("#u_printed_quantity").val($(["tr[itemid=" + BR.Id, "] td[printed_quantity]"].join('')).html());
                    $(".dialogBody *").keyup();
                });
                
                $("#grdLst tbody tr .lnkSetPrint").unbind("click");
                $("#grdLst tbody tr .lnkSetPrint").click(function () {
                    BR.Id = $(this).attr("itemid");
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    $("#u_requested_quantity").val($(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html());
                    $("#u_printed_quantity").val($("#u_requested_quantity").val());
                    $("#u_remarks").val($(["tr[itemid=" + BR.Id, "] td[remarks]"].join('')).html());
                    BR.Status = CONST.record_status.printed;
                    BR.MaintainData(CONST.transaction_type.update);
                });
                $("#grdLst tbody tr .lnkServed").unbind("click");
                $("#grdLst tbody tr .lnkServed").click(function () {
                    BR.Id = $(this).attr("itemid");
                    BR.Status = $(["tr[itemid=" + BR.Id, "] td[status]"].join('')).attr("status");
                    $("#u_requested_quantity").val($(["tr[itemid=" + BR.Id, "] td[requested_quantity]"].join('')).html());
                    $("#u_printed_quantity").val($(["tr[itemid=" + BR.Id, "] td[printed_quantity]"].join('')).html());
                    $("#u_remarks").val($(["tr[itemid=" + BR.Id, "] td[remarks]"].join('')).html());
                    BR.Status = CONST.record_status.served;
                    BR.MaintainData(CONST.transaction_type.update);
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
                if (currentProcess == CONST.transaction_type.update && data.status != null) {
                    var barcodeIds = data.status.description.split(',');
                    if (barcodeIds.length == 2 && isNaN(barcodeIds[0]) == false && isNaN(barcodeIds[1]) == false) {
                        $("#printPreviewer").attr("src", ["PrintBarcodePreview?minbc=", barcodeIds[0], "&maxbc=", barcodeIds[1]].join(''));
                    }
                }
                $(".ion-search").click();
            }
        };
        Site.PostData("/DTS/MaintainBarcode", postEvt, param, BR.BarcodeData);
    },
}
$(function () {
    BR.Initialize();
});