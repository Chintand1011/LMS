using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPBids.Entities.Base;
using static OPBids.Common.Enum;

namespace OPBids.Entities.View.Setting
{
	public class ProjectSubCategoryVM:BaseVM
	{
		[Display(Name = "Project Category Id")]
		public int proj_catid { get; set; }


		[Display(Name = "SubCategory")]
		[StringLength(100)]
		[Required]
		public string proj_subcat { get; set; }

		[Display(Name = "Definition")]
		[StringLength(225)]
		[Required]
		public string proj_subcatdesc { get; set; }

		[Display(Name = "Status")]
		[StringLength(1)]
		public string status { get; set; }

	}
}
