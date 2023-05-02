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
    public class ProjectAreas:BaseModel
    {        
        public int city_id { get; set; }
                
        public int district_id { get; set; }
                
        public int barangay_id { get; set; }
               
        [StringLength(1)]
        public string status { get; set; }

    }
}