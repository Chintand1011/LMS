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
	public class ProjectCategory: BaseModel
	{

		[Required]
		[StringLength(100)]
	    public string proj_cat { get; set; }

		[Required]
		[StringLength(225)]
		public string proj_desc { get; set; }
		[Required]
		[StringLength(1)]
		public string status { get; set; }
	}
}