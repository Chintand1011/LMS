using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Report.Datasets
{
    public class ProjectRequestItemsDS
    {
        public int id { get; set; }
        public int project_id { get; set; }
        public string description { get; set; }
        public string unit { get; set; }
        public int quantity{ get; set; }
        public decimal unit_cost { get; set; }
    }
}
