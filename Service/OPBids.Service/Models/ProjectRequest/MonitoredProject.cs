using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.ProjectRequest
{
    public class MonitoredProject : Base.BaseModel
    {
        public int project_request_id { get; set; }
    }
}