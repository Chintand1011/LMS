using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Suppliers;
using OPBids.Web.Helper;
using OPBids.Web.Logic.ProjectRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Logic.Supplier
{
    public class SuppliersLogic: SupplierLogicBase
    {

        public ActionResult Index()
        {
            return View();
        }
        public override ActionResult PartialView(SupplierPayloadVM param)
        {
            return PartialView(Constant.DTS.PartialView.Received, new List<ProjectRequestVM>());
        }
        public override Result<IEnumerable<T>> Maintain<T>(SupplierPayloadVM param)
        {
            return ProcessData<T>(param, Constant.ServiceEnpoint.SupplierRequest.MaintainOpenForBidding);
        }
        /*
        public ActionResult HeaderView(PayloadVM payload)
        {
            ViewData["project_grantee_filter"] = new CommonLogic(Request).GetProjectGranteeFilter(new PayloadVM());
            ViewData["project_category_filter"] = new CommonLogic(Request).GetProjectCategoryFilter(new PayloadVM());
            return PartialView(new ControllerLogic.Supplier().Logic(Request, payload.sub_menu_id).HeaderView(payload.sub_menu_id));
        }

        public ActionResult ResultView(PayloadVM payload)
        {
            return new ControllerLogic.Supplier().Logic(Request, payload.sub_menu_id).ResultView(payload);
        }
        */
        public ActionResult GetSupplierProjects(SupplierPayloadVM payload)
        {
            Result<IEnumerable<ProjectRequestVM>> _result;

            ApiManager<Result<IEnumerable<ProjectRequestVM>>> apiManager =
                                                         new ApiManager<Result<IEnumerable<ProjectRequestVM>>>();

            _result = apiManager.Invoke(
                                    ConfigManager.BaseServiceURL,
                                    Constant.ServiceEnpoint.SupplierRequest.GetSupplierProjects, payload);

            if (_result.status.code == Constant.Status.Success)
            {
                
            }
            return PartialView("Supplier/ProjectList", _result.value);
        }

    }
}