using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.DTS;
using OPBids.Service.Data;
using OPBids.Service.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.DTS
{
    public class RequestBarcodeLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<RequestBarcodeVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<RequestBarcodeVM>> rslts;
            switch (param.requestBarcode.process.ToSafeString().ToUpper())
            {
                case Constant.TransactionType.Search:
                    rslts = GetData(param, null);
                    break;
                case Constant.TransactionType.Create:
                    rslts = Create(param);
                    break;
                case Constant.TransactionType.Update:
                    rslts = Update(param);
                    break;
                case Constant.TransactionType.StatusUpdate:
                    rslts = UpdateStatus(param);
                    break;
                default:
                    rslts = GetData(param, null);
                    break;
            }
            return rslts;
        }

        public Result<IEnumerable<RequestBarcodeVM>> GetData(DocumentsPayload payload, string additionalInfo)
        {
            var _result = new Result<IEnumerable<RequestBarcodeVM>>();
            _result.value = (from d in db.RequestBarcodes
                             join a1 in db.AccessUser on d.created_by equals a1.id into a2
                             from a3 in a2.DefaultIfEmpty()
                             join s1 in db.Departments on a3.dept_id equals s1.id into s4
                             from s5 in s4.DefaultIfEmpty()
                             where ((payload.filter.id == 0 || d.id == payload.filter.id) &&
                             (payload.filter.request_status == null || d.status == payload.filter.request_status) &&
                             (payload.filter.requested_by == null || a3.first_name.Contains(payload.filter.requested_by) ||
                             a3.last_name.Contains(payload.filter.requested_by)) &&
                             (payload.filter.date_requested_from == null || d.created_date >= payload.filter.date_requested_from) &&
                             (payload.filter.date_requested_to == null || d.created_date <= payload.filter.date_requested_to))
                             select new RequestBarcodeVM()
                             {
                                 department = s5.dept_code.ToString(),
                                 id = d.id,
                                 department_id = s5.id,
                                 status = d.status,
                                 created_by = d.created_by,
                                 printed_quantity = d.printed_quantity,
                                 requested_by = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                                 created_by_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                                 remarks = d.remarks,
                                 requested_quantity = d.requested_quantity,
                                 created_date = d.created_date.ToString(),
                                 updated_by = d.updated_by,
                                 updated_date = d.updated_date.ToString()
                             }).ToList();
            _result.total_count = _result.value.Count();
            if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
                _result.status.description = additionalInfo ?? _result.status.description;
            }
            return _result;
        }

        public Result<IEnumerable<RequestBarcodeVM>> Create(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<RequestBarcodeVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.created_date = DateTime.Today;
                    param.updated_date = DateTime.Today;
                    var itm = new RequestBarcode()
                    {
                        status = param.requestBarcode.status,
                        updated_by = param.requestBarcode.updated_by,
                        updated_date = param.updated_date,
                        created_by = param.requestBarcode.created_by,
                        created_date = param.created_date,
                        id = param.id,
                        printed_quantity = param.requestBarcode.printed_quantity,
                        remarks = param.requestBarcode.remarks,
                        requested_quantity = param.requestBarcode.requested_quantity,
                    };
                    db.RequestBarcodes.Add(itm);
                    db.SaveChanges();
                    param.filter.id = itm.id;
                    return GetData(param, null);
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

        public Result<IEnumerable<RequestBarcodeVM>> Update([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<RequestBarcodeVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    var item = db.RequestBarcodes.Find(param.requestBarcode.id);
                    item.updated_by = param.requestBarcode.updated_by;
                    item.updated_date = param.updated_date;
                    item.printed_quantity = param.requestBarcode.printed_quantity.ToSafeInt() + item.printed_quantity.ToSafeInt();
                    item.remarks = param.requestBarcode.remarks;
                    item.status = param.requestBarcode.status;
                    item.requested_quantity = param.requestBarcode.requested_quantity;
                    db.RequestBarcodes.AddOrUpdate(item);
                    db.SaveChanges();
                    param.filter.id = param.requestBarcode.id;
                    var itmId = new List<int>();
                    if (param.requestBarcode.printed_quantity.ToSafeInt() > 0)
                    {
                        var counter = 0;
                        while (counter < param.requestBarcode.printed_quantity.ToSafeInt())
                        {
                            var itm = new PrintedBarcodes()
                            {
                                created_by = param.requestBarcode.updated_by,
                                created_date = DateTime.Now,
                                request_barcode_id = param.requestBarcode.id,
                                updated_by = param.requestBarcode.updated_by,
                                updated_date = DateTime.Now
                            };
                            db.PrintedBarcodes.Add(itm);
                            db.SaveChanges();
                            itmId.Add(db.PrintedBarcodes.Where(a => a.request_barcode_id == param.requestBarcode.id).Max(a => a.id));
                            counter++;
                        }
                    }
                    int? minItmId = null;
                    int? maxItmId = null;
                    if (itmId.Count() > 0)
                    {
                        minItmId = itmId.Min();
                        maxItmId = itmId.Max();
                    }
                    return GetData(param, string.Concat(minItmId.ToSafeString(), ",", maxItmId.ToSafeString()));
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
        public Result<IEnumerable<RequestBarcodeVM>> UpdateStatus([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<RequestBarcodeVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    var item = db.RequestBarcodes.Find(param.requestBarcode.id);
                    item.updated_by = param.requestBarcode.updated_by;
                    item.updated_date = param.updated_date;
                    item.status = param.requestBarcode.status;
                    db.RequestBarcodes.AddOrUpdate(item);
                    db.SaveChanges();
                    param.filter.id = param.requestBarcode.id;
                    return GetData(param, null);
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