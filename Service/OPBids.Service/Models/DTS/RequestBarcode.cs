using System;
using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class RequestBarcode : BaseModel
    {
        public int requested_quantity { get; set; }
        public int printed_quantity { get; set; }
        [StringLength(1)]
        public string status { get; set; }
        [StringLength(1000)]
        public string remarks { get; set; }
    }
}
