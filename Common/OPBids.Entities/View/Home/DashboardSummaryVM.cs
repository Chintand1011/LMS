using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
namespace OPBids.Entities.View.Home
{
    public class DashboardSummaryVM : BaseVM
    {
        public SummaryInfo summary1 { get; set; }
        public SummaryInfo summary2 { get; set; }
        public SummaryInfo summary3 { get; set; }
    }
}
