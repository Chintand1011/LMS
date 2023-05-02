using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
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
    public class DocumentTypeLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<DocumentTypeVM>> GetDocumentType(DocumentTypeVM param)
        {
            var _result = new Result<IEnumerable<DocumentTypeVM>>();
            if (param.search_key == null || param.search_key == string.Empty)
            {
                _result.value = (from a in db.DocumentType
                                 join b in db.DocumentCategory on a.document_category_id equals b.id into dc
                                 from dcu in dc.DefaultIfEmpty()
                                 where a.status != Constant.RecordStatus.Deleted
                                 select new DocumentTypeVM (){created_by = a.created_by, created_date = a.created_date.ToString(),
                                     document_category_id = a.document_category_id,
                                     document_category_code = dcu.document_category_code,
                                     document_category_name = dcu.document_category_name,
                                     document_type_code = a.document_type_code, document_type_description = a.document_type_description,
                                     id = a.id, status = a.status, updated_by = a.updated_by, updated_date = a.updated_date.ToString()}).ToList();
            }
            else
            {
                _result.value = (from a in db.DocumentType
                                 join b in db.DocumentCategory on a.document_category_id equals b.id into dc
                                 from dcu in dc.DefaultIfEmpty()
                                 where (a.document_type_description.ToLower().Contains(param.search_key.ToLower()) ||
                                 a.document_type_code.ToLower().Contains(param.search_key.ToLower())) &&
                                 a.status != Constant.RecordStatus.Deleted
                                 select new DocumentTypeVM(){created_by = a.created_by, created_date = a.created_date.ToString(),
                                     document_category_id = a.document_category_id,
                                     document_category_code = dcu.document_category_code,
                                     document_category_name = dcu.document_category_name,
                                     document_type_code = a.document_type_code, document_type_description = a.document_type_description,
                                     id = a.id, status = a.status, updated_by = a.updated_by, updated_date = a.updated_date.ToString()
                                 }).ToList();
            }
            _result.total_count = _result.value.Count();
            if (param.page_index != -1)
            {
                _result.page_count = _result.value.Count().GetPageCount();
                _result.value = _result.value.Skip(Constant.AppSettings.PageItemCount * param.page_index).
                                     Take(Constant.AppSettings.PageItemCount);
            }
            return _result;
        }

        public Result<IEnumerable<DocumentTypeVM>> CreateDocumentType(DocumentTypeVM param)
        {
            var _result = new Result<IEnumerable<DocumentTypeVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.DocumentType.Add(new DocumentType()
                    {
                        updated_date = DateTime.Now,
                        updated_by = param.updated_by,
                        status = Constant.RecordStatus.Active,
                        created_by = param.created_by,
                        created_date = DateTime.Now,
                        document_category_id = param.document_category_id,
                        document_type_code = param.document_type_code,
                        document_type_description = param.document_type_description,
                        id = param.id
                    });
                    db.SaveChanges();

                    //Select all records
                    //  _result = GetDocumentType(new Payload() { });
                    _result = GetDocumentType(new DocumentTypeVM() { page_index = param.page_index } );
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

        public Result<IEnumerable<DocumentTypeVM>> UpdateDocumentType([FromBody] DocumentTypeVM param)
        {
            var _result = new Result<IEnumerable<DocumentTypeVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.DocumentType.AddOrUpdate(new DocumentType() {updated_date = DateTime.Now, updated_by = param.updated_by, status = param.status,
                        created_by = param.created_by, created_date = DateTime.Now, document_category_id = param.document_category_id,
                        document_type_code = param.document_type_code, document_type_description = param.document_type_description, id = param.id } );
                    db.SaveChanges();
                    _result = GetDocumentType(param);
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

        public Result<IEnumerable<DocumentTypeVM>> StatusUpdateDocumentType([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<DocumentTypeVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _DocumentType = db.DocumentType.Find(Convert.ToInt32(id));
                            _DocumentType.status = payload.status;
                            _DocumentType.updated_date = DateTime.Now;
                            _DocumentType.updated_by = payload.user_id;
                            db.DocumentType.AddOrUpdate(_DocumentType);
                        }
                    }
                    db.SaveChanges();
                    _result = GetDocumentType(new DocumentTypeVM() { });
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