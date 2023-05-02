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
    public class SenderRecipientUser : BaseModel
    {
        [StringLength(10)]
        public string salutation { get; set; }
        [StringLength(120)]
        [Index(IsUnique = true)]
        public string email_address { get; set; }
        [Required]
        [StringLength(100)]
        public string first_name { get; set; }
        [StringLength(2)]
        public string mi { get; set; }
        [Required]
        [StringLength(120)]
        public string last_name { get; set; }
    }
}