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
    public class ProcurementTypeController : ApiController
    {
        [HttpPost]
        [Route("service/GetProcurementType")]
        public Result<IEnumerable<ProcurementType>> GetProcurementType([FromBody] Payload payload)
        {
            return new ProcurementTypeLogic().GetProcurementType(payload);
        }

        [HttpPost]
        [Route("service/CreateProcurementType")]
        public Result<IEnumerable<ProcurementType>> CreateProcurementType([FromBody] ProcurementTypeVM procurementtypeVM)
        {
            var ProcurementType = procurementtypeVM.ToDomain();
            return new ProcurementTypeLogic().CreateProcurementType(ProcurementType);
        }

        [HttpPost]
        [Route("service/UpdateProcurementType")]
        public Result<IEnumerable<ProcurementType>> UpdateProcurementType([FromBody] ProcurementTypeVM procurementtypeVM)
        {
            // Validate and Map to Domain model
            ProcurementType ProcurementType = procurementtypeVM.ToDomain();
            return new ProcurementTypeLogic().UpdateProcurementType(ProcurementType);
        }

        [HttpPost]
        [Route("service/StatusUpdateProcurementType")]
        public Result<IEnumerable<ProcurementType>> StatusUpdateProcurementType([FromBody] Payload payload)
        {
            return new ProcurementTypeLogic().StatusUpdateProcurementType(payload);
        }
    }
}
