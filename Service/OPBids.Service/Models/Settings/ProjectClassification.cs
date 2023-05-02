using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.Settings
{
    public class ProjectClassification : Base.BaseModel
    {
        [Required]
        [StringLength(20)]
        [Index(IsUnique = true)]
        public string classification { get; set; }

        [Required]
        [StringLength(200)]
        public string description { get; set; }

        [Required]
        [StringLength(1)]
        public string status { get; set; }
    }
}