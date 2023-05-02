using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Home
{
    public class DashboardTableDataVM
    {
        public DashboardTableHeaderVM header { get; set; }
        public List<DashboardTableResultVM> result { get; set; }
    }
}
