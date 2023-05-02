using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Web.Mvc;
using OPBids.Entities.Common;
using OPBids.Entities.View.Setting;
using OPBids.Common;
using OPBids.Web.Helper;
using System.Web.UI.WebControls;
using Microsoft.Owin;
namespace OPBids.Web.Logic.Setting
{
	public class ProjectAreasLogic:SettingLogicBase
	{
        IOwinContext _context;
        public ProjectAreasLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
		{
			return Constant.Setting.HeaderView.ProjectAreas;
		}

		public override ActionResult ResultView(SettingVM setting)
		{
			switch (setting.txn)
			{
                case Constant.TransactionType.Search:
                    return Search(setting);
                case Constant.TransactionType.Save:
                    return Save(setting);
                case Constant.TransactionType.StatusUpdate:
                    return StatusUpdate(setting);
                default:
                    //default temp val until permission is set
                    return Search(setting);
                    //return null;
            }
        }
		public override GridView DownloadExcel()
		{
			return DownloadExcel<ProjectAreasVM>("ProjectAreas", Constant.ServiceEnpoint.Settings.GetProjectAreas);
		}
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<ProjectAreasVM>("ProjectAreas", Constant.ServiceEnpoint.Settings.GetProjectAreas);
        }
        public override string HTMLTable()
        {
            return HTMLTable<ProjectAreasVM>(Constant.ServiceEnpoint.Settings.GetProjectAreas);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
		{
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetProjectAreas);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
		}

		public override ActionResult Search(SettingVM setting)
		{
			return PartialView(Constant.Setting.ResultView.ProjectAreas,
				SearchData<ProjectAreasVM>(setting));
		}
		public override IEnumerable<T> SearchSub<T>(SettingVM setting)
		{
			return new List<T>();
		}
		public override ActionResult Save(SettingVM setting)
		{
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();            
            var _projectareas = setting.projectareas;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateProjectAreas;
            _projectareas.updated_by = user_id;
            if (_projectareas.id == 0)
            {
                _projectareas.created_by = user_id;
            }
            else
            {                
                curUrl = Constant.ServiceEnpoint.Settings.UpdateProjectAreas;
            }
            Result<IEnumerable<ProjectAreasVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectAreasVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _projectareas);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectAreas,
                _list.value == null ? new List<ProjectAreasVM>() : _list.value);
        }

		public override ActionResult StatusUpdate(SettingVM setting)
		{
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            setting.updated_by = user_id;
            Result<IEnumerable<ProjectAreasVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectAreasVM>>>();            
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateProjectAreas, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectAreas,
                _list.value == null ? new List<ProjectAreasVM>() : _list.value);
        }

        public ActionResult GetAndSaveProjectAreas(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            var curUrl = Constant.ServiceEnpoint.Settings.GetAndSaveProjectAreas;
            setting.created_by = user_id;
            setting.updated_by = user_id;
            Result<IEnumerable<ProjectAreasVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectAreasVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, setting);
            return new JsonResult { Data = _list.value };
        }
    }
}