using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Shared;
using OPBids.Service.Logic.Shared;
using OPBids.Service.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Shared
{
    public class SharedController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectTotal")]
        public Result<ProjectTotalVM> GetProjectTotal([FromBody] Payload payload)
        {
            return new SharedLogic().GetProjectTotal(payload);
        }
        [HttpPost]
        [Route("service/GetDocumentTotal")]
        public Result<ProjectTotalVM> GetDocumentTotal([FromBody] Payload payload)
        {
            return new SharedLogic().GetDocumentTotal(payload);
        }

        [HttpPost]
        [Route("service/GetProjectProgress")]
        public Result<List<ProgressVM>> GetProjectProgress([FromBody] PayloadVM payload) {
            return new SharedLogic().GetProjectProgress(payload);
        }

        [HttpPost]
        [Route("service/GetSettingsList")]
        public Result<List<KeyValue>> GetSettingsList([FromBody] Payload payload) {
            return new SharedLogic().GetSettingsList(payload);
        }

        [HttpPost]
        [Route("service/MaintainUserAnnouncement")]
        public Result<IEnumerable<UserAnnouncementVM>> MaintainUserAnnouncement([FromBody] DocumentsPayload payload)
        {
            return new UserAnnouncementLogic().MaintainData(payload);
        }
        [HttpPost]
        [Route("service/MaintainUserNotification")]
        public Result<IEnumerable<UserNotificationVM>> MaintainUserNotification([FromBody] DocumentsPayload payload)
        {
            return new UserNotificationLogic().MaintainData(payload);
        }

        #region DocumentReceiving
        [HttpPost]
        [Route("service/CheckProjectRequestDocument")]
        public Result<ProjectRequestVM> CheckProjectRequestDocument(PayloadVM payload)
        {
            return new SharedLogic().CheckProjectRequestDocument(payload);
        }

        [HttpPost]
        [Route("service/GetProjectLogs")]
        public Result<IEnumerable<ProjectRequestHistoryVM>> GetProjectLogs(PayloadVM payload)
        {
            return new SharedLogic().GetProjectLogs(payload);
        }
        #endregion

        [HttpPost]
        [Route("service/GetProjectInfoReport")]
        public Result<byte[]> GetProjectInfoReport([FromBody] Payload payload)
        {
           return new ReportsLogic().ProjectRequestReport(payload);
        }

        [HttpPost]
        [Route("service/GetAbstractofBids")]
        public Result<byte[]> GetAbstractofBids([FromBody] Payload payload)
        {
            return  new ReportsLogic().AbstractofBids(payload);
        }

        [HttpPost]
        [Route("service/GetInvitationToBid")]
        public Result<byte[]> GetInvitationToBid([FromBody] Payload payload)
        {
            return new ReportsLogic().InvitationToBid(payload);
        }

        [HttpPost]
        [Route("service/GetPostingApproval")]
        public Result<byte[]> GetPostingApproval([FromBody] Payload payload)
        {
            return new ReportsLogic().PostingApproval(payload);
        }

        [HttpPost]
        [Route("service/GetPostQualification")]
        public Result<byte[]> GetPostQualification([FromBody] Payload payload)
        {
            return new ReportsLogic().PostQualification(payload);
        }


        [HttpPost]
        [Route("service/GetLCBMemo")]
        public Result<byte[]> GetLCBMemo([FromBody] Payload payload)
        {
            return new ReportsLogic().LCBMemo(payload);
        }

        [HttpPost]
        [Route("service/GetLCBNotice")]
        public Result<byte[]> GetLCBNotice([FromBody] Payload payload)
        {
            return new ReportsLogic().LCBNotice(payload);
        }

        [HttpPost]
        [Route("service/GetNOP")]
        public Result<byte[]> GetNOP([FromBody] Payload payload)
        {
            return new ReportsLogic().NOP(payload);
        }

        [HttpPost]
        [Route("service/GetNOA")]
        public Result<byte[]> GetNOA([FromBody] Payload payload)
        {
            return new ReportsLogic().NOA(payload);
        }

    }
}
