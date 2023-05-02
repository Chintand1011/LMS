using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public partial class SettingsController : ApiController
    {
        [HttpPost]
        [Route("service/GetDocumentCategory")]
        public Result<IEnumerable<DocumentCategory>> GetDocumentCategory([FromBody] DocumentCategoryVM payload)
        {
            return new DocumentCategoryLogic().GetDocumentCategory(payload);
        }

        [HttpPost]
        [Route("service/CreateDocumentCategory")]
        public Result<IEnumerable<DocumentCategory>> CreateDocumentCategory([FromBody] DocumentCategoryVM DocumentCategoryVM)
        {
            // Validate and Map to Domain model
            var DocumentCategory = DocumentCategoryVM.ToDomain();
            return new DocumentCategoryLogic().CreateDocumentCategory(DocumentCategory);
        }

        [HttpPost]
        [Route("service/UpdateDocumentCategory")]
        public Result<IEnumerable<DocumentCategory>> UpdateDocumentCategory([FromBody] DocumentCategoryVM DocumentCategoryVM)
        {
            // Validate and Map to Domain model
            DocumentCategory DocumentCategory = DocumentCategoryVM.ToDomain();
            return new DocumentCategoryLogic().UpdateDocumentCategory(DocumentCategoryVM);
        }

        [HttpPost]
        [Route("service/StatusUpdateDocumentCategory")]
        public Result<IEnumerable<DocumentCategory>> StatusUpdateDocumentCategory([FromBody] DocumentCategoryVM payload)
        {
            return new DocumentCategoryLogic().StatusUpdateDocumentCategory(payload);
        }
    }
}