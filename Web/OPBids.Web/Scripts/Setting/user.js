$(function () {
    $("#btnSearch").click(function () {
        var _access_type_code = $("#access_type_code").val();
        var _access_type_name = $("#access_type_name").val();
        var _filter = JSON.stringify({ 'access_type_code': _access_type_code, 'access_type_name': _access_type_name });
        ajaxHelper.Invoke("/User/RenderSearch", _filter, "html", renderSearchCallback(result));
    });

    var renderSearchCallback = function (result) {        
        return function (result) {
            $("#result").html(result);
        };       
    }

}); 