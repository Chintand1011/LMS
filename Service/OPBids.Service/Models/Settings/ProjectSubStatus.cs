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
    public class ProjectSubStatus:BaseModel
    {
        [Required]
        public int proj_statid { get; set; }

        [Display(Name = "Code")]
        [StringLength(75)]
        [Required]
        public string proj_substatcode { get; set; }

        [Display(Name = "Definition")]
        [StringLength(250)]
        [Required]
        public string proj_substatdescription { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}