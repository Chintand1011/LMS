using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Report.Datasets
{
    public class ProjectBidsDS
    {
        public int id { get; set; }
        public string created_by { get; set; }
        public decimal bid_amount { get; set; }
        public int bid_bond { get; set; }
        public string bid_form { get; set; }
        public int duration { get; set; }
        public int variance { get; set; }

        //----------------
        public string bidder { get; set; }
    }
}
