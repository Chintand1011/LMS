﻿using System;
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
    public class AccessTypesLogic : SettingLogicBase
    {
        IOwinContext _context;
        public AccessTypesLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId) {
            return Constant.Setting.HeaderView.AccessTypes;
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
            return DownloadExcel<AccessTypesVM>("AccessType", Constant.ServiceEnpoint.Settings.GetAccessType);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<AccessTypesVM>("AccessType", Constant.ServiceEnpoint.Settings.GetAccessType);
        }
        public override string HTMLTable()
        {
            return HTMLTable<AccessTypesVM>(Constant.ServiceEnpoint.Settings.GetAccessType);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetAccessType);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override ActionResult Search(SettingVM setting)
        {
            return PartialView(Constant.Setting.ResultView.AccessTypes,
                SearchData<AccessTypesVM>(setting));
        }
        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {
            return new List<T>();
        }

        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            var _accessTypes = setting.accessTypes;
            if (_accessTypes == null)
            {
                _accessTypes = new AccessTypesVM();
            }
            var curUrl = Constant.ServiceEnpoint.Settings.CreateAccessType;
            _accessTypes.updated_by = user_id;
            if (_accessTypes.id == 0)
            {
                _accessTypes.created_by = user_id;
            }
            else
            {
                curUrl = Constant.ServiceEnpoint.Settings.UpdateAccessType;
            }
            Result<IEnumerable<AccessTypesVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessTypesVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                curUrl, _accessTypes);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessTypes,
                _list.value == null ? new List<AccessTypesVM>() : _list.value);
        }

        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<AccessTypesVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<AccessTypesVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateAccessType, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.AccessTypes,
                _list.value == null ? new List<AccessTypesVM>() : _list.value);
        }
    }
}