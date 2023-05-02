using System;
using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class DocumentLogs : BaseModel
    {
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Date Log")]
        public DateTime log_date { get; set; }
        [Display(Name = "Recipient ID")]
        public int receipient_id { get; set; }
        [Display(Name = "Remarks")]
        [StringLength(1000)]
        public string remarks { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
