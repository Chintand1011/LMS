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
    public class ProjectSubStatusLogic: SettingLogicBase
    {
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.ProjectSubStatus;
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
            return DownloadExcel<ProjectSubStatusVM>("ProjectSubStatus", Constant.ServiceEnpoint.Settings.GetProjectSubStatus);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<ProjectSubStatusVM>("ProjectSubStatus", Constant.ServiceEnpoint.Settings.GetProjectSubStatus);
        }
        public override string HTMLTable()
        {
            return HTMLTable<ProjectSubStatusVM>(Constant.ServiceEnpoint.Settings.GetProjectSubStatus);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetProjectSubStatus);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.ProjectSubStatus,
                SearchData<ProjectSubStatusVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var _projectsubstatus = setting.projectsubstatus;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateProjectSubStatus;
            if (_projectsubstatus.id == 0)
            {
                _projectsubstatus.created_by = 1;
                _projectsubstatus.updated_by = 1;
            }
            else
            {
                _projectsubstatus.updated_by = 1;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateProjectSubStatus;
            }
            Result<IEnumerable<ProjectSubStatusVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectSubStatusVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _projectsubstatus);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectSubStatus,
                _list.value == null ? new List<ProjectSubStatusVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<ProjectSubStatusVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ProjectSubStatusVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateProjectSubStatus, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProjectSubStatus,
                _list.value == null ? new List<ProjectSubStatusVM>() : _list.value);
        }

    }
}