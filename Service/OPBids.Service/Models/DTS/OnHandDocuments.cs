using System;
using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class OnHandDocuments : BaseModel
    {
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
        [Display(Name = "Is E-Doc")]
        public bool is_edoc { get; set; }
    }
}
