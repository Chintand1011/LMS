using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.Supplier
{
    public class SupplierSearchVM
    {
        [Display(Name = "Date Submitted From")]
        public DateTime? submitted_from { get; set; }

        [Display(Name = "Date Submitted To")]
        public DateTime? submitted_to { get; set; }

        [Display(Name = "Date Required From")]
        [StringLength(11)]
        public DateTime? required_from { get; set; }

        [Display(Name = "Date Required To")]
        [StringLength(11)]
        public DateTime? required_to { get; set; }


        [Display(Name = "Estimated Budget")]
        public int budget_min { get; set; }
        [Display(Name = "Estimated Budget")]
        public int budget_max { get; set; }

        [Display(Name = "Project Grantee")]
        public List<string> grantee { get; set; }

        [Display(Name = "Category")]
        public List<string> category { get; set; }

        [Display(Name = "Project Name")]
        public string project_name { get; set; }

        [Display(Name = "Ref #")]
        public int RefNo { get; set; }
    }
}
