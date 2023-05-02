var PBP = {
    MaxCount: 0,
    CurrentCount: 0,
    BarcodeType: null,
    DateToday: null,
    Initialize: function () {
        if (Site.GetParameterByName("minbc") != null && Site.GetParameterByName("maxbc") != null) {
            var minBC = Site.GetParameterByName("minbc");
            var maxBC = Site.GetParameterByName("maxbc");
            PBP.MaxCount = maxBC - minBC;
            var dt = new Date();
            var deptCode = ["000".substring(0, "000".length - window.sessionStorage['curBarcode'].length), window.sessionStorage['curBarcode']].join('');
            var barcodeFormat = [dt.getYear(), "0000000000000"].join('');
            while (minBC <= maxBC) {
                switch (PBP.BarcodeType) {
                    case 0:
                        $("#barcodeContainer").append(["<div class='barcodePreviewContainer'><center><img width='125px;' style='padding-top:3px;display:block;padding-bottom:3px;' ",
                            "onload='javascript:PBP.ImageLoaded();' src='../Content/images/barcodePrintTemplate.png' /><svg class='barcode' id='barcode", minBC, "'></svg></center><div style='visibility:hidden;font-size:10px;text-align:center;left:-92px;top:-35px; transform: rotate(-90deg);position:relative;z-index:10000;'>", PBP.DateToday, "</div><div style='font-size:10px;text-align:center;left:155px;top:-50px; transform: rotate(-90deg);position:fixed;white-space:nowrap;z-index:10000;'>Dept Code: ", deptCode, "</div></div>"].join(''));
                        JsBarcode(["#barcode", minBC].join(''), [barcodeFormat.substring(0, barcodeFormat.length - minBC.toString().length), minBC].join(''), {
                            format: "CODE128",
                            lineColor: "#000000",
                            width: 1.4,
                            height: 40,
                            fontSize: 14,
                            margin: 0,
                            displayValue: true,
                            textAlign: "center"
                        });
                        break;
                    case 1:
                        $("#barcodeContainer").append(["<div class='barcodePreviewContainer'><center><img width='123px;' style='padding-top:3px;display:block;padding-bottom:3px;' ",
                            "onload='javascript:PBP.ImageLoaded();' src='../Content/images/barcodePrintTemplate.png' /><svg class='barcode' id='barcode", minBC, "' style='display:block;'></svg></center><div style='font-size:10px;text-align:center;left:-92px;top:-35px; transform: rotate(-90deg);position:relative;z-index:10000;'>", PBP.DateToday, "</div><div style='font-size:10px;text-align:center;left:154px;top:-50px; transform: rotate(-90deg);position:fixed;white-space:nowrap;z-index:10000;'>Dept Code: ", deptCode, "</div></div>"].join(''));
                        JsBarcode(["#barcode", minBC].join(''), [barcodeFormat.substring(0, barcodeFormat.length - minBC.toString().length), minBC].join(''), {
                            format: "CODE128",
                            lineColor: "#000000",
                            width: 1.4,
                            height: 30,
                            fontSize: 14,
                            margin: 0,
                            displayValue: true,
                            textAlign: "center"
                        });
                        break;
                    default:
                        var qrCodeVal = [barcodeFormat.substring(0, barcodeFormat.length - minBC.toString().length), minBC].join('')
                        $("#barcodeContainer").append(["<div class='barcodePreviewContainer'><div class='barcode' id='barcode", minBC, "' style='padding:20px 5px 0px;width:70px;height:70px;float:left;'></div><div style='float:left;'><div style='font-size:12px;text-align:center;left:70px;top:45px; transform: rotate(-90deg);position:relative;white-space:nowrap;z-index:10000;'>Dept Code: ", deptCode, "</div><div style='font-size:11px;text-align:center;padding:0px;position:relative;top:20px;'>Print Date: ", PBP.DateToday, "</div><br/><img width='110px;' style='padding:3px 0px 3px;display:block;' ",
                            "onload='javascript:PBP.ImageLoaded();' src='../Content/images/qrTemplate.png' /></div></div>"].join(''));
                        $(["#barcode", minBC].join('')).qrcode({ width: 70, height: 70, text: qrCodeVal });
                        break;
                }
                minBC++;
            }
        }
    },
    ImageLoaded: function () {
        PBP.CurrentCount++;
        if (PBP.CurrentCount >= PBP.MaxCount) {
            window.print();
        }
    }
}
$(function () {
    PBP.Initialize();
});