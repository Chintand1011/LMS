using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.Home
{
    public class TWGResultHeadersVM
    {
        public TWGHeaderItem forEvaluation { get; set; }
        public TWGHeaderItem forDocumentSubmission { get; set; }
        public TWGHeaderItem forPostQualification { get; set; }

        public class TWGHeaderItem
        {
            public string amount { get; set; }
            public int count { get; set; }
        }
    }

}
