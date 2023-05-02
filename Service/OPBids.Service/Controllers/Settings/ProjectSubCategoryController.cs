using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;
using OPBids.Entities.Common;

namespace OPBids.Service.Controllers.Settings
{
    public class ProjectSubCategoryController : ApiController
	{
		[HttpPost]
		[Route("service/GetProjectSubCategory")]
		public Result<IEnumerable<ProjectSubCategory>> GetProjectSubCategory([FromBody] Payload payload)
		{
			return new ProjectSubCategoryLogic().GetProjectSubCategory(payload);
		}

		[HttpPost]
		[Route("service/CreateProjectSubCategory")]
		public Result<IEnumerable<ProjectSubCategory>> CreateProjectSubCategory([FromBody] ProjectSubCategoryVM projectsubcategoryVM)
		{
			var ProjectSubCategory = projectsubcategoryVM.ToDomain();
			return new ProjectSubCategoryLogic().CreateProjectSubCategory(ProjectSubCategory);
		}

		[HttpPost]
		[Route("service/UpdateProjectSubCategory")]
		public Result<IEnumerable<ProjectSubCategory>> UpdateProjectSubCategory([FromBody] ProjectSubCategoryVM projectsubcategoryVM)
		{
			// Validate and Map to Domain model
			ProjectSubCategory ProjectSubCategory = projectsubcategoryVM.ToDomain();
			return new ProjectSubCategoryLogic().UpdateProjectSubCategory(ProjectSubCategory);
		}

		[HttpPost]
		[Route("service/StatusUpdateProjectSubCategory")]
		public Result<IEnumerable<ProjectSubCategory>> StatusUpdateProjectSubCategory([FromBody] Payload payload)
		{
			return new ProjectSubCategoryLogic().StatusUpdateProjectSubCategory(payload);
		}
	}
}
