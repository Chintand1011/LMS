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
    public class DepartmentsLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<DepartmentsVM>> GetDepartments(Payload payload)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from a in db.Departments
                                 join d in db.Departments on a.parent_dept_id equals d.id into pd
                                 from pdu in pd.DefaultIfEmpty()
                                 join u in db.AccessUser on a.headed_by equals u.id into u
                                 from uu in u.DefaultIfEmpty()
                                 where a.status != Constant.RecordStatus.Deleted
                                 select new DepartmentsVM()
                                 {
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     is_internal = a.is_internal,
                                     dept_code = a.dept_code,
                                     dept_description = a.dept_description,
                                     designation = a.designation,
                                     headed_by = a.headed_by,
                                     headed_by_name = string.Concat(uu.first_name, " ", uu.mi, " ", uu.last_name),
                                     id = a.id,
                                     parent_dept_id =
                                     a.parent_dept_id,
                                     status = a.status,
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString()
                                 }).ToList();
            }
            else
            {
                _result.value = (from a in db.Departments
                                 join d in db.Departments on a.parent_dept_id equals d.id into pd
                                 from pdu in pd.DefaultIfEmpty()
                                 join u in db.AccessUser on a.headed_by equals u.id into u
                                 from uu in u.DefaultIfEmpty()
                                 where (a.dept_code.ToLower().Contains(payload.search_key.ToLower()) ||
                                 a.dept_description.ToLower().Contains(payload.search_key.ToLower())) &&
                                 (a.status == null || a.status != Constant.RecordStatus.Deleted)
                                 select new DepartmentsVM()
                                 {
                                     is_internal = a.is_internal,
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     dept_code = a.dept_code,
                                     dept_description = a.dept_description,
                                     designation = a.designation,
                                     headed_by = a.headed_by,
                                     headed_by_name = string.Concat(uu.first_name, " ", uu.mi, " ", uu.last_name),
                                     id = a.id,
                                     parent_dept_id = a.parent_dept_id,
                                     status = a.status,
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString()
                                 }).ToList();
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

        public Result<IEnumerable<DepartmentsVM>> IsExistDepartments(Payload payload)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            _result.value = (from a in db.Departments
                             join d in db.Departments on a.parent_dept_id equals d.id into pd
                             from pdu in pd.DefaultIfEmpty()
                             join u in db.AccessUser on a.headed_by equals u.id into u
                             from uu in u.DefaultIfEmpty()
                             where (a.dept_code.ToLower() == payload.search_key.ToLower() && a.id != payload.id) &&
                             (a.status == null || a.status != Constant.RecordStatus.Deleted)
                             select new DepartmentsVM()
                             {
                                 is_internal = a.is_internal,
                                 created_by = a.created_by,
                                 created_date = a.created_date.ToString(),
                                 dept_code = a.dept_code,
                                 dept_description = a.dept_description,
                                 designation = a.designation,
                                 headed_by = a.headed_by,
                                 headed_by_name = string.Concat(uu.first_name, " ", uu.mi, " ", uu.last_name),
                                 id = a.id,
                                 parent_dept_id = a.parent_dept_id,
                                 status = a.status,
                                 updated_by = a.updated_by,
                                 updated_date = a.updated_date.ToString()
                             }).ToList();
            _result.total_count = _result.value.Count();
            return _result;
        }

        public Result<IEnumerable<DepartmentsVM>> GetDepartmentsToAssign(Payload payload)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            _result.value = (from a in db.Departments
                             join d in db.Departments on a.parent_dept_id equals d.id into pd
                             from pdu in pd.DefaultIfEmpty()
                             join u in db.AccessUser on a.headed_by equals u.id into u
                             from uu in u.DefaultIfEmpty()
                             where (payload.search_key == null || a.dept_code.ToLower().Contains(payload.search_key.ToLower()) ||
                             a.dept_description.ToLower().Contains(payload.search_key.ToLower())) &&
                             a.status != Constant.RecordStatus.Deleted && (a.parent_dept_id == 0 || a.parent_dept_id == payload.id) &&
                             a.id != payload.id
                             select new DepartmentsVM()
                             {
                                 is_internal = a.is_internal,
                                 created_by = a.created_by,
                                 created_date = a.created_date.ToString(),
                                 dept_code = a.dept_code,
                                 dept_description = a.dept_description,
                                 designation = a.designation,
                                 headed_by = a.headed_by,
                                 headed_by_name = string.Concat(uu.first_name, " ", uu.mi, " ", uu.last_name),
                                 id = a.id,
                                 parent_dept_id = a.parent_dept_id,
                                 status = a.status,
                                 updated_by = a.updated_by,
                                 updated_date = a.updated_date.ToString()
                             }).ToList().OrderByDescending(a => a.parent_dept_id);
            _result.value = _result.value.Take(100);
            return _result;
        }

        public Result<IEnumerable<DepartmentsVM>> CreateDepartments(DepartmentsVM param)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.Departments.Add(new Department()
                    {
                        updated_date = DateTime.Now,
                        updated_by = param.updated_by,
                        created_by = param.created_by,
                        created_date = DateTime.Now,
                        dept_code = param.dept_code,
                        dept_description = param.dept_description,
                        designation = param.designation,
                        headed_by = param.headed_by,
                        id = param.id,
                        parent_dept_id = param.parent_dept_id,
                        status = Constant.RecordStatus.Active,
                        is_internal = param.is_internal
                    });
                    db.SaveChanges();

                    //Select all records
                    _result = GetDepartments(new Payload() { page_index = param.page_index });
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

        public Result<IEnumerable<DepartmentsVM>> UpdateDepartments([FromBody] DepartmentsVM param)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (db.Departments.Any(a => a.dept_code.ToLower() == param.dept_code.ToLower() && a.id != param.id))
                    {
                        throw new Exception("Department code already exists");
                    }
                    db.Departments.AddOrUpdate(new Department()
                    {
                        updated_date = DateTime.Now,
                        updated_by = param.updated_by,
                        dept_code = param.dept_code,
                        is_internal = param.is_internal,
                        dept_description = param.dept_description,
                        designation = param.designation,
                        headed_by = param.headed_by,
                        id = param.id,
                        parent_dept_id = param.parent_dept_id,
                        status = param.status
                    });
                    db.SaveChanges();
                    _result = GetDepartments(new Payload() { page_index = param.page_index });
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


        public Result<IEnumerable<DepartmentsVM>> AssignSubDepartments([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        db.Departments.Where(a => a.parent_dept_id == payload.id).ToList().ForEach(a =>
                        {
                            a.parent_dept_id = 0;
                        });
                        db.SaveChanges();
                        foreach (string id in payload.item_list)
                        {
                            var _Departments = db.Departments.Find(Convert.ToInt32(id));
                            _Departments.parent_dept_id = payload.id;
                            _Departments.updated_date = DateTime.Now;
                            _Departments.updated_by = payload.user_id;
                            db.Departments.AddOrUpdate(_Departments);
                        }
                    }
                    db.SaveChanges();
                    _result = GetDepartments(new Payload() { page_index = payload.page_index });
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
        public Result<IEnumerable<DepartmentsVM>> StatusUpdateDepartments([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<DepartmentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _Departments = db.Departments.Find(Convert.ToInt32(id));
                            _Departments.status = payload.status;
                            _Departments.updated_date = DateTime.Now;
                            _Departments.updated_by = payload.user_id;
                            db.Departments.AddOrUpdate(_Departments);
                        }
                    }
                    db.SaveChanges();
                    _result = GetDepartments(new Payload() { page_index = payload.page_index });
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