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
    public class ProcurementTypeLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProcurementType>> GetProcurementType(Payload payload)
        {
            var _result = new Result<IEnumerable<ProcurementType>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProcurementType
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ProcurementType
                                 where (types.proc_type.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.proc_typedesc.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProcurementType>> CreateProcurementType(ProcurementType procurementtype)
        {
            var _result = new Result<IEnumerable<ProcurementType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    procurementtype.status = Constant.RecordStatus.Active;
                    procurementtype.created_date = DateTime.Now;
                    procurementtype.updated_date = DateTime.Now;

                    db.ProcurementType.Add(procurementtype);
                    db.SaveChanges();
                    //Select all records
                    _result = GetProcurementType(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<ProcurementType>> UpdateProcurementType([FromBody] ProcurementType procurementtype)
        {
            var _result = new Result<IEnumerable<ProcurementType>>();
            try
            {
                using (var db = new DatabaseContext())
                {

                    procurementtype.updated_date = DateTime.Now;
                    db.ProcurementType.AddOrUpdate(procurementtype);
                    db.SaveChanges();
                    _result = GetProcurementType(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<ProcurementType>> StatusUpdateProcurementType([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProcurementType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _ProcurementType = db.ProcurementType.Find(Convert.ToInt32(id));
                            _ProcurementType.status = payload.status;
                            _ProcurementType.updated_date = DateTime.Now;
                            _ProcurementType.updated_by = payload.user_id;
                            db.ProcurementType.AddOrUpdate(_ProcurementType);
                        }
                    }
                    db.SaveChanges();
                    _result = GetProcurementType(new Payload() { page_index = payload.page_index });
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