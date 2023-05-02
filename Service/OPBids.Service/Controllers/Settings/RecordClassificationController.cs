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
    public class RecordClassificationController : ApiController
    {
        [HttpPost]
        [Route("service/GetRecordClassification")]
        public Result<IEnumerable<RecordClassification>> GetRecordClassification([FromBody] Payload payload)
        {
            return new RecordClassificationLogic().GetRecordClassification(payload);
        }

        [HttpPost]
        [Route("service/CreateRecordClassification")]
        public Result<IEnumerable<RecordClassification>> CreateRecordClassification([FromBody] RecordClassificationVM ClassificationVM)
        {
            var RecordClassification = ClassificationVM.ToDomain();
            return new RecordClassificationLogic().CreateRecordClassification(RecordClassification);
        }

        [HttpPost]
        [Route("service/UpdateRecordClassification")]
        public Result<IEnumerable<RecordClassification>> UpdateRecordClassification([FromBody] RecordClassificationVM ClassificationVM)
        {
            // Validate and Map to Domain model
            RecordClassification RecordClassification = ClassificationVM.ToDomain();
            return new RecordClassificationLogic().UpdateRecordClassification(RecordClassification);
        }
    }
}
