var Supplier = {
    CurData: "CurData",
    ContextMenu: "<div class='item-setting'><div class='dropdown'><button style='width:42px; height:14px; padding:0; line-height:1;' class='setting-icon' role='button' id='dropdownMenuLink' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='fa fa-gear'></i>&nbsp;&nbsp;<i class='fa fa-caret-down'></i></button><div class='dropdown-menu setting-cmd' aria-labelledby='dropdownMenuLink' style='display:none;'><div style='white-space:nowrap;font-size:20px;font-weight:bold;' class='itemName'></div><span class='subItemName'></span><br /><ul class='contextItemContainer'></ul></div></div></div>",
    Clone: function (obj) {
        return $.parseJSON(JSON.stringify(obj));
    },
    AttachmentParam: {
        GUID: null,
        Blob: null,
    },
    param: {
        process: null,
        menu_id: null,
        filter: {
            id: null,
            created_by: null,
            page_index: null,
            created_by_name: null,
            created_date: null,
            updated_by: null,
            updated_by_name: null,
            updated_date: null,
            submitted_from: null,
            submitted_to: null,
            required_from: null,
            required_to: null,
            budget_min: null,
            budget_max: null,
            grantee: null,
            category: null,
            project_name: null,
            RefNo: null,
        },
        supplier: {
            RefNo: null,
            category: null,
            date_submitted: null,
            project: null,
            project_desc: null,
            amount: null,
            status: null,
            approved_budget: null,
            project_duration: null,
            bid_bond: null,
            id: null,
            created_by: null,
            page_index: null,
            created_by_name: null,
            created_date: null,
            updated_by: null,
            updated_by_name: null,
            updated_date: null,
            notes: null,
        },
        documentAttachments: [],
        documentAttachment: {
            process: null,
            id: null,
            attachment_name: null,
            barcode_no: null,
            file_name: null,
            status: null,
            project_id: null,
        },
    },
    Initialize: function () {
        $(".wrapper").css({ "overflow": "hidden" });
        $(".dividerHr").css({ "display": "none" });
        if (localStorage.getItem("sub-menu-id") == null || localStorage.getItem("sub-menu-id") == undefined) {
            localStorage.setItem("sub-menu-id", CONST.menu_id.OpenBid);
        }
        var _subMenuID = localStorage.getItem("sub-menu-id");
        var param = Supplier.Clone(Supplier.param);
        if (_subMenuID !== undefined && _subMenuID !== null) {
            param.menu_id = _subMenuID;
            var _filter = JSON.stringify(param);
            Supplier.PopuplatePartialView(_filter);
        }
        $(".side-menu-link").click(function (e) {
            var _subMenuId = $(this).attr("sub-menu-id");
            var param = Supplier.Clone(Supplier.param);
            param.menu_id = _subMenuID;
            var _filter = JSON.stringify(param);
            Supplier.PopuplatePartialView(_filter);
        });
    },
    PopuplatePartialView: function (filter) {
        var _result;
        ajaxHelper.Invoke("/Supplier/PartialView", filter, "html", Supplier.RenderResultCallback(_result));
    },
    RenderResultCallback: function (result) {
        return function (result) {
            $("#contentSection").html(result);
        };
    },
    MaintainLogs: function (curProcess, batch_id, curRemarks) {
        var param = Supplier.Clone(Supplier.param);
        param.documentLog.process = curProcess;
        param.documentLog.batch_id = batch_id;
        if (CONST.transaction_type.save == curProcess) {
            var paramLogs = Supplier.Clone(Supplier.param.documentLog);
            paramLogs.batch_id = batch_id;
            paramLogs.process = curProcess;
            paramLogs.id = null;
            paramLogs.status = CONST.record_status.onhand;
            paramLogs.receipient_id = Site.UserId;
            paramLogs.remarks = curRemarks;
            param.documentLogs.push(paramLogs)
        }
        Site.PostData("/Supplier/MaintainDocumentLogs", null, param, Supplier.CurData);
    },
}
$(function () {
    Supplier.Initialize();
});