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
    public class SupplierLogic: SettingLogicBase
    {
        IOwinContext _context;
        public SupplierLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.Supplier;
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
                case Constant.TransactionType.DownloadList:

                default:
                    //default temp val until permission is set
                    return Search(setting);
                    //return null;
            }
        }
        public override GridView DownloadExcel()
        {
            return DownloadExcel<SupplierVM>("Suppliers", Constant.ServiceEnpoint.Settings.GetSupplier);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<SupplierVM>("Suppliers", Constant.ServiceEnpoint.Settings.GetSupplier);
        }
        public override string HTMLTable()
        {
            return HTMLTable<SupplierVM>(Constant.ServiceEnpoint.Settings.GetSupplier);
        }
        
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetSupplier);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.Supplier,
                SearchData<SupplierVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _supplier = setting.supplier;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateSupplier;
            _supplier.updated_by = user_id;
            if (_supplier.id == 0)
            {
                // TODO: Get current user
                _supplier.created_by = user_id;
            }
            else
            {                
                curUrl = Constant.ServiceEnpoint.Settings.UpdateSupplier;
            }
            Result<IEnumerable<SupplierVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<SupplierVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _supplier);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.Supplier,
                _list.value == null ? new List<SupplierVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<SupplierVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<SupplierVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateSupplier, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.Supplier,
                _list.value == null ? new List<SupplierVM>() : _list.value);
        }

        //public override IEnumerable<T> Download<T>(SettingVM setting)
        //{
        //    var _data = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetSupplier);
        //    GridView gv = new GridView();
        //    gv.DataSource = _data;
        //    gv.DataBind();
        //    Session["Cars"] = gv;

        //    retun View(_data);

        //}  

        public ActionResult GetSupplierAccessUser(SettingVM setting)
        {
            var curUrl = Constant.ServiceEnpoint.Settings.GetSupplierAccessUser;
            Result<IEnumerable<AccessUsersVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessUsersVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, setting);
            return new JsonResult { Data = _list.value };
        }
    }
}