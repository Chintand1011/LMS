using OPBids.Service.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.Suppliers
{
    public class ProjectDocument : BaseModel
    {
        public int supplier_id { get; set; }
        public int project_id { get; set; }

        [StringLength(255)]
        public string name { get; set; }//document name
    }
}