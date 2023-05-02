using System.ComponentModel.DataAnnotations;

namespace OPBids.Service.Models.ProjectRequest
{
    public class ProjectBidChecklist : Base.BaseModel
    {
        public int project_request_id { get; set; }

        public int bid_id { get; set; }

        [StringLength(1)]
        public string stage { get; set; }

        [StringLength(10)]
        public string checklist_id { get; set; }

        [StringLength(5)]
        public string type { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        public bool? eligibility { get; set; }
    }
}