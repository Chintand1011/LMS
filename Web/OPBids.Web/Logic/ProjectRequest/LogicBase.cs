using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;
using OPBids.Entities.Base;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Web.Helper;

namespace OPBids.Web.Logic.ProjectRequest
{
    public abstract class LogicBase : Controller
    {
        #region Session Variables
        string _group, _userid, _searchProjectStatus;
        bool _vip;

        public HttpRequestBase _context { get; set; }

        public string GroupID
        {
            get
            {
                if (_group.IsNullOrEmpty())
                {
                    this._group = AuthHelper.GetClaims(_context.GetOwinContext(), Constant.Auth.Claims.GroupId);
                }
                return this._group;
            }
        }
        public string UserID
        {
            get
            {
                if (_userid.IsNullOrEmpty())
                {
                    this._userid = AuthHelper.GetClaims(_context.GetOwinContext(), Constant.Auth.Claims.UserId);
                }
                return this._userid;
            }
        }
        public bool VIP
        {
            get
            {
                return AuthHelper.GetClaims(_context.GetOwinContext(), Constant.Auth.Claims.VIP).ToSafeBool();
            }
        }
        public string ProjectStatus
        {
            get
            {
                return this._searchProjectStatus;
            }
            set {
                _searchProjectStatus = value;
            }

        }

        #endregion

        #region Abstract Class
        public abstract string HeaderView(string subMenuId);
        public abstract ActionResult ResultView(PayloadVM payload);
        public abstract ActionResult Search(PayloadVM payload);
        public abstract ActionResult Save(PayloadVM payload);
        public abstract ActionResult ProjectAttachments(PayloadVM payload);

        public virtual ActionResult GetProjectRequestInformation(PayloadVM payload)
        {
            Result<PayloadVM> _list = new ApiManager<Result<PayloadVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequestInformation, payload);
            return PartialView(Constant.ProjectRequest.ResultView.ProjectRequestInformation, _list.value);
        }

        #endregion

        #region Base Functions

        public IEnumerable<T> SearchData<T>(PayloadVM payload, string service)
        {
            //Constant.ServiceEnpoint.ProjectRequest.GetProjectRequest
            return ProcessData<T>(payload, service);
        }

        public ActionResult Get(PayloadVM payload)
        {
            var _result = ProcessData<ProjectRequestVM>(payload, Constant.ServiceEnpoint.ProjectRequest.GetProjectRequest);
            return new JsonResult() { Data = _result };
        }

        protected ActionResult GetBid(PayloadVM payload, string view)
        {
            Result<IEnumerable<ProjectBidVM>> _list = new ApiManager<Result<IEnumerable<ProjectBidVM>>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetProjectBid, payload);
            if (_list.status.code == Constant.Status.Success && _list.value.Count() > 0)
            {
                return PartialView(view, _list.value);
            }
            else
            {
                return null;
            }
        }

        protected ActionResult GetLCB(PayloadVM payload) {
            Result<ProjectBidVM> _lcb = new ApiManager<Result<ProjectBidVM>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.GetLowestCalculatedBid, payload);
            return new JsonResult() { Data = _lcb };
        }

        protected IEnumerable<T> ProcessData<T>(PayloadVM payload, string service)
        {
            var apiManager = new ApiManager<Result<IEnumerable<T>>>();
            Result<IEnumerable<T>> _list;
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                service, payload);
            return _list.value == null ? new List<T>() : _list.value;
        }

        protected ActionResult StatusUpdate(PayloadVM payload, string view, string type)
        {
            payload.projectRequest.updated_by = Convert.ToInt16(this.UserID);

            if (type == Constant.TransactionType.StatusUpdate)
            {
                Result<string[]> _list = new ApiManager<Result<string[]>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateRecordStatus, payload);
                return new JsonResult() { Data = _list };
            }
            else
            {
                Result<int> _list = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateProjectStatus, payload);
                return new JsonResult() { Data = _list };
            }
        }

        protected ActionResult MonitorProject(PayloadVM payload) {
            payload.monitoredProject.created_by = Convert.ToInt16(this.UserID);
            Result<bool> _list = new ApiManager<Result<bool>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.MonitorProject, payload);
            return new JsonResult() { Data = _list };
        }

        protected void FormatProjectRequest(IEnumerable<ProjectRequestVM> _list)
        {
            _list.ToList().ForEach(x =>
            {
                x.required_date = DateTime.Parse(x.required_date).ToString(Constant.DateFormat);
                x.created_date = DateTime.Parse(x.created_date).ToString(Constant.DateFormat);
                x.session_group_id = this.GroupID;
            });
        }

        #endregion


    }
}