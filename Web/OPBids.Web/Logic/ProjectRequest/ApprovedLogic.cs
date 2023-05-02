using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Web.Helper;

namespace OPBids.Web.Logic.ProjectRequest
{
    public class ApprovedLogic : LogicBase
    {
        #region Context
        public ApprovedLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
        }
        #endregion

        #region View
        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.Approved;
        }

        public override ActionResult ResultView(PayloadVM payload)
        {
            this.InitializePayload(payload);
            return Search(payload);            
        }
        #endregion

        #region CRUD

        public override ActionResult Search(PayloadVM payload)
        {
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            payload.projectSearch.project_status = "APPROVED";
            payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };
            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);
            return PartialView(Constant.ProjectRequest.ResultView.Approved, _result.value);
        }     

        #endregion

        #region Utilities

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            //payload.projectSearch.project_substatus_min = Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_WaitingBudgetDocs;
            //payload.projectSearch.project_substatus_max = Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_80Percent;
        }

        public override ActionResult ProjectAttachments(PayloadVM payload)
        {
            this.InitializePayload(payload);
            Result<IEnumerable<ProjectRequestAttachmentVM>> _result;
            ApiManager<Result<IEnumerable<ProjectRequestAttachmentVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectRequestAttachmentVM>>>();
            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestAttachments, payload);
            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();
        }

        public override ActionResult Save(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}