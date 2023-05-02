using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OPBids.Entities.View.Setting;
using OPBids.Common;
using OPBids.Web.Logic;
using OPBids.Web.Logic.Setting;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            // Set Default Search Result
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDepartmentsToAssign(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.Departments).SearchSub<DepartmentsVM>(setting).OrderBy(a => a.dept_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult IsExistDepartments(SettingVM setting)
        {
            setting.page_index = -1;
            setting.status = Constant.TransactionType.IsExists;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.Departments).SearchSub<DepartmentsVM>(setting).OrderBy(a => a.dept_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDepartment(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.Departments).SearchData<DepartmentsVM>(setting).OrderBy(a => a.dept_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetRecordCategory()
        {
            var setting = new SettingVM();
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.RecordCategory).SearchData<RecordCategoryVM>(setting).OrderBy(a => a.category_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetRecordClassification()
        {
            var setting = new SettingVM();
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.RecordClassification).SearchData<RecordClassificationVM>(setting).OrderBy(a => a.classification_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProjectCategory(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.ProjCategory).SearchData<ProjectCategoryVM>(setting).OrderBy(a => a.proj_desc));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAccessUser(SettingVM setting)
        {
            setting.page_index = -1;
            if (Request.QueryString["searchKey"] != null)
            {
                setting.search_key = Request.QueryString["searchKey"];
            }
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.AccessUsers).SearchData<AccessUsersVM>(setting).OrderBy(a => a.first_name));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetAccessUserPassword(SettingVM setting) {
            return new AccessUsersLogic(Request.GetOwinContext()).ResetAccessUserPassword(setting);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAccessGroups(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.AccessGroups).SearchData<AccessGroupVM>(setting).OrderBy(a => a.group_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDocumentCategory(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.DocumentCategory).SearchData<DocumentCategoryVM>(setting).OrderBy(a => a.document_category_name));
        }
        public ActionResult HeaderView(SettingVM setting) {
            return PartialView(
                    new ControllerLogic.Setting(Request.GetOwinContext())
                        .Logic(setting.sub_menu_id)
                            .HeaderView(setting.sub_menu_id));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult DownloadFile(string setting)
        {
            var item = new ControllerLogic.Setting(Request.GetOwinContext()).Logic(setting).DownloadCSV();

            DataTable dataTable = new DataTable();
            System.ComponentModel.PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(AccessGroupVM));


            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            List<string[]> list = new List<string[]>();


            //object[] values = new object[propertyDescriptorCollection.Count];
            //foreach (var list1 in item.Item2)
            //{
            //    for (int i = 0; i < values.Length; i++)
            //    {
            //        values[i] = propertyDescriptorCollection[i].GetValue(list1);
            //    }
            //    dataTable.Rows.Add(values);
            //}








            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Concat("attachment; filename=", item.Item1));
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.Write(item.Item2);
            Response.Flush();
            Response.End();
            return View();

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Print(string setting)
        {
            var item = new ControllerLogic.Setting(Request.GetOwinContext()).Logic(setting).HTMLTable();

            Response.ClearContent();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Write(item);
            Response.Flush();
            Response.End();
            return View();
        }
        public ActionResult ResultView(SettingVM setting) {
            return new ControllerLogic.Setting(Request.GetOwinContext())
                        .Logic(setting.sub_menu_id)
                            .ResultView(setting);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDashBoardConfigs(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Setting.Selection.DashboardConfig).SearchData<DashboardConfigVM>(setting).OrderBy(a => a.dashboard_desc));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProjectAreas(SettingVM setting)
        {
            setting.page_index = -1;
            if (Request.QueryString["searchKey"] != null)
            {
                setting.search_key = Request.QueryString["searchKey"];
            }
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.ProjArea).SearchData<ProjectAreasVM>(setting));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProjectAreasCity(SettingVM setting)
        {
            setting.page_index = -1;
            if (Request.QueryString["searchKey"] != null)
            {
                setting.search_key = Request.QueryString["searchKey"];
            }
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.ProjAreaCity).SearchData<ProjectAreasCityVM>(setting).OrderBy(a => a.city_name));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAndSaveProjectAreas(SettingVM setting)
        {
            return new ProjectAreasLogic(Request.GetOwinContext()).GetAndSaveProjectAreas(setting);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAndSaveProjectAreasCity(SettingVM setting)
        {
            return new ProjectAreasCityLogic(Request.GetOwinContext()).GetAndSaveProjectAreasCity(setting);
        }        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProjectAreasDistrict(SettingVM setting)
        {
            setting.page_index = -1;
            if (Request.QueryString["searchKey"] != null)
            {
                setting.search_key = Request.QueryString["searchKey"];
            }
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.ProjAreaDist).SearchData<ProjectAreasDistrictVM>(setting).OrderBy(a => a.district_name));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAndSaveProjectAreasDistrict(SettingVM setting)
        {            
            return new ProjectAreasDistrictLogic(Request.GetOwinContext()).GetAndSaveProjectAreasDistrict(setting);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProjectAreasDistrictByCity(SettingVM setting)
        {            

            return new ProjectAreasDistrictLogic(Request.GetOwinContext()).GetProjectAreasDistrictByCity(setting);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetProjectAreasBarangay(SettingVM setting)
        {
            setting.page_index = -1;
            if (Request.QueryString["searchKey"] != null)
            {
                setting.search_key = Request.QueryString["searchKey"];
            }
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.ProjAreaBrgy).SearchData<ProjectAreasBarangayVM>(setting).OrderBy(a => a.barangay_name));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAndSaveProjectAreasBarangay(SettingVM setting)
        {
            return new ProjectAreasBarangayLogic(Request.GetOwinContext()).GetAndSaveProjectAreasBarangay(setting);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetSupplierAccessUser(SettingVM setting)
        {
            return new SupplierLogic(Request.GetOwinContext()).GetSupplierAccessUser(setting);
        }
    }
}