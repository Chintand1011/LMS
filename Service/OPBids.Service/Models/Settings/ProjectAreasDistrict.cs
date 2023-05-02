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
    public class ProjectAreasDistrict:BaseModel
    {
        [Display(Name = "City ID")]
        [Required]
        public int city_id { get; set; }
        
        [Display(Name = "District")]
        [StringLength(50)]
        [Required]
        public string district_name { get; set; }        

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}