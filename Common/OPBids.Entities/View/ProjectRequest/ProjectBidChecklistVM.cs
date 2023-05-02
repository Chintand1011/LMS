using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectBidChecklistVM : Base.BaseVM
    {
        public int project_request_id { get; set; }

        public int bid_id { get; set; }

        [StringLength(1)]
        public string stage { get; set; }

        [StringLength(5)]
        public string checklist_id { get; set; }

        [StringLength(5)]
        public string type { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public bool? eligibility { get; set; }
    }
}
