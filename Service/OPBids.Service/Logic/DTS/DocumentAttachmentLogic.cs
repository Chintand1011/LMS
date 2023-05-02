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
    public class DocumentAttachmentLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<DocumentAttachmentVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<DocumentAttachmentVM>> rslts;
            switch (param.documentAttachment.process.ToUpper().Trim())
            {
                case Constant.TransactionType.Search:
                case Constant.TransactionType.IsExists:
                    rslts = GetData(param);
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

        public Result<IEnumerable<DocumentAttachmentVM>> GetData(DocumentsPayload payload)
        {
            var bcdlst = new List<string>();
            if (payload.filter.barcode_no == null)
            {
                payload.filter.barcode_no = "";
            }
            else
            {
                bcdlst.AddRange(payload.filter.barcode_no.ToLower().Trim().Split(new char[] { ',' }));
            }
            var _result = new Result<IEnumerable<DocumentAttachmentVM>>();
            _result.value = (from d in db.DocumentAttachment
                             join a1 in db.AccessUser on d.created_by equals a1.id into a2
                             from a3 in a2.DefaultIfEmpty()
                             join b1 in db.AccessUser on d.updated_by equals b1.id into b2
                             from b3 in b2.DefaultIfEmpty()
                             where ((payload.documentAttachment.process == Constant.TransactionType.IsExists &&
                             d.batch_id != payload.documentAttachment.batch_id && bcdlst.Contains(d.barcode_no.ToLower())) ||
                             (payload.documentAttachment.process != Constant.TransactionType.IsExists && d.batch_id == payload.documentAttachment.batch_id)) &&
                             (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.Validated)
                             select new DocumentAttachmentVM()
                             {
                                 id = d.id,
                                 status = d.status,
                                 barcode_no = d.barcode_no,
                                 file_name = d.file_name,
                                 batch_id = d.batch_id,
                                 attachment_name = d.attachment_name,
                                 updated_by_name = b3.username,
                                 created_by_name = a3.username,
                                 created_by = d.created_by,
                                 created_date = d.created_date.ToString(),
                                 updated_by = d.updated_by,
                                 updated_date = d.updated_date.ToString()
                             }).ToList();
            return _result;
        }

        public Result<IEnumerable<DocumentAttachmentVM>> Save(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentAttachmentVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.created_date = DateTime.Now;
                    param.updated_date = DateTime.Now;
                    param.documentAttachment.batch_id = 0;

                    param.documentAttachments.ForEach(a => {
                        param.documentAttachment.batch_id = a.batch_id;
                        if (a.id == 0)
                        {
                            db.DocumentAttachment.Add(
                            new DocumentAttachment()
                            {
                                status = Constant.RecordStatus.Active,
                                updated_by = a.updated_by,
                                batch_id = a.batch_id,
                                attachment_name = a.attachment_name,
                                updated_date = param.updated_date,
                                created_by = a.created_by,
                                created_date = param.created_date,
                                file_name = a.file_name,
                                barcode_no = a.barcode_no
                            });
                        }
                        else
                        {
                            db.DocumentAttachment.AddOrUpdate(
                            new DocumentAttachment()
                            {
                                id = a.id,
                                status = a.status == null ? Constant.RecordStatus.Active : a.status,
                                batch_id = a.batch_id,
                                updated_by = a.updated_by,
                                attachment_name = a.attachment_name,
                                updated_date = param.updated_date,
                                file_name = a.file_name,
                                barcode_no = a.barcode_no
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