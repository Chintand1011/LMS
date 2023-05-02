using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Setting
{
    public class SupplierVM: BaseVM
    {
        [Display(Name = "Supplier User ID")]        
        public int user_id { get; set; }

        [Display(Name = "Supplier First Name")]
        [StringLength(100)]
        public string supp_first_name { get; set; }

        [Display(Name = "Supplier Middle Initial")]
        [StringLength(100)]
        public string supp_mi { get; set; }

        [Display(Name = "Supplier Last Name")]
        [StringLength(120)]
        public string supp_last_name { get; set; }

        [Display(Name = "Contact Person")]
        [StringLength(100)]
        [Required]
        public string contact_person { get; set; }

        [Display(Name = "Company Code")]
        [StringLength(25)]
        [Required]
        public string company_code{ get; set; }

        [Display(Name = "Company Name")]
        [StringLength(100)]
        [Required]
        public string comp_name { get; set; }

        [Display(Name = "Address")]
        [StringLength(150)]
        [Required]
        public string address { get; set; }

        [Display(Name = "E-mail Address")]
        [StringLength(50)]
        [Required]
        public string email { get; set; }

        [Display(Name = "Contact Number")]
        [StringLength(25)]
        [Required]
        public string contact_no { get; set; }

        [Display(Name = "Tax Identification No")]
        [StringLength(50)]
        [Required]
        public string tin { get; set; }

        
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

        [Display(Name = "System ID")]
        public int sys_id { get; set; }
    }
}
