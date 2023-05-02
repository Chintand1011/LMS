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
    public class SenderRecipient : BaseModel
    {
        public int user_id { get; set; }
        public bool is_system_user { get; set; }
        [StringLength(1)]
        public string status { get; set; }
        public int department_id { get; set; }
        public bool is_sender { get; set; }
        public bool is_recipient { get; set; }
        [StringLength(11)]
        public string mobile_no { get; set; }
    }
}