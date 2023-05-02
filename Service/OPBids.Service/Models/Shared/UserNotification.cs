using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPBids.Service.Models.Shared
{
    public class UserNotification
    {
        public int id { get; set; }
        [Display(Name = "Message")]
        public string message { get; set; }
        [Display(Name = "Is Read")]
        public bool is_read { get; set; }
        [Display(Name = "Is Starred")]
        public bool is_starred { get; set; }
        [Display(Name = "Is Hidden")]
        public bool is_hidden { get; set; }
        [Display(Name = "Recipient Ids")]
        public string recipient_ids { get; set; }
        [Display(Name = "Sender Id")]
        public int sender_id { get; set; }
        [Display(Name = "Date Sent")]
        public DateTime date_sent { get; set; }
    }
}