using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Home
{
    public class DashboardTableHeaderVM
    {
        public string title { get; set; }
       
        public List<KeyValuePair<int, string>> completionFilter { get; set; }
        public List<KeyValuePair<int, string>> statusFilter { get; set; }
    }
}
