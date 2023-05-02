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
    public class RecordClassification : BaseModel
    {

        [Display(Name = "Classification Code")]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string classification_code { get; set; }

        [Display(Name = "Classification Description")]
        [StringLength(100)]
        public string classification_desc { get; set; }
    }
}