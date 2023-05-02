using OPBids.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectRequestItemVM: BaseVM
    {
        [Display(Name = "Process")]
        public string process { get; set; }
        
        [Display(Name = "Id")]
        public override int id { get; set; }
        
        [Display(Name = "Unit")]
        [StringLength(10)]
        public string unit { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Item Description")]
        [StringLength(150)]
        public string description { get; set; }
        
        [Display(Name = "Quantity")]
        public string quantity{ get; set; }

        [Display(Name = "Unit Cost")]
        public string unit_cost{ get; set; }

        [Display(Name = "Project ID")]
        public int project_id { get; set; }

    }
}
