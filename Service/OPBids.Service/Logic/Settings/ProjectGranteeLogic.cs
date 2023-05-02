using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;


namespace OPBids.Service.Logic.Settings
{
    public class ProjectGranteeLogic: ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<ProjectGrantee>> GetProjectGrantee(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectGrantee>>();

            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectGrantees
                                    where types.status != Constant.RecordStatus.Deleted
                                    select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ProjectGrantees
                                 where (types.grantee_code.ToLower().Contains(payload.search_key.ToLower()) || types.grantee_name.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProjectGrantee>> GetProjectGranteeAuto(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectGrantee>>();

            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                if (payload.setting_list.ToList().Contains(Constant.Setting.Selection.ProjectGranteePrevUsed))
                {
                    _result.value = (from types in db.ProjectGrantees
                                     join pr in db.ProjectRequests on types.id.ToString() equals pr.grantee
                                     orderby pr.updated_date descending
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).Distinct().Take(5).ToList();
                }
                else
                {
                    _result.value = (from types in db.ProjectGrantees
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
                }
            }
            else
            {
                _result.value = (from types in db.ProjectGrantees
                                 where (types.grantee_code.ToLower().Contains(payload.search_key.ToLower()) || types.grantee_name.ToLower().Contains(payload.search_key.ToLower())) &&
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