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
    public class SourceFunds: BaseModel
    {
        [Display(Name = "Source of Fund Code")]
        [StringLength(25)]
        [Required]
        public string source_code { get; set; }

        [Display(Name = "Description")]
        [StringLength(100)]
        [Required]
        public string source_description { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}