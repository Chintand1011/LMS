using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Data;
using OPBids.Service.Models.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;


namespace OPBids.Service.Logic.Settings
{
    public class DashBoardConfigLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<DashboardConfig>> GetDashBoardConfig(Payload payload)
        {
            var _result = new Result<IEnumerable<DashboardConfig>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.DashboardConfigs
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();

            }
            else
            {
                _result.value = (from types in db.DashboardConfigs
                                 where types.dashboard_desc.ToLower().Contains(payload.search_key.ToLower()) &&
                                 types.status != Constant.RecordStatus.Deleted
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
    }
}


