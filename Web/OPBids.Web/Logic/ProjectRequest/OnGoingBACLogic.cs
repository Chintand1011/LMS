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
    public class OnGoingBACLogic : LogicBase
    {
        #region Context
        public OnGoingBACLogic(HttpRequestBase httpContext)
        {
            base._context = httpContext;
        }
        #endregion

        #region View
        public override ActionResult GetProjectRequestInformation(PayloadVM payload)
        {
            Result<PayloadVM> _list = new ApiManager<Result<PayloadVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestInformation, payload);
            bool hasProjecBids = false;
            if (_list.value.projectBids != null)
            {
                hasProjecBids = true;
                var lowestBid = _list.value.projectBids.OrderByDescending(x => x.bid_amount).Take(1).SingleOrDefault();
                if (lowestBid != null)
                {
                    ViewBag.LowestBidName = lowestBid.bidder_name;
                    ViewBag.LowestBidAddress = lowestBid.bidder_address;
                    ViewBag.LowestBidAuthRep = lowestBid.auth_rep;
                }
            }
            ViewBag.hasProjecBids = hasProjecBids;
            return PartialView(Constant.ProjectRequest.ResultView.ProjectRequestInformation, _list.value);
        }

        public override string HeaderView(string subMenuId)
        {
            return Constant.ProjectRequest.HeaderView.OnGoingBAC;
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
                case Constant.TransactionType.GetBid:
                    return base.GetBid(payload, Constant.ProjectRequest.ResultView.OnGoingBidderList);
                case Constant.TransactionType.GetLCB:
                    return base.GetLCB(payload);
                case Constant.TransactionType.StatusUpdate:
                    return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.OnGoingBAC, Constant.TransactionType.StatusUpdate);
                case Constant.TransactionType.ProcessUpdate:
                    return ProcessUpdate(payload);
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
            payload.projectSearch.record_status = Constant.RecordStatus.Active;
            payload.projectSearch.project_status = "ONGOING";
            payload.monitoredProject = new MonitoredProjectVM() { created_by = Convert.ToInt16(base.UserID) };

            Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);
            
            base.FormatProjectRequest(_result.value.items);
            return PartialView(Constant.ProjectRequest.ResultView.OnGoingBAC, _result.value);            
        }

        public override ActionResult Save(PayloadVM payload)
        {
            if (payload.projectRequest.required_date != null) {
                payload.projectRequest.required_date = DateTime.ParseExact(payload.projectRequest.required_date, Constant.DateFormat, CultureInfo.InvariantCulture).ToString(Constant.DateTimeFormat);
            }            
            payload.projectRequest.updated_by = Convert.ToInt16(base.UserID);

            Result<int> _list = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequest, payload);
            return new JsonResult() { Data = _list };
        }

        public ActionResult ProcessUpdate(PayloadVM payload) {
            if (payload.projectRequest.user_action == Constant.UserAction.UpdateImplementationSatus) {
                switch (payload.projectRequest.imp_perc_status) {
                    case "16.1":
                        payload.projectRequest.project_status = Constant.ProjectRequest.ProjectStatus.ProjectInstallation;
                        payload.projectRequest.project_substatus = Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_Initial;
                        break;
                    case "16.2":
                        payload.projectRequest.project_status = Constant.ProjectRequest.ProjectStatus.ProjectInstallation;
                        payload.projectRequest.project_substatus = Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_40Percent;
                        break;
                    case "16.3":
                        payload.projectRequest.project_status = Constant.ProjectRequest.ProjectStatus.ProjectInstallation;
                        payload.projectRequest.project_substatus = Constant.ProjectRequest.ProjectSubStatus.UnderImplementation_80Percent;
                        break;
                    case "17.1":
                        payload.projectRequest.project_status = Constant.ProjectRequest.ProjectStatus.Completed;
                        payload.projectRequest.project_substatus = Constant.ProjectRequest.ProjectSubStatus.Completed;
                        break;
                }
            }

            return base.StatusUpdate(payload, Constant.ProjectRequest.ResultView.OnGoingBAC, Constant.TransactionType.ProcessUpdate);
        }

        public ActionResult AdvertiseProject(PayloadVM payload)
        {
            payload.projectAdvertisement.created_by = Convert.ToInt16(this.UserID);
            payload.projectAdvertisement.updated_by = Convert.ToInt16(this.UserID);

            if (payload.txn == Constant.TransactionType.Save)
            {
                Result<bool> _result = new ApiManager<Result<bool>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateProjectRequestAdvertisement, payload);
                return new JsonResult() { Data = _result };
            }
            else
            {
                Result<ProjectRequestAdvertisementVM> _result = new ApiManager<Result<ProjectRequestAdvertisementVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestAdvertisement, payload);
                return new JsonResult() { Data = _result };
            }
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