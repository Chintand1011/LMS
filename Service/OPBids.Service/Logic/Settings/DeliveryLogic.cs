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
    public class DeliveryLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<Delivery>> GetDelivery(Payload payload)
        {
            var _result = new Result<IEnumerable<Delivery>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.Delivery
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.Delivery
                                 where (types.delivery_code.ToLower().Contains(payload.search_key.ToLower()) || types.delivery_description.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<Delivery>> CreateDelivery(Delivery Delivery)
        {
            var _result = new Result<IEnumerable<Delivery>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    Delivery.status = Constant.RecordStatus.Active;
                    Delivery.created_date = DateTime.Now;
                    Delivery.updated_date = DateTime.Now;

                    db.Delivery.Add(Delivery);
                    db.SaveChanges();

                    //Select all records
                    _result = GetDelivery(new Payload() { });
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

        public Result<IEnumerable<Delivery>> UpdateDelivery([FromBody] Delivery Delivery)
        {
            var _result = new Result<IEnumerable<Delivery>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    Delivery.updated_date = DateTime.Now;

                    db.Delivery.AddOrUpdate(Delivery);
                    db.SaveChanges();
                    _result = GetDelivery(new Payload() { });
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

        public Result<IEnumerable<Delivery>> StatusUpdateDelivery([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<Delivery>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _Delivery = db.Delivery.Find(Convert.ToInt32(id));
                            _Delivery.status = payload.status;
                            _Delivery.updated_date = DateTime.Now;
                            _Delivery.updated_by = payload.user_id;
                            db.Delivery.AddOrUpdate(_Delivery);
                        }
                    }
                    db.SaveChanges();
                    _result = GetDelivery(new Payload() { page_index = payload.page_index });
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