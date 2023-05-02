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
using OPBids.Entities.View.Setting;

namespace OPBids.Service.Logic.Settings
{
    public class DocumentCategoryLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<DocumentCategory>> GetDocumentCategory(DocumentCategoryVM payload)
        {

            var _result = new Result<IEnumerable<DocumentCategory>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.DocumentCategory
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.DocumentCategory
                                 where (types.document_category_name.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.document_category_code.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<DocumentCategory>> CreateDocumentCategory(DocumentCategory DocumentCategory)
        {
            var _result = new Result<IEnumerable<DocumentCategory>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    DocumentCategory.status = Constant.RecordStatus.Active;
                    DocumentCategory.created_date = DateTime.Now;
                    DocumentCategory.updated_date = DateTime.Now;

                    db.DocumentCategory.Add(DocumentCategory);
                    db.SaveChanges();

                    //Select all records
                    _result = GetDocumentCategory(new DocumentCategoryVM() { });
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

        public Result<IEnumerable<DocumentCategory>> UpdateDocumentCategory([FromBody] DocumentCategoryVM param)
        {
            var _result = new Result<IEnumerable<DocumentCategory>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    //DocumentCategory.updated_date = DateTime.Now;
                    //db.DocumentCategory.AddOrUpdate(DocumentCategory);
                    //db.SaveChanges();

                    db.DocumentCategory.AddOrUpdate(new DocumentCategory()
                    {
                        updated_date = DateTime.Now,
                        updated_by = param.updated_by,
                        document_category_code = param.document_category_code,
                        document_category_name = param.document_category_name,
                        id = param.id,
                        status = param.status,
                    });
                    db.SaveChanges();
                    _result = GetDocumentCategory(param);
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

        public Result<IEnumerable<DocumentCategory>> StatusUpdateDocumentCategory([FromBody] DocumentCategoryVM payload)
        {
            var _result = new Result<IEnumerable<DocumentCategory>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _DocumentCategory = db.DocumentCategory.Find(Convert.ToInt32(id));
                            _DocumentCategory.status = payload.status;
                            _DocumentCategory.updated_date = DateTime.Now;
                            _DocumentCategory.updated_by = payload.updated_by;
                            db.DocumentCategory.AddOrUpdate(_DocumentCategory);
                        }
                    }
                    db.SaveChanges();
                    _result = GetDocumentCategory(payload);
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