using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using static OPBids.Common.Enum;

namespace OPBids.Entities.View.Setting
{
    public class SenderRecipientUserVM : BaseVM
    {
        [Display(Name = "User ID")]
        public int user_id { get; set; }
        [Display(Name = "Is System User")]
        public bool is_system_user { get; set; }
        [Display(Name = "Designation")]
        [StringLength(10)]
        public string salutation { get; set; }
        [Display(Name = "Email Address")]
        [StringLength(120)]
        [DataType(DataType.EmailAddress)]
        public string email_address { get; set; }
        [Display(Name = "Name")]
        [StringLength(221)]
        public string full_name { get { return string.Concat(first_name, " ", mi, " ", last_name); } }
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string first_name { get; set; }
        [Display(Name = "Middle Initial")]
        [StringLength(2)]
        public string mi { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(120)]
        public string last_name { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "Department ID")]
        public int department_id { get; set; }
        [Display(Name = "Department")]
        public string department_name { get; set; }
        [Display(Name = "Mobile No")]
        [StringLength(11)]
        public string mobile_no { get; set; }
        [Display(Name = "Sender")]
        public bool is_sender { get; set; }
        [Display(Name = "Recipient")]
        public bool is_recipient { get; set; }
    }
}
