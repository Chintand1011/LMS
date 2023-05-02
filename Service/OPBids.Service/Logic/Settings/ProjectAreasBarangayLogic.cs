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
    public class ProjectAreasBarangayLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<IEnumerable<ProjectAreasBarangay>> GetProjectAreasBarangay(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasBarangay>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {
                _result.value = (from types in db.ProjectAreasBarangay
                                 where types.status != Constant.RecordStatus.Deleted
                                 select types).ToList();
            }
            else
            {
                _result.value = (from city in db.ProjectAreasBarangay
                                 where (city.barangay_name.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProjectAreasBarangay>> CreateProjectAreasBarangay(ProjectAreasBarangay projectareasbarangay)
        {
            var _result = new Result<IEnumerable<ProjectAreasBarangay>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectareasbarangay.status = Constant.RecordStatus.Active;
                    projectareasbarangay.created_date = DateTime.Now;
                    projectareasbarangay.updated_date = DateTime.Now;

                    db.ProjectAreasBarangay.Add(projectareasbarangay);
                    db.SaveChanges();

                    //Select all records
                    _result.value = (from types in db.ProjectAreasBarangay
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

        public Result<IEnumerable<ProjectAreasBarangay>> UpdateProjectAreasBarangay([FromBody] ProjectAreasBarangay projectareasbarangay)
        {
            var _result = new Result<IEnumerable<ProjectAreasBarangay>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    projectareasbarangay.updated_date = DateTime.Now;

                    db.ProjectAreasBarangay.AddOrUpdate(projectareasbarangay);
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectAreasBarangay
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

        public Result<IEnumerable<ProjectAreasBarangay>> StatusUpdateProjectAreasBarangay([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasBarangay>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _city = db.ProjectAreasBarangay.Find(Convert.ToInt32(id));
                            _city.status = payload.status;
                            _city.updated_date = DateTime.Now;
                            _city.updated_by = payload.user_id;
                            db.ProjectAreasBarangay.AddOrUpdate(_city);
                        }
                    }
                    db.SaveChanges();

                    _result.value = (from types in db.ProjectAreasBarangay
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

        public Result<IEnumerable<ProjectAreasBarangay>> GetAndSaveProjectAreasBarangay(Payload payload)
        {
            int id = Convert.ToInt32(payload.item_list[0]);
            int newcity_id = Convert.ToInt32(payload.item_list[1]);
            int oldcity_id = Convert.ToInt32(payload.item_list[2]);
            int newdist_id = Convert.ToInt32(payload.item_list[3]);
            int olddist_id = Convert.ToInt32(payload.item_list[4]);
            var barangay_name = payload.item_list[5];

            var _result = new Result<IEnumerable<ProjectAreasBarangay>>();
            var projectareasbrgy = new ProjectAreasBarangay();
            int _brgyid = id;

            if (id == Constant.ProjectAreaGetAndSave.NewID)
            {
                var _newdupbrgy = new Result<IEnumerable<ProjectAreasBarangay>>
                {
                    value = (from area in db.ProjectAreasBarangay
                             where (area.city_id.Equals(newcity_id)) &&
                             (area.district_id.Equals(newdist_id)) &&
                             (area.barangay_name.ToLower().Contains(barangay_name.ToLower())) &&
                             area.status != Constant.RecordStatus.Deleted
                             select area).ToList()
                };
                if (_newdupbrgy.value.Count() == 0)
                {
                    _brgyid = AddBarangay(newcity_id, newdist_id, barangay_name, Convert.ToInt32(payload.created_by));
                }
                else
                {
                    _brgyid = _newdupbrgy.value.First().id;
                }
            }
            else
            {
                if (newcity_id != oldcity_id || newdist_id != olddist_id)
                {
                    _brgyid = AddBarangay(newcity_id, newdist_id, barangay_name, Convert.ToInt32(payload.created_by));
                }
                else
                {
                    UpdateBarangay(id, newcity_id, newdist_id, barangay_name, Convert.ToInt32(payload.updated_by));
                }
            }

            _result = new Result<IEnumerable<ProjectAreasBarangay>>
            {
                value = (from area in db.ProjectAreasBarangay
                         where (area.id.Equals(_brgyid)) &&
                         area.status != Constant.RecordStatus.Deleted
                         select area).ToList()
            };

            return _result;
        }

        private int AddBarangay(int city_id, int district_id, string barangay_name, int created_by)
        {
            int newid = 0;
            var projectareasbrgy = new ProjectAreasBarangay();
            using (var db = new DatabaseContext())
            {
                projectareasbrgy.city_id = city_id;
                projectareasbrgy.district_id = district_id;
                projectareasbrgy.barangay_name = barangay_name;
                projectareasbrgy.status = Constant.RecordStatus.Active;
                projectareasbrgy.created_by = created_by;
                projectareasbrgy.created_date = DateTime.Now;
                projectareasbrgy.updated_by = created_by;
                projectareasbrgy.updated_date = DateTime.Now;

                db.ProjectAreasBarangay.Add(projectareasbrgy);
                db.SaveChanges();

                newid = projectareasbrgy.id;
            }

            return newid;
        }

        private void UpdateBarangay(int id, int city_id, int district_id, string barangay_name, int updated_by)
        {
            var projectareasbrgy = new ProjectAreasBarangay();
            var _exstdist = new Result<IEnumerable<ProjectAreasBarangay>>
            {
                value = (from area in db.ProjectAreasBarangay
                         where (area.id.Equals(id)) &&
                         area.status != Constant.RecordStatus.Deleted
                         select area).ToList()
            };

            using (var db = new DatabaseContext())
            {
                projectareasbrgy.city_id = city_id;
                projectareasbrgy.district_id = district_id;
                projectareasbrgy.barangay_name = barangay_name;
                projectareasbrgy.created_by = _exstdist.value.First().created_by;
                projectareasbrgy.created_date = _exstdist.value.First().created_date;
                projectareasbrgy.updated_by = updated_by;
                projectareasbrgy.updated_date = DateTime.Now;
                projectareasbrgy.status = _exstdist.value.First().status;
                projectareasbrgy.id = id;
                db.ProjectAreasBarangay.AddOrUpdate(projectareasbrgy);
                db.SaveChanges();
            }
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