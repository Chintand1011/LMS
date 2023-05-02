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
    public class AccessGroup : BaseModel
    {
        [Required]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string group_code { get; set; }
        [Required]
        [StringLength(100)]
        public string group_description { get; set; }
        [Required]
        [StringLength(1)]
        public string status { get; set; }

       public int dashboard_id { get; set; }
    }
}