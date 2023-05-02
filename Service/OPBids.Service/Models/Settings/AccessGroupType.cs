using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Service.Models.Settings
{
    public class AccessGroupType : BaseModel
    {
        public int access_type_id { get; set; }
        public int access_group_id { get; set; }
        public bool? view_transact_data { get; set; }
        public bool? add_edit_data { get; set; }
        public bool? delete_data { get; set; }
        public bool? record_section { get; set; }
    }
}