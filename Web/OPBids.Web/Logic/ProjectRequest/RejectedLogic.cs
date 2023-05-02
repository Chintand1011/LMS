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
    public class RejectedLogic : LogicBase
    {
        #region Context
        public RejectedLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
        }
        #endregion

        public override string HeaderView(string subMenuId)
        {
            if (base.GroupID == Constant.AccessGroups.HoPE.ToString())
            {
                return Constant.ProjectRequest.HeaderView.RejectedBatch;
            }
            else {
                return Constant.ProjectRequest.HeaderView.Rejected;
            }
            
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

        public override ActionResult ResultView(PayloadVM payload)
        {
            switch (payload.txn)
            {
                case Constant.TransactionType.StatusUpdate:
                    if (base.GroupID == Constant.AccessGroups.HoPE.ToString())
                    {
                        payload.projectRequestBatch.updated_by = Convert.ToInt16(this.UserID);
                        Result<int> _list = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateBatchRecordStatus, payload);
                        return new JsonResult() { Data = _list };
                    }
                    else
                    {
                        return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.Rejected, Constant.TransactionType.StatusUpdate);                        
                    }

                default:
                    return Search(payload);
            }
        }

        public override ActionResult Save(PayloadVM payload)
        {
            throw new NotImplementedException();
        }

        public override ActionResult Search(PayloadVM payload)
        {            
            if (base.GroupID == Constant.AccessGroups.HoPE.ToString())
            {
                if (payload.projectSearch == null)
                {
                    return PartialView(Constant.ProjectRequest.ResultView.RejectedBatch, new ProjectSearchResultVM()
                    {
                        count = 0,
                        page_index = 1,
                        items = new List<ProjectRequestVM>()
                    });
                }
                payload.projectSearch.record_status = Constant.RecordStatus.Rejected;
                payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };

                Result<ProjectBatchSearchResultVM> _result = new ApiManager<Result<ProjectBatchSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequestBatch, payload);
                this.FormatBatch(_result);
                return PartialView(Constant.ProjectRequest.ResultView.RejectedBatch, _result.value);
            }
            else {
                if (payload.projectSearch == null)
                {
                    return PartialView(Constant.ProjectRequest.ResultView.Rejected, new ProjectSearchResultVM()
                    {
                        count = 0,
                        page_index = 1,
                        items = new List<ProjectRequestVM>()
                    });
                }
                payload.projectSearch.record_status = Constant.RecordStatus.Rejected;
                payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };

                Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);
                base.FormatProjectRequest(_result.value.items);
                _result.value.vip = base.VIP;
                return PartialView(Constant.ProjectRequest.ResultView.Rejected, _result.value);
            }
            
        }

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Rejected;
            payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };
        }

        private void FormatBatch(Result<ProjectBatchSearchResultVM> result) {
            if (result.value != null && result.value.items.Count() > 0) {
                result.value.items.ToList().ForEach(batch => {
                    if (batch.project_substatus == Constant.ProjectRequest.ProjectSubStatus.ProcApproval_Hope) {
                        batch.project_substatus_desc = "HoPE Rejected";
                    }
                });
                result.value.vip = base.VIP;
            }
        }

    }
}