using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Service.Models.Settings
{
    public class BarcodeSetting : BaseModel
    {
        public bool barcode_only { get; set; }
        public bool barcode_with_print_date { get; set; }
        public bool qr_only { get; set; }
    }
}