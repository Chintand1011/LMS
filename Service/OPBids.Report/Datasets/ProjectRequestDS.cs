using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Report.Datasets
{
    public class ProjectRequestDS
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string grantee { get; set; }
        public DateTime? required_date { get; set; }
        public decimal estimated_budget { get; set; }
        public decimal approved_budget { get; set; }
        public int pr_number { get; set; }
        public int batch_id { get; set; }
        public string source_fund { get; set; }
        public DateTime? start_date { get; set; }
    }
}
