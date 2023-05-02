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
    public class DocumentTypeLogic: SettingLogicBase
    {
        IOwinContext _context;
        public DocumentTypeLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.DocumentType;
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
            return DownloadExcel<DocumentTypeVM>("DocumentTypes", Constant.ServiceEnpoint.Settings.GetDepartments);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<DocumentTypeVM>("DocumentTypes", Constant.ServiceEnpoint.Settings.GetDepartments);
        }
        public override string HTMLTable()
        {
            return HTMLTable<DocumentTypeVM>(Constant.ServiceEnpoint.Settings.GetDepartments);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetDocumentType);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            var rslts = SearchData<DocumentTypeVM>(setting);
            return PartialView(Constant.Setting.ResultView.DocumentType, rslts);
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }
        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _documentType = setting.documentType;
           // _documentType.page_index = setting.page_index;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateDocumentType;
            _documentType.updated_by = user_id;
            if (_documentType.id == 0)
            {
                // TODO: Get current user
                _documentType.created_by = user_id;
            }
            else
            {
                curUrl = Constant.ServiceEnpoint.Settings.UpdateDocumentType;
            }
            Result<IEnumerable<DocumentTypeVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<DocumentTypeVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _documentType);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.DocumentType,
                _list.value == null ? new List<DocumentTypeVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<DocumentTypeVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<DocumentTypeVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateDocumentType, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.DocumentType,
                _list.value == null ? new List<DocumentTypeVM>() : _list.value);
        }
    }
}