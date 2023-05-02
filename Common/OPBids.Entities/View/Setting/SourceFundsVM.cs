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
    public class SourceFundsVM:BaseVM
    {
        [Display(Name = "Code")]
        [StringLength(25)]
        [Required]
        public string source_code { get; set; }

        [Display(Name = "Definition")]
        [StringLength(100)]
        [Required]
        public string source_description { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}
