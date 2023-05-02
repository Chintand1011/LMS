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
    public class Delivery:BaseModel
    {
        [Required]
        [StringLength(75)]
        [Index(IsUnique = true)]
        public string delivery_code { get; set; }
        [Required]
        [StringLength(150)]
        public string delivery_description { get; set; }
        [Required]
        [StringLength(1)]
        public string status { get; set; }


    }
}