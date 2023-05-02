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
    public class ApprovalHopeLogic : LogicBase
    {
        #region Context
        public ApprovalHopeLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
            base.ProjectStatus = Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope;
        }
        #endregion

        #region View
        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.ApprovalHope;
        }

        public override ActionResult ResultView(PayloadVM payload)
        {
            this.InitializePayload(payload);

            switch (payload.txn)
            {
                case Constant.TransactionType.Search:
                    return Search(payload);
                case Constant.TransactionType.Save:
                    return Save(payload);
                case Constant.TransactionType.StatusUpdate:
                    return BatchStatusUpdate(payload, Constant.ProjectRequest.ResultView.ApprovalHope, Constant.TransactionType.StatusUpdate);
                case Constant.TransactionType.ProcessUpdate:
                    return BatchStatusUpdate(payload, Constant.ProjectRequest.ResultView.ApprovalHope, Constant.TransactionType.ProcessUpdate);
                default:
                    return Search(payload);
            }
        }

        internal ActionResult GetBatchProjectRequestList(PayloadVM payload)
        {
            if (payload.projectSearch.batch_id == "0")
            {
                return new JsonResult() { Data = null };
            }
            else {
                IEnumerable<ProjectRequestVM> _list = base.SearchData<ProjectRequestVM>(payload, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestListForBatch);
                return new JsonResult() { Data = _list };
            }
        }
        #endregion

        #region CRUD


        public override ActionResult Search(PayloadVM payload)
        {
            Result<ProjectBatchSearchResultVM> _result = new ApiManager<Result<ProjectBatchSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequestBatch, payload);
            return PartialView(Constant.ProjectRequest.ResultView.ApprovalHope, _result.value);            
        }

        public override ActionResult Save(PayloadVM payload)
        {
            var _endpoint = string.Empty;

            payload.projectRequestBatch.updated_by = Convert.ToInt16(base.UserID);
            payload.projectRequestBatch.record_status = Constant.RecordStatus.Active;

            if (payload.projectRequestBatch.id == 0)
            {
                // Create Project Request
                _endpoint = Constant.ServiceEnpoint.ProjectRequest.CreateProjectRequestBatch;

                payload.projectRequestBatch.created_by = Convert.ToInt16(base.UserID);
            }
            else
            {
                // Update Project Request
                _endpoint = Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequestBatch;
            }

            Result<int> _result = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, _endpoint, payload);
            return new JsonResult() { Data = _result };
        }

        protected ActionResult BatchStatusUpdate(PayloadVM payload, string view, string type)
        {
            payload.projectRequestBatch.updated_by = Convert.ToInt16(this.UserID);

            if (type == Constant.TransactionType.StatusUpdate)
            {
                Result<int> _list = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateBatchRecordStatus, payload);
                return new JsonResult() { Data = _list };
            }
            else
            {
                Result<int> _list = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateBatchProjectStatus, payload);
                return new JsonResult() { Data = _list };
            }
        }


        public ActionResult GetProjectRequestForBatch(PayloadVM payload)
        {
            if (payload.projectSearch == null)
            {
                return PartialView(Constant.ProjectRequest.ResultView.PostQualification, new ProjectSearchResultVM()
                {
                    count = 0,
                    page_index = 1,
                    items = new List<ProjectRequestVM>()
                });
            }
            payload.projectSearch.project_substatus = Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_ProcMethodRecom;
            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequest, payload);

            base.FormatProjectRequest(_result.value.items);
            return PartialView(Constant.ProjectRequest.ResultView.PostQualification, _result.value);

            //IEnumerable<ProjectRequestVM> _list = base.SearchData<ProjectRequestVM>(payload, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequest);
            //return new JsonResult() { Data = _list.Where(x => x.batch_id == 0 || x.batch_id.ToString() == payload.projectSearch.batch_id) };
        }

        #endregion

        #region Utilities

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            payload.projectSearch.project_status = Constant.ProjectRequest.ProjectStatus.PreBidding_ProcApproval_Hope;
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

        #endregion
    }
}