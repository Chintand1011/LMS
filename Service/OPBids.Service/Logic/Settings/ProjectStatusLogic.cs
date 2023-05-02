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
    public class ProjectStatusLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProjectStatus>> GetProjectStatus(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectStatus>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectStatus
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ProjectStatus
                                 where (types.proj_statcode.ToLower().Contains(payload.search_key.ToLower()) || types.proj_statdescription.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProjectStatus>> CreateProjectStatus(ProjectStatus projectstatus)
        {
            var _result = new Result<IEnumerable<ProjectStatus>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectstatus.status = Constant.RecordStatus.Active;
                    projectstatus.created_date = DateTime.Now;
                    projectstatus.updated_date = DateTime.Now;

                    db.ProjectStatus.Add(projectstatus);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProjectStatus
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

        public Result<IEnumerable<ProjectStatus>> UpdateProjectStatus([FromBody] ProjectStatus projectstatus)
        {
            var _result = new Result<IEnumerable<ProjectStatus>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectstatus.updated_date = DateTime.Now;

                    db.ProjectStatus.AddOrUpdate(projectstatus);
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectStatus
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

        public Result<IEnumerable<ProjectStatus>> StatusUpdateProjectStatus([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectStatus>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _projectstatus = db.ProjectStatus.Find(Convert.ToInt32(id));
                            _projectstatus.status = payload.status;
                            _projectstatus.updated_date = DateTime.Now;
                            _projectstatus.updated_by = payload.user_id;
                            db.ProjectStatus.AddOrUpdate(_projectstatus);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectStatus
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