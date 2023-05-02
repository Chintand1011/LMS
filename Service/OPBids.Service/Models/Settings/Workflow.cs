using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OPBids.Service.Models.Base;

namespace OPBids.Service.Models.Settings
{
    public class Workflow : BaseModel
    {
        [Required]
        [StringLength(5)]
        public string type { get; set; }

        [Required]
        public int seq_no { get; set; }

        [Required]
        [StringLength(10)]
        public string project_status { get; set; }

        [Required]
        [StringLength(10)]
        public string project_substatus { get; set; }

        [Required]
        [StringLength(500)]
        public string project_status_desc { get; set; }

        [Required]
        [StringLength(500)]
        public string project_substatus_desc { get; set; }

        [Required]
        public int sla { get; set; }

        [Required]
        [StringLength(10)]
        public string access_group { get; set; }

        [Required]
        [StringLength(1)]
        public string record_status { get; set; }

    }
}