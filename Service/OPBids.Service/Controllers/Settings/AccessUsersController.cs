using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public partial class SettingsController : ApiController
    {
        [HttpPost]
        [Route("service/GetAccessUser")]
        public Result<IEnumerable<AccessUsersVM>> GetAccessUser([FromBody] Payload payload)
        {
            var rslts = new AccessUserLogic().GetAccessUser(payload);
            return rslts;
        }

        [HttpPost]
        [Route("service/CreateAccessUser")]
        public Result<IEnumerable<AccessUsersVM>> CreateAccessUser([FromBody] AccessUsersVM accessUser)
        {
            return new AccessUserLogic().CreateAccessUser(accessUser);
        }

        [HttpPost]
        [Route("service/UpdateAccessUser")]
        public Result<IEnumerable<AccessUsersVM>> UpdateAccessUser([FromBody] AccessUsersVM accessUser)
        {
            return new AccessUserLogic().UpdateAccessUser(accessUser);
        }

        [HttpPost]
        [Route("service/StatusUpdateAccessUser")]
        public Result<IEnumerable<AccessUsersVM>> StatusUpdateAccessUser([FromBody] Payload payload)
        {
            return new AccessUserLogic().StatusUpdateAccessUser(payload);
        }

        [HttpPost]
        [Route("service/ResetAccessUserPassword")]
        public Result<bool> ResetAccessUserPassword([FromBody] AccessUsersVM accessUser)
        {
            return new AccessUserLogic().ResetAccessUserPassword(accessUser.email_address);
        }

        [HttpPost]
        [Route("service/GetAccessUserByUserName")]
        public AccessUsersVM GetAccessUserByUserName([FromBody] Payload payload)
        {
            return new AccessUserLogic().GetAccessUserByUserName(payload.auth_x_un);
        }


        [HttpPost]
        [Route("service/UpdateUerInfo")]
        public bool UpdateUserInfo([FromBody] AccessUsersVM model)
        {
            return new AccessUserLogic().UpdateUserInfo(model);
        }


    }
}
