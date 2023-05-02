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
    public class ProjectAreasCityLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();
        
        public Result<IEnumerable<ProjectAreasCity>> GetProjectAreasCity(Payload payload)
        {            
            var _result = new Result<IEnumerable<ProjectAreasCity>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectAreasCity
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from city in db.ProjectAreasCity
                                 where (city.city_name.ToLower().Contains(payload.search_key.ToLower())) &&
                                 city.status != Constant.RecordStatus.Deleted
                                 select city).ToList();
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

        public Result<IEnumerable<ProjectAreasCity>> CreateProjectAreasCity(ProjectAreasCity projectareascity)
        {
            var _result = new Result<IEnumerable<ProjectAreasCity>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectareascity.status = Constant.RecordStatus.Active;
                    projectareascity.created_date = DateTime.Now;
                    projectareascity.updated_date = DateTime.Now;

                    db.ProjectAreasCity.Add(projectareascity);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProjectAreasCity
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

        public Result<IEnumerable<ProjectAreasCity>> UpdateProjectAreasCity([FromBody] ProjectAreasCity projectareascity)
        {
            var _result = new Result<IEnumerable<ProjectAreasCity>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectareascity.updated_date = DateTime.Now;

                    db.ProjectAreasCity.AddOrUpdate(projectareascity);
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectAreasCity
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

        public Result<IEnumerable<ProjectAreasCity>> StatusUpdateProjectAreasCity([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasCity>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _city = db.ProjectAreasCity.Find(Convert.ToInt32(id));
                            _city.status = payload.status;
                            _city.updated_date = DateTime.Now;
                            _city.updated_by = payload.user_id;
                            db.ProjectAreasCity.AddOrUpdate(_city);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectAreasCity
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

        public Result<IEnumerable<ProjectAreasCity>> GetAndSaveProjectAreasCity(Payload payload)
        {
            int id = Convert.ToInt32(payload.item_list[0]);
            var city_name = payload.item_list[1];

            var _result = new Result<IEnumerable<ProjectAreasCity>>();
            var projectareascity = new ProjectAreasCity();
            int _cityid = 0;

            if (id == Constant.ProjectAreaGetAndSave.NewID)
            {
                var _dupcity = new Result<IEnumerable<ProjectAreasCity>>
                {
                    value = (from area in db.ProjectAreasCity
                             where (area.city_name.ToLower().Contains(city_name.ToLower())) &&
                             area.status != Constant.RecordStatus.Deleted
                             select area).ToList()
                };
                if (_dupcity.value.Count() == 0)
                {
                    using (var db = new DatabaseContext())
                    {
                        projectareascity.city_name = city_name;
                        projectareascity.status = Constant.RecordStatus.Active;
                        projectareascity.created_by = Convert.ToInt32(payload.created_by);
                        projectareascity.created_date = DateTime.Now;
                        projectareascity.updated_by = Convert.ToInt32(payload.updated_by);
                        projectareascity.updated_date = DateTime.Now;

                        db.ProjectAreasCity.Add(projectareascity);
                        db.SaveChanges();

                        _cityid = projectareascity.id;
                    }
                }
                else
                {
                    _cityid = _dupcity.value.First().id;
                }
            }
            else
            {
                var _dupcity = new Result<IEnumerable<ProjectAreasCity>>
                {
                    value = (from area in db.ProjectAreasCity
                             where (!area.id.Equals(id)) &&
                             (area.city_name.ToLower().Contains(city_name.ToLower())) &&
                             area.status != Constant.RecordStatus.Deleted
                             select area).ToList()
                };

                if (_dupcity.value.Count() == 0)
                {
                    var _exstdist = new Result<IEnumerable<ProjectAreasCity>>
                    {
                        value = (from area in db.ProjectAreasCity
                                 where (area.id.Equals(id)) &&
                                 area.status != Constant.RecordStatus.Deleted
                                 select area).ToList()
                    };

                    using (var db = new DatabaseContext())
                    {
                        projectareascity.city_name = city_name;
                        projectareascity.created_by = _exstdist.value.First().created_by;
                        projectareascity.created_date = _exstdist.value.First().created_date;
                        projectareascity.updated_by = Convert.ToInt32(payload.updated_by);
                        projectareascity.updated_date = DateTime.Now;
                        projectareascity.status = _exstdist.value.First().status;
                        projectareascity.id = id;
                        db.ProjectAreasCity.AddOrUpdate(projectareascity);
                        db.SaveChanges();

                        _cityid = id;
                    }
                }
                else
                {
                    _cityid = _dupcity.value.First().id;
                }
            }

            _result = new Result<IEnumerable<ProjectAreasCity>>
            {
                value = (from area in db.ProjectAreasCity
                         where (area.id.Equals(_cityid)) &&
                         area.status != Constant.RecordStatus.Deleted
                         select area).ToList()
            };

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