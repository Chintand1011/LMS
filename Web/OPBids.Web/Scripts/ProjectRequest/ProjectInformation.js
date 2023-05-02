var PROJINFO = {
    InitializeProjectInformation: function (_id) {
        Site.DrawConfidential(".DrawConfidential");
        Site.DrawConfidential2(".DrawConfidential");
        PROJINFO.InitializeModal("#MainModalContainer");
        PROJINFO.InitializeProjectRequest();
        PROJINFO.InitializeRequestItems();

        PROJINFO.GetAttachments(_id);
        PROJINFO.GetProjectItems(_id);

        if (typeof INVITATIONTOBID !== 'undefined') {
            INVITATIONTOBID.DisableFields();
        };
        if (typeof RFQ !== 'undefined') {
            RFQ.DisableFields();
        };
        if (typeof VIEWBIDADVERTISE !== 'undefined') {            
            VIEWBIDADVERTISE.DisableFields();
        };
        if (typeof BIDDERINFOLIST !== 'undefined') {
            BIDDERINFOLIST.DisableTools();
        }

        //enable calendar icon
        Site.GenerateControls("#formRankResult #div_prepared");
        Site.GenerateControls("#formAbstractBid #div_prepared");
        Site.GenerateControls("#formLCB #div_prepared");
        Site.GenerateControls("#twgEvalLCB #div_prepared");

        Site.GenerateControls("#formPQNotice #div_prepared");
        Site.GenerateControls("#formPQReport #div_prepared");
        Site.GenerateControls("#formNoticeAward #div_prepared");
        Site.GenerateControls("#formNoticeProceed #div_prepared");
                                        
        $("#MainModalContainer").modal("show");
    },
    InitializeModal: function (name) {
        $(name + " .sectionTab .tabCaption").click(function () {
            $(name + " .sectionTab").removeAttr("selected");
            $(this).parent().attr("selected", "");
            $(name + " form").css({ "display": "none" });
            $([name + " #", $(this).attr("formid")].join('')).css({ "display": "" });

            if ($(this).attr("formid") === "formAttachments") {
                $(name + " #footerNote").html("Click the plus (+) sign or drag a file to attach.");
            }
        });
        Site.GenerateControls(name + " #dataMainModal");
        $(name + " #generalTab").click();
    },

    InitializeProjectRequest: function () {        
    },

    InitializeRequestItems: function () {
        var $addItem = $("#projectRequestItems #addRemoveToolbar .toolbar-add");
        $addItem.unbind("click");
        $addItem.click(
            function () {
                var $itemsTable = $("#projectRequestItems #projectItems tbody");
                $itemsTable.append(["<tr class='item' process='", CONST.transaction_type.create, "'><td><input type='checkbox' value='0' class='check-selected id' /></td>",
                    "<td></td>",
                    "<td><input type='text' class='form-control text-box unit' caption='Unit' required='true'/></td>",
                    "<td><textarea class='form-control text-box description' caption='Item Description' required='true'></textarea></td>",
                    "<td><input type='text' class='form-control text-box quantity' caption='Quantity' number required='true'/></td>",
                    "<td><input type='text' class='form-control text-box unit-cost' caption='Unit Cost' money required='true'/></td>",
                    "<td><input type='text' class='form-control text-box total' money='true' readonly/></td>",
                    "</tr > "].join(''));

                Site.GenerateControls($itemsTable);
                $itemsTable.find("tr:last").on("change", function () { ONGOING.ComputeItemTotalCost(this); });
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
                ONGOING.ComputeItemTotalCost();
            }
        );
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

    GetAttachments: function (project_id) {
        var _param = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': project_id,
        });
        var _result;
        ajaxHelper.Invoke("/ProjectRequest/ProjectAttachments", _param, "json", function (result) {
            if (result) {
                $("#MainModalContainer #formAttachments .attachmentItem").remove();
                $("#MainModalContainer #formAttachments .btnBrowse").parent().remove();

                $(result).each(function () {

                    var imgCls = Utilities.GetFileTypeClass(this.file_name);
                    $("#MainModalContainer #formAttachments").append(["<div status ='", this.status, "'  attachmentid ='", this.id,
                        "' filename='", this.file_name, "' process='", CONST.transaction_type.update,
                        "' class='attachmentItem' style='float:left;'><div class='attachmentName breakWord'>",
                        this.attachment_name, "</div><a target='_blank' href='", FileUploader.GetDocument(this.file_name), "'",
                        " title='Click item to preview' download  data-container='.modal-body'  data-toggle='popover' data-placement='auto' data-html='true' data-trigger='hover' data-content='<p><strong>Uploaded On</strong> - ", this.created_date, " </p><p><strong>Uploaded By</strong> - ", this.updated_by_name , "'><div class='", imgCls, "'></div></a><input type='text' placeholder='Barcode' maxlength='20' value='",
                        this.barcode_no, "' ", "disabled", "/></div>"].join(''));
                });

                $('[data-toggle="popover"]').popover();
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
    },
}
$(function () {
    

});


