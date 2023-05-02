using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPBids.Service.Models.Settings
{
    public class ContractType:BaseModel
        
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