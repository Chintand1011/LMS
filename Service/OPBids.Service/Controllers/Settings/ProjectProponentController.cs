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
    public class ProjectProponentController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectProponent")]
        public Result<IEnumerable<ProjectProponent>> GetProjectProponent([FromBody] Payload payload)
        {
            return new ProjectProponentLogic().GetProjectProponent(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectProponent")]
        public Result<IEnumerable<ProjectProponent>> CreateProjectProponent([FromBody] ProjectProponentVM projectproponentVM)
        {
            var ProjectProponent = projectproponentVM.ToDomain();
            return new ProjectProponentLogic().CreateProjectProponent(ProjectProponent);
        }

        [HttpPost]
        [Route("service/UpdateProjectProponent")]
        public Result<IEnumerable<ProjectProponent>> UpdateProjectProponent([FromBody] ProjectProponentVM projectproponentVM)
        {
            // Validate and Map to Domain model
            ProjectProponent projectroponent = projectproponentVM.ToDomain();
            return new ProjectProponentLogic().UpdateProjectProponent(projectroponent);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectProponent")]
        public Result<IEnumerable<ProjectProponent>> StatusUpdateProjectProponent([FromBody] Payload payload)
        {
            return new ProjectProponentLogic().StatusUpdateProjectProponent(payload);
        }
    }
}