using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.DTS;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Suppliers;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.ProjectRequest;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.Suppliers
{
    public class ProjectAttachmentLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<ProjectRequestAttachmentVM>> MaintainData(SupplierPayloadVM param)
        {
            Result<IEnumerable<ProjectRequestAttachmentVM>> rslts;
            switch (param.documentAttachment.process.ToUpper().Trim())
            {
                case Constant.TransactionType.Search:
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

        public Result<IEnumerable<ProjectRequestAttachmentVM>> GetData(SupplierPayloadVM payload)
        {
            var _result = new Result<IEnumerable<ProjectRequestAttachmentVM>>();
            _result.value = (from d in db.ProjectRequestAttachments
                             join a1 in db.AccessUser on d.created_by equals a1.id into a2
                             from a3 in a2.DefaultIfEmpty()
                             join b1 in db.AccessUser on d.updated_by equals b1.id into b2
                             from b3 in b2.DefaultIfEmpty()
                             where d.project_id == payload.documentAttachment.project_id &&
                             (d.status == Constant.RecordStatus.Active || d.status == Constant.RecordStatus.Validated)
                             select new ProjectRequestAttachmentVM()
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
                                 project_id = d.project_id,
                                 updated_date = d.updated_date.ToString()
                             }).ToList();
            return _result;
        }

        public Result<IEnumerable<ProjectRequestAttachmentVM>> Save(SupplierPayloadVM param)
        {
            var _result = new Result<IEnumerable<ProjectRequestAttachmentVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.documentAttachments.ForEach(a => {
                        param.documentAttachment.id = a.id;
                        if (a.id == 0)
                        {
                            db.ProjectRequestAttachments.Add(
                            new ProjectRequestAttachment()
                            {
                                project_id = a.project_id,
                                status = Constant.RecordStatus.Active,
                                updated_by = a.updated_by,
                                batch_id = a.batch_id,
                                attachment_name = a.attachment_name,
                                updated_date = DateTime.Now,
                                created_by = a.created_by,
                                created_date = DateTime.Now,
                                file_name = a.file_name,
                                barcode_no = a.barcode_no
                            });
                        }
                        else
                        {
                            var itm = db.ProjectRequestAttachments.FirstOrDefault(b => b.id == param.documentAttachment.id);
                            itm.project_id = a.project_id;
                            itm.status = a.status == null ? Constant.RecordStatus.Active : a.status;
                            itm.batch_id = a.batch_id;
                            itm.updated_by = a.updated_by;
                            itm.attachment_name = a.attachment_name;
                            itm.updated_date = DateTime.Now;
                            itm.file_name = a.file_name;
                            itm.barcode_no = a.barcode_no;
                            db.ProjectRequestAttachments.AddOrUpdate(itm);
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