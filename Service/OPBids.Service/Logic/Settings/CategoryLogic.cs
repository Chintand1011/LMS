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
    public class CategoryLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<Category>> GetCategory(Payload payload)
        {
            var _result = new Result<IEnumerable<Category>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.Category
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.Category
                                 where (types.cat_code.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.cat_name.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<Category>> CreateCategory(Category category)
        {
            var _result = new Result<IEnumerable<Category>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    category.status = Constant.RecordStatus.Active;
                    category.created_date = DateTime.Now;
                    category.updated_date = DateTime.Now;

                    db.Category.Add(category);
                    db.SaveChanges();
                    //Select all records
                    _result = GetCategory(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<Category>> UpdateCategory([FromBody] Category category)
        {
            var _result = new Result<IEnumerable<Category>>();
            try
            {
                using (var db = new DatabaseContext())
                {

                    category.updated_date = DateTime.Now;
                    db.Category.AddOrUpdate(category);
                    db.SaveChanges();
                    _result = GetCategory(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<Category>> StatusUpdateCategory([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<Category>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (var id in payload.item_list)
                        {
                            var _Category = db.Category.Find(Convert.ToInt32(id));
                            _Category.status = payload.status;
                            _Category.updated_date = DateTime.Now;
                            _Category.updated_by = payload.user_id;
                            db.Category.AddOrUpdate(_Category);
                        }
                    }
                    db.SaveChanges();
                    _result = GetCategory(new Payload() { page_index = payload.page_index });
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