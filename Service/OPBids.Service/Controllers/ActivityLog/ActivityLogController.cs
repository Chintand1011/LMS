using OPBids.Entities.Common;
using OPBids.Entities.View.Shared;
using OPBids.Service.Logic.ActivityLog;
using System.Collections.Generic;
using System.Web.Http;


namespace OPBids.Service.Controllers.ActivityLog
{
    public class ActivityLogController : ApiController
    {
        [HttpPost]
        [Route("service/ActivityLog")]
        public IEnumerable<ActivityLogModel> GetActivityLogByUserId([FromBody] Payload payload)
        {

            return new ActivityLogLogic().GetActivityLogByUserId(payload);
        }

        [Route("service/InsertActivityLog")]
        public Result<bool> InsertActivityLog([FromBody] ActivityLogModel model)
        {

            ///dfasdf
            return new ActivityLogLogic().InsertActivityLog(model);
        } 
    }
}
