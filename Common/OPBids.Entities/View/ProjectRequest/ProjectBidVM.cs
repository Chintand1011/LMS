using System;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectBidVM : Base.BaseVM
    {
        public int project_request_id { get; set; }
        public string project_request_title { get; set; }
        public string bid_opening_date { get; set; }
        public string bid_opening_time { get; set; }
        public string bid_opening_place { get; set; }
        public string bid_amount { get; set; }

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

        [StringLength(11)]
        public string prepared_date { get; set; }

        public bool procured_docs { get; set; }

        [StringLength(50)]
        public string eval_result { get; set; }

        [StringLength(50)]
        public string gen_eval { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }

        public string bidder_name { get; set; }
        public string bidder_address { get; set; }
        public string auth_rep { get; set; }

    }
}
