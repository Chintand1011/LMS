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
    public class DocumentLogsLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public Result<IEnumerable<DocumentLogsVM>> MaintainData(DocumentsPayload param)
        {
            Result<IEnumerable<DocumentLogsVM>> rslts;
            switch (param.documentLog.process.ToUpper().Trim())
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

        public Result<IEnumerable<DocumentLogsVM>> GetData(DocumentsPayload payload)
        {
            var _result = new Result<IEnumerable<DocumentLogsVM>>();
            _result.value = (from d in db.DocumentLogs
                             join a1 in db.AccessUser on d.receipient_id equals a1.id into a2
                             from a3 in a2.DefaultIfEmpty()
                             where d.batch_id == payload.documentLog.batch_id
                             select new DocumentLogsVM()
                             {
                                 id = d.id,
                                 batch_id = d.batch_id,
                                 status = d.status,
                                 receipient_id = d.receipient_id,
                                 receipient_name = string.Concat(a3.first_name, " ", a3.mi, " ", a3.last_name),
                                 remarks = d.remarks,
                                 log_date = d.log_date.ToString(),
                                 sort_date = d.log_date,
                             }).OrderByDescending(a => a.sort_date).ToList();
            return _result;
        }

        public Result<IEnumerable<DocumentLogsVM>> Save(DocumentsPayload param)
        {
            var _result = new Result<IEnumerable<DocumentLogsVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    param.id = 0;
                    param.documentLogs.ForEach(a => {
                        if (a.id == 0)
                        {
                            db.DocumentLogs.Add(
                            new DocumentLogs()
                            {
                                log_date = DateTime.Now,
                                remarks = a.remarks,
                                batch_id = a.batch_id,
                                status = Constant.RecordStatus.Active,
                                updated_by = a.updated_by,
                                updated_date = DateTime.Now,
                                created_by = a.created_by,
                                created_date = DateTime.Now,
                                receipient_id = a.receipient_id
                            });
                        }
                        else
                        {
                            db.DocumentLogs.AddOrUpdate(
                            new DocumentLogs()
                            {
                                id = a.id,
                                batch_id = a.batch_id,
                                remarks = a.remarks,
                                log_date = DateTime.Now,
                                status = a.status,
                                updated_by = a.updated_by,
                                updated_date = DateTime.Now,
                                receipient_id = a.receipient_id
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