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
    public class ProjectProponentLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProjectProponent>> GetProjectProponent(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectProponent>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectProponents
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from types in db.ProjectProponents
                                 where (types.proponent_name.ToLower().Contains(payload.search_key.ToLower()) || types.proponent_designation.ToLower().Contains(payload.search_key.ToLower()))
                                 && types.status != Constant.RecordStatus.Deleted
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

        public Result<IEnumerable<ProjectProponent>> CreateProjectProponent(ProjectProponent projectproponent)
        {
            var _result = new Result<IEnumerable<ProjectProponent>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectproponent.status = Constant.RecordStatus.Active;
                    projectproponent.created_date = DateTime.Now;
                    projectproponent.updated_date = DateTime.Now;

                    db.ProjectProponents.Add(projectproponent);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProjectProponents
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

        public Result<IEnumerable<ProjectProponent>> UpdateProjectProponent([FromBody] ProjectProponent projectproponent)
        {
            var _result = new Result<IEnumerable<ProjectProponent>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectproponent.updated_date = DateTime.Now;

                    db.ProjectProponents.AddOrUpdate(projectproponent);
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectProponents
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

        public Result<IEnumerable<ProjectProponent>> StatusUpdateProjectProponent([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectProponent>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _projectproponent = db.ProjectProponents.Find(Convert.ToInt32(id));
                            _projectproponent.status = payload.status;
                            _projectproponent.updated_date = DateTime.Now;
                            _projectproponent.updated_by = payload.user_id;
                            db.ProjectProponents.AddOrUpdate(_projectproponent);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectProponents
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