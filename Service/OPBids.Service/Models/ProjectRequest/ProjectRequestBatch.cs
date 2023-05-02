using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectRequestBatch : Base.BaseModel
    {
        public int applicable_year { get; set; }

        [StringLength(20)]
        public string procurement_method { get; set; }

        public DateTime? pre_bid_date { get; set; }
        [StringLength(200)]
        public string pre_bid_place { get; set; }

        public DateTime? bid_deadline_date { get; set; }
        [StringLength(200)]
        public string bid_deadline_place { get; set; }

        public DateTime? bid_opening_date { get; set; }
        [StringLength(200)]
        public string bid_opening_place { get; set; }
        [StringLength(1000)]
        public string bid_notes { get; set; }

        public DateTime? philgeps_publish_date { get; set; }
        [StringLength(100)]
        public string philgeps_publish_by { get; set; }

        public DateTime? mmda_publish_date { get; set; }
        [StringLength(100)]
        public string mmda_publish_by { get; set; }

        public DateTime? conspost_date_lobby { get; set; }
        public DateTime? conspost_date_reception { get; set; }
        public DateTime? conspost_date_command { get; set; }
        [StringLength(100)]
        public string conspost_by { get; set; }

        public DateTime? newspaper_sent_date { get; set; }
        [StringLength(200)]
        public string newspaper_publisher { get; set; }
        [StringLength(200)]
        public string newspaper_received_by { get; set; }

        public DateTime? newspaper_post_date { get; set; }
        [StringLength(100)]
        public string newspaper_post_by { get; set; }



        [StringLength(10)]
        public string project_status { get; set; }

        [StringLength(10)]
        public string project_substatus { get; set; }

        [StringLength(1)]
        public string record_status { get; set; }

        public int sla { get; set; }

        public int current_user { get; set; }

        [StringLength(200)]
        public string user_action { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }

        public DateTime? routed_date { get; set; }

        [StringLength(150)]
        public string philgeps_att { get; set; }

        [StringLength(150)]
        public string mmda_portal_att { get; set; }

        [StringLength(150)]
        public string conspost_lobby_att { get; set; }

        [StringLength(150)]
        public string conspost_reception_att { get; set; }

        [StringLength(150)]
        public string conspost_command_att { get; set; }

        [StringLength(150)]
        public string newspaper_att{ get; set; }
    }
}
