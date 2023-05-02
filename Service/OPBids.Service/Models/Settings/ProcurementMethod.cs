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
    public class ProcurementMethod: BaseModel
     {
        [Required]
        [StringLength(15)]
        [Index(IsUnique = true)]
        public string proc_code { get; set; }

        [Required]
        [StringLength(100)]
        public string procurement_description { get; set; }

        [Required]
        [StringLength(3)]
        public string procurement_mode { get; set; }

        [Required]
        [StringLength(1)]
        public string status { get; set; }


    }
}