using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class BarcodeSettingVM : BaseVM
    {        
        [Display(Name = "Barcode Only")]
        public bool barcode_only { get; set; }
        [Display(Name = "Barcode with Print Date")]
        public bool barcode_with_print_date { get; set; }
        [Display(Name = "QR Only")]
        public bool qr_only { get; set; }
    }
}
