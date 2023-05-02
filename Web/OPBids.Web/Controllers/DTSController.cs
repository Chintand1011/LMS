using OPBids.Common;
using OPBids.Entities.Common;
using OPBids.Entities.View.DTS;
using OPBids.Entities.View.Setting;
using OPBids.Web.Helper;
using OPBids.Web.Logic;
using OPBids.Web.Logic.ActivityLog;
using OPBids.Web.Logic.Setting;
using OPBids.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace OPBids.Web.Controllers
{
    public class DTSController : Controller
    {
        // GET: DTS
        #region Default View Renderer
        public ActionResult Index()
        {
            
            Logic.ActivityLog.ActivityLogHelper.InsertActivityLog(ActivityLogModule.DTS, ActivityLogType.DTSDashBoard, "View DTS DashBoard", "View DTS DashBoard");


            return View();
        }
        public ActionResult PartialView(DocumentsPayloadVM param)
        {
            return new ControllerLogic.DTS().Logic(param.menu_id).PartialView(param);
        }
        public ActionResult DocumentFilter()
        {
            return View();
        }
        public ActionResult PrintBarcodePreview()
        {
            var apiManager = new ApiManager<Result<IEnumerable<BarcodeSettingVM>>>();
            var _list = apiManager.Invoke(ConfigManager.BaseServiceURL,
                Constant.ServiceEnpoint.Settings.GetBarcodeSetting, new SettingVM());
            _list.value = _list.value == null ? new List<BarcodeSettingVM>() : _list.value;
            var barcodeTypes = _list.value.FirstOrDefault();
            ViewBag.BarcodeType = (barcodeTypes.barcode_only == true ? 0 : (barcodeTypes.barcode_with_print_date ? 1 : 2));
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainBarcode(DocumentsPayloadVM param)
        {
            var rslts = new ControllerLogic.DTS().Logic(param.menu_id).Maintain<RequestBarcodeVM>(param);
            rslts.value = rslts.value.OrderBy(a => a.id);
            return Json(rslts);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainDocuments(DocumentsPayloadVM param)
        {
            var rslts = new ControllerLogic.DTS().Logic(param.menu_id).Maintain<DocumentsVM>(param);
            rslts.value = rslts.value.OrderBy(a => a.id);
            return Json(rslts);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainDocumentAttachments(DocumentsPayloadVM param)
        {
            return Json(new DocumentAttachmentsLogic(Request.GetOwinContext()).Maintain<DocumentAttachmentVM>(param).OrderBy(a => a.id));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainDocumentRoutes(DocumentsPayloadVM param)
        {
            return Json(new DocumentRoutesLogic(Request.GetOwinContext()).Maintain<DocumentRoutesVM>(param).OrderBy(a => 
            param.documentRoute.process.ToUpper() == Constant.TransactionType.GetRoutesBar.ToUpper() ? a.sequence :  a.id));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult MaintainDocumentLogs(DocumentsPayloadVM param)
        {
            return Json(new DocumentLogsLogic(Request.GetOwinContext()).Maintain<DocumentLogsVM>(param).OrderByDescending(a => a.sort_date));
        }
        #endregion
        #region Settings
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDocumentCategory(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.DocumentCategory).SearchData<DocumentCategoryVM>(setting).OrderBy(a => a.document_category_name));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDocumentType(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.DocumentType).SearchData<DocumentTypeVM>(setting).OrderBy(a => a.document_type_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetSenderRecipient(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.SenderRecipient).SearchData<SenderRecipientUserVM>(setting).OrderBy(a => a.first_name));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDeliveryType(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.Delivery).SearchData<DeliveryVM>(setting).OrderBy(a => a.delivery_code));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetDocumentSecurity(SettingVM setting)
        {
            setting.page_index = -1;
            return Json(new ControllerLogic.Setting(Request.GetOwinContext()).Logic(Constant.Menu.DocumentSecurityLevel).SearchData<DocumentSecurityLevelVM>(setting).OrderBy(a => a.code));
        }
        #endregion
    }
}