using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OPBids.Service.Models.Base;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectRequestHistory : BaseModel
    {
        public int project_request_id { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(1000)]
        public string description { get; set; }

        [StringLength(100)]
        public string grantee { get; set; }

        public decimal estimated_budget { get; set; }
        public decimal approved_budget { get; set; }

        public DateTime required_date { get; set; }

        [StringLength(5)]
        public string category { get; set; }

        [StringLength(5)]
        public string classification { get; set; }

        [StringLength(5)]
        public string contract_type { get; set; }

        public int security_level { get; set; }

        public int devivery_type { get; set; }

        [StringLength(20)]
        public string earmark { get; set; }

        public DateTime? earmark_date { get; set; }

        [StringLength(5)]
        public string source_fund { get; set; }

        public int batch_id { get; set; }

        [StringLength(10)]
        public string project_status { get; set; }

        [StringLength(10)]
        public string project_substatus { get; set; }

        [StringLength(1)]
        public string record_status { get; set; }

        public int sla { get; set; }

        public int current_user { get; set; }

        public string user_action { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }

        public DateTime? routed_date { get; set; }

        [StringLength(500)]
        public string change_log { get; set; }
    }
}