using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.Common
{
    public class KeyValue : Base.BaseVM
    {
        public string type { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
}
