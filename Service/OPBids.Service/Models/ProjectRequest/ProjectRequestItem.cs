using OPBids.Service.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectRequestItem : BaseModel
    {
       
        public int project_id { get; set; }

        [StringLength(1000)]
        public string description { get; set; }

        [StringLength(100)]
        public string unit { get; set; }

        public int quantity { get; set; }

        public decimal unit_cost { get; set; }
    }
}