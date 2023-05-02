var PROJAREAS = {
    AreaData: null,
    Status: null,
    SaveAction: null,    
    Area: null,  
    AreaID: null,
    CityID: null,
    OldCityID: null,
    DistrictID: null,
    OldDistrictID: null,
    BarangayID: null,    
    Initialize: function () {
        if ($("#headerTitle").length <= 0) {
            setTimeout(PROJAREAS.Initialize, 1000);
            return;
        }
        $("#pageContainer").css({ "display": "inline-block" });
        PROJAREAS.AreaData = (new Date().getTime()).toString();        
        window.title = $("#headerTitle").html();
        $("#toolbar-delete-text .moduleCaption").html($("#headerTitle").html());
        $(document).attr("title", $("#headerTitle").html());
        $("#txt_search_key").attr("placeholder", "Search by City or District or Barangay");
        $("#toolbarContent").css({ "display": "block" });
        $("#toolbarContent .toolbar-add-batch").hide();
        $(".toolbar-add").remove();
        var addbtnbrgy = "";
        addbtnbrgy = "<a class='toolbar-add' href='#' id='btnBrgy'>";
        addbtnbrgy += "<img src='/Content/images/icon-add.png' />";
        addbtnbrgy += "<span id='toolbar-add-text' style='color: steelblue1'>Add Barangay</span></a>";
        $("#addRemoveToolbar").prepend(addbtnbrgy);
        var addbtndist = "";
        addbtndist = "<a class='toolbar-add' href='#' id='btnDist'>";
        addbtndist += "<img src='/Content/images/icon-add.png' />";
        addbtndist += "<span id='toolbar-add-text' style='color: steelblue1'>Add District</span></a>";
        $("#addRemoveToolbar").prepend(addbtndist);
        var addbtncity = "";
        addbtncity = "<a class='toolbar-add' href='#' id='btnCity'>";
        addbtncity += "<img src='/Content/images/icon-add.png' />";
        addbtncity += "<span id='toolbar-add-text' style='color: steelblue1'>Add City</span></a>";
        $("#addRemoveToolbar").prepend(addbtncity);
        $("#txt_search_key").unbind("keyup");
        $("#txt_search_key").keyup(function (e) {
            if (e.which == 13) {
                $(".toolbar-search").click();
            }
        });
        $(".modal-dialog").css({ "top": "200px" });
        Site.GenerateControls('#saveModal');
        $("table.table-striped tbody td[status],span[statusid]").each(function () {
            $(this).html(Site.GetStatusDescription($(this).attr("statusid")));
        });
        $(".item-setting-inactive[statusidsetter]").each(function () {
            if ($(this).attr("statusidsetter").toUpperCase() == "I") {
                $(this).html("Set as Active")
            }
        });
        PROJAREAS.GetProjectAreasCity();        
        $(".toolbar-add").unbind("click");
        $(".toolbar-add").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create");
            PROJAREAS.Status = CONST.record_status.activate;
            $('#saveModal').modal('show');
            $("#saveModal *[required]").keyup();
        });        
        $("#btnCity").unbind("click");
        $("#btnCity").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");
            $("#processId").html("Create New");
            $("h4 .moduleCaption").html("City");            
            PROJAREAS.Area = "City";
            RedrawAddEntry(PROJAREAS.Area);
            PROJAREAS.Status = CONST.record_status.activate;
            PROJAREAS.SaveAction = "add";
            $('#saveModal').modal('show');            
            $("#saveModal *[required]").keyup();
            
        });
        $("#btnDist").unbind("click");
        $("#btnDist").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");            
            $("#processId").html("Create New");
            $("h4 .moduleCaption").html("District");
            PROJAREAS.Area = "District";
            RedrawAddEntry(PROJAREAS.Area);
            PROJAREAS.Status = CONST.record_status.activate;
            PROJAREAS.SaveAction = "add";
            $('#saveModal').modal('show');
            $("#u_city_id").val('').selectpicker('refresh');
            $("#saveModal *[required]").keyup();
        });
        $("#btnBrgy").unbind("click");
        $("#btnBrgy").click(function (e) {
            e.preventDefault();
            Site.ClearAllData('#saveModal');
            $("#u_id").val("0");            
            $("#processId").html("Create New");
            $("h4 .moduleCaption").html("Barangay");            
            PROJAREAS.Area = "Barangay";
            RedrawAddEntry(PROJAREAS.Area);
            PROJAREAS.Status = CONST.record_status.activate;
            PROJAREAS.SaveAction = "add";            
            $('#saveModal').modal('show');
            $("#u_city_id").val('').selectpicker('refresh');            
            $("#u_district_id option").remove();
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
                PROJAREAS.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_city_id").val(), $("#u_city_name").val(), $("#u_district_id").val(), $("#u_district_name").val(), $("#u_barangay_id").val(), $("#u_barangay_name").val(), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });

        $(".toolbar-search").unbind("click");
        $(".toolbar-search").click(function () {            
            PROJAREAS.MaintainData(CONST.transaction_type.search, $("#u_id").val(), $("#u_city_id").val(), $("#u_city_name").val(), $("#u_district_id").val(), $("#u_district_name").val(), $("#u_barangay_id").val(), $("#u_barangay_name").val(), PROJAREAS.Status, null);
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
                if (PROJAREAS.Area == "City") {
                    if (PROJAREAS.SaveAction == "add") {      
                        PROJAREAS.AreaID = 0;
                        PROJAREAS.CityID = 0;
                        PROJAREAS.DistrictID = 0;
                        PROJAREAS.BarangayID = 0;
                    }                
                    PROJAREAS.EntryCityValidationAndSaveProjectAreas();
                } else if (PROJAREAS.Area == "District") {
                    PROJAREAS.CityID = $("#u_city_id").val();
                    if (PROJAREAS.SaveAction == "add") {
                        PROJAREAS.AreaID = 0;
                        PROJAREAS.OldCityID = 0;
                        PROJAREAS.DistrictID = 0;                        
                        PROJAREAS.BarangayID = 0;
                    }                      
                    PROJAREAS.EntryDistrictValidationAndSaveProjectAreas();
                } else if (PROJAREAS.Area == "Barangay") {
                    PROJAREAS.CityID = $("#u_city_id").val();
                    PROJAREAS.DistrictID = $("#u_district_id").val();
                    if (PROJAREAS.SaveAction == "add") {
                        PROJAREAS.AreaID = 0;
                        PROJAREAS.OldCityID = 0;
                        PROJAREAS.OldDistrictID = 0;
                        PROJAREAS.BarangayID = 0;                    
                    }                      
                    PROJAREAS.EntryBarangayValidationAndSaveProjectAreas();
                }                
            }
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

        $(".item-setting-edit").unbind("click");
        $(".item-setting-edit").click(function (e) {
            e.preventDefault();
            $("#updateModal").modal("toggle");            

            var _id = $(this).attr("data-itemid");            
            $("#u_id").val(_id);   
            PROJAREAS.AreaID = _id;
            PROJAREAS.CityID = $(["tr#row-" + _id, " td[city_id]"].join('')).attr("city_id");
            PROJAREAS.OldCityID = PROJAREAS.CityID
            $("#u_city_name").val($(["tr#row-" + _id, " td[city_id]"].join('')).html().trim());
            PROJAREAS.DistrictID = $(["tr#row-" + _id, " td[district_id]"].join('')).attr("district_id");
            PROJAREAS.OldDistrictID = PROJAREAS.DistrictID
            $("#u_district_name").val($(["tr#row-" + _id, " td[district_id]"].join('')).html().trim());
            PROJAREAS.BarangayID = $(["tr#row-" + _id, " td[barangay_id]"].join('')).attr("barangay_id");            
            $("#u_barangay_name").val($(["tr#row-" + _id, " td[barangay_id]"].join('')).html().trim());
            if (PROJAREAS.BarangayID != 0) {
                PROJAREAS.Area = "Barangay"
            } else if (PROJAREAS.DistrictID != 0) {
                PROJAREAS.Area = "District"
            } else {
                PROJAREAS.Area = "City"                
            }
            $("#processId").html("Edit " + PROJAREAS.Area);
            RedrawAddEntry(PROJAREAS.Area);
            PROJAREAS.Status = $(["tr#row-" + _id, " td[status]"].join('')).attr("statusid").trim();
            PROJAREAS.SaveAction = "edit"; 
            $('#saveModal').modal('show');
            $('#u_city_id').selectpicker('val', PROJAREAS.CityID);
            Site.DropDownRefresh("#u_city_id");
            PROJAREAS.GetProjectAreasDistrictByCity();            
            $("#saveModal *[required]").keyup();            
        });
        $(".item-setting-inactive").unbind("click");
        $(".item-setting-inactive").click(function (e) {
            e.preventDefault();
            var _item_list = [$(this).data("itemid")];
            var stat = $(this).attr("statusidsetter");            
            PROJAREAS.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_city_id").val(), $("#u_city_name").val(), $("#u_district_id").val(), $("#u_district_name").val(), $("#u_barangay_id").val(), $("#u_barangay_name").val(), 
                (stat == "I" ? CONST.record_status.activate : CONST.record_status.deactivate), _item_list);
        });
        $(".item-setting-delete").click(function (e) {
            var ctl = this;
            e.preventDefault();
            var confirmEvt = function () {
                var _item_list = [$(ctl).data("itemid")];
                PROJAREAS.MaintainData(CONST.transaction_type.statusUpdate, $("#u_id").val(), $("#u_city_id").val(), $("#u_city_name").val(), $("#u_district_id").val(), $("#u_district_name").val(), $("#u_barangay_id").val(), $("#u_barangay_name").val(), CONST.record_status.delete, _item_list);
            }
            Site.Dialogs.Confirm(["Remove ", $("#headerTitle").html()].join(''), ["Are you sure you want to remove selected ",
                $("#headerTitle").html(), "(s)?"].join(''), "Yes", "Cancel", confirmEvt, null);
        });
        $('#u_city_id').change(function (e) {
            if (PROJAREAS.Area == "Barangay") {
                PROJAREAS.CityID = $(this).val();
                PROJAREAS.GetProjectAreasDistrictByCity();
            }            
        });
        function RedrawAddEntry(area) {
            if (area == "City") {
                $("#city_name").show();
                $("#city_id").hide();
                $("#district_name").hide();
                $("#district_id").hide();
                $("#barangay_name").hide();
                $("#barangay_id").hide();
            } else if (area == "District") {
                $("#city_name").hide();
                $("#city_id").show();
                $("#district_name").show();
                $("#district_id").hide();
                $("#barangay_name").hide();
                $("#barangay_id").hide();
            } else if (area == "Barangay") {
                $("#city_name").hide();
                $("#city_id").show();
                $("#district_name").hide();
                $("#district_id").show();
                $("#barangay_name").show();
                $("#barangay_id").hide();
            } 
        };
    },
    MaintainData: function (process, id, city_id, city_name, district_id, district_name, barangay_id, barangay_name, status, item_list) {
        var _result;
        var param = {
            'id': id,
            'city_id': city_id,
            'city_name': city_name,
            'district_id': district_id,
            'district_name': district_name,
            'barangay_id': barangay_id,
            'barangay_name': barangay_name,            
            'status': status,
            'page_index': $(".paging").val(),
        };

        var _filter = JSON.stringify({
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': process,
            'search_key': $("#txt_search_key").val(),
            'projectareas': param,
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
    EntryCityValidationAndSaveProjectAreas: function () {        
        var city_name = $("#u_city_name").val();        
        var _item_list = [PROJAREAS.CityID, city_name];
        var param = {
            'id': null,
            'city_name': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': CONST.menu_id.ProjAreaCity,
            'txn': null,
            'search_key': null,
            'projectareascity': param,
            'status': null,
            'item_list': _item_list
        };
        var postEvt = function (e) {
            var data = $("body").data(PROJAREAS.AreaData);            
            var _ErrMsg = "";            
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (PROJAREAS.SaveAction == "add" && this.city_name.toLowerCase() == city_name.toLowerCase()) {                        
                        PROJAREAS.CityID = this.id;
                        return;
                    } 
                    if (PROJAREAS.SaveAction == "edit" && this.id != PROJAREAS.CityID && this.city_name.toLowerCase() == city_name.toLowerCase()) {
                        _ErrMsg = "Cannot update City! " + city_name + " already exists!";
                        return;
                    }
                });

                if (_ErrMsg != "") {
                    Site.Dialogs.Alert(_ErrMsg);
                }
            }
            if (PROJAREAS.SaveAction == "add") {
                PROJAREAS.EntryProjAreaValidationAndSaveProjectAreas("City", city_name);
                //Site.Dialogs.Alert("City Saved!");
            }
            else {
                PROJAREAS.MaintainData(CONST.transaction_type.search, null, null, null, null, null, null, null, PROJAREAS.Status, null);
            }
        }

        Site.PostData("/Setting/GetAndSaveProjectAreasCity", postEvt, _filter, PROJAREAS.AreaData);
        
    },
    EntryDistrictValidationAndSaveProjectAreas: function () {        
        var district_name = $("#u_district_name").val();
        var _item_list = [PROJAREAS.DistrictID, PROJAREAS.CityID, PROJAREAS.OldCityID, district_name];
        var param = {
            'id': null,
            'city_id': null,
            'district_name': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.menu_id.ProjAreaDist,
            'txn': null,
            'search_key': null,
            'projectareasdistrict': param,
            'status': null,
            'item_list': _item_list
        };
        var postEvt = function () {
            var data = $("body").data(PROJAREAS.AreaData);
            var _ErrMsg = "";
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (PROJAREAS.SaveAction == "add" && this.city_id == PROJAREAS.CityID && this.district_name.toLowerCase() == district_name.toLowerCase()) {
                        PROJAREAS.DistrictID = this.id;
                        return;
                    } else if (PROJAREAS.SaveAction == "edit" ) {
                        if (this.id != PROJAREAS.DistrictID &&
                            this.city_id == PROJAREAS.CityID && this.city_id == PROJAREAS.OldCityID &&
                            this.district_name.toLowerCase() == district_name.toLowerCase()) {
                            _ErrMsg = "Cannot update District! " + district_name + " already exists!";
                            return;
                        } else {
                            PROJAREAS.DistrictID = this.id;
                        }
                    }
                });
            } 

            if (_ErrMsg != "") {
                Site.Dialogs.Alert(_ErrMsg);
            } else {
                if (PROJAREAS.SaveAction == "add") { PROJAREAS.EntryProjAreaValidationAndSaveProjectAreas("District", district_name); }
                else {
                    if (PROJAREAS.SaveAction == "edit" && PROJAREAS.CityID != PROJAREAS.OldCityID) {                        
                        PROJAREAS.EntryProjAreaValidationAndSaveProjectAreas("District", district_name);
                    } else {
                        PROJAREAS.MaintainData(CONST.transaction_type.search, null, null, null, null, null, null, null, PROJAREAS.Status, null);
                    }                    
                }
            }
        }

        Site.PostData("/Setting/GetAndSaveProjectAreasDistrict", postEvt, _filter, PROJAREAS.AreaData);
    },
    EntryBarangayValidationAndSaveProjectAreas: function () {       
        var barangay_name = $("#u_barangay_name").val();
        var _item_list = [PROJAREAS.BarangayID, PROJAREAS.CityID, PROJAREAS.OldCityID, PROJAREAS.DistrictID, PROJAREAS.OldDistrictID, barangay_name];
        var param = {
            'id': null,
            'city_id': null,
            'district_id': null,
            'barangay_name': null,
            'status': null,
        };

        var _filter = {
            'sub_menu_id': CONST.menu_id.ProjAreaBrgy,
            'txn': null,
            'search_key': null,
            'projectareasbarangay': param,
            'status': null,
            'item_list': _item_list
        };
        var postEvt = function () {            
            var data = $("body").data(PROJAREAS.AreaData);
            var _ErrMsg = "";
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (PROJAREAS.SaveAction == "add" && this.barangay_name.toLowerCase() == barangay_name.toLowerCase()) {
                        PROJAREAS.BarangayID = this.id;
                        return;
                    } else if (PROJAREAS.SaveAction == "edit") {
                        if (this.id != PROJAREAS.BarangayID &&
                            this.city_id == PROJAREAS.CityID && this.city_id == PROJAREAS.OldCityID &&
                            this.district_id == PROJAREAS.DistrictID && this.district_id == PROJAREAS.OldDistrictID &&
                            this.barangay_name.toLowerCase() == barangay_name.toLowerCase()) {
                            _ErrMsg = "Cannot update Barangay! " + barangay_name + " already exists!";
                            return;
                        } else {
                            PROJAREAS.BarangayID = this.id;
                        }                        
                    }
                });
            } 

            if (_ErrMsg != "") {
                Site.Dialogs.Alert(_ErrMsg);
            } else {
                if (PROJAREAS.SaveAction == "add") { PROJAREAS.EntryProjAreaValidationAndSaveProjectAreas("Barangay", barangay_name); }
                else {                    
                    if (PROJAREAS.SaveAction == "edit" && (PROJAREAS.CityID != PROJAREAS.OldCityID || PROJAREAS.DistrictID != PROJAREAS.OldDistrictID)) {
                        PROJAREAS.EntryProjAreaValidationAndSaveProjectAreas("Barangay", district_name);
                    } else {
                        PROJAREAS.MaintainData(CONST.transaction_type.search, null, null, null, null, null, null, null, PROJAREAS.Status, null);
                    }
                }
            }
        }

        Site.PostData("/Setting/GetAndSaveProjectAreasBarangay", postEvt, _filter, PROJAREAS.AreaData);
    },
    EntryProjAreaValidationAndSaveProjectAreas: function (area, value) {
        var _id = PROJAREAS.AreaID;
        var _cityid = PROJAREAS.CityID;        
        var _distid = PROJAREAS.DistrictID;        
        var _brgyid = PROJAREAS.BarangayID;
       var param = {
            'id': null,
            'city_id': null,
            'city_name': null,
            'district_id': null,
            'district_name': null,
            'barangay_id': null,
            'barangay_name': null,
            'status': null,
            'area': null,
        };
        var _filter = {
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': null,
            'search_key': null,
            'projectareas': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            var data = $("body").data(PROJAREAS.AreaData);
            var _ErrMsg = "";
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    if (PROJAREAS.SaveAction == "add") {
                        if (this.city_id == _cityid && this.district_id == _distid && this.barangay_id == _brgyid) {
                            _ErrMsg = "Cannot create new " + area + "! " + value + " already exists";
                            return;
                        }
                    }
                });

                if (_ErrMsg != "") {
                    Site.Dialogs.Alert(_ErrMsg);
                }
            }
            if (_ErrMsg == "") {
                PROJAREAS.MaintainData(CONST.transaction_type.save, _id, _cityid, $("#u_city_name").val(), _distid, $("#u_district_name").val(), _brgyid, $("#u_barangay_name").val(), PROJAREAS.Status, null);
                $('#saveModal').modal('hide');                
            }
        }

        Site.PostData("/Setting/GetProjectAreas", postEvt, _filter, PROJAREAS.AreaData);        
    },
    GetProjectAreasCity: function () {
        var param = {
            'id': null,
            'city_name': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': CONST.menu_id.ProjAreaCity,
            'txn': null,
            'search_key': null,
            'projectareascity': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#u_city_id option").remove();
            var data = $("body").data(PROJAREAS.AreaData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_city_id").append(["<option value='", this.id, "'>", this.city_name, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#u_city_id");
        }
        Site.PostData("/Setting/GetProjectAreasCity", postEvt, _filter, PROJAREAS.AreaData);
    },
    GetProjectAreasDistrict: function () {
        var param = {
            'id': null,
            'district_name': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': null,
            'search_key': null,
            'projectareasdistrict': param,
            'status': null,
            'item_list': null
        };
        var postEvt = function () {
            $("#u_district_id option").remove();
            var data = $("body").data(PROJAREAS.AreaData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_district_id").append(["<option value='", this.id, "'>", this.district_name, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#u_district_id");
        }
        Site.PostData("/Setting/GetProjectAreasDistrict", postEvt, _filter, PROJAREAS.AreaData);
    },
    GetProjectAreasDistrictByCity: function () {
        var _item_list = [PROJAREAS.CityID];
        var param = {
            'id': null,
            'district_name': null,
            'status': null,
        };
        var _filter = {
            'sub_menu_id': localStorage.getItem("sub-menu-id"),
            'txn': null,
            'search_key': null,
            'projectareasdistrict': param,
            'status': null,
            'item_list': _item_list
        };
        var postEvt = function () {
            $("#u_district_id option").remove();
            var data = $("body").data(PROJAREAS.AreaData);
            if (data != null && data != undefined && data != "") {
                $(data).each(function () {
                    $("#u_district_id").append(["<option value='", this.id, "'>", this.district_name, "</option>"].join(''));
                })
            }
            Site.DropDownCommonSettings("#u_district_id");            
            if (PROJAREAS.DistrictID != 0) {
                $('#u_district_id').selectpicker('val', PROJAREAS.DistrictID);
            }
            Site.DropDownRefresh("#u_district_id");
        }
        Site.PostData("/Setting/GetProjectAreasDistrictByCity", postEvt, _filter, PROJAREAS.AreaData);
    }
}
$(function () {
    PROJAREAS.Initialize();
});