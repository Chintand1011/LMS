using OPBids.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectRequestVM : BaseVM
    {
        [Display(Name = "Project Title")]
        [StringLength(100)]
        [MaxLength(100)]
        public string title { get; set; }

        [Display(Name = "Project Description")]
        [StringLength(1000)]
        [MaxLength(1000)]
        public string description { get; set; }

        [Display(Name = "Project Grantee")]
        [StringLength(100)]
        [MaxLength(100)]
        public string grantee { get; set; }
        public string grantee_name { get; set; }

        [Display(Name = "Estimated Budget")]
        [StringLength(15)]
        public string estimated_budget { get; set; }

        [Display(Name = "Approved Budget")]
        [StringLength(15)]
        public string approved_budget { get; set; }

        [Display(Name = "Required Date")]
        public string required_date { get; set; }

        [Display(Name = "Category")]
        [StringLength(15)]
        public string category { get; set; }
        public string category_desc { get; set; }


        [Display(Name = "Classification")]
        [StringLength(5)]
        public string classification { get; set; }
        public string classification_desc { get; set; }

        [Display(Name = "Type of Contract")]
        [StringLength(5)]
        public string contract_type { get; set; }
        public string contract_type_desc { get; set; }

        [Display(Name = "Secutity Level")]
        public string security_level { get; set; }
        public string security_level_desc { get; set; }

        [Display(Name = "Delivery Type")]
        public string delivery_type { get; set; }
        public string delivery_type_desc { get; set; }

        [Display(Name = "Earmark No")]
        [StringLength(20)]
        [MaxLength(20)]
        public string earmark { get; set; }

        [Display(Name = "Issuance Date")]
        public string earmark_date { get; set; }

        [Display(Name = "Source of Fund")]
        [StringLength(5)]
        public string source_fund { get; set; }
        public string source_fund_desc { get; set; }

        public int batch_id { get; set; }

        [StringLength(10)]
        public string project_status { get; set; }
        public string project_status_desc { get; set; }

        [StringLength(10)]
        public string project_substatus { get; set; }
        public string project_substatus_desc { get; set; }

        [StringLength(1)]
        public string record_status { get; set; }

        public int sla { get; set; }

        public int current_user { get; set; }

        [StringLength(200)]
        public string user_action { get; set; }

        [StringLength(1000)]
        [MaxLength(1000)]
        public string notes { get; set; }

        public string routed_date { get; set; }

        public bool isEditable { get; set; }

        public int project_duration { get; set; }
        public int bid_bond { get; set; }

        // For current user's group_id
        public string session_group_id { get; set; }

        public string imp_perc_status { get; set; }

        public string procurement_method { get; set; }
        public string bid_opening_date { get; set; }
        public string bid_opening_place { get; set; }

        public string endUser { get; set; }
        public string departmentName { get; set; }
        public string bidderName { get; set; }
        public string supplierAddr { get; set; }
        
        public List<ProjectRequestAttachmentVM> attachments { get; set; }

        public List<ProjectRequestItemVM> project_items { get; set; }

        public int index { get; set; }

        [Display(Name = "P.R.#")]
        public string pr_number { get; set; }

        public string submitted_date { get; set; }
        public bool isMonitored { get; set; }

        [Display(Name = "Deadline")]
        [StringLength(11)]
        public string rfq_deadline { get; set; }

        [Display(Name = "Place")]
        [StringLength(200)]
        public string rfq_place { get; set; }

        [Display(Name = "Requested By")]
        [StringLength(50)]
        public string rfq_requestor { get; set; }

        [StringLength(50)]
        public string rfq_requestor_dept { get; set; }

        [Display(Name = "Requested Date")]
        [StringLength(11)]
        public string rfq_request_date { get; set; }

        public int days_to_required_date { get; set; }
        public string start_date { get; set; }
        public string completed_date { get; set; }
    }
}
