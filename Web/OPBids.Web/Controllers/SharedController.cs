using OPBids.Common;
using OPBids.Entities.Base;
using OPBids.Entities.Common;
using OPBids.Entities.View.DTS;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Entities.View.Shared;
using OPBids.Web.Helper;
using OPBids.Web.Logic;
using OPBids.Web.Logic.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using System.IO;

namespace OPBids.Web.Controllers
{
    [Authorize]
    public class SharedController : Controller
    {
         OPBids.Common.BarcodeReader.FileTools _fileTools;

        public string GhostScriptDirectory {

            get
            {
                return ConfigurationManager.AppSettings["GhostScriptDirectory"].ToString();
            }
        }

        public SharedController()
        {
            _fileTools = new Common.BarcodeReader.FileTools(GhostScriptDirectory, Constant.AppSettings.UploadFilePath);

            _fileTools.OnWriteFileFailed += (string message) =>
            {
                //delegate to log failed events on fileTools
            };

            _fileTools.OnWriteSuccess += (string message) => {

                //delegate to log any success events on fileTools
            };

           

        }

        

        //[OutputCache(Duration = 60)]//TODO vary by login?
        public ActionResult TopBarPartial()
        {
            #region "TopBar"
            var userInfo = new UserInfoVM()
            {
                Id = Guid.NewGuid().ToString(),
                Name = AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.FullName),
                Position = "",// "Manager",
                Department = AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.Department),
                DeptCode = AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DeptCode),
                username = AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.UserName),
            };
            #endregion
            ViewBag.Today = DateTime.Now.ToString("MMMM dd, yyyy, dddd");
            return PartialView("_TopBarPartial", userInfo);

        }
        public ActionResult UserNotification()
        {
            return View();
        }
        public ActionResult UserAnnouncement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MaintainUserAnnouncement(DocumentsPayloadVM param)
        {
            var rslts = new ControllerLogic.Shared().Logic(Constant.Menu.Announcement).Maintain<UserAnnouncementVM>(param);
            return Json(rslts);
        }

        [HttpPost]
        public ActionResult MaintainUserNotification(DocumentsPayloadVM param)
        {
            var rslts = new ControllerLogic.Shared().Logic(Constant.Menu.Notifications).Maintain<UserNotificationVM>(param);
            return Json(rslts);
        }
        public ActionResult GetUserAlerts(BaseVM payload)
        {
            var ctxt = Request.GetOwinContext();
            UserAlertsVM model = new UserAlertsVM()
            {
                Announcements = new UserAnnouncementLogic().Maintain<UserAnnouncementVM>(new Entities.View.DTS.DocumentsPayloadVM()
                {
                    userAnnouncement = new UserAnnouncementVM() { sender_id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.UserId).ToSafeInt() }
                }).value.ToList().Where(a => a.is_read == false).ToList(),
                Notifications = new UserNotificationLogic().Maintain<UserNotificationVM>(new Entities.View.DTS.DocumentsPayloadVM()
                {
                    userNotification = new UserNotificationVM() { sender_id = AuthHelper.GetClaims(ctxt, Constant.Auth.Claims.UserId).ToSafeInt() }
                }).value.ToList().Where(a => a.is_read == false).ToList()
            };

            return new JsonResult { Data = model };
        }

        public ActionResult GetProjectTotal(Payload payload)
        {
            return new SharedLogic(Request).GetProjectTotal(payload);
        }
        public ActionResult GetDocumentTotal(Payload payload)
        {
            return new SharedLogic(Request).GetDocumentTotal(payload);
        }

        public ActionResult GetSettingsList(Payload payload) {
            return Json(new SharedLogic(Request).GetSettingsList(payload));
        }


        [HttpPost]
        public ActionResult GetProjectProgress(PayloadVM payload)
        {
            return new SharedLogic(Request).GetProjectProgress(payload);
        }


        [Route("UploadFile")]
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            var folder = System.Web.HttpContext.Current.Request.QueryString["folder"];
            var filename = System.Web.HttpContext.Current.Request.QueryString["filename"];
            if (folder != null && filename != null)
            {
                folder = string.Concat(folder.Trim(new char[] { '\\' }), "\\");
                try
                {
                    var filePath = string.Concat(Constant.AppSettings.UploadFilePath.TrimEnd("\\".ToCharArray()) + "\\", folder);
                    if (System.IO.Directory.Exists(filePath) == false)
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    filePath = string.Concat(filePath, filename);
                    var httpContext = System.Web.HttpContext.Current;

                    if (httpContext.Request.Files.Count > 0)
                    {
                        for (int i = 0; i < httpContext.Request.Files.Count; i++)
                        {
                            HttpPostedFile httpPostedFile = httpContext.Request.Files[i];
                            if (httpPostedFile != null)
                            {
                                httpPostedFile.SaveAs(filePath);
                                var mf = new MergeFileManager();
                                mf.MergeFile(filePath);
                            }
                        }
                    }
                    return new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent("Upload Complete")
                    };
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent("Upload file error has occurred")
                    };
                }
            }
            else
            {
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("No filename to upload")
                };
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UploadDocsBarcode()
        {
            var model = new List<Common.BarcodeReader.Models.Document>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                
                var old_fileName = Path.GetFileName(file.FileName);
                var fileType = Path.GetExtension(file.FileName);
                var new_FileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fileType);

                
                
                var path = Path.Combine(GhostScriptDirectory, new_FileName);
                file.SaveAs(path);

                //file.ContentType 

                var outputFile = _fileTools.ProcessFiles(path, file.ContentType);

                //make sure that the exiting filename was not tampared
                outputFile.ActualFileName = old_fileName;

                model.Add(outputFile);

            }

            return PartialView("_FileUploadContainer", model);

        }
        #region DocumentReceiving
        [HttpPost]
        public ActionResult CheckProjectRequestDocument(PayloadVM payload)
        {
            return new SharedLogic(Request).CheckProjectRequestDocument(payload);
        }
        
        [HttpPost]
        public ActionResult ReceiveProjectRequestDocument(PayloadVM payload)
        {
            return new SharedLogic(Request).ReceiveProjectRequestDocument(payload);
        }

        [HttpPost]
        public ActionResult GetProjectLogs(PayloadVM payload)
        {
            return new SharedLogic(Request).GetProjectLogs(payload);
        }
        [HttpPost]
        public ActionResult ViewProjectRequestDocument(PayloadVM payload)
        {
            return new SharedLogic(Request).ViewProjectRequestDocument(payload);
        }

        #endregion


        #region " Reports " 
       
        public ActionResult GetPdf(Payload payload)
        {

            Result<byte[]> _result;
            var apiManager = new ApiManager<Result<byte[]>>();
            payload.dashboard_id = Convert.ToInt16(AuthHelper.GetClaims(Request.GetOwinContext(), Constant.Auth.Claims.DashboardId));
            _result = apiManager.Invoke(ConfigManager.BaseServiceURL, "service/GetProjectInfoReport", payload);

            byte[] pdf = _result.value;
            Response.AppendHeader("Content-Disposition", "inline;proj_" + payload.id + ".pdf");
            return File(pdf, "application/pdf");
        }
        #endregion

        [HttpPost]
        public ActionResult GetThumbnail(Payload payload)
        {
            try
            {


                string filename = payload.search_key; // "2019042902024786.jpg";
                string filepath = string.Concat(Constant.AppSettings.UploadFilePath.TrimEnd("\\".ToCharArray()), "\\ProjectRequestBatch\\", payload.id.ToString(), "\\", filename);
                byte[] filedata = System.IO.File.ReadAllBytes(filepath);
                //string contentType = MimeMapping.GetMimeMapping(filepath);


                using (System.IO.MemoryStream myMemStream = new System.IO.MemoryStream(filedata))
                {
                    System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(myMemStream);

                    //var aspectRatio = (fullsizeImage.Height / fullsizeImage.Width);
                    var newWidth = 120;
                    var newHeight = 100;//

                    if (fullsizeImage.Height >= fullsizeImage.Width)
                    {
                        newWidth = (fullsizeImage.Width * 100) / fullsizeImage.Height;
                    }
                    else
                    {
                        newHeight = (fullsizeImage.Height * 120) / fullsizeImage.Width;
                    }

                    
                    System.Drawing.Image newImage = new System.Drawing.Bitmap(fullsizeImage, new System.Drawing.Size(newWidth, newHeight));

                    //System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
                    using (System.IO.MemoryStream thumbnail = new System.IO.MemoryStream())
                    {
                        newImage.Save(thumbnail, System.Drawing.Imaging.ImageFormat.Jpeg);  //Or whatever format you want.
                        string mimeType = "image/jpeg";


                        string base64 = Convert.ToBase64String(thumbnail.ToArray());

                        return new JsonResult()
                        {
                            Data = string.Format("data:{0};base64,{1}", mimeType, base64)
                        };
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult GetImage(Payload payload)
        {
            string filename = payload.search_key; // "2019042902024786.jpg";
            string filepath = string.Concat(Constant.AppSettings.UploadFilePath.TrimEnd("\\".ToCharArray()), "\\ProjectRequestBatch\\", payload.id.ToString(), "\\", filename);

            return File(filepath, "application/force-download", payload.search_key);

        }

    }
}