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
    public partial class DeliveryController : ApiController
    {
        [HttpPost]
        [Route("service/GetDelivery")]
        public Result<IEnumerable<Delivery>> GetDelivery([FromBody] Payload payload)
        {
            return new DeliveryLogic().GetDelivery(payload);
        }

        [HttpPost]
        [Route("service/CreateDelivery")]
        public Result<IEnumerable<Delivery>> CreateDelivery([FromBody] DeliveryVM deliveryVM)
        {
            // Validate and Map to Domain model
            var Delivery = deliveryVM.ToDomain();
            return new DeliveryLogic().CreateDelivery(Delivery);
        }

        [HttpPost]
        [Route("service/UpdateDelivery")]
        public Result<IEnumerable<Delivery>> UpdateDelivery([FromBody] DeliveryVM deliveryVM)
        {
            // Validate and Map to Domain model
            Delivery Delivery = deliveryVM.ToDomain();
            return new DeliveryLogic().UpdateDelivery(Delivery);
        }

        [HttpPost]
        [Route("service/StatusUpdateDelivery")]
        public Result<IEnumerable<Delivery>> StatusUpdateDelivery([FromBody] Payload payload)
        {
            return new DeliveryLogic().StatusUpdateDelivery(payload);
        }
    }
}