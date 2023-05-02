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
    public class RecordCategory : BaseModel
    {
        [Display(Name = "Classification Id")]
        public int classification_id { get; set; }

        [Display(Name = "Category Code")]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string category_code { get; set; }

        [Display(Name = "Category Description")]
        [StringLength(100)]
        public string category_desc { get; set; }

        [Display(Name = "Retention Period (in years)")]
        public int retention_period { get; set; }
    }
}