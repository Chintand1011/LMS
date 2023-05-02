using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;
namespace OPBids.Service.Models.Settings
{
    public class ProjectGrantee: BaseModel
    {
        [Required]
        [StringLength(100)]
        public string grantee_code { get; set; }

        [Required]
        [StringLength(225)]
        public string grantee_name { get; set; }
        [Required]
        [StringLength(1)]
        public string status { get; set; }
    }
}