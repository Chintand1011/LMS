var BS = {
    Status: null,
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(BS.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        window.title = $("#headerTitle").html();
        $(".moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $("#addRemoveToolbar").html("<h4 style='white-space:nowrap;color:#007AFF;'>* Select Barcode Format To Use when printing barcode stickers</h4>");
        $(".input-group *:not(.toolbar-print):not(.toolbar-print *)").css({ "display": "none" });
        $(".toolbar-print").unbind("click");
        $(".toolbar-print").click(function () {
            alert("clicked print");
        });
        $(".p-view-result").css({ "background-color": "#d8d8d8" });
        $("input[type='radio']").change(function () {
            BS.MaintainData();
        });
    },
    MaintainData: function () {
        var _result;
        var param = {
            'id': $(".barcodeEntry tr[itemid]").attr("itemid"),
            'barcode_only': $("#barcodeOnly").is(':checked'),
            'barcode_with_print_date': $("#barcodeWithPrint").is(':checked'),
            'qr_only': $("#qrOnly").is(':checked'),
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': CONST.transaction_type.save,
            'search_key': $("#txt_search_key").val(),
            'barcodeSetting': param,
        });
        ajaxHelper.Invoke(
            "/Setting/ResultView",
            _filter,
            "html",
            renderResultCallback(_result));
        $(".modal-backdrop").remove();
    }
}
$(function () {
    BS.Initialize();
});