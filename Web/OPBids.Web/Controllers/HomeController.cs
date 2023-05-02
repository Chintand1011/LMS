using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Web.Logic;
using OPBids.Entities.Base;
using OPBids.Entities.View.Home;
using OPBids.Common;
using OPBids.Web.Helper;
using OPBids.Entities.Common;
using OPBids.Web.Logic.ActivityLog;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int roleId = AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId).ToSafeInt();
            int productType = AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.CurrentProduct).ToSafeInt();
            ViewBag.Role = roleId.ToSafeString();
            if (productType == 2)
            {
                return Redirect("../DTS/Index");
            }
            else
            {

                Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.PFMS, ActivityLogType.PFMSDashBoard, "View PFMS DashBoard", "View PFMS DashBoard");

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult SummaryHeader(Payload payload)
        {            
            return new DashboardLogic(Request).SummaryHeader(payload);
        }

        public ActionResult DashboardTable(BaseVM payload)
        {
            return new DashboardLogic(Request).DashboardTable(payload);
        }

        public ActionResult DashboardCharts(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).DashboardCharts(payload);
        }

        public ActionResult AGMResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).AGMTableResult(payload);
        }

        public ActionResult UserResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).UserTableResult(payload);
        }

        public ActionResult BudgetResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).BudgetTableResult(payload);
        }

        public ActionResult BACSECResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).BACSECTableResult(payload);
        }

        public ActionResult HOPEResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).HOPETableResult(payload);
        }
        public ActionResult SupplierResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).SupplierTableResult(payload);
        }

        public ActionResult BACResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).BACTableResult(payload);
        }
        public ActionResult TWGResultView(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).TWGTableResult(payload);
        }

        public ActionResult TWGResultHeaders(DashboardPayloadVM payload)
        {
            return new DashboardLogic(Request).TWGResultHeaders(payload);
        }
    }
}