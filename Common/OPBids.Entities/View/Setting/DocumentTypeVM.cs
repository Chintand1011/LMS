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
    public class DocumentTypeVM : BaseVM
    {
        [Display(Name = "Document Category ID")]
        public int document_category_id { get; set; }
        [Display(Name = "Category Code")]
        [StringLength(75)]
        public string document_category_code { get; set; }
        [Display(Name = "Category Name")]
        [StringLength(100)]
        public string document_category_name { get; set; }
        [Display(Name ="Document Type")]
        [StringLength(75)]
        public string document_type_code { get; set; }
        [Display(Name = "Definition")]
        [StringLength(150)]
        public string document_type_description { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
