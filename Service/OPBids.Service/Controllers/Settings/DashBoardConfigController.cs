using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;
using OPBids.Entities.Common;

namespace OPBids.Service.Controllers.Settings
{
    public class DashBoardConfigController : ApiController
    {
        [HttpPost]
        [Route("service/GetDashBoardConfig")]
        public Result<IEnumerable<DashboardConfig>> GetDashboardConfig([FromBody] Payload payload)
        {
            return new DashBoardConfigLogic().GetDashBoardConfig(payload);
        }

    }
}