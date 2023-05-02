using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using static OPBids.Common.Enum;

namespace OPBids.Entities.View.Setting
{
    public class ProjectProponentVM: BaseVM
    {
        [Display(Name = "Name")]
        [StringLength(250)]
        [Required]
        public string proponent_name { get; set; }


        [Display(Name = "Designation")]
        [Required]
        [StringLength(75)]
        public string proponent_designation { get; set; }


        [Display(Name = "Dept_Id")]
        [Required]
        public int dept_id { get; set; }

        [Display(Name = "Department")]        
        public string department { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [StringLength(75)]
        public string proponent_emailadd { get; set; }
        
        [Display(Name = "Contact No.")]
        [Required]
        [StringLength(75)]
        public string proponent_contactno { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }


    }
}
