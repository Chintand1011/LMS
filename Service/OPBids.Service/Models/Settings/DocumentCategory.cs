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
    public class DocumentCategory : BaseModel
    {
        [StringLength(75)]
        [Required]
        [Index(IsUnique = true)]
        public string document_category_code { get; set; }
        [StringLength(150)]
        [Required]
        public string document_category_name { get; set; }
        [StringLength(1)]
        public string status { get; set; }
    }
}