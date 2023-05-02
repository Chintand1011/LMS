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
    public class PlanLogic : LogicBase
    {
        #region Context
        public PlanLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
            //base.ProjectStatus = Constant.ProjectRequest.ProjectStatus.PreBidding_BudgetApproved;
        }
        #endregion

        #region View
        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.Plan;
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
                    return BatchStatusUpdate(payload, Constant.ProjectRequest.ResultView.Plan, Constant.TransactionType.StatusUpdate);
                case Constant.TransactionType.ProcessUpdate:
                    return BatchStatusUpdate(payload, Constant.ProjectRequest.ResultView.Plan, Constant.TransactionType.ProcessUpdate);
                default:
                    return Search(payload);
            }
        }
        #endregion

        #region CRUD


        public override ActionResult Search(PayloadVM payload)
        {
            Result<ProjectBatchSearchResultVM> _result = new ApiManager<Result<ProjectBatchSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequestBatch, payload);
            return PartialView(Constant.ProjectRequest.ResultView.Plan, _result.value);
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


        public ActionResult GetProjectRequestForBatch(PayloadVM payload) {

            payload.projectSearch.project_substatus = Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_ProcMethodRecom;

            IEnumerable<ProjectRequestVM> _list = base.SearchData<ProjectRequestVM>(payload, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestListForBatch);        
            return new JsonResult() { Data = _list };
            //return new JsonResult() { Data = _list.Where(x => x.batch_id == 0 || x.batch_id.ToString() == payload.projectSearch.batch_id) };
        }

        #endregion

        #region Utilities

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            //payload.projectSearch.project_status = Constant.ProjectRequest.ProjectStatus.PreBidding_BudgetApproved;
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

        public ActionResult GetBidInvitation(PayloadVM payload)
        {            
            //IEnumerable<ProjectRequestBatchVM> _list = base.SearchData<ProjectRequestBatchVM>(payload, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestBatch);
          
            Result<IEnumerable<ProjectRequestBatchVM>> _result;

            ApiManager<Result<IEnumerable<ProjectRequestBatchVM>>> apiManager = new ApiManager<Result<IEnumerable<ProjectRequestBatchVM>>>();

            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestBatch, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();
        }

        public ActionResult GetProjectRequestBatchAds(PayloadVM payload)
        {
            Result<ProjectRequestBatchVM> _result;

            ApiManager<Result<ProjectRequestBatchVM>> apiManager = new ApiManager<Result<ProjectRequestBatchVM>>();

            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestBatchAdvertisements, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();
        }
        #endregion
    }
}