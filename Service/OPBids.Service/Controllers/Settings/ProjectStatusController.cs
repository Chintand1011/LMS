using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using OPBids.Service.Utilities;
using System.Collections.Generic;
using System.Web.Http;


namespace OPBids.Service.Controllers.Settings
{
    public class ProjectStatusController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectStatus")]
        public Result<IEnumerable<ProjectStatus>> GetProjectStatus([FromBody] Payload payload)
        {
            return new ProjectStatusLogic().GetProjectStatus(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectStatus")]
        public Result<IEnumerable<ProjectStatus>> CreateProjectStatus([FromBody] ProjectStatusVM ProjectStatusVM)
        {
            var ProjectStatus = ProjectStatusVM.ToDomain();
            return new ProjectStatusLogic().CreateProjectStatus(ProjectStatus);
        }

        [HttpPost]
        [Route("service/UpdateProjectStatus")]
        public Result<IEnumerable<ProjectStatus>> UpdateProjectStatus([FromBody] ProjectStatusVM ProjectStatusVM)
        {
            // Validate and Map to Domain model
            ProjectStatus ProjectStatus = ProjectStatusVM.ToDomain();
            return new ProjectStatusLogic().UpdateProjectStatus(ProjectStatus);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectStatus")]
        public Result<IEnumerable<ProjectStatus>> StatusUpdateProjectStatus([FromBody] Payload payload)
        {
            return new ProjectStatusLogic().StatusUpdateProjectStatus(payload);
        }
    }
}