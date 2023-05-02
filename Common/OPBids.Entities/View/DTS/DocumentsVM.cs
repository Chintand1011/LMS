using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using OPBids.Entities.View.Home;

namespace OPBids.Entities.View.DTS
{
    public class DocumentsVM : BaseVM
    {
        [Display(Name = "Process")]
        public string process { get; set; }
        [Display(Name = "Batch No")]
        public override int id { get; set; }
        [Display(Name = "Category ID")]
        public int category_id { get; set; }
        [Display(Name = "Category")]
        [StringLength(100)]
        public string category_name { get; set; }
        [StringLength(100)]
        public string category_code { get; set; }
        [Display(Name = "Tags")]
        [StringLength(2000)]
        public string tags { get; set; }
        [Display(Name = "Document Type ID")]
        public int document_type_id { get; set; }
        [Display(Name = "Type")]
        [StringLength(75)]
        public string document_type_name { get; set; }
        [Display(Name = "Document Code")]
        [StringLength(20)]
        public string document_code { get; set; }
        [Display(Name = "Sender ID")]
        public int sender_id { get; set; }
        [Display(Name = "Sender")]
        [StringLength(150)]
        public string sender_name { get; set; }
        [Display(Name = "Recipient ID")]
        public int receipient_id { get; set; }
        [Display(Name = "Recipient")]
        [StringLength(150)]
        public string receipient_name { get; set; }
        [Display(Name = "ETD to Recipient")]
        public string etd_to_recipient { get; set; }
        [Display(Name = "Delivery Type ID")]
        public int delivery_type_id { get; set; }
        [Display(Name = "Delivery Type")]
        [StringLength(75)]
        public string delivery_type_name { get; set; }
        [Display(Name = "Document Security Level ID")]
        public int document_security_level_id { get; set; }
        [Display(Name = "Document Security")]
        [StringLength(75)]
        public string document_security_level { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "is Disposable")]
        public bool is_disposable { get; set; }
        [Display(Name = "Is E-Doc")]
        public bool is_edoc { get; set; }
        [Display(Name = "Years - Retention")]
        public int years_retention { get; set; }
        [Display(Name = "Statistics")]
        public int statistics { get; set; }
        [Display(Name = "Statistics1")]
        public int statistics1 { get; set; }
        [Display(Name = "Track Status")]
        [StringLength(1)]
        public string track_status { get; set; }
        [Display(Name = "Sender Deprtment Id")]
        public int sender_department_id { get; set; }
        [Display(Name = "Sender Deprtment Name")]
        public string sender_department_name { get; set; }
        [Display(Name = "Document Stage")]
        public string doc_stage { get; set; }
        [Display(Name = "Department Processed")]
        public int dept_processed { get; set; }
        [Display(Name = "Document Classification")]
        public int document_classification { get; set; }
        [Display(Name = "Record Category")]
        public int record_category { get; set; }
    }
}
