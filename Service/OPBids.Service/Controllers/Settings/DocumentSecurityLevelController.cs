using OPBids.Entities.Common;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public class DocumentSecurityLevelController : ApiController
    {
        [HttpPost]
        [Route("service/GetDocumentSecurityLevel")]
        public Result<IEnumerable<DocumentSecurityLevel>> GetDocumentSecurityLevel([FromBody] Payload payload)
        {
            return new DocumentSecurityLevelLogic().GetDocumentSecurityLevel(payload);
        }
    }
}
