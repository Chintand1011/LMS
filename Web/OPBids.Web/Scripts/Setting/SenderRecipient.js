var SR = {
    CurData: null,
    UserData: null,
    Status: null,
    Results: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(SR.Initialize, 1000);
            return;
        }
        $(".toolbar-add, .item-setting-edit").css({ "display": (window.localStorage["add-edit"] == "0" ? "none" : "") });
        $(".toolbar-delete, .item-setting-delete").css({ "display": (window.localStorage["delete"] == "0" ? "none" : "") });
        SR.CurData = (new Date().getTime()).toString();
        SR.UserData = SR.CurData + "1";
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Email or Name");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        $("#btnCreateDept").unbind("click");
        $("#btnCreateDept").click(function () {
            localStorage.setItem("sub-menu-id", CONST.menu_id.Departments);
            window.location.reload();
        })
        $(".modal-dialog").css({ "margin": "200px auto !important" });
        Site.GenerateControls('#saveModal');
        $(CONST.salutations.split(',')).each(function (idx, val) {
            $("#u_salutation").append(["<option>", val, "</option>"].join(''));
        });
        $("table.table-striped tbody td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".item-setting-inactive[statusidsetter]").each(function () {
            if ($(this).attr("statusidsetter").toUpperCase() == "I") {
                $(this).html("Set as Active")
            }
        });
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            SR.Status = CONST.record_status.activate;
            var yesEvt = function () {
                $("#u_fullname_search").val('');
                SR.GetAccessUsers($("#u_fullname_search").val());
                $('#selectUserModal').modal('show');
                $("#u_first_name, #u_mi, #u_last_name, #u_salutation, #u_email_address").attr("disabled", true);
                $('#u_is_system_user').prop("checked", true);
                $("#u_department_id option").css({ "display": "" });
                $("#u_department_id option[is_internal='false']").css({ "display": "none" });
                Site.DropDownCommonSettings(".modal-body #u_department_id");
            }
            var noEvt = function () {
                $('#u_is_system_user').prop("checked", false);
                $('#saveModal').modal('show');
                SR.ReadOnlyPopUp(false);
                $("#u_first_name, #u_mi, #u_last_name, #u_salutation, #u_email_address").attr("disabled", false);
                $("#u_department_id").val($("#u_department_id option:first-child").attr("value"));
                $("#u_department_id").selectpicker('refresh');
                $("#u_email_address").removeAttr("disabled").val('');
                $("#saveModal *[required]").keyup();
                $("#u_department_id option").css({ "display": "" });
                $("#u_department_id option[is_internal='true']").css({ "display": "none" });
                Site.DropDownCommonSettings(".modal-body #u_department_id");
            }
            Site.Dialogs.Confirm("", "Do you want to Add Sender or Recipient from existing users?", "Yes", "No", yesEvt, noEvt);
        });
        var postEvt = function () {
            Site.DropDownCommonSettings("#u_department_id");
        }
        setTimeout(postEvt, 2000);
        $(".toolbar-delete").unbind("click");
        $(".toolbar-delete").click(function () {
            var _item_list = [];
            debugger;
            $(".check-selected:checked").each(function () {
                _item_list.push($(this).attr("itemid"));
            });
            if (_item_list.length <= 0) {
                Site.Dialogs.Alert("Cannot delete. Please select which one should be deleted.");
                return;
            }
            var confirmEvt = function () {
                SR.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, null, null, null, null, null, null, false,
                    false, CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            SR.MaintainData(CONST.transaction_type.search, $('#u_id').val(), $('#u_user_id').val(), $('#u_is_system_user').prop("checked"), $('#u_salutation').val(),
                $('#u_email_address').val(), $('#u_first_name').val(), $('#u_mi').val(), $('#u_last_name').val(), $('#u_department_id').val(),
                $('#u_mobile_no').val(), $('#u_is_sender').is(":checked"), $('#u_is_recipient').is(":checked"), SR.Status, null);
        });

        $(".toolbar-download").unbind("click");
        $(".toolbar-download").click(function () {
            window.location.href = ["/Setting/DownloadFile?setting=", localStorage.getItem("sub-menu-id")].join('');
        });

        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
            Site.Print(["/Setting/Print?setting=", localStorage.getItem("sub-menu-id")].join(''));
        });
        $("#btnCancel").unbind("click");
        $("#btnCancel").click(function () {
            if ($("#processId").html().toLowerCase() == "view") {
                $('#saveModal').modal('hide');
            }
            else
            {
                $('#saveModal').modal('hide');
            }
        });

        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            var isValid = Site.ValidateRequiredEntries("#saveModal", null);
            if (isValid == true) {
                if ($('#u_is_sender:checked, #u_is_recipient:checked').length <= 0) {
                    Site.Dialogs.Alert("Cannot Save. Please check at least one Sender/Recipient.");
                    isValid = false;
                    return;
                }
            }
            if (isValid == true) {
                SR.MaintainData(CONST.transaction_type.save, $('#u_id').val(), $('#u_user_id').val(), $('#u_is_system_user').prop("checked"), $('#u_salutation').val(),
                    $('#u_email_address').val(), $('#u_first_name').val(), $('#u_mi').val(), $('#u_last_name').val(), $('#u_department_id').val(), $('#u_mobile_no').val(),
                    $('#u_is_sender').is(":checked"), $('#u_is_recipient').is(":checked"), SR.Status, null);
                $('#saveModal').modal('hide');
                SR.ReadOnlyPopUp(false);
            }
        });
        $("#btnSearchAccessUser").unbind("click");
        $("#btnSearchAccessUser").click(function () {
            SR.GetAccessUsers($("#u_fullname_search").val());
        });
        $("#u_fullname_search").keydown(function (e) {
            if (e.keyCode == 13) {
                $("#btnSearchAccessUser").click();
                e.preventDefault();
                return false;
            }
        });
        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#u_is_system_user").prop("checked", $(["tr#row-" + _id, " td[is_system_user]"].join('')).attr("is_system_user").trim().toUpperCase() == "TRUE");
            $("#u_mobile_no").val($(["tr#row-" + _id, " td[mobile_no]"].join('')).attr("mobile_no").trim());
            $("#u_user_id").val($(["tr#row-" + _id, " td[user_id]"].join('')).attr("user_id").trim());
            $("#u_salutation").val($(["tr#row-" + _id, " td[salutation]"].join('')).html().trim());
            $("#u_department_id").val($(["tr#row-" + _id, " td[department_id]"].join('')).attr("department_id").trim());
            $("#u_department_id").selectpicker('refresh');
            $("#u_email_address").val($(["tr#row-" + _id, " td[email_address]"].join('')).attr("email_address"));
            $("#u_first_name").val($(["tr#row-" + _id, " td span[first_name]"].join('')).html().trim());
            $("#u_mi").val($(["tr#row-" + _id, " td span[mi]"].join('')).html().trim());
            $("#u_last_name").val($(["tr#row-" + _id, " td span[last_name]"].join('')).html().trim());
            $("#u_is_sender").prop("checked", $(["tr#row-" + _id, " td[is_sender]"].join('')).find("input[type='checkbox']").is(":checked"));
            $("#u_is_recipient").prop("checked", $(["tr#row-" + _id, " td[is_recipient]"].join('')).find("input[type='checkbox']").is(":checked"));
            SR.Status = $(["tr#row-" + _id, " td[statusid]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            SR.ReadOnlyPopUp(false);
            $("#u_email_address").attr("disabled", true);
            $("#saveModal *[required], #saveModal input").keyup();
            $("#u_department_id option").css({ "display": "" });
            $(["#u_department_id option[is_internal='", ($("#u_is_system_user").prop("checked") == true ? "false" : "true"),
                "']"].join('')).css({ "display": "none" });
            $("#btnCreateDept").css({ "display": "none" });
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");
            SR.MaintainData(CONST.transaction_type.statusUpdate, $('#u_id').val(), $('#u_user_id').val(), $('#u_is_system_user').prop("checked"), $('#u_salutation').val(),
                $('#u_email_address').val(), $('#u_first_name').val(), $('#u_mi').val(), $('#u_last_name').val(), $('#u_department_id').val(),
                $('#u_mobile_no').val(), $('#u_is_sender').is(":checked"), $('#u_is_recipient').is(":checked"),
                (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(".item-setting-viewinfo").unbind("click");
        $(".item-setting-viewinfo").click(function (e) {
            $('#saveModal').modal('show');
            $("#processId").html("View");
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#u_is_system_user").prop("checked", $(["tr#row-" + _id, " td[is_system_user]"].join('')).attr("is_system_user").trim().toUpperCase() == "TRUE");
            $("#u_mobile_no").val($(["tr#row-" + _id, " td[mobile_no]"].join('')).attr("mobile_no").trim());
            $("#u_user_id").val($(["tr#row-" + _id, " td[user_id]"].join('')).attr("user_id").trim());
            $("#u_salutation").val($(["tr#row-" + _id, " td[salutation]"].join('')).html().trim());
            $("#u_department_id").val($(["tr#row-" + _id, " td[department_id]"].join('')).attr("department_id").trim());
            $("#u_department_id").selectpicker('refresh');
            $("#u_email_address").val($(["tr#row-" + _id, " td[email_address]"].join('')).attr("email_address"));
            $("#u_first_name").val($(["tr#row-" + _id, " td span[first_name]"].join('')).html().trim());
            $("#u_mi").val($(["tr#row-" + _id, " td span[mi]"].join('')).html().trim());
            $("#u_last_name").val($(["tr#row-" + _id, " td span[last_name]"].join('')).html().trim());
            $("#u_is_sender").prop("checked", $(["tr#row-" + _id, " td[is_sender]"].join('')).find("input[type='checkbox']").is(":checked"));
            $("#u_is_recipient").prop("checked", $(["tr#row-" + _id, " td[is_recipient]"].join('')).find("input[type='checkbox']").is(":checked"));
            SR.Status = $(["tr#row-" + _id, " td[statusid]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#u_email_address").attr("disabled", true);
            $("#saveModal *[required], #saveModal input").keyup();
            $("#u_department_id option").css({ "display": "" });
            $(["#u_department_id option[is_internal='", ($("#u_is_system_user").prop("checked") == true ? "false" : "true"),
                "']"].join('')).css({ "display": "none" });
            SR.ReadOnlyPopUp(true);
        });
        $(".item-setting-delete").unbind("click");
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                var _item_list = [$(ctl).data("itemid")];
                SR.MaintainData(CONST.transaction_type.statusUpdate, $('#u_id').val(), $('#u_user_id').val(), $('#u_is_system_user').prop("checked"), $('#u_salutation').val(),
                    $('#u_email_address').val(), $('#u_first_name').val(), $('#u_mi').val(), $('#u_last_name').val(), $('#u_department_id').val(),
                    $('#u_mobile_no').val(), $('#u_is_sender').is(":checked"), $('#u_is_recipient').is(":checked"), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
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
    },
    GetSenderRecipient: function (postSaveEvt) {
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
            'search_key': $('#u_email_address').val(),
            'senderRecipientUser': param,
            'status': null,
            'item_list': null,
            'page_index': -1,
        };
        var postEvt = function () {
            var data = $("body").data(SR.UserData);
            var isValid = true;
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (parseInt($("#u_id").val()) != this.id && this.email_address.toLowerCase().trim() ==
                        $('#u_email_address').val().toLowerCase().trim()) {
                        isValid = false;
                    }
                });
            }
            if (isValid == true) {
                postSaveEvt();
            }
            else {
                Site.Dialogs.Alert("Cannot proceed. Email address already exists.");
                return;
            }
        }
        Site.PostData("/DTS/GetSenderRecipient", postEvt, _filter, SR.UserData);
    },
    ReadOnlyPopUp: function (isReadOnly) {
        if (isReadOnly == true){
            $("#btnSave, #btnCreateDept").css({ "display": "none" });
            $("#u_mobile_no, #u_is_sender, #u_is_recipient").attr("disabled", true);
            $("#saveModal .modal-body").find("#u_salutation, #u_first_name, #u_mi, #u_last_name, #u_email_address, #u_department_id").attr("disabled", "disabled");
        }
        else{
            $("#btnSave, #btnCreateDept").css({"display":""});
            $("#u_mobile_no, #u_is_sender, #u_is_recipient").attr("disabled", false);
            $("#saveModal .modal-body").find("#u_salutation, #u_first_name, #u_mi, #u_last_name, #u_email_address, #u_department_id").removeAttr("disabled");
            var isInternal = $("#u_is_system_user").prop("checked") == true;
            $("#u_salutation, #u_first_name, #u_mi, #u_last_name, #u_department_id").attr("disabled", isInternal);
        }
        $("#u_department_id").selectpicker('refresh');
        $(".modal-dialog").css({ "top": "200px" });
    },
    MaintainData: function (process, id, user_id, is_system_user, salutation, email_address, first_name, mi, last_name, department_id, mobile_no, is_sender,
        is_recipient, status, item_list) {
        var postEvt = function () {
            var _result;
            var param = {
                'id': id,
                'user_id': user_id,
                'is_system_user': is_system_user,
                'salutation': salutation,
                'email_address': email_address,
                'first_name': first_name,
                'mi': mi,
                'last_name': last_name,
                'status': status,
                'department_id': department_id,
                'department_name': null,
                'mobile_no': mobile_no,
                'is_sender': is_sender,
                'is_recipient': is_recipient
            };
            var _filter = JSON.stringify({
                'sub_menu_id': localStorage.getItem("sub-menu-id"),
                'txn': process,
                'search_key': $("#txt_search_key").val(),
                'senderRecipientUser': param,
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
        }
        if (process == CONST.transaction_type.save) {
            SR.GetSenderRecipient(postEvt);
        }
        else {
            postEvt();
        }
    },
    GetAccessUsers: function (txtSearch) {
        var _result;
        var param = {
            'id': null,
            'username': null,
            'email_address': null,
            'first_name': null,
            'mi': null,
            'last_name': null,
            'group_id': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': CONST.menu_id.AccessUsers,
            'txn': null,
            'search_key': txtSearch,
            'accessUsers': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#accessUserList tbody").html('');
            var data = $("body").data(SR.UserData);
            var availableTags = [];
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    availableTags.push([this.first_name, " ", this.mi, " ", this.last_name].join(''));
                    $("#accessUserList tbody").append(["<tr><td>", this.username, "</td><td><button dept_id='", this.dept_id, "' email_address='",
                        this.email_address, "' userid='", this.id, "' salutation='", this.salutation, "' type='button' class='buttonLink'>",
                        ["<span first_name style='padding-right:5px;'>", this.first_name, "</span><span mi style='padding-right:5px;'>", this.mi,
                            "</span><span last_name>", this.last_name, "</span>"].join(''),
                        "</button></td></tr>"].join(''));
                })
            }
            autocomplete($("#u_fullname_search")[0], availableTags);
            $("#accessUserList button").click(function(){
                $("#u_salutation").val($(this).attr("salutation"));
                $("#u_user_id").val($(this).attr("userid"));
                $("#u_first_name").val($(this).find("span[first_name]").html());
                $("#u_mi").val($(this).find("span[mi]").html());
                $("#u_last_name").val($(this).find("span[last_name]").html());
                $("#u_email_address").val($(this).attr("email_address"));
                $("#u_department_id").val($(this).attr("dept_id"));
                $("#u_department_id").selectpicker('refresh');
                $('#selectUserModal').modal('hide');
                $('#saveModal').modal('show');
                SR.ReadOnlyPopUp(true);
                $("#u_mobile_no, #u_is_sender, #u_is_recipient").attr("disabled", false);
                $("#btnSave, #btnCreateDept").css({ "display": "" });
                $("#saveModal *[required], #saveModal input").keyup();
            });
        };
        if (txtSearch == null || txtSearch == "") {
            Site.PostData("/Setting/GetAccessUser", postEvt, _filter, SR.UserData);
        }
        else {
            Site.PostData(["/Setting/GetAccessUser?searchKey=", txtSearch].join(''), postEvt, _filter, SR.UserData);
        }
    },
}
$(function() {
    SR.Initialize();
});