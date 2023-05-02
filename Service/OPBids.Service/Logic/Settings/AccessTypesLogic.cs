using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
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
    public class AccessTypesLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<AccessType>> GetAccessType(SettingVM payload)
        {
            var _result = new Result<IEnumerable<AccessType>>();
            if (payload.accessTypes == null)
            {
                payload.accessTypes = new AccessTypesVM();
            }
            if (payload.accessTypes.parent_id == null || payload.accessTypes.parent_id == 0)
            {
                payload.accessTypes.parent_id = 79;
            }
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from b in db.AccessTypes
                                 where (b.status == Constant.RecordStatus.Header || b.status != Constant.RecordStatus.Deleted && b.parent_id == payload.accessTypes.parent_id)
                                 select b).ToList().OrderBy(a => (a.parent_id == null || a.parent_id == 0 ? a.id : a.parent_id)).
                                 ThenBy(a => (a.status == Constant.RecordStatus.Header ? -1 : a.seq_no));
            }
            else
            {
                _result.value = (from b in db.AccessTypes
                                 where (b.status == Constant.RecordStatus.Header || b.name == null || b.name == "" || b.name.ToLower().Contains(payload.search_key.ToLower())) &&
                                 (b.status == Constant.RecordStatus.Header || b.status != Constant.RecordStatus.Deleted && b.parent_id == payload.accessTypes.parent_id)
                                 select b).ToList().OrderBy(a => (a.parent_id == null || a.parent_id == 0 ? a.id : a.parent_id)).
                                 ThenBy(a => (a.status == Constant.RecordStatus.Header ? -1 : a.seq_no));
            }
            _result.total_count = _result.value.Count();
            if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Where(a => a.status != Constant.RecordStatus.Deleted).Count().GetPageCount();
                var headers = _result.value.Where(a => a.status == Constant.RecordStatus.Header);
                _result.value = _result.value.Where(a => a.status != Constant.RecordStatus.Deleted).
                    Skip(Constant.AppSettings.PageItemCount * payload.page_index).Take(Constant.AppSettings.PageItemCount);
                _result.value = _result.value.Union(headers);
                _result.value.ToList().ForEach(a => {
                    a.sys_id = payload.accessTypes.parent_id ?? 0;
                });
                _result.value = _result.value.OrderBy(a => (a.parent_id == null || a.parent_id == 0 ? a.id : a.parent_id));
            }
            return _result;
        }

        public Result<IEnumerable<AccessType>> CreateAccessType(AccessType accessType)
        {
            var _result = new Result<IEnumerable<AccessType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    accessType.created_date = DateTime.Now;
                    accessType.updated_date = DateTime.Now;

                    db.AccessTypes.Add(accessType);
                    db.SaveChanges();

                    //Select all records
                    _result = GetAccessType(new SettingVM()
                    {
                        page_index = 0,
                        accessTypes = new AccessTypesVM() { parent_id = accessType.parent_id }
                    });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }

        public Result<IEnumerable<AccessType>> UpdateAccessType([FromBody] AccessType accessType)
        {
            var _result = new Result<IEnumerable<AccessType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    accessType.updated_date = DateTime.Now;

                    db.AccessTypes.AddOrUpdate(accessType);
                    db.SaveChanges();

                    _result = GetAccessType(new SettingVM()
                    {
                        page_index = 0,
                        accessTypes = new AccessTypesVM() { parent_id = accessType.parent_id }
                    });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }
        public Result<IEnumerable<AccessType>> StatusUpdateAccessType([FromBody] SettingVM payload)
        {
            var _result = new Result<IEnumerable<AccessType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _AccessTypes = db.AccessTypes.Find(Convert.ToInt32(id));
                            _AccessTypes.status = payload.status;
                            _AccessTypes.updated_date = DateTime.Now;
                            _AccessTypes.updated_by = payload.accessTypes.updated_by;
                            db.AccessTypes.AddOrUpdate(_AccessTypes);
                        }
                    }
                    db.SaveChanges();

                    _result = GetAccessType(new SettingVM()
                    {
                        page_index = payload.page_index,
                        accessTypes = new AccessTypesVM() { parent_id = payload.accessTypes.parent_id }
                    });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
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