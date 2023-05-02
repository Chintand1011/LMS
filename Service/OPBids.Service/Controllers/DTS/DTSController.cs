using OPBids.Entities.View.DTS;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.DTS;
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
        [Route("service/MaintainOnHandDocuments")]
        public Result<IEnumerable<DocumentsVM>> MaintainOnHandDocuments([FromBody] DocumentsPayload param)
        {
            return new OnHandDocumentsLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainRequestBarcode")]
        public Result<IEnumerable<RequestBarcodeVM>> MaintainRequestBarcode([FromBody] DocumentsPayload param)
        {
            return new RequestBarcodeLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainReceivedDocuments")]
        public Result<IEnumerable<DocumentsVM>> MaintainReceivedDocuments([FromBody] DocumentsPayload param)
        {
            return new ReceivedDocumentsLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainFinalizedDocuments")]
        public Result<IEnumerable<DocumentsVM>> MaintainFinalizedDocuments([FromBody] DocumentsPayload param)
        {
            return new FinalizedDocumentsLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainArchivedDocuments")]
        public Result<IEnumerable<DocumentsVM>> MaintainArchivedDocuments([FromBody] DocumentsPayload param)
        {
            return new ArchivedDocumentsLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainDocumentAttachments")]
        public Result<IEnumerable<DocumentAttachmentVM>> MaintainDocumentAttachments([FromBody] DocumentsPayload param)
        {
            return new DocumentAttachmentLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainDocumentLogs")]
        public Result<IEnumerable<DocumentLogsVM>> MaintainDocumentLogs([FromBody] DocumentsPayload param)
        {
            return new DocumentLogsLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/MaintainDocumentRoutes")]
        public Result<IEnumerable<DocumentRoutesVM>> MaintainDocumentRoutes([FromBody] DocumentsPayload param)
        {
            return new DocumentRoutesLogic().MaintainData(param);
        }
        [HttpPost]
        [Route("service/DTSDashboard")]
        public Result<IEnumerable<DocumentsVM>> DTSDashboard([FromBody] DocumentsPayload param)
        {
            return new DTSDashboardLogic().MaintainData(param);
        }
        
    }
}
