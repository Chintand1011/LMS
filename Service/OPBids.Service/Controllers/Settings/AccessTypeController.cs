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
        #region Access Types
        [HttpPost]
        [Route("service/GetAccessType")]
        public Result<IEnumerable<AccessType>> GetAccessType([FromBody] SettingVM payload)
        {
            return new AccessTypesLogic().GetAccessType(payload);
        }

        [HttpPost]
        [Route("service/CreateAccessType")]
        public Result<IEnumerable<AccessType>> CreateAccessType([FromBody] AccessTypesVM accessTypesVM)
        {
            // Validate and Map to Domain model
            AccessType accessType = accessTypesVM.ToDomain();
            return new AccessTypesLogic().CreateAccessType(accessType);
        }

        [HttpPost]
        [Route("service/UpdateAccessType")]
        public Result<IEnumerable<AccessType>> UpdateAccessType([FromBody] AccessTypesVM accessTypesVM)
        {
            // Validate and Map to Domain model
            AccessType accessType = accessTypesVM.ToDomain();
            return new AccessTypesLogic().UpdateAccessType(accessType);
        }

        [HttpPost]
        [Route("service/StatusUpdateAccessType")]
        public Result<IEnumerable<AccessType>> StatusUpdateAccessType([FromBody] SettingVM payload)
        {
            return new AccessTypesLogic().StatusUpdateAccessType(payload);
        }
        #endregion#endregion
    }
}
