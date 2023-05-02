using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Service.Data;
using OPBids.Service.Models;
using OPBids.Service.Models.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;

namespace OPBids.Service.Logic.Settings
{
    public class ProjectAreasLogic:ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        
        public Result<IEnumerable<ProjectAreasVM>> GetProjectAreas(Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasVM>>();
            if (payload.search_key == null || payload.search_key == string.Empty)
            {                
                _result.value = (from a in db.ProjectAreas
                                 join c in db.ProjectAreasCity on a.city_id equals c.id into ac
                                 from pac in ac.DefaultIfEmpty()
                                 join d in db.ProjectAreasDistrict on a.district_id equals d.id into d
                                 from pad in d.DefaultIfEmpty()
                                 join b in db.ProjectAreasBarangay on a.barangay_id equals b.id into b
                                 from pab in b.DefaultIfEmpty()
                                 where a.status != Constant.RecordStatus.Deleted
                                 orderby pac.city_name, pad.district_name, pab.barangay_name  
                                 select new ProjectAreasVM()
                                 {
                                     id = a.id,
                                     city_id = a.city_id,
                                     city_name = pac.city_name,
                                     district_id = a.district_id,
                                     district_name = pad.district_name ?? "all",
                                     barangay_id = a.barangay_id,
                                     barangay_name = pab.barangay_name ?? "all",
                                     status = a.status,
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(), 
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString()
                                 }).ToList();
            }
            else
            {                
                _result.value = (from a in db.ProjectAreas
                                 join c in db.ProjectAreasCity on a.city_id equals c.id into ac
                                 from pac in ac.DefaultIfEmpty()
                                 join d in db.ProjectAreasDistrict on a.district_id equals d.id into d
                                 from pad in d.DefaultIfEmpty()
                                 join b in db.ProjectAreasBarangay on a.barangay_id equals b.id into b
                                 from pab in b.DefaultIfEmpty()
                                 where (pac.city_name.ToLower().Contains(payload.search_key.ToLower()) || pad.district_name.ToLower().Contains(payload.search_key.ToLower()) || pab.barangay_name.ToLower().Contains(payload.search_key.ToLower())) && 
                                 a.status != Constant.RecordStatus.Deleted
                                 orderby pac.city_name, pad.district_name, pab.barangay_name
                                 select new ProjectAreasVM()
                                 {
                                     id = a.id,
                                     city_id = a.city_id,
                                     city_name = pac.city_name,
                                     district_id = a.district_id,
                                     district_name = pad.district_name ?? "all",
                                     barangay_id = a.barangay_id,
                                     barangay_name = pab.barangay_name ?? "all",
                                     status = a.status,
                                     created_by = a.created_by,
                                     created_date = a.created_date.ToString(),
                                     updated_by = a.updated_by,
                                     updated_date = a.updated_date.ToString()
                                 }).ToList();

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

        public Result<IEnumerable<ProjectAreasVM>> CreateProjectAreas(ProjectAreasVM projectareas)
        {
            var _result = new Result<IEnumerable<ProjectAreasVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    db.ProjectAreas.Add(new ProjectAreas()
                    {
                        updated_date = DateTime.Now,
                        updated_by = projectareas.updated_by,
                        created_by = projectareas.created_by,
                        created_date = DateTime.Now,
                        city_id = projectareas.city_id,
                        district_id = projectareas.district_id,
                        barangay_id = projectareas.barangay_id,
                        id = projectareas.id,
                        status = Constant.RecordStatus.Active,
                    });
                    db.SaveChanges();

                    //Select all records
                    _result = GetProjectAreas(new Payload() { page_index = projectareas.page_index });
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

        public Result<IEnumerable<ProjectAreasVM>> UpdateProjectAreas([FromBody] ProjectAreasVM projectareas)
        {
            var _result = new Result<IEnumerable<ProjectAreasVM>>();
            try
            {
                var _exstarea = new Result<IEnumerable<ProjectAreas>>
                {
                    value = (from area in db.ProjectAreas
                             where (area.id.Equals(projectareas.id)) &&
                             area.status != Constant.RecordStatus.Deleted
                             select area).ToList()
                };

                using (var db = new DatabaseContext())
                {
                    db.ProjectAreas.AddOrUpdate(new ProjectAreas()
                    {
                        created_date = _exstarea.value.First().created_date,
                        created_by = _exstarea.value.First().created_by,
                        updated_date = DateTime.Now,
                        updated_by = projectareas.updated_by,
                        city_id = projectareas.city_id,
                        district_id = projectareas.district_id,
                        barangay_id = projectareas.barangay_id,
                        id = projectareas.id,
                        status = projectareas.status
                    });
                    db.SaveChanges();
                    _result = GetProjectAreas(new Payload() { page_index = projectareas.page_index });
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

        public Result<IEnumerable<ProjectAreasVM>> StatusUpdateProjectAreas([FromBody] Payload payload)
        {
            var _result = new Result<IEnumerable<ProjectAreasVM>>();
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (payload.item_list.Count() > 0)
                    {
                        foreach (string id in payload.item_list)
                        {
                            var _projectareas = db.ProjectAreas.Find(Convert.ToInt32(id));
                            var created_date = _projectareas.created_date;
                            var created_by = _projectareas.created_by;

                            _projectareas.status = payload.status;
                            _projectareas.updated_date = DateTime.Now;
                            _projectareas.updated_by = payload.updated_by;
                            _projectareas.created_date = created_date;
                            _projectareas.created_by = created_by;
                            db.ProjectAreas.AddOrUpdate(_projectareas);
                        }
                    }
                    db.SaveChanges();
                    _result = GetProjectAreas(new Payload() { page_index = payload.page_index });
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
        public Result<IEnumerable<ProjectAreasVM>> GetAndSaveProjectAreas(Payload payload)
        {
            int id = Convert.ToInt32(payload.item_list[0]);
            int city_id = Convert.ToInt32(payload.item_list[1]);
            int oldcity_id = Convert.ToInt32(payload.item_list[2]);
            int dist_id = Convert.ToInt32(payload.item_list[3]);
            int olddist_id = Convert.ToInt32(payload.item_list[4]);
            int brgy_id = Convert.ToInt32(payload.item_list[5]);

            var _result = new Result<IEnumerable<ProjectAreasVM>>();

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