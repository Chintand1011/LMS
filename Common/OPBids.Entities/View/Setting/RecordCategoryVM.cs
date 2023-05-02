using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class RecordCategoryVM : BaseVM
    {
        [Display(Name = "Classification Id")]
        public int classification_id { get; set; }

        [Display(Name = "Category Code")]
        [StringLength(30)]
        public string category_code { get; set; }

        [Display(Name = "Category Description")]
        [StringLength(100)]
        public string category_desc { get; set; }

        [Display(Name = "Retention Period (in years)")]
        public int retention_period { get; set; }
    }
}
