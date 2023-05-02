using OPBids.Entities.Base;

namespace OPBids.Entities.View.ProjectRequest
{
    public class MonitoredProjectVM : BaseVM
    {
        public int project_request_id { get; set; }
        public string action { get; set; }
    }
}
