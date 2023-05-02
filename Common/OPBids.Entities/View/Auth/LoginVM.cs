using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.Auth
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "User Name")]
        public string user_name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool persist { get; set; }
    }
}
