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
    public class ProjectAreasController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectAreas")]
        public Result<IEnumerable<ProjectAreasVM>> GetProjectAreas([FromBody] Payload payload)
        {
            return new ProjectAreasLogic().GetProjectAreas(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectAreas")]
        public Result<IEnumerable<ProjectAreasVM>> CreateProjectAreas([FromBody] ProjectAreasVM projectareas)
        {            
            return new ProjectAreasLogic().CreateProjectAreas(projectareas);            
        }

        [HttpPost]
        [Route("service/UpdateProjectAreas")]
        public Result<IEnumerable<ProjectAreasVM>> UpdateProjectAreas([FromBody] ProjectAreasVM projectareas)
        {
            return new ProjectAreasLogic().UpdateProjectAreas(projectareas);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectAreas")]
        public Result<IEnumerable<ProjectAreasVM>> StatusUpdateProjectAreas([FromBody] Payload payload)
        {
            return new ProjectAreasLogic().StatusUpdateProjectAreas(payload);
        }

        [HttpPost]
        [Route("service/GetAndSaveProjectAreas")]
        public Result<IEnumerable<ProjectAreasVM>> GetAndSaveProjectAreas([FromBody] Payload payload)
        {
            return new ProjectAreasLogic().GetAndSaveProjectAreas(payload);
        }
    }
}