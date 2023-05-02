using OPBids.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.Supplier
{
    public class SuppliersVM : BaseVM
    {
        [Display(Name = "Ref #")]
        public int ref_no { get; set; }

        [Display(Name = "Category")]
        public string category { get; set; }

        [Display(Name = "Date Submitted")]
        public string date_submitted { get; set; }

        [Display(Name = "Deadline")]
        public string deadline { get; set; }

        [Display(Name = "Project")]
        public string project { get; set; }

        [Display(Name = "Project Description")]
        public string project_desc { get; set; }

        [Display(Name = "Amount")]
        public string amount { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Approved Budget")]
        public string approved_budget { get; set; }

        [Display(Name = "Notes")]
        public string notes { get; set; }

        [Display(Name = "Project Duration")]
        public int project_duration { get; set; }

        [Display(Name = "Bid Bond")]
        public int bid_bond { get; set; }

        public int index { get; set; }

    }
}
