using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Web.Helper;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Logic.ProjectRequest
{
    public class MonitorLogic : LogicBase
    {
        #region Context
        public MonitorLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
        }
        #endregion

        #region View
        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.Monitor;
        }

        public override ActionResult ResultView(PayloadVM payload)
        {
            this.InitializePayload(payload);            
            switch (payload.txn)
            {
                case Constant.TransactionType.Monitor:
                    return base.MonitorProject(payload);
                default:
                    return Search(payload);
            }
        }
        #endregion

        #region CRUD

        public override ActionResult Search(PayloadVM payload)
        {
            payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };
            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchMonitorProject, payload);
            return PartialView(Constant.ProjectRequest.ResultView.Monitor, _result.value);
        }

        #endregion

        #region Utilities

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Active;            
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