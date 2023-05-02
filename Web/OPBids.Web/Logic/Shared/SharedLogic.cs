using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Shared;
using OPBids.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Logic.Shared
{
    public class SharedLogic : Controller
    {
        // All stage where user can receive docs
        private string[] DocReceivableStage = new string[] {
            Constant.ProjectRequest.ProjectSubStatus.WaitingEUDocs,
            Constant.ProjectRequest.ProjectSubStatus.WaitingBACSECValidDocs,
            Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_WaitingBudgetDocs,
            Constant.ProjectRequest.ProjectSubStatus.ProcApproval_Hope,
            Constant.ProjectRequest.ProjectSubStatus.ITB_Preparation,
            Constant.ProjectRequest.ProjectSubStatus.OpeningOfBids,
            Constant.ProjectRequest.ProjectSubStatus.ForRanking,
            Constant.ProjectRequest.ProjectSubStatus.ForLCBNotice,
            Constant.ProjectRequest.ProjectSubStatus.ForEvaluation,
            Constant.ProjectRequest.ProjectSubStatus.PostEvaluation,
            Constant.ProjectRequest.ProjectSubStatus.ForPostQualification,
            Constant.ProjectRequest.ProjectSubStatus.PostQualification
        };

        // All stage where user can receive docs and trigger workflow change
        private string[] DocReceivableWorkflowChange = new string[] {
            Constant.ProjectRequest.ProjectSubStatus.WaitingEUDocs,
            Constant.ProjectRequest.ProjectSubStatus.WaitingBACSECValidDocs,
            Constant.ProjectRequest.ProjectSubStatus.BudgetApproved_WaitingBudgetDocs };


        #region Context
        HttpRequestBase _context;
        public SharedLogic(HttpRequestBase httpContext)
        {
            this._context = httpContext;
        }
        #endregion

        public ActionResult GetProjectTotal(Payload payload)
        {
            Result<ProjectTotalVM> _result;
            var apiManager = new ApiManager<Result<ProjectTotalVM>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(this._context.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.SharedRequest.GetProjectTotal, payload);
            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();
        }
        public ActionResult GetDocumentTotal(Payload payload)
        {
            Result<ProjectTotalVM> _result;
            var apiManager = new ApiManager<Result<ProjectTotalVM>>();
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.SharedRequest.GetDocumentTotal, payload);
            if (_result.status.code == Constant.Status.Success)
            {
                return new JsonResult { Data = _result.value };
            }
            return new EmptyResult();
        }

        public Result<List<KeyValue>> GetSettingsList(Payload payload)
        {
            Result<List<KeyValue>> _result = new Result<List<KeyValue>>();
            ApiManager<Result<List<KeyValue>>> apiManager = new ApiManager<Result<List<KeyValue>>>();
            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.SharedRequest.GetSettingsList, payload);
            return _result;
        }

        public ActionResult GetProjectProgress(PayloadVM payload)
        {
            Result<List<ProgressVM>> _result;
            ApiManager<Result<List<ProgressVM>>> apiManager = new ApiManager<Result<List<ProgressVM>>>();
            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.SharedRequest.GetProjectProgress, payload);
            return PartialView(Constant.Shared.PartialView.Progress, _result.value);
        }

        #region DocumentReceiving
        public ActionResult CheckProjectRequestDocument(PayloadVM payload)
        {
            Status _status = new Status();
            try
            {
                var groupID = Convert.ToInt16(AuthHelper.GetClaims(this._context.GetOwinContext(), Constant.Auth.Claims.GroupId));

                Result<ProjectRequestVM> _result = new ApiManager<Result<ProjectRequestVM>>().Invoke(
                                        ConfigManager.BaseServiceURL,
                                        Constant.ServiceEnpoint.SharedRequest.CheckProjectRequestDocument, payload);

                if (_result.status.code == Constant.Status.Success)
                {
                    if (_result.value != null && !string.IsNullOrEmpty(_result.value.project_status))
                    {
                        if (_result.value.session_group_id.Split(',').ToList().Contains(groupID.ToString()))
                        {
                            if (DocReceivableStage.Contains(_result.value.project_substatus))
                            {
                                _result.value.isEditable = true;
                            }
                            else {
                                _result.value.isEditable = false;
                            }
                        }
                        else
                        {
                            _result.value.isEditable = false;
                        }
                    }
                    return new JsonResult { Data = _result };
                }
            }
            catch (Exception ex)
            {
                _status.code = Constant.Status.Failed;
                _status.description = ex.Message;
            }
            return new JsonResult { Data = _status };
        }

        public ActionResult ReceiveProjectRequestDocument(PayloadVM payload)
        {            
            try
            {
                var _userID = Convert.ToInt16(AuthHelper.GetClaims(this._context.GetOwinContext(), Constant.Auth.Claims.UserId));
                payload.projectRequest.updated_by = _userID;
                payload.projectRequest.user_action = Constant.UserAction.Received;
                payload.projectRequest.current_user = _userID;

                if (DocReceivableStage.Contains(payload.projectRequest.project_substatus)) {
                    if (DocReceivableWorkflowChange.Contains(payload.projectRequest.project_substatus)) {
                        Result<int> _wlist = new ApiManager<Result<int>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateProjectStatus, payload);
                        return new JsonResult() { Data = _wlist };
                    }
                }
                
                payload.item_list = new string[] { payload.projectRequest.id.ToString() };
                Result<string[]> _list = new ApiManager<Result<string[]>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.ProjectRequest.UpdateRecordStatus, payload);
                return new JsonResult() { Data = _list };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new Status() { code = Constant.Status.Failed, description= "Error Encoutered" } };
            }            
        }

        public ActionResult GetProjectLogs(PayloadVM payload) {
            try
            {
                Result<IEnumerable<ProjectRequestHistoryVM>> _wlist = new ApiManager<Result<IEnumerable<ProjectRequestHistoryVM>>>().Invoke(ConfigManager.BaseServiceURL, Constant.ServiceEnpoint.SharedRequest.GetProjectLogs, payload);
                return new JsonResult() { Data = _wlist };                
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new Status() { code = Constant.Status.Failed, description = "Error Encoutered" } };
            }
        }

        public ActionResult ViewProjectRequestDocument(PayloadVM payload)//temp
        {
            Status _status = new Status();
            try
            {
                var groupID = Convert.ToInt16(AuthHelper.GetClaims(this._context.GetOwinContext(), Constant.Auth.Claims.GroupId));

                Result<ProjectSearchResultVM> _result = new ApiManager<Result<ProjectSearchResultVM>>().Invoke(
                                        ConfigManager.BaseServiceURL,
                                        Constant.ServiceEnpoint.ProjectRequest.SearchProjectRequest, payload);

                if (_result.status.code == Constant.Status.Success)
                {
                    if (_result.value.items.Any())
                    {
                        return new JsonResult { Data = new Result<ProjectRequestVM>() { value = _result.value.items.FirstOrDefault() } };
                    }
                }

                _status.code = Constant.Status.Failed;
                return new JsonResult { Data = new Result<ProjectRequestVM>() { status = _status } };
            }
            catch (Exception ex)
            {
                _status.code = Constant.Status.Failed;
                _status.description = ex.Message;
            }
            return new JsonResult { Data = new Result<ProjectRequestVM>() { status = _status } };
        }
        #endregion
    }
}