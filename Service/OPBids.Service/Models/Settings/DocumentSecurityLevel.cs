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
    public class DocumentSecurityLevel : BaseModel
    {
        [StringLength(75)]
        [Required]
        [Index(IsUnique = true)]
        public string code { get; set; }

        [StringLength(150)]
        [Required]
        public string description { get; set; }
    }
}