using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Home
{
    public class DashboardTableResultVM
    {
        public int refNo { get; set; }
        public bool monitored { get; set; }
        public string category { get; set; }
        public int agingInDays { get; set; }
        public int agingInTWG { get; set; }
        public int agingInStatus { get; set; }
        public string project { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; }
        public DateTime dateSubmitted { get; set; }

        public string description { get; set; }
        public string created_date { get; set; }
        public string required_date { get; set; }
        public string grantee { get; set; }
        

    }
}
