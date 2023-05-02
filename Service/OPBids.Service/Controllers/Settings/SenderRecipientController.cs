using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Logic.Settings;
using OPBids.Service.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace OPBids.Service.Controllers.Settings
{
    public partial class SenderRecipientController : ApiController
    {
        [HttpPost]
        [Route("service/GetSenderRecipient")]
        public Result<IEnumerable<SenderRecipientUserVM>> GetSenderRecipient([FromBody] Payload payload)
        {
            var rslts = new SenderRecipientLogic().GetSenderRecipient(payload);
            return rslts;
        }

        [HttpPost]
        [Route("service/CreateSenderRecipient")]
        public Result<IEnumerable<SenderRecipientUserVM>> CreateSenderRecipient([FromBody] SettingVM senderRecipientVM)
        {
            return new SenderRecipientLogic().CreateSenderRecipient(senderRecipientVM);
        }

        [HttpPost]
        [Route("service/UpdateSenderRecipient")]
        public Result<IEnumerable<SenderRecipientUserVM>> UpdateSenderRecipient([FromBody] SettingVM senderRecipientVM)
        {
            return new SenderRecipientLogic().UpdateSenderRecipient(senderRecipientVM);
        }

        [HttpPost]
        [Route("service/UpdateSenderRecipientStatus")]
        public Result<IEnumerable<SenderRecipientUserVM>> UpdateSenderRecipientStatus([FromBody] Payload payload)
        {
            return new SenderRecipientLogic().StatusUpdateSenderRecipient(payload);
        }
    }
}
