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
    public class AccessGroupTypeLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<AccessGroupTypeVM>> GetAccessGroupType(Payload payload) {
            var _result = new Result<IEnumerable<AccessGroupTypeVM>>();
            if (payload.id == 0)
            {
                payload.id = db.AccessGroup.FirstOrDefault().id;
            }
            if (payload.search_key == null || payload.search_key == string.Empty) {
                _result.value = (from a in db.AccessTypes
                                 join c in db.AccessGroupTypes on new { t1 = a.id, t2 = payload.id } equals new { t1 = c.access_type_id, t2 = c.access_group_id } into at
                                 from atu in at.DefaultIfEmpty()
                                 join b in db.AccessGroup on atu.access_group_id equals b.id into ag
                                 from agu in ag.DefaultIfEmpty()
                                 where (atu.access_group_id == payload.id || (a.id != 0 || a.status == Constant.RecordStatus.Header) &&
                                 (a.status == Constant.RecordStatus.Active || a.status == Constant.RecordStatus.Header))
                                 select new AccessGroupTypeVM()
                                 {
                                     access_group = agu.group_code,
                                     access_group_id = payload.id,
                                     access_type_id = a.id,
                                     add_edit_data = (a.add_edit_data == true ? (atu.access_group_id == payload.id ? atu.add_edit_data ?? false : false) : (bool?)null),
                                     code = a.code,
                                     controller = a.controller,
                                     icon = a.icon,
                                     css_class = a.css_class,
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     delete_data = (a.delete_data == true ? (atu.access_group_id == payload.id ? atu.delete_data ?? false : false) : (bool?)null),
                                     record_section = (a.record_section == true ? (atu.access_group_id == payload.id ? atu.record_section ?? false : false) : (bool?)null),
                                     description = a.description,
                                     disp_menu_to_mobile = a.disp_menu_to_mobile,
                                     access_group_type_id = (atu.access_group_id == payload.id ? atu.id : 0),
                                     name = a.name,
                                     parent_id = (a.parent_id == null || a.parent_id == 0 ? a.id : a.parent_id),
                                     seq_no = a.seq_no,
                                     status = a.status,
                                     sys_id = a.sys_id,
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString(),
                                     view_transact_data = (a.view_transact_data == true ? (atu.access_group_id == payload.id ? atu.view_transact_data ?? false : false) : (bool?)null)
                                 }).ToList().OrderBy(a=> a.parent_id).ThenBy(a => (a.status == Constant.RecordStatus.Header ? - 1:  a.seq_no)).ToList();
            }
            else {
                _result.value = (from a in db.AccessTypes
                                 join c in db.AccessGroupTypes on new { t1 = a.id, t2 = payload.id } equals new { t1 = c.access_type_id, t2 = c.access_group_id } into at
                                 from atu in at.DefaultIfEmpty()
                                 join b in db.AccessGroup on atu.access_group_id equals b.id into ag
                                 from agu in ag.DefaultIfEmpty()
                                 where (atu.access_group_id == payload.id || (a.id != 0 || a.status == Constant.RecordStatus.Header) &&
                                 (a.status == Constant.RecordStatus.Active || a.status == Constant.RecordStatus.Header) &&
                                 a.name.Contains(payload.search_key))
                                 select new AccessGroupTypeVM()
                                 {
                                     access_group = agu.group_code,
                                     access_group_id = payload.id,
                                     access_type_id = a.id,
                                     add_edit_data = (a.add_edit_data == true ? (atu.access_group_id == payload.id ? atu.add_edit_data ?? false: false): (bool?)null),
                                     code = a.code,
                                     controller = a.controller,
                                     icon = a.icon,
                                     css_class = a.css_class,
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     delete_data = (a.delete_data == true ? (atu.access_group_id == payload.id ? atu.delete_data ?? false: false) : (bool?)null),
                                     record_section = (a.record_section == true ? (atu.access_group_id == payload.id ? atu.record_section ?? false : false) : (bool?)null),
                                     description = a.description,
                                     disp_menu_to_mobile = a.disp_menu_to_mobile,
                                     access_group_type_id = (atu.access_group_id == payload.id ? atu.id : 0),
                                     name = a.name,
                                     parent_id = (a.parent_id == null || a.parent_id == 0 ? a.id : a.parent_id),
                                     seq_no = a.seq_no,
                                     status = a.status,
                                     sys_id = a.sys_id,
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString(),
                                     view_transact_data = (a.view_transact_data == true ? (atu.access_group_id == payload.id ? atu.view_transact_data ?? false: false) : (bool?)null)
                                 }).ToList().OrderBy(a => a.parent_id).ThenBy(a => (a.status == Constant.RecordStatus.Header ? -1 : a.seq_no)).ToList();
            }
            return _result;
        }
        public Result<IEnumerable<AccessGroupTypeVM>> GetAccessGroupMenu(Payload param)
        {
            var _result = new Result<IEnumerable<AccessGroupTypeVM>>();
            try
            {
                _result = GetAccessGroupType(new Payload() { id = param.id });
                _result.value = _result.value.Where(a => a.view_transact_data == true || a.add_edit_data == true || a.record_section == true ||
                a.delete_data == true || a.status == Constant.RecordStatus.Header && 
                _result.value.Any(b => b.parent_id == a.access_type_id && 
                b.status == Constant.RecordStatus.Active && (b.view_transact_data == true || b.add_edit_data == true || b.record_section == true ||
                b.delete_data == true)) == true).ToList();
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

        public Result<IEnumerable<AccessGroupTypeVM>> SaveAccessGroupType(AccessGroupTypeVM[] param) {
            var _result = new Result<IEnumerable<AccessGroupTypeVM>>();
            try {
                using (var db = new DatabaseContext()) {
                    param.ToList().ForEach(a =>
                    {
                        var itm = new AccessGroupType()
                        {
                            id = a.access_group_type_id ?? 0,
                            access_group_id = a.access_group_id ?? 0,
                            access_type_id = a.access_type_id ?? 0,
                            add_edit_data = a.add_edit_data,
                            record_section = a.record_section,
                            created_by = a.created_by,
                            created_date = DateTime.Now,
                            delete_data = a.delete_data,
                            updated_by = a.updated_by,
                            updated_date = DateTime.Now,
                            view_transact_data = a.view_transact_data
                        };
                        if (itm.id == 0)
                        {
                            db.AccessGroupTypes.Add(itm);
                        }
                        else
                        {
                            db.AccessGroupTypes.AddOrUpdate(itm);
                        }
                    });
                    db.SaveChanges();

                    //Select all records
                    _result = GetAccessGroupType(new Payload() {id = param.FirstOrDefault().access_group_id ?? 0 });
                }
            } catch (Exception ex) {
                //TODO: Exception handling
                _result.status = new Status() {
                                        code = Constant.Status.Failed,
                                        description = ex.Message };
            }
            return _result;
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}