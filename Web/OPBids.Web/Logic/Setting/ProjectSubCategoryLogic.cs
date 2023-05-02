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
namespace OPBids.Web.Logic.Setting
{
    public class ProjectSubCategoryLogic:SettingLogicBase
    {
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.ProjectSubCategory;
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
            return DownloadExcel<ProjectSubCategoryVM>("ProjectSubCategory", Constant.ServiceEnpoint.Settings.GetProjectSubCategory);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<ProjectSubCategoryVM>("ProjectSubCategory", Constant.ServiceEnpoint.Settings.GetProjectSubCategory);
        }
        public override string HTMLTable()
        {
            return HTMLTable<ProjectSubCategoryVM>(Constant.ServiceEnpoint.Settings.GetProjectSubCategory);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetProjectSubCategory);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.ProjectSubCategory,
                SearchData<ProjectSubCategoryVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var _projectsubcategory = setting.projectsubcategory;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateProjectSubCategory;
            if (_projectsubcategory.id == 0)
            {
                _projectsubcategory.created_by = 1;
                _projectsubcategory.updated_by = 1;
            }
            else
            {
                _projectsubcategory.updated_by = 1;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateProjectSubCategory;
            }
            Result<IEnumerable<ProjectSubCategoryVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectSubCategoryVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _projectsubcategory);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectSubCategory,
                _list.value == null ? new List<ProjectSubCategoryVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<ProjectSubCategoryVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectSubCategoryVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateProjectSubCategory, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectSubCategory,
                _list.value == null ? new List<ProjectSubCategoryVM>() : _list.value);
        }

    }
}