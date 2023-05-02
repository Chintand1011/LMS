using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class DocumentCategoryVM: BaseVM
    {        
        [Display(Name = "Code")]
        [StringLength(75)]
        [Required]
        public string document_category_code { get; set; }

        [Display(Name = "Definition")]
        [StringLength(150)]
        [Required]
        public string document_category_name { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
