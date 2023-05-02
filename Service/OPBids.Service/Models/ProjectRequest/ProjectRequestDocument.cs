using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectRequestDocument : Base.BaseModel
    {
        public int project_request_id { get; set; }
        public string file_name { get; set; }
        public string attachment_name { get; set; }
        public string barcode { get; set; }
        public string status { get; set; }
        public string created_stage { get; set; }
        public string updated_stage { get; set; }
    }
}