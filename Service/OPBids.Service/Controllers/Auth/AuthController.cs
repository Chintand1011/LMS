using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Auth;
using System.Web.Http;


namespace OPBids.Service.Controllers.Auth
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("service/Authenticate")]
        public Result<AccessUsersVM> Authenticate([FromBody] Payload payload)
        {
            return new AuthLogic().Authenticate(payload);
        }

        [HttpPost]
        [Route("service/ChangePassword")]
        public Result<AccessUsersVM> ChangePassword([FromBody] Payload payload)
        {
            return new AuthLogic().ChangePassword(payload);
        }
    }
}
