using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public class ProcurementMethodController : ApiController
    {
        [HttpPost]
        [Route("service/GetProcurementMethod")]
        public Result<IEnumerable<ProcurementMethod>> GetProcurementMethod([FromBody] Payload payload)
        {
            return new ProcurementMethodLogic().GetProcurementMethod(payload);
        }

        [HttpPost]
        [Route("service/CreateProcurementMethod")]
        public Result<IEnumerable<ProcurementMethod>> CreateProcurementMethod([FromBody] ProcurementMethodVM ProcurementMethodVM)
        {
            // Validate and Map to Domain model
            var ProcurementMethod = ProcurementMethodVM.ToDomain();
            return new ProcurementMethodLogic().CreateProcurementMethod(ProcurementMethod);
        }

        [HttpPost]
        [Route("service/UpdateProcurementMethod")]
        public Result<IEnumerable<ProcurementMethod>> UpdateProcurementMethod([FromBody] ProcurementMethodVM ProcurementMethodVM)
        {
            // Validate and Map to Domain model
            ProcurementMethod ProcurementMethod = ProcurementMethodVM.ToDomain();
            return new ProcurementMethodLogic().UpdateProcurementMethod(ProcurementMethod);
        }

        [HttpPost]
        [Route("service/StatusUpdateProcurementMethod")]
        public Result<IEnumerable<ProcurementMethod>> StatusUpdateProcurementMethod([FromBody] Payload payload)
        {
            return new ProcurementMethodLogic().StatusUpdateProcurementMethod(payload);
        }
    }
}
