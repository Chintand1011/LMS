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
    public class Category: BaseModel
    {

        [Display(Name = "Category Code")]
        [StringLength(10)]
        [Index(IsUnique = true)]
        public string cat_code { get; set; }

        [Display(Name = "Category Name")]
        [StringLength(100)]
        public string cat_name { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}