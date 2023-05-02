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
    public class DocumentAttachmentVM : BaseVM
    {
        [Display(Name = "Process")]
        public string process { get; set; }
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Attachment ID")]
        public override int id { get; set; }
        [Display(Name = "Attachment Name")]
        [StringLength(150)]
        public string attachment_name { get; set; }
        [Display(Name = "Barcode No")]
        [StringLength(20)]
        public string barcode_no { get; set; }
        [Display(Name = "File Name")]
        [StringLength(150)]
        public string file_name { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
    }
}
