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
	public class ProjectSubCategory:BaseModel
	{
	
		[Required]
		public int proj_catid { get; set; }

		[Required]
		[StringLength(100)]
		public string proj_subcat { get; set; }

		[Required]
		[StringLength(225)]
		public string proj_subcatdesc { get; set; }


		[Required]
		[StringLength(1)]
		public string status { get; set; }
	}
}
