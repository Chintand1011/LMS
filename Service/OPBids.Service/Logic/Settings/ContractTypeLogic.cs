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
    public class ContractTypeLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ContractType>> GetContractType(Payload payload)
        {
            var _result = new Result<IEnumerable<ContractType>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ContractType
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ContractType
                                 where (types.contract_type.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.contract_desc.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ContractType>> CreateContractType(ContractType contracttype)
        {
            var _result = new Result<IEnumerable<ContractType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    contracttype.status = Constant.RecordStatus.Active;
                    contracttype.created_date = DateTime.Now;
                    contracttype.updated_date = DateTime.Now;

                    db.ContractType.Add(contracttype);
                    db.SaveChanges();
                    //Select all records
                    _result = GetContractType(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<ContractType>> UpdateContractType([FromBody] ContractType contracttype)
        {
            var _result = new Result<IEnumerable<ContractType>>();
            try
            {
                using (var db = new DatabaseContext())
                {

                    contracttype.updated_date = DateTime.Now;
                    db.ContractType.AddOrUpdate(contracttype);
                    db.SaveChanges();
                    _result = GetContractType(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<ContractType>> StatusUpdateContractType([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ContractType>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _ContractType = db.ContractType.Find(Convert.ToInt32(id));
                            _ContractType.status = payload.status;
                            _ContractType.updated_date = DateTime.Now;
                            _ContractType.updated_by = payload.user_id;
                            db.ContractType.AddOrUpdate(_ContractType);
                        }
                    }
                    db.SaveChanges();
                    _result = GetContractType(new Payload() { page_index = payload.page_index });
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