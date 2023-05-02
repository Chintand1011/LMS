using System;
using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class ArchivedDocuments : BaseModel
    {
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Category ID")]
        public int category_id { get; set; }
        [Display(Name = "Document Type ID")]
        public int document_type_id { get; set; }
        [Display(Name = "Document Code")]
        [StringLength(40)]
        public string document_code { get; set; }
        [Display(Name = "Tags")]
        [StringLength(2000)]
        public string tags { get; set; }
        [Display(Name = "Sender ID")]
        public int sender_id { get; set; }
        [Display(Name = "Recipient ID")]
        public int receipient_id { get; set; }
        [Display(Name = "ETD to Recipient")]
        public DateTime etd_to_recipient { get; set; }
        [Display(Name = "Delivery Type ID")]
        public int delivery_type_id { get; set; }
        [Display(Name = "Document Security Level ID")]
        public int document_security_level_id { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "is Disposable")]
        public bool is_disposable { get; set; }
        [Display(Name = "Years - Retention")]
        public int years_retention { get; set; }
        [Display(Name = "Is E-Doc")]
        public bool is_edoc { get; set; }
        [Display(Name = "Department Finalized")]
        public int dept_finalized { get; set; }
        [Display(Name = "Date Finalized")]
        public DateTime date_finalized { get; set; }
        [Display(Name = "Department Archived")]
        public int dept_archived { get; set; }
        [Display(Name = "Date Archived")]
        public DateTime date_archived { get; set; }
        [Display(Name = "Document Classification")]
        public int document_classification { get; set; }
        [Display(Name = "Record Category")]
        public int record_category { get; set; }
    }
}
