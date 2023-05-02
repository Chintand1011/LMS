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
    public class ContractTypeController: ApiController
    {
        [HttpPost]
        [Route("service/GetContractType")]
        public Result<IEnumerable<ContractType>> GetContractType([FromBody] Payload payload)
        {
            return new ContractTypeLogic().GetContractType(payload);
        }

        [HttpPost]
        [Route("service/CreateContractType")]
        public Result<IEnumerable<ContractType>> CreateContractType([FromBody] ContractTypeVM contracttypeVM)
        {
            var ContractType = contracttypeVM.ToDomain();
            return new ContractTypeLogic().CreateContractType(ContractType);
        }

        [HttpPost]
        [Route("service/UpdateContractType")]
        public Result<IEnumerable<ContractType>> UpdateSupplier([FromBody] ContractTypeVM contracttypeVM)
        {
            // Validate and Map to Domain model
            ContractType ContractType = contracttypeVM.ToDomain();
            return new ContractTypeLogic().UpdateContractType(ContractType);
        }

        [HttpPost]
        [Route("service/StatusUpdateContractType")]
        public Result<IEnumerable<ContractType>> StatusUpdateContractType([FromBody] Payload payload)
        {
            return new ContractTypeLogic().StatusUpdateContractType(payload);
        }
    }
}
