var UN = {
    Id: null,
    UserId: null,
    CurData: null,
    SRData: null,
    Status: null,
    Initialize: function () {
        UN.CurData = (new Date().getTime()).toString();
        UN.SRData = [(new Date().getTime()).toString(), "-1"].join('');
        UN.MaintainData(CONST.transaction_type.search);

        $("#btnMarkAsRead").click(function () {
            var ids = [];
            $("#lstNotifications tbody tr input[type='checkbox']:checked").each(function(){
                ids.push($(this).attr("itemid"));
            });
            UN.MaintainData(CONST.transaction_type.read, ids.join(','));
        });
        $("#btnMarkAsUnread").click(function () {
            var ids = [];
            $("#lstNotifications tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UN.MaintainData(CONST.transaction_type.unread, ids.join(','));
        });
        $("#btnStarred").click(function () {
            var ids = [];
            $("#lstNotifications tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UN.MaintainData(CONST.transaction_type.star, ids.join(','));
        });
        $("#btnRemoveStar").click(function () {
            var ids = [];
            $("#lstNotifications tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UN.MaintainData(CONST.transaction_type.unstar, ids.join(','));
        });
        $("#btnHideNotification").click(function () {
            var ids = [];
            $("#lstNotifications tbody tr input[type='checkbox']:checked").each(function () {
                ids.push($(this).attr("itemid"));
            });
            UN.MaintainData(CONST.transaction_type.hide, ids.join(','));
        });
    },
    MaintainData: function (currentProcess, pIds, pMessage, pIsRead, pIsStarred, pIshidden) {
        var recipientIds = [];
        if (CONST.transaction_type.search == currentProcess) {
            recipientIds.push(UN.UserId);
        }
        if (CONST.transaction_type.search != currentProcess && $("#lstNotifications tbody tr input[type='checkbox']:checked").length <= 0) {
            return;
        }
        var param = {
                menu_id: localStorage.getItem("sub-menu-id"),
                UserNotification: {
                ids: pIds,
                process: currentProcess,
                id: UN.Id,
                message: $.trim(pMessage),
                is_read: pIsRead,
                is_starred: pIsStarred,
                is_hidden: pIshidden,
                recipient_ids: recipientIds.join(','),
                department_ids: null,
                recipient_names: null,
                sender_id: UN.UserId,
                sender_name: null,
                date_sent: null,
            },
        }
        var postEvt = function () {
            var data = $("body").data(UN.CurData);
            var newCount = 0;
            $("#lstNotifications tbody").html('');
            if (data.value != null && data.value != undefined && data.value != "") {
                $(data.value).each(function () {
                    UN.Id = this.id;
                    $("#lstNotifications tbody").append(["<tr ", (this.is_read == false ? "style='background-color:#b3cdfb;'" : ""),
                    "><td style='padding-left:10px !important;'><input itemid='", this.id, "' type='checkbox' style='width:15px;height:15px;' /></td><td>",
                        "<div class='profilePic'></div></td><td style='padding-left:20px;' ", (this.is_starred == true ? "class='rowStar'" : ""),
                        ">", decodeURI(this.message), "<br />", "<span style='font-size:10px;color:#808080;'><span style='font-size:10px;color:#0061FE;'>",
                        this.date_sent, "</span></td></tr>"].join(''));
                    if (this.is_read == false) {
                        newCount++;
                    }
                });
                if (currentProcess != CONST.transaction_type.search) {
                    window.location.reload();
                }
            }
            $(".totalNotifications").html(newCount);
        };
        Site.PostData("/Shared/MaintainUserNotification", postEvt, param, UN.CurData);
    }

  
}
$(function() {
    UN.Initialize();
});