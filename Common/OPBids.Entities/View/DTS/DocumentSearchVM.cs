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
    public class DocumentSearchVM : BaseVM
    {
        [Display(Name = "Date Requested From")]
        public DateTime? date_requested_from { get; set; }
        [Display(Name = "Date Requested To")]
        public DateTime? date_requested_to { get; set; }
        [Display(Name = "Requested By")]
        [StringLength(150)]
        public string requested_by { get; set; }
        [Display(Name = "Request Status")]
        [StringLength(1)]
        public string request_status { get; set; }
        [Display(Name = "Date Submitted From")]
        public DateTime? date_submitted_from { get; set; }
        [Display(Name = "Date Submitted To")]
        public DateTime? date_submitted_to { get; set; }
        [Display(Name = "ETD From")]
        public DateTime? etd_from { get; set; }
        [Display(Name = "ETD To")]
        public DateTime? etd_to { get; set; }
        [Display(Name = "Sender Name")]
        public string sender_name { get; set; }
        [Display(Name = "Recipient Name")]
        public string receipient_name { get; set; }
        [Display(Name = "Category")]
        public string category_name { get; set; }
        [Display(Name = "Type")]
        public string document_type_name { get; set; }
        [Display(Name = "Batch No")]
        public override int id { get; set; }
        [Display(Name = "Barcode No")]
        public string barcode_no { get; set; }
        [Display(Name = "Document Code")]
        [StringLength(40)]
        public string document_code { get; set; }
        [Display(Name = "Department ID")]
        public int department_id { get; set; }

        [Display(Name = "Is Records Division")]
        public bool record_section { get; set; }
    }
}
