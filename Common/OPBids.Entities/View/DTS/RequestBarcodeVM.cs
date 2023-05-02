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
    public class RequestBarcodeVM : BaseVM
    {
        [Display(Name = "Process")]
        public string process { get; set; }
        [Display(Name = "Department ID")]
        public int department_id { get; set; }
        [StringLength(250)]
        [Display(Name = "Department")]
        public string department { get; set; }
        [Display(Name = "Request Number")]
        public override int id { get; set; }
        [Display(Name = "Requested By")]
        [StringLength(150)]
        public string requested_by { get; set; }
        [Display(Name = "Requested Quantity")]
        public int requested_quantity { get; set; }
        [Display(Name = "Printed Quantity")]
        public int printed_quantity { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "Remarks")]
        [StringLength(1000)]
        public string remarks { get; set; }
    }
}
