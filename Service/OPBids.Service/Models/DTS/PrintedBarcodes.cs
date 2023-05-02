using System;
using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class PrintedBarcodes : BaseModel
    {
        public int request_barcode_id { get; set; }
    }
}
