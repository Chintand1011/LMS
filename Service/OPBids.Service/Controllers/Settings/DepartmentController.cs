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
    public partial class SettingsController : ApiController
    {
        [HttpPost]
        [Route("service/GetDepartments")]
        public Result<IEnumerable<DepartmentsVM>> GetDepartments([FromBody] Payload payload)
        {
            return new DepartmentsLogic().GetDepartments(payload);
        }
        
        [HttpPost]
        [Route("service/AssignSubDepartments")]
        public Result<IEnumerable<DepartmentsVM>> AssignSubDepartments([FromBody] Payload payload)
        {
            return new DepartmentsLogic().AssignSubDepartments(payload);
        }
        [HttpPost]
        [Route("service/IsExistDepartments")]
        public Result<IEnumerable<DepartmentsVM>> IsExistDepartments([FromBody] Payload payload)
        {
            return new DepartmentsLogic().IsExistDepartments(payload);
        }
        [HttpPost]
        [Route("service/GetDepartmentsToAssign")]
        public Result<IEnumerable<DepartmentsVM>> GetDepartmentsToAssign([FromBody] Payload payload)
        {
            return new DepartmentsLogic().GetDepartmentsToAssign(payload);
        }

        [HttpPost]
        [Route("service/CreateDepartments")]
        public Result<IEnumerable<DepartmentsVM>> CreateDepartments([FromBody] DepartmentsVM departments)
        {
               return new DepartmentsLogic().CreateDepartments(departments);
        }

        [HttpPost]
        [Route("service/UpdateDepartments")]
        public Result<IEnumerable<DepartmentsVM>> UpdateDepartments([FromBody] DepartmentsVM departments)
        {
            return new DepartmentsLogic().UpdateDepartments(departments);
        }

        [HttpPost]
        [Route("service/StatusUpdateDepartments")]
        public Result<IEnumerable<DepartmentsVM>> StatusUpdateDepartments([FromBody] Payload payload)
        {
            return new DepartmentsLogic().StatusUpdateDepartments(payload);
        }
    }
}