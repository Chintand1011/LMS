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
    public class ProjectCategoryController : ApiController
    {
		[HttpPost]
		[Route("service/GetProjectCategory")]
		public Result<IEnumerable<ProjectCategory>> GetProjectCategory([FromBody] Payload payload)
		{
			return new ProjectCategoryLogic().GetProjectCategory(payload);
		}

		[HttpPost]
		[Route("service/CreateProjectCategory")]
		public Result<IEnumerable<ProjectCategory>> CreateProjectCategory([FromBody] ProjectCategoryVM projectcategoryVM)
		{
			var ProjectCategory = projectcategoryVM.ToDomain();
			return new ProjectCategoryLogic().CreateProjectCategory(ProjectCategory);
		}

		[HttpPost]
		[Route("service/UpdateProjectCategory")]
		public Result<IEnumerable<ProjectCategory>> UpdateProjectCategory([FromBody] ProjectCategoryVM projectcategoryVM)
		{
			// Validate and Map to Domain model
			ProjectCategory ProjectCategory = projectcategoryVM.ToDomain();
			return new ProjectCategoryLogic().UpdateProjectCategory(ProjectCategory);
		}

		[HttpPost]
		[Route("service/StatusUpdateProjectCategory")]
		public Result<IEnumerable<ProjectCategory>> StatusUpdateProjectCategory([FromBody] Payload payload)
		{
			return new ProjectCategoryLogic().StatusUpdateProjectCategory(payload);
		}
	}
}