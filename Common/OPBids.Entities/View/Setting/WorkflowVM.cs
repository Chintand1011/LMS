using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class WorkflowVM : BaseVM
    {
        
        public int type { get; set; }
        [Display(Name = "Type")]
        public string type_name { get; set; }

        [Display(Name = "Sequence Name")]
        [StringLength(10)]
        public string seq_title { get; set; }

        [Display(Name = "Sequence No")]
        public int seq_no { get; set; }

        [Display(Name = "Description")]
        [StringLength(200)]
        public string seq_description { get; set; }

        public int actor { get; set; }

        [Display(Name = "Sequence Actor")]
        public string actor_name { get; set; }

        [Display(Name = "SLA (Days)")]
        public int sla { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}
