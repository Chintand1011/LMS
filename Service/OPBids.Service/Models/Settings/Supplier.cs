using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Service.Models.Settings
{
    public class Supplier:BaseModel
    {
        [StringLength(100)]
        [Required]
        public string contact_person { get; set; }

        [StringLength(25)]
        [Required]
        public string company_code { get; set; }

        [StringLength(100)]
        [Required]
        public string comp_name { get; set; }

        [StringLength(150)]
        [Required]
        public string address { get; set; }

        [StringLength(50)]
        [Required]
        public string email { get; set; }

        [StringLength(25)]
        [Required]
        public string contact_no { get; set; }

        [StringLength(50)]
        [Required]
        public string tin { get; set; }


       [StringLength(1)]
        public string status { get; set; }

        [Required]
        public int user_id { get; set; }

    }
}