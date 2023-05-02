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
	public class ProjectCategoryVM:BaseVM
	{

		[Display(Name = "Category Code")]
		[StringLength(100)]
		[Required]
		public string proj_cat { get; set; }

		[Display(Name = "Definition")]
		[StringLength(225)]
		[Required]
		public string proj_desc { get; set; }

		[Display(Name = "Status")]
		[StringLength(1)]
		public string status { get; set; }

	}
}
