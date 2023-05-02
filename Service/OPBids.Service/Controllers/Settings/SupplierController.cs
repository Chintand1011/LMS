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
    public partial class SettingsController : ApiController
    {
        [HttpPost]
        [Route("service/GetSupplier")]
        public Result<IEnumerable<SupplierVM>> GetSupplier([FromBody] Payload payload)
        {
            return new SupplierLogic().GetSupplier(payload);
        }

        [HttpPost]
        [Route("service/CreateSupplier")]
        public Result<IEnumerable<SupplierVM>> CreateSupplier([FromBody] SupplierVM supplier)
        {            
            return new SupplierLogic().CreateSupplier(supplier);
        }

        [HttpPost]
        [Route("service/UpdateSupplier")]
        public Result<IEnumerable<SupplierVM>> UpdateSupplier([FromBody] SupplierVM supplier)
        {            
            return new SupplierLogic().UpdateSupplier(supplier);
        }

        [HttpPost]
        [Route("service/StatusUpdateSupplier")]
        public Result<IEnumerable<SupplierVM>> StatusUpdateSupplier([FromBody] Payload payload)
        {
            return new SupplierLogic().StatusUpdateSupplier(payload);
        }

        [HttpPost]
        [Route("service/GetSupplierAccessUser")]
        public Result<IEnumerable<AccessUsersVM>> GetSupplierAccessUser([FromBody] Payload payload)
        {
            return new SupplierLogic().GetSupplierAccessUser(payload);
        }
    }
}