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
    public class ProjectCategoryLogic:SettingLogicBase
    {

        IOwinContext _context;
        public ProjectCategoryLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }

        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.ProjectCategory;
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
            return DownloadExcel<ProjectCategoryVM>("ProjectCategory", Constant.ServiceEnpoint.Settings.GetProjectCategory);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<ProjectCategoryVM>("ProjectCategory", Constant.ServiceEnpoint.Settings.GetProjectCategory);
        }
        public override string HTMLTable()
        {
            return HTMLTable<ProjectCategoryVM>(Constant.ServiceEnpoint.Settings.GetProjectCategory);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetProjectCategory);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.ProjectCategory,
                SearchData<ProjectCategoryVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var _projectcategory = setting.projectcategory;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateProjectCategory;
            if (_projectcategory.id == 0)
            {
                _projectcategory.created_by = 1;
                _projectcategory.updated_by = 1;
            }
            else
            {
                _projectcategory.updated_by = 1;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateProjectCategory;
            }
            Result<IEnumerable<ProjectCategoryVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectCategoryVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _projectcategory);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectCategory,
                _list.value == null ? new List<ProjectCategoryVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<ProjectCategoryVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectCategoryVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateProjectCategory, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectCategory,
                _list.value == null ? new List<ProjectCategoryVM>() : _list.value);
        }
    }
}