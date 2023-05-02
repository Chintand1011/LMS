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
    public partial class AccessGroupTypeController : ApiController
    {
        #region Access Group Type
        [HttpPost]
        [Route("service/GetAccessGroupType")]
        public Result<IEnumerable<AccessGroupTypeVM>> GetAccessGroupType([FromBody] Payload payload)
        {
            return new AccessGroupTypeLogic().GetAccessGroupType(payload);
        }
        [HttpPost]
        [Route("service/GetAccessGroupMenu")]
        public Result<IEnumerable<AccessGroupTypeVM>> GetAccessGroupMenu([FromBody] Payload payload)
        {
            return new AccessGroupTypeLogic().GetAccessGroupMenu(payload);
        }
        [HttpPost]
        [Route("service/SaveAccessGroupType")]
        public Result<IEnumerable<AccessGroupTypeVM>> SaveAccessGroupType([FromBody] AccessGroupTypeVM[] accessGroupTypeVM)
        {
            return new AccessGroupTypeLogic().SaveAccessGroupType(accessGroupTypeVM);
        }
        #endregion
    }
}
