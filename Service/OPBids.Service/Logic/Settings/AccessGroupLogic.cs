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
    public class AccessGroupLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<AccessGroup>> GetAccessGroup(Payload payload) {
            var _result = new Result<IEnumerable<AccessGroup>>();
            if (payload.search_key == null || payload.search_key == string.Empty) {
                _result.value = (from types in db.AccessGroup
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else {
                _result.value = (from types in db.AccessGroup
                                 where (types.group_code.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.group_description.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<AccessGroup>> CreateAccessGroup(AccessGroup accessGroup) {
            var _result = new Result<IEnumerable<AccessGroup>>();
            try {
                using (var db = new DatabaseContext()) {
                    accessGroup.status = Constant.RecordStatus.Active;
                    accessGroup.created_date = DateTime.Now;
                    accessGroup.updated_date = DateTime.Now;

                    db.AccessGroup.Add(accessGroup);
                    db.SaveChanges();

                    //Select all records
                    _result = GetAccessGroup(new Payload() { });
                }
            } catch (Exception ex) {
                //TODO: Exception handling
                _result.status = new Status()
                {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }

        public Result<IEnumerable<AccessGroup>> UpdateAccessGroup([FromBody] AccessGroup accessGroup) {
            var _result = new Result<IEnumerable<AccessGroup>>();
            try {
                using (var db = new DatabaseContext()) {
                    accessGroup.updated_date = DateTime.Now;

                    db.AccessGroup.AddOrUpdate(accessGroup);
                    db.SaveChanges();

                    _result = GetAccessGroup(new Payload() { });
                }
            }
            catch (Exception ex) {
                //TODO: Exception handling
                _result.status = new Status() {
                                        code = Constant.Status.Failed,
                                        description = ex.Message
                };
            }
            return _result;
        }

        public Result<IEnumerable<AccessGroup>> StatusUpdateAccessGroup([FromBody] Payload payload) {
            var _result = new Result<IEnumerable<AccessGroup>>();
            try {
                using (var db = new DatabaseContext()) {
                    if (payload.item_list.Count() > 0) {                        
                        foreach (string id in payload.item_list) {
                            var _AccessGroup = db.AccessGroup.Find(Convert.ToInt32(id));
                            _AccessGroup.status = payload.status;
                            _AccessGroup.updated_date = DateTime.Now;
                            _AccessGroup.updated_by = payload.user_id;
                            db.AccessGroup.AddOrUpdate(_AccessGroup);
                        }
                    }
                    db.SaveChanges();

                    _result = GetAccessGroup(new Payload() { page_index = payload.page_index });
                }
            }
            catch (Exception ex) {
                //TODO: Exception handling
                _result.status = new Status() {
                    code = Constant.Status.Failed,
                    description = ex.Message
                };
            }
            return _result;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}