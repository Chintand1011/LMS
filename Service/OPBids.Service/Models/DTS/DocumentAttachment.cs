using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class DocumentAttachment : BaseModel
    {
        [Display(Name = "Barcode No")]
        [StringLength(20)]
        public string barcode_no { get; set; }
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Attachment Name")]
        [StringLength(150)]
        public string attachment_name { get; set; }
        [Display(Name = "File Name")]
        [StringLength(150)]
        public string file_name { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
