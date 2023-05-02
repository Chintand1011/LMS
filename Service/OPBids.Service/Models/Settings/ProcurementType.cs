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
    public class ProcurementType: BaseModel
    {
        [Display(Name = "Procurement Type")]
        [StringLength(10)]
        [Index(IsUnique = true)]
        public string proc_type { get; set; }

        [Display(Name = "Description")]
        [StringLength(100)]
        public string proc_typedesc { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}