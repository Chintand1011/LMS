using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectBid : Base.BaseModel
    {
        public int project_request_id { get; set; }

        public decimal bid_amount { get; set; }

        public int bid_bond { get; set; }

        [StringLength(50)]
        public string bid_form { get; set; }

        public int duration { get; set; }

        public int variance { get; set; }

        [StringLength(2)]
        public string bid_status { get; set; }

        [StringLength(1)]
        public string record_status { get; set; }

        [StringLength(200)]
        public string prepared_by { get; set; }

        public DateTime? prepared_date { get; set; }

        public bool procured_docs { get; set; }

        [StringLength(50)]
        public string eval_result { get; set; }

        [StringLength(50)]
        public string gen_eval { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }

    }
}