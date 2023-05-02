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
    public class SupplierLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<SupplierVM>> GetSupplier(Payload payload)
        {
            var _result = new Result<IEnumerable<SupplierVM>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {                                
                _result.value = (from s in db.Supplier
                                 join au in
                                (from u in db.AccessUser
                                 join d in db.Departments on u.dept_id equals d.id into ud
                                 from d in ud.DefaultIfEmpty()
                                 where d.dept_code.ToUpper() == Constant.Departments.DeptCode_SuppDept &&
                                 u.status != Constant.RecordStatus.Deleted &&
                                 d.status != Constant.RecordStatus.Deleted
                                 select new AccessUsersVM()
                                 {
                                     id = u.id,
                                     first_name = u.first_name,
                                     mi = u.mi,
                                     last_name = u.last_name
                                 }) on s.user_id equals au.id into sau
                                 from u in sau.DefaultIfEmpty()
                                 where s.status != Constant.RecordStatus.Deleted
                                 select new SupplierVM()
                                 {
                                     id = s.id,
                                     user_id = s.user_id,
                                     supp_first_name = u.first_name ?? "",
                                     supp_mi = u.mi ?? "",
                                     supp_last_name = u.last_name ?? "",
                                     contact_person = s.contact_person,
                                     company_code = s.company_code,
                                     comp_name = s.comp_name,
                                     address = s.address,
                                     email = s.email,
                                     contact_no = s.contact_no,
                                     tin = s.tin,
                                     status = s.status,
                                     created_by = s.created_by,
                                     created_date = s.created_date.ToString(),
                                     updated_by = s.updated_by,
                                     updated_date = s.updated_date.ToString()
                                 }).ToList();
            }
            else
            {
                _result.value = (from s in db.Supplier
                                 join au in
                                (from u in db.AccessUser
                                 join d in db.Departments on u.dept_id equals d.id into ud
                                 from d in ud.DefaultIfEmpty()
                                 where d.dept_code.ToUpper() == Constant.Departments.DeptCode_SuppDept &&
                                 u.status != Constant.RecordStatus.Deleted &&
                                 d.status != Constant.RecordStatus.Deleted
                                 select new AccessUsersVM()
                                 {
                                     id = u.id,
                                     first_name = u.first_name,
                                     mi = u.mi,
                                     last_name = u.last_name
                                 }) on s.user_id equals au.id into sau
                                 from u in sau.DefaultIfEmpty()
                                 where (s.comp_name.ToLower().Contains(payload.search_key.ToLower())) &&
                                 s.status != Constant.RecordStatus.Deleted
                                 select new SupplierVM()
                                 {
                                     id = s.id,
                                     user_id = s.user_id,
                                     supp_first_name = u.first_name ?? "",
                                     supp_mi = u.mi ?? "",
                                     supp_last_name = u.last_name ?? "",
                                     contact_person = s.contact_person,
                                     company_code = s.company_code,
                                     comp_name = s.comp_name,
                                     address = s.address,
                                     email = s.email,
                                     contact_no = s.contact_no,
                                     tin = s.tin,
                                     status = s.status,
                                     created_by = s.created_by,
                                     created_date = s.created_date.ToString(),
                                     updated_by = s.updated_by,
                                     updated_date = s.updated_date.ToString()
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

        public Result<IEnumerable<SupplierVM>> CreateSupplier(SupplierVM supplier)
        {
            var _result = new Result<IEnumerable<SupplierVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {                    
                    db.Supplier.Add(new Supplier()
                    {
                        updated_date = DateTime.Now,
                        updated_by = supplier.updated_by,
                        created_by = supplier.created_by,
                        created_date = DateTime.Now,
                        user_id = supplier.user_id,
                        contact_person = supplier.contact_person,
                        company_code = supplier.company_code,
                        comp_name = supplier.comp_name,
                        address = supplier.address,
                        email = supplier.email,
                        contact_no = supplier.contact_no,
                        tin = supplier.tin,
                        id = supplier.id,
                        status = Constant.RecordStatus.Active,
                    });

                    db.SaveChanges();
                    //Select all records
                    _result = GetSupplier(new Payload() { page_index = supplier.page_index });
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

        public Result<IEnumerable<SupplierVM>> UpdateSupplier([FromBody] SupplierVM supplier)
        {
            var _result = new Result<IEnumerable<SupplierVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.Supplier.AddOrUpdate(new Supplier()
                    {
                        updated_date = DateTime.Now,
                        updated_by = supplier.updated_by,
                        user_id = supplier.user_id,
                        contact_person = supplier.contact_person,
                        company_code = supplier.company_code,
                        comp_name = supplier.comp_name,
                        address = supplier.address,
                        email = supplier.email,
                        contact_no = supplier.contact_no,
                        tin = supplier.tin,
                        id = supplier.id,
                        status = Constant.RecordStatus.Active,
                    });
                    db.SaveChanges();
                    _result = GetSupplier(new Payload() { page_index = supplier.page_index });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<SupplierVM>> StatusUpdateSupplier([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<SupplierVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _Supplier = db.Supplier.Find(Convert.ToInt32(id));
                            _Supplier.status = payload.status;
                            _Supplier.updated_date = DateTime.Now;
                            _Supplier.updated_by = payload.user_id;
                            db.Supplier.AddOrUpdate(_Supplier);
                        }
                    }
                    db.SaveChanges();
                    _result = GetSupplier(new Payload() { page_index = payload.page_index });
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

        public Result<IEnumerable<AccessUsersVM>> GetSupplierAccessUser(Payload payload)
        {
            int user_id = 0;
            if (payload.item_list.Count() > 0)
            {
                user_id = Convert.ToInt32(payload.item_list[0]);
            }
            
            var _result = new Result<IEnumerable<AccessUsersVM>>();

            _result = new Result<IEnumerable<AccessUsersVM>>
            {
                value = (from u in db.AccessUser
                         join d in db.Departments on u.dept_id equals d.id into ud
                         from d in ud.DefaultIfEmpty()
                         where d.dept_code.ToUpper() == Constant.Departments.DeptCode_SuppDept &&
                         u.status != Constant.RecordStatus.Deleted &&
                         d.status != Constant.RecordStatus.Deleted &&
                         !(from s in db.Supplier
                           where s.user_id != user_id &&
                           s.status != Constant.RecordStatus.Deleted
                           select s.user_id).Contains(u.id)
                         select new AccessUsersVM()
                         {
                             id = u.id,
                             first_name = u.first_name ?? "",
                             mi = u.mi ?? "",
                             last_name = u.last_name ?? ""
                         }).ToList()
            };

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