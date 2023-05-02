var CP = {
    Initialize: function () {
                     
        $("#btnSave").unbind("click");
        $("#btnSave").click(function () {
            if (Site.ValidateRequiredEntries("#saveModal", null) == true) {
                CP.EntryValidationAndSaveAccessUser();
            }
        });

    },
    
    EntryValidationAndSaveAccessUser: function () {
        if ($("#n_password").val() === $("#con_password").val()) {
            var param = {
                'id': null,
                'username': null,

                }
            Site.Dialogs.Alert('Hello');    
         //    Site.PostData("/Setting/ChangePassword", postEvt, _filter, CP.CurData);
        }else {
            Site.Dialogs.Alert('Password should match');
        }
      }
}

$(function () {
    CP.Initialize();
});