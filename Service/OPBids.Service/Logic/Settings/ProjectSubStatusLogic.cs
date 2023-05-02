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
    public class ProjectSubStatusLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProjectSubStatus>> GetProjectSubStatus(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectSubStatus>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectSubStatus
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ProjectSubStatus
                                 where (types.proj_substatcode.ToLower().Contains(payload.search_key.ToLower()) ||
                                 types.proj_substatdescription.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProjectSubStatus>> CreateProjectSubStatus(ProjectSubStatus projectsubstatus)
        {
            var _result = new Result<IEnumerable<ProjectSubStatus>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectsubstatus.status = Constant.RecordStatus.Active;
                    projectsubstatus.created_date = DateTime.Now;
                    projectsubstatus.updated_date = DateTime.Now;

                    db.ProjectSubStatus.Add(projectsubstatus);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProjectSubStatus
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
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

        public Result<IEnumerable<ProjectSubStatus>> UpdateProjectSubStatus([FromBody] ProjectSubStatus projectsubstatus)
        {
            var _result = new Result<IEnumerable<ProjectSubStatus>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectsubstatus.updated_date = DateTime.Now;

                    db.ProjectSubStatus.AddOrUpdate(projectsubstatus);
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectSubStatus
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
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

        public Result<IEnumerable<ProjectSubStatus>> StatusUpdateProjectSubStatus([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectSubStatus>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _ProjectSubStatus = db.ProjectSubStatus.Find(Convert.ToInt32(id));
                            _ProjectSubStatus.status = payload.status;
                            _ProjectSubStatus.updated_date = DateTime.Now;
                            _ProjectSubStatus.updated_by = payload.user_id;
                            db.ProjectSubStatus.AddOrUpdate(_ProjectSubStatus);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectSubStatus
                                     where types.status != Constant.RecordStatus.Deleted
                                     select types).ToList();
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