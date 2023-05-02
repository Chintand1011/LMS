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
    public class ProjectAreasDistrictController : ApiController
    {
        [HttpPost]
        [Route("service/GetProjectAreasDistrict")]
        public Result<IEnumerable<ProjectAreasDistrict>> GetProjectAreasDistrict([FromBody] Payload payload)
        {
            return new ProjectAreasDistrictLogic().GetProjectAreasDistrict(payload);
        }

        [HttpPost]
        [Route("service/CreateProjectAreasDistrict")]
        public Result<IEnumerable<ProjectAreasDistrict>> CreateProjectAreasDistrict([FromBody] ProjectAreasDistrictVM ProjectAreasDistrictVM)
        {
            var ProjectAreasDistrict = ProjectAreasDistrictVM.ToDomain();
            return new ProjectAreasDistrictLogic().CreateProjectAreasDistrict(ProjectAreasDistrict);
        }

        [HttpPost]
        [Route("service/UpdateProjectAreasDistrict")]
        public Result<IEnumerable<ProjectAreasDistrict>> UpdateProjectAreasDistrict([FromBody] ProjectAreasDistrictVM ProjectAreasDistrictVM)
        {
            // Validate and Map to Domain model
            ProjectAreasDistrict ProjectAreasDistrict = ProjectAreasDistrictVM.ToDomain();
            return new ProjectAreasDistrictLogic().UpdateProjectAreasDistrict(ProjectAreasDistrict);
        }

        [HttpPost]
        [Route("service/StatusUpdateProjectAreasDistrict")]
        public Result<IEnumerable<ProjectAreasDistrict>> StatusUpdateProjectAreasDistrict([FromBody] Payload payload)
        {
            return new ProjectAreasDistrictLogic().StatusUpdateProjectAreasDistrict(payload);
        }

        [HttpPost]
        [Route("service/GetAndSaveProjectAreasDistrict")]
        public Result<IEnumerable<ProjectAreasDistrict>> GetAndSaveProjectAreasDistrict([FromBody] Payload payload)
        {
            return new ProjectAreasDistrictLogic().GetAndSaveProjectAreasDistrict(payload);
        }

        [HttpPost]
        [Route("service/GetProjectAreasDistrictByCity")]
        public Result<IEnumerable<ProjectAreasDistrict>> GetProjectAreasDistrictByCity([FromBody] Payload payload)
        {
            return new ProjectAreasDistrictLogic().GetProjectAreasDistrictByCity(payload);
        }
    }
}