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
    public class ProcurementMethodLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProcurementMethod>> GetProcurementMethod(Payload payload)
        {
            var _result = new Result<IEnumerable<ProcurementMethod>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProcurementMethod
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ProcurementMethod
                                 where (types.proc_code.ToLower().Contains(payload.search_key.ToLower()) || types.procurement_description.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProcurementMethod>> CreateProcurementMethod(ProcurementMethod ProcurementMethod)
        {
            var _result = new Result<IEnumerable<ProcurementMethod>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    ProcurementMethod.status = Constant.RecordStatus.Active;
                    ProcurementMethod.created_date = DateTime.Now;
                    ProcurementMethod.updated_date = DateTime.Now;

                    db.ProcurementMethod.Add(ProcurementMethod);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProcurementMethod
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
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

        public Result<IEnumerable<ProcurementMethod>> UpdateProcurementMethod([FromBody] ProcurementMethod ProcurementMethod)
        {
            var _result = new Result<IEnumerable<ProcurementMethod>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    ProcurementMethod.updated_date = DateTime.Now;

                    db.ProcurementMethod.AddOrUpdate(ProcurementMethod);
                    db.SaveChanges();

                    _result.value = (from types in db.ProcurementMethod
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
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

        public Result<IEnumerable<ProcurementMethod>> StatusUpdateProcurementMethod([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProcurementMethod>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _ProcurementMethod = db.ProcurementMethod.Find(Convert.ToInt32(id));
                            _ProcurementMethod.status = payload.status;
                            _ProcurementMethod.updated_date = DateTime.Now;
                            _ProcurementMethod.updated_by = payload.user_id;
                            db.ProcurementMethod.AddOrUpdate(_ProcurementMethod);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProcurementMethod
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
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
