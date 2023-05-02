using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Settings;
using OPBids.Service.Models.ProjectRequest;
using OPBids.Service.Models.Home;
using OPBids.Service.Models.Suppliers;
using OPBids.Entities.View.DTS;
using OPBids.Service.Models.Shared;
using OPBids.Service.Models.ActivityLog;

namespace OPBids.Service.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=OPBidsDatabase")
        {

            ////Uncomment to log generated sql
            this.Database.Log = s => Common.LogHelper.LogInfo(s);
        }

        public DbSet<AccessType> AccessTypes { get; set; }

        public DbSet<AccessGroupType> AccessGroupTypes { get; set; }

        public DbSet<AccessGroup> AccessGroup { get; set; }

        public DbSet<ArchivedDocuments> ArchivedDocuments { get; set; }

        public DbSet<UserAnnouncement> UserAnnouncement { get; set; }

        public DbSet<UserNotification> UserNotification { get; set; }

        public DbSet<OnHandDocuments> OnHandDocuments { get; set; }

        public DbSet<FinalizedDocuments> FinalizedDocuments { get; set; }

        public DbSet<ReceivedDocuments> ReceivedDocuments { get; set; }

        public DbSet<RequestBarcode> RequestBarcodes { get; set; }

        public DbSet<PrintedBarcodes> PrintedBarcodes { get; set; }

        public DbSet<DocumentAttachment> DocumentAttachment { get; set; }

        public DbSet<DocumentLogs> DocumentLogs { get; set; }

        public DbSet<DocumentRoutes> DocumentRoutes { get; set; }

        public DbSet<AccessUser> AccessUser { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<DocumentCategory> DocumentCategory { get; set; }

        public DbSet<RecordCategory> RecordCategory { get; set; }

        public DbSet<RecordClassification> RecordClassification { get; set; }

        public DbSet<DocumentSecurityLevel> DocumentSecurityLevel { get; set; }

        public DbSet<BarcodePrintingStatus> BarcodePrintingStatus { get; set; }

        public DbSet<BarcodeSetting> BarcodeSetting { get; set; }

        public DbSet<DocumentType> DocumentType { get; set; }

        public DbSet<ProjectRequest> ProjectRequests { get; set; }

        public DbSet<Delivery> Delivery { get; set; }

        public DbSet<ProjectRequestHistory> ProjectRequestsHistory { get; set; }

        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Workflow> Workflows { get; set; }

        public DbSet<SenderRecipientUser> SenderRecipientUser { get; set; }

        public DbSet<SenderRecipient> SenderRecipient { get; set; }

        public DbSet<ProcurementMethod> ProcurementMethod { get; set; }

        public DbSet<ProcurementType> ProcurementType { get; set; }

        public DbSet<ContractType> ContractType { get; set; }

        public DbSet<ProjectDocument> ProjectDocuments { get; set; }

        public DbSet<SourceFunds> SourceFunds { get; set; }

        public DbSet<DashboardConfig> DashboardConfigs { get; set; }

        public DbSet<ProjectCategory> ProjectCategory { get; set; }

        public DbSet<ProjectSubCategory> ProjectSubCategory { get; set; }

        public DbSet<ProjectClassification> ProjectClassification { get; set; }

        public DbSet<ProjectStatus> ProjectStatus { get; set; }

        public DbSet<ProjectSubStatus> ProjectSubStatus { get; set; }

        public DbSet<ProjectRequestBatch> ProjectRequestBatch { get; set; }
        public DbSet<ProjectRequestBatchHistory> ProjectRequestBatchHistory { get; set; }

        public DbSet<ProjectRequestAttachment> ProjectRequestAttachments { get; set; }

        public DbSet<ProjectBid> ProjectBid { get; set; }

        public DbSet<ProjectProponent> ProjectProponents { get; set; }

        public DbSet<ProjectBidChecklist> ProjectBidChecklist { get; set; }

        public DbSet<ProjectGrantee> ProjectGrantees { get; set; }

        public DbSet<ProjectRequestItem> ProjectRequestItems { get; set; }

        public DbSet<MonitoredProject> MonitoredProjects { get; set; }

        public DbSet<ProjectAreas> ProjectAreas { get; set; }

        public DbSet<ProjectAreasCity> ProjectAreasCity { get; set; }

        public DbSet<ProjectAreasDistrict> ProjectAreasDistrict { get; set; }

        public DbSet<ProjectAreasBarangay> ProjectAreasBarangay { get; set; }

        public DbSet<ProjectRequestAdvertisement> ProjectRequestAdvertisements { get; set; }

        public DbSet<ActivityLog> ActivityLog { get; set; }
    }
}