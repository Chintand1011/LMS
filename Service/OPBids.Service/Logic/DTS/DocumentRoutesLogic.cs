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
    public class DocumentRoutesLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<DocumentRoutesVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<DocumentRoutesVM>> rslts;
            if (param.documentRoute != null)
            {
                switch (param.documentRoute.process.ToUpper().Trim())
                {
                    case Constant.TransactionType.Search:
                        rslts = GetData(param);
                        break;
                    case Constant.TransactionType.GetRoutesBar:
                        rslts = GetRoutesBar(param);
                        break;
                    case Constant.TransactionType.UpdateSequence:
                        rslts = UpdateSequence(param);
                        break;
                    case Constant.TransactionType.Save:
                        rslts = Save(param);
                        break;
                    default:
                        rslts = GetData(param);
                        break;
                }
                return rslts;
            }
            return null;
        }

        public Result<IEnumerable<DocumentRoutesVM>> GetData(DocumentsPayload payload)
        {
            var _result = new Result<IEnumerable<DocumentRoutesVM>>();
            _result.value = (from d in db.DocumentRoutes
                             join a1 in db.AccessUser on d.receipient_id equals a1.id into a2
                             from a3 in a2.DefaultIfEmpty()
                             join b1 in db.Departments on d.department_id equals b1.id into b2
                             from b3 in b2.DefaultIfEmpty()
                             where d.batch_id == payload.documentRoute.batch_id && d.is_sender == false &&
                             (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.Validated)
                             select new DocumentRoutesVM()
                             {
                                 id = d.id,
                                 status = d.status,
                                 receipient_id = d.receipient_id,
                                 receipient_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                                 department_id = d.department_id,
                                 department_name = b3.dept_code,
                                 sequence = d.sequence,
                                 current_receiver = d.current_receiver,
                             }).ToList();
            return _result;
        }

        public Result<IEnumerable<DocumentRoutesVM>> GetRoutesBar(DocumentsPayload payload)
        {
            var _result = new Result<IEnumerable<DocumentRoutesVM>>();
            var dr = new List<DocumentRoutesVM>();
            dr.AddRange(
                from d in db.DocumentRoutes
                join a1 in db.AccessUser on d.receipient_id equals a1.id into a2
                from a3 in a2.DefaultIfEmpty()
                join b1 in db.Departments on d.department_id equals b1.id into b2
                from b3 in b2.DefaultIfEmpty()
                where d.batch_id == payload.documentRoute.batch_id && d.is_sender == false &&
                (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.Validated)
                select new DocumentRoutesVM()
                {
                    id = d.id,
                    status = d.status,
                    receipient_id = d.receipient_id,
                    receipient_name = d.sequence == null ? " " : string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                    department_id = d.department_id,
                    department_name = d.sequence == null ? " " : b3.dept_code,
                    sequence = (d.sequence == null ? 1000 : d.sequence + 1),
                    updated_date = (d.sequence == null ? "" : d.updated_date.ToString()),
                    current_receiver = d.current_receiver,
                });
            dr.AddRange(
                from d in db.OnHandDocuments
                join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                equals new { t1 = s.id, t2 = s.status } into s1
                from s2 in s1.DefaultIfEmpty()
                join a1 in db.AccessUser on s2.user_id equals a1.id into a2
                from a3 in a2.DefaultIfEmpty()
                join b1 in db.Departments on a3.dept_id equals b1.id into b2
                from b3 in b2.DefaultIfEmpty()
                where d.id == payload.documentRoute.batch_id
                select new DocumentRoutesVM()
                {
                    id = 0,
                    status = d.status,
                    receipient_id = d.receipient_id,
                    receipient_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                    department_id = a3 == null ? 0 : a3.dept_id,
                    department_name = b3.dept_code,
                    sequence = 1,
                    updated_date = a3 == null ? "" : d.created_date.ToString()
                });
            dr.AddRange(
                from d in db.ReceivedDocuments
                join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                equals new { t1 = s.id, t2 = s.status } into s1
                from s2 in s1.DefaultIfEmpty()
                join a1 in db.AccessUser on s2.user_id equals a1.id into a2
                from a3 in a2.DefaultIfEmpty()
                join b1 in db.Departments on a3.dept_id equals b1.id into b2
                from b3 in b2.DefaultIfEmpty()
                where d.batch_id == payload.documentRoute.batch_id
                select new DocumentRoutesVM()
                {
                    id = 0,
                    status = d.status,
                    receipient_id = d.receipient_id,
                    receipient_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                    department_id = a3 == null ? 0 : a3.dept_id,
                    department_name = b3.dept_code,
                    sequence = 1,
                    updated_date = a3 == null ? "" : d.created_date.ToString()
                });
            dr.AddRange(
                from d in db.FinalizedDocuments
                join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                equals new { t1 = s.id, t2 = s.status } into s1
                from s2 in s1.DefaultIfEmpty()
                join a1 in db.AccessUser on s2.user_id equals a1.id into a2
                from a3 in a2.DefaultIfEmpty()
                join b1 in db.Departments on a3.dept_id equals b1.id into b2
                from b3 in b2.DefaultIfEmpty()
                where d.batch_id == payload.documentRoute.batch_id
                select new DocumentRoutesVM()
                {
                    id = 0,
                    status = d.status,
                    receipient_id = d.receipient_id,
                    receipient_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                    department_id = a3 == null ? 0 : a3.dept_id,
                    department_name = b3.dept_code,
                    sequence = 1,
                    updated_date = a3 == null ? "" : d.created_date.ToString()
                });
            dr.AddRange(
                from d in db.ArchivedDocuments
                join s in db.SenderRecipient on new { t1 = d.sender_id, t2 = Constant.RecordStatus.Active }
                equals new { t1 = s.id, t2 = s.status } into s1
                from s2 in s1.DefaultIfEmpty()
                join a1 in db.AccessUser on s2.user_id equals a1.id into a2
                from a3 in a2.DefaultIfEmpty()
                join b1 in db.Departments on a3.dept_id equals b1.id into b2
                from b3 in b2.DefaultIfEmpty()
                where d.batch_id == payload.documentRoute.batch_id
                select new DocumentRoutesVM()
                {
                    id = 0,
                    status = d.status,
                    receipient_id = d.receipient_id,
                    receipient_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                    department_id = a3 == null ? 0 : a3.dept_id,
                    department_name = b3.dept_code,
                    sequence = 1,
                    updated_date = a3 == null ? "" : d.created_date.ToString(),
                });
            _result.value = dr.OrderBy(a => a.sequence);
            return _result;
        }
        public Result<IEnumerable<DocumentRoutesVM>> Save(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentRoutesVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.created_date = DateTime.Now;
                    param.updated_date = DateTime.Now;
                    param.id = 0;

                    param.documentRoutes.ForEach(a =>
                    {
                        if (a.id == 0)
                        {
                            db.DocumentRoutes.Add(
                            new DocumentRoutes()
                            {
                                status = Constant.RecordStatus.Active,
                                batch_id = a.batch_id,
                                is_sender = false,
                                updated_by = a.updated_by,
                                updated_date = param.updated_date,
                                created_by = a.created_by,
                                created_date = param.created_date,
                                department_id = a.department_id,
                                receipient_id = a.receipient_id,
                                current_receiver = a.current_receiver,
                            });
                        }
                        else
                        {
                            db.DocumentRoutes.AddOrUpdate(
                            new DocumentRoutes()
                            {
                                id = a.id,
                                status = a.status == null ? Constant.RecordStatus.Active : a.status,
                                batch_id = a.batch_id,
                                updated_by = a.updated_by,
                                is_sender = false,
                                updated_date = param.updated_date,
                                department_id = a.department_id,
                                receipient_id = a.receipient_id,
                                current_receiver = a.current_receiver,
                            });
                        }
                    });
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
        public Result<IEnumerable<DocumentRoutesVM>> UpdateSequence(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentRoutesVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.DocumentRoutes.Where(a => a.batch_id == param.documentRoute.batch_id).ToList().ForEach(a =>
                    {
                        a.current_receiver = false;
                        db.DocumentRoutes.AddOrUpdate(a);
                    });
                    db.SaveChanges();
                    var itm = db.DocumentRoutes.Where(a => a.batch_id == param.documentRoute.batch_id && a.department_id == param.documentRoute.department_id).FirstOrDefault();
                    itm.sequence = db.DocumentRoutes.Where(a => a.batch_id == param.documentRoute.batch_id).Max(b => b.sequence ?? 0);
                    itm.sequence = itm.sequence + 1;
                    itm.updated_by = param.documentRoute.updated_by;
                    itm.updated_date = DateTime.Now;
                    itm.current_receiver = true;
                    db.DocumentRoutes.AddOrUpdate(itm);
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