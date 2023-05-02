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
  public class DashboardConfig:BaseModel
    {

        [Required]
        [Index(IsUnique = true)]
        public int dashboard_id { get; set; }

        [StringLength(100)]
        public string dashboard_desc { get; set; }

        [StringLength(1)]
        public string status { get; set; }

    }
}
