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
    public class ProjectAreasBarangayController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectAreasBarangay")]
        public Result<IEnumerable<ProjectAreasBarangay>> GetProjectAreasBarangay([FromBody] Payload payload)
        {
            return new ProjectAreasBarangayLogic().GetProjectAreasBarangay(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectAreasBarangay")]
        public Result<IEnumerable<ProjectAreasBarangay>> CreateProjectAreasBarangay([FromBody] ProjectAreasBarangayVM ProjectAreasBarangayVM)
        {
            var ProjectAreasBarangay = ProjectAreasBarangayVM.ToDomain();
            return new ProjectAreasBarangayLogic().CreateProjectAreasBarangay(ProjectAreasBarangay);
        }

        [HttpPost]
        [Route("service/UpdateProjectAreasBarangay")]
        public Result<IEnumerable<ProjectAreasBarangay>> UpdateProjectAreasBarangay([FromBody] ProjectAreasBarangayVM ProjectAreasBarangayVM)
        {
            // Validate and Map to Domain model
            ProjectAreasBarangay ProjectAreasBarangay = ProjectAreasBarangayVM.ToDomain();
            return new ProjectAreasBarangayLogic().UpdateProjectAreasBarangay(ProjectAreasBarangay);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectAreasBarangay")]
        public Result<IEnumerable<ProjectAreasBarangay>> StatusUpdateProjectAreasBarangay([FromBody] Payload payload)
        {
            return new ProjectAreasBarangayLogic().StatusUpdateProjectAreasBarangay(payload);
        }

        [HttpPost]
        [Route("service/GetAndSaveProjectAreasBarangay")]
        public Result<IEnumerable<ProjectAreasBarangay>> GetAndSaveProjectAreasBarangay([FromBody] Payload payload)
        {
            return new ProjectAreasBarangayLogic().GetAndSaveProjectAreasBarangay(payload);
        }
    }
}