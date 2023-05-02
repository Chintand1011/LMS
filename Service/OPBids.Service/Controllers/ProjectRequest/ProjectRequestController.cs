using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic;
using OPBids.Service.Models.ProjectRequest;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers
{
    public class ProjectRequestController : ApiController
    {
        #region Project Request
        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequest)]
        public Result<ProjectSearchResultVM> GetProjectRequest([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequest(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestInformation)]
        public Result<PayloadVM> GetProjectRequestInformation([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestInformation(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest)]
        public Result<ProjectSearchResultVM> SearchProjectRequest([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().SearchProjectRequest(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.CreateProjectRequest)]
        public Result<int> CreateProjectRequest([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().CreateProjectRequest(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequest)]
        public Result<int> UpdateProjectRequest([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateProjectRequest(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateRecordStatus)]
        public Result<string[]> UpdateRecordStatus([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateRecordStatus(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateProjectStatus)]
        public Result<int> UpdateProjectStatus([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateProjectStatus(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestAttachments)]
        public Result<IEnumerable<ProjectRequestAttachmentVM>> GetProjectRequestAttachments([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestAttachments(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.CreateProjectRequestAttachment)]
        public Result<int> CreateProjectRequestAttachment([FromBody] ProjectRequestAttachmentVM attachment)
        {
           return new ProjectRequestLogic().SaveDocumentAttachment(attachment);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectGranteesFilter)]
        public Result<IEnumerable<ProjectGranteeVM>> GetProjectGranteesFilter([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectGranteesFilter();
        }
        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectCategoriesFilter)]
        public Result<IEnumerable<ProjectCategoryVM>> GetProjectCategoriesFilter([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectCategoriesFilter();
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestItems)]
        public Result<IEnumerable<ProjectRequestItemVM>> GetProjectRequestItems([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestItems(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectInfo)]
        public Result<IEnumerable<ProjectRequestVM>> GetProjectInfo([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectInfo(payload);
        }
        #endregion

        #region Project Request Batch

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestBatch)]
        public Result<IEnumerable<ProjectRequestBatchVM>> GetProjectRequestBatch([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestBatch(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestListForBatch)]
        public Result<IEnumerable<ProjectRequestVM>> GetProjectRequestListForBatch([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestListForBatch(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequestBatch)]
        public Result<ProjectBatchSearchResultVM> SearchProjectRequestBatch([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().SearchProjectRequestBatch(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.CreateProjectRequestBatch)]
        public Result<int> CreateProjectRequestBatch([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().CreateProjectRequestBatch(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequestBatch)]
        public Result<int> UpdateProjectRequestBatch([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateProjectRequestBatch(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateBatchRecordStatus)]
        public Result<int> UpdateBatchRecordStatus([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateBatchRecordStatus(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateBatchProjectStatus)]
        public Result<int> UpdateBatchProjectStatus([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateBatchProjectStatus(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestBatchAdvertisements)]
        public Result<ProjectRequestBatchVM> GetProjectRequestBatchAdvertisement([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestBatchAdvertisement(payload);
        }

        #endregion

        #region Project Bid

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectBid)]
        public Result<IEnumerable<ProjectBidVM>> GetProjectBid([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectBid(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetLowestCalculatedBid)]
        public Result<ProjectBidVM> GetLowestCalculatedBid([FromBody] PayloadVM payload) {
            return new ProjectRequestLogic().GetLowestCalculatedBid(payload);
        }

        #endregion

        #region Project Bid Checklist
        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectBidChecklists)]
        public Result<List<ProjectBidChecklist>> GetProjectBidChecklists([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectBidChecklists(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.CreateProjectBidChecklists)]
        public Result<bool> CreateProjectBidChecklists([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().CreateProjectBidChecklists(payload);
        }
        #endregion

        #region Project Request Advertisement
        
        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestAdvertisement)]
        public Result<ProjectRequestAdvertisementVM> GetProjectRequestAdvertisement([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().GetProjectRequestAdvertisement(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequestAdvertisement)]
        public Result<bool> UpdateProjectRequestAdvertisement([FromBody] PayloadVM payload)
        {
            return new ProjectRequestLogic().UpdateProjectRequestAdvertisement(payload);
        }
        #endregion

        #region MonitoredProject
        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.MonitorProject)]
        public Result<bool> MonitoredProject([FromBody] PayloadVM payload)
        {
            return new MonitoredProjectLogic().MonitoredProject(payload);
        }

        [HttpPost]
        [Route(Constant.ServiceEnpoint.ProjectRequest.SearchMonitorProject)]
        public Result<ProjectSearchResultVM> SearchMonitorProject([FromBody] PayloadVM payload)
        {
            return new MonitoredProjectLogic().SearchMonitorProject(payload);
        }
        #endregion
    }
}