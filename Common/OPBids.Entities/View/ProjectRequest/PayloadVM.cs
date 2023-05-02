using OPBids.Entities.Base;
using OPBids.Entities.View.Shared;
using System.Collections.Generic;

namespace OPBids.Entities.View.ProjectRequest
{
    public class PayloadVM : BaseVM
    {
        public string sub_menu_id { get; set; }
        public string txn { get; set; }
        public string status { get; set; }
        public string[] item_list { get; set; }

        public ProjectRequestVM projectRequest { get; set; }
        public ProjectRequestBatchVM projectRequestBatch { get; set; }
        public ProjectSearchVM projectSearch { get; set; }
        public ProjectRequestAdvertisementVM projectAdvertisement { get; set; }
        public List<ProjectBidVM> projectBids { get; set; }
        public ProjectBidVM awardedBid { get; set; }
        public List<ProjectRequestAttachmentVM> documentAttachments { get; set; }
        public List<ProjectBidChecklistVM> projectBidChecklists { get; set; }
        public List<ProjectRequestItemVM> projectItems { get; set; }
        public List<ProjectRequestHistoryVM> projectRequestHistories { get; set; }
        public MonitoredProjectVM monitoredProject { get; set; }
        public List<ProgressVM> progressList { get; set; }
    }
}
