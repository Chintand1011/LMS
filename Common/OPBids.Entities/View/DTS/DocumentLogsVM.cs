using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using OPBids.Entities.View.Home;

namespace OPBids.Entities.View.DTS
{
    public class DocumentLogsVM : BaseVM
    {
        [Display(Name = "Process")]
        public string process { get; set; }
        [Display(Name = "Logs ID")]
        public override int id { get; set; }
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Date Log")]
        public string log_date { get; set; }
        [Display(Name = "Recipient ID")]
        public int receipient_id { get; set; }
        [Display(Name = "Recipient")]
        [StringLength(150)]
        public string receipient_name { get; set; }
        [Display(Name = "Remarks")]
        [StringLength(1000)]
        public string remarks { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
