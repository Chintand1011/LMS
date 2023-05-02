using OPBids.Entities.Common;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public class BarcodePrintingStatusController : ApiController
    {
        [HttpPost]
        [Route("service/GetBarcodePrintingStatus")]
        public Result<IEnumerable<BarcodePrintingStatus>> GetBarcodePrintingStatus([FromBody] Payload payload)
        {
            return new BarcodePrintingStatusLogic().GetBarcodePrintingStatus(payload);
        }
    }
}
