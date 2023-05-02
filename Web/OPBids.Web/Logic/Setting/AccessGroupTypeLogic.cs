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
using OPBids.Entities.Base;
using System.Web.UI.WebControls;
using Microsoft.Owin;

namespace OPBids.Web.Logic.Setting
{
    public class AccessGroupTypeLogic : SettingLogicBase
    {
        IOwinContext _context;
        public AccessGroupTypeLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId) {
            return Constant.Setting.HeaderView.AccessGroupType;
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
            return DownloadExcel<AccessGroupTypeVM>("AccessGroupType", Constant.ServiceEnpoint.Settings.GetAccessGroupType);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<AccessGroupTypeVM>("AccessGroupType", Constant.ServiceEnpoint.Settings.GetAccessGroupType);
        }
        public override string HTMLTable()
        {
            return HTMLTable<AccessGroupTypeVM>(Constant.ServiceEnpoint.Settings.GetAccessGroupType);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetAccessGroupType);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public Result<IEnumerable<T>> GetAccessGroupMenu<T>(SettingVM setting)
        {
            return ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetAccessGroupMenu);
        }
        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.AccessGroupType,
                SearchData<AccessGroupTypeVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }

        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            setting.accessGroupType.ToList().ForEach(a => {
                if (a.id == 0)
                {
                    a.created_by = user_id;
                    a.updated_by = user_id;
                }
                else
                {
                    a.updated_by = user_id;
                }
            });
            Result<IEnumerable<AccessGroupTypeVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessGroupTypeVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.SaveAccessGroupType, setting.accessGroupType);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessGroupType,
                _list.value == null ? new List<AccessGroupTypeVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<AccessGroupTypeVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessGroupTypeVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateAccessType, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessTypes,
                _list.value == null ? new List<AccessGroupTypeVM>() : _list.value);
        }
    }
}