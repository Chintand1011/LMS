using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OPBids.Entities.View.Shared
{
    public class MenuList
    {
        [Display(Name = "Menu")]
        public List<Menu> Menus { get; set; }
    }
    
    public class Menu
    {
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Controller and Action")]
        public string ControllerAction { get; set; }

        [Display(Name = "CssClass")]
        public string CssClass { get; set; }

        [Display(Name = "SubMenu")]
        public List<SubMenu> SubMenus { get; set; }

        [Display(Name = "AddOrEdit")]
        public bool AddOrEdit { get; set; }

        [Display(Name = "Delete")]
        public bool Delete { get; set; }

        [Display(Name = "Record_Section")]
        public bool RecordSection { get; set; }

    }
    public class SubMenu
    {
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Controller and Action")]
        public string ControllerAction { get; set; }
        
        [Display(Name = "CssClass")]
        public string CssClass { get; set; }

        [Display(Name = "AddOrEdit")]
        public bool AddOrEdit { get; set; }

        [Display(Name = "Delete")]
        public bool Delete { get; set; }

        [Display(Name = "Record_Section")]
        public bool RecordSection { get; set; }
    }
}