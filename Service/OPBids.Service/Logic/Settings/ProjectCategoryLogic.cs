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
	public class ProjectCategoryLogic:ApiController
	{
		private DatabaseContext db = new DatabaseContext();

		public Result<IEnumerable<ProjectCategory>> GetProjectCategory(Payload payload)
		{
			var _result = new Result<IEnumerable<ProjectCategory>>();
			if (payload.search_key == null || payload.search_key == string.Empty)
			{
				_result.value = (from types in db.ProjectCategory
								 where types.status != Constant.RecordStatus.Deleted
								 select types).ToList();
			}
			else
			{
				_result.value = (from types in db.ProjectCategory
								 where (types.proj_cat.ToLower().Contains(payload.search_key.ToLower()) || types.proj_desc.ToLower().Contains(payload.search_key.ToLower())) &&
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

        public Result<IEnumerable<ProjectCategory>> CreateProjectCategory(ProjectCategory ProjectCategory)
		{
			var _result = new Result<IEnumerable<ProjectCategory>>();
			try
			{
				using (var db = new DatabaseContext())
				{
					ProjectCategory.status = Constant.RecordStatus.Active;
					ProjectCategory.created_date = DateTime.Now;
					ProjectCategory.updated_date = DateTime.Now;

					db.ProjectCategory.Add(ProjectCategory);
					db.SaveChanges();

					//Select all records
					_result.value = (from types in db.ProjectCategory
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

		public Result<IEnumerable<ProjectCategory>> UpdateProjectCategory([FromBody] ProjectCategory ProjectCategory)
		{
			var _result = new Result<IEnumerable<ProjectCategory>>();
			try
			{
				using (var db = new DatabaseContext())
				{
					ProjectCategory.updated_date = DateTime.Now;

					db.ProjectCategory.AddOrUpdate(ProjectCategory);
					db.SaveChanges();

					_result.value = (from types in db.ProjectCategory
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

		public Result<IEnumerable<ProjectCategory>> StatusUpdateProjectCategory([FromBody] Payload payload)
		{
			var _result = new Result<IEnumerable<ProjectCategory>>();
			try
			{
				using (var db = new DatabaseContext())
				{
					if (payload.item_list.Count() > 0)
					{
						foreach (string id in payload.item_list)
						{
							var _ProjectCategory = db.ProjectCategory.Find(Convert.ToInt32(id));
							_ProjectCategory.status = payload.status;
							_ProjectCategory.updated_date = DateTime.Now;
							_ProjectCategory.updated_by = payload.user_id;
							db.ProjectCategory.AddOrUpdate(_ProjectCategory);
						}
					}
					db.SaveChanges();

					_result.value = (from types in db.ProjectCategory
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
