﻿using OPBids.Common;
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
    public class RecordCategoryLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<RecordCategory>> GetRecordCategory(Payload payload)
        {
            var _result = new Result<IEnumerable<RecordCategory>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.RecordCategory
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.RecordCategory
                                 where (types.category_code.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.category_desc.ToLower().Contains(payload.search_key.ToLower()))
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

        public Result<IEnumerable<RecordCategory>> CreateRecordCategory(RecordCategory category)
        {
            var _result = new Result<IEnumerable<RecordCategory>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    category.created_date = DateTime.Now;
                    category.updated_date = DateTime.Now;

                    db.RecordCategory.Add(category);
                    db.SaveChanges();
                    //Select all records
                    _result = GetRecordCategory(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
            }
            return _result;
        }

        public Result<IEnumerable<RecordCategory>> UpdateRecordCategory([FromBody] RecordCategory category)
        {
            var _result = new Result<IEnumerable<RecordCategory>>();
            try
            {
                using (var db = new DatabaseContext())
                {

                    category.updated_date = DateTime.Now;
                    db.RecordCategory.AddOrUpdate(category);
                    db.SaveChanges();
                    _result = GetRecordCategory(new Payload() { });
                }
            }
            catch (Exception ex)
            {
                //TODO: Exception handling
                _result.status = new Status() { code = Constant.Status.Failed, description = ex.Message };
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