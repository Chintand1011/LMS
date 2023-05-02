var AU = {
    CurData: null,
    Status: null,
    Results: null,
    SaveAction: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(AU.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        AU.CurData = (new Date().getTime()).toString();        
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by Username/Name, Dept. Code/Name, Access Group, or Email").css({"width":"400px"});
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        $("[data-toggle=tooltip]").mouseenter(function () {
            var $this = $(this);
            $this.attr('title', $this.val());
        });
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
            $("#u_dept_id").val($("#u_dept_id option:first-child").attr("value"));
            Site.DropDownRefresh("#u_dept_id");
            $("#processId").html("Create");
            AU.Status = CONST.record_status.activate;
            AU.SaveAction = "add";
            $("#saveModal *[required]").keyup();
            $('#saveModal').modal('show');
            $("#u_password").val("").removeAttr("required").attr("required", "").parent().parent().css({ "display": "" });
            $("#u_repassword").val("").removeAttr("required").attr("required", "").parent().parent().css({ "display": "" });
        });
        AU.GetAccessGroups();
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
                AU.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, null, null, null, null, null, null, null, null, null, CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });
        $(".toolbar-add").click(function () {
            $("#u_password").val("").removeAttr("required").attr("required", "").parent().parent().css({ "display": "" });
            $("#u_repassword").val("").removeAttr("required").attr("required", "").parent().parent().css({ "display": "" });
        });
        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            AU.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_salutation").val(), $("#u_username").val(), null,
                $("#u_email_address").val(), $("#u_first_name").val(), $("#u_mi").val(), $("#u_last_name").val(), $("#u_group_id").val(), $("#u_dept_id").val(),
                    $("#u_vip_access").prop("checked"), $("#u_pfms_access").prop("checked"), $("#u_dts_access").prop("checked"),
                    AU.Status, null);
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
                AU.EntryValidationAndSaveAccessUser();
            }
        });
        $("#btnCreateAccessGroup").unbind("click");
        $("#btnCreateAccessGroup").click(function () {
            localStorage.setItem("sub-menu-id", CONST.menu_id.AccessGroups);
            window.location.reload();
        })
        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $("#u_dept_id").val($(["tr#row-" + _id, " td[dept_id]"].join('')).html());
            $("#u_salutation").val($(["tr#row-" + _id, " td[salutation]"].join('')).attr("salutation"));
            $("#u_username").val($(["tr#row-" + _id, " td[username]"].join('')).html().trim());
            $("#u_password").val("").removeAttr("required").parent().parent().css({ "display": "none" });
            $("#u_repassword").val("").removeAttr("required").parent().parent().css({ "display": "none" });
            $("#u_email_address").val($(["tr#row-" + _id, " td[email_address]"].join('')).html());
            $("#u_first_name").val($(["tr#row-" + _id, " td span[first_name]"].join('')).html().trim());
            $("#u_mi").val($(["tr#row-" + _id, " td span[mi]"].join('')).html().trim());
            $("#u_last_name").val($(["tr#row-" + _id, " td span[last_name]"].join('')).html().trim());
            $("#u_group_id").val($(["tr#row-" + _id, " td[group_id]"].join('')).attr("group_id"));
            $('#u_group_id').selectpicker('val', $('#u_group_id').val());
            $('#u_dept_id').selectpicker('val', $('#u_dept_id').val());
            Site.DropDownRefresh("#u_group_id");
            Site.DropDownRefresh("#u_dept_id");
            $("#u_vip_access").prop("checked", $(["tr#row-" + _id, " td[vip_access]"].join('')).find("input[type='checkbox']").is(":checked"));
            $("#u_dts_access").prop("checked", $(["tr#row-" + _id, " td[dts_access]"].join('')).find("input[type='checkbox']").is(":checked"));
            $("#u_pfms_access").prop("checked", $(["tr#row-" + _id, " td[pfms_access]"].join('')).find("input[type='checkbox']").is(":checked"));
            AU.Status = $(["tr#row-" + _id, " td[statusid]"].join('')).attr("statusid").trim();
            AU.SaveAction = "edit";
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        $(".item-setting-reset").unbind("click");
        $(".item-setting-reset").click(function (e) {
            e.preventDefault();
            var _id = $(this).attr("data-itemid");
            var email = $(["tr#row-" + _id, " td[email_address]"].join('')).html();
            console.log(email);
            var _result;
            var param = {
                'email_address': email
            };
            var _filter = JSON.stringify({
                'accessUsers': param,
            });
            ajaxHelper.Invoke(
                "/Setting/ResetAccessUserPassword",
                _filter,
                "json",
                AU.renderResetPasswordCallback(_result));
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");
            AU.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_salutation").val(), $("#u_username").val(), null,
                $("#u_email_address").val(), $("#u_first_name").val(), $("#u_mi").val(), $("#u_last_name").val(), $("#u_group_id").val(), $("#u_dept_id").val(),
                    $("#u_vip_access").prop("checked"), $("#u_pfms_access").prop("checked"), $("#u_dts_access").prop("checked"),
                    (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                var _item_list = [$(ctl).data("itemid")];
                AU.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_salutation").val(), $("#u_username").val(), null, 
                    $("#u_email_address").val(), $("#u_first_name").val(), $("#u_mi").val(), $("#u_last_name").val(), $("#u_group_id").val(), $("#u_dept_id").val(),
                    $("#u_vip_access").prop("checked"), $("#u_pfms_access").prop("checked"), $("#u_dts_access").prop("checked"),
                    CONST.record_status.delete, _item_list);
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
    },
    MaintainData: function (process, id, salutation, username, password, email_address, first_name, mi, last_name, group_id, dept_id, vip_access, pfms_access, dts_access, status, item_list) {
        var _result;
        var param = {
            'id': id,
            'username': username,
            'password': password,
            'email_address': email_address,
            'first_name': first_name,
            'mi': mi,
            'last_name': last_name,
            'group_id': group_id,
            'dept_id': dept_id,
            'status': status,
            'salutation': salutation,
            'vip_access': vip_access,
            'pfms_access': pfms_access,
            'dts_access': dts_access,
            'page_index': $(".paging").val(),
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'accessUsers': param,
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
    GetAccessGroups: function () {
        var _result;
        var param = {
            'id': null,
            'dept_code': null,
            'dept_name': null,
            'is_main_dept': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.menu_id.AccessGroups,
            'txn': null,
            'search_key': null,
            'accessGroups': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#u_group_id option").remove();
            var data = $("body").data(AU.CurData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_group_id").append(["<option value='", this.id, "' desc='", this.group_description, "'>", this.group_code, "</option>"].join(''));
                })
                Site.DropDownCommonSettings("#u_group_id");
            }
        }
        var postEvt2 = function () {
            $("#u_dept_id option").remove();
            var data = $("body").data(AU.CurData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_dept_id").append(["<option desc='", this.dept_description, "' value='", this.id, "'>", this.dept_code, "</option>"].join(''));
                })
                Site.DropDownCommonSettings("#u_dept_id");
            }
        }


        Site.PostData("/Setting/GetAccessGroups", postEvt, _filter, AU.CurData);
        Site.PostData("/Setting/GetDepartment",postEvt2, _filter, AU.CurData);
    },
    EntryValidationAndSaveAccessUser: function () {
        var regexName = new RegExp("^[a-zA-Z\b-]+$");
        var regexEmail = new RegExp('[ñ]')
        var regexUsername = new RegExp('[ ]')
        var email_address = $("#u_email_address").val();        
        var username = $("#u_username").val();
        var _id = $("#u_id").val();

        if (email_address.match(regexEmail)) {
            Site.Dialogs.Alert("Email address cannot accept ñ character!");
            return;
        } 

        if (username.match(regexUsername)) {
            Site.Dialogs.Alert("Username cannot accept space character!");
            return;
        } 

        if ($("#u_password").val() === $("#u_repassword").val()) {            
            var param = {
                'id': null,
                'username': null,
                'email_address': null,
                'status': null,
            };

            var _filter = {
                'sub_menu_id': localStorage.getItem("sub-menu-id"),
                'txn': null,
                'search_key': null,
                'accessUsers': param,
                'status': null,
                'item_list': null
            };            
            var postEvt = function () {
                var data = $("body").data(AU.CurData);
                var _ErrMsg = "";
                if (data != null && data != undefined && data != "") {                                        
                    $(data).each(function () {
                        if (AU.SaveAction == "add" && this.username == username) {
                            _ErrMsg = "Username already exists!";
                            return;
                        }
                        if (this.id != _id && this.email_address == email_address) {
                            _ErrMsg = "Email Address already exists!";
                            return;
                        }
                    });

                    if (_ErrMsg != "") {
                        Site.Dialogs.Alert(_ErrMsg);
                    } 
                }
                if (_ErrMsg == "") {
                    AU.MaintainData(CONST.transaction_type.save, $("#u_id").val(), $("#u_salutation").val(), $("#u_username").val(), $("#u_password").val(),
                        $("#u_email_address").val(), $("#u_first_name").val(), $("#u_mi").val(), $("#u_last_name").val(), $("#u_group_id").val(), $("#u_dept_id").val(),
                        $("#u_vip_access").prop("checked"), $("#u_pfms_access").prop("checked"),
                        $("#u_dts_access").prop("checked"), AU.Status, null);
                    $('#saveModal').modal('hide');
                }                
            }

            Site.PostData("/Setting/GetAccessUser", postEvt, _filter, AU.CurData);            
        } else {
            Site.Dialogs.Alert("Password should match.");
        }
    },
    renderResetPasswordCallback: function (result) {
        return function (result) {
            if (result.status.code == 0) {
                Site.Dialogs.Alert("Reset password successful.<br>Please check email for Change Password link.", "Close", null);
            } else {
                Site.Dialogs.Alert("Error resetting password. Please try again.", "Close", null);
            }
        }
    }
}
$(function() {
    AU.Initialize();
});