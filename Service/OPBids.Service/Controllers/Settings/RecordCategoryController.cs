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
    public class RecordCategoryController : ApiController
    {
        [HttpPost]
        [Route("service/GetRecordCategory")]
        public Result<IEnumerable<RecordCategory>> GetRecordCategory([FromBody] Payload payload)
        {
            return new RecordCategoryLogic().GetRecordCategory(payload);
        }

        [HttpPost]
        [Route("service/CreateRecordCategory")]
        public Result<IEnumerable<RecordCategory>> CreateRecordCategory([FromBody] RecordCategoryVM CategoryVM)
        {
            var RecordCategory = CategoryVM.ToDomain();
            return new RecordCategoryLogic().CreateRecordCategory(RecordCategory);
        }

        [HttpPost]
        [Route("service/UpdateRecordCategory")]
        public Result<IEnumerable<RecordCategory>> UpdateRecordCategory([FromBody] RecordCategoryVM CategoryVM)
        {
            // Validate and Map to Domain model
            var RecordCategory = CategoryVM.ToDomain();
            return new RecordCategoryLogic().UpdateRecordCategory(RecordCategory);
        }
    }
}
