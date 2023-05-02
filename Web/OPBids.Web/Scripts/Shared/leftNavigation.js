
$(function () {
    // Clear selected menu
    $(".sidebar-menu").children().removeClass("active");
    // Make selected class active
    $(".sidebar-menu").children().each(function () {

        var active_menu = (localStorage.getItem("active-menu-id") == "undefined" || localStorage.getItem("active-menu-id") == null) ? "P-DASH" : localStorage.getItem("active-menu-id");
        if (active_menu) {
            var menu = $(this).children("[menu-id$=" + active_menu + "]")[0];
            if ($(menu).length > 0) {
                $(menu).parent().addClass("active");
                return false;
            }
        }
    });
    
    $(".side-menu-link").click(function (e) {        
        var _activeMenu = localStorage.getItem("active-menu-id");
        var _selectedMenu = $(this).attr("menu-id");
        var _subMenuID = $(this).attr("sub-menu-id");
        // Set Active Menu ID
        localStorage.setItem("active-menu-id", _selectedMenu);
        localStorage.setItem("sub-menu-id", _subMenuID == null || _subMenuID == undefined ? _selectedMenu : _subMenuID);
        localStorage.setItem("add-edit", $(this).attr("addedit"));
        localStorage.setItem("delete", $(this).attr("delete"));

        try {
            if (e.originalEvent.isTrusted) {
                Site.ResetTempData();
            }
        }
        catch (e) {}
        
    });

    var totalResultCallback = function (result) {
        
        return function (result) {
            //chart under logo
            if (result == null || result.data == null) { return;}

            var total = result.total;
            var desc = result.desc;
            var data = result.data.map(function (e) {
                return e.amount;
            });

            var ctx_stat = document.getElementById("chart_stat").getContext('2d');
            var chart_stat = new Chart(ctx_stat, {
                type: 'bar',
                data: {
                    datasets:
                        [
                            {
                                //label: 'Bar Dataset',
                                data: data, // [3, 5, 8, 10, 2, 6, 4, 5, 6, 7, 11, 10],
                                backgroundColor: '#62CB31'
                            }
                        ],
                    labels: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]

                },
                options: {
                    scales: {
                        yAxes: [
                            {
                                display: false,
                            },
                        ],
                        xAxes: [
                            {
                                display: false,
                            }
                        ]
                    },
                    plugins: {
                        datalabels: {
                            display: false,
                        }
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        enabled: false
                    },
                    labels: {
                        display: false,
                        enabled: false
                    },
                    responsive: true,
                    maintainAspectRatio: false,

                }
            });

            $(".header-chart-desc").text(desc);
            $(".header-chart-total").text(total);
        };

       
    }

    function GetGrandTotal() {
        var result;
        if ($("#dtsProduct").is("[active]")) {
            ajaxHelper.NoWaitInvoke("/Shared/GetDocumentTotal", null, "json", totalResultCallback(result));
        }
        else {
            ajaxHelper.NoWaitInvoke("/Shared/GetProjectTotal", null, "json", totalResultCallback(result));
        }
    }
    
    GetGrandTotal();
});
