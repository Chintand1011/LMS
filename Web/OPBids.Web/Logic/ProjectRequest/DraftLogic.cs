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
    public class DraftLogic : LogicBase
    {
        #region Context
        public DraftLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
            base.ProjectStatus = Constant.ProjectRequest.ProjectStatus.Draft;
        }
        #endregion

        #region View

        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.Draft;
        }

        public override ActionResult ResultView(PayloadVM payload)
        {
            //this.InitializePayload(payload);

            switch (payload.txn)
            {
                //case Constant.TransactionType.Search:
                //    return Search(payload);
                case Constant.TransactionType.Save:
                    return Save(payload);
                case Constant.TransactionType.StatusUpdate:
                    return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.Draft, Constant.TransactionType.StatusUpdate);
                case Constant.TransactionType.ProcessUpdate:
                    return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.Draft, Constant.TransactionType.ProcessUpdate);
                default:
                    return Search(payload);
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
        #endregion

        #region CRUD


        public override ActionResult Search(PayloadVM payload)
        {
            if (payload.projectSearch == null)
            {
                return PartialView(Constant.ProjectRequest.ResultView.Draft, new ProjectSearchResultVM() {
                    count = 0,
                    page_index = 1,
                    items = new List<ProjectRequestVM>()
                });
            }
            this.InitializePayload(payload);
            //IEnumerable<ProjectRequestVM> _list = base.SearchData<ProjectRequestVM>(payload, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest);
            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);
            //
            base.FormatProjectRequest(_result.value.items);
            return PartialView(Constant.ProjectRequest.ResultView.Draft, _result.value);
        }

        public override ActionResult Save(PayloadVM payload)
        {
            var _endpoint = string.Empty;

            ProjectRequestVM _request = payload.projectRequest;
            _request.required_date = DateTime.ParseExact(_request.required_date, Constant.DateFormat, CultureInfo.InvariantCulture).ToString(Constant.DateTimeFormat);
            _request.updated_by = Convert.ToInt16(base.UserID);
            _request.approved_budget = _request.estimated_budget;
            _request.record_status = Constant.RecordStatus.Active;

            if (_request.id == 0)
            {
                // Create Project Request
                _endpoint = Constant.ServiceEnpoint.ProjectRequest.CreateProjectRequest;

                _request.created_by = Convert.ToInt16(base.UserID);
                payload.projectRequest = _request;                
            }
            else
            {
                // Update Project Request
                _endpoint = Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequest;
                payload.projectRequest = _request;                
            }

            Result<int> _result = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, _endpoint, payload);
            return new JsonResult() { Data = _result };
        }

        #endregion

        #region Utilities

        private void InitializePayload(PayloadVM payload)
        {
            payload.projectSearch = payload.projectSearch == null ? new ProjectSearchVM() : payload.projectSearch;
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            payload.projectSearch.project_status = Constant.ProjectRequest.ProjectStatus.Draft;
            payload.projectSearch.current_user = Convert.ToInt16(base.UserID);
        }

        #endregion
    }
}