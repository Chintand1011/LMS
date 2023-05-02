using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectRequestBatchVM : Base.BaseVM
    {
        public int applicable_year { get; set; }

        [StringLength(20)]
        public string procurement_method { get; set; }
        [StringLength(200)]
        public string procurement_method_desc { get; set; }

        [StringLength(11)]
        public string pre_bid_date { get; set; }
        [StringLength(8)]
        public string pre_bid_time { get; set; }
        [StringLength(200)]
        public string pre_bid_place { get; set; }

        [StringLength(11)]
        public string bid_deadline_date { get; set; }
        [StringLength(8)]
        public string bid_deadline_time { get; set; }
        [StringLength(200)]
        public string bid_deadline_place { get; set; }

        [StringLength(11)]
        public string bid_opening_date { get; set; }
        [StringLength(8)]
        public string bid_opening_time { get; set; }
        [StringLength(200)]
        public string bid_opening_place { get; set; }
        [StringLength(1000)]
        public string bid_notes { get; set; }

        [StringLength(11)]
        public string philgeps_publish_date { get; set; }
        [StringLength(100)]
        public string philgeps_publish_by { get; set; }

        [StringLength(11)]
        public string mmda_publish_date { get; set; }
        [StringLength(100)]
        public string mmda_publish_by { get; set; }

        [StringLength(11)]
        public string conspost_date_lobby { get; set; }
        [StringLength(11)]
        public string conspost_date_reception { get; set; }
        [StringLength(11)]
        public string conspost_date_command { get; set; }
        [StringLength(100)]
        public string conspost_by { get; set; }

        [StringLength(11)]
        public string newspaper_sent_date { get; set; }
        [StringLength(200)]
        public string newspaper_publisher { get; set; }
        [StringLength(100)]
        public string newspaper_received_by { get; set; }

        [StringLength(11)]
        public string newspaper_post_date { get; set; }
        [StringLength(100)]
        public string newspaper_post_by { get; set; }

        [Display(Name = "# of Projects")]
        public int total_projects { get; set; }
        [Display(Name = "Total Amount")]
        public string total_amount { get; set; }

        [StringLength(10)]
        public string project_status { get; set; }
        public string project_status_desc { get; set; }

        [StringLength(10)]
        public string project_substatus { get; set; }
        public string project_substatus_desc { get; set; }

        [StringLength(1)]
        public string record_status { get; set; }

        public int sla { get; set; }

        public int current_user { get; set; }

        [StringLength(200)]
        public string user_action { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }

        [StringLength(11)]
        public string routed_date { get; set; }

        public int index { get; set; }

        public string procurement_mode { get; set; }

        public string philgeps_att { get; set; }

        public string mmda_portal_att { get; set; }

        public string conspost_lobby_att { get; set; }

        public string conspost_reception_att { get; set; }

        public string conspost_command_att { get; set; }

        public string newspaper_att { get; set; }

    }
}
