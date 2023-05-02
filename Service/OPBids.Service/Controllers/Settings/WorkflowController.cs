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
    public class WorkflowController : ApiController
    {
        [HttpPost]
        [Route("service/GetWorkflow")]
        public Result<IEnumerable<Workflow>> GetWorkflow([FromBody] Payload payload)
        {
            var rslts = new WorkflowLogic().GetWorkflow(payload);
            return rslts;
        }

        [HttpPost]
        [Route("service/CreateWorkflow")]
        public Result<IEnumerable<Workflow>> CreateWorkflow([FromBody] WorkflowVM workflowVM)
        {
            // Validate and Map to Domain model
            var Workflow = workflowVM.ToDomain();
            return new WorkflowLogic().CreateWorkflow(Workflow);
        }

        [HttpPost]
        [Route("service/UpdateWorkflow")]
        public Result<IEnumerable<Workflow>> UpdateWorkflow([FromBody] WorkflowVM workflowVM)
        {
            // Validate and Map to Domain model
            Workflow Workflow = workflowVM.ToDomain();
            return new WorkflowLogic().UpdateWorkflow(Workflow);
        }

        [HttpPost]
        [Route("service/StatusUpdateWorkflow")]
        public Result<IEnumerable<Workflow>> StatusUpdateWorkflow([FromBody] Payload payload)
        {
            return new WorkflowLogic().StatusUpdateWorkflow(payload);
        }
    }
}
