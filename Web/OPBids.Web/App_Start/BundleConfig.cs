using System.Web;
using System.Web.Optimization;

namespace OPBids.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Layout
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Site/wickedpicker.js",
                        "~/Scripts/Shared/FileUploader.js",
                        "~/Scripts/Site/jquery-{version}.js",
                        "~/Scripts/Site/jquery-ui.js",
                        "~/Scripts/Shared/autocomplete.js",
                        "~/Scripts/Site/moment.min.js",
                        "~/Scripts/jquery.tooltipster.min.js",
                        "~/Scripts/Site/popper.min.js",
                        "~/Scripts/Shared/attachment.js",
                        "~/Scripts/Site/selectize.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/PrintBarcodePreview").Include(
                        "~/Scripts/Site/site.js",
                        "~/Scripts/Site/jquery-{version}.js",
                        "~/Scripts/BarcodeReader/GenerateBarcode.js",
                        "~/Scripts/BarcodeReader/GenerateQRcode.js",
                        "~/Scripts/BarcodeReader/PrintBarcodePreview.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Site/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/Site/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Site/bootstrap.js",
                      "~/Scripts/Site/bootstrap-multiselect.js",
                      "~/Scripts/Site/bootstrap-datepicker.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/autocomplete.css",                      
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/filter").Include(
                      "~/Content/tooltipster.css"));

            bundles.Add(new StyleBundle("~/Content/PrintBarcodePreview").Include(
                      "~/Content/PrintBarcodePreview.css"));

            bundles.Add(new StyleBundle("~/Content/commonCss").Include(
                    "~/Content/tooltipster.css",
                    "~/Content/progress-tracker.css",
                    "~/Content/wickedpicker.css",
                      "~/Content/selectize.min.css",
                      "~/Content/jquery-ui.css"));


            bundles.Add(new ScriptBundle("~/bundles/navigationscripts").Include(
                        "~/Scripts/Shared/leftNavigation.js"));

            bundles.Add(new ScriptBundle("~/bundles/appscripts").Include(
                        "~/Scripts/Site/site.js",
                        "~/Scripts/Site/const.js"));
            #endregion


            #region Settings
            bundles.Add(new ScriptBundle("~/bundles/userscripts").Include(
                        "~/Scripts/Setting/user.js"));

            bundles.Add(new ScriptBundle("~/bundles/agpScript").Include(
                        "~/Scripts/Setting/AccessGroupPermission.js"));

            bundles.Add(new ScriptBundle("~/bundles/agScript").Include(
                        "~/Scripts/Setting/AccessGroup.js"));

            bundles.Add(new ScriptBundle("~/bundles/dslScript").Include(
                        "~/Scripts/Setting/DocumentSecurityLevel.js"));

            bundles.Add(new ScriptBundle("~/bundles/bpsScript").Include(
                        "~/Scripts/Setting/BarcodePrintingStatus.js"));

            bundles.Add(new ScriptBundle("~/bundles/bsScript").Include(
                        "~/Scripts/Setting/BarcodeSetting.js"));

            bundles.Add(new ScriptBundle("~/bundles/auScript").Include(
                        "~/Scripts/Setting/AccessUsers.js"));

            bundles.Add(new ScriptBundle("~/bundles/atScript").Include(
                        "~/Scripts/Setting/AccessTypes.js"));

            bundles.Add(new ScriptBundle("~/bundles/srScript").Include(
                    "~/Scripts/jquery.tooltipster.min.js",
                    "~/Scripts/Setting/SenderRecipient.js"));

            bundles.Add(new ScriptBundle("~/bundles/deptScript").Include(
                        "~/Scripts/Setting/Department.js"));

            bundles.Add(new ScriptBundle("~/bundles/docCatScript").Include(
                        "~/Scripts/Setting/DocumentCategory.js"));

            bundles.Add(new ScriptBundle("~/bundles/docTypeScript").Include(
                        "~/Scripts/Setting/DocumentType.js"));

            bundles.Add(new ScriptBundle("~/bundles/delScript").Include(
                        "~/Scripts/Setting/Delivery.js"));

            bundles.Add(new ScriptBundle("~/bundles/supScript").Include(
                        "~/Scripts/Setting/Supplier.js"));

            bundles.Add(new ScriptBundle("~/bundles/contypeScript").Include(
                        "~/Scripts/Setting/ContractType.js"));

            bundles.Add(new ScriptBundle("~/bundles/wfScript").Include(
                        "~/Scripts/Setting/Workflow.js"));

			bundles.Add(new ScriptBundle("~/bundles/ldScript").Include(
                        "~/Scripts/Setting/Loader.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectcatScript").Include(
                        "~/Scripts/Setting/ProjectCat.js"));

            bundles.Add(new ScriptBundle("~/bundles/procmethodScript").Include(
                        "~/Scripts/Setting/ProcMethod.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectstatScript").Include(
                        "~/Scripts/Setting/ProjectStat.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectsubstatScript").Include(
                        "~/Scripts/Setting/ProjectSubStat.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectsubcatScript").Include(
                        "~/Scripts/Setting/ProjectSubCat.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectpropScript").Include(
                        "~/Scripts/Setting/ProjectProp.js"));

            bundles.Add(new ScriptBundle("~/bundles/sourcefundScript").Include(
                        "~/Scripts/Setting/SourceFund.js"));

            bundles.Add(new ScriptBundle("~/bundles/projectareasScript").Include(
                        "~/Scripts/Setting/ProjectAreas.js"));

            bundles.Add(new ScriptBundle("~/bundles/cpScript").Include("~/Scripts/Setting/ChangePassword.js"));

            #endregion


            bundles.Add(new ScriptBundle("~/bundles/announcementScript").Include(
                "~/Scripts/Shared/UserAnnouncement.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/notificationScript").Include("~/Scripts/Shared/UserNotification.js"));

            #region Supplier
            bundles.Add(new ScriptBundle("~/bundles/supplierIndexScript").Include(
            "~/Scripts/Supplier/Index.js"));
            bundles.Add(new ScriptBundle("~/bundles/openForBiddingScript").Include(
                        "~/Scripts/Supplier/OpenForBidding.js"));
            bundles.Add(new ScriptBundle("~/bundles/withSubmittedBidsScript").Include(
                        "~/Scripts/Supplier/WithSbumittedBids.js"));
            bundles.Add(new ScriptBundle("~/bundles/lostOpportunitiesScript").Include(
                        "~/Scripts/Supplier/LostOpportunities.js"));
            #endregion


            #region Project Request

            bundles.Add(new ScriptBundle("~/bundles/projectRequestScript").Include(
                        "~/Scripts/ProjectRequest/ProjectRequest.js"));

            bundles.Add(new ScriptBundle("~/bundles/draftScript").Include(
                        "~/Scripts/ProjectRequest/Draft.js"));

            bundles.Add(new ScriptBundle("~/bundles/projInfoScript").Include(
                        "~/Scripts/ProjectRequest/ProjectInformation.js"));

            bundles.Add(new ScriptBundle("~/bundles/onGoingEndUserScript").Include(
                        "~/Scripts/ProjectRequest/OnGoingEndUser.js"));

            bundles.Add(new ScriptBundle("~/bundles/onGoingBACScript").Include(
                        "~/Scripts/ProjectRequest/OnGoingBAC.js"));

            bundles.Add(new ScriptBundle("~/bundles/onGoingScript").Include(
                        "~/Scripts/ProjectRequest/OnGoing.js"));

            bundles.Add(new ScriptBundle("~/bundles/approvalBudgetScript").Include(
                        "~/Scripts/ProjectRequest/ApprovalBudget.js"));

            bundles.Add(new ScriptBundle("~/bundles/approvalHopeScript").Include(
                        "~/Scripts/ProjectRequest/ApprovalHope.js"));

            bundles.Add(new ScriptBundle("~/bundles/planScript").Include(
                        "~/Scripts/ProjectRequest/Plan.js"));

            bundles.Add(new ScriptBundle("~/bundles/rankingScript").Include(
                        "~/Scripts/ProjectRequest/Ranking.js"));

            bundles.Add(new ScriptBundle("~/bundles/postQualificationScript").Include(
                        "~/Scripts/ProjectRequest/PostQualification.js"));

            bundles.Add(new ScriptBundle("~/bundles/delayedScript").Include(
                        "~/Scripts/ProjectRequest/Delayed.js"));

            bundles.Add(new ScriptBundle("~/bundles/completedScript").Include(
                        "~/Scripts/ProjectRequest/Completed.js"));

            bundles.Add(new ScriptBundle("~/bundles/rejectedScript").Include(
                        "~/Scripts/ProjectRequest/Rejected.js"));

            bundles.Add(new ScriptBundle("~/bundles/rejectedBatchScript").Include(
                        "~/Scripts/ProjectRequest/RejectedBatch.js"));

            bundles.Add(new ScriptBundle("~/bundles/approvedScript").Include(
                        "~/Scripts/ProjectRequest/Approved.js"));

            bundles.Add(new ScriptBundle("~/bundles/monitorScript").Include(
                        "~/Scripts/ProjectRequest/Monitor.js"));

            bundles.Add(new ScriptBundle("~/bundles/incomingScript").Include(
                        "~/Scripts/ProjectRequest/Incoming.js"));
            #endregion


            #region DTS
            bundles.Add(new ScriptBundle("~/bundles/dtsIndexScript").Include(
                        "~/Scripts/Shared/chartjs-plugin-datalabels.js",
                        "~/Scripts/Shared/chartjs-plugin-labels.min.js",
                        "~/Scripts/DTS/Index.js"));
            bundles.Add(new ScriptBundle("~/bundles/settingScript").Include(
                        "~/Scripts/Site/bootstrap-paginator.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsFilterScript").Include(
                        "~/Scripts/jquery.tooltipster.min.js",
                        "~/Scripts/DTS/Filter.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsRequestBarcodeScript").Include(
                        "~/Scripts/DTS/RequestBarcode.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsPrintBarcodeScript").Include(
                        "~/Scripts/DTS/PrintBarcode.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsDashboardScript").Include(
                        "~/Scripts/BarcodeReader/BarcodeReader.js",
                        "~/Scripts/DTS/DTSDashboard.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsOnHandScript").Include(
                        "~/Scripts/BarcodeReader/BarcodeReader.js",
                        "~/Scripts/DTS/OnHand.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsRecievedScript").Include(
                        "~/Scripts/BarcodeReader/BarcodeReader.js",
                        "~/Scripts/DTS/Received.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsFinalizedScript").Include(
                        "~/Scripts/DTS/Finalized.js"));
            bundles.Add(new ScriptBundle("~/bundles/dtsArchivedScript").Include(
                        "~/Scripts/DTS/Archived.js"));
            #endregion

        }
    }
}
