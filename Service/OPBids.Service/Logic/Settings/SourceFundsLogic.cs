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
    public class SourceFundsLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<SourceFunds>> GetSourceFunds(Payload payload)
        {
            var _result = new Result<IEnumerable<SourceFunds>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.SourceFunds
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.SourceFunds
                                 where (types.source_code.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.source_description.ToLower().Contains(payload.search_key.ToLower())) &&
                                 types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            //_result.total_count = _result.value.Count();
            //if (payload.page_index != -1)
            //{
            //    _result.page_count = _result.value.Count().GetPageCount();
            //    _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
            //                         Take(Constant.AppSettings.PageItemCount);
            //}
            return _result;
        }

        public Result<IEnumerable<SourceFunds>> CreateSourceFunds(SourceFunds sourceFunds)
        {
            var _result = new Result<IEnumerable<SourceFunds>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    sourceFunds.status = Constant.RecordStatus.Active;
                    sourceFunds.created_date = DateTime.Now;
                    sourceFunds.updated_date = DateTime.Now;

                    db.SourceFunds.Add(sourceFunds);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.SourceFunds
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

        public Result<IEnumerable<SourceFunds>> UpdateSourceFunds([FromBody] SourceFunds sourceFunds)
        {
            var _result = new Result<IEnumerable<SourceFunds>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    sourceFunds.updated_date = DateTime.Now;

                    db.SourceFunds.AddOrUpdate(sourceFunds);
                    db.SaveChanges();

                    _result = GetSourceFunds(new Payload() {});
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

        public Result<IEnumerable<SourceFunds>> StatusUpdateSourceFunds([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<SourceFunds>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _SourceFunds = db.SourceFunds.Find(Convert.ToInt32(id));
                            _SourceFunds.status = payload.status;
                            _SourceFunds.updated_date = DateTime.Now;
                            _SourceFunds.updated_by = payload.user_id;
                            db.SourceFunds.AddOrUpdate(_SourceFunds);
                        }
                    }
                    db.SaveChanges();

                    _result = GetSourceFunds(new Payload() { page_index = payload.page_index });
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
