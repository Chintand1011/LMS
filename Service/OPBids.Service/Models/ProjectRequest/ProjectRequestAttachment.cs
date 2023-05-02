using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectRequestAttachment : Base.BaseModel
    {
        public int project_id { get; set; }
        public string file_name { get; set; }
        public string attachment_name { get; set; }
        public string barcode_no { get; set; }
        public int batch_id { get; set; }
        public string status { get; set; }
    }
}