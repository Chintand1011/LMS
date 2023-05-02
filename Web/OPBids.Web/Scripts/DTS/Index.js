var DTS = {
    IsRecordSection: null,
    CurData: "CurData",
    RoutesBarData: "RoutesBarData",
    ContextMenu: "<div class='item-setting'><div class='show'><button style='width:42px;' class='setting-icon' role='button' id='dropdownMenuLink' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'><i class='fa fa-gear'></i>&nbsp;&nbsp;<i class='fa fa-caret-down'></i></button><div class='dropdown-menu setting-cmd' aria-labelledby='dropdownMenuLink' style='display:none;'><div style='white-space:nowrap;font-size:20px;font-weight:bold;' class='itemName'></div><span class='subItemName'></span><br /><ul class='contextItemContainer'></ul></div></div></div>",
    Clone: function (obj) {
        return $.parseJSON(JSON.stringify(obj));
    },
    AttachmentParam: {
        GUID: null,
        Blob: null,
    },
    GenerateRouteBar: function (batch_id) {
        var postEvt = function () {
            var data = $("body").data(DTS.RoutesBarData);
            $(".routesBar").html("<table><tr></tr></table>");
            if (data != null && data != undefined && data != "") {
                var deptName = "";
                var counter = 0;
                var isPrevFilled = false;
                $(data).each(function () {
                    var deptName = this.department_name;
                    if (deptName == null | deptName == undefined) {
                        deptName = "";
                    }
                    isPrevFilled = (this.sequence != 1000);
                    counter++;
                    $(".routesBar table tr").append(["<td id='bar", counter, "' ", (this.sequence != 1000 ? "filled" : ""), "><span title='", deptName.replace(/'/g, "''"),"'>",
                        Site.ShortVal(deptName, 10), "<br />", (this.updated_date == "" ? "" : Site.FixDateString(this.updated_date).split(' ')[0]), "</span></td>"].join(''));
                    if (counter > 1 && isPrevFilled == true && this.sequence != 1000) {
                        if (counter - 1 == 1) {
                            $(["#bar", counter - 1].join('')).attr("greenLine", "");
                        }
                        else {
                            if ($(["#bar", counter - 1].join('')).attr("greengreyLine") != null && $(["#bar", counter - 1].join('')).attr("greengreyLine") != undefined){
                                $(["#bar", counter - 1].join('')).attr("greenLine", "");
                            }
                            else {
                                $(["#bar", counter - 1].join('')).attr("greengreyLine", "");
                            }
                        }
                        if (counter == 1 || $(data).length == counter) {
                            $(["#bar", (counter)].join('')).attr("greenLine", "");
                        }
                        else {
                            $(["#bar", (counter)].join('')).attr("greengreyLine", "");
                        }
                    }
                });
            }
        }
        var param = DTS.Clone(DTS.param);
        param.documentRoute.process = CONST.transaction_type.getRoutesBar;
        param.documentRoute.batch_id = batch_id;
        Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, param, DTS.RoutesBarData);
        
    },
    UpdateSequence: function (batch_id, postEvt) {
        var param = DTS.Clone(DTS.param);
        param.documentRoute.process = CONST.transaction_type.updateSequence;
        param.documentRoute.batch_id = batch_id;
        param.documentRoute.updated_by = Site.UserId;
        param.documentRoute.department_id = Site.DepartmentId;
        Site.PostData("/DTS/MaintainDocumentRoutes", postEvt, param, DTS.RoutesBarData);
    },
    param: {
        id: null,
        created_by: null,
        created_date: null,
        updated_by: null,
        updated_date: null,
        page_index: null,
        filter: {
            date_requested_from: null,
            date_requested_to: null,
            requested_by: null,
            request_status: null,
            date_submitted_from: null,
            date_submitted_to: null,
            etd_from: null,
            etd_to: null,
            sender_name: null,
            receipient_name: null,
            category_name: null,
            document_type_name: null,
            id: null,
            barcode_no: null,
            document_code: null,
            department_id: null,
            created_by:null,
        },
        requestBarcode: {
            process: null,
            id: null,
            requested_quantity: null,
            printed_quantity: null,
            status: null,
            remarks: null,
            created_by: null,
            updated_by: null,
        },
        requestBarcodes: [],
        document: {
            process: null,
            id: null,
            category_id: null,
            category_name: null,
            document_type_id: null,
            document_type_name: null,
            document_code: null,
            sender_id: null,
            tags: null,
            dept_processed: null,
            sender_name: null,
            receipient_id: null,
            receipient_name: null,
            is_edoc: null,
            etd_to_recipient: null,
            delivery_type_id: null,
            delivery_type_name: null,
            document_security_level_id: null,
            document_security_level: null,
            status: null,
            is_disposable: null,
            years_retention: null,
            created_by: null,
            updated_by: null,
            document_classification: null,
            record_category: null,
        },
        documentAttachments: [],
        documentAttachment: {
            process: null,
            id: null,
            attachment_name: null,
            batch_id: null,
            barcode_no: null,
            file_name: null,
            status: null,
        },
        documentRoutes: [],
        documentRoute: {
            process: null,
            id: null,
            batch_id: null,
            department_id: null,
            department_name: null,
            receipient_id: null,
            receipient_name: null,
            status: null,
        },
        documentLogs:[],
        documentLog: { 
            process: null,
            id: null,
            batch_id: null,
            receipient_id: null,
            receipient_name: null,
            remarks: null,
            status: null,
        },
        userNotification: {
            ids: null,
            process: null,
            message: null,
            is_read: null,
            is_starred: null,
            is_hidden: null,
            recipient_ids: null,
            department_ids: null,
            recipient_names: null,
            sender_id: null,
            sender_name: null,
            date_sent: null,
        }
    },
    Initialize: function () {
        $(".wrapper").css({ "overflow": "hidden" });
        $(".dividerHr").css({ "display": "none" });
        if (localStorage.getItem("sub-menu-id") == null || localStorage.getItem("sub-menu-id") == undefined) {
            localStorage.setItem("sub-menu-id", CONST.menu_id.DTSDashboard);
        }
        var _subMenuID = localStorage.getItem("sub-menu-id");
        var param = DTS.Clone(DTS.param);
        if (_subMenuID !== undefined && _subMenuID !== null) {
            param.menu_id = _subMenuID;
            var _filter = JSON.stringify(param);
            DTS.PopuplatePartialView(_filter);
        }
        $(".side-menu-link").click(function (e) {
            
            var _subMenuId = $(this).attr("sub-menu-id");
            if (_subMenuId != undefined) {
                var param = DTS.Clone(DTS.param);
                param.menu_id = _subMenuID;
                var _filter = JSON.stringify(param);
                DTS.PopuplatePartialView(_filter);
            }
        });
        
    },
    PopuplatePartialView: function (filter) {
        var _result;
        ajaxHelper.Invoke("/DTS/PartialView", filter, "html", DTS.RenderResultCallback(_result));
    },
    RenderResultCallback: function (result) {
        return function (result) {
            $("#contentSection").html(result);
        };
    },
    MaintainLogs: function (curProcess, batch_id, curRemarks) {
        var param = DTS.Clone(DTS.param);
        param.documentLog.process = curProcess;
        param.documentLog.batch_id = batch_id;
        if (CONST.transaction_type.save == curProcess) {
            var paramLogs = DTS.Clone(DTS.param.documentLog);
            paramLogs.batch_id = batch_id;
            paramLogs.process = curProcess;
            paramLogs.id = null;
            paramLogs.status = CONST.record_status.onhand;
            paramLogs.receipient_id = Site.UserId;
            paramLogs.remarks = curRemarks;
            param.documentLogs.push(paramLogs);
        }
        Site.PostData("/DTS/MaintainDocumentLogs", null, param, DTS.CurData);
    },
}
$(function () {
    DTS.Initialize();
});