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
    public class DocumentRoutesVM : BaseVM
    {
        [Display(Name = "Process")]
        public string process { get; set; }
        [Display(Name = "Route Number")]
        public override int id { get; set; }
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Department ID")]
        public int department_id { get; set; }
        [Display(Name = "Department")]
        [StringLength(120)]
        public string department_name { get; set; }
        [Display(Name = "Recipient ID")]
        public int receipient_id { get; set; }
        [Display(Name = "Recipient")]
        [StringLength(150)]
        public string receipient_name { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "Sequence")]
        public int? sequence { get; set; }
        [Display(Name = "CurrentReceiver")]
        public bool current_receiver { get; set; }
        [Display(Name = "Is Records Division")]
        public bool is_record_section { get; set; }
        [Display(Name = "Is Sender")]
        public bool is_sender { get; set; }
    }
}
