using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base; 

namespace OPBids.Entities.View.Setting
{
   public class DeliveryVM:BaseVM
    {
        [Display(Name = "Code")]
        [StringLength(75)]
        public string delivery_code { get; set; }

        [Display(Name = "Definition")]
        [StringLength(150)]
        public string delivery_description { get; set; }

        [Display(Name = "Status")]
        [StringLength(1)]
        public string status { get; set; }

    }
}
