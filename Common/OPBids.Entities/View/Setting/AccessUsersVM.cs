using OPBids.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.Setting
{
    public class AccessUsersVM : BaseVM
    {
        [Display(Name = "Salutation")]
        [StringLength(20)]
        public string salutation { get; set; }
        [Display(Name ="UserName")]
        [StringLength(75)]
        public string username { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(30)]
        [DataType(DataType.EmailAddress)]
        
        public string email_address { get; set; }

        [Display(Name = "Password")]
        [StringLength(30)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Activation Code")]
        [StringLength(50)]
        public string activation_code { get; set; }
        public bool activation_flag { get; set; }
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string first_name { get; set; }
        [Display(Name = "Middle Initial")]
        [StringLength(100)]
        public string mi { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(120)]
        public string last_name { get; set; }
        [Display(Name = "VIP Access")]
        public bool vip_access { get; set; }
        [Display(Name = "PFMS Access")]
        public bool pfms_access { get; set; }
        [Display(Name = "DTS Access")]
        public bool dts_access { get; set; }
        [Required]
        [Display(Name = "Group ID")]
        public int group_id { get; set; }
        [Display(Name = "Access Group")]
        [StringLength(30)]
        public string group_code { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        public int dashboard_id { get; set; }

        public int dept_id { get; set; }
        [Display(Name = "Department")]
        public string dept_description { get; set; }
        [Display(Name = "Department Code")]
        public string dept_code { get; set; }


        [Display(Name = "Nick Name")]
        [StringLength(15)]
        public string nickname { get; set; }

        [Display(Name = "Business Email Address")]
        [StringLength(50)]
        [Required(ErrorMessage ="Business Email Address is required")]
        public string business_email_address { get; set; }


        [Display(Name = "Contact Number")]
        [StringLength(15)]
        public string contact_no { get; set; }


        [Display(Name = "Mobile Number")]
        [StringLength(15)]
        public string mobile_no { get; set; }


        [Display(Name = "Profile Link")]
        [StringLength(200)]
        public string profile_link { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string imagefileupload { get; set; }

        [Display(Name = "Address1")]
        [StringLength(250)]
        public string address1 { get; set; }

        [Display(Name = "Address2")]
        [StringLength(250)]
        public string address2 { get; set; }

        [Display(Name = "Address3")]
        [StringLength(250)]
        public string address3 { get; set; }

        [Display(Name = "Birthday")]
        public string  DOB { get; set; }

    }
}
