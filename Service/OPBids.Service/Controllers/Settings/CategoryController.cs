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
    public class CategoryController : ApiController
    {
        [HttpPost]
        [Route("service/GetCategory")]
        public Result<IEnumerable<Category>> GetCategory([FromBody] Payload payload)
        {
            return new CategoryLogic().GetCategory(payload);
        }

        [HttpPost]
        [Route("service/CreateCategory")]
        public Result<IEnumerable<Category>> CreateCategory([FromBody] CategoryVM CategoryVM)
        {
            var Category = CategoryVM.ToDomain();
            return new CategoryLogic().CreateCategory(Category);
        }

        [HttpPost]
        [Route("service/UpdateCategory")]
        public Result<IEnumerable<Category>> UpdateCategory([FromBody] CategoryVM CategoryVM)
        {
            // Validate and Map to Domain model
            Category Category = CategoryVM.ToDomain();
            return new CategoryLogic().UpdateCategory(Category);
        }

        [HttpPost]
        [Route("service/StatusUpdateCategory")]
        public Result<IEnumerable<Category>> StatusUpdateCategory([FromBody] Payload payload)
        {
            return new CategoryLogic().StatusUpdateCategory(payload);
        }
    }
}
