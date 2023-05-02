using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
   public class ProcurementTypeVM: BaseVM
    {
        [Display(Name = "Procurement Type")]
        [StringLength(10)]
        public string proc_type { get; set; }

        [Display(Name = "Definition")]
        [StringLength(100)]
        public string proc_typedesc { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }



    }
}
