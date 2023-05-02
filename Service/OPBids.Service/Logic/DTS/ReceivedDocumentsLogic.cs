﻿using OPBids.Common;
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
    public class ReceivedDocumentsLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<DocumentsVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<DocumentsVM>> rslts;
            switch (param.document.process.ToSafeString().ToUpper().Trim())
            {
                case Constant.TransactionType.Search:
                case Constant.TransactionType.DashBoard:
                    rslts = GetData(param);
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
                case Constant.TransactionType.Promote:
                    rslts = Promote(param);
                    break;
                default:
                    rslts = GetData(param);
                    break;
            }
            return rslts;
        }

        public Result<IEnumerable<DocumentsVM>> GetData(DocumentsPayload payload)
        {
            if (payload.filter.etd_to != null)
            {
                payload.filter.etd_to = payload.filter.etd_to.Value.AddDays(1);
            }
            if (payload.filter.date_submitted_to != null)
            {
                payload.filter.date_submitted_to = payload.filter.date_submitted_to.Value.AddDays(1);
            }
            var _result = new Result<IEnumerable<DocumentsVM>>();
            _result.value = (from d in db.ReceivedDocuments
                             join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                             equals new { t1 = s.id, t2 = s.status } into s1
                             from s2 in s1.DefaultIfEmpty()
                             join s3 in db.SenderRecipientUser on new { t1 = s2.user_id, t2 = s2.is_system_user }
                             equals new { t1 = s3.id, t2 = false } into s4
                             from s5 in s4.DefaultIfEmpty()
                             join a1 in db.AccessUser on new { t1 = s2.user_id, t2 = Constant.RecordStatus.Active, t3 = s2.is_system_user }
                             equals new { t1 = a1.id, t2 = a1.status, t3 = true } into a2
                             from a3 in a2.DefaultIfEmpty()
                             join a1d in db.Departments on a3.dept_id equals a1d.id into a2d
                             from a3d in a2d.DefaultIfEmpty()
                             join r in db.SenderRecipient on new { t1 = d.receipient_id, t2 = Constant.RecordStatus.Active }
                             equals new { t1 = r.id, t2 = r.status } into r1
                             from r2 in r1.DefaultIfEmpty()
                             join r3 in db.SenderRecipientUser on new { t1 = r2.user_id, t2 = r2.is_system_user }
                             equals new { t1 = r3.id, t2 = false } into r4
                             from r5 in r4.DefaultIfEmpty()
                             join ra1 in db.AccessUser on new { t1 = r2.user_id, t2 = Constant.RecordStatus.Active, t3 = r2.is_system_user }
                             equals new { t1 = ra1.id, t2 = ra1.status, t3 = true } into ra2
                             from ra3 in ra2.DefaultIfEmpty()
                             join cat in db.DocumentCategory on new { t1 = d.category_id, t2 = Constant.RecordStatus.Active } equals new { t1 = cat.id, t2 = cat.status } into c1
                             from c2 in c1.DefaultIfEmpty()
                             join dt in db.Delivery on new { t1 = d.delivery_type_id, t2 = Constant.RecordStatus.Active } equals new { t1 = dt.id, t2 = dt.status } into dt1
                             from dt2 in dt1.DefaultIfEmpty()
                             join ds in db.DocumentSecurityLevel on d.document_security_level_id equals ds.id into ds1
                             from ds2 in ds1.DefaultIfEmpty()
                             join dtype in db.DocumentType on d.document_type_id equals dtype.id into dtype1
                             from dtype2 in dtype1.DefaultIfEmpty()
                             where d.status == Constant.RecordStatus.Active && (payload.filter.id == 0 || d.batch_id == payload.filter.id) &&
                             (payload.filter.receipient_name == null || payload.filter.receipient_name.Replace(" ", "").Contains((r2.is_system_user ?
                             string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)).Replace(" ", ""))) &&
                             (payload.filter.sender_name == null || payload.filter.sender_name.Replace(" ", "").Contains((s2.is_system_user ?
                             string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)).Replace(" ", ""))) &&
                             (payload.filter.category_name == null || payload.filter.category_name.Contains(c2.document_category_code)) &&
                             (payload.filter.document_code == null || d.document_code.ToLower().Contains(payload.filter.document_code.ToLower())) &&
                             (payload.filter.document_type_name == null || payload.filter.document_type_name.Contains(dtype2.document_type_code)) &&
                             (payload.filter.etd_from == null || d.etd_to_recipient >= payload.filter.etd_from) &&
                             (payload.filter.etd_to == null || d.etd_to_recipient <= payload.filter.etd_to) &&
                             (payload.filter.date_submitted_from == null || d.created_date >= payload.filter.date_submitted_from) &&
                             (payload.filter.date_submitted_to == null || d.created_date <= payload.filter.date_submitted_to) &&
                             (payload.filter.barcode_no == null || db.DocumentAttachment.Any(c => c.barcode_no == payload.filter.barcode_no && c.batch_id == d.batch_id)) &&
                             (db.DocumentRoutes.Any(b => b.batch_id == d.batch_id && b.current_receiver == true && ((b.department_id == payload.filter.department_id &&
                             (ds2.id == 1 || ds2.id == 2 || ds2.id == 4)) || ((ds2.id == 3 || ds2.id == 5 || ds2.id == 6) && b.receipient_id == payload.filter.created_by))))
                             select new DocumentsVM()
                             {
                                 sender_department_id = a3.dept_id,
                                 id = d.batch_id,
                                 status = d.status,
                                 delivery_type_id = dt2 == null ? 0 : dt2.id,
                                 category_code = c2 == null ? "" : c2.document_category_code,
                                 category_id = c2 == null ? 0 : c2.id,
                                 category_name = c2.document_category_name,
                                 created_by = d.created_by,
                                 created_date = d.created_date.ToString(),
                                 is_edoc = d.is_edoc,
                                 delivery_type_name = dt2.delivery_code,
                                 document_code = d.document_code,
                                 document_security_level = ds2.description,
                                 document_security_level_id = d.document_security_level_id,
                                 document_type_id = d.document_type_id,
                                 document_type_name = dtype2.document_type_code,
                                 etd_to_recipient = d.etd_to_recipient.ToString(),
                                 receipient_id = d.receipient_id,
                                 receipient_name = (r2.is_system_user ? string.Concat(ra3.first_name, " ", ra3.mi, " ", ra3.last_name) : string.Concat(r5.first_name, " ", r5.mi, " ", r5.last_name)),
                                 sender_id = d.sender_id,
                                 sender_name = (s2.is_system_user ? string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name) : string.Concat(s5.first_name, " ", s5.mi, " ", s5.last_name)),
                                 updated_by = d.updated_by,
                                 updated_date = d.updated_date.ToString(),
                                 sort_date = d.created_date,
                                 sender_department_name = a3d.dept_code,
                             }).ToList();
            _result.total_count = _result.value.Count();
            if (payload.document.process.ToSafeString().ToUpper().Trim() == Constant.TransactionType.DashBoard)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.OrderBy(a => a.sort_date).Take(10);
            }
            else if (payload.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * payload.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
            }
            return _result;
        }

        public Result<IEnumerable<DocumentsVM>> Create(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.created_date = DateTime.Now;
                    param.updated_date = DateTime.Now;
                    var itm = new ReceivedDocuments()
                    {
                        etd_to_recipient = param.document.etd_to_recipient.ToDate(),
                        receipient_id = param.document.receipient_id,
                        sender_id = param.document.sender_id,
                        status = param.document.status,
                        is_edoc = param.document.is_edoc,
                        updated_by = param.document.updated_by,
                        updated_date = param.updated_date,
                        batch_id = param.document.id,
                        category_id = param.document.category_id,
                        created_by = param.document.created_by,
                        created_date = param.created_date,
                        delivery_type_id = param.document.delivery_type_id,
                        document_code = param.document.document_code,
                        document_security_level_id = param.document.document_security_level_id,
                        document_type_id = param.document.document_type_id
                    };
                    db.ReceivedDocuments.Add(itm);
                    db.SaveChanges();
                    param.filter.id = itm.id;
                    return GetData(param);
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

        public Result<IEnumerable<DocumentsVM>> Update([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    db.ReceivedDocuments.AddOrUpdate(new ReceivedDocuments()
                    {
                        etd_to_recipient = param.document.etd_to_recipient.ToDate(),
                        receipient_id = param.document.receipient_id,
                        is_edoc = param.document.is_edoc,
                        sender_id = param.document.sender_id,
                        status = param.document.status,
                        updated_by = param.document.updated_by,
                        updated_date = param.updated_date,
                        category_id = param.document.category_id,
                        delivery_type_id = param.document.delivery_type_id,
                        document_code = param.document.document_code,
                        document_security_level_id = param.document.document_security_level_id,
                        document_type_id = param.document.document_type_id,
                        id = param.document.id
                    });
                    db.SaveChanges();
                    param.filter.id = param.document.id;
                    return GetData(param);
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
        public Result<IEnumerable<DocumentsVM>> UpdateStatus([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.updated_date = DateTime.Now;
                    param.id = 0;
                    var item = db.ReceivedDocuments.Find(param.document.id);
                    item.status = param.document.status;
                    db.ReceivedDocuments.AddOrUpdate(item);
                    db.SaveChanges();
                    return GetData(param);
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
        public Result<IEnumerable<DocumentsVM>> Promote([FromBody] DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    var itm = db.ReceivedDocuments.FirstOrDefault(a => a.batch_id == param.document.id);
                    db.FinalizedDocuments.Add(new FinalizedDocuments()
                    {
                        category_id = itm.category_id,
                        is_edoc = itm.is_edoc,
                        created_by = itm.created_by,
                        created_date = itm.created_date,
                        delivery_type_id = itm.delivery_type_id,
                        document_code = itm.document_code,
                        document_security_level_id = itm.document_security_level_id,
                        document_type_id = itm.document_type_id,
                        etd_to_recipient = itm.etd_to_recipient,
                        batch_id = itm.batch_id,
                        receipient_id = itm.receipient_id,
                        sender_id = itm.sender_id,
                        status = itm.status,
                        updated_by = itm.updated_by,
                        updated_date = DateTime.Now,
                        date_finalized = DateTime.Now,
                        dept_finalized = param.document.dept_processed,
                        date_archived = DateTime.Now
                    });
                    db.ReceivedDocuments.Remove(itm);
                    db.SaveChanges();
                    return GetData(param);
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