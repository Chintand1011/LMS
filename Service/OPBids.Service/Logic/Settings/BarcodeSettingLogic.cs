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
    public class BarcodeSettingLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<BarcodeSetting>> GetBarcodeSetting(Payload payload)
        {
            var _result = new Result<IEnumerable<BarcodeSetting>>();
            _result.value = (from types in db.BarcodeSetting select types).ToList();
            _result.total_count = _result.value.Count();
            if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
            }
            return _result;
        }

        public Result<IEnumerable<BarcodeSetting>> CreateBarcodeSetting(BarcodeSetting barcodeSetting)
        {
            var _result = new Result<IEnumerable<BarcodeSetting>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    barcodeSetting.created_date = DateTime.Now;
                    barcodeSetting.updated_date = DateTime.Now;

                    db.BarcodeSetting.Add(barcodeSetting);
                    db.SaveChanges();

                    //Select all records
                    _result = GetBarcodeSetting(new Payload() { });
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

        public Result<IEnumerable<BarcodeSetting>> UpdateBarcodeSetting([FromBody] BarcodeSetting barcodeSetting)
        {
            var _result = new Result<IEnumerable<BarcodeSetting>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    barcodeSetting.updated_date = DateTime.Now;

                    db.BarcodeSetting.AddOrUpdate(barcodeSetting);
                    db.SaveChanges();

                    _result = GetBarcodeSetting(new Payload() { });
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