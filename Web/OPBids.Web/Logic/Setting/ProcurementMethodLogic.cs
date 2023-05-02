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
using System.Web.UI.WebControls;
using Microsoft.Owin;

namespace OPBids.Web.Logic.Setting
{
	public class ProcurementMethodLogic: SettingLogicBase
	{
        IOwinContext _context;
        public ProcurementMethodLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }

        public override string HeaderView(string subMenuId)
		{
			return Constant.Setting.HeaderView.ProcurementMethod;
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
			return DownloadExcel<ProcurementMethodVM>("ProcurementMethod", Constant.ServiceEnpoint.Settings.GetProcurementMethod);
		}
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<ProcurementMethodVM>("ProcurementMethod", Constant.ServiceEnpoint.Settings.GetProcurementMethod);
        }
        public override string HTMLTable()
        {
            return HTMLTable<ProcurementMethodVM>(Constant.ServiceEnpoint.Settings.GetProcurementMethod);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
		{
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetProcurementMethod);
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
		}
	
		public override ActionResult Search(SettingVM setting)
		{
			var rslts = SearchData<ProcurementMethodVM>(setting);
			return PartialView(Constant.Setting.ResultView.ProcurementMethod, rslts);
		}

		public override ActionResult Save(SettingVM setting)
		{
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _procurementMethod = setting.procurementmethod;
			var curUrl = Constant.ServiceEnpoint.Settings.CreateProcurementMethod;
            _procurementMethod.updated_by = user_id;
            if (_procurementMethod.id == 0)
			{
				// TODO: Get current user
				_procurementMethod.created_by = user_id;
			}
			else
			{
				curUrl = Constant.ServiceEnpoint.Settings.UpdateProcurementMethod;
			}
			Result<IEnumerable<ProcurementMethodVM>> _list;
			var apiManager = new ApiManager<Result<IEnumerable<ProcurementMethodVM>>>();
			_list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _procurementMethod);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProcurementMethod,
				_list.value == null ? new List<ProcurementMethodVM>() : _list.value);
		}

		public override ActionResult StatusUpdate(SettingVM setting)
		{
			Result<IEnumerable<ProcurementMethodVM>> _list;
			var apiManager = new ApiManager<Result<IEnumerable<ProcurementMethodVM>>>();
			_list = apiManager.Invoke(ConfigManager.BaseServiceURL,
				Constant.ServiceEnpoint.Settings.StatusUpdateProcurementMethod, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.ProcurementMethod,
				_list.value == null ? new List<ProcurementMethodVM>() : _list.value);
		}

		public override IEnumerable<T> SearchSub<T>(SettingVM setting)
		{
			return new List<T>();
		}
	}
}