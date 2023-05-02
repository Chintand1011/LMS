using OPBids.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPBids.Entities.View.Shared
{
    public class UserNotificationVM : BaseVM
    {
        [Display(Name = "IDs")]
        public string ids { get; set; }
        [Display(Name = "Process")]
        public string process { get; set; }
        [Display(Name = "Message")]
        public string message { get; set; }
        [Display(Name = "Is Read")]
        public bool? is_read { get; set; }
        [Display(Name = "Is Starred")]
        public bool? is_starred { get; set; }
        [Display(Name = "Is Hidden")]
        public bool is_hidden { get; set; }
        [Display(Name = "Recipient Ids")]
        public string recipient_ids { get; set; }
        [Display(Name = "Department Ids")]
        public string department_ids { get; set; }
        [Display(Name = "Recipient Names")]
        public string recipient_names { get; set; }
        [Display(Name = "Sender Id")]
        public int sender_id { get; set; }
        [Display(Name = "Sender Name")]
        public string sender_name { get; set; }
        [Display(Name = "Date Sent")]
        public string date_sent { get; set; }
    }
}