using OPBids.Common;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Web.Helper;
using OPBids.Web.Logic;
using OPBids.Web.Logic.ProjectRequest;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class ProjectRequestController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult HeaderView(PayloadVM payload)
        {
            ViewData["project_grantee_filter"] = new CommonLogic(Request).GetProjectGranteeFilter(new PayloadVM());
            ViewData["project_category_filter"] = new CommonLogic(Request).GetProjectCategoryFilter(new PayloadVM());
            return PartialView(new ControllerLogic.ProjectRequest().Logic(Request, payload.sub_menu_id).HeaderView(payload.sub_menu_id));
        }

        public ActionResult ResultView(PayloadVM payload)
        {            
            return new ControllerLogic.ProjectRequest().Logic(Request, payload.sub_menu_id).ResultView(payload);
        }

        public ActionResult GetProjectInformation(PayloadVM payload)
        {
            return new ControllerLogic.ProjectRequest().Logic(Request, payload.sub_menu_id).GetProjectRequestInformation(payload);
        }

        public ActionResult ProjectAttachments(PayloadVM payload)
        {
            return new CommonLogic(Request).ProjectAttachments(payload);

        }

        public ActionResult AddAttachment(ProjectRequestAttachmentVM attachment)
        {
            return new CommonLogic(Request).AddAttachment(attachment);
        }

        #region Ongoing
        public ActionResult GetProjectRequestForBatch(PayloadVM payload)
        {
            return new PlanLogic(Request).GetProjectRequestForBatch(payload);
        }

        public ActionResult AdvertiseProject(PayloadVM payload) {
            return new OnGoingBACLogic(Request).AdvertiseProject(payload);
        }
        #endregion
        
        #region Approval
        public ActionResult GetBatchProjectRequestList(PayloadVM payload)
        {
            return new ApprovalHopeLogic(Request).GetBatchProjectRequestList(payload);
        }
        #endregion

        #region Ranking
        public ActionResult CreateProjectBidChecklists(PayloadVM payload)
        {
            return new RankingLogic(Request).CreateProjectBidChecklists(payload);
        }
        public ActionResult GetProjectBidChecklists(PayloadVM payload) {
            return new RankingLogic(Request).GetProjectBidChecklists(payload);
        }
        #endregion


        public ActionResult ProjectItems(PayloadVM  payload)
        {
            return new CommonLogic(Request).ProjectItems(payload);
        }

        #region Completed
        public ActionResult ProjectInfo(PayloadVM payload)
        {
            return new CommonLogic(Request).ProjectInfo(payload);
        }

        public ActionResult GetBidInvitation(PayloadVM payload)
        {
            return new PlanLogic(Request).GetBidInvitation(payload);
        }
        #endregion


        #region " Batch Advertisements "

        public ActionResult GetProjectRequestBatchAds(PayloadVM payload)
        {
            return new PlanLogic(Request).GetProjectRequestBatchAds(payload);
        }

        #endregion
    }
}