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
    public class ProjectAreasBarangay:BaseModel
    {
        [Display(Name = "City ID")]
        [Required]
        public int city_id { get; set; }
               
        [Display(Name = "District ID")]
        [Required]
        public int district_id { get; set; }
        
        [Display(Name = "Barangay")]
        [StringLength(50)]
        [Required]
        public string barangay_name { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}