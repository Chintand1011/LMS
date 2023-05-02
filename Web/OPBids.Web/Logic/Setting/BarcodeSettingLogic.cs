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
    public class BarcodeSettingLogic : SettingLogicBase
    {
        IOwinContext _context;
        public BarcodeSettingLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId) {
            return Constant.Setting.HeaderView.BarcodeSetting;
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
            return DownloadExcel<BarcodeSettingVM>("BarcodeSetting", Constant.ServiceEnpoint.Settings.GetBarcodeSetting);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<BarcodeSettingVM>("BarcodeSetting", Constant.ServiceEnpoint.Settings.GetBarcodeSetting);
        }
        public override string HTMLTable()
        {
            return HTMLTable<BarcodeSettingVM>(Constant.ServiceEnpoint.Settings.GetBarcodeSetting);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetBarcodeSetting);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.BarcodeSetting,
                SearchData<BarcodeSettingVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }

        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            var _BarcodeSettings = setting.barcodeSetting;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateBarcodeSetting;
            _BarcodeSettings.updated_by = user_id;
            if (_BarcodeSettings.id == 0)
            {
                _BarcodeSettings.created_by = user_id;
            }
            else
            {
                curUrl = Constant.ServiceEnpoint.Settings.UpdateBarcodeSetting;
            }
            Result<IEnumerable<BarcodeSettingVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<BarcodeSettingVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _BarcodeSettings);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.BarcodeSetting,
                _list.value == null ? new List<BarcodeSettingVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            return null;
        }
    }
}