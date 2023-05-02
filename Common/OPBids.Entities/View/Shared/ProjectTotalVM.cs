using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Shared
{
   public class ProjectTotalVM
    {
        public string desc { get; set; }

        public List<ChartData> data { get; set; }

        public string total { get; set; }
    }

    public class ChartData
    {
        public int index { get; set; }
        public decimal amount { get; set; }
        public decimal sum_amount { get; set; }
    }
}
