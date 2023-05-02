using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectRequestHistoryVM : Base.BaseVM
    {
        public int project_request_id { get; set; }       

        public string user_action { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }        

        [StringLength(500)]
        public string change_log { get; set; }

        public string department_desc { get; set; }
        public string project_status_desc { get; set; }

        public string project_substatus_desc { get; set; }
    }
}
