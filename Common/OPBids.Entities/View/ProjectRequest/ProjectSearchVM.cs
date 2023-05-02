using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectSearchVM
    {
        public string duration_scope { get; set; }

        [Display(Name = "Date Submitted From")]
        public DateTime? submitted_from { get; set; }

        [Display(Name = "Date Submitted To")]
        public DateTime? submitted_to { get; set; }

        [Display(Name = "Date Required From")]
        public DateTime? required_from { get; set; }

        [Display(Name = "Date Required To")]
        public DateTime? required_to { get; set; }


        [Display(Name = "Estimated Budget")]
        public decimal budget_min { get; set; }
        [Display(Name = "Estimated Budget")]
        public decimal budget_max { get; set; }

        [Display(Name = "Project Grantee")]
        public List<string> grantee { get; set; }

        [Display(Name = "Category")]
        public List<string> category { get; set; }

        [Display(Name = "Project Name / Draft #")]
        public string project_name { get; set; }
        public string id { get; set; }
        public string batch_id { get; set; }
        public bool no_batch_id { get; set; }
        public int applicable_year { get; set; }
        public string barcode { get; set; }
        public string bid_id { get; set; }
        public string project_status { get; set; }
        public string project_substatus { get; set; }
        public string project_substatus_min { get; set; }
        public string project_substatus_max { get; set; }
        public string record_status { get; set; }

        public string stage { get; set; }
        
        public int page_index { get; set; }
        public int page_size { get; set; }

        public int current_user { get; set; }
        public bool? isMonitored { get; set; }
        public bool? get_total { get; set; }

        public string dashboard_option { get; set; }
    }
}
