using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Shared
{
    public class ActivityLogModel
    {
        public string RecordId { get; set; }

        public string UserId { get; set; }


        public string Module { get; set; }

        public string IPAddress { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Activities { get; set; }

        public string Action { get; set; }

        public string Type { get; set; }

        public DateTime DateTime { get; set; }
    }
}
