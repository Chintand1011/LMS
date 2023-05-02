using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using static OPBids.Common.Enum;

namespace OPBids.Entities.View.Setting
{
   public class ProjectStatusVM:BaseVM
    {
        [Display(Name = "Code")]
        [StringLength(75)]
        [Required]
        public string proj_statcode { get; set; }

        [Display(Name = "Definition")]
        [StringLength(250)]
        [Required]
        public string proj_statdescription { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}

