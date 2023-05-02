using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public partial class DocumentTypeController : ApiController
    {
        [HttpPost]
        [Route("service/GetDocumentType")]
        public Result<IEnumerable<DocumentTypeVM>> GetDocumentType([FromBody] DocumentTypeVM payload)
        {
            return new DocumentTypeLogic().GetDocumentType(payload);
        }

        [HttpPost]
        [Route("service/CreateDocumentType")]
        public Result<IEnumerable<DocumentTypeVM>> CreateDocumentType([FromBody] DocumentTypeVM documentType)
        {
            // Validate and Map to Domain model
            return new DocumentTypeLogic().CreateDocumentType(documentType);
        }

        [HttpPost]
        [Route("service/UpdateDocumentType")]
        public Result<IEnumerable<DocumentTypeVM>> UpdateDocumentType([FromBody] DocumentTypeVM documentType)
        {
            // Validate and Map to Domain model
            return new DocumentTypeLogic().UpdateDocumentType(documentType);
        }

        [HttpPost]
        [Route("service/StatusUpdateDocumentType")]
        public Result<IEnumerable<DocumentTypeVM>> StatusUpdateDocumentType([FromBody] Payload payload)
        {
            return new DocumentTypeLogic().StatusUpdateDocumentType(payload);
        }
    }
}