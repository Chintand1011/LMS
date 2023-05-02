using System.ComponentModel.DataAnnotations;
using OPBids.Service.Models.Base;

namespace OPBids.Entities.View.DTS
{
    public class DocumentRoutes : BaseModel
    {
        [Display(Name = "Batch No")]
        public int batch_id { get; set; }
        [Display(Name = "Department ID")]
        public int department_id { get; set; }
        [Display(Name = "Recipient ID")]
        public int receipient_id { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "Sequence")]
        public int? sequence { get; set; }
        [Display(Name = "CurrentReceiver")]
        public bool current_receiver { get; set; }
        [Display(Name = "Is Sender")]
        public bool is_sender { get; set; }
    }
}
