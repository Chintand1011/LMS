using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Common;
using OPBids.Entities.Base;
using OPBids.Entities.Common;
using OPBids.Entities.View.Home;
using OPBids.Web.Helper;
namespace OPBids.Web.Logic
{
    [Authorize]
    public class DashboardLogic: Controller
    {
        #region Context
        HttpRequestBase _context;
        public DashboardLogic(HttpRequestBase httpContext)
        {
            this._context = httpContext;
        }
        #endregion

        #region Summary Header

        public ActionResult SummaryHeader(Payload payload)
        {
            Result<DashboardSummaryVM> _result;

            ApiManager<Result<DashboardSummaryVM>> apiManager = new ApiManager<Result<DashboardSummaryVM>>();

            payload.group_id = Convert.ToInt16(AuthHelper.GetClaims(this._context.GetOwinContext(), Constant.Auth.Claims.GroupId));

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetSummaryHeader, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                //TODO:update iconCls based on type
                //_result.value.summary1.iconCls = CssHelper.GetSurveyTypeIconCls(_result.value.summary1.type);
                //_result.value.summary2.iconCls = CssHelper.GetSurveyTypeIconCls(_result.value.summary2.type);
                //_result.value.summary3.iconCls = CssHelper.GetSurveyTypeIconCls(_result.value.summary3.type);
            }
            switch(this._context.QueryString["v"])
            {
                case "1":
                    return PartialView("SummaryHeader", _result.value);
                default:
                    return PartialView("SummaryHeader_v2", _result.value);
                    
            }
            
        }

        #endregion

        #region Dashboard Tables

        public ActionResult DashboardTable(BaseVM payload)
        {
            Result<DashboardTableDataVM> _result;

            ApiManager<Result<DashboardTableDataVM>> apiManager = new ApiManager<Result<DashboardTableDataVM>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("DashboardTable", _result.value);
        }

        public ActionResult AGMTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("TableAGM/TableResult", _result.value);
        }

        public ActionResult UserTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("TableUser/TableResult", _result.value);
        }

        public ActionResult BudgetTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("TableBudget/TableResult", _result.value);
        }

        public ActionResult BACSECTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("TableBACSEC/TableResult", _result.value);
        }

        public ActionResult HOPETableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("TableHOPE/TableResult", _result.value);
        }

        public ActionResult SupplierTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("TableSupplier/TableResult", _result.value);
        }

        public ActionResult BACTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }

            if (payload.status == "delayed")//dummy 
            {
                return PartialView("TableBAC/TableResult2", _result.value);
            }
            else
            {
                return PartialView("TableBAC/TableResult", _result.value);
            }
        }

        public ActionResult TWGTableResult(DashboardPayloadVM payload)
        {
            Result<IEnumerable<DashboardTableResultVM>> _result;

            ApiManager<Result<IEnumerable<DashboardTableResultVM>>> apiManager = new ApiManager<Result<IEnumerable<DashboardTableResultVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardTable, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }

            return PartialView("TableTWG/TableResult", _result.value);
        }

        #endregion

        #region Dashboard Charts

        public ActionResult DashboardCharts(DashboardPayloadVM payload)
        {
            var dashboard_id = AuthHelper.GetClaims(_context.GetOwinContext(), Constant.Auth.Claims.DashboardId);
            payload.id = Convert.ToInt16(dashboard_id);

            Result<DashboardChartsVM> _result;

            ApiManager<Result<DashboardChartsVM>> apiManager = new ApiManager<Result<DashboardChartsVM>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetDashboardCharts, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }
            return PartialView("DashboardCharts", _result.value);
        }

        #endregion

        

        public ActionResult TWGResultHeaders(DashboardPayloadVM payload)
        {
            Result<TWGResultHeadersVM> _result;

            ApiManager<Result<TWGResultHeadersVM>> apiManager = new ApiManager<Result<TWGResultHeadersVM>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.DashboardRequest.GetTWGResultHeaders, payload);

            if (_result.status.code == Constant.Status.Success)
            {
            }

            return new JsonResult { Data = _result.value };
           
        }
    }



}