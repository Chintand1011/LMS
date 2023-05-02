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
    public class ProjectAreasDistrictVM : BaseVM
    {
        [Display(Name = "City ID")]        
        public int city_id { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string city_name { get; set; }

        [Display(Name = "District")]
        [StringLength(50)]
        public string district_name { get; set; }
        
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        
    }
}
