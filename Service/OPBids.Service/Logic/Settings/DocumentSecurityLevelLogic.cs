using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.Settings
{
    public class DocumentSecurityLevelLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<DocumentSecurityLevel>> GetDocumentSecurityLevel(Payload payload)
        {
            var _result = new Result<IEnumerable<DocumentSecurityLevel>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.DocumentSecurityLevel
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.DocumentSecurityLevel
                                 where (types.code.ToLower().Contains(payload.search_key.ToLower()) || 
                                 types.description.ToLower().Contains(payload.search_key.ToLower())) 
                                 select types).ToList();
            }
            _result.total_count = _result.value.Count();
            if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
            }
            return _result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}