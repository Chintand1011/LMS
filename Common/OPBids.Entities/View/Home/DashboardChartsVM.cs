using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Home
{
    public class DashboardChartsVM
    {
        public ChartGroup ChartGroup1 { get; set; }
        public ChartGroup ChartGroup2 { get; set; }
    }

    public class ChartGroup
    {
        public string title { get; set; }
        public string columnDescription { get; set; }
        public List<ChartData> data { get; set; }
    }

    public class ChartData
    {
        public string name { get; set; }
        public int count { get; set; }
        public decimal amount { get; set; }
    }
}
