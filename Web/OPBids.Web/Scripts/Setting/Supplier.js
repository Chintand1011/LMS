var SP = {
    CurData: null,
    AUData: null,
    Status: null,
    SuppID: null,
    Initialize: function() {
        if ($("#headerTitle").length <= 0) {
            setTimeout(SP.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        SP.AUData = (new Date().getTime()).toString();        
        window.title = $("#headerTitle").html();
        //$("#addCaption, #removeCaption").html($("#headerTitle").html());
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Company Name");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        SP.CurData = (new Date().getTime()).toString();
        $(".modal-dialog").css({ "top": "200px" });
        Site.GenerateControls('#saveModal');
        $("table.table-striped tbody td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            SP.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');            
            SP.SuppID = 0;
            SP.GetSupplierAccessUser();
            $("#saveModal *[required]").keyup();
        });

        $(".toolbar-delete").unbind("click");
        $(".toolbar-delete").click(function () {            
            var confirmEvt = function () {
                var _item_list = [];
                $(".check-selected:checked").each(function () {
                    _item_list.push($(this).attr("itemid"));
                });
                if (_item_list.length <= 0) {
                    Site.Dialogs.Alert("Cannot delete. Please select which one should be deleted.");
                    return;
                }
                SP.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, null, null, null, null, null, CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            //SP.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_name").val(), SP.Status, null);
            SP.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_user_id").val(), $("#u_contact_person").val(), $("#u_company_code").val(), $("#u_comp_name").val(),
                $("#u_address").val(), $("#u_email").val(), $("#u_contact_no").val(), $("#u_tin").val(), SP.Status, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
            Site.Print(["/Setting/Print?setting=", localStorage.getItem("sub-menu-id")].join(''));
        });

        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            if (Site.ValidateRequiredEntries("#saveModal", null) == true) {
                SP.MaintainData(CONST.transaction_type.save, $("#u_id").val(), $("#u_user_id").val(), $("#u_contact_person").val(), $("#u_company_code").val(), $("#u_comp_name").val(),
                    $("#u_address").val(), $("#u_email").val(), $("#u_contact_no").val(), $("#u_tin").val(), SP.Status, null);
                $('#saveModal').modal('hide');
            }
        });
        $("#btnCreateAccessUser").unbind("click");
        $("#btnCreateAccessUser").click(function () {
            localStorage.setItem("sub-menu-id", CONST.menu_id.AccessUsers);
            window.location.reload();
        })

        $(document).on('click touchend', function (e) {
            $(".setting-cmd").hide();
        });
        $(".setting-icon").unbind("click");
        $(".setting-icon").click(function (e) {
            e.preventDefault();
            //$(".setting-cmd").hide();
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

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#u_contact_person").val($(["tr#row-" + _id, " td[contact_person]"].join('')).html().trim());
            $("#u_company_code").val($(["tr#row-" + _id, " td[company_code]"].join('')).html().trim());            
            $("#u_comp_name").val($(["tr#row-" + _id, " td[comp_name]"].join('')).html().trim());
            $("#u_address").val($(["tr#row-" + _id, " td[address]"].join('')).html().trim());
            $("#u_email").val($(["tr#row-" + _id, " td[email]"].join('')).html().trim());
            $("#u_contact_no").val($(["tr#row-" + _id, " td[contact_no]"].join('')).html().trim());
            $("#u_tin").val($(["tr#row-" + _id, " td[tin]"].join('')).html().trim());
            SP.Status = $(["tr#row-" + _id, " td[status]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            SP.SuppID = $(["tr#row-" + _id, " td[user_id]"].join('')).attr("user_id");            
            Site.DropDownRefresh("#u_user_id");
            SP.GetSupplierAccessUser();            
            $("#saveModal *[required]").keyup();
        });        
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            //SP.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_name").val(), CONST.record_status.deactivate, _item_list);
            SP.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_user_id").val(), $("#u_contact_person").val(), $("#u_company_code").val(), $("#u_comp_name").val(),
                $("#u_address").val(), $("#u_email").val(), $("#u_contact_no").val(), $("#u_tin").val(), CONST.record_status.deactivate, _item_list);
            $('#saveModal').modal('hide');
        });
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                var _item_list = [$(ctl).data("itemid")];
                SP.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_user_id").val(), $("#u_contact_person").val(), $("#u_company_code").val(), $("#u_comp_name").val(),
                    $("#u_address").val(), $("#u_email").val(), $("#u_contact_no").val(), $("#u_tin").val(), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });
    },    
    MaintainData: function (process, id, user_id, contact_person, comp_code,comp_name,address,email,contact_no,tin, status, item_list) {
    var _result;
        var param = {
        'id': id,
        'user_id': user_id,            
        'contact_person': contact_person,
        'company_code': comp_code,
        'comp_name': comp_name,
        'address': address,
        'email': email,
        'contact_no': contact_no,
        'tin':tin,
        'status': status,
        'page_index': $(".paging").val(),
    };

    var _filter = JSON.stringify({
        'sub_menu_id': localStorage.getItem("sub-menu-id"),
        'txn': process,
        'search_key': $("#txt_search_key").val(),
        'supplier': param,
        'status': status,
        'page_index': $(".paging").val(),
        'item_list': item_list
    });
    ajaxHelper.Invoke(
        "/Setting/ResultView",
        _filter,
        "html",
        renderResultCallback(_result));
    $(".modal-backdrop").remove();
    },
    GetSupplierAccessUser: function () {
        var _item_list = [(SP.SuppID == null ? 0 : SP.SuppID)];
        var _filter = {
            'sub_menu_id': CONST.menu_id.Supplier,
            'item_list': _item_list
        };
        var postEvt = function () {
            $("#u_user_id option").remove();
            var data = $("body").data(SP.AUData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    var accusername = this.first_name + " " + this.mi + " " + this.last_name;

                    $("#u_user_id").append(["<option value='", this.id, "'>", accusername, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#u_user_id");
            if (SP.SuppID != 0) {
                $('#u_user_id').selectpicker('val', SP.SuppID);
            } else {
                $("#u_user_id").val('').selectpicker('refresh');
            }
            Site.DropDownRefresh("#u_user_id");
        }
        Site.PostData("/Setting/GetSupplierAccessUser", postEvt, _filter, SP.AUData);
    }
}
$(function() {
    SP.Initialize();
});