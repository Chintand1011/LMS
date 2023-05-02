using System;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectRequestAdvertisement : Base.BaseModel
    {

        public int project_request_id { get; set; }

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

    }
}