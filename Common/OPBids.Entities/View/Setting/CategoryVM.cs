using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class CategoryVM: BaseVM
    {
        [Display(Name = "Category Code")]
        [StringLength(10)]
        public string cat_code { get; set; }

        [Display(Name = "Category Name")]
        [StringLength(100)]
        public string cat_name { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
