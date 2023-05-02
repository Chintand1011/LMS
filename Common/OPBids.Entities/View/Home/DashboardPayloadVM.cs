using OPBids.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Home
{
    public class DashboardPayloadVM:BaseVM
    {
        public string[] completion { get; set; }
        public string status { get; set; }
        public bool view_all { get; set; }
    }
}
