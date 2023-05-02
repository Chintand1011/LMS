using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using OPBids.Entities.View.Suppliers;
using OPBids.Entities.View.Setting;
using OPBids.Common;
using OPBids.Web.Logic.Supplier;
using OPBids.Web.Logic;
using OPBids.Web.Logic.Setting;
using OPBids.Web.Logic.ProjectRequest;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Supplier;
using System.Web;
using OPBids.Web.Logic.Suppliers;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PartialView(SupplierPayloadVM param)
        {
            return new ControllerLogic.Supplier().Logic(param.menu_id).PartialView(param);
        }
        public ActionResult SupplierFilter()
        {
            //ViewData["project_grantee_filter"] = new CommonLogic(Request).GetProjectGranteeFilter(new PayloadVM());
            //ViewData["project_category_filter"] = new CommonLogic(Request).GetProjectCategoryFilter(new PayloadVM());
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainSupplier(SupplierPayloadVM param)
        {
            var rslts = new ControllerLogic.Supplier().Logic(param.menu_id).Maintain<SuppliersVM>(param);
            return Json(rslts);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainProjectAttachments(SupplierPayloadVM param)
        {
            var rslts = new ProjectAttachmentsLogic(Request.GetOwinContext()).Maintain<ProjectRequestAttachmentVM>(param).value;
            rslts = rslts == null ? new List<ProjectRequestAttachmentVM>() : rslts;
            return Json(rslts.OrderBy(a => a.id));
        }
        public ActionResult HeaderView(SupplierPayloadVM payload)
        {
            return PartialView(new Logic.ControllerLogic.ProjectRequest().Logic(Request, payload.menu_id).HeaderView(payload.menu_id));
          //  return PartialView(new Logic.Supplier.SuppliersLogic.GetSupplierProjects


            //return null;
        }

        public ActionResult ResultView(SupplierPayloadVM payload)
        {

            //return new ControllerLogic.ProjectRequest().Logic(Request, payload.sub_menu_id).ResultView(payload);
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetSupplierAccessUser(SettingVM setting)
        {
            return new SupplierLogic(Request.GetOwinContext()).GetSupplierAccessUser(setting);
        }

        #region Plan
        public ActionResult GetOpenBid(SupplierPayloadVM payload)
        {
            //return new PlanLogic(Request).GetProjectRequestForBatch(payload);
            return null;
        }
        #endregion

    }
}
