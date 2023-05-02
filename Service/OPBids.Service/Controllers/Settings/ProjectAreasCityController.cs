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
    public class ProjectAreasCityController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectAreasCity")]
        public Result<IEnumerable<ProjectAreasCity>> GetProjectAreasCity([FromBody] Payload payload)
        {
            return new ProjectAreasCityLogic().GetProjectAreasCity(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectAreasCity")]
        public Result<IEnumerable<ProjectAreasCity>> CreateProjectAreasCity([FromBody] ProjectAreasCityVM ProjectAreasCityVM)
        {
            var ProjectAreasCity = ProjectAreasCityVM.ToDomain();
            return new ProjectAreasCityLogic().CreateProjectAreasCity(ProjectAreasCity);
        }

        [HttpPost]
        [Route("service/UpdateProjectAreasCity")]
        public Result<IEnumerable<ProjectAreasCity>> UpdateProjectAreasCity([FromBody] ProjectAreasCityVM ProjectAreasCityVM)
        {
            // Validate and Map to Domain model
            ProjectAreasCity ProjectAreasCity = ProjectAreasCityVM.ToDomain();
            return new ProjectAreasCityLogic().UpdateProjectAreasCity(ProjectAreasCity);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectAreasCity")]
        public Result<IEnumerable<ProjectAreasCity>> StatusUpdateProjectAreasCity([FromBody] Payload payload)
        {
            return new ProjectAreasCityLogic().StatusUpdateProjectAreasCity(payload);
        }

        [HttpPost]
        [Route("service/GetAndSaveProjectAreasCity")]
        public Result<IEnumerable<ProjectAreasCity>> GetAndSaveProjectAreasCity([FromBody] Payload payload)
        {
            return new ProjectAreasCityLogic().GetAndSaveProjectAreasCity(payload);
        }
    }
}