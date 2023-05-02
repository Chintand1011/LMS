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
    public partial class SettingsController : ApiController
    {
        [HttpPost]
        [Route("service/GetAccessGroup")]
        public Result<IEnumerable<AccessGroup>> GetAccessGroup([FromBody] Payload payload)
        {
            return new AccessGroupLogic().GetAccessGroup(payload);
        }

        [HttpPost]
        [Route("service/CreateAccessGroup")]
        public Result<IEnumerable<AccessGroup>> CreateAccessGroup([FromBody] AccessGroupVM accessGroupVM)
        {
            // Validate and Map to Domain model
            var AccessGroup = accessGroupVM.ToDomain();
            return new AccessGroupLogic().CreateAccessGroup(AccessGroup);
        }

        [HttpPost]
        [Route("service/UpdateAccessGroup")]
        public Result<IEnumerable<AccessGroup>> UpdateAccessGroup([FromBody] AccessGroupVM accessGroupVM)
        {
            // Validate and Map to Domain model
            AccessGroup AccessGroup = accessGroupVM.ToDomain();
            return new AccessGroupLogic().UpdateAccessGroup(AccessGroup);
        }

        [HttpPost]
        [Route("service/StatusUpdateAccessGroup")]
        public Result<IEnumerable<AccessGroup>> StatusUpdateAccessGroup([FromBody] Payload payload)
        {
            return new AccessGroupLogic().StatusUpdateAccessGroup(payload);
        }
    }
}
