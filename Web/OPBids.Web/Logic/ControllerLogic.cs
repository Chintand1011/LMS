using OPBids.Common;
using System.Web.Mvc;
using OPBids.Web.Logic.Setting;
using OPBids.Web.Logic.ProjectRequest;
using OPBids.Web.Logic.Supplier;
using System.Web;
using OPBids.Web.Logic.DTS;
using Microsoft.Owin;
using OPBids.Web.Helper;
using OPBids.Web.Logic.Shared;

namespace OPBids.Web.Logic
{
    public class ControllerLogic : Controller
    {
        public class Shared : Controller
        {
            public DTSLogicBase Logic(string subMenuId)
            {
                switch (subMenuId)
                {
                    case Constant.Menu.Announcement:
                        return new UserAnnouncementLogic();
                    case Constant.Menu.Notifications:
                        return new UserNotificationLogic();
                    default:
                        return new UserAnnouncementLogic();
                }
            }
        }
        public class DTS : Controller
        {
            public DTSLogicBase Logic(string subMenuId)
            {
                switch (subMenuId)
                {
                    case Constant.Menu.Archived:
                        return new ArchivedDocumentsLogic();
                    case Constant.Menu.Received:
                        return new ReceivedDocumentsLogic();
                    case Constant.Menu.Finalized:
                        return new FinalizedDocumentsLogic();
                    case Constant.Menu.OnHand:
                        return new OnHandDocumentsLogic();
                    case Constant.Menu.DashboardDTS:
                        return new DTSDashboardLogic();
                    case Constant.Menu.ReqBarCode:
                        return new RequestBarcodeLogic();
                    case Constant.Menu.PrintBarCode:
                        return new PrintBarcodeLogic();
                    default:
                        return new DTSDashboardLogic();
                }
            }
        }
        public class Setting : Controller
        {
            IOwinContext _context;
            public Setting(IOwinContext httpContext)
            {
                this._context = httpContext;
            }
            public SettingLogicBase Logic(string subMenuId)
            {
                switch (subMenuId)
                {
                    case Constant.Menu.DocumentSecurityLevel:
                        return new DocumentSecurityLevelLogic(_context);
                    case Constant.Menu.BarcodePrintingStatus:
                        return new BarcodePrintingStatusLogic(_context);
                    case Constant.Menu.AccessTypes:
                        return new AccessTypesLogic(_context);
                    case Constant.Menu.RecordCategory:
                        return new RecordCategoryLogic(_context);
                    case Constant.Menu.RecordClassification:
                        return new RecordClassificationLogic(_context);
                    case Constant.Menu.AccessGroups:
                        return new AccessGroupLogic(_context);
                    case Constant.Menu.AccessGroupPermission:
                        return new AccessGroupTypeLogic(_context);
                    case Constant.Menu.AccessUsers:
                        return new AccessUsersLogic(_context);
                    case Constant.Menu.Departments:
                        return new DepartmentLogic(_context);
                    case Constant.Menu.Delivery:
                        return new DeliveryLogic(_context);
                    case Constant.Menu.DocumentCategory:
                        return new DocumentCategoryLogic(_context);
                    case Constant.Menu.DocumentType:
                        return new DocumentTypeLogic(_context);
                    case Constant.Menu.Category:
                        //return new CategoryLogic();
                        return null;                    
                    case Constant.Menu.Supplier:
                        return new SupplierLogic(_context);
                    case Constant.Menu.Workflow:
                        return new WorkflowLogic();
                    case Constant.Menu.BarCode:
                        return new BarcodeSettingLogic(_context);
                    case Constant.Menu.SenderRecipient:
                        return new SenderRecipientLogic(_context);
                    case Constant.Menu.SubCategory:
                        return null;
                    case Constant.Menu.ProcMethod:
                        return new ProcurementMethodLogic(_context);
                    case Constant.Menu.ContractTypes:
                        return new ContractTypeLogic(_context);
                    case Constant.Menu.SourceFunds:
                        return new SourceFundsLogic(_context);
                    case Constant.Menu.ProjProp:
                        return new ProjectProponentLogic(_context);
                    case Constant.Menu.ProjArea:
                        return new ProjectAreasLogic(_context);
                    case Constant.Menu.ProjCategory:
                        return new ProjectCategoryLogic(_context);
                    case Constant.Menu.ProjSubCategory:
                        return new ProjectSubCategoryLogic();
                    case Constant.Menu.ProjSubStatus:
                        return new ProjectSubStatusLogic();
                    case Constant.Setting.Selection.DashboardConfig: 
                        return new DashBoardConfigLogic();
                    case Constant.Menu.ProjStatus:
                        return new ProjectStatusLogic(_context);
                    case Constant.Menu.ProjAreaCity:
                        return new ProjectAreasCityLogic(_context);
                    case Constant.Menu.ProjAreaDist:
                        return new ProjectAreasDistrictLogic(_context);
                    case Constant.Menu.ProjAreaBrgy:
                        return new ProjectAreasBarangayLogic(_context);
                    default:
                        return new AccessTypesLogic(_context);

                }
            }
        }

        public class ProjectRequest : Controller {
            public LogicBase Logic(HttpRequestBase httpContext, string subMenuId) {
                int _group = AuthHelper.GetClaims(httpContext.GetOwinContext(), Constant.Auth.Claims.GroupId).ToSafeInt();
                switch (subMenuId)
                {
                    case Constant.Menu.Draft:
                        return new DraftLogic(httpContext);
                    case Constant.Menu.Ongoing:
                        if (_group == Constant.AccessGroups.EndUser)
                        {
                            return new OnGoingEndUserLogic(httpContext);
                        }
                        else if (_group == Constant.AccessGroups.BAC || _group == Constant.AccessGroups.BACSEC) {
                            return new OnGoingBACLogic(httpContext);
                        }
                        else
                        {
                            return new OnGoingLogic(httpContext);
                        }
                        
                    case Constant.Menu.ForApproval:                        
                        if (_group == Constant.AccessGroups.Budget)
                        {
                            // Budget Group
                            return new ApprovalBudgetLogic(httpContext);
                        }
                        else
                        {
                            // Hope Group
                            return new ApprovalHopeLogic(httpContext);
                        }
                    case Constant.Menu.InProject:
                        return new IncomingLogic(httpContext);
                    case Constant.Menu.ProjectPlans:
                        return new PlanLogic(httpContext);
                    case Constant.Menu.ForRanking:
                        return new RankingLogic(httpContext);
                    case Constant.Menu.ForPostQA:
                        return new PostQualificationLogic(httpContext);
                    case Constant.Menu.Delayed:
                        return new DelayedLogic(httpContext);
                    case Constant.Menu.Completed:
                        return new CompletedLogic(httpContext);
                    case Constant.Menu.Rejected:
                        return new RejectedLogic(httpContext);
                    case Constant.Menu.Approved:
                        return new ApprovedLogic(httpContext);
                    case Constant.Menu.MonProject:
                        return new MonitorLogic(httpContext);
                    default:
                        return null;
                }
            }

            
        }

        public class Supplier : Controller
        {
            public SupplierLogicBase Logic(string subMenuId)
            {
                switch (subMenuId)
                {
                    case Constant.Menu.OpenBid:
                        return new OpenForBiddingLogic();
                    case Constant.Menu.WitSubmittedBids:
                        return new WithSubmittedBidsLogic();
                    case Constant.Menu.LostOpp:
                        return new LostOpportunitiesLogic();
                    default:
                        return null;
                }
            }
        }
    }
}