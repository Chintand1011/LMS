using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OPBids.Service.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPBids.Service.Models.Settings
{
    public class Department: BaseModel
    {
        public int parent_dept_id { get; set; }
        [StringLength(120)]
        [Required]
        [Index(IsUnique = true)]
        public string dept_code { get; set; }
        [StringLength(250)]
        [Required]
        public string dept_description { get; set; }
        public int headed_by { get; set; }
        [StringLength(100)]
        [Required]
        public string designation { get; set; }
        [StringLength(1)]
        public string status { get; set; }

        public bool is_internal { get; set; }
    }
}