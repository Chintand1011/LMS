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
    public class SourceFundsController : ApiController
    {
        [HttpPost]
        [Route("service/GetSourceFunds")]
        public Result<IEnumerable<SourceFunds>> GetSourceFunds([FromBody] Payload payload)
        {
            return new SourceFundsLogic().GetSourceFunds(payload);
        }

        [HttpPost]
        [Route("service/CreateSourceFunds")]
        public Result<IEnumerable<SourceFunds>> CreateSourceFunds([FromBody] SourceFundsVM SourceFundsVM)
        {
            var SourceFunds = SourceFundsVM.ToDomain();
            return new SourceFundsLogic().CreateSourceFunds(SourceFunds);
        }

        [HttpPost]
        [Route("service/UpdateSourceFunds")]
        public Result<IEnumerable<SourceFunds>> UpdateSupplier([FromBody] SourceFundsVM SourceFundsVM)
        {
            // Validate and Map to Domain model
            SourceFunds SourceFunds = SourceFundsVM.ToDomain();
            return new SourceFundsLogic().UpdateSourceFunds(SourceFunds);
        }

        [HttpPost]
        [Route("service/StatusUpdateSourceFunds")]
        public Result<IEnumerable<SourceFunds>> StatusUpdateSourceFunds([FromBody] Payload payload)
        {
            return new SourceFundsLogic().StatusUpdateSourceFunds(payload);
        }
    }
}