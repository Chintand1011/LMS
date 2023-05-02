using OPBids.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace OPBids.Entities.View.Setting
{
    public class AccessGroupTypeVM : BaseVM
    {
        public int? access_group_type_id { get; set; }
        [Display(Name = "Access Type ID")]
        public int? access_type_id { get; set; }
        [Display(Name = "Access Group ID")]
        public int? access_group_id { get; set; }
        [Display(Name = "Access Group")]
        [StringLength(200)]
        public string access_group { get; set; }
        [Display(Name = "Access Code")]
        [StringLength(25)]
        public string code { get; set; }
        [Display(Name = "Controller Action")]
        [StringLength(100)]
        public string controller { get; set; }
        [Display(Name = "CSS Class")]
        [StringLength(100)]
        public string css_class { get; set; }
        [Display(Name = "Access Type")]
        [StringLength(100)]
        public string name { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "View or Transaction")]
        public bool? view_transact_data { get; set; }
        [Display(Name = "Add Or Edit")]
        public bool? add_edit_data { get; set; }
        [Display(Name = "Delete")]
        public bool? delete_data { get; set; }
        [Display(Name = "Records Division")]
        public bool? record_section { get; set; }
        [Display(Name = "Sequence Number")]
        public int? seq_no { get; set; }
        [Display(Name = "Parent ID")]
        public int? parent_id { get; set; }
        [Display(Name = "System Id")]
        public int? sys_id { get; set; }
        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }
        [Display(Name = "Display Menu to Mobile")]
        public bool disp_menu_to_mobile { get; set; }
        [Display(Name = "Icon")]
        [StringLength(50)]
        public string icon { get; set; }
    }
}
