using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.Auth
{
    public class ChangePwdVM
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [Display(Name = "Enter New Password")]
        public string new_pwd { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [Display(Name = "Confirm New Password")]
        public string con_pwd { get; set; }

        public string act_code { get; set; }

        public int id { get; set; }
    }
}
