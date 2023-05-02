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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace OPBids.Web.Logic.Setting
{
    public class AccessGroupLogic : SettingLogicBase
    {
        IOwinContext _context;
        public AccessGroupLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId) {
            return Constant.Setting.HeaderView.AccessGroup;
        }

        public override ActionResult ResultView(SettingVM setting) {
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
            return DownloadExcel<AccessGroupVM>("AccessGroup", Constant.ServiceEnpoint.Settings.GetAccessGroup);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<AccessGroupVM>("AccessGroup", Constant.ServiceEnpoint.Settings.GetAccessGroup);
        }
        public override string HTMLTable()
        {
            return HTMLTable<AccessGroupVM>(Constant.ServiceEnpoint.Settings.GetAccessGroup);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetAccessGroup);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.AccessGroup,
                SearchData<AccessGroupVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _accessGroup = setting.accessGroup;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateAccessGroup;
            _accessGroup.updated_by = user_id;
            if (_accessGroup.id == 0)
            {
                // TODO: Get current user
                _accessGroup.created_by = user_id;
            }
            else
            {
                curUrl = Constant.ServiceEnpoint.Settings.UpdateAccessGroup;
            }
            Result<IEnumerable<AccessGroupVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessGroupVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _accessGroup);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessGroup,
                _list.value == null ? new List<AccessGroupVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<AccessGroupVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessGroupVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateAccessGroup, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessGroup,
                _list.value == null ? new List<AccessGroupVM>() : _list.value);
        }
    }
}