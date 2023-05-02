
var DEPT = {
    CurrentId: null,
    DeptData: null,
    UserData: null,
    Status: null,
    ParentDepartmentId: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(DEPT.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        DEPT.DeptData = (new Date().getTime()).toString();
        DEPT.UserData = (new Date().getTime() + 1).toString();
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
		$("#txt_search_key").attr("placeholder", "Search by Code or Definition");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        $(".modal-dialog").css({ "top": "200px" });
        $("#txSearchSelected").keyup(function (e) {
            $("#subDepartments tbody td[deptcode]").each(function () {
                var curTR = $(this).parent();
                $(curTR).css({"display":"none"});
                if ($(this).html().includes($("#txSearchSelected").val().trim())) {
                    $(curTR).css({"display":""});
                }
            });
        });
        $("#txSearchAvailable").keyup(function (e) {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13') {
                DEPT.GetDepartmentsToAssign(DEPT.CurrentId, false);
            }
        });
        Site.GenerateControls('#saveModal');
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            DEPT.ParentDepartmentId = 0;
            $("#processId").html("Create");
            DEPT.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');
            $("#u_headed_by").val('').selectpicker('refresh');
            $("#saveModal *[required]").keyup();
        });
        DEPT.GetAccessUsers();
        $("#btnViewUserInfo").unbind("click");
        $("#btnViewUserInfo").click(function () {
            localStorage.setItem("sub-menu-id", CONST.menu_id.AccessUsers);
            window.location.reload();
        })
        $("#rightArrow").unbind("click");
        $("#rightArrow").click(function () {
            $("#departmentsAvailable tbody input[type='checkbox']:checked").each(function () {
                $("#subDepartments tbody").append(["<tr><td deptcode>", $(this).parent().parent().find("td[deptcode]").html(),
                    "</td><td style='text-align:right;'><input dept_id='", $(this).attr("dept_id"), "' type='checkbox' /></td></tr>"].join(''));
                $(this).parent().parent().remove();
            });
        });
        $("#rightrightArrow").unbind("click");
        $("#rightrightArrow").click(function () {
            $("#departmentsAvailable tbody input[type='checkbox']").each(function () {
                $("#subDepartments tbody").append(["<tr><td deptcode>", $(this).parent().parent().find("td[deptcode]").html(),
                    "</td><td style='text-align:right;'><input dept_id='", $(this).attr("dept_id"), "' type='checkbox' /></td></tr>"].join(''));
                $(this).parent().parent().remove();
            });
        });
        $("#leftArrow").unbind("click");
        $("#leftArrow").click(function () {
            $("#subDepartments tbody input[type='checkbox']:checked").each(function () {
                $("#departmentsAvailable tbody").append(["<tr><td style='text-align:left;'><input dept_id='", $(this).attr("dept_id"),
                    "' type='checkbox' /></td><td deptcode>", $(this).parent().parent().find("td[deptcode]").html(),
                    "</td></tr>"].join(''));
                $(this).parent().parent().remove();
            });
        });
        $("#leftleftArrow").unbind("click");
        $("#leftleftArrow").click(function () {
            $("#subDepartments tbody input[type='checkbox']").each(function () {
                $("#departmentsAvailable tbody").append(["<tr><td style='text-align:left;'><input dept_id='", $(this).attr("dept_id"),
                    "' type='checkbox' /></td><td deptcode>", $(this).parent().parent().find("td[deptcode]").html(),
                    "</td></tr>"].join(''));
                $(this).parent().parent().remove();
            });
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
                DEPT.MaintainData(CONST.transaction_type.statusUpdate, 0, null, null, null, $("#u_designation").val(), CONST.record_status.delete, null, _item_list);                
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $("#btnSaveSubDepartments").unbind("click");
        $("#btnSaveSubDepartments").click(function () {
            var _item_list = [];
            $("#subDepartments tbody input[type='checkbox']").each(function () {
                _item_list.push($(this).attr("dept_id"));
            });
            DEPT.MaintainData(CONST.transaction_type.assign, $("#u_id").val(), null, null, null, null, null, null, _item_list);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {
            DEPT.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_dept_code").val(), $("#u_dept_description").val(), $("#u_headed_by").val(), $("#u_designation").val(), DEPT.Status, null, null);
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
                var postEvt = function () {
                    DEPT.MaintainData(CONST.transaction_type.save, $("#u_id").val(), $("#u_dept_code").val(), $("#u_dept_description").val(), $("#u_headed_by").val(), $("#u_designation").val(), DEPT.Status, $("#u_is_internal").prop("checked"), null);
                    $('#saveModal').modal('hide');
                }
                DEPT.IsExistDepartments(postEvt);
            }
        });

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");
            $("#processId").html("Edit");

            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            DEPT.ParentDepartmentId = $(["tr#row-" + _id, " td[parent_dept_id]"].join('')).attr("parent_dept_id");
            $("#u_headed_by").val($(["tr#row-" + _id, " td[headed_by]"].join('')).attr("headed_by")).selectpicker('refresh');
            $("#u_dept_code").val($(["tr#row-" + _id, " td[dept_code]"].join('')).text().trim());
            $("#u_dept_description").val($(["tr#row-" + _id, " td[dept_description]"].join('')).text().trim());
            $("#u_is_internal").prop("checked", ($(["tr#row-" + _id, " td[is_internal]"].join('')).attr("is_internal") == "1"));
            $("#u_designation").val($(["tr#row-" + _id, " td[designation]"].join('')).text().trim());
            DEPT.Status = $(["tr#row-" + _id, " td[statusid]"].join('')).attr("statusid").trim();
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });
        $(".item-setting-delete").unbind("click");
        $(".item-setting-delete").click(function (e) {
            e.preventDefault();
            var ctl = this;
            var confirmEvt = function () {
                var _item_list = [$(ctl).attr("data-itemid")];
                DEPT.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_dept_code").val(), $("#u_dept_description").val(), $("#u_headed_by").val(), $("#u_designation").val(), CONST.record_status.delete, $("#u_is_internal").prop("checked"), _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });
        $(".item-setting-assign").unbind("click");
        $(".item-setting-assign").click(function (e) {
            e.preventDefault();
            var _id = $(this).attr("data-itemid");
            $("#u_id").val(_id);
            $(".lblParentDepartment").html($(["tr#row-" + _id, " td[dept_code]"].join('')).html().toString().toUpperCase());
            $(".lblHeadedBy").html([$(["tr#row-" + _id, " td[headed_by]"].join('')).html().toString().toUpperCase(), " - ",
                $(["tr#row-" + _id, " td[designation]"].join('')).html().trim()].join(''));
            DEPT.CurrentId = _id;
            DEPT.GetDepartmentsToAssign(_id, true);
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
    MaintainData: function (process, id, dept_code, dept_description, headed_by, designation, status, is_internal,item_list) {
        var _result;
        var param = {
            'id': id,
            'parent_dept_id': DEPT.ParentDepartmentId,
            'dept_code': dept_code,
            'dept_description': dept_description,
            'headed_by': headed_by,
            'designation': designation,
            "status": status,
            "is_internal": is_internal,
            'page_index': $(".paging").val(),
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'id': id,
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'department': param,
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
    IsExistDepartments: function (postSaveEvt) {
        var _result;
        var param = {
            'id': $("#u_id").val(),
            'parent_dept_id': null,
            'dept_code': $("#u_dept_code").val(),
            'dept_description': null,
            'headed_by': null,
            'designation': null,
            "status": null,
        };
        var _filter = {
            'sub_menu_id': CONST.menu_id.Departments,
            'txn': null,
            'id': $("#u_id").val(),
            'search_key': $("#u_dept_code").val(),
            'department': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            var data = $("body").data(DEPT.DeptData);
            if (data != null && data != undefined && data != "") {
                Site.Dialogs.Alert("Cannot save. Department code already exists.", null, null);
            }
            else {
                postSaveEvt();
            }
        }
        Site.PostData("/Setting/IsExistDepartments", postEvt, _filter, DEPT.DeptData);
    },
    GetDepartmentsToAssign: function (parentId, clearAvailable) {
        var _result;
        var param = {
            'id': null,
            'parent_dept_id': parentId,
            'dept_code': null,
            'dept_description': null,
            'headed_by': null,
            'designation': null,
            "status": null,
        };
        var _filter = {
            'sub_menu_id': CONST.menu_id.Departments,
            'txn': null,
            'id': parentId,
            'search_key': $("#txSearchAvailable").val(),
            'department': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#departmentsAvailable tbody").html('');
            if (clearAvailable == true) {
                $("#subDepartments tbody").html('');
            }
            var data = $("body").data(DEPT.DeptData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (this.parent_dept_id == 0) {
                        if ($(["#subDepartments tbody tr input[dept_id='", this.id, "']"].join('')).length <= 0) {
                            $("#departmentsAvailable tbody").append(["<tr><td style='text-align:left;'><input dept_id='", this.id, "' type='checkbox' /></td><td deptcode>", this.dept_code, "</td></tr>"].join(''));
                        }
                    }
                    else {
                        if (clearAvailable == true) {
                            $("#subDepartments tbody").append(["<tr><td deptcode>", this.dept_code, "</td><td style='text-align:right;'><input dept_id='", this.id, "' type='checkbox' /></td></tr>"].join(''));
                        }
                    }
                })
            }
            $('#assignSubLevel').modal('show');
            $(".modal-dialog").css({ "top": "10px" });
        }
        Site.PostData("/Setting/GetDepartmentsToAssign", postEvt, _filter, DEPT.DeptData);
    },
    GetAccessUsers: function () {
        var _result;
        var param = {
            'id': null,
            'username': null,
            'email_address': null,
            'first_name': null,
            'mi': null,
            'last_name': null,
            'group_id': null,
            'department_id': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.menu_id.AccessUsers,
            'txn': null,
            'search_key': null,
            'accessUsers': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#u_headed_by option").remove();
            var data = $("body").data(DEPT.UserData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_headed_by").append(["<option value='", this.id, "'>", this.first_name, " ", this.mi, " ", this.last_name, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#u_headed_by");
        }
        Site.PostData("/Setting/GetAccessUser", postEvt, _filter, DEPT.UserData);
    },
}
$(function () {
    DEPT.Initialize();
});