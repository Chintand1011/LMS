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
    public class ContractTypeLogic: SettingLogicBase
    {
        IOwinContext _context;
        public ContractTypeLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }

        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.ContractType;
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
                    return Search(setting);
                    //return null;
            }
        }
        public override GridView DownloadExcel()
        {
            return DownloadExcel<ContractTypeVM>("ContractType", Constant.ServiceEnpoint.Settings.GetContractType);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<ContractTypeVM>("ContractType", Constant.ServiceEnpoint.Settings.GetContractType);
        }
        public override string HTMLTable()
        {
            return HTMLTable<ContractTypeVM>(Constant.ServiceEnpoint.Settings.GetContractType);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetContractType);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }
        public override ActionResult Search(SettingVM setting)
        {
            var rslts = SearchData<ContractTypeVM>(setting);
            return PartialView(Constant.Setting.ResultView.ContractType, rslts);
        }

        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _contractype = setting.contractype;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateContractType;
            if (_contractype.id == 0)
            {
                // TODO: Get current user
                _contractype.created_by = user_id;
                _contractype.updated_by = user_id;
            }
            else
            {
                _contractype.updated_by = user_id;
                curUrl = Constant.ServiceEnpoint.Settings.UpdateContractType;
            }
            Result<IEnumerable<ContractTypeVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ContractTypeVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _contractype);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ContractType,
                _list.value == null ? new List<ContractTypeVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<ContractTypeVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<ContractTypeVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateContractType, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ContractType,
                _list.value == null ? new List<ContractTypeVM>() : _list.value);
        }

        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
    }
}
