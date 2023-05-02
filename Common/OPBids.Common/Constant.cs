using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Common
{
    public static class Constant
    {
        public const string DateFormat = "dd-MMM-yyyy";
        public const string DateTimeFormat = "dd-MMM-yyyy HH:mm:ss";
        public const string TimeFormat = "hh:mm tt";
        public const string PleaseSelect = "-- Please Select --";
        public const string DatePickerFormat = "dd-M-yyyy"; //same as dd-MMM-yyyy
        public const string SearchDateFormat = "dd-MMM-yyyy"; //same as dd-MMM-yyyy

        public static class Status {
            public const int Success = 0;
            public const int Failed = 1;
            public const int CannotContinue = 2;
        }

        public static class AccessGroups {
            public const int BAC = 3;
            public const int BACSEC = 4;
            public const int EndUser = 5;
            public const int Budget = 6;
            public const int HoPE = 7;
        }

        public static class AppSettings
        {
            public static int PageItemCount
            {
                get { return (int)(new AppSettingsReader()).GetValue("PageItemCount", typeof(int)); }
            }
            public static string Smtp
            {
                get { return (string)(new AppSettingsReader()).GetValue("Smtp", typeof(string)); }
            }
            public static string UploadFileUrl
            {
                get { return (string)(new AppSettingsReader()).GetValue("UploadFileUrl", typeof(string)); }
            }
            public static string UploadFilePath
            {
                get { return (string)(new AppSettingsReader()).GetValue("UploadFilePath", typeof(string)); }
            }
            public static bool IsEmailSSL
            {
                get { return (string)(new AppSettingsReader()).GetValue("IsEmailSSL", typeof(string)) == "1"; }
            }
            public static bool IsTestEmail
            {
                get { return (string)(new AppSettingsReader()).GetValue("IsTestEmail", typeof(string)) == "1"; }
            }
            public static int EmailPort
            {
                get { return int.Parse((new AppSettingsReader()).GetValue("EmailPort", typeof(string)).ToString()); }
            }
            public static string EmailFrom
            {
                get { return (string)(new AppSettingsReader()).GetValue("EmailFrom", typeof(string)); }
            }
            public static string EmailPassword
            {
                get { return (string)(new AppSettingsReader()).GetValue("EmailPassword", typeof(string)); }
            }
            public static bool RequireSSL
            {
                get { return (string)(new AppSettingsReader()).GetValue("RequireSSL", typeof(string)) == "1"; }
            }
            public static string AppName
            {
                get { return (string)(new AppSettingsReader()).GetValue("AppName", typeof(string)); }
            }
            public static string TestEmailAddress
            {
                get { return (string)(new AppSettingsReader()).GetValue("TestEmailAddress", typeof(string)); }
            }
            public static string VerifyEmailUrl
            {
                get { return (string)(new AppSettingsReader()).GetValue("VerifyEmailUrl", typeof(string)); }
            }
        }

        public static class RecordStatus
        {
            public const string Active = "A";
            public const string MarkForArchive = "M";
            public const string Validated = "V";
            public const string Header = "H";
            public const string Draft = "D";
            public const string Inactive = "I";
            public const string Deleted = "X";
            public const string Cancelled = "C";
            public const string Completed = "F";
            public const string Rejected = "R";
            public const string Approve = "AP";
            public const string Return = "T";
            public const string Received = "RD";
        }

        public static class BidStatus {
            public const string Awarded = "A";
            public const string Passed = "P";
            public const string ForChecking = "FC";
            public const string ForEvaluation = "FE";
            public const string Disqualified = "DQ";
        }

        public static class TransactionType {
            public const string Get = "G";
            public const string GetBid = "GB";
            public const string GetLCB = "GLCB";
            public const string Search = "S";
            public const string DashBoard = "DB";
            public const string IsExists = "IX";
            public const string Track = "T";
            public const string New = "N";
            public const string Save = "CU";
            public const string Create = "C";
            public const string Update = "U";
            public const string UpdateSequence = "US";
            public const string Assign = "A";
            public const string StatusUpdate = "SU";
            public const string Delete = "X";
            public const string Promote = "P";
            public const string GetRoutesBar = "GRB";
            public const string Top5AgingDocuments = "TOP5AGING";
            public const string ProcessUpdate = "PU";
            public const string DownloadList = "DL";
            public const string Read = "R";
            public const string UnRead = "UR";
            public const string Star = "ST";
            public const string UnStar = "UST";
            public const string Hide = "H";
            public const string Withdraw = "W";
            public const string Monitor = "MP";

        }
        public static class UserAction
        {
            public const string Approve = "A";
            public const string Return = "T";
            public const string Reject = "R";
            public const string Received = "RD";
            public const string UpdateImplementationSatus = "US";
        }

        public static class Permission
        {
            public const int Read = 1;
            public const int Write = 2;
        }

        public static class ProcurementMode {
            public const string SVP = "SVP";
            public const string Bidding = "BID";
        }

        public static class WorkflowType {
            public const string Base = "Base";
            public const string SVP = "SVP";
            public const string Bid = "BID";
        }

        public static class ProgressStatus {
            public const string Pending = "";
            public const string Active = "is-active";                        
            public const string Rejected = "is-rejected";
            public const string Complete = "is-complete";
        }

        public static class Setting
        {
            public static class HeaderView {
                public const string RecordCategory = "RecordCategory/RecordCategoryHeader";
                public const string RecordClassification = "RecordClassification/RecordClassificationHeader";
                public const string SenderRecipient = "SenderRecipient/SenderRecipientHeader";
                public const string AccessTypes = "AccessTypes/AccessTypesHeader";
                public const string AccessGroupType = "AccessGroupType/AccessGroupTypeHeader";
                public const string AccessGroup = "AccessGroups/AccessGroupHeader";
                public const string DocumentType = "DocumentType/DocumentTypeHeader";
                public const string AccessUsers = "AccessUsers/AccessUsersHeader";
                public const string Department = "Department/DepartmentHeader";
                public const string DocumentCategory = "DocumentCategory/DocumentCategoryHeader";
                public const string DocumentSecurityLevel = "DocumentSecurityLevel/DocumentSecurityLevelHeader";
                public const string BarcodePrintingStatus = "BarcodePrintingStatus/BarcodePrintingStatusHeader";
                public const string BarcodeSetting = "BarcodeSetting/BarcodeSettingHeader";
                public const string Delivery= "Delivery/DeliveryHeader";
                public const string Supplier = "Supplier/SupplierHeader";
                public const string Workflow = "Workflow/WorkflowHeader";
                public const string ContractType = "ContractType/ContractHeader";
				public const string ProcurementMethod = "ProcurementMethod/ProcurementMethodHeader";
				public const string ProjectCategory = "ProjectCategory/ProjectCategoryHeader";
				public const string ProjectSubCategory = "ProjectSubCategory/ProjectSubCategoryHeader";
                public const string SourceFunds = "SourceFunds/SourceFundsHeader";
				public const string ProjectStatus = "ProjectStatus/ProjectStatusHeader";
                public const string ProjectSubStatus = "ProjectSubStatus/ProjectSubStatusHeader";
                public const string ProjectProponent = "ProjectProponent/ProjectProponentHeader";
                public const string ProjectAreas = "ProjectAreas/ProjectAreasHeader";
            }

            public static class ResultView {
                public const string SenderRecipient = "SenderRecipient/SenderRecipientResult";
                public const string AccessTypes = "AccessTypes/AccessTypesResult";
                public const string AccessGroupType = "AccessGroupType/AccessGroupTypeResult";
                public const string DocumentType = "DocumentType/DocumentTypeResult";
                public const string AccessGroup = "AccessGroups/AccessGroupResult";
                public const string AccessRoles = "AccessRoles/AccessRolesResult";
                public const string AccessUsers = "AccessUsers/AccessUsersResult";
                public const string Department = "Department/DepartmentResult";
                public const string DocumentCategory = "DocumentCategory/DocumentCategoryResult";
                public const string DocumentSecurityLevel = "DocumentSecurityLevel/DocumentSecurityLevelResult";
                public const string BarcodePrintingStatus = "BarcodePrintingStatus/BarcodePrintingStatusResult";
                public const string BarcodeSetting = "BarcodeSetting/BarcodeSettingResult";
                public const string Delivery = "Delivery/DeliveryResult";
                public const string RecordCategory = "RecordCategory/RecordCategoryResult";
                public const string RecordClassification = "RecordClassification/RecordClassificationResult";
                public const string Supplier = "Supplier/SupplierResults";
                public const string Workflow = "Workflow/WorkflowResults";
                public const string ContractType = "ContractType/ContractResult";
				public const string ProcurementMethod = "ProcurementMethod/ProcurementMethodResult";
				public const string ProjectCategory = "ProjectCategory/ProjectCategoryResult";
				public const string ProjectSubCategory = "ProjectSubCategory/ProjectSubCategoryResult";
                public const string SourceFunds = "SourceFunds/SourceFundsResult";
				public const string ProjectStatus = "ProjectStatus/ProjectStatusResult";
                public const string ProjectSubStatus = "ProjectSubStatus/ProjectSubStatusResult";
                public const string ProjectProponent = "ProjectProponent/ProjectProponentResult";
                public const string DashboardConfig = "DashboardConfig/DashboardConfigResult";
                public const string ProjectAreas = "ProjectAreas/ProjectAreasResult";
            }

            public static class Selection {
                public const string SenderRecipient = "SenderRecipient";
                public const string AccessTypes = "AccessTypes";
                public const string AccessGroupType = "AccessGroupType";
                public const string DocumentType = "DocumentType";
                public const string AccessGroup = "AccessGroup";
                public const string AccessRoles = "AccessRoles";
                public const string AccessUsers = "AccessUsers";
                public const string Department = "Department";
                public const string DocumentCategory = "DocumentCategory";
                public const string DocumentSecurityLevel = "DocumentSecurityLevel";
                public const string BarcodePrintingStatus = "BarcodePrintingStatus";
                public const string BarcodeSetting = "BarcodeSetting";
                public const string Delivery = "Delivery";
                public const string Supplier = "Supplier";
                public const string Workflow = "Workflow";
                public const string ContractType = "ContractType";
                public const string ProcurementMethod = "ProcurementMethod";
                public const string ProjectCategory = "ProjectCategory";
                public const string ProjectSubCategory = "ProjectSubCategory";
                public const string ProjectClassification = "ProjectClassification";
                public const string SourceFunds = "SourceFunds";
				public const string ProjectStatus = "ProjectStatus";
                public const string ProjectSubStatus = "ProjectSubStatus";
                public const string ProjectProponent = "ProjectProponent";
                public const string DashboardConfig = "DashBoardConfig";
                public const string ProjectGrantee = "ProjectGrantee";
                public const string ProjectGranteeAuto = "ProjectGranteeAuto";
                public const string ProjectGranteePrevUsed = "ProjectGranteePrevUsed";
                public const string ProjectAreas = "ProjectAreas";
                public const string ProjectAreasCity = "ProjectAreasCity";
                public const string ProjectAreasDistrict = "ProjectAreasDistrict";
                public const string ProjectAreasBarangay = "ProjectAreasBarangay";
            }
        }

        public static class DTS
        {
            public static class PartialView
            {
                public const string Received = "Received";
                public const string OnHand = "OnHand";
                public const string DTSDashboard = "DTSDashboard";
                public const string Finalized = "Finalized";
                public const string Archived = "Archived";
                public const string PrintBarcode = "PrintBarcode";
                public const string RequestBarcode = "RequestBarcode";
            }
        }
        public static class Supplier
        {
            public static class PartialView
            {
                public const string OpenForBidding = "OpenForBidding";
                public const string WithSubmittedBids = "WithSubmittedBids";
                public const string LostOpportunities = "LostOpportunities";
            }
        }
        public static class Shared
        {
            public static class PartialView
            {
                public const string Progress = "~/Views/ProjectRequest/_DisplayTemplates/ProgressBar.cshtml";
            }
        }
        public static class Departments
        {
            public const string DeptCode_SuppDept = "SUPPDEPT";            
        }
        public static class ProjectRequest
        {
            public static class HeaderView {
                public const string Draft = "Draft/DraftHeader";
                public const string OpenForBiddingHeader = "OpenForBidding/OpenForBiddingHeader";

                public const string OnGoing = "OnGoing/OnGoingHeader";
                public const string OnGoingEndUser = "OnGoing/OnGoingEndUserHeader";
                public const string OnGoingBAC = "OnGoing/OnGoingBACHeader";

                public const string Incoming = "Incoming/IncomingHeader";

                public const string ApprovalBudget = "Approval/ApprovalBudgetHeader";
                public const string ApprovalHope = "Approval/ApprovalHopeHeader";

                public const string Approved = "Approved/ApprovedHeader";

                public const string Plan = "Plan/PlanHeader";
                public const string Ranking = "Ranking/RankingHeader";
                public const string PostQualification = "PostQualification/PostQualificationHeader";
                public const string Delayed = "Delayed/DelayedHeader";
                public const string Completed = "Completed/CompletedHeader";
                public const string Rejected = "Rejected/RejectedHeader";
                public const string RejectedBatch = "Rejected/RejectedBatchHeader";

                public const string Monitor = "Monitor/MonitorHeader";
            }

            public static class ResultView {
                public const string Draft = "Draft/DraftResult";
                public const string OpenForBidding = "OpenForBidding/OpenForBiddingResult";

                public const string OnGoing = "OnGoing/OnGoingResult";
                public const string OnGoingEndUser = "OnGoing/OnGoingEndUserResult";
                public const string OnGoingBAC = "OnGoing/OnGoingBACResult";
                public const string OnGoingBidderList = "OnGoing/BidderList";

                public const string Incoming = "Incoming/IncomingResult";

                public const string ApprovalBudget = "Approval/ApprovalBudgetResult";
                public const string ApprovalHope = "Approval/ApprovalHopeResult";

                public const string Approved = "Approved/ApprovedResult";

                public const string Plan = "Plan/PlanResult";
                
                public const string Ranking = "Ranking/RankingResult";
                public const string RankingBidderList = "Ranking/BidderList";
                public const string PostQualification = "PostQualification/PostQualificationResult";
                public const string Delayed = "Delayed/DelayedResult";
                public const string Completed = "Completed/CompletedResult";
                public const string Rejected = "Rejected/RejectedResult";
                public const string RejectedBatch = "Rejected/RejectedBatchResult";

                public const string Monitor = "Monitor/MonitorResult";

                public const string ProjectRequestInformation = "MainModal";
            }

            public static class ProjectStatus
            {
                // DRAFT
                public const string Draft = "PS-1";
                // NEWLY REQUESTED
                public const string NewlyRequested = "PS-2";
                // FOR BUDGET APPROVAL
                public const string ForBudgetApproval = "PS-3";
                // PRE-BIDDING  :   BUDGET APPROVED
                public const string PreBidding_BudgetApproved = "PS-4";
                // PRE-BIDDING  :   PROCUREMENT APPROVAL - APPROVAL OF HOPE
                public const string PreBidding_ProcApproval_Hope = "PS-5";
                // PRE-BIDDING  :   PROCUREMENT APPROVAL - PROC METHOD APPROVED
                public const string PreBidding_ProcApproval_BACSEC = "PS-6";
                // PRE-BIDDING  :   POSTING AND BIDDING
                public const string PreBidding_Posting = "PS-7";
                // PRE-BIDDING  :   CLOSED AND REOPEN
                public const string PreBidding_Close_ReOpen = "PS-8";
                // UNDER TWG QUALIFICATION  :   FOR RANKING
                public const string UnderTWGQualification_Ranking = "PS-9";
                // UNDER TWG QUALIFICATION  :   FOR LCB NOTICE
                public const string UnderTWGQualification_LCB = "PS-10";
                // UNDER TWG QUALIFICATION  :   FOR EVALUATION
                public const string UnderTWGQualification_Eval = "PS-11";
                // UNDER TWG QUALIFICATION  :   POST EVALUATION
                public const string UnderTWGQualification_PostEval = "PS-12";
                // UNDER TWG QUALIFICATION  :   FOR POST QUALIFICATION
                public const string UnderTWGQualification_ForPostQual = "PS-13";
                // UNDER TWG QUALIFICATION  :   POST QUALIFICATION
                public const string UnderTWGQualification_PostQual = "PS-14";
                // PROJECT AWARDING
                public const string ProjectAwarding = "PS-15";
                // PROJECT INSTALLATION
                public const string ProjectInstallation = "PS-16";
                // COMPLETED
                public const string Completed = "PS-17";
            }

            public static class ProjectSubStatus
            {
                // DRAFT
                public const string Draft = "PSS-1.1";
                // WAITING FOR END USER'S PROJECT DOCS
                public const string WaitingEUDocs = "PSS-2.1";
                // BAC-SEC PROJECT VALIDATION
                public const string BACSECProjectValidation = "PSS-2.2";
                // WAITING FOR BAC SEC PROJECT VALIDATED DOCS
                public const string WaitingBACSECValidDocs = "PSS-3.1";
                // FOR BUDGET APPROVAL
                public const string ForBudgetApproval = "PSS-3.2";
                // BUDGET APPROVED - WAITING FOR BUDGET APPROVED DOCS
                public const string BudgetApproved_WaitingBudgetDocs = "PSS-4.1";
                // BUDGET APPROVED - PROC METHOD RECOMMENDATION
                public const string BudgetApproved_ProcMethodRecom = "PSS-4.2";
                // PROCUREMENT APPROVAL - APPROVAL OF HOPE
                public const string ProcApproval_Hope = "PSS-5.1";
                // PROCUREMENT APPROVAL - PROC METHOD APPROVED
                public const string ITB_Preparation = "PSS-6.1";
                // PROCUREMENT APPROVAL - RFQ PREPARATION
                public const string RFQ_Preparation = "PSS-6.5";
                // OPEN FOR BIDDING
                public const string OpenForBidding = "PSS-7.1";
                // ADVERTISEMENT POSTING
                //public const string AdvertisementPosting = "PSS-7.1";
                // OPEN FOR BIDDING - BAC/BACSEC
                //public const string OpenForBidding_BAC = "PSS-7.2";
                // OPEN FOR BIDDING - SUPPLIER
                //public const string OpenForBidding_SUPPLIER = "PSS-7.3";
                // CLOSED FOR SHORTLISTING
                public const string CloseForShortlising = "PSS-8.1";
                // RE-OPEN FOR BIDDING
                public const string ReOpenForBidding = "PSS-8.2";
                // OPENING OF BIDS
                public const string OpeningOfBids = "PSS-8.3";
                // FOR RANKING
                public const string ForRanking = "PSS-9.1";
                // FOR LCB NOTICE
                public const string ForLCBNotice = "PSS-10.1";
                // FOR EVALUATION
                public const string ForEvaluation = "PSS-11.1";
                // POST EVALUATION
                public const string PostEvaluation = "PSS-12.1";
                // FOR POST QUALIFICATION
                public const string ForPostQualification = "PSS-13.1";
                // POST QUALIFICATION
                public const string PostQualification = "PSS-14.1";
                // FOR RECOMMENDATION
                public const string ForRecommendation = "PSS-15.1";
                // FOR AWARDING
                public const string ForAwarding = "PSS-15.2";
                // UNDER IMPLEMENTATION  - Initial Installation
                public const string UnderImplementation_Initial = "PSS-16.1";
                // UNDER IMPLEMENTATION - 40%
                public const string UnderImplementation_40Percent = "PSS-16.2";
                // UNDER IMPLEMENTATION - 80%
                public const string UnderImplementation_80Percent = "PSS-16.3";
                // COMPLETED
                public const string Completed = "PSS-17.1";

            }

            // TODO: Delete this
            //public static class Process {
            //    public const string Draft = "PROC-010";
            //    public const string UpdateRequest = "PROC-020";
            //    public const string ForBudgetApproval = "PROC-030";
            //    public const string ProjectConsolidation_Project = "PROC-040";
            //    public const string ProjectConsolidation_PPMP = "PROC-041";
            //    public const string ApprovalOfProcMethod = "PROC-050";
            //    public const string ProcMethodApproved = "PROC-060";
            //    public const string OpenForBidding_Batch = "PROC-070";
            //    public const string OpenForBidding_Project = "PROC-071";
            //    public const string OpenForBidding_Supplier = "PROC-072";


            //    //TODO:
            //    public const string ForRecommendation = "PROC-XXX";
            //    public const string UnderImplementation = "PROC-XXX";
            //}
        }

        public static class Auth {
            public static class Claims {
                public const string UserId = "user_id";
                public const string UserName = "UserName";
                public const string FullName = "full_name";
                public const string GroupId = "group_id";
                public const string Email = "email";
                public const string DeptId = "dept_id";
                public const string DeptCode = "dept_code";
                public const string Department = "dept_description";
                public const string DashboardId = "dashboard_id";
                public const string PfmsAccess = "pfms_access";
                public const string DtsAccess = "dts_access";
                public const string CurrentProduct = "CurrentProduct";
                public const string VIP = "VIP";
            }
        }

        public static class ServiceEnpoint
        {
            public static class DTS
            {
                public const string MaintainReceived = "service/MaintainReceivedDocuments";
                public const string MaintainRequestBarcode = "service/MaintainRequestBarcode";
                public const string DTSDashboard = "service/DTSDashboard";
                public const string MaintainOnHand = "service/MaintainOnHandDocuments";
                public const string MaintainUserAnnouncement = "service/MaintainUserAnnouncement";
                public const string MaintainUserNotification = "service/MaintainUserNotification";
                public const string MaintainFinalized = "service/MaintainFinalizedDocuments";
                public const string MaintainArchived = "service/MaintainArchivedDocuments";
                public const string MaintainDocumentLogs = "service/MaintainDocumentLogs";
                public const string MaintainDocumentAttachment = "service/MaintainDocumentAttachments";
                public const string MaintainDocumentRoutes = "service/MaintainDocumentRoutes";
            }
            public static class Settings {
                public const string GetRecordCategory = "service/GetRecordCategory";
                public const string CreateRecordCategory = "service/CreateRecordCategory";
                public const string UpdateRecordCategory = "service/UpdateRecordCategory";

                public const string GetRecordClassification = "service/GetRecordClassification";
                public const string CreateRecordClassification = "service/CreateRecordClassification";
                public const string UpdateRecordClassification = "service/UpdateRecordClassification";

                public const string GetAccessGroupType = "service/GetAccessGroupType";
                public const string GetAccessGroupMenu = "service/GetAccessGroupMenu";
                public const string SaveAccessGroupType = "service/SaveAccessGroupType";

                public const string GetSenderRecipient = "service/GetSenderRecipient";
                public const string CreateSenderRecipient = "service/CreateSenderRecipient";
                public const string UpdateSenderRecipient = "service/UpdateSenderRecipient";
                public const string UpdateSenderRecipientStatus = "service/UpdateSenderRecipientStatus";

                public const string GetDocumentSecurityLevel = "service/GetDocumentSecurityLevel";
                public const string CreateDocumentSecurityLevel = "service/CreateDocumentSecurityLevel";
                public const string UpdateDocumentSecurityLevel = "service/UpdateDocumentSecurityLevel";

                public const string GetBarcodePrintingStatus = "service/GetBarcodePrintingStatus";
                public const string CreateBarcodePrintingStatus = "service/CreateBarcodePrintingStatus";
                public const string UpdateBarcodePrintingStatus = "service/UpdateBarcodePrintingStatus";

                public const string GetBarcodeSetting = "service/GetBarcodeSetting";
                public const string CreateBarcodeSetting = "service/CreateBarcodeSetting";
                public const string UpdateBarcodeSetting = "service/UpdateBarcodeSetting";

                public const string GetDocumentType = "service/GetDocumentType";
                public const string CreateDocumentType = "service/CreateDocumentType";
                public const string UpdateDocumentType = "service/UpdateDocumentType";
                public const string StatusUpdateDocumentType = "service/StatusUpdateDocumentType";

                public const string GetDelivery = "service/GetDelivery";
                public const string CreateDelivery = "service/CreateDelivery";
                public const string UpdateDelivery = "service/UpdateDelivery";
                public const string StatusUpdateDelivery = "service/StatusUpdateDelivery";

                public const string GetDocumentCategory = "service/GetDocumentCategory";
                public const string CreateDocumentCategory = "service/CreateDocumentCategory";
                public const string UpdateDocumentCategory = "service/UpdateDocumentCategory";
                public const string StatusUpdateDocumentCategory = "service/StatusUpdateDocumentCategory";

                public const string GetAccessUser = "service/GetAccessUser";
                public const string CreateAccessUser = "service/CreateAccessUser";
                public const string UpdateAccessUser = "service/UpdateAccessUser";                
                public const string StatusUpdateAccessUser = "service/StatusUpdateAccessUser";
                public const string ResetAccessUserPassword = "service/ResetAccessUserPassword";

                public const string IsExistDepartments = "service/IsExistDepartments";
                public const string GetDepartments = "service/GetDepartments";
                public const string GetDepartmentsToAssign = "service/GetDepartmentsToAssign";
                public const string CreateDepartments = "service/CreateDepartments";
                public const string UpdateDepartments = "service/UpdateDepartments";
                public const string StatusUpdateDepartments = "service/StatusUpdateDepartments";
                public const string AssignSubDepartments = "service/AssignSubDepartments";

                public const string GetAccessType = "service/GetAccessType";
                public const string CreateAccessType = "service/CreateAccessType";
                public const string UpdateAccessType = "service/UpdateAccessType";
                public const string StatusUpdateAccessType = "service/StatusUpdateAccessType";

                public const string GetAccessGroup = "service/GetAccessGroup";
                public const string CreateAccessGroup = "service/CreateAccessGroup";
                public const string UpdateAccessGroup = "service/UpdateAccessGroup";
                public const string StatusUpdateAccessGroup = "service/StatusUpdateAccessGroup";

                public const string CreateAccessGroupType = "service/CreateAccessGroupType";
                public const string UpdateAccessGroupType = "service/UpdateAccessGroupType";
                public const string StatusUpdateAccessGroupType = "service/StatusUpdateAccessGroupType";

                public const string CreateSupplier = "service/CreateSupplier";
                public const string UpdateSupplier = "service/UpdateSupplier";
                public const string StatusUpdateSupplier = "service/StatusUpdateSupplier";
                public const string GetSupplier= "service/GetSupplier";
                public const string GetSupplierAccessUser = "service/GetSupplierAccessUser";

                public const string CreateCategory = "service/CreateCategory";
                public const string UpdateCategory = "service/UpdateCategory";
                public const string StatusUpdateCategory = "service/StatusUpdateCategory";

                public const string GetWorkflow = "service/GetWorkflow";
                public const string CreateWorkflow = "service/CreateWorkflow";
                public const string UpdateWorkflow = "service/UpdateWorkflow";
                public const string StatusUpdateWorkflow = "service/StatusUpdateWorkflow";

                public const string CreateContractType = "service/CreateContractType";
                public const string UpdateContractType = "service/UpdateContractType";
                public const string StatusUpdateContractType = "service/StatusUpdateContractType";
                public const string GetContractType = "service/GetContractType";

				public const string GetProjectCategory = "service/GetProjectCategory";
				public const string CreateProjectCategory = "service/CreateProjectCategory";
				public const string UpdateProjectCategory = "service/UpdateProjectCategory";
				public const string StatusUpdateProjectCategory = "service/StatusUpdateProjectCategory";
                

				public const string GetProjectSubCategory = "service/GetProjectSubCategory";
				public const string CreateProjectSubCategory = "service/CreateProjectSubCategory";
				public const string UpdateProjectSubCategory = "service/UpdateProjectSubCategory";
				public const string StatusUpdateProjectSubCategory = "service/StatusUpdateProjectSubCategory";

                
                public const string GetProcurementMethod = "service/GetProcurementMethod";
				public const string CreateProcurementMethod = "service/CreateProcurementMethod";
				public const string UpdateProcurementMethod = "service/UpdateProcurementMethod";
				public const string StatusUpdateProcurementMethod = "service/StatusUpdateProcurementMethod";

                public const string GetSourceFunds = "service/GetSourceFunds";
                public const string CreateSourceFunds = "service/CreateSourceFunds";
                public const string UpdateSourceFunds = "service/UpdateSourceFunds";
                public const string StatusUpdateSourceFunds = "service/StatusUpdateSourceFunds";

                public const string GetProjectStatus = "service/GetProjectStatus";
                public const string CreateProjectStatus = "service/CreateProjectStatus";
                public const string UpdateProjectStatus = "service/UpdateProjectStatus";
                public const string StatusUpdateProjectStatus = "service/StatusUpdateProjectStatus";

                public const string GetProjectSubStatus = "service/GetProjectSubStatus";
                public const string CreateProjectSubStatus = "service/CreateProjectSubStatus";
                public const string UpdateProjectSubStatus = "service/UpdateProjectSubStatus";
                public const string StatusUpdateProjectSubStatus = "service/StatusUpdateProjectSubStatus";
                //public const string CreateSourceFunds = "service/CreateSourceFunds";
                //public const string UpdateSourceFunds = "service/UpdateSourceFunds";
                //public const string StatusUpdateSourceFunds = "service/StatusUpdateSourceFunds";

                public const string GetProjectProponent = "service/GetProjectProponent";
                public const string CreateProjectProponent = "service/CreateProjectProponent";
                public const string UpdateProjectProponent = "service/UpdateProjectProponent";
                public const string StatusUpdateProjectProponent = "service/StatusUpdateProjectProponent";

                public const string GetDashBoardConfig = "service/GetDashBoardConfig";

                public const string GetProjectAreas = "service/GetProjectAreas";
                public const string CreateProjectAreas = "service/CreateProjectAreas";
                public const string UpdateProjectAreas = "service/UpdateProjectAreas";
                public const string StatusUpdateProjectAreas = "service/StatusUpdateProjectAreas";
                public const string GetAndSaveProjectAreas = "service/GetAndSaveProjectAreas";

                public const string GetProjectAreasCity = "service/GetProjectAreasCity";
                public const string CreateProjectAreasCity = "service/CreateProjectAreasCity";
                public const string UpdateProjectAreasCity = "service/UpdateProjectAreasCity";
                public const string StatusUpdateProjectAreasCity = "service/StatusUpdateProjectAreasCity";
                public const string GetAndSaveProjectAreasCity = "service/GetAndSaveProjectAreasCity";

                public const string GetProjectAreasDistrict = "service/GetProjectAreasDistrict";
                public const string CreateProjectAreasDistrict = "service/CreateProjectAreasDistrict";
                public const string UpdateProjectAreasDistrict = "service/UpdateProjectAreasDistrict";
                public const string StatusUpdateProjectAreasDistrict = "service/StatusUpdateProjectAreasDistrict";
                public const string GetAndSaveProjectAreasDistrict = "service/GetAndSaveProjectAreasDistrict";
                public const string GetProjectAreasDistrictByCity = "service/GetProjectAreasDistrictByCity";

                public const string GetProjectAreasBarangay = "service/GetProjectAreasBarangay";
                public const string CreateProjectAreasBarangay = "service/CreateProjectAreasBarangay";
                public const string UpdateProjectAreasBarangay = "service/UpdateProjectAreasBarangay";
                public const string StatusUpdateProjectAreasBarangay = "service/StatusUpdateProjectAreasBarangay";
                public const string GetAndSaveProjectAreasBarangay = "service/GetAndSaveProjectAreasBarangay";
            }

			public static class ProjectRequest {
                
                public const string GetProjectRequest = "service/GetProjectRequest";
                public const string GetProjectRequestInformation = "service/GetProjectRequestInformation";
                public const string CreateProjectRequest = "service/CreateProjectRequest";
                public const string UpdateProjectRequest = "service/UpdateProjectRequest";
                public const string UpdateRecordStatus = "service/UpdateProjectRequestRecordStatus";
                public const string UpdateProjectStatus = "service/UpdateProjectRequestProjectStatus";

                public const string GetProjectRequestBatch = "service/GetProjectRequestBatch";
                public const string GetProjectRequestListForBatch = "service/GetProjectRequestListForBatch";
                public const string CreateProjectRequestBatch = "service/CreateProjectRequestBatch";
                public const string UpdateProjectRequestBatch = "service/UpdateProjectRequestBatch";
                public const string UpdateBatchRecordStatus = "service/UpdateBatchRecordStatus";
                public const string UpdateBatchProjectStatus = "service/UpdateBatchProjectStatus";

                public const string GetProjectBid = "service/GetProjectBid";                
                public const string GetProjectBidChecklists = "service/GetProjectBidChecklists";
                public const string CreateProjectBidChecklists = "service/CreateProjectBidChecklists";
                public const string GetLowestCalculatedBid = "service/GetLowestCalculatedBid";

                public const string GetProjectRequestAttachments = "service/GetProjectRequestAttachments";
                public const string CreateProjectRequestAttachment = "service/CreateProjectRequestAttachment";
                public const string SearchProjectRequest = "service/SearchProjectRequest";
                public const string SearchProjectRequestBatch = "service/SearchProjectRequestBatch";
                public const string GetProjectGranteesFilter = "service/GetProjectGranteesFilter";
                public const string GetProjectCategoriesFilter = "service/GetProjectCategoriesFilter";
                public const string GetProjectRequestItems = "service/GetProjectRequestItems";
                public const string GetProjectInfo = "service/GetProjectInfo";

                public const string MonitorProject = "service/MonitoredProject";
                public const string SearchMonitorProject = "service/SearchMonitorProject";

                public const string UpdateProjectRequestAdvertisement = "service/UpdateProjectRequestAdvertisement";
                public const string GetProjectRequestAdvertisement = "service/GetProjectRequestAdvertisement";
                public const string GetProjectRequestBatchAdvertisements = "service/GetProjectRequestBatchAdvertisement";
            }

            public static class DashboardRequest
            {
                public const string GetSummaryHeader = "service/GetSummaryHeader";
                public const string GetDashboardTable = "service/GetDashboardTable";
                public const string GetDashboardCharts = "service/GetDashboardCharts";
                public const string GetTWGResultHeaders = "service/GetTWGResultHeaders";
                public const string GetDashboardTableEndUser = "service/GetDashboardTableEndUser";
                public const string GetDashboardTableBac = "service/GetDashboardTableBac";
                public const string GetDashboardTableBacSec = "service/GetDashboardTableBacSec";
                public const string GetDashboardTableBudget = "service/GetDashboardTableBudget";
                public const string GetDashboardTableHope = "service/GetDashboardTableHope";
                public const string GetDashboardTableTWG = "service/GetDashboardTableTWG";
                public const string GetDashboardTableSupplier = "service/GetDashboardTableSupplier";
                public const string GetDashboardTableAGM = "service/GetDashboardTableAGM";
            }

            public static class SupplierRequest
            {
                public const string GetSupplierProjects = "service/GetSupplierProjects";
                public const string MaintainOpenForBidding = "service/MaintainOpenForBidding";
                public const string MaintainWithSubmittedBids = "service/MaintainWithSubmittedBids";
                public const string MaintainLostOpportunities = "service/MaintainLostOpportunities";
                public const string MaintainProjectAttachments = "service/MaintainProjectAttachments";
            }

            public static class SharedRequest
            {
                public const string GetProjectTotal = "service/GetProjectTotal";
                public const string GetDocumentTotal = "service/GetDocumentTotal";
                public const string GetSettingsList = "service/GetSettingsList";

                public const string CheckProjectRequestDocument = "service/CheckProjectRequestDocument";
                public const string ReceiveProjectRequestDocument = "ReceiveProjectRequestDocument";

                public const string GetProjectLogs = "service/GetProjectLogs";
                public const string GetProjectProgress = "service/GetProjectProgress";
            }
        }

        public static class Menu {
            public const string Dashboard = "P-DASH";
            public const string SenderRecipient = "P-SET-SENDRECIPIENT";
            public const string Draft = "P-DRAFT";
            public const string Announcement = "D-ANN";
            public const string Notifications = "P-NOT";
            public const string Reports = "P-RPT";
            public const string DTSReports = "D-RPT";
            public const string Settings = "P-SET";
            public const string Signout = "P-EXIT";

            //DTS
            public const string DashboardDTS = "D-DASH";
            public const string ReqBarCode = "D-REQBARCODE";
            public const string OnHand = "D-ONHAND";
            public const string Archived = "D-ARCHIVED";
            public const string Received = "D-RECEIVED";
            public const string Finalized = "D-FINALIZED";
            public const string RecSection = "D-RECSECTION";
            public const string PrintBarCode = "D-PRINTBARCODE";
            public const string BarcodePrintingStatus = "D-BARCODEPRINTSTATUS";
            public const string DocumentSecurityLevel = "D-DOCUMENTSECURITYLEVEL";
            
            //General Reports
            public const string RepSenderStat = "R-SENDERSTAT";
            public const string RepRecStat = "R-RECSTAT";
            public const string RepForArcDoc = "R-FORARCDOC";
            public const string RepCanDoc = "R-CANCELLEDDOC";

            //Analytical Reports
             public const string RepSumAnalytics = "R-SUMANALYTICS";
            public const string RepComSumAnalytics = "R-COMANALYTICS";
            public const string RepGrpAnalytics = "R-GRPANALYTICS";
            public const string RepComGrpAnalytics = "R-COMGRPANALYTICS";

            //PFMS
            public const string OpenBid = "P-OPENBID";
            public const string ForRanking = "P-FORRANKING";
            public const string ForPostQA = "P-POSTQA";
            public const string Summary= "P-Summary";
            public const string Approved = "P-APPROVED";
            public const string ForApproval = "P-FAPPROVAL";
            public const string Rejected = "P-REJECTED";
            public const string Completed = "P-COMPLETED";
            public const string Ongoing = "P-ONGOING";
            public const string WitSubmittedBids = "P-WITHSUBMITTEDBIDS";
            public const string LostOpp = "P-LOSTOPP";
            public const string Drafts = "P-DRAFTS";
            public const string Delayed = "P-DELAYED";
            public const string InProject = "P-INPROJ";
            public const string MonProject = "P-MONPROJ";
            public const string ProjectPlans = "P-PROJECTPLAN";

            public const string ProjectMonitoring = "P-PROJMONITOR";




            // PFMS Settings SubMenu 
  
            public const string DTSSettings = "D-SETTING";
            public const string AccessTypes = "P-SET-ACCTYPE";
            public const string AccessGroups = "P-SET-ACCGRP";
            public const string AccessUsers = "P-SET-ACCUSER";
            public const string Departments = "P-SET-DEPT";
            public const string Delivery = "P-SET-DEL";
            public const string DocumentCategory = "P-SET-DOCCAT";
            public const string DocumentType = "P-SET-DOCTYP";
            public const string Category = "P-SET-CAT";
            public const string RecordCategory = "P-SET-RECCAT";
            public const string RecordClassification = "P-SET-RECCLASS";
            public const string Supplier = "P-SET-SUP";
            public const string Workflow = "P-SET-WFLOW";
            public const string BarCode= "D-SET-BARCODE";
            public const string AccessGroupPermission = "P-SET-ACCGRPPER";

            public const string PFMSSettings = "P-SETTING";
            public const string SubCategory = "P-SET-SUBCAT";
            public const string ProcMethod = "P-SET-PROCMETHOD";
            public const string ContractTypes = "P-SET-CONTYPE";
            public const string SourceFunds = "P-SET-SOURCEFUNDS";
            public const string ProjSubStatus = "P-SET-PROJSUBSTAT";
            public const string ProjStatus = "P-OVERSTAT";
            public const string ProjProp = "P-SET-PROJPROP";
            public const string ProjArea = "P-SET-PROJAREA";
            public const string ProjCategory = "P-SET-PROJCAT";
            //public const string ProjStatus = "P-OVERSTAT";
            public const string ProjSubCategory = "P-SET-PROJSUBCAT";
            public const string ProjAreaCity = "P-SET-PROJAREACITY";
            public const string ProjAreaDist = "P-SET-PROJAREADIST";
            public const string ProjAreaBrgy = "P-SET-PROJAREABRGY";

        }

        public static class SupplierProject
        {
            public const string Awarded = "A";
            public const string Bidding = "B";
            public const string Cancelled = "C";
            public const string Lost = "L";
        }

        public static class ProjectAreaGetAndSave
        {
            public const int NewID = 0;
        }

        public static class Dashboard {
            public static class SummaryItem {

                #region SUMMARY TABLE 1
                public const string Completed = "DSI-COMPLETED";
                public const string OnGoing = "DSI-ONGOING";
                public const string Delayed = "DSI-DELAYED";
                public const string Drafts = "DSI-DRAFTS";

                public const string Approved = "DSI-APPROVED";
                public const string ForApproval = "DSI-FORAPP";
                public const string Rejected = "DSI-REJ";

                public const string Recommended = "DSI-RECOM";
                public const string Incoming = "DSI-INCOMING";
                #endregion

                #region SUMMARY TABLE 2
                public const string UnderTWG = "DSI-UTWG";
                public const string PreBid = "DSI-PREBID";
                public const string BudgetApproval = "DSI-BUDAPP";
                public const string NewlyRequest = "DSI-NEWREQ";

                public const string Past10Days = "DSI-PAST10";
                public const string Days10Below = "DSI-10BELOW";
                public const string NewlySubmitted = "DSI-NEWLYSUB";

                public const string Past21Days = "DSI-21DAYS";
                public const string Between1321 = "DSI-13-21";
                public const string Less13 = "DSI-LESS13";

                public const string UnderImplementation = "DSI-UNIMP";
                public const string BACResolution = "DSI-BACRESO";
                public const string AwardByTWG = "DSI-AWTWG";

                public const string Past30Days = "DSI-PAST30";
                public const string Between1530 = "DSI-15-30";
                public const string Less15 = "DSI-LESS15";
                #endregion

                #region SUMMARY TABLE 3
                public const string Top1 = "DSI-TOP1";
                public const string Top2 = "DSI-TOP2";
                public const string Top3 = "DSI-TOP3";

                public const string Past17Days = "DSI-PAST17";
                public const string Between1017 = "DSI-10-17";
                public const string Less10 = "DSI-LESS10";

                public const string Missed = "DSI-MISSED";
                public const string Unawarded = "DSI-UNAWARD";
                #endregion

                #region SHARED TABLE
                public const string Above80 = "DSI-ABOVE80";
                public const string Between5080 = "DSI-50-80"; //51-80
                public const string Less50 = "DSI-LESS50";
                public const string UILess10 = "DSI-UILESS10";
                #endregion
                
            }
        }
    }
}
