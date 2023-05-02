var DTSFilter = {
    DCData: null,
    DTData: null,
    SRData: null,
    DELDTData: null,
    DSLData: null,
    DEPTData: null,
    Initialize: function () {
        DTSFilter.DCData = (new Date().getTime()).toString();
        DTSFilter.DTData = [(new Date().getTime()).toString(), "1"].join('');
        DTSFilter.SRData = [(new Date().getTime()).toString(), "2"].join('');
        DTSFilter.DELDTData = [(new Date().getTime()).toString(), "3"].join('');
        DTSFilter.DSLData = [(new Date().getTime()).toString(), "4"].join('');
        DTSFilter.DEPTData = [(new Date().getTime()).toString(), "5"].join('');
        DTSFilter.GetDocumentCategory();
        DTSFilter.GetDocumentType();
        DTSFilter.GetSenderRecipient();
        DTSFilter.GetDeliveryType();
        DTSFilter.GetDocumentSecurity();
        DTSFilter.GetDepartment();
        Site.GenerateControls(".filterContainer, #dataEntryModal");
    },
    GetDocumentCategory: function () {
        var _result;
        var param = {
            'id': null,
            'document_category_code': null,
            'document_category_name': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': null,
            'txn': null,
            'search_key': null,
            'documentCategory': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $(".category_id option").remove();
            $(".category_id").append(["<option value='0'>All</option>"].join(''));
            var data = $("body").data(DTSFilter.DCData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $(".category_id").append(["<option value='", this.id, "' title='", this.document_category_name, "'>", this.document_category_code, "</option>"].join(''));
                });
            }
            Site.DropDownCommonSettings(".filterContainer #u_category_ids");
            $(".filterContainer #u_category_id").change(function () {
                if ($(this).val() == "" || $(this).val() == null || $(this).val() == undefined) {
                    $(".filterContainer #u_document_type_id option").removeAttr("hidden", "").css({ "display": "" });
                    $(".filterContainer [data-id='u_document_type_id']").parent().find("ul.dropdown-menu.inner li").removeAttr("hidden").css({ "display": "" });
                }
                else {
                    $(".filterContainer #u_document_type_id option").attr("hidden", "").css({ "display": "none" });
                    $(".filterContainer [data-id='u_document_type_id']").next().find("ul.dropdown-menu.inner li").attr("hidden", "").css({ "display": "none" });
                    $($(this).val()).each(function () {
                        var catLst = $([".filterContainer #u_document_type_id option[categoryId='", this, "']"].join(''));
                        $(catLst).css({ "display": "" }).removeAttr("hidden").each(function () {
                            var curText = $(this).text();
                            var curCtl = $(".filterContainer [data-id='u_document_type_id']").next().find("ul.dropdown-menu.inner li .text");
                            $(curCtl).each(function () {
                                if ($(this).html() == curText) {
                                    $(this).parent().parent().css({ "display": "" }).removeAttr("hidden");
                                }
                            });
                        });
                    });
                }
                $(".filterContainer #u_document_type_id").val("");
                $(".filterContainer #u_document_type_id").selectpicker('refresh');
            });
        }
        Site.PostData("/DTS/GetDocumentCategory", postEvt, _filter, DTSFilter.DCData);
    },
    GetDocumentType: function () {
        var _result;
        var param = {
            'id': null,
            'document_category_id': null,
            'document_type_code': null,
            'document_type_description': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': null,
            'txn': null,
            'search_key': null,
            'documentType': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $(".document_type_id option").remove();
            $(".document_type_id").append(["<option value='0'>All</option>"].join(''));
            var data = $("body").data(DTSFilter.DTData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $(".document_type_id").append(["<option categoryId='", this.document_category_id, "' value='", this.id, "' title='",
                        this.document_type_description, "'>", this.document_type_code, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#formGeneral #u_document_type_ids");
            $(".document_type_id").val('0');
        }
        Site.PostData("/DTS/GetDocumentType", postEvt, _filter, DTSFilter.DTData);
    },
    GetDocumentSecurity: function () {
        var _result;
        var param = {
            'status': status,
        };
        var _filter = JSON.stringify({
            'sub_menu_id': null,
            'txn': null,
            'search_key': null,
            'documentSecurityLevel': param,
            'status': null,
            'item_list': null
        });
        var postEvt = function () {
            $(".document_security_level_id option").remove();
            $(".document_security_level_id").append(["<option value='0' title=''>All</option>"].join(''));
            var data = $("body").data(DTSFilter.DSLData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $(".document_security_level_id").append(["<option title='", this.description, "' value='", this.id, "'>", this.code, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#formGeneral #u_document_security_level_ids");
            $(".document_security_level_id").val('0');
        }
        Site.PostData("/DTS/GetDocumentSecurity", postEvt, _filter, DTSFilter.DSLData);
    },
    GetDeliveryType: function () {
        var _result;
        var param = {
            'id': null,
            'delivery_code': null,
            'delivery_description': null,
            'status': null,
        };

        var _filter = JSON.stringify({
            'sub_menu_id': null,
            'txn': null,
            'search_key': null,
            'delivery': param,
            'status': null,
            'item_list': null
        });
        var postEvt = function () {
            $(".delivery_type_id option").remove();
            $(".delivery_type_id").append(["<option value='0'>All</option>"].join(''));
            var data = $("body").data(DTSFilter.DELDTData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $(".delivery_type_id").append(["<option value='", this.id, "'>", this.delivery_code, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#formGeneral #u_delivery_type_ids");
            $(".delivery_type_id").val('0');
        }
        Site.PostData("/DTS/GetDeliveryType", postEvt, _filter, DTSFilter.DELDTData);
    },
    GetSenderRecipient: function () {
        var _result;
        var param = {
            'id': null,
            'user_id': null,
            'is_system_user': null,
            'salutation': null,
            'email_address': null,
            'first_name': null,
            'mi': null,
            'last_name': null,
            'status': null,
            'department_id': null,
            'department_name': null,
            'mobile_no': null,
            'is_sender': null,
            'is_recipient': null
        };
        var _filter = {
            'sub_menu_id': null,
            'txn': null,
            'search_key': null,
            'senderRecipientUser': param,
            'status': null,
            'item_list': null,
            'page_index': -1,
        };
        var postEvt = function () {
            $(".sender_id option").remove();
            $(".receipient_id option").remove();
            $(".sender_id, .receipient_id").append(["<option value='0'>All</option>"].join(''));
            var data = $("body").data(DTSFilter.SRData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (this.is_recipient == true) {
                        $(".receipient_id").append(["<option style='display:none;' department_id='", this.department_id, "' userid='", this.user_id,
                            "' value='", this.id, "'>", this.first_name, " ", this.mi, " ", this.last_name, "</option>"].join(''));
                    }
                    if (this.is_sender == true) {
                        $(".sender_id").append(["<option department_id='", this.department_id, "' email='", this.email_address, "' userid='", this.user_id,
                            "' value='", this.id, "'>", this.first_name, " ", this.mi, " ", this.last_name, "</option>"].join(''));
                    }
                })
            }
            $(".sender_id, .receipient_id").val('0');
        }
        Site.PostData("/DTS/GetSenderRecipient", postEvt, _filter, DTSFilter.SRData);
    },
    GetDepartment: function () {
        var _result;
        var param = {
            'id': null,
            'parent_dept_id': null,
            'dept_code': null,
            'dept_description': null,
            'headed_by': null,
            'designation': null,
            "status": null,
        };

        var _filter = {
            'menu_id': '4',
            'txn': null,
            'search_key': null,
            'department': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $(".department_id option").remove();
            $(".department_id").append(["<option value='0'>All</option>"].join(''));
            var data = $("body").data(DTSFilter.DEPTData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $(".department_id").append(["<option value='", this.id, "'>", this.dept_code, "</option>"].join(''));
                })
            }
            $(".department_id").val('0');
        }
        Site.PostData("/Setting/GetDepartment", postEvt, _filter, DTSFilter.DEPTData);
    },
}
$(function () {
    DTSFilter.Initialize();
    $("#select.selectpicker").selectpicker({
        noneSelectedText: ''
    });
});