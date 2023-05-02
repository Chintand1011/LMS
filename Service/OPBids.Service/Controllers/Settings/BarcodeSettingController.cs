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
    public class BarcodeSettingController : ApiController
    {
        [HttpPost]
        [Route("service/GetBarcodeSetting")]
        public Result<IEnumerable<BarcodeSetting>> GetBarcodeSetting([FromBody] Payload payload)
        {
            return new BarcodeSettingLogic().GetBarcodeSetting(payload);
        }

        [HttpPost]
        [Route("service/CreateBarcodeSetting")]
        public Result<IEnumerable<BarcodeSetting>> CreateBarcodeSetting([FromBody] BarcodeSettingVM barcodeSettingVM)
        {
            // Validate and Map to Domain model
            var barcodeSetting = barcodeSettingVM.ToDomain();
            return new BarcodeSettingLogic().CreateBarcodeSetting(barcodeSetting);
        }

        [HttpPost]
        [Route("service/UpdateBarcodeSetting")]
        public Result<IEnumerable<BarcodeSetting>> UpdateBarcodeSetting([FromBody] BarcodeSettingVM barcodeSettingVM)
        {
            // Validate and Map to Domain model
            var barcodeSetting = barcodeSettingVM.ToDomain();
            return new BarcodeSettingLogic().UpdateBarcodeSetting(barcodeSetting);
        }
    }
}
