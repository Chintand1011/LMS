using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
   public class ProcurementMethodVM: BaseVM
    {

        [StringLength(15)]
        [Display(Name = "Code")]
        public string proc_code { get; set; }



        [Required]
        [StringLength(100)]
        [Display(Name = "Definition")]
        public string procurement_description { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Mode of Procurement")]
        public string procurement_mode { get; set; }

        [Required]
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }


    }
}
