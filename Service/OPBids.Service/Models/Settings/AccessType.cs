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
    public class AccessType : BaseModel
    {
        [StringLength(25)]
        //[Required]
        //[Index(IsUnique = true)]
        public string code{ get; set; }
        [StringLength(100)]
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        public bool? view_transact_data { get; set; }
        public bool? add_edit_data { get; set; }
        public bool? delete_data { get; set; }
        public bool? record_section { get; set; }
        public int seq_no { get; set; }
        [Required]
        public int sys_id { get; set; }
        public int? parent_id { get; set; }
        [StringLength(1)]
        public string status { get; set; }
        public bool disp_menu_to_mobile { get; set; }

        [StringLength(100)]
        public string controller { get; set; }

        [StringLength(100)]
        public string css_class { get; set; }

        [StringLength(50)]
        public string icon { get; set; }

    }
}