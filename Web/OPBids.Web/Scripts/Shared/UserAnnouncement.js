var UA = {
    Id: null,
    UserId: null,
    CurData: null,
    SRData: null,
    Status: null,
    SelectedRecipients: null,
    Initialize: function () {
        UA.CurData = (new Date().getTime()).toString();
        UA.SRData = [(new Date().getTime()).toString(), "-1"].join('');
        UA.MaintainData(CONST.transaction_type.search);
        $("#btnAnnouncement").click(function () {
            UA.GetSenderRecipient();
        });
        $("#btnMarkAsRead").click(function () {
            var ids = [];
            $("#lstAnnouncements tbody tr input[type='checkbox']:checked").each(function(){
                ids.push($(this).attr("itemid"));
            });
            UA.MaintainData(CONST.transaction_type.read, ids.join(','));
        });
        $("#btnMarkAsUnread").click(function () {
            var ids = [];
            $("#lstAnnouncements tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UA.MaintainData(CONST.transaction_type.unread, ids.join(','));
        });
        $("#btnStarred").click(function () {
            var ids = [];
            $("#lstAnnouncements tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UA.MaintainData(CONST.transaction_type.star, ids.join(','));
        });
        $("#btnRemoveStar").click(function () {
            var ids = [];
            $("#lstAnnouncements tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UA.MaintainData(CONST.transaction_type.unstar, ids.join(','));
        });
        $("#btnHideAnnouncement").click(function () {
            var ids = [];
            $("#lstAnnouncements tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UA.MaintainData(CONST.transaction_type.hide, ids.join(','));
        });
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
            'item_list': null
        };
        var postEvt = function () {
            var options = [];
            var data = $("body").data(UA.SRData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (this.is_recipient == true) {
                        options.push(["<option value='user_", this.id, "'>", this.first_name, " ", this.mi, " ", this.last_name, "</option>"].join(''));
                    }
                })
            }
            var SendEvt = function () {
                UA.MaintainData(CONST.transaction_type.create, null, $("#dialogInput").val(), false, false, false)
            }
            Site.Dialogs.Input("Make Announcement", "Announcement:", "Send", "Close", SendEvt, null);
            $("#dialogInput").attr("maxlength", 120);
            $("#dialogInput").attr("rows", 5);
            $(".dialogBody").css({"padding-bottom":"0px"})
            Site.GenerateControls('.dialogBody');
            $("#dialogInput").attr("placeholder", "What's your announcement?");
            $("#negativeDialogButton").css({ "display": "none" });
            var cntnt = ["<div style='font-size:16px;padding-bottom:10px;'>",
                "<select id='recipient_ids' multiple required caption='Recipients' class='form-control selectpicker' data-none-selected-text>",
                "<optgroup label='Suggested User' id='groupUser' style='font-size:16px;font-weight:bold;'>",
                options.join(''), "</optgroup><optgroup label='Suggested Group' id='groupDepartment' ", 
                "style='font-size:16px;font-weight:bold;'><=0=></optgroup></select>"].join('')
            UA.GetDepartments(cntnt);
        }
        Site.PostData("/DTS/GetSenderRecipient", postEvt, _filter, UA.SRData);
    },
    GetDepartments: function (cntnt) {
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
            var data = $("body").data(UA.SRData);
            var subCntnt = [];
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    subCntnt.push(["<option value='dept_", this.id, "'>",
                        this.dept_description, "</option>"].join(''));
                })
            }
            cntnt = cntnt.replace("<=0=>", subCntnt.join(''));
            $(".dialogBody").css({ "padding-top": "10px" }).prepend(cntnt);

            $('#recipient_ids').selectpicker({
                liveSearch: true,
                showSubtext: true,
                noneSelectedText: 'Select Recipient...'
            }).on('changed.bs.select', function (e) {
                UA.SelectedRecipients = [];
                $(e.target.selectedOptions).each(function () {
                    UA.SelectedRecipients.push($(this).val());
                });
            });
        }
        Site.PostData("/Setting/GetDepartment", postEvt, _filter, UA.SRData);
    },
    MaintainData: function (currentProcess, pIds, pMessage, pIsRead, pIsStarred, pIshidden) {
        var recipientIds = [];
        var departmentIds = [];
        var curVal = '';
        $(UA.SelectedRecipients).each(function () {
            curVal = this;
            if (curVal.replace('user_', '') != curVal) {
                recipientIds.push(curVal.replace('user_', ''));
            }
            if (curVal.replace('dept_', '') != curVal) {
                departmentIds.push(curVal.replace('dept_', ''));
            }
        })
        if (CONST.transaction_type.create != currentProcess && CONST.transaction_type.search != currentProcess && $("#lstAnnouncements tbody tr input[type='checkbox']:checked").length <= 0) {
            return;
        }
        var param = {
                menu_id: localStorage.getItem("sub-menu-id"),
                userAnnouncement: {
                ids: pIds,
                process: currentProcess,
                id: UA.Id,
                message: $.trim(pMessage),
                is_read: pIsRead,
                is_starred: pIsStarred,
                is_hidden: pIshidden,
                recipient_ids: recipientIds.join(','),
                department_ids: departmentIds.join(','),
                recipient_names: null,
                sender_id: UA.UserId,
                sender_name: null,
                date_sent: null,
            },
        }
        var postEvt = function () {
            var data = $("body").data(UA.CurData);
            var newCount = 0;
            $("#lstAnnouncements tbody").html('');
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    UA.Id = this.id;
                    $("#lstAnnouncements tbody").append(["<tr ", (this.is_read == false ? "style='background-color:#b3cdfb;'" : ""),
                    "><td style='padding-left:10px !important;'><input itemid='", this.id, "' type='checkbox' style='width:15px;height:15px;' /></td><td>",
                        "<div class='profilePic'></div></td><td style='padding-left:20px;' ", (this.is_starred == true ? "class='rowStar'" : ""),
                        "><span style='font-size:14px;font-weight:bold;'>[", this.sender_name,
                        "]</span>&nbsp;&nbsp;<span style='font-size:10px;color:#0061FE;'>", this.date_sent, "</span><br />",
                        this.message, "<br />", "<span style='font-size:10px;color:#808080;'>Other Recipient(s):&nbsp;", this.recipient_names,
                        "</span></td></tr>"].join(''));
                    if (this.is_read == false) {
                        newCount++;
                    }
                });
                if (currentProcess != CONST.transaction_type.search) {
                    window.location.reload();
                }
            }
            $(".totalAnnouncements").html(newCount);
        };
        Site.PostData("/Shared/MaintainUserAnnouncement", postEvt, param, UA.CurData);
    }

  
}
$(function() {
    UA.Initialize();
});