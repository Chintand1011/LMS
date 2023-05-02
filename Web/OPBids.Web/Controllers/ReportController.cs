using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AbstractOfBids(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetAbstractofBids", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""AbstractOfBids" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult InvitationToBid(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetInvitationToBid", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""InvitationToBid" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult PostingApproval(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetPostingApproval", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""PostingApproval" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult PostQualification(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetPostQualification", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""PostQualification" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult LCBMemo(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetLCBMemo", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""Memorandum" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult LCBNotice(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetLCBNotice", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""LCBNotice" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult NOP(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetNOP", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""NOP" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult NOA(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetNOA", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""NOA" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult NOPQ(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetNOPQ", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""NOPQ" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult RoutingSlip(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            payload.user_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.UserId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetRoutingSlip", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""RoutingSlip" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }

        public ActionResult Transmittal(Payload payload)
        {
            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            payload.user_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.UserId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetTransmittal", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", @"inline;filename=""Transmittal" + payload.id + @".pdf""");
            return File(pdf, "application/pdf");
        }
    }
}