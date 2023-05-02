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
    public class SourceFundsLogic: SettingLogicBase
    {
        IOwinContext _context;
        public SourceFundsLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.SourceFunds;
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
            return DownloadExcel<SourceFundsVM>("SourceFunds", Constant.ServiceEnpoint.Settings.GetSourceFunds);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<SourceFundsVM>("SourceFunds", Constant.ServiceEnpoint.Settings.GetSourceFunds);
        }
        public override string HTMLTable()
        {
            return HTMLTable<SourceFundsVM>(Constant.ServiceEnpoint.Settings.GetSourceFunds);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetSourceFunds);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.SourceFunds,
                SearchData<SourceFundsVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();

            var _sourcefunds = setting.sourcefunds;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateSourceFunds;
            if (_sourcefunds.id == 0)
            {
                _sourcefunds.created_by = user_id;
                _sourcefunds.updated_by = user_id;
            }
            else
            {
                _sourcefunds.updated_by = user_id;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateSourceFunds;
            }
            Result<IEnumerable<SourceFundsVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<SourceFundsVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _sourcefunds);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.SourceFunds,
                _list.value == null ? new List<SourceFundsVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<SourceFundsVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<SourceFundsVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateSourceFunds, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.SourceFunds,
                _list.value == null ? new List<SourceFundsVM>() : _list.value);
        }
    }
}
