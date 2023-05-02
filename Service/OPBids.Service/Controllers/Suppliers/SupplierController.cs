using OPBids.Entities.View.DTS;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.DTS;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;
using OPBids.Entities.Common;
using OPBids.Service.Logic;
using OPBids.Entities.View.Supplier;
using OPBids.Entities.View.Suppliers;
using OPBids.Service.Logic.Suppliers;
using OPBids.Entities.View.ProjectRequest;

namespace OPBids.Service.Controllers.Suppliers
{
    public partial class SupplierController : ApiController
    {

        [HttpPost]
        [Route("service/MaintainOpenForBidding")]
        public Result<IEnumerable<SuppliersVM>> MaintainOpenForBidding([FromBody] SupplierPayloadVM payload)
        {
            return new OpenForBiddingLogic().MaintainData(payload);
        }

        [HttpPost]
        [Route("service/MaintainWithSubmittedBids")]
        public Result<IEnumerable<SuppliersVM>> MaintainWithSubmittedBids([FromBody] SupplierPayloadVM payload)
        {
            return new WithSubmittedBidsLogic().MaintainData(payload);
        }

        [HttpPost]
        [Route("service/MaintainLostOpportunities")]
        public Result<IEnumerable<SuppliersVM>> MaintainLostOpportunities([FromBody] SupplierPayloadVM payload)
        {
            return new LostOpportunitiesLogic().MaintainData(payload);
        }

        [HttpPost]
        [Route("service/MaintainProjectAttachments")]
        public Result<IEnumerable<ProjectRequestAttachmentVM>> MaintainProjectAttachments([FromBody] SupplierPayloadVM param)
        {
            return new ProjectAttachmentLogic().MaintainData(param);
        }
       
    }
}
