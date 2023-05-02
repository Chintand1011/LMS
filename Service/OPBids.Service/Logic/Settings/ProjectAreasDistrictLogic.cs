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
    public class ProjectAreasDistrictLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProjectAreasDistrict>> GetProjectAreasDistrict(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasDistrict>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectAreasDistrict
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from city in db.ProjectAreasDistrict
                                 where (city.district_name.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProjectAreasDistrict>> CreateProjectAreasDistrict(ProjectAreasDistrict projectareasdistrict)
        {
            var _result = new Result<IEnumerable<ProjectAreasDistrict>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectareasdistrict.status = Constant.RecordStatus.Active;
                    projectareasdistrict.created_date = DateTime.Now;
                    projectareasdistrict.updated_date = DateTime.Now;

                    db.ProjectAreasDistrict.Add(projectareasdistrict);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProjectAreasDistrict
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

        public Result<IEnumerable<ProjectAreasDistrict>> UpdateProjectAreasDistrict([FromBody] ProjectAreasDistrict projectareasdistrict)
        {
            var _result = new Result<IEnumerable<ProjectAreasDistrict>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectareasdistrict.updated_date = DateTime.Now;

                    db.ProjectAreasDistrict.AddOrUpdate(projectareasdistrict);
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectAreasDistrict
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

        public Result<IEnumerable<ProjectAreasDistrict>> StatusUpdateProjectAreasDistrict([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasDistrict>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _city = db.ProjectAreasDistrict.Find(Convert.ToInt32(id));
                            _city.status = payload.status;
                            _city.updated_date = DateTime.Now;
                            _city.updated_by = payload.user_id;
                            db.ProjectAreasDistrict.AddOrUpdate(_city);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectAreasDistrict
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

        public Result<IEnumerable<ProjectAreasDistrict>> GetAndSaveProjectAreasDistrict(Payload payload)
        {
            int id = Convert.ToInt32(payload.item_list[0]);
            int newcity_id = Convert.ToInt32(payload.item_list[1]);
            int oldcity_id = Convert.ToInt32(payload.item_list[2]);
            var district_name = payload.item_list[3];

            var _result = new Result<IEnumerable<ProjectAreasDistrict>>();
            var projectareasdist = new ProjectAreasDistrict();
            int _distid = id;

            if (id == Constant.ProjectAreaGetAndSave.NewID)
            {
                var _newdupdist = new Result<IEnumerable<ProjectAreasDistrict>>
                {
                    value = (from area in db.ProjectAreasDistrict
                             where (area.city_id.Equals(newcity_id)) &&
                             (area.district_name.ToLower().Contains(district_name.ToLower())) &&
                             area.status != Constant.RecordStatus.Deleted
                             select area).ToList()
                };
                if (_newdupdist.value.Count() == 0)
                {
                    _distid = AddDistrict(newcity_id, district_name, Convert.ToInt32(payload.created_by));
                } else
                {
                    _distid = _newdupdist.value.First().id;
                }
            } else
            {
                if (newcity_id != oldcity_id)
                {
                    _distid = AddDistrict(newcity_id, district_name, Convert.ToInt32(payload.created_by));
                }
                else
                {
                    UpdateDistrict(id, newcity_id, district_name, Convert.ToInt32(payload.updated_by));
                }
            }            
           
            _result = new Result<IEnumerable<ProjectAreasDistrict>>
            {
                value = (from area in db.ProjectAreasDistrict
                         where (area.id.Equals(_distid)) &&
                         area.status != Constant.RecordStatus.Deleted
                         select area).ToList()
            };

            return _result;
        }

        private int AddDistrict(int city_id, string district_name, int created_by)
        {
            int newid = 0;
            var projectareasdist = new ProjectAreasDistrict();
            using (var db = new DatabaseContext())
            {
                projectareasdist.city_id = city_id;
                projectareasdist.district_name = district_name;
                projectareasdist.status = Constant.RecordStatus.Active;
                projectareasdist.created_by = created_by;
                projectareasdist.created_date = DateTime.Now;
                projectareasdist.updated_by = created_by;
                projectareasdist.updated_date = DateTime.Now;

                db.ProjectAreasDistrict.Add(projectareasdist);
                db.SaveChanges();

                newid = projectareasdist.id;
            }

            return newid;
        }

        private void UpdateDistrict(int id, int city_id, string district_name, int updated_by)
        {
            var projectareasdist = new ProjectAreasDistrict();
            var _exstdist = new Result<IEnumerable<ProjectAreasDistrict>>
            {
                value = (from area in db.ProjectAreasDistrict
                         where (area.id.Equals(id)) &&
                         area.status != Constant.RecordStatus.Deleted
                         select area).ToList()
            };

            using (var db = new DatabaseContext())
            {
                projectareasdist.city_id = city_id;
                projectareasdist.district_name = district_name;
                projectareasdist.created_by = _exstdist.value.First().created_by;
                projectareasdist.created_date = _exstdist.value.First().created_date;
                projectareasdist.updated_by = updated_by;
                projectareasdist.updated_date = DateTime.Now;
                projectareasdist.status = _exstdist.value.First().status;
                projectareasdist.id = id;
                db.ProjectAreasDistrict.AddOrUpdate(projectareasdist);
                db.SaveChanges();
            }
        }

        public Result<IEnumerable<ProjectAreasDistrict>> GetProjectAreasDistrictByCity(Payload payload)
        {
            int city_id = Convert.ToInt32(payload.item_list[0]);
            var _result = new Result<IEnumerable<ProjectAreasDistrict>>()
            {
                value = (from area in db.ProjectAreasDistrict
                         where (area.city_id.Equals(city_id)) &&
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