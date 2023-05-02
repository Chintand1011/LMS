using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Setting
{
    public class ProjectGranteeVM
    {
        public int id { get; set; }
        public string grantee_code { get; set; }
        public string grantee_name { get; set; }
        public string status { get; set; }
    }
}
