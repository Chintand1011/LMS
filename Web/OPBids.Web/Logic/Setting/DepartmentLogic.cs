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
    public class DepartmentLogic: SettingLogicBase
    {
        IOwinContext _context;
        public DepartmentLogic(IOwinContext httpContext)
        {
            this._context = httpContext;
        }
        public override string HeaderView(string subMenuId)
        {
            return Constant.Setting.HeaderView.Department;
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
                case Constant.TransactionType.Assign:
                    return AssignSubDepartments(setting);
                default:
                    //default temp val until permission is set
                    return Search(setting);
                    //return null;
            }
        }
        public override GridView DownloadExcel()
        {
            return DownloadExcel<DepartmentsVM>("Departments", Constant.ServiceEnpoint.Settings.GetDepartments);
        }
        public override Tuple<string, string> DownloadCSV()
        {
            return DownloadCSV<DepartmentsVM>("Departments", Constant.ServiceEnpoint.Settings.GetDepartments);
        }
        public override string HTMLTable()
        {
            return HTMLTable<DepartmentsVM>(Constant.ServiceEnpoint.Settings.GetDepartments);
        }
        public override IEnumerable<T> SearchData<T>(SettingVM setting)
        {
            var rslts = ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetDepartments);
            ViewBag.total_count = rslts.value.Count();
            ViewBag.page_count = rslts.page_count;
            ViewBag.page_reset = false;
            return rslts.value;
        }

        public override IEnumerable<T> SearchSub<T>(SettingVM setting)
        {         
            if(setting.status == Constant.TransactionType.IsExists)
            {
                return ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.IsExistDepartments).value;
            }
            else
            {
                return ProcessData<T>(setting, Constant.ServiceEnpoint.Settings.GetDepartmentsToAssign).value;
            }
        }

        public override ActionResult Search(SettingVM setting)
        {
            var rslts = SearchData<DepartmentsVM>(setting);
            return PartialView(Constant.Setting.ResultView.Department, rslts);
        }

        public override ActionResult Save(SettingVM setting)
        {
            var user_id = AuthHelper.GetClaims(_context, Constant.Auth.Claims.UserId).ToSafeInt();
            // Validate
            var _department = setting.department;
            var curUrl = Constant.ServiceEnpoint.Settings.CreateDepartments;
            _department.updated_by = user_id;
            if (_department.id == 0)
            {
                // TODO: Get current user
                _department.created_by = user_id;
            }
            else
            {
                curUrl = Constant.ServiceEnpoint.Settings.UpdateDepartments;
            }
            Result<IEnumerable<DepartmentsVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<DepartmentsVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL, curUrl, _department);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.Department,
                _list.value == null ? new List<DepartmentsVM>() : _list.value);
        }

        public ActionResult AssignSubDepartments(SettingVM setting)
        {
            Result<IEnumerable<DepartmentsVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<DepartmentsVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.AssignSubDepartments, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.Department,
                _list.value == null ? new List<DepartmentsVM>() : _list.value);
        }
        public override ActionResult StatusUpdate(SettingVM setting)
        {
            Result<IEnumerable<DepartmentsVM>> _list;
            var apiManager = new ApiManager<Result<IEnumerable<DepartmentsVM>>>();
            _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.StatusUpdateDepartments, setting);
            ViewBag.total_count = _list.value.Count();
            ViewBag.page_count = _list.page_count;
            ViewBag.page_reset = false;
            return PartialView(Constant.Setting.ResultView.Department,
                _list.value == null ? new List<DepartmentsVM>() : _list.value);
        }
    }
}