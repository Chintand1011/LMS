using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Report.Datasets
{
    public class ProjectRequestBatchDS
    {
        public int id { get; set; }


        public string philgeps_publish_by { get; set; }
        public DateTime? philgeps_publish_date { get; set; }

        //mmda portal
        public string mmda_publish_by { get; set; }
        public DateTime? mmda_publish_date { get; set; }

        //conspicuous place
        public DateTime? conspost_date_lobby { get; set; }
        public DateTime? conspost_date_reception { get; set; }//redemption
        public DateTime? conspost_date_command { get; set; }
        public string conspost_by { get; set; }

        //nespaper
        public string newspaper_publisher { get; set; }
        public DateTime? newspaper_sent_date { get; set; }
        public string newspaper_receive_by { get; set; }
        public DateTime? newspaper_post_date { get; set; }//date published
        public string newspaper_post_by { get; set; }


        public DateTime? pre_bid_date { get; set; }
        public string pre_bid_place { get; set; }
        public DateTime? bid_deadline_date { get; set; }
        public string bid_deadine_place { get; set; }
        public DateTime? bid_opening_date { get; set; }
        public string bid_opening_place { get; set; }
    }
}
