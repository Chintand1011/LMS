using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class DepartmentsVM:BaseVM
    {
        [Display(Name = "Parent Department")]
        public int parent_dept_id { get; set; }
        [StringLength(120)]
        [Required]
        [Display(Name = "Department Code")]
        public string dept_code { get; set; }
        [StringLength(250)]
        [Required]
        [Display(Name = "Definition")]
        public string dept_description { get; set; }
        [Display(Name = "Headed By Id")]
        public int headed_by { get; set; }
        [Display(Name = "Headed By")]
        public string headed_by_name { get; set; }
        [StringLength(100)]
        [Required]
        [Display(Name = "Designation")]
        public string designation { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

        [Display(Name = "Internal Dept.")]
         public bool is_internal { get; set; }
        
    }
}
