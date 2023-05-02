var ATTACHMENT = function () {
    //Use only for advertisement attachment?
    //Set allowed file types to images
    //Removed attachment name and barcode

    var _attachmentObj = {
        isEnable: true,
        btnBrowse: null,
        fileBrowse: null,
        formBrowse: null,
        limit: 999,
        fileArray: []
    };

    var _clone = function (obj) {
        return $.parseJSON(JSON.stringify(obj));
    };

    var _attachmentParam = {
        GUID: null,
        Blob: null,
    };

    //BEGIN - to maintain
    var Philgeps = _clone(_attachmentObj);
    var MMDA = _clone(_attachmentObj);
    var cons_mmda = _clone(_attachmentObj);
    var cons_redep = _clone(_attachmentObj);
    var cons_command = _clone(_attachmentObj);
    var NewsPaper = _clone(_attachmentObj);

    var _getAttachmentObj = function (fileBrowseId) {
        switch (fileBrowseId) {
            case "fleBrowse_philgeps":
                Philgeps.isEnable = true;
                Philgeps.btnBrowse = "btnBrowse_philgeps";
                Philgeps.fileBrowse = "fleBrowse_philgeps";
                Philgeps.formBrowse = "formPhilgeps";
                Philgeps.limit = 1;
                return Philgeps;
            case "fleBrowse_mmda":
                MMDA.isEnable = true;
                MMDA.btnBrowse = "btnBrowse_mmda";
                MMDA.fileBrowse = "fleBrowse_mmda";
                MMDA.formBrowse = "formMMDA";
                MMDA.limit = 1;
                return MMDA;
            case "fleBrowse_cons_mmda":
                cons_mmda.isEnable = true;
                cons_mmda.btnBrowse = "btnBrowse_cons_lobby";
                cons_mmda.fileBrowse = "fleBrowse_cons_mmda";
                cons_mmda.formBrowse = "formConspicuous .mmda";
                cons_mmda.limit = 1;
                return cons_mmda;
            case "fleBrowse_cons_redep":
                cons_redep.isEnable = true;
                cons_redep.btnBrowse = "btnBrowse_cons_redep";
                cons_redep.fileBrowse = "fleBrowse_cons_redep";
                cons_redep.formBrowse = "formConspicuous .redep";
                cons_redep.limit = 1;
                return cons_redep;
            case "fleBrowse_cons_command":
                cons_command.isEnable = true;
                cons_command.btnBrowse = "btnBrowse_cons_command";
                cons_command.fileBrowse = "fleBrowse_cons_command";
                cons_command.formBrowse = "formConspicuous .command";
                cons_command.limit = 1;
                return cons_command;
            case "fleBrowse_news":
                NewsPaper.isEnable = true;
                NewsPaper.btnBrowse = "btnBrowse_news";
                NewsPaper.fileBrowse = "fleBrowse_news";
                NewsPaper.formBrowse = "formNewspaper";
                NewsPaper.limit = 1;
                return NewsPaper;
            default:
                return null;
        }
    };
    //END - to maintain

    var _handleLimit = function (fileBrowseId) {
        var attachmentObj = _getAttachmentObj(fileBrowseId);
        if (attachmentObj.fileArray.length < attachmentObj.limit) {
            ATTACHMENT.Enable(fileBrowseId);
        }
        else {
            ATTACHMENT.Disable(fileBrowseId);
        }
    };

    var _validate = function (fileBrowseId, blob, val) {
        var attachmentObj = _getAttachmentObj(fileBrowseId);
        var fBrowse = attachmentObj.fileBrowse;
        var formBrowse = attachmentObj.formBrowse;
        val = val.toLowerCase();
        console.log(val);
        var regex = new RegExp("(.*?)\.(jpg|png|gif)$");

        if (attachmentObj != null) {
            console.log(regex.test(val.toLowerCase()));
            if ((regex.test(val.toLowerCase()))) {
                //var attachmentName = window.prompt("Please enter an attachment name", "");
                //if (attachmentName == null || attachmentName == "") {
                //    $("#" + fBrowse).replaceWith($("#" + fBrowse).val('').clone(true));
                //    return;
                //}
                var param = _clone(_attachmentParam);
                param.GUID = moment(new Date()).format("YYYYMMDDhhmmssSS");
                //param.AttachmentName = attachmentName;
                param.FileName = val;
                param.Blob = blob;
                attachmentObj.fileArray.push(param);
                var imgCls = Utilities.GetFileTypeClass(val);
                $("#" + formBrowse + " .output").append(["<div status='A' class='attachmentItem' guid='", param.GUID,
					"' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
					"<div class='", imgCls, "' title='For upload'><img/></div></div>"].join(''));

                if (attachmentObj.isEnable) {
                    $(["#" + formBrowse + " .attachmentItem[guid='", param.GUID, "']"].join('')).append(["<div style='text-align:center;width:100%;'><button type='button' class='removeItem btn btn-danger btn-block' fileBrowseId='", fileBrowseId, "'></button></div>"].join(''));
                    $(["#" + formBrowse + " .attachmentItem[guid='", param.GUID, "'] .removeItem"].join('')).click(function () {
                        var fbId = $(this).attr('fileBrowseId');
                        var attObj = ATTACHMENT.GetAttachmentObj(fbId);
                        var guidValue = $(this).parent().parent().attr("GUID");
                        attObj.fileArray = attObj.fileArray.filter(function (obj) {
                            return obj.GUID !== guidValue;
                        });
                        $(this).parent().parent().remove();
                        ATTACHMENT.HandleLimit(fbId);
                    });
                }

                ATTACHMENT.HandleLimit(fileBrowseId);
            }
            else {
                Site.Dialogs.Alert("Invalid file extension, please upload a file with jpg, png, gif extensions.", "OK", null);
            }
            $("#" + fBrowse).replaceWith($("#" + fBrowse).val('').clone(true));
        }
    };


    var _enable = function (fileBrowseId) {
        var attObj = _getAttachmentObj(fileBrowseId);
        attObj.isEnable = true;
        var btnBrowse = attObj.btnBrowse;
        var fileBrowse = attObj.fileBrowse;
        var attachmentForm = attObj.attachmentForm;
        $("#" + btnBrowse).parent().show();
        $("#" + btnBrowse).unbind().click(function () {
            $("#" + fileBrowse).click();
        });

        $("#" + fileBrowse).unbind().change(function () {
            ATTACHMENT.Validate($(this).attr('id'), this.files[0], $(this).val());
        });

        $("#" + attachmentForm).unbind("filedrop");
        $("#" + attachmentForm).filedrop({
            callback: function (blobData, curId, pFile) {
                ATTACHMENT.Validate(fileBrowse, blobData, pFile[0].name);
            }
        });
    };

    var _disable = function (fileBrowseId) {
        var attObj = _getAttachmentObj(fileBrowseId);
        attObj.isEnable = false;
        var btnBrowse = attObj.btnBrowse;
        var fileBrowse = attObj.fileBrowse;
        var attachmentForm = attObj.attachmentForm;

        $("#" + btnBrowse).parent().hide();

        $("#" + btnBrowse).unbind("click");
        $("#" + fileBrowse).unbind("change");
        $("#" + attachmentForm).unbind();
    };

    var _setValue = function (fileBrowseId, val, readonly) {

        if (val != null && val != "") {
        var attachmentObj = _getAttachmentObj(fileBrowseId);
        var btnBrowse = attachmentObj.btnBrowse;
        var formBrowse = attachmentObj.formBrowse;
        if (attachmentObj != null) {
            $("#" + btnBrowse).parent().hide();
            var imgCls = Utilities.GetFileTypeClass(val);

            $("#" + formBrowse + " .output").empty().append(["<div status='A' class='attachmentItem' guid='", 
                "' process='", CONST.transaction_type.save, "' filename='", val, "' style='float:left;'>",
                "<div class='", imgCls, "'><img/></div></div>"].join(''));

            if (attachmentObj.isEnable && readonly!=true) {
                $(["#" + formBrowse + " .attachmentItem"].join('')).append(["<div style='text-align:center;width:100%;'><button type='button' class='removeItem btn btn-danger btn-block' fileBrowseId='", fileBrowseId, "'></button></div>"].join(''));
                $(["#" + formBrowse + " .attachmentItem .removeItem"].join('')).click(function () {
                    var fbId = $(this).attr('fileBrowseId');
                    var attObj = ATTACHMENT.GetAttachmentObj(fbId);
                    var guidValue = $(this).parent().parent().attr("GUID");
                    attObj.fileArray = attObj.fileArray.filter(function (obj) {
                        return obj.GUID !== guidValue;
                    });
                    $(this).parent().parent().remove();
                    ATTACHMENT.HandleLimit(fbId);
                });
            }
            }
        }
    };

    var _reset = function () {
        ATTACHMENT.Philgeps = _clone(_attachmentObj);
        ATTACHMENT.MMDA = _clone(_attachmentObj);
        ATTACHMENT.cons_mmda = _clone(_attachmentObj);
        ATTACHMENT.cons_redep = _clone(_attachmentObj);
        ATTACHMENT.cons_command = _clone(_attachmentObj);
        ATTACHMENT.NewsPaper = _clone(_attachmentObj);

        $(".browseContainer .output").empty();
        $(".browseContainer .trigger").show();


    };
    return {
        Enable: function (fileBrowseId) {
            _enable(fileBrowseId);
        },
        Disable: function (fileBrowseId) {
            _disable(fileBrowseId);
        },
        Validate: function (fileBrowseId, blob, val) {
            _validate(fileBrowseId, blob, val);
        },
        Clone: function (obj) {
            return $.parseJSON(JSON.stringify(obj));
        },
        AttachmentParam: {
            GUID: null,
            Blob: null,
        },
        AttachmentObj: {
            btnBrowse: null,
            fileBrowse: null,
            formBrowse: null,
            fileArray: []
        },
        GetAttachmentObj: function (fileBrowseId) {
            return _getAttachmentObj(fileBrowseId);
        },
        HandleLimit: function (fileBrowseId) {
            _handleLimit(fileBrowseId);
        },
        SetValue: function (fileBrowseId, val, readonly) {
            _setValue(fileBrowseId, val, readonly)
        },
        Reset: function() {
            _reset();
        }
    }
}();