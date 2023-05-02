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
    public class AccessGroupVM : BaseVM
    {
        [Display(Name = "Code")]
        [StringLength(30)]
        [Required]
         public string group_code { get; set; }


        [Display(Name = "Description")]
        [StringLength(100)]
        [Required]
        public string group_description { get; set; }
        [Display(Name = "Status")]

        [StringLength(1)]
        public string status { get; set; }

        [Display(Name = "DashBoard ")]
        public int dashboard_id { get; set; }
    }
}

