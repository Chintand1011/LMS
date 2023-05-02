using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OPBids.Common;
using System.Web.Mvc;
using OPBids.Entities.View.Setting;
using OPBids.Entities.Common;
using OPBids.Web.Helper;
using System.Web.UI.WebControls;
using Microsoft.Owin;

namespace OPBids.Web.Logic.Setting
{
    public class ProjectAreasDistrictLogic : SettingLogicBase
    {
        IOwinContext _context;
        public ProjectAreasDistrictLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
        {
            return null;
        }

        public override ActionResult ResultView(SettingVM setting)
        {
            return null;
        }
        public override GridView DownloadExcel()
        {
            return null;
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return null;
        }
        public override string HTMLTable()
        {
            return null;
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            setting.created_by = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            setting.updated_by = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetProjectAreasDistrict);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return null;
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return null;
        }
        public override ActionResult Save(SettingVM setting)
        {
            return null;
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            return null;
        }

        public ActionResult GetAndSaveProjectAreasDistrict(SettingVM setting)
        {            
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            var curUrl = Constant.ServiceEnpoint.Settings.GetAndSaveProjectAreasDistrict;
            setting.created_by = user_id;
            setting.updated_by = user_id;
            Result<IEnumerable<ProjectAreasDistrictVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectAreasDistrictVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, setting);
            return new JsonResult { Data = _list.value };
        }

        public ActionResult GetProjectAreasDistrictByCity(SettingVM setting)
        {
            var curUrl = Constant.ServiceEnpoint.Settings.GetProjectAreasDistrictByCity;
            Result<IEnumerable<ProjectAreasDistrictVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectAreasDistrictVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, setting);
            return new JsonResult { Data = _list.value };
        }
    }
}