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
using System.Threading.Tasks;

namespace OPBids.Web.Logic.Setting
{
    public class AccessUsersLogic : SettingLogicBase
    {
        IOwinContext _context;
        public AccessUsersLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.AccessUsers;
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
            return DownloadExcel<AccessUsersVM>("AccessUser", Constant.ServiceEnpoint.Settings.GetAccessUser);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<AccessUsersVM>("AccessUser", Constant.ServiceEnpoint.Settings.GetAccessUser);
        }
        public override string HTMLTable()
        {
            return HTMLTable<AccessUsersVM>(Constant.ServiceEnpoint.Settings.GetAccessUser);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetAccessUser);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            var rslts = SearchData<AccessUsersVM>(setting);
            return PartialView(Constant.Setting.ResultView.AccessUsers, rslts);
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _accessUsers = setting.accessUsers;

            // Encrypt password
            AesHelper aes = new AesHelper(ConfigManager.AesKey, ConfigManager.AesIV);
            if (_accessUsers.password.ToSafeString() != "")
            {
                _accessUsers.password = aes.Encrypt(_accessUsers.password);
            }

            var curUrl = Constant.ServiceEnpoint.Settings.CreateAccessUser;
            if (_accessUsers.id == 0)
            {
                // TODO: Get current user
                _accessUsers.created_by = user_id;
                _accessUsers.updated_by = user_id;
            }
            else
            {
                _accessUsers.updated_by = user_id;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateAccessUser;
            }
            Result<IEnumerable<AccessUsersVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessUsersVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _accessUsers);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessUsers, _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<AccessUsersVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessUsersVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateAccessUser, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessUsers, _list.value);
        }

        public  ActionResult ResetAccessUserPassword(SettingVM setting) {
            Result<bool> _status;
            var apiManager = new ApiManager<Result<bool>>();
            _status = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.ResetAccessUserPassword, setting.accessUsers);
            return new JsonResult { Data = _status };
        }

    }
}