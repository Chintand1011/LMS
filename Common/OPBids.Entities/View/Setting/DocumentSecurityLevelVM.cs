using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class DocumentSecurityLevelVM : BaseVM
    {        
        [Display(Name = "Security Level")]
        [StringLength(75)]
        [Required]
        public string code { get; set; }

        [Display(Name = "Definition")]
        [StringLength(150)]
        [Required]
        public string description { get; set; }
    }
}
