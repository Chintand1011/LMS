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
    public class ProjectSubStatusVM:BaseVM
    {
        [Display(Name = "Project Status Id")]
        public int proj_statid{ get; set; }


        [Display(Name = "Sub-Status")]
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
