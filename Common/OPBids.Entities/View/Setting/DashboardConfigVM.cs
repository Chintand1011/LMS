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
    public class DashboardConfigVM : BaseVM
    {
        [Display(Name = "Dashboard id")]
        [Required]
        public int dashboard_id { get; set; }
        
        [Display(Name = "Dashboard Desc")]
        [StringLength(75)]
        public string dashboard_desc { get; set; }
        
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        
    }
}
