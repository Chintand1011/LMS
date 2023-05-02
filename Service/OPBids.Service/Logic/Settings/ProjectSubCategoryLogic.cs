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
	public class ProjectSubCategoryLogic:ApiController
	{
		private DatabaseContext db = new DatabaseContext();

		public Result<IEnumerable<ProjectSubCategory>> GetProjectSubCategory(Payload payload)
		{
			var _result = new Result<IEnumerable<ProjectSubCategory>>();
			if (payload.search_key == null || payload.search_key == string.Empty)
			{
				_result.value = (from types in db.ProjectSubCategory
								 where types.status != Constant.RecordStatus.Deleted
								 select types).ToList();
			}
			else
			{
				_result.value = (from types in db.ProjectSubCategory
								 where (types.proj_subcat.ToLower().Contains(payload.search_key.ToLower()) || types.proj_subcatdesc.ToLower().Contains(payload.search_key.ToLower())) &&
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

		public Result<IEnumerable<ProjectSubCategory>> CreateProjectSubCategory(ProjectSubCategory projectsubcategory)
		{
			var _result = new Result<IEnumerable<ProjectSubCategory>>();
			try
			{
				using (var db = new DatabaseContext())
				{
                    projectsubcategory.status = Constant.RecordStatus.Active;
                    projectsubcategory.created_date = DateTime.Now;
                    projectsubcategory.updated_date = DateTime.Now;

					db.ProjectSubCategory.Add(projectsubcategory);
					db.SaveChanges();

					//Select all records
					_result.value = (from types in db.ProjectSubCategory
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

		public Result<IEnumerable<ProjectSubCategory>> UpdateProjectSubCategory([FromBody] ProjectSubCategory projectsubcategory)
		{
			var _result = new Result<IEnumerable<ProjectSubCategory>>();
			try
			{
				using (var db = new DatabaseContext())
				{
                    projectsubcategory.updated_date = DateTime.Now;

					db.ProjectSubCategory.AddOrUpdate(projectsubcategory);
					db.SaveChanges();

					_result.value = (from types in db.ProjectSubCategory
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

		public Result<IEnumerable<ProjectSubCategory>> StatusUpdateProjectSubCategory([FromBody] Payload payload)
		{
			var _result = new Result<IEnumerable<ProjectSubCategory>>();
			try
			{
				using (var db = new DatabaseContext())
				{
					if (payload.item_list.Count() > 0)
					{
						foreach (string id in payload.item_list)
						{
							var _ProjectSubCategory = db.ProjectSubCategory.Find(Convert.ToInt32(id));
							_ProjectSubCategory.status = payload.status;
							_ProjectSubCategory.updated_date = DateTime.Now;
							_ProjectSubCategory.updated_by = payload.user_id;
							db.ProjectSubCategory.AddOrUpdate(_ProjectSubCategory);
						}
					}
					db.SaveChanges();

					_result.value = (from types in db.ProjectSubCategory
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
