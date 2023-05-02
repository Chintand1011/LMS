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
    public class OnGoingLogic : LogicBase
    {
        #region Context
        public OnGoingLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
            base.ProjectStatus = Constant.ProjectRequest.ProjectStatus.NewlyRequested;
        }
        #endregion

        #region View
        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.OnGoing;
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
                case Constant.TransactionType.Get:
                    return base.Get(payload);
                case Constant.TransactionType.StatusUpdate:
                    return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.OnGoing, Constant.TransactionType.StatusUpdate);
                case Constant.TransactionType.ProcessUpdate:
                    return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.OnGoing, Constant.TransactionType.ProcessUpdate);
                default:
                    return Search(payload);
            }
        }
        #endregion

        #region CRUD

        public override ActionResult Search(PayloadVM payload)
        {            
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            payload.projectSearch.project_status = "ONGOING";
            payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };

            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);
            
            base.FormatProjectRequest(_result.value.items);
            return PartialView(Constant.ProjectRequest.ResultView.OnGoing, _result.value);
        }

        public override ActionResult Save(PayloadVM payload)
        {
            payload.projectRequest.required_date = DateTime.ParseExact(payload.projectRequest.required_date, Constant.DateFormat, CultureInfo.InvariantCulture).ToString(Constant.DateTimeFormat);
            payload.projectRequest.updated_by = Convert.ToInt16(base.UserID);

            Result<int> _list = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequest, payload);
            return new JsonResult() { Data = _list };
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

        #endregion
    }
}