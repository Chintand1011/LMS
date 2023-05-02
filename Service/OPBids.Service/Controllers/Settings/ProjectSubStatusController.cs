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
    public class ProjectSubStatusController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectSubStatus")]
        public Result<IEnumerable<ProjectSubStatus>> GetProjectSubStatus([FromBody] Payload payload)
        {
            return new ProjectSubStatusLogic().GetProjectSubStatus(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectSubStatus")]
        public Result<IEnumerable<ProjectSubStatus>> CreateProjectSubStatus([FromBody] ProjectSubStatusVM ProjectSubStatusVM)
        {
            var ProjectSubStatus = ProjectSubStatusVM.ToDomain();
            return new ProjectSubStatusLogic().CreateProjectSubStatus(ProjectSubStatus);
        }

        [HttpPost]
        [Route("service/UpdateProjectSubStatus")]
        public Result<IEnumerable<ProjectSubStatus>> UpdateSupplier([FromBody] ProjectSubStatusVM ProjectSubStatusVM)
        {
            // Validate and Map to Domain model
            ProjectSubStatus ProjectSubStatus = ProjectSubStatusVM.ToDomain();
            return new ProjectSubStatusLogic().UpdateProjectSubStatus(ProjectSubStatus);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectSubStatus")]
        public Result<IEnumerable<ProjectSubStatus>> StatusUpdateProjectSubStatus([FromBody] Payload payload)
        {
            return new ProjectSubStatusLogic().StatusUpdateProjectSubStatus(payload);
        }
    }
}