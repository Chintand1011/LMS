using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class RecordClassificationVM : BaseVM
    {
        [Display(Name = "Classification Code")]
        [StringLength(30)]
        public string classification_code { get; set; }

        [Display(Name = "Classification Description")]
        [StringLength(100)]
        public string classification_desc { get; set; }
    }
}
