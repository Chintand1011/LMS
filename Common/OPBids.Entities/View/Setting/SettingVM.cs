using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using OPBids.Entities.View.Home;
using OPBids.Entities.View.Supplier;

namespace OPBids.Entities.View.Setting
{
    public class SettingVM : BaseVM
    {
        public string sub_menu_id { get; set; }
        public string search_key { get; set; }
        public string filename { get; set; }
        public string txn { get; set; }
        public string status { get; set; }
        public int access { get; set; }
        public AccessTypesVM accessTypes { get; set; }
        public AccessGroupVM accessGroup { get; set; }
        public AccessGroupTypeVM[] accessGroupType { get; set; }
        public AccessUsersVM accessUsers { get; set; }
        public DepartmentsVM department { get; set; }
        public DocumentCategoryVM documentCategory { get; set; }
        public SenderRecipientUserVM senderRecipientUser { get; set; }
        public DocumentSecurityLevelVM documentSecurityLevel { get; set; }
        public BarcodePrintingStatusVM barcodePrintingStatus { get; set; }
        public BarcodeSettingVM barcodeSetting { get; set; }
        public DocumentTypeVM documentType { get; set; }
        public string[] item_list { get; set; }
        public DeliveryVM delivery { get; set; }
        public SupplierVM supplier { get; set; }
        public WorkflowVM workflow { get; set; }

        public ProcurementMethodVM procurementmethod { get; set; }
        public ProcurementTypeVM procurementtype { get; set; }
        public CategoryVM category { get; set; }
        public RecordCategoryVM recordCategory { get; set; }
        public RecordClassificationVM recordClassification { get; set; }
        public ContractTypeVM contractype { get; set; }
        public ProjectCategoryVM projectcategory { get; set; }
        public ProjectSubCategoryVM projectsubcategory { get; set; }
        public SourceFundsVM sourcefunds { get; set; }

        public ProjectStatusVM projectstatus { get; set; }
        public ProjectSubStatusVM projectsubstatus { get; set; }
        public ProjectProponentVM projectproponent { get; set; }
        public DashboardConfigVM dashboardconfig { get; set; }
        public ProjectAreasVM projectareas { get; set; }
        public ProjectAreasCityVM projectareascity { get; set; }
        public ProjectAreasDistrictVM projectareasdistrict { get; set; }
        public ProjectAreasBarangayVM projectareasbarangay { get; set; }
    }
}
