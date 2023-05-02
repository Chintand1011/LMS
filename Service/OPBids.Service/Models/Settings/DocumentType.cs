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
    public class DocumentType : BaseModel
    {
        [Display(Name = "Document Category ID")]
        public int document_category_id { get; set; }
        [StringLength(75)]
        [Required]
        [Index(IsUnique = true)]
        [Display(Name = "Code")]
        public string document_type_code { get; set; }
        [StringLength(150)]
        [Required]
        [Display(Name = "Description")]
        public string document_type_description { get; set; }
        [StringLength(1)]
        public string status { get; set; }
    }
}