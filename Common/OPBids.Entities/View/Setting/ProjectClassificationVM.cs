using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Setting
{
    public class ProjectClassificationVM : Base.BaseVM
    {
        [Display(Name = "Classification")]
        [StringLength(20)]
        [Required]
        public string classification { get; set; }

        [Display(Name = "Description")]
        [StringLength(200)]
        public string description { get; set; }

        [Display(Name = "Status")]
        [Required]
        [StringLength(1)]
        public string status { get; set; }
    }
}
