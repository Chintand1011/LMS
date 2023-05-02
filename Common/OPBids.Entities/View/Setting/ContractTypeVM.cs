using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
  public  class ContractTypeVM:BaseVM
    {
        [Display(Name = "Contract Type")]
        [StringLength(10)]
        public string contract_type { get; set; }

        [Display(Name = "Description")]
        [StringLength(100)]
        public string contract_desc { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}
