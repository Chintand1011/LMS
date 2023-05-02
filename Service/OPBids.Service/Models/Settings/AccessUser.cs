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
    public class AccessUser : BaseModel
    {
        [StringLength(10)]
        public string salutation { get; set; }
        [StringLength(75)]
        [Required]
        public string username { get; set; }
        [StringLength(120)]
        [Required]
        //[Index(IsUnique = true)]
        public string email_address { get; set; }
        [StringLength(30)]
        public string password { get; set; }
        [StringLength(50)]
        public string activation_code { get; set; }
        public bool activation_flag { get; set; }
        [StringLength(150)]
        [Required]
        public string first_name { get; set; }
        public string mi { get; set; }
        public string last_name { get; set; }
        public bool vip_access { get; set; }
        public bool pfms_access { get; set; }
        public bool dts_access { get; set; }
        [StringLength(1)]
        [Required]
        public string status { get; set; }
        [Required]
        public int group_id { get; set; }

        [Required]
        public int dept_id { get; set; }

        [StringLength(15)]
        public string nickname { get; set; }


        [StringLength(120)]
        public string business_email_address { get; set; }

        [StringLength(50)]
        public string mobile_no { get; set; }

        [StringLength(50)]
        public string contact_no { get; set; }


        [StringLength(250)]
        public string profile_link { get; set; }

        [StringLength(250)]
        public string address1 { get; set; }


        [StringLength(250)]
        public string address2 { get; set; }


        [StringLength(250)]
        public string address3 { get; set; }


        public DateTime DOB { get; set; }
    }

}