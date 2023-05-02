using OPBids.Entities.View.Home;
using OPBids.Service.Logic.Home;
using OPBids.Service.Models;
using OPBids.Service.Models.Home;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;
using OPBids.Entities.Common;

namespace OPBids.Service.Controllers.Home
{
    public class DashboardController : ApiController
    {
        [HttpPost]
        [Route("service/GetSummaryHeader")]
        public Result<Entities.View.Home.DashboardSummaryVM> GetDashboardSummary([FromBody] Payload payload)
        {
            return new DashboardLogic().GetDashboardSummary(payload);
        }

        [HttpPost]
        [Route("service/GetDashboardTable")]
        public Result<IEnumerable<DashboardTableResultVM>> GetDashboardTable([FromBody] DashboardPayloadVM payload)
        {
            return new DashboardLogic().GetDashboardTable(payload);
        }
        [HttpPost]
        [Route("service/GetDashboardCharts")]
        public Result<Entities.View.Home.DashboardChartsVM> GetDashboardCharts([FromBody] Payload payload)
        {
            return new DashboardLogic().GetDashboardCharts(payload);
        }
        
        [HttpPost]
        [Route("service/GetTWGResultHeaders")]
        public Result<Entities.View.Home.TWGResultHeadersVM> GetTWGResultHeaders([FromBody] DashboardPayloadVM payload)
        {
            return new DashboardLogic().GetTWGResultHeaders(payload);
        }

       
    }
}